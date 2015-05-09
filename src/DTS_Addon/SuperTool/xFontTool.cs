using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DTS_Addon.SuperTool
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xFontTool : MonoBehaviour
    {
        void OnGUI()
        {
            //a = GUI.TextField(new Rect(50, 50, 100, 20), a);

            //if (GUI.Button(new Rect(50, 75, 100, 20), "3423"))
            //{
            //    var go = GameObject.Find("ProgressBar");
            //    go.transform.localScale = new Vector3(1, 5, 1);
            //    go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            //}
            if (SuperTools.UIxFontTool == true)
            {
                xFont.XFont.IsWatch = true;
                xFontWindow = GUI.Window(104, xFontWindow, CxFontWindow, "汉化监视");
            }
            else
            {
                xFont.XFont.IsWatch = false;
            }

        }

        public List<Node> Nodes;

        Rect xFontWindow = new Rect(100, 100, 400, 400);
        Vector2 scrollPosition;


        void CxFontWindow(int id)
        {

            GUI.DragWindow(new Rect(0, 0, 380, 30));

            GUI.Label(new Rect(10, 20, 400, 20), xFont.XFont.FindStr);
            GUI.Label(new Rect(10, 40, 400, 20), xFont.XFont.xFontStr);
            GUI.Label(new Rect(10, 60, 400, 20), xFont.XFont.xTextStr);
            GUI.Label(new Rect(10, 80, 400, 20), xFont.XFont.AllStr);

            //开始滚动视图  
            scrollPosition = GUI.BeginScrollView(new Rect(5, 100, 390, 295), scrollPosition, new Rect(0, 0, 370, (xFont.XFont.sts.Length + xFont.XFont.strs.Length) * 20));

            int index = 0;
            GUI.Label(new Rect(0, index * 20, 370, 20), "SpriteText:" + xFont.XFont.sts.Length.ToString());
            index++;
            foreach (var item in xFont.XFont.sts)
            {
                GUI.TextField(new Rect(0, index * 20, 350, 20), item.name + ":" + item.Text);
                if (GUI.Button(new Rect (350,index *20,20,20),"+"))
                {
                    File.AppendAllText("GameData/DTS_zh/App.txt", item.Text);
                }

                index++;
            }
            GUI.Label(new Rect(0, index * 20, 370, 20), "SpriteTextRich:" + xFont.XFont.sts.Length.ToString());
            index++;
            foreach (var item in xFont.XFont.strs)
            {
                GUI.TextField(new Rect(0, index * 20, 350, 20), item.name + ":" + item.Text);
                if (GUI.Button(new Rect(350, index * 20, 20, 20), "+"))
                {
                    File.AppendAllText("GameData/DTS_zh/App.txt", item.Text);
                }
                index++;
            }

            //结束滚动视图  
            GUI.EndScrollView();

        }
    }
}
