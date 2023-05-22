using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using LevanPangInterview.Events.Game.Score;
using LevanPangInterview.Models;
using TMPro;
using UnibusEvent;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

namespace LevanPangInterview.Views
{
	public class ScoreHUDView : View
	{
		//////// Members

        [Header("View Item")]
        public  GameObject                   ScoreHudItemPrefb;
        public  GameObject                   ScireHudItemsParent;

        // Private                           
        private List<ScoreHUDViewItem>       hudItems;

        private ObjectPool<ScoreHUDViewItem> HudItemPool;

        //////// Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log("System Init");

            this.BindUntilDestroy<Events.Game.Score.PlayerScoreUpdated>(OnPlayerScoreUpdated);

            this.HudItemPool = new ObjectPool<ScoreHUDViewItem>
            (
                createFunc      : () => Instantiate(original: ScoreHudItemPrefb
                                                   ,parent  : ScireHudItemsParent.transform).GetComponent<ScoreHUDViewItem>()
               ,actionOnGet     : (item) => item.gameObject.SetActive(true)
               ,actionOnRelease : (item) => item.gameObject.SetActive(false)
            );
        }

        public void Setup()
        {
            try
            {
                Logger.Log("Setup");

                // Create HudViewItem foe each active player int he game (in case it will ever be more than 2...)

                var PlayersData = Link.GetData<Models.PlayersCollection>();
                var gameData    = Link.GetData<GameplayModel>();

                var count       = gameData.PlayerCount;

                Logger.Log($"Count : {count}");

                hudItems = new List<ScoreHUDViewItem>();

                for (int i = count - 1; i >= 0; i--)
                {
                    var player    = PlayersData[i];
                    var hudItem   = this.HudItemPool.Get();

                    hudItem.BackgroundImage.color = player.Color;
                    hudItem.PlayerNameText.text   = player.Name + ":";
                    hudItem.PlayerScoreText.text  = player.Score.ToString();

                    this.hudItems.Add(hudItem);
                }

                hudItems.Reverse();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public void CleanUp()
        {
            Logger.Log("Cleanup");

            foreach (var hud in hudItems)
            {
                Destroy(hud.gameObject);
            }

            hudItems.Clear();
        }

        //////// Events

        private void OnPlayerScoreUpdated(PlayerScoreUpdated e)
        {
            hudItems[e.PlayerIndex].PlayerScoreText.text = e.PlayerScore.ToString();
        }
    } 
}
