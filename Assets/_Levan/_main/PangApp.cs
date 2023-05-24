using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LevanPangInterview.Controllers;
using LevanPangInterview.Views;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using Unity.VisualScripting;
using UnibusEvent;
using LevanPangInterview.Events.App;
using LevanPangInterview.Models;
using JetBrains.Annotations;
using LevanPangInterview.Gameplay.Controllers;
using LevanPangInterview.Services;

namespace LevanPangInterview
{
    public class PangApp : MonoBehaviour
    {
        //////////////////////////////////////////////// Members
        
        public bool IsRunning = true;
        
        private MainMenuView           mainMenuView;
        private GameController         gameController;
        private ScoreView              scoreView;
        private LeaderboardsView       leaderboardsView;
        private ScoreController        scoreController;

        //////////////////////////////////////////////// Lifecycle 

        public async Task Awake()
        {
            Logger.Log("App.Awake");

            Link.Instance.Initialize(this);
            
            // Chaching References
            this.mainMenuView           = Link.GetView<MainMenuView>();
            this.gameController         = Link.GetController<GameController>();
            this.scoreView              = Link.GetView<ScoreView>();
            this.leaderboardsView       = Link.GetView<LeaderboardsView>();
            this.scoreController        = Link.GetController<ScoreController>();

            await SetupApp();
            await SystemInit();
        }

        public async Task Start()
        {
            Logger.LogSeperator();
            Logger.Log("Start");
            
            await SetupApp();

            await StartApp();
        }

        public async Task StartApp()
        {
            Logger.LogSeperator();
            Logger.Log("StartApp");

            await Task.Yield(); // Skip frame to let objects possibly Initialize on Start()

            await Execute();
        }

        //////////////////////////////////////////////// Initialization
        
        public async Task SetupApp()
        {
            try
            {
                Logger.Log($"SetupApp");

                // Hide All Views
                
                Logger.LogSeperator();
                Logger.Log("Hiding Views");

                foreach (var view in Link.Instance.Views)
                {
                    view.Hide();                
                }

                // Disable Game Controller 
                Logger.Log("Disabling Game Controller");
                this.gameController.Disable();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }

        public async Task SystemInit()
        {
            try
            {                
                Logger.LogSeperator();
                Logger.Log($"SystemInit");

                // Initialize Services

                Logger.LogSeperator();
                Logger.Log($"initializing Services");
                await Link.Storage.OnSystemInit();
                await Link.SimpleCloudService.OnSystemInit();
                await Link.Data.OnSystemInit();
                
                // Initialize Controllers

                Logger.LogSeperator();
                Logger.Log($"Initializing Controllers");

                foreach (var controller in Link.Instance.Controllers)
                {
                    await controller.OnSystemInit();
                }
                
                // Initialize Views
                Logger.LogSeperator();
                Logger.Log($"Initializing Views");

                foreach (var view in Link.Instance.Views)
                {
                    await view.OnSystemInit();                
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }


        //////////////////////////////////////////////// Actions

        public async Task Execute()
        {
            while (IsRunning)
            {
                await RunMainMenu();
            }
        }


        public async Task RunMainMenu()
        {
            var   mainMenu = Link.GetView<MainMenuView>();
            await mainMenu.Execute();

            var mainMenuResult = mainMenu.result;

            switch (mainMenuResult)
            {
                case MainMenuResult.SinglePlayer : await RunGmae(playerCount:1); break;
                case MainMenuResult.MultiPlayer  : await RunGmae(playerCount:2); break;
                case MainMenuResult.Leaderboards : await RunLeaderboards();      break;
                default: break;
            }            
        }

        public async Task RunGmae(int playerCount)
        {
            Logger.LogSeperator();
            Logger.Log($"RunGmae() player count = {playerCount}");
            
            // Setup
            var      gameData       = Link.GetData<Models.GameplayModel>();
            gameData.PlayerCount    = playerCount;

            var game = Link.GetController<GameController>();
            
            // Run Game

            game.Enable();

            Logger.Log("Gameplay Started");
            
            await game.Execute();
            
            Logger.Log("Gameplay Finished");

            game.Disable();


            // Check Result
            var result = game.Result;

            if (result ==  GameResult.Esc)
            {
                return; // To main menu
            }
            else
            {
                await RunGameOverFlow();
                await RunLeaderboards();
            }
        }

        public async Task RunGameOverFlow()
        {   
            Logger.LogSeperator();
            Logger.Log("RunGameOverFlow");

            await scoreController.RunScoreFlow();
        }

        public async Task RunLeaderboards()
        {
            Logger.LogSeperator();
            Logger.Log("RunLeaderboards");

            await this.leaderboardsView.Execute();
        }

    }
}
