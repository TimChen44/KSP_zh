//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using UnityEngine;

//namespace DTS_Addon
//{
//    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
//    public class xImage : MonoBehaviour
//    {
//        private static Dictionary<string, int> config;


//        //记录是否载入

//        static List<int> LoadedLevels;

//        static Dictionary<string, string> ImageFiles;//文件列表

//        bool IsOver = false;

//        void Start()
//        {
//            if (ImageFiles != null) return;
//            ImageFiles = new Dictionary<string, string>();
//            foreach (var item in Directory.GetFiles("GameData/DTS_zh/Image/").ToList())
//            {//载入所有图片资源
//                ImageFiles.Add(Path.GetFileNameWithoutExtension(item), item);
//            }

//            if (config == null) LoadConfig();//载入配置

//            LoadedLevels = new List<int>();
//        }

//        void Update()
//        {

//            if (IsOver == true) return;
//            if (LoadedLevels.Contains(Application.loadedLevel)) return;
//            LoadedLevels.Add(Application.loadedLevel);


//            var mats = Resources.FindObjectsOfTypeAll<Material>();

//            foreach (var item in mats)
//            {  //便利所有材质
//                var texture = item.GetTexture("_MainTex");
//                if (texture == null) continue;

//                if (ImageFiles.ContainsKey(texture.name) == true)
//                {//找到可以替换的材质并替换
//                    Texture2D t2D = new Texture2D(config[texture.name], config[texture.name], TextureFormat.ARGB32, false);
//                    t2D.LoadImage(System.IO.File.ReadAllBytes(ImageFiles[texture.name]));
//                    item.SetTexture("_MainTex", t2D);
//                    ImageFiles.Remove(texture.name);

//                    GameObject go = new GameObject("DDOL_" + texture.name);
//                    go.AddComponent<MeshRenderer>().material = item;
//                    GameObject.DontDestroyOnLoad(go);
//                }
//            }
//            IsOver = true;
//        }

//        public static void LoadConfig()
//        {
//            XmlDocument doc = new XmlDocument();
//            config = new Dictionary<string, int>();
//            doc.Load(File.OpenRead("GameData/DTS_zh/zhImage.xml"));
//            foreach (XmlElement item in doc.DocumentElement.ChildNodes)
//            {
//                config[item.GetAttribute("id")] = int.Parse(item.InnerText);

//            }
//        }
//    }
//}