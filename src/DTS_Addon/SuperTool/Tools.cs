using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DTS_Addon
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class Tools : MonoBehaviour
    {
        void Start()
        {

        }


        void Update()
        {
            #region 对象捕捉

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.C))
            {
                CaptureRun = !CaptureRun;
            }

            if (CaptureRun == true && SuperTools.UICapture == true)
            {
                if (CaptureCamera == null) CaptureCamera = Camera.main;
                RaycastHit hit = new RaycastHit();
                Physics.Raycast(CaptureCamera.ScreenPointToRay(Input.mousePosition), out hit, 90000);
                if (hit.transform != null)
                {
                    CaptureObjct = hit.transform.gameObject;

                    var path = CaptureObjct.name;
                    var pTransform = CaptureObjct.transform;
                    while (pTransform.parent != null)
                    {
                        pTransform = pTransform.parent;
                        path = pTransform.name + "\\" + path;
                    }

                    if (CapturePath != path)
                    {//地址不一样说明换了一个东西，所以要刷新
                        CapturePath = path;
                        CaptureComponents = CaptureObjct.GetComponents<Component>().ToList();
                    }
                }
            }

            #endregion
        }


        string a = "";

        void OnGUI()
        {
            //a = GUI.TextField(new Rect(50, 50, 100, 20), a);

            //if (GUI.Button(new Rect(50, 75, 100, 20), "3423"))
            //{
            //    var go = GameObject.Find("ProgressBar");
            //    go.transform.localScale = new Vector3(1, 5, 1);
            //    go.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, go.transform.position.z);
            //}
            if (SuperTools.UITree == true)
                NodeWindow = GUI.Window(101, NodeWindow, CNodeWindow, "对象树");

            if (SObject != null && (SuperTools.UITree == true || SuperTools.UICapture == true))
                ComponentsWindow = GUI.Window(102, ComponentsWindow, CComponentsWindow, "对象组件:" + SObject.name);

            if (SuperTools.UICapture == true)
                CaptureWindow = GUI.Window(103, CaptureWindow, CCaptureWindow, "捕获对象:");
        }

        #region 对橡树
        public List<Node> Nodes;

        Rect NodeWindow = new Rect(100, 100, 500, 500);

        //定义存储滚动条的位置变量  
        Vector2 scrollPosition;
        Vector3 viewRect = new Vector3(0, 0, 0);

        string findText = "";

        void CNodeWindow(int id)
        {
            if (GUI.Button(new Rect(5, 20, 60, 20), "扫描对象"))
            {
                GetNodes();
            }

            /*************/
            findText = GUI.TextField(new Rect(70, 20, 100, 20), findText);

            if (GUI.Button(new Rect(170, 20, 60, 20), "fName"))
            {
                foreach (var item in Nodes) fNameNode(item);
            }
            else if (GUI.Button(new Rect(230, 20, 60, 20), "fTextMesh"))
            {
                foreach (var item in Nodes) fTextMeshNode(item);
            }
            else if (GUI.Button(new Rect(290, 20, 60, 20), "SpriteText"))
            {
                foreach (var item in Nodes) fSpriteText(item);
            }
            else if (GUI.Button(new Rect(350, 20, 60, 20), "AllNode"))
            {
                foreach (var item in Nodes) fAllNode(item);
            }
            /*-----------*/

            GUI.DragWindow(new Rect(0, 0, 480, 30));
            if (Nodes == null) return;



            //开始滚动视图  
            scrollPosition = GUI.BeginScrollView(new Rect(5, 50, 490, 460), scrollPosition, new Rect(0, 0, 480, Node.nodeSort * 20));
            nSort = 0;
            foreach (var item in Nodes)
            {
                GUINode(item, 0);
            }
            //结束滚动视图  
            GUI.EndScrollView();
            a = nSort.ToString();
        }

        /**********/
        public void fNameNode(Node node)
        {
            if (node.gameObject != null)
            {
                node.visable = node.gameObject.name.ToLower().Contains(findText.ToLower());
                if (node.visable == true) SetP(node);
            }
            else
            {
                node.visable = false;
            }

            foreach (var item in node.Nodes)
            {
                fNameNode(item);
            }
        }

        public void fTextMeshNode(Node node)
        {
            if (node.gameObject != null)
            {
                var textMesh = node.gameObject.GetComponent<TextMesh>();
                if (textMesh != null)
                {
                    node.visable = textMesh.text.ToLower().Contains(findText.ToLower());
                    if (node.visable == true) SetP(node);
                }
                else
                {
                    node.visable = false;
                }
            }
            else
            {
                node.visable = false;
            }

            foreach (var item in node.Nodes)
            {
                fTextMeshNode(item);
            }
        }
        public void fSpriteText(Node node)
        {
            if (node.gameObject != null)
            {
                var spriteText = node.gameObject.GetComponent<SpriteText>();
                if (spriteText != null)
                {
                    node.visable = spriteText.text.ToLower().Contains(findText.ToLower());
                    if (node.visable == true) SetP(node);
                }
                else
                {
                    node.visable = false;
                }
            }
            else
            {
                node.visable = false;
            }

            foreach (var item in node.Nodes)
            {
                fTextMeshNode(item);
            }
        }

        public void SetP(Node node)
        {//设置当前标签和母标签
            node.IsFind = true;
            var p = node.Parent;
            while (p != null)
            {
                p.Ext = true;
                p.visable = true;
                p.IsFind = false;
                p = p.Parent;
            }
        }

        public void fAllNode(Node node)
        {//显示全部
            node.visable = true;
            foreach (var item in node.Nodes)
            {
                fAllNode(item);
            }
        }

        /*-----------*/

        //获得所有变化的树
        private void GetNodes()
        {
            Nodes = new List<Node>();

            var mbs = GameObject.FindObjectsOfType<Transform>();


            List<Transform> roots = new List<Transform>();

            foreach (var item in mbs)
            {
                if (item.parent == null) roots.Add(item);
            }
            //a = roots.Count.ToString(); 

            //var roots = mbs.Where(x => x.parent == null).ToList();

            Node.nodeSort = 0;

            foreach (var item in roots)
            {
                Nodes.Add(new Node(item.gameObject, mbs));
            }

        }

        int nSort = 0;//显示的位置

        public void GUINode(Node node, int level)
        {
            if (node.visable == true)/************/
            {
                var ext = node.GetExt();
                if (ext != "")
                {
                    if (GUI.Button(new Rect(level * 20, nSort * 20, 20, 20), ext))
                    {
                        node.Ext = !node.Ext;
                    }
                }

                GUI.TextField(new Rect(20 + level * 20, nSort * 20, 380 - level * 20, 20), node.GetTitle());
                if (node.gameObject != null)
                {
                    if (GUI.Button(new Rect(400, nSort * 20, 40, 20), "显/藏"))
                    {
                        node.gameObject.SetActive(!node.gameObject.activeSelf);
                    }
                    else if (GUI.Button(new Rect(440, nSort * 20, 30, 20), "详"))
                    {
                        SObject = node.gameObject;
                        Components = node.gameObject.GetComponents<Component>().ToList();
                    }
                }
                nSort += 1;//增加一位
            }

            if (node.Ext == true)
            {
                foreach (var item in node.Nodes)
                {
                    GUINode(item, level + 1);
                }
            }
        }

        #endregion

        #region 对象组件

        public GameObject SObject { get; set; }
        public List<Component> Components { get; set; }
        public Component Component { get; set; }

        Rect ComponentsWindow = new Rect(100, 100, 300, 500);
        Vector2 CCscrollPosition;
        Vector2 ComponentPosition;

        void CComponentsWindow(int id)
        {
            if (SObject != null)
            {
                var p = SObject.transform;
                string s = SObject.name;
                while (p.parent != null)
                {
                    p = p.parent;
                    s = p.name + "/" + s;
                }
                GUI.TextField(new Rect(5, 100, 200, 20), s);




                GUI.Label(new Rect(5, 25, 200, 20), "P:" + SObject.transform.position.ToString());
                GUI.Label(new Rect(5, 50, 200, 20), "R:" + SObject.transform.localEulerAngles.ToString());
                GUI.Label(new Rect(5, 75, 200, 20), "S:" + SObject.transform.lossyScale.ToString());

                //组件清单
                CCscrollPosition = GUI.BeginScrollView(new Rect(5, 125, 290, 150), CCscrollPosition, new Rect(0, 0, 290, Components.Count * 20));
                for (int i = 0; i < Components.Count; i++)
                {
                    var compt = Components[i];
                    GUI.Label(new Rect(0, i * 20, 260, 20), (compt.gameObject.activeSelf == true ? "O" : "X") + " - " + compt.GetType().Name + ":" + compt.GetType().FullName);
                    if (GUI.Button(new Rect(260, i * 20, 30, 20), "编"))
                    {
                        Component = compt;
                    }
                }
                //结束滚动视图  
                GUI.EndScrollView();





                //SpriteTextRich str = SObject.GetComponent<SpriteTextRich>();

                //if (str != null)
                //{
                //    GUI.Label(new Rect(0, 0, 60, 20), "Text");
                //    str.Text = GUI.TextField(new Rect(60, 0, 190, 20), str.Text);


                //    SpriteFontMultiple sfm = str.font;
                //    GUI.Label(new Rect(0, 20, 60, 20), "sfm.name");
                //    str.Text = GUI.TextField(new Rect(60, 20, 190, 20), sfm.name);

                //    for (int i = 0; i < str.font.fonts.Length; i++)
                //    {
                //        SpriteFontMultiple.SpriteFontInstance sfi = str.font.fonts[i];

                //        GUI.Label(new Rect(0, 40 + i * 40, 60, 20), i.ToString() + ".name");
                //        str.Text = GUI.TextField(new Rect(60, 40 + i * 40, 190, 20), sfi.name);

                //        GUI.Label(new Rect(0, 60 + i * 40, 60, 20), i.ToString() + ".fontText");
                //        GUI.TextField(new Rect(60, 40 + i * 40, 190, 20), sfi.material.name);
                //    }

                //}


                if (Component != null)
                {
                    if (Component is MeshFilter)
                    {
                        var mg = Component as MeshFilter;

                        var mat = mg.GetComponent<Renderer>().material;
                        if (mat != null)
                        {
                            GUI.TextField(new Rect(135, 25, 165, 20), "mat:" + mat.name);

                            if (mat.shader != null)
                            {
                                GUI.TextField(new Rect(135, 50, 165, 20), "shader:" + mat.shader.name);
                            }

                            var texture = mat.GetTexture("_MainTex");
                            if (texture != null)
                            {
                                GUI.TextField(new Rect(135, 75, 165, 20), "tex:" + texture.name + "  " + texture.texelSize.ToString());

                                var ps = texture.GetType().GetProperties();
                                //组件数据修改  
                                ComponentPosition = GUI.BeginScrollView(new Rect(5, 250, 290, 250), ComponentPosition, new Rect(0, 0, 280, ps.Length * 20));

                                for (int i = 0; i < ps.Length; i++)
                                {
                                    GUI.Label(new Rect(0, i * 20, 100, 20), ps[i].Name + ":");

                                    object value = ps[i].GetValue(texture, null);

                                    var v = GUI.TextField(new Rect(100, i * 20, 150, 20), value == null ? "null" : value.ToString());

                                }
                                //结束滚动视图  
                                GUI.EndScrollView();
                            }

                        }

                     

                    }
                    else
                    {
                        var ps = Component.GetType().GetProperties();
                        //组件数据修改  
                        ComponentPosition = GUI.BeginScrollView(new Rect(5, 250, 290, 250), ComponentPosition, new Rect(0, 0, 280, ps.Length * 20));

                        for (int i = 0; i < ps.Length; i++)
                        {
                            GUI.Label(new Rect(0, i * 20, 60, 20), ps[i].Name + ":");

                            object value = ps[i].GetValue(Component, null);

                            var v = GUI.TextField(new Rect(100, i * 20, 190, 20), value == null ? "null" : value.ToString());

                        }
                        //结束滚动视图  
                        GUI.EndScrollView();



                    }
                }

            }

            GUI.DragWindow(new Rect(0, 0, 200, 30));
        }


        #endregion


        #region 对象捕捉

        bool CaptureRun = false;

        GameObject CaptureObjct;

        Rect CaptureWindow = new Rect(100, 100, 600, 200);

        Vector2 CapturePosition;
        public List<Component> CaptureComponents { get; set; }
        string CapturePath = "";

        Camera CaptureCamera;

        void CCaptureWindow(int id)
        {
            GUI.Label(new Rect(0, 0, 200, 20), "激活热键：LeftCtrl + LeftAlt + C");

            GUI.Label(new Rect(0, 20, 50, 20), CaptureRun.ToString());

            if (CaptureCamera != null)
                GUI.Label(new Rect(50, 20, 100, 20), CaptureCamera.name);

            int l = 0;
            foreach (var item in Camera.allCameras)
            {
                if (GUI.Button(new Rect(l * 80 + 150, 20, 80, 20), item.name))
                {
                    CaptureCamera = item;

                }
                l++;
            }

            GUI.DragWindow(new Rect(0, 0, 600, 30));
            if (CaptureObjct != null)
            {
                GUI.TextField(new Rect(5, 50, 590, 20), CapturePath);

                GUI.Label(new Rect(5, 75, 590, 20), "P:" + CaptureObjct.transform.position.ToString() + "   " +
                    "R:" + CaptureObjct.transform.localEulerAngles.ToString() + "   " +
                    "S:" + CaptureObjct.transform.lossyScale.ToString());

                //组件清单
                CapturePosition = GUI.BeginScrollView(new Rect(5, 100, 590, 100), CapturePosition, new Rect(0, 0, 590, CaptureComponents.Count * 20));
                for (int i = 0; i < CaptureComponents.Count; i++)
                {
                    var compt = CaptureComponents[i];
                    GUI.Label(new Rect(0, i * 20, 260, 20), (compt.gameObject.activeSelf == true ? "O" : "X") + " - " + compt.GetType().Name + ":" + compt.GetType().FullName);
                    if (GUI.Button(new Rect(260, i * 20, 30, 20), "编"))
                    {
                        SObject = compt.gameObject;
                        Components = compt.gameObject.GetComponents<Component>().ToList();
                        Component = compt;
                    }
                }
                //结束滚动视图  
                GUI.EndScrollView();
            }
        }

        #endregion
    }

    //变换节点
    public class Node
    {
        public static int nodeSort = 0;

        public Node(GameObject go, Transform[] mbs)
        {
            visable = true;
            Ext = false;

            gameObject = go;
            name = go.name;
            tag = go.tag;

            /************/
            var scripts = go.GetComponents<MonoBehaviour>();
            if (scripts.Length > 0)
            {
                HasScript = true;

                foreach (var item in scripts)
                {
                    Script += item.GetType().Name + ",";
                }
                Script.TrimEnd(',');
            }
            oneTitle = string.Format("{2}{1}-{0}({3})", go.name, go.activeSelf == true ? "O" : "X", HasScript == true ? ">" : "", Script);
            /*-----------*/

            sort = nodeSort;
            nodeSort += 1;

            Nodes = new List<Node>();

            foreach (var mb in mbs)
            {
                if (mb.parent != go.transform) continue;
                var node = new Node(mb.gameObject, mbs);
                node.Parent = this;
                Nodes.Add(node);
            }
        }

        public string GetTitle()
        {
            if (gameObject != null)
                return string.Format("{2}-{0} - {1} {3}", gameObject.tag, gameObject.name,
                    gameObject.activeSelf == true ? "O" : "X",
                   IsFind == true ? "<--" : "");
            else
                return "--" + oneTitle;
        }

        public string tag { get; set; }
        public string name { get; set; }

        public int sort { get; set; }

        public string oneTitle { get; set; }

        public GameObject gameObject { get; set; }

        public List<Node> Nodes { get; set; }

        public Node Parent { get; set; }

        public bool Ext { get; set; }

        public string GetExt()
        {
            if (Nodes.Count == 0) return "";
            else if (Ext == true) return "-";
            else return "+";
        }

        /**********/
        public bool visable { get; set; }

        public bool HasScript = false;

        public string Script = "";


        public bool IsFind = false;
    }

}