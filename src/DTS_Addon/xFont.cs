using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xFont : MonoBehaviour
    {
        public static xFont XFont;


        GameObject _UI;

        void Awake()
        {

        }


        void Start()
        {
            XFont = this;


            _UI = GameObject.Find("_UI");
        }

        DateTime b = DateTime.Now;

        long alltime = 0;
        long runtime = 0;
        int i = 0;

        string ss = "";

        string count = "";

        void Update()
        {
            if (_UI == null) return;

            DateTime r = DateTime.Now;

            SpriteText[] sts = _UI.gameObject.GetComponentsInChildren<SpriteText>();

            SpriteTextRich[] strs = _UI.gameObject.GetComponentsInChildren<SpriteTextRich>();

            foreach (SpriteText item in sts)
                getFontInfo(item);

            foreach (var item in strs)
                getFontInfoRich(item);


            count = "SpriteText:" + sts.Count() + "     SpriteTextRich:" + strs.Count();

            runtime += DateTime.Now.Subtract(r).Ticks;
            alltime += DateTime.Now.Subtract(b).Ticks;
            i += 1;
            if (i > 30)
            {
                ss = ((double)runtime / (double)alltime).ToString("0.00%");
                ss += "性能消耗" + "".PadRight((int)(((double)runtime / (double)alltime) * 100), '|');
                i = 0;
                alltime = 0;
                runtime = 0;
            }

            b = DateTime.Now;

        }

        void OnGUI()
        {
            GUI.Label(new Rect(10, 30, 500, 30), ss);
            GUI.Label(new Rect(10, 50, 500, 50), count);
        }

        public void getFontInfo(SpriteText ST)
        {
            if (FontList.Count == 0)
            {
                //xFontExt.loadFontList();
                LoadDict();
            }
            //Debug.LogWarning("xFont:" + ST.font.name + "-->>");

            if (ST.font.name[0] == 'c' && ST.font.name[1] == 'n') return;
            //if (ST.active == false || ST.enabled == false || ST.renderer.isVisible == false) return;

            Font2Font f2f = GetFont2Font(ST.font.name);
            Debug.Log("xFont:" + ST.font.name + "-->>" + f2f.Name);
            ST.SetFont(f2f.fontDef, f2f.fontMat);

        }

        public void getFontInfoRich(SpriteTextRich ST)
        {
            if (FontList.Count == 0)
            {
                //xFontExt.loadFontList();
                LoadDict();
            }

            for (int i = 0; i < ST.font.fonts.Length; i++)
            {
                if (ST.font.fonts[i].fontText.name[0] == 'c' && ST.font.fonts[i].fontText.name[1] == 'n') return;

                Font2Font f2f = GetFont2Font(ST.font.fonts[i].fontText.name);
                Debug.Log("xFont:" + ST.font.name + "-->>" + f2f.Name);
                ST.font.fonts[i].fontText = f2f.fontDef;
                ST.font.fonts[i].material = f2f.fontMat;
                ST.font.fonts[i].SpriteFont = FontStore.GetFont(f2f.fontDef);
            }
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


        public static void LoadDict()
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

    }

    public static class xFontExt
    {


    }
}