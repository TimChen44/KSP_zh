using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace DTS_zh
{
    [KSPAddon(KSPAddon.Startup.Instantly, false)]
    public class xConfg : MonoBehaviour
    {
        void OnGUI()
        {
            //if (GUI.Button(new Rect(50, 300, 100, 20), "再次加载"))
            //{
            //    List<Config> configs = LoadXml();
            //    Debug.LogWarning(configs.Count.ToString());

            //    loaded = false;
            //}
        }


        private bool loaded = false;

        public void Update()
        {
            if (loaded == true) return;
            HzConfig();
            loaded = true;
        }

        //汉化
        public void HzConfig()
        {
            List<Config> configs = LoadXml();

            foreach (var config in configs)
            {
                //var kspConfig = GameDatabase.Instance.root.AllConfigs
                //    .FirstOrDefault(x => x.name == config.Name && x.type == config.Name && x.url == config.Url);
                var kspConfig = GetUrlConfig(GameDatabase.Instance.root.AllConfigs, config);
                if (kspConfig == null)
                {
                    continue;
                }

                var nodecopy = DeepCopy(kspConfig.config);

                HzValues(nodecopy.values, config.Values);
                HzNodes(nodecopy.nodes, config.Nodes);

                kspConfig.config.ClearData();
                nodecopy.CopyTo(kspConfig.config);

                //HzValues(kspConfig.config.values, config.Values);

                //HzNodes(kspConfig.config.nodes, config.Nodes);
            }
        }

        private static ConfigNode DeepCopy(ConfigNode from)
        {
            ConfigNode to = new ConfigNode(from.name);
            foreach (ConfigNode.Value value in from.values)
                to.AddValue(value.name, value.value);
            foreach (ConfigNode node in from.nodes)
            {
                ConfigNode newNode = DeepCopy(node);
                to.nodes.Add(newNode);
            }
            return to;
        }

        //linq支持有问题，就用这个替代
        public UrlDir.UrlConfig GetUrlConfig(IEnumerable<UrlDir.UrlConfig> AllConfigs, Config config)
        {
            foreach (UrlDir.UrlConfig item in AllConfigs)
            {
                bool ItsYou = false;
                if (config.Url != null && item.url == config.Url)
                    ItsYou = true;
                else if (config.Name != null && item.name == config.Name)
                    ItsYou = true;

                if (ItsYou == true)
                {
                    //如果没有id就直接返回
                    if (config.Id == null || config.Id == "") return item;
                    //如果有ID，那么就多增加ID方面的判断-----//此处逻辑要修改，让他更加灵活
                    foreach (ConfigNode.Value kspValue in item.config.values)
                    {
                        if (kspValue.name == "id" && kspValue.value == config.Id)
                        {
                            return item;
                        }
                    }
                }
            }
            return null;
        }


        //汉化参数
        public void HzValues(ConfigNode.ValueList valueList, List<Value> values)
        {
            foreach (ConfigNode.Value kspValue in valueList)
            {
                var hzValue = GetValue(values, kspValue.name);
                //var hzValue = values.FirstOrDefault(x => x.name == kspValue.name);
                if (hzValue != null)
                {
                    kspValue.value = hzValue.value;
                    values.Remove(hzValue);
                }
            }
        }

        //linq支持有问题，就用这个替代
        public Value GetValue(List<Value> values, string name)
        {
            foreach (var item in values)
            {
                if (item.name == name) return item;
            }
            return null;
        }

        //汉化节点
        public void HzNodes(ConfigNode.ConfigNodeList nodeList, List<Node> nodes)
        {
            foreach (ConfigNode kspNode in nodeList)
            {
                var hzNode = GetNode(nodes, kspNode.name);
                // var hzNode = nodes.FirstOrDefault(x => x.Id == kspNode.id);
                if (hzNode != null)
                {
                    HzValues(kspNode.values, hzNode.Values);
                    HzNodes(kspNode.nodes, hzNode.Nodes);
                }
            }
        }

        //linq支持有问题，就用这个替代
        public Node GetNode(List<Node> nodes, string name)
        {
            foreach (var item in nodes)
            {
                if (item.Name == name) return item;
            }
            return null;
        }


        //载入汉化资源
        public List<Config> LoadXml()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("GameData/DTS_zh/zhConfig.xml");

            List<Config> configs = new List<Config>();
            foreach (XmlNode item in doc.ChildNodes)
            {
                if (item.Name == "Configs")
                {
                    LoadConfigs(item, configs);
                }
            }
            return configs;
        }

        //载入所有配置节点
        public void LoadConfigs(XmlNode parentNode, List<Config> configs)
        {
            foreach (XmlNode sitem in parentNode.ChildNodes)
            {
                if (!(sitem is XmlElement)) continue;
                var item = sitem as XmlElement;

                Config config = new Config();
                config.Name = item.GetAttribute("name");
                //config.Type = item.GetAttribute("type");
                config.Url = item.GetAttribute("url");
                config.Id = item.GetAttribute("id");

                if (config.Name == "") config.Name = null;
                if (config.Url == "") config.Url = null;

                foreach (XmlNode sonNode in item.ChildNodes)
                {
                    if (sonNode.Name == "Values")
                        LoadValues(sonNode, config.Values);
                    else if (sonNode.Name == "Nodes")
                        LoadNodes(sonNode, config.Nodes);
                }
                configs.Add(config);
            }


        }

        //载入子配置节点
        public void LoadNodes(XmlNode parentNode, List<Node> nodes)
        {
            foreach (XmlNode sitem in parentNode.ChildNodes)
            {
                if (!(sitem is XmlElement)) continue;
                var item = sitem as XmlElement;

                Node node = new Node();
                //node.Id = item.GetAttribute("id");
                node.Name = item.GetAttribute("name");

                foreach (XmlNode sonNode in item.ChildNodes)
                {
                    if (sonNode.Name == "Values")
                        LoadValues(sonNode, node.Values);
                    else if (sonNode.Name == "Nodes")
                        LoadNodes(sonNode, node.Nodes);
                }
                nodes.Add(node);
            }


        }
        //载入值的配置节点
        public void LoadValues(XmlNode parentNode, List<Value> values)
        {
            foreach (XmlNode sitem in parentNode.ChildNodes)
            {
                if (!(sitem is XmlElement)) continue;
                var item = sitem as XmlElement;

                Value value = new Value();
                value.name = item.GetAttribute("name");
                value.value = item.InnerText;
                values.Add(value);
            }
        }

    }


    //配置
    public class Config
    {
        public Config()
        {
            Values = new List<Value>();
            Nodes = new List<Node>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        //public string Type { get; set; }
        public string Url { get; set; }

        public List<Value> Values { get; set; }
        public List<Node> Nodes { get; set; }
    }

    public class Value
    {
        public string name { get; set; }
        public string value { get; set; }
    }

    public class Node
    {
        public Node()
        {
            Values = new List<Value>();
            Nodes = new List<Node>();
        }

        //public string Id { get; set; }
        public string Name { get; set; }

        public List<Value> Values { get; set; }
        public List<Node> Nodes { get; set; }
    }
}