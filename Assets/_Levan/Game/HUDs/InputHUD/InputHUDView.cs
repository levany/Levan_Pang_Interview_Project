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

        public GameObject OnePlayerLayout;
        public GameObject TwoPlayerLayout;

        // Lifecycle

        public void Setup()
        {
            var playerCount = Link.GetData<GameplayModel>().PlayerCount;

            if (playerCount == 1)
            {
                OnePlayerLayout.SetActive(true);
                TwoPlayerLayout.SetActive(false);
            }
            else if (playerCount == 2)
            {
                TwoPlayerLayout.SetActive(true);
                OnePlayerLayout.SetActive(false);
            }
            else
            {
                // the screen is not big enough for this
            }
        }

        public void CleanUp()
        {
            // Nothing musch to cleanup yet.
        }
    }
}