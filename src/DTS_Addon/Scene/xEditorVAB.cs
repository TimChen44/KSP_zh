using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Diagnostics;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.EditorVAB, false)]
    public class xEditorVAB : MonoBehaviour
    {
        [Conditional("DEBUG")]
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "EditorVAB");
        }
    }
}
