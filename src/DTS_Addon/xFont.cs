using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Linq;
using System.Xml;
using UnityEngine;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xFont : MonoBehaviour
    {
        public static xFont XFont;

        GameObject _UI;
        GameObject _EngineersReport;

        void Start()
        {
            XFont = this;
            _UI = GameObject.Find("_UI");
            _EngineersReport = GameObject.Find("EngineersReport");
        }

        public bool IsWatch = false;
        DateTime FrameTime = DateTime.Now;
        long AllTicks = 0;
        long FindTicks = 0;
        long xFontTicks = 0;
        long xTextTicks = 0;
        int i = 0;
        public string FindStr = "";
        public string xFontStr = "";
        public string xTextStr = "";
        public string AllStr = "";
        string count = "";

        public List<SpriteText> sts;
        public List<SpriteTextRich> strs;

        void Update()
        {
            if (_UI == null) return;

            DateTime r = DateTime.Now;

            sts = _UI.gameObject.GetComponentsInChildren<SpriteText>().ToList();
            strs = _UI.gameObject.GetComponentsInChildren<SpriteTextRich>().ToList();

            if (_EngineersReport!=null )
            {
                sts.AddRange(_EngineersReport.gameObject.GetComponentsInChildren<SpriteText>());
                strs.AddRange(_EngineersReport.gameObject.GetComponentsInChildren<SpriteTextRich>());
            }

            #region 监视性能
            if (IsWatch)
            {
                count = "SpriteText:" + sts.Count() + "     SpriteTextRich:" + strs.Count();

                FindTicks += DateTime.Now.Subtract(r).Ticks;
                r = DateTime.Now;
            }
            #endregion

            foreach (SpriteText item in sts)
                SetSpriteTextFont(item);

            bool RichResult = false;
            foreach (var item in strs)
            {
                if (SetSpriteTextRichFont(item) == true)
                    RichResult = true;
            }
            if (RichResult == true)
            {
                Debug.Log("[xFont2]RichResult = True");
                foreach (var item in strs)
                {
                    item.Text = item.Text;
                }
            }


            #region 监视性能
            if (IsWatch)
            {
                xFontTicks += DateTime.Now.Subtract(r).Ticks;
                r = DateTime.Now;
            }
            #endregion

            foreach (SpriteText item in sts)
                SetSpriteText(item);

            #region 监视性能
            if (IsWatch)
            {
                xTextTicks += DateTime.Now.Subtract(r).Ticks;
                AllTicks += DateTime.Now.Subtract(FrameTime).Ticks;

                i += 1;
                if (i > 30)
                {
                    float findP = ((float)FindTicks / (float)AllTicks);
                    FindStr = P2S("定位消耗", findP);

                    float fontP = ((float)xFontTicks / (float)AllTicks);
                    xFontStr = P2S("字库消耗", fontP);

                    float textP = ((float)xTextTicks / (float)AllTicks);
                    xTextStr = P2S("翻译消耗", textP);

                    float allP = ((float)(FindTicks + xFontTicks + xTextTicks) / (float)AllTicks);
                    AllStr = P2S("合计消耗", allP);

                    i = 0;
                    AllTicks = 0;
                    FindTicks = 0;
                    xFontTicks = 0;
                    xTextTicks = 0;
                }

                FrameTime = DateTime.Now;
            }
            #endregion
        }

        //格式化输出内容
        public string P2S(string title, float p)
        {
            int i = (int)(p * 100);
            return title + ":" + p.ToString("00.00%") + "".PadRight(i, '|');
        }

        void OnGUI()
        {

            //GUI.Label(new Rect(10, 50, 500, 50), count);
        }

        #region xFont

        public void SetSpriteTextFont(SpriteText ST)
        {
            if (FontList.Count == 0)
            {
                LoadxFont();
            }
            //Debug.LogWarning("xFont:" + ST.font.name + "-->>");

            if (ST.font.name[0] == 'c' && ST.font.name[1] == 'n') return;
            Font2Font f2f = GetFont2Font(ST.font.name);
            Debug.Log("[xFont1]" + ST.font.name + "-->>" + f2f.Name + ST.text);
            ST.SetFont(f2f.fontDef, f2f.fontMat);

        }

        public bool SetSpriteTextRichFont(SpriteTextRich ST)
        {
            bool result = false;
            if (FontList.Count == 0)
            {
                LoadxFont();
            }
           // if (ST.font.name[0] == 'c' && ST.font.name[1] == 'n') return result;

            //Debug.Log("[xFont2-1]" + ST.font.name + " :: " + ST.text);
            for (int i = 0; i < ST.font.fonts.Length; i++)
            {
                if (ST.font.fonts[i].fontText.name[0] == 'c' && ST.font.fonts[i].fontText.name[1] == 'n') continue;

                Font2Font f2f = GetFont2Font(ST.font.fonts[i].fontText.name);
                Debug.Log("[xFont2]" + ST.font.name + "-->>" + f2f.Name + " :: " + ST.text);
                ST.font.fonts[i].fontText = f2f.fontDef;
                ST.font.fonts[i].material = f2f.fontMat;
                ST.font.fonts[i].SpriteFont = FontStore.GetFont(f2f.fontDef);
                //Debug.Log("[xFont2-2]" + ST.font.name + "-->>" + f2f.Name + " :: " + ST.text);
                result = true;
            }
            ST.font.name = "cn" + ST.font.name;
            return result;
        }

        public static Font2Font GetFont2Font(string name)
        {
            if (FontList.ContainsKey(name) == true)
            {
                //Debug.LogWarning("xFont:命中" + FontList.Count.ToString());
                return FontList[name];
            }
            else
            {
                //Debug.LogWarning("xFont:脱吧" + FontList.Count.ToString());
                return FontList.First().Value;
            }
        }

        public static void LoadxFont()
        {
            Dictionary<string, Font2Font> isLoaded = new Dictionary<string, Font2Font>();
            FontList = new Dictionary<string, Font2Font>();
            XmlDocument doc = new XmlDocument();
            doc.Load(File.OpenRead("GameData/DTS_zh/zhFont.xml"));
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                if (!(node is XmlElement)) continue;
                XmlElement item = (XmlElement)node;

                if (isLoaded.ContainsKey(item.InnerText))
                {//找到已经加载的文件就不用再次加载了，重复加载会出现异常
                    FontList.Add(item.GetAttribute("name"), isLoaded[item.InnerText]);
                }
                else
                {
                    Font2Font font = new Font2Font();
                    font.Load(item.InnerText);
                    isLoaded.Add(item.InnerText, font);

                    FontList.Add(item.GetAttribute("name"), font);
                }
                Debug.LogWarning("xFont:载入" + item.GetAttribute("name") + "==" + item.InnerText);
            }
            Debug.LogWarning("xFont:载入" + FontList.Count.ToString());
        }

        public static Dictionary<string, Font2Font> FontList = new Dictionary<string, Font2Font>();

        //不知道为什么载入字体代码只能写在DTS_zh.xFont原因不明
        //如果写在这里载入的字体出现问题
        public struct Font2Font
        {
            public string Name;
            //private string sFilePath;
            public TextAsset fontDef;
            public Material fontMat;
            public void Load(string fontName)
            {
                this.Name = fontName;
                //查找指定着色器
                this.fontMat = new Material(Shader.Find("Sprite/Vertex Colored"));


                //载入贴图（题外话：KSP做的资源载入感觉不错，可以考虑收纳）

                //Debug.Log("fontName:" + fontName + "  fontPath" + fontName);

                Texture texture = GameDatabase.Instance.GetTexture("DTS_zh/cnfont/" + fontName, false);
                if (texture == null)
                {
                    MonoBehaviour.print("[FontReplacer] Error:Texture Assert Failed .");
                }
                //设置材质球贴图
                //"_MainTex"是主要的漫反射纹理，也能通过 mainTexture 属性访问
                this.fontMat.SetTexture("_MainTex", texture);
                //创建不卸载的资源
                UnityEngine.Object.DontDestroyOnLoad(this.fontMat);
                if (this.fontMat == null)
                {
                    MonoBehaviour.print("[FontReplacer] Error:fontMat Assert Failed .");
                }
                string fontPath = System.IO.Directory.GetCurrentDirectory() + "/GameData/DTS_zh/cnfont/" + fontName;
                var www = new WWW("file:///" + fontPath + ".fnt.unity3d");

                this.fontDef = www.assetBundle.mainAsset as TextAsset;
                if (this.fontDef == null)
                {
                    MonoBehaviour.print("[FontReplacer] Error:sptFont Assert Failed .");
                }
            }


        }

        #endregion

        #region xText

        public void SetSpriteText(SpriteText ST)
        {
            if (xTextDict == null)
            {
                LoadxText();
            }
            if (ST.Text.Length == 0) return;
            if (ST.text.Length > 0 && !(ST.text[0] > '0' && ST.text[0] < 'z')) return;//用于排除已经被汉化的文字

            if (xTextDict.ContainsKey(ST.Text))
            {
                Debug.Log("[xText]" + ST.Text + "-->>" + xTextDict[ST.Text]);
                ST.Text = xTextDict[ST.Text];
            }
        }

        private static Dictionary<string, string> xTextDict;

        public void LoadxText()
        {
            XmlDocument doc = new XmlDocument();
            xTextDict = new Dictionary<string, string>();
            doc.Load(File.OpenRead("GameData/DTS_zh/zhText.xml"));
            foreach (XmlElement item in doc.DocumentElement.ChildNodes)
            {
                xTextDict[item.GetAttribute("name")] = ((XmlCDataSection)item.FirstChild).InnerText;
            }
        }

        #endregion
    }

}