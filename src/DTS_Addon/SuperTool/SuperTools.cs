using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class SuperTools : MonoBehaviour
    {
        public static bool ConfigTest = false;
        public static bool UITree = false;
        public static bool UICapture = false;
        public static bool UIxFontTool = false;

        public bool ShowSuperTools = false;

        void Start()
        {
            //GameObject.DontDestroyOnLoad(this);
        }


        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.F11))
            {
                ShowSuperTools = !ShowSuperTools;
            }

        }
        Rect window = new Rect(100, 100, 250, 190);

        void OnGUI()
        {
            if (ShowSuperTools == false) return;
            window = GUI.Window(99, window, SuperWindow, "Super Debug Tools");

        }

        void SuperWindow(int id)
        {

            if (GUI.Button(new Rect(10, 20, 50, 50), "资源"))
            {
                ConfigTest = !ConfigTest;
            }
            else if (GUI.Button(new Rect(70, 20, 50, 50), "对象树"))
            {
                UITree = !UITree;
            }
            else if (GUI.Button(new Rect(130, 20, 50, 50), "对象\r\n捕捉"))
            {
                UICapture = !UICapture;
            }
            else if (GUI.Button(new Rect(190, 20, 50, 50), "汉化\r\n监视"))
            {
                UIxFontTool = !UIxFontTool;
            }
            else if (GUI.Button(new Rect(10, 80, 50, 50), "重新载入\r\xConfig"))
            {
                DTS_zh.xConfg.XConfg.HzConfig();
            }
            else if (GUI.Button(new Rect(70, 80, 50, 50), "重新载入\r\nzhText"))
            {
                DTS_Addon.xFont.XFont.LoadxText();
            }
            else if (GUI.Button(new Rect(130, 80, 50, 50), "重新载入\r\nzhItem"))
            {
                DTS_Addon.xItem.Load();
            }

            GUI.Label(new Rect(10, 150, 170, 20), HighLogic.LoadedScene.ToString());


            GUI.DragWindow(new Rect(0, 0, 190, 140));

        }

    }
}
