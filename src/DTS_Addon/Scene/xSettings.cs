﻿using System;
using System.Collections.Generic;

using System.Text;
using System.Diagnostics;
using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.Settings, false)]
    public class xSettings : MonoBehaviour
    {
        [Conditional("DEBUG")]
        void OnGUI()
        {
          GUI.Label(new Rect(10, 10, 200, 20), "Settings");
        }
    }
}
