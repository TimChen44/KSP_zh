using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;


[KSPAddon(KSPAddon.Startup.MainMenu, false)]
class xMenu : MonoBehaviour
{
    bool isover = false;

    void Update()
    {
        if (isover == true) return;
        var menu = GameObject.Find("MainMenu");
        if (menu == null) return;

        var mainMenu = menu.GetComponent<MainMenu>();

        TM(mainMenu.backBtn, "返回");
        TM(mainMenu.startBtn, "开始游戏");
        TM(mainMenu.settingBtn, "设置");
        TM(mainMenu.commBtn, "社区");
        TM(mainMenu.continueBtn, "继续游戏");
        TM(mainMenu.creditsBtn, "制作组");
        //TM(mainMenu.instrcBtn, "test");
        TM(mainMenu.newGameBtn, "新游戏");

        TM(mainMenu.quitBtn, "退出");
        TM(mainMenu.scenariosBtn, "场景");
        TM(mainMenu.settingBtn, "设置");
        //TM(mainMenu.spaceportBtn, "SpacePort");
        TM(mainMenu.trainingBtn, "训练");

        TextMesh t1 = mainMenu.updBtn.transform.GetComponent<TextMesh>();
        t1.text = "版本：壹点贰\r\n\r\n如果你喜欢本游戏请购买正版。\r\n\r\n如果你喜欢本游戏的汉化并想让他更加完善请访问https://github.com/TimChen44/KSP_zh\r\n\r\n如果你用盗版游戏并使用本汉化造成的电脑爆炸、房屋倒塌等灾难本汉化组一概不负责任。\r\n\r\n警告：对因为使用本汉化或游戏安装路径中含有中文字符产生的任何非自然现象请自行解决。";
        t1.fontSize = 20;
        mainMenu.updBtn.gameObject.SetActive(true);

        menu.transform.FindChild("stage 2").FindChild("Header").GetComponent<TextMesh>().text = "开始游戏";

        isover = true;
    }

    public static void TM(TextButton3D tb3D, string text)
    {
        TextMesh t1 = tb3D.transform.GetComponent<TextMesh>();
        t1.text = text;
        t1.fontSize = 20;
    }

    void OnGUI()
    {

    }




}

