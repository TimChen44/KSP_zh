using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Diagnostics;
namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class xFlight : MonoBehaviour
    {
        void Start()
        {
    
        }

        void Update()
        {

        }
        [Conditional("DEBUG")]
        void OnGUI()
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Flight");
        }
    }
}
