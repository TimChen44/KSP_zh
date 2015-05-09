using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.PSystemSpawn, false)]
    public class xPSystemSpawn : MonoBehaviour
    {
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "PSystemSpawn");
        }
    }
}
