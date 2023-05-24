using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Models;
using UnityEngine;

namespace LevanPangInterview.Views
{
    public class InputHUDView : View
    {
        // Members

        [Header("Mobile")]
        public GameObject OnePlayerLayout_Mobile;
        public GameObject TwoPlayerLayout_Mobile;

        [Header("PC")]
        public GameObject OnePlayerLayout_PC;
        public GameObject TwoPlayerLayout_PC;

        // Lifecycle

        public void Setup()
        {
            var  playerCount = Link.GetData<GameplayModel>().PlayerCount;
            bool isMobile    = (Application.platform == RuntimePlatform.Android) || (Link.AppSettings.Always_Show_Mobile_Controls); // (to be able to test in the editor)

            #if UNITY_ANDROID 
            isMobile = true; // for editor
            #endif

            bool isPC        = !isMobile;

            OnePlayerLayout_Mobile.SetActive( isMobile && playerCount == 1);
            TwoPlayerLayout_Mobile.SetActive( isMobile && playerCount >  1);
            OnePlayerLayout_PC    .SetActive(!isMobile && playerCount ==  1);
            TwoPlayerLayout_PC    .SetActive(!isMobile && playerCount > 1);
        }

        public void CleanUp()
        {
            // Nothing musch to cleanup yet.
        }

    }
}