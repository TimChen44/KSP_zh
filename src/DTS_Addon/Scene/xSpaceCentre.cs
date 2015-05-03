using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DTS_Addon.Scene
{
    [KSPAddon(KSPAddon.Startup.SpaceCentre, false)]
    public class xSpaceCentre : MonoBehaviour
    {
        ScreenSafeGUIText SSGT;

        void Start()
        {
            SSGT = GameObject.FindObjectOfType<ScreenSafeGUIText>();
            SSGT.textSize = 20;
        }


        void Update()
        {
            if ((SSGT.text != "") || (SSGT.text != null))
            {
                switch (SSGT.text)
                {
                    case "Vehicle Assembly Building":
                        SSGT.text = "航天器装配大楼";
                        break;
                    case "Astronaut Complex":
                        SSGT.text = "航天员训练中心";
                        break;
                    case "Spaceplane Hangar":
                        SSGT.text = "航天飞机机库";
                        break;
                    case "Flag Pole":
                        SSGT.text = "旗杆";
                        break;
                    case "Research and Development":
                        SSGT.text = "研究与开发中心";
                        break;
                    case "Tracking Station":
                        SSGT.text = "太空任务跟踪站";
                        break;
                    case "Launch Pad":
                        SSGT.text = "发射台";
                        break;
                    case "Runway":
                        SSGT.text = "跑道";
                        break;
                    case "Mission Control":
                        SSGT.text = "任务控制中心";
                        break;
                }
                
            }
        }
    }
}
