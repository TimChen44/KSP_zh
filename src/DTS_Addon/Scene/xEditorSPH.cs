using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.EditorSPH, false)]
    public class xEditorSPH : MonoBehaviour
    {
        [Conditional("DEBUG")]
        void OnGUI()
        {
           // GUI.Label(new Rect(10, 10, 200, 20), "EditorSPH");
        }
    }
}
