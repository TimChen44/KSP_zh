using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.MainMenu, false)]
    public class xMainMenu : MonoBehaviour
    {
        void OnGUI()
        {
           // GUI.Label(new Rect(10, 10, 200, 20), "MainMenu");
        }
    }
}
