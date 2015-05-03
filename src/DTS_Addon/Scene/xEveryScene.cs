using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.EveryScene, false)]
    public class xEveryScene : MonoBehaviour
    {
 
        void Start()
        {

        }




        void Update()
        {

        }


        //void OnGUI()
        //{
        //    if (mat !=null )
        //    {
        //        GUI.Label(new Rect(10, 10, 200, 30), ms);
        //        GUI.DrawTexture(new Rect(10, 50, 200, 200), mat.mainTexture);
        //        GUI.DrawTexture(new Rect(10, 250, 200, 200), mat2.mainTexture);

        //    }
        //    //GUI.Label(new Rect(10, 50, 300, 2000), a);


        //    if (GUI.Button(new Rect(210, 10, 100, 20), ""))
        //    {
        //        Load("cn10", "Arial10");
        //        Load("cn10", "Arial11");
        //        Load("cn12", "Arial12");
        //        Load("cn14", "Arial14");
        //        Load("cn14b", "Arial14Bold");
        //        Load("cn16", "Arial16");
        //        Load("cn16b", "Arial16_Mk2");
        //        Load("cn12", "Calibri12");
        //        Load("cn14", "Calibri14");
        //        Load("cn16", "Calibri16");
        //    }

        //}
        //Material mat;
        //Material mat2;
        //string ms = "";
        //public void Load(string cnFont, string enFont)
        //{
        //    try
        //    {
        //        if (DTS_zh.xFont.FontList.Count == 0)
        //        {
        //            DTS_zh.xFont.loadFontList();
        //        }

        //        var mats = Resources.FindObjectsOfTypeAll<Material>();
        //        foreach (var item in mats)
        //        {  //便利所有材质

        //            if (item.name == enFont)
        //            {
        //                item.SetTexture("_MainTex", DTS_zh.xFont.FontList[cnFont].fontMat.mainTexture);
        //                Debug.LogWarning("LoadedFont:" + item.name);
        //                mat = item;
        //                mat2 = DTS_zh.xFont.FontList[cnFont].fontMat;
        //                ms = enFont + " : " + cnFont;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.LogError(ex.ToString());
        //    }
        //}


    }



}
