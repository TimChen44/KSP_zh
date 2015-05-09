using System;
using System.Collections.Generic;

using System.Text;

using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.TrackingStation, false)]
    public class xTrackingStation : MonoBehaviour
    {

        
        void Start()
        {
            //ScreenSafeUI ssui = new ScreenSafeUI();

        
         

        }


        void Update()
        {
          //  var go=GameObject.Find("description");
          //if (go!=null )
          //{
          //    var st = go.GetComponent<SpriteText>();
          //    if (st != null) st.Text = DateTime.Now.ToString();
          //}
        }

        void OnGUI()
        {
            
        }
    }
}
