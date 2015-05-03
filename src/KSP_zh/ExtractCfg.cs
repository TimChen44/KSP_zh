using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KSP_zh
{
    class ExtractCfg
    {
        public static void Extract(string ksppath, string vel)
        {
            string Parts = ksppath + @"\GameData\Squad\Parts";

            StringBuilder en = new StringBuilder();
            StringBuilder zh = new StringBuilder();



            foreach (var item in System.IO.Directory.GetFiles(Parts, "*.cfg", System.IO.SearchOption.AllDirectories))
            {
                IEnumerable<string> lines = System.IO.File.ReadLines(item, Encoding.Default);

                string nameline = lines.FirstOrDefault(x => x.IndexOf("name") == 0);

                if (nameline == null) continue;

                string name = nameline.Substring(nameline.IndexOf("=") + 1).Trim();
                //en.AppendLine(name);
                //zh.AppendLine(name);

                string titleline = lines.FirstOrDefault(x => x.IndexOf("title") == 0);
                if (string.IsNullOrWhiteSpace(titleline) == false)
                {
                    en.AppendLine(GetValue(name, titleline));
                    zh.AppendLine(GetValue(name, titleline));
                }

                string descriptionline = lines.FirstOrDefault(x => x.IndexOf("description") == 0);
                if (string.IsNullOrWhiteSpace(descriptionline) == false)
                {
                    en.AppendLine(GetValue(name, descriptionline));
                    zh.AppendLine(GetValue(name, descriptionline));
                }

                string manufacturerline = lines.FirstOrDefault(x => x.IndexOf("manufacturer") == 0);
                if (string.IsNullOrWhiteSpace(manufacturerline) == false)
                {
                    en.AppendLine(GetValue(name, manufacturerline));
                    zh.AppendLine(GetValue(name, manufacturerline));
                }



                en.AppendLine();
                zh.AppendLine();

            }

            if (System.IO.Directory.Exists(Application.StartupPath + "\\" + vel) == false)
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\" + vel);

            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\zh.txt", zh.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\en.txt", zh.ToString());


        }

        public static string GetValue(string name, string line)
        {
            string temp = "{0}.{1}={2}";
            return string.Format(temp, name,
                    line.Substring(0, line.IndexOf("=") - 1).Trim(),
                         line.Substring(line.IndexOf("=") + 1).Trim());
        }


        public static void Injection(string ksppath, string zhPath)
        {
            string Parts = ksppath + @"\GameData\Squad\Parts";

            IEnumerable<string> zhs = File.ReadLines(zhPath, Encoding.Default);
            foreach (var item in System.IO.Directory.GetFiles(Parts, "*.cfg", System.IO.SearchOption.AllDirectories))
            {
                List<string> lines = System.IO.File.ReadLines(item, Encoding.Default).ToList();
                //查找名称所在行
                string nameline = lines.FirstOrDefault(x => x.IndexOf("name") == 0);

                if (nameline == null) continue;
                //获得名称
                string name = nameline.Substring(nameline.IndexOf("=") + 1).Trim();

                //查看这个名称是否有汉化文件
                if (zhs.Any(x => x.IndexOf(name) == 0) == false) continue;

                //title的汉化
                int titleLN = GetLineNumber(lines, "title");
                if (titleLN != -1)//存在这个属性就对他汉化
                {
                    //找到汉化的信息
                    var titleZh = zhs.FirstOrDefault(x => x.IndexOf(name + ".title=") == 0);
                    if (!string.IsNullOrWhiteSpace(titleZh))//存在汉化信息
                    {
                        //更新文件中的信息
                        lines[titleLN] = lines[titleLN].Substring(0, lines[titleLN].IndexOf("=") + 1)
                         + " "   + titleZh.Substring(titleZh.IndexOf("=") + 1).Trim();
                    }
                }

                //description的汉化
                int descriptionLN = GetLineNumber(lines, "description");
                if (descriptionLN != -1)//存在这个属性就对他汉化
                {
                    //找到汉化的信息
                    var descriptionZh = zhs.FirstOrDefault(x => x.IndexOf(name + ".description=") == 0);
                    if (!string.IsNullOrWhiteSpace(descriptionZh))//存在汉化信息
                    {
                        //更新文件中的信息
                        lines[descriptionLN] = lines[descriptionLN].Substring(0, lines[descriptionLN].IndexOf("=") + 1)
                           + " " + descriptionZh.Substring(descriptionZh.IndexOf("=") + 1).Trim();
                    }
                }

                //description的汉化
                int manufacturerLN = GetLineNumber(lines, "manufacturer");
                if (manufacturerLN != -1)//存在这个属性就对他汉化
                {
                    //找到汉化的信息
                    var manufacturerZh = zhs.FirstOrDefault(x => x.IndexOf(name + ".manufacturer=") == 0);
                    if (!string.IsNullOrWhiteSpace(manufacturerZh))//存在汉化信息
                    {
                        //更新文件中的信息
                        lines[manufacturerLN] = lines[manufacturerLN].Substring(0, lines[manufacturerLN].IndexOf("=") + 1)
                           + " " + manufacturerZh.Substring(manufacturerZh.IndexOf("=") + 1).Trim();
                    }
                }

                System.IO.File.WriteAllLines(item, lines,Encoding.Unicode );
            }


        }

        public static int GetLineNumber(List<string> lines, string text)
        {
            for (int i = 0; i < lines.Count(); i++)
            {
                if (lines[i].IndexOf(text) == 0) return i;
            }
            return -1;
        }



    }
}
