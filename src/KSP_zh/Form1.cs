using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace KSP_zh
{
    public partial class Form1 : Form
    {
        static string KSPPath = @"C:\Program Files (x86)\Steam\SteamApps\common\Kerbal Space Program";
        static string vel = "0.0";

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("版本不能为空");
                return;
            }
            OpenFileDialog kspOpen = new OpenFileDialog();
            kspOpen.FileName = "KSP.exe";
            if (kspOpen.ShowDialog() == DialogResult.Cancel) return;

            vel = textBox1.Text;

            KSPPath = System.IO.Path.GetDirectoryName(kspOpen.FileName);
            label1.Text = System.IO.Path.GetDirectoryName(kspOpen.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ExtractText.GetText(KSPPath, vel);
           // ExtractCfg.Extract(KSPPath, vel);
            MessageBox.Show("OK");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog cfgzhOpen = new OpenFileDialog();
            if (cfgzhOpen.ShowDialog() == DialogResult.Cancel) return;
            ExtractCfg.Injection(KSPPath, cfgzhOpen.FileName);
            MessageBox.Show("OK");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (oldvel.Text == "")
            {
                MessageBox.Show("比对版本不能为空");
                return;
            }
            UpXml.CreateXmlUP(KSPPath, vel, oldvel.Text,label4);

            //UpXml.CreateXmlLose(KSPPath, vel, oldvel.Text, label4);

            MessageBox.Show("OK");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ConvertText.ConvertTxt();
            MessageBox.Show("OK");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ConvertText.ConvertXml();
            MessageBox.Show("OK");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (oldvel.Text == "")
            {
                MessageBox.Show("比对版本不能为空");
                return;
            }
            UpXml.CreateXmlUP_x64(KSPPath, vel, oldvel.Text, label4);

            UpXml.CreateXmlLose_x64(KSPPath, vel, oldvel.Text, label4);

            MessageBox.Show("OK");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            //if (oldvel.Text == "")
            //{
            //    MessageBox.Show("比对版本不能为空");
            //    return;
            //}
            vel = textBox1.Text;
            UpXml.CreateXmlUP_x32to64(KSPPath, vel, label4);

            MessageBox.Show("OK");
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ExtractText.GetText64(KSPPath, vel);
            // ExtractCfg.Extract(KSPPath, vel);
            MessageBox.Show("OK");
        }
    }
}
