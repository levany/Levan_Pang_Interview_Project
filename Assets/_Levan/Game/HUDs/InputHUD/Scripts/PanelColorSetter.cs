using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Models;
using UnityEngine;
using UnityEngine.UI;

namespace LevanPangInterview
{
    public class PanelColorSetter : MonoBehaviour
    {
        public int playerNumber = 0;

        public void OnEnable()
        {
            try
            {
                var image       = GetComponent<Image>();
                var players     = Link.GetData<PlayersCollection>();
                var settings    = Link.GetData<GameplayModel>();
                
                var playerCount = settings.PlayerCount;

                if (playerCount < playerNumber) 
                    return;

                var player      = players[playerNumber - 1];
                var color       = player.Color;

                image.color  = color;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{ex.Message}");
            }
        }
    }

}