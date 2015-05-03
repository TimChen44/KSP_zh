using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using UnityEngine;

public  class TimDebug
{

    public  void OnGUI(MainMenu mainMenu)
    {

       // GUIStyle gs = new GUIStyle(GUI.skin.GetStyle("Label"));
       // gs.fontSize = 28;
       // gs.fontStyle = FontStyle.Bold;
       //// gs.font.material =    mainMenu .backBtn.renderer.material;

       // GUI.Label(new Rect(220, 220, 500, 500), "深度时空荣誉汉化", gs);

       // GUI.Label(new Rect(300, 300, 500, 500), "深度时空荣誉汉化", gs);
       // GUI.Label(new Rect(400, 200, 500, 500), "深度时空荣誉汉化", gs);
       // GUI.Label(new Rect(200, 400, 500, 500), "深度时空荣誉汉化", gs);

        sss = "ccc";
    }

    private string sss = "dddd";
    //public static void getText(String text)
    //{
    //    logout(String.Format("This Text is '{0}'",text));
    //}
    // public static void logout(String text)
    // {
    //     text = text + System.Environment.NewLine;
    //     FileStream fs = new FileStream("logout.log", FileMode.Append); //获得字节数组
    //     byte [] data =new UTF8Encoding().GetBytes(text); //开始写入  
    //     fs.Write (data, 0, data.Length);    //清空缓冲区、关闭流 fs.Flush(); 
    //     fs.Close();  
    // }

}