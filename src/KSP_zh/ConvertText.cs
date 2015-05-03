using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KSP_zh
{
   public static  class ConvertText
    {
       public static void ConvertTxt()
       {
           OpenFileDialog xmlDig = new OpenFileDialog();
           xmlDig.Filter = "*.xml|*.xml";
           if (xmlDig.ShowDialog() == DialogResult.Cancel) return;

           var xml = LoadDict(xmlDig.FileName);

           string temp = @"{0} = {1}";
           StringBuilder txt = new StringBuilder();
           foreach (var item in xml)
           {
               txt.AppendLine(string.Format(temp, item.Key.ToString(), item.Value.Replace("\r", @"\r").Replace("\n", @"\n")));
           }

           File.WriteAllText(xmlDig.FileName + ".txt", txt.ToString());
           
       }

       public static Dictionary<int, string> LoadDict(string xmlPath)
       {
           XmlDocument doc = new XmlDocument();
           Dictionary<int, string> dict = new Dictionary<int, string>();
           doc.Load(File.OpenRead(xmlPath));
           foreach (XmlElement item in doc.DocumentElement.ChildNodes)
           {
               dict[int.Parse(item.GetAttribute("id"))] = ((XmlCDataSection)item.FirstChild).InnerText;
           }
           return dict;
       }



       public static void ConvertXml()
       {
           OpenFileDialog txtDig = new OpenFileDialog();
           txtDig.Filter="*.txt|*.txt";
           if (txtDig.ShowDialog() == DialogResult.Cancel) return;

           StringBuilder xml = new StringBuilder();
           string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
           xml.AppendLine("<zn>");
           var aa = File.ReadAllLines(txtDig.FileName);
           foreach (var item in File.ReadAllLines(txtDig.FileName))
           {
               string key = item.Substring(0, item.IndexOf("=") - 1).TrimEnd();
               string value = item.Substring(item.IndexOf("=") + 1).TrimStart().Replace(@"\r", "\r").Replace(@"\n", "\n");

               xml.AppendLine(string.Format(temp, key, value));
           }
           xml.AppendLine("</zn>");

           File.WriteAllText(txtDig.FileName + ".xml", xml.ToString());

       }
    }
}
