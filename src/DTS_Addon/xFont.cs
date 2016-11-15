using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Linq;
using System.Xml;
using UnityEngine;
using TMPro;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xFont : MonoBehaviour
    {
        public static xFont XFont;

        GameObject _UI;
        GameObject _EngineersReport;
        public static bool isLoaded = false;
        public static TMP_FontAsset fontDef;
        public static void LoadxFont()
        {
            string fontPath = System.IO.Directory.GetCurrentDirectory() + "/GameData/DTS_zh/cnfont/" + "msyhUIL";
            var www = new WWW("file:///" + fontPath + ".assetbundle");
            xFont.fontDef = www.assetBundle.mainAsset as TMP_FontAsset;
            if (xFont.fontDef == null)
            {
                
            }
            else
            {
                xFont.isLoaded = true;
            }
        }
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

        public List<TMP_Text> TMP_texts;
        public static Dictionary<string, TMP_FontAsset> FontList = new Dictionary<string, TMP_FontAsset>();

        
        void Update()
        {
            if (_UI == null) return;

            DateTime r = DateTime.Now;

            TMP_texts = _UI.gameObject.GetComponentsInChildren<TMP_Text>().ToList();

            if (_EngineersReport!=null )
            {
                TMP_texts.AddRange(_EngineersReport.gameObject.GetComponentsInChildren<TMP_Text>());
            }

            #region 监视性能
            if (IsWatch)
            {
                count = "TMP_Text:" + TMP_texts.Count();

                FindTicks += DateTime.Now.Subtract(r).Ticks;
                r = DateTime.Now;
            }
            #endregion

            foreach (TMP_Text item in TMP_texts) {

            }
        



            #region 监视性能
            if (IsWatch)
            {
                xFontTicks += DateTime.Now.Subtract(r).Ticks;
                r = DateTime.Now;
            }
            #endregion

            foreach (TMP_Text item in TMP_texts)
                SetTMP_Text(item);

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










        //不知道为什么载入字体代码只能写在DTS_zh.xFont原因不明


        #endregion


        public void SetTMP_TextFont(TMP_Text TMP_T)
        {
            //Debug.LogWarning("xFont:" + TMP_T.font.name + "-->>");
            if (TMP_T.font.name[0] == 'm' && TMP_T.font.name[1] == 's') return;
            Debug.Log("[xFont1]" + TMP_T.font.name + "-->>" + TMP_T.text);
            TMP_T.font=xFont.fontDef;
        }
        #region xText

        public void SetTMP_Text(TMP_Text ST)
        {
            if (xTextDict == null)
            {
                LoadxText();
            }
            if (ST.text.Length == 0) return;
            if (ST.text.Length > 0 && !(ST.text[0] > '0' && ST.text[0] < 'z')) return;//用于排除已经被汉化的文字

            if (xTextDict.ContainsKey(ST.text))
            {
                Debug.Log("[xText]" + ST.text + "-->>" + xTextDict[ST.text]);
                ST.text = xTextDict[ST.text];
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