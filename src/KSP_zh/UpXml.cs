using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace KSP_zh
{
    public static class UpXml
    {
        public static  void CreateXmlUP(string ksppath, string nVel, string oVel,Label label)
        {
            Dictionary<int, string> nVel_en = LoadDict(nVel + @"\en.xml");

            Dictionary<int, string> oVel_en = LoadDict(oVel + @"\en.xml");
            Dictionary<int, string> oVel_zh = LoadDict(oVel + @"\zh.xml");

            StringBuilder en_up = new StringBuilder();
            StringBuilder zh_up = new StringBuilder();
            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            en_up.AppendLine("<en>");
            zh_up.AppendLine("<zh>");

            int i = 0;
            int count = nVel_en.Count;
            foreach (var item in nVel_en)
            {
                i++;
                label.Text = i.ToString() + "/" + count.ToString();
                Application.DoEvents();

                //找到旧版本中匹配的条目
                var oEns = oVel_en.AsParallel().Where(x => x.Value == item.Value && x.Key == item.Key);
                if (oEns.Count() == 0)
                    oEns = oVel_en.AsParallel().Where(x => x.Value == item.Value);
                if (oEns.Count() != 1) continue;

                
                int oEnid=oEns.First().Key;

                //跳过没有中文的条目
                if (oVel_zh.ContainsKey(oEnid) == false) continue;
        
                //找到中文条目
                var oZh = oVel_zh.AsParallel().FirstOrDefault(x => x.Key == oEnid);

              //跳过没有汉化的条目
                if (IsHasCHZN(oZh.Value )==false ) continue;

                en_up.AppendLine(string.Format(temp, item.Key, item.Value));
                zh_up.AppendLine(string.Format(temp, item.Key, oZh.Value));
            }

            en_up.AppendLine("</en>");
            zh_up.AppendLine("</zh>");

            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\en_up.xml", en_up.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\zh_up.xml", zh_up.ToString());

        }
        public static void CreateXmlLose(string ksppath, string nVel, string oVel, Label label)
        {
            Dictionary<int, string> nVel_en = LoadDict(nVel + @"\en.xml");

            Dictionary<int, string> oVel_en = LoadDict(oVel + @"\en.xml");
            Dictionary<int, string> oVel_zh = LoadDict(oVel + @"\zh.xml");

            StringBuilder en_lose = new StringBuilder();
            StringBuilder zh_lose = new StringBuilder();
            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            en_lose.AppendLine("<en>");
            zh_lose.AppendLine("<zh>");

            int i = 0;
            int count = oVel_zh.Count;
            foreach (var item in oVel_zh)
            {
                i++;
                label.Text = i.ToString() + "/" + count.ToString();
                Application.DoEvents();

                //找到旧的标签
                var oEn = oVel_en.AsParallel().FirstOrDefault(x => x.Key == item.Key);
                 //找新的英文
                var nEn = nVel_en.AsParallel().Where(x => x.Value == oEn.Value && x.Key == oEn.Key);
                //找到就说明已经支持，不需要操作
                if (nEn != null) continue;

                //找不到就是遗失，需要调整


                en_lose.AppendLine(string.Format(temp, oEn.Key, oEn.Value));
                zh_lose.AppendLine(string.Format(temp, item.Key, item.Value));
            }

            en_lose.AppendLine("</en>");
            zh_lose.AppendLine("</zh>");

            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\en_lose.xml", en_lose.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\zh_lose.xml", zh_lose.ToString());

        }

        public static void CreateXmlUP_x64(string ksppath, string nVel, string oVel, Label label)
        {
            Dictionary<int, string> nVel_en = LoadDict(nVel + @"\en_x64.xml");

            Dictionary<int, string> oVel_en = LoadDict(oVel + @"\en_x64.xml");
            Dictionary<int, string> oVel_zh = LoadDict(oVel + @"\zh_x64.xml");

            StringBuilder en_up = new StringBuilder();
            StringBuilder zh_up = new StringBuilder();
            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            en_up.AppendLine("<en>");
            zh_up.AppendLine("<zh>");

            int i = 0;
            int count = nVel_en.Count;
            foreach (var item in nVel_en)
            {
                i++;
                label.Text = i.ToString() + "/" + count.ToString();
                Application.DoEvents();

                //找到旧版本中匹配的条目
                var oEns = oVel_en.AsParallel().Where(x => x.Value == item.Value && x.Key == item.Key);
                if (oEns.Count() == 0)
                    oEns = oVel_en.AsParallel().Where(x => x.Value == item.Value);
                if (oEns.Count() != 1) continue;


                int oEnid = oEns.First().Key;

                //跳过没有中文的条目
                if (oVel_zh.ContainsKey(oEnid) == false) continue;

                //找到中文条目
                var oZh = oVel_zh.AsParallel().FirstOrDefault(x => x.Key == oEnid);

                //跳过没有汉化的条目
                if (IsHasCHZN(oZh.Value) == false) continue;

                en_up.AppendLine(string.Format(temp, item.Key, item.Value));
                zh_up.AppendLine(string.Format(temp, item.Key, oZh.Value));
            }

            en_up.AppendLine("</en>");
            zh_up.AppendLine("</zh>");

            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\en_up_x64.xml", en_up.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\zh_up_x64.xml", zh_up.ToString());

        }
        public static void CreateXmlLose_x64(string ksppath, string nVel, string oVel, Label label)
        {
            Dictionary<int, string> nVel_en = LoadDict(nVel + @"\en_x64.xml");

            Dictionary<int, string> oVel_en = LoadDict(oVel + @"\en_x64.xml");
            Dictionary<int, string> oVel_zh = LoadDict(oVel + @"\zh_x64.xml");

            StringBuilder en_lose = new StringBuilder();
            StringBuilder zh_lose = new StringBuilder();
            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            en_lose.AppendLine("<en>");
            zh_lose.AppendLine("<zh>");

            int i = 0;
            int count = oVel_zh.Count;
            foreach (var item in oVel_zh)
            {
                i++;
                label.Text = i.ToString() + "/" + count.ToString();
                Application.DoEvents();

                //找到旧的标签
                var oEn = oVel_en.AsParallel().FirstOrDefault(x => x.Key == item.Key);
                //找新的英文
                var nEn = nVel_en.AsParallel().Where(x => x.Value == oEn.Value && x.Key == oEn.Key);
                //找到就说明已经支持，不需要操作
                if (nEn != null) continue;

                //找不到就是遗失，需要调整


                en_lose.AppendLine(string.Format(temp, oEn.Key, oEn.Value));
                zh_lose.AppendLine(string.Format(temp, item.Key, item.Value));
            }

            en_lose.AppendLine("</en>");
            zh_lose.AppendLine("</zh>");

            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\en_lose_x64.xml", en_lose.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + nVel + @"\zh_lose_x64.xml", zh_lose.ToString());

        }


         public static void CreateXmlUP_x32to64(string ksppath, string vel,  Label label)
        {
            Dictionary<int, string> vel_en_x64 = LoadDict(vel + @"\en_x64.xml");

            Dictionary<int, string> vel_en = LoadDict(vel + @"\en.xml");
            Dictionary<int, string> vel_zh = LoadDict(vel + @"\zh_up.xml");

            StringBuilder en_up = new StringBuilder();
            StringBuilder zh_up = new StringBuilder();
            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            en_up.AppendLine("<en>");
            zh_up.AppendLine("<zh>");

            StringBuilder en_lose = new StringBuilder();
            StringBuilder zh_lose = new StringBuilder();
            en_lose.AppendLine("<en>");
            zh_lose.AppendLine("<zh>");

            int i = 0;
            int count = vel_zh.Count;
            foreach (var item in vel_zh)
            {
                i++;
                label.Text = i.ToString() + "/" + count.ToString();
                Application.DoEvents();

                var en = vel_en.AsParallel().First(x =>x.Key == item.Key);

                var en_x64s = vel_en_x64.AsParallel().Where(x => x.Value == en.Value);
                if (en_x64s.Count() == 1)
                {//找到条目
                    var en_x64 = en_x64s.First();

                    en_up.AppendLine(string.Format(temp, en_x64.Key, en_x64.Value));
                    zh_up.AppendLine(string.Format(temp, en_x64.Key, item.Value));
                }
                else
                {//找不到条目
                    en_lose.AppendLine(string.Format(temp, item.Key, en.Value));
                    zh_lose.AppendLine(string.Format(temp, item.Key, item.Value));
                }
              
            }

            en_up.AppendLine("</en>");
            zh_up.AppendLine("</zh>");

            en_lose.AppendLine("</en>");
            zh_lose.AppendLine("</zh>");

            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\en_up_x64.xml", en_up.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\zh_up_x64.xml", zh_up.ToString());

            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\en_lose_x64.xml", en_lose.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\zh_lose_x64.xml", zh_lose.ToString());

        }

        public static  Dictionary<int, string> LoadDict(string xmlPath)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            doc.Load(File.OpenRead(xmlPath));
            foreach (XmlNode itemN in doc.DocumentElement.ChildNodes)
            {
                if (!(itemN is XmlElement)) continue;

                XmlElement item = itemN as XmlElement;
                dict[int.Parse(item.GetAttribute("id"))] = ((XmlCDataSection)item.FirstChild).InnerText;
            }
            return dict;
        }

        public static bool IsHasCHZN(string inputData)
        {
            Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        } 

    }
}
