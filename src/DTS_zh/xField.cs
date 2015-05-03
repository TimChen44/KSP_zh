//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using UnityEngine;

//namespace DTS_zh
//{
//    //ac BaseField.ctor(KSPField, FieldInfo, Object)//传参数1
//    public class xField
//    {
//        private static Dictionary<string, string> dict_Field;

//        public static void Trans(KSPField actionAttr)
//        {
//            if (dict_Field == null)
//            {
//                LoadDict();
//            }
//            if (dict_Field.ContainsKey(actionAttr.guiName))
//            {
//                actionAttr.guiName = dict_Field[actionAttr.guiName];
//            }
//            else
//            {
//               // Debug.Log("[xField]Not find guiName:" + actionAttr.guiName);
//            }
//        }

//        public static void LoadDict()
//        {
//            XmlDocument doc = new XmlDocument();
//            dict_Field = new Dictionary<string, string>();
//            doc.Load(File.OpenRead("GameData/DTS_zh/zhField.xml"));
//            foreach (XmlElement item in doc.DocumentElement.ChildNodes)
//            {
//                dict_Field[item.GetAttribute("name")] = ((XmlCDataSection)item.FirstChild).InnerText;
//            }
//        }
//    }
//}
