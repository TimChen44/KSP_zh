using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;
namespace DTS_zh
{
    public static class xFont
    {
        // Fields
        private const string Font_10 = "cn10";
        private const string Font_12 = "cn12";
        private const string Font_14 = "cn14";
        private const string Font_14b = "cn14b";
        private const string Font_16 = "cn16";
        private const string Font_16b = "cn16b";
        public static Dictionary<string, Font2Font> FontList = new Dictionary<string, Font2Font>();

        // Methods
        public static void loadFontList()
        {
            string[] strArray = new string[] { "cn10", "cn12", "cn14", "cn14b", "cn16", "cn16b" };
            foreach (string str in strArray)
            {
                Font2Font font = new Font2Font();
                font.Load(str);
                FontList.Add(str, font);
            }
        }


        private static Dictionary<string, SpriteFontMultiple.SpriteFontInstance> SSList = new Dictionary<string, SpriteFontMultiple.SpriteFontInstance>();

        public static void getFontInfoRich(SpriteTextRich ST)
        {
            if (FontList.Count == 0)
            {
                loadFontList();
            }

           // Debug.LogWarning("[xFont:getFontInfoRich]font.name:" + ST.font.name);
            for (int i = 0; i < ST.font.fonts.Length; i++)
            {

                string str = "";
                switch (ST.font.fonts[i].fontText.name)
                {
                    case "Arial, Fancy":
                        str = "cn12";
                        break;

                    case "Arial10":
                        str = "cn10";
                        break;

                    case "Arial11":
                        str = "cn10";
                        break;

                    case "Arial12":
                        str = "cn12";
                        break;

                    case "Arial14":
                        str = "cn14";
                        break;

                    case "Arial14Bold":
                        str = "cn14b";
                        break;

                    case "Arial16":
                        str = "cn16";
                        break;

                    case "Arial16_Mk2":
                        str = "cn16b";
                        break;

                    case "Calibri12":
                        str = "cn12";
                        break;

                    case "Calibri14":
                        str = "cn14";
                        break;

                    case "Calibri16":
                        str = "cn16";
                        break;

                    default:
                        str = "cn12";
                        Debug.LogWarning("[xFont]" + ST.font.name + " is Null");
                        break;
                }
                if (ST.font.fonts[i].fontText.name != str)
                {
                    Debug.Log("[xFont]font[" + i.ToString() + "].name:" + ST.font.fonts[i].name);

                    Debug.Log("[xFont]font[" + i.ToString() + "].material.name:" + ST.font.fonts[i].material.name);
                    Debug.Log("[xFont]font[" + i.ToString() + "].fontText.name:" + ST.font.fonts[i].fontText.name);

                    ST.font.fonts[i].fontText = FontList[str].fontDef;
                    ST.font.fonts[i].material = FontList[str].fontMat;
                    //if (SSList.ContainsKey(str) == false)
                    //{
                    //    SpriteFont sf = new SpriteFont(FontList[str].fontDef);
                    //    SpriteFontMultiple.SpriteFontInstance ss = new SpriteFontMultiple.SpriteFontInstance();
                    //    ss.SpriteFont = sf;
                    //    ss.material = FontList[str].fontMat;
                    //    SSList.Add(str, ss);
                    //}
                    //ST.font.fonts[i] = SSList[str];

                    Debug.Log("[xFont]font[" + i.ToString() + "].name:" + str + " - OK");
                }
            }
        }



        public static void getFontInfo(SpriteText ST)
        {
            if (FontList.Count == 0)
            {
                loadFontList();
            }
            //Debug.LogWarning("[xFont:getFontInfo]font.name:" + ST.font.name);

            string str = "";
            switch (ST.font.name)
            {
                case "Arial, Fancy":
                    str = "cn12";
                    break;

                case "Arial10":
                    str = "cn10";
                    break;

                case "Arial11":
                    str = "cn10";
                    break;

                case "Arial12":
                    str = "cn12";
                    break;

                case "Arial14":
                    str = "cn14";
                    break;

                case "Arial14Bold":
                    str = "cn14b";
                    break;

                case "Arial16":
                    str = "cn16";
                    break;

                case "Arial16_Mk2":
                    str = "cn16b";
                    break;

                case "Calibri12":
                    str = "cn12";
                    break;

                case "Calibri14":
                    str = "cn14";
                    break;

                case "Calibri16":
                    str = "cn16";
                    break;

                default:
                    str = "cn12";
                    Debug.LogWarning("[xFont]" + ST.font.name + " is Null");
                    break;
            }
            if (ST.font.name != str)
            {

                ST.SetFont(FontList[str].fontDef, FontList[str].fontMat);
            }
        }

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
}
