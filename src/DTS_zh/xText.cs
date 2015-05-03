using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace DTS_zh
{
    public class xText
    {
        private static Dictionary<string, string> dict_Field;

        //Assembly-CSharp-firstpass\SpriteText.ProcessString
        //str = xText.Trans(str);
        public static string Trans(string value)
        {
            if (dict_Field == null)
            {
                LoadDict();
            }
            if (dict_Field.ContainsKey(value))
            {
                return dict_Field[value];
            }
            else
            {
                //  Debug.Log("[xText]Not find Text:" + value);
                return value;
            }
        }

        public static void LoadDict()
        {
            XmlDocument doc = new XmlDocument();
            dict_Field = new Dictionary<string, string>();
            doc.Load(File.OpenRead("GameData/DTS_zh/zhText.xml"));
            foreach (XmlElement item in doc.DocumentElement.ChildNodes)
            {
                dict_Field[item.GetAttribute("name")] = ((XmlCDataSection)item.FirstChild).InnerText;
            }
        }
    }
}
