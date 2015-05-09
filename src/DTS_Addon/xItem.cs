using System;
using System.Collections.Generic;

using System.Text;
using System.Xml;
using UnityEngine;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xItem : MonoBehaviour
    {
        static List<zItem> zItems;//所有需要汉化的对象

        List<zItem> NzItems;//当前屏幕的对象

        GameObject _UI;




        void Start()
        {
            if (zItems == null)
                Load();

            _UI = GameObject.Find("_UI");

            if (_UI != null)
            {
                Debug.Log("UI..................................................");
            }
            if (zItems != null)
            {
                NzItems = new List<zItem>();
                foreach (var item in zItems)
                {
                    if (item.Scene == HighLogic.LoadedScene)
                    {
                        NzItems.Add(item);
                    }

                }
            }
        }

        void Update()
        {
            if (_UI == null) return;
            foreach (var item in NzItems)
            {
                if (item.NowObject == null)
                {
                    item.NowObject = _UI.transform.Find(item.Path);
                }
                if (item.NowObject != null)
                {
                    switch (item.Type)
                    {
                        case "SpriteText":
                            var st = item.NowObject.gameObject.GetComponent<SpriteText>();
                            if (st == null) continue;
                            if (item.zDict.ContainsKey(st.Text))
                            {
                                st.Text = item.zDict[st.Text];
                            }
                            break;
                        case "SpriteTextRich":
                            var str = item.NowObject.gameObject.GetComponent<SpriteTextRich>();
                            if (str == null) continue;
                            if (item.zDict.ContainsKey(str.Text))
                            {
                                str.Text = item.zDict[str.Text];
                            }
                            break;
                    }
                }
            }
        }

        void OnGUI()
        {
            //p = GUI.TextField(new Rect(10, 50, 300, 20), p);

            //if (GUI.Button(new Rect(10, 100, 100, 20), "ABC"))
            //{
            //    tf = _UI.transform.Find(p);
            //}

            //if (tf != null)
            //{
            //    GUI.Label(new Rect(10, 150, 200, 20), tf.gameObject.name);
            //}
        }


        public static void Load()
        {
            //载入资源
            XmlDocument doc = new XmlDocument();
            doc.Load("GameData/DTS_zh/zhItem.xml");

            zItems = new List<zItem>();
            foreach (XmlNode items in doc.ChildNodes)
            {
                if (items.Name == "Items")
                {
                    foreach (XmlNode item in items.ChildNodes)
                    {
                        if (!(item is XmlElement)) continue;
                        var itemElement = item as XmlElement;

                        zItem zitem = new zItem();
                        zitem.Scene = ToGameScenes(itemElement.GetAttribute("scene"));
                        zitem.Path = itemElement.GetAttribute("path");
                        zitem.Type = itemElement.GetAttribute("type");

                        zitem.zDict = new Dictionary<string, string>();
                        foreach (XmlNode itemString in item.ChildNodes)
                        {
                            if (!(itemString is XmlElement)) continue;
                            zitem.zDict[((XmlElement)itemString).GetAttribute("name")] = itemString.InnerText;
                        }
                        zItems.Add(zitem);
                    }
                }
            }

            Debug.Log("xItem Loaded:" + zItems.Count.ToString());


        }

        public static GameScenes ToGameScenes(string scene)
        {
            switch (scene.ToUpper())
            {
                case "LOADING":
                    return GameScenes.LOADING;
                case "LOADINGBUFFER":
                    return GameScenes.LOADINGBUFFER;
                case "MAINMENU":
                    return GameScenes.MAINMENU;
                case "SETTINGS":
                    return GameScenes.SETTINGS;
                case "CREDITS":
                    return GameScenes.CREDITS;
                case "SPACECENTER":
                    return GameScenes.SPACECENTER;
                case "EDITOR":
                    return GameScenes.EDITOR;
                case "FLIGHT":
                    return GameScenes.FLIGHT;
                case "TRACKSTATION":
                    return GameScenes.TRACKSTATION;
                //case "SPH":
                //    return GameScenes.SPH;
                case "PSYSTEM":
                    return GameScenes.PSYSTEM;

                default:
                    return GameScenes.LOADING;
            }

        }


        public class zItem
        {
            public GameScenes Scene { get; set; }

            public string Path { get; set; }

            public string Type { get; set; }

            //需要修改的对象
            public Transform NowObject { get; set; }

            public Dictionary<string, string> zDict { get; set; }


        }

    }
}