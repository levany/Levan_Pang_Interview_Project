using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevanPangInterview.Models
{
    [CreateAssetMenu(fileName = "AppSettings", menuName = "Models/AppSettings", order = 0)]
    public class AppSettings : Model
    {
        public bool              ShouldStrechWorldToFitScreen   = true;
        public Key               EascapeKey                     = Key.Escape;
        public ScreenOrientation ScreenOrientation              = ScreenOrientation.LandscapeLeft;

        public bool              Always_Show_Android_Controls   = true; // Its easier to test;

        public int               MaxSupportedPlayers            = 2;

        public bool              Use_Simple_Cloud_Service       = true;

        public bool              CHEAT_HERO_CANT_DIE            = false;
        public bool              CHEAT_ESC_KEY_WINS_LEVEL       = false;
    }
}
