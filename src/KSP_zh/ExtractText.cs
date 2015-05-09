using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

namespace KSP_zh
{
    public class ExtractText
    {
        public static void GetText(string ksppath,string vel)
        {

            string s = "QXNzZW1ibHktQ1NoYXJwQXNzZW1ibHktQ1NoYXJw";
            byte[] bytes = Convert.FromBase64String(s);
            s = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            System.Reflection.Assembly dll = System.Reflection.Assembly.LoadFile(ksppath + @"\KSP_Data\Managed\Assembly-CSharp.dll");
            Stream stream = dll.GetManifestResourceStream(s);
            byte[] num = Getbyte(stream);


            Encoding encoding2 = Encoding.Unicode;

            StringBuilder en = new StringBuilder();
            StringBuilder zh = new StringBuilder();
            int count = 0;
            int id = 0x1;

            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            //<string id="1"><![CDATA[Detonator]]></string>

            en.AppendLine("<en>");
            zh.AppendLine("<zh>");
           
            while (id < num.Length)
            {
                string strID = id.ToString();
                count = num[id];
                if (count == 0x80)
                {
                    count = num[id + 1];
                    id += 1;
                }
                else if (count > 0x80)
                {
                    count = (count & -129) << 8;
                    count |= num[id + 1];
                    id += 1;
                }
                string str1 = encoding2.GetString(num, id + 1, count);
                en.AppendLine(string.Format(temp, strID, str1));
                zh.AppendLine(string.Format(temp, strID, str1));
                id = id + 1 + count;
            }
            en.AppendLine("</en>");
            zh.AppendLine("</zh>");

            if (System.IO.Directory.Exists(Application.StartupPath + "\\" + vel) == false)
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\" + vel);

            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\zh.xml", zh.ToString());
            File.WriteAllText(Application.StartupPath + "\\"  + vel +  @"\en.xml", en.ToString());

            return;


        }

        public static void GetText64(string ksppath, string vel)
        {

            string s = "QXNzZW1ibHktQ1NoYXJwQXNzZW1ibHktQ1NoYXJw";
            byte[] bytes = Convert.FromBase64String(s);
            s = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
            System.Reflection.Assembly dll = System.Reflection.Assembly.LoadFile(ksppath + @"\KSP_x64_Data\Managed\Assembly-CSharp.dll");
            Stream stream = dll.GetManifestResourceStream(s);
            byte[] num = Getbyte(stream);


            Encoding encoding2 = Encoding.Unicode;

            StringBuilder en = new StringBuilder();
            StringBuilder zh = new StringBuilder();
            int count = 0;
            int id = 0x1;

            string temp = @"<string id=""{0}""><![CDATA[{1}]]></string>";
            //<string id="1"><![CDATA[Detonator]]></string>

            en.AppendLine("<en>");
            zh.AppendLine("<zh>");

            while (id < num.Length)
            {
                string strID = id.ToString();
                count = num[id];
                if (count == 0x80)
                {
                    count = num[id + 1];
                    id += 1;
                }
                else if (count > 0x80)
                {
                    count = (count & -129) << 8;
                    count |= num[id + 1];
                    id += 1;
                }
                string str1 = encoding2.GetString(num, id + 1, count);
                en.AppendLine(string.Format(temp, strID, str1));
                zh.AppendLine(string.Format(temp, strID, str1));
                id = id + 1 + count;
            }
            en.AppendLine("</en>");
            zh.AppendLine("</zh>");

            if (System.IO.Directory.Exists(Application.StartupPath + "\\" + vel) == false)
                System.IO.Directory.CreateDirectory(Application.StartupPath + "\\" + vel);

            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\zh_x64.xml", zh.ToString());
            File.WriteAllText(Application.StartupPath + "\\" + vel + @"\en_x64.xml", en.ToString());

            return;


        }


        internal static byte[] Getbyte(Stream steam)
        {
            byte[] buffer;
            int num8 = steam.ReadByte();
            byte num = (byte)num8;
            num = (byte)~num;
            for (int i = 1; i < 2; i++)
            {
                int num9 = steam.ReadByte();
            }
        Label_0023:
            switch (1)
            {
                case 0:
                    goto Label_0023;

                default:
                    {
                        if (1 == 0)
                        {
                        }
                        long length = steam.Length;
                        long position = steam.Position;
                        buffer = new byte[length - position];
                        int num12 = steam.Read(buffer, 0, buffer.Length);
                        if ((num & 0x20) == 0)
                        {
                            return buffer;
                        }
                        break;
                    }
            }
        Label_0062:
            switch (6)
            {
                case 0:
                    goto Label_0062;

                default:
                    for (int j = 0; j < buffer.Length; j++)
                    {
                        buffer[j] = (byte)~buffer[j];
                    }
                    break;
            }
        Label_0082:
            switch (6)
            {
                case 0:
                    goto Label_0082;
            }
            return buffer;
        }
    }
}
