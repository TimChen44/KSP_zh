using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    public class xEditorAny : MonoBehaviour
    {
        [Conditional("DEBUG")]
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "EditorAny");
        }
    }
}
