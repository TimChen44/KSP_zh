using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;
namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class ConfigTest : MonoBehaviour
    {
        Vector2 hc;

        int h = 0;
        Rect window = new Rect(100, 100, 600, 600);


        string find = "";

        string tp = "";

        string findvalue = "";
        string fvalue = "";
        string urlvalue = "";
        string maxLevel = "3";

        void OnGUI()
        {
            if (SuperTools.ConfigTest == false) return;
            window = GUI.Window(104, window, CNodeWindow, "资源");
        }

       static  bool testMody = false;

        void CNodeWindow(int id)
        {
            GUI.DragWindow(new Rect(0, 0, 580, 30));

            GUI.Label(new Rect(5, 20, 100, 20), "最大深度");
            maxLevel = GUI.TextField(new Rect(105, 20, 100, 25), maxLevel);

            GUI.Label(new Rect(5, 60, 100, 20), "检索");
            GUI.Label(new Rect(105, 40, 100, 20), "name");
            GUI.Label(new Rect(205, 40, 100, 20), "type");
            GUI.Label(new Rect(305, 40, 100, 20), "value1");
            GUI.Label(new Rect(405, 40, 100, 20), "value2");
            GUI.Label(new Rect(505, 40, 100, 20), "url");

            find = GUI.TextField(new Rect(105, 60, 100, 25), find);
            tp = GUI.TextField(new Rect(205, 60, 100, 25), tp);
            fvalue = GUI.TextField(new Rect(305, 60, 100, 25), fvalue);
            findvalue = GUI.TextField(new Rect(405,60, 100, 25), findvalue);
            urlvalue = GUI.TextField(new Rect(505, 60, 100, 25), urlvalue);

            h = 0;
            hc = GUI.BeginScrollView(new Rect(0, 90, 600, 600), hc, new Rect(0, 0, 590, 40000));

            //sht("GameDatabase.Instance.root");
            sht("name", GameDatabase.Instance.root.name, 0);
            //sht("path", GameDatabase.Instance.root.path);
            sht("url", GameDatabase.Instance.root.url, 0);
            sht("type", GameDatabase.Instance.root.type.ToString(), 0);

            GUI.Label(new Rect(0, h * 20, 200, 20), "                          ");
            h++;

            //sht("GameDatabase.Instance.root.AllConfigs");
            foreach (UrlDir.UrlConfig config in GameDatabase.Instance.root.AllConfigs)
            {
                if (config.name.Contains(find) == false) continue;
                if (config.type.Contains(tp) == false) continue;
                if (config.url.Contains(urlvalue) == false) continue;

                sht("name", config.name, 0);
                sht("type", config.type, 0);//AGENT,PART,PROP,RESOURCE_DEFINITION,EXPERIMENT_DEFINITION,STORY_DEF,INTERNAL,
                sht("url", config.url, 0);

                sht("parent.name", config.parent.name, 0);
                sht("parent.url", config.parent.url, 0);
                sht("parent.fileType", config.parent.fileType.ToString(), 0);

                //sht("GameDatabase.Instance.root.AllConfigs.config");

                shlabel("values.Count", config.config.values.Count.ToString(), 0);

                //if (config.url == "Squad/Parts/Command/advSasModule/part/advSasModule" && testMody == false)
                //{
                //    testMody = true;
                //    config.config.AddValue("--description", "这个系统使用一组以'相当'高速旋转的圆盘来产生控制飞船必需的扭矩.还包含");
                //}
                
                foreach (ConfigNode.Value value in config.config.values)
                {
                    shValue(value, 0);
                }

                shlabel("nodes.Count", config.config.nodes.Count.ToString(), 0);

                showNode(config.config.nodes, 1);

                shh1();
            }
            GUI.EndScrollView();
        }


        public void showNode(ConfigNode.ConfigNodeList nodes, int level)
        {
           
            foreach (ConfigNode node in nodes)
            {
                GUI.Label(new Rect(0 + level*30, h * 20, 200, 20), "--------------------------");
                h++;

                sht("node.id", node.id, level);
                sht("node.name", node.name, level);

                shlabel("values.Count", node.values.Count.ToString(), level);

                if (level > System.Convert.ToInt32( maxLevel)) return;
                foreach (ConfigNode.Value value in node.values)
                {
                    shValue(value, level);
                }

                shlabel("nodes.Count", node.nodes.Count.ToString(), level);
                showNode(node.nodes, level + 1);

            }
        }


        void sht(string name)
        {
            if (findvalue != "") return;
            GUI.Label(new Rect(0, h * 20, 590, 20), name);

            h++;
        }

        void sht(string name, string value, int level)
        {
            if (findvalue != "") return;
            GUI.Label(new Rect(5 + level * 30, h * 20, 140 - level * 30, 20), name);
            GUI.TextField(new Rect(140 + level * 30, h * 20, 450 - level * 30, 20), value);
            h++;
        }

        void shlabel(string name, string value, int level)
        {
            if (findvalue != "") return;
            GUI.Label(new Rect(5 + level * 30, h * 20, 140 - level * 30, 20), name);
            GUI.Label(new Rect(140 + level * 30, h * 20, 450 - level * 30, 20), value);
            h++;
        }

        void shh1()
        {
            if (findvalue != "") return;
            GUI.Label(new Rect(0, h * 20, 200, 20), "                          ");
            h++;
            GUI.Label(new Rect(0, h * 20, 200, 20), "==========================");
            h++;

        }


        void shValue(ConfigNode.Value value, int level)
        {
            if (value.value.Contains(findvalue) == false) return;
            if (value.value.Contains(fvalue) == false) return;
            GUI.Label(new Rect(5 + level * 30, h * 20, 140 - level * 30, 20), value.name);
            value.value = GUI.TextField(new Rect(140 + level * 30, h * 20, 450 - level * 30, 20), value.value);

            h++;
        }
    }
}