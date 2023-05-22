using System;
using System.Collections;
using System.Collections.Generic;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Views
{
    public enum MainMenuResult
    {
        None,
        SinglePlayer,
        MultiPlayer,
        Leaderboards
    }

    public class MainMenuView : View
    {
        //// Properties
        
        public MainMenuResult result = MainMenuResult.None;

        //// UI Events

        public void OnSinglePlayerButtonClicked()
        {
            Logger.Log($"MainMenuView.OnSinglePlayerButtonClicked()");

          //Unibus.Dispatch(new Events.App.StartSinglePlayer()); // Not needed
            this.result = MainMenuResult.SinglePlayer;

            this.CompleteExectiotion();
        }

        public void OnMultiplayerButtonCLicked()
        {
            Logger.Log($"MainMenuView.OnMultiplayerButtonCLicked()");

          //Unibus.Dispatch(new Events.App.StartMultiPlayer()); // Not needed
            this.result = MainMenuResult.MultiPlayer;

            this.CompleteExectiotion();
        }

        public void OnLeaderboardsButtonCLicked()
        {
            Logger.Log($"MainMenuView.OnLeaderboardsButtonCLicked()");

          //Unibus.Dispatch(new Events.App.ShowLeaderboards()); // Not needed
            this.result = MainMenuResult.Leaderboards;

            this.CompleteExectiotion();
        }
    }
}