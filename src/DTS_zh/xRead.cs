using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
namespace DTS_zh
{
    public class xRead
    {
        private static Dictionary<int, string> dict_zh;
        private static Dictionary<int, string> dict_en;

        //A.倒数第4个.(Int32) : String//传参数1
        public static string Trans(int id)
        {
            //Debug.LogWarning("[Trans]" + id.ToString());

            if (dict_zh == null)
            {
                Load();
            }
            if (dict_zh.ContainsKey(id))
            {
                return dict_zh[id];
            }


            if (dict_en == null)
            {
                Load();
            }
            if (dict_en.ContainsKey(id))
            {
                return dict_en[id];
            }
            return "[Not Find " + id.ToString() + "]";
        }
        public static void Load()
        {
            Loaden();
            Loadzh();
        }


        public static void Loadzh()
        {
            XmlDocument doc = new XmlDocument();
            dict_zh = new Dictionary<int, string>();

            //#if x32
            doc.Load(File.OpenRead("GameData/DTS_zh/zh.xml"));
            //#else   //x64
            //            doc.Load(File.OpenRead("GameData/DTS_zh/zh_x64.xml"));
            //#endif

            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                if (!(item is XmlElement)) continue;

                if (((XmlElement)item).HasAttribute("noT") == true)
                {
                    dict_zh[int.Parse(((XmlElement)item).GetAttribute("id"))] = dict_en[int.Parse(((XmlElement)item).GetAttribute("id"))];
                }
                else
                {
                    dict_zh[int.Parse(((XmlElement)item).GetAttribute("id"))] = ((XmlCDataSection)item.FirstChild).InnerText;
                }
            }

            Debug.Log("[xRead]Loaded:" + dict_zh.Count.ToString());
        }

        public static void Loaden()
        {
            XmlDocument doc = new XmlDocument();
            dict_en = new Dictionary<int, string>();
            //#if x32
            doc.Load(File.OpenRead("GameData/DTS_zh/en.xml"));
            //#else   //x64
            // doc.Load(File.OpenRead("GameData/DTS_zh/en_x64.xml"));
            //#endif
            foreach (XmlNode item in doc.DocumentElement.ChildNodes)
            {
                if (!(item is XmlElement)) continue;
                dict_en[int.Parse(((XmlElement)item).GetAttribute("id"))] = ((XmlCDataSection)item.FirstChild).InnerText;
            }

        }

        public string Test(int a)
        {
            string zh = DTS_zh.xRead.Trans(a);
            if (zh != null) return zh;


            return null;
        }
    }
}