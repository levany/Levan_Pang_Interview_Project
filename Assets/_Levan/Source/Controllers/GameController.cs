using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Gameplay.Controllers;
using LevanPangInterview.Views;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Controllers
{
    public enum GameResult
    {
        None,
        Esc,
        Lose,
        Win
    }

    public class GameController : Controller
    {
        //////////////////////////////////////////////////////////////// Members

        // references
        
        private LevelController   LevelController;
        private PangMechanics     Mechanics;
        private PlayersController PlayerController;
        private ScoreController   ScoreController;

        private ScoreHUDView      ScoreHUDView;
        private InputHUDView      InputHUDView;

        //////////////////////////////////////////////////////////////// Properties

        public GameResult Result = GameResult.None;

        //////////////////////////////////////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            this.LevelController   = Link.GetController<Gameplay.Controllers.LevelController>();
            this.Mechanics         = Link.GetController<PangMechanics>();
            this.PlayerController  = Link.GetController<PlayersController>();
            this.ScoreController   = Link.GetController<ScoreController>();
            this.ScoreHUDView      = Link.GetView<ScoreHUDView>();
            this.InputHUDView      = Link.GetView<InputHUDView>();
        }

        //////////////////////////////////////////////////////////////// Main Flow

        [ContextMenu("Execute")]
        public async Task Execute()
        {
            Logger.LogSeperator();
            Logger.Log("GameController.Execute()");

            Setup();
            await this.RunLevels();
            this.Finish();
            CleaUp();

            Logger.Log("GameController.Execute().Finished");
        }

        //////////////////////////////////////////////////////////////// Main Flow Steps

        public void Setup()
        {
            Logger.Log($"Setup");

            // Player and score
            PlayerController.Setup();
            ScoreController.Setup();

            // Setup pang mechanics
            this.Mechanics.Setup();

            //// Setup hud Huds
            this.ScoreHUDView.Show();
            this.ScoreHUDView.Setup();

            //// Setup Input Hud 
            this.InputHUDView.Show();
            this.InputHUDView.Setup();
        }

        public async Task RunLevels()
        {
            Logger.Log($"RunLevels");

            var GameplayData = Link.GetData<Models.GameplayModel>();
            
            foreach (var level in GameplayData.Levels)
            {
                // Run the level

                await RunLevel(level);

                // Check Level Result

                if (LevelController.LevelResult == LevelResult.Esc)
                {
                    Logger.Log($"LevelResult - ESC - returning");
                    this.Result = GameResult.Esc;
                    return;
                }
                else if (LevelController.LevelResult == LevelResult.Lose)
                {
                    Logger.Log($"LevelResult - Lose - returning");
                    this.Result = GameResult.Lose;
                    return;
                }
                else if (Result == GameResult.Win)
                {
                    Logger.Log($"LevelResult - Win - Continuing");
                    this.Result = GameResult.Win;
                    // Go To Next Level
                    continue;
                }
            }
        }

        public void Finish()
        {
            Logger.Log($"Finish");
          //Unibus.Dispatch(new Events.App.GameplayFinished()); // No longer used
        }

        private void CleaUp()
        {
            Logger.Log($"Doing The oposite of Setup()");

            // Cleanup pang mechanics
            this.Mechanics.CleanUp();

            //// Setup hud Huds
            this.ScoreHUDView.CleanUp();
            this.ScoreHUDView.Hide();

            //// Cleanup Input Hud - Only on android
            this.InputHUDView.CleanUp();
            this.InputHUDView.Hide();
        }

        //////////////////////////////////////////////////////////////// Methods

        public async Task RunLevel(Models.Level level)
        {
            Logger.Log($"level = {level}, name={level.name}");

            var LevelController = Link.GetController<LevelController>();

            LevelController.Level = level;
            await LevelController.Execute();

            Logger.Log($"Finished Running Level {level.LevelName} with result {this.LevelController.LevelResult}");
        }
    }
}
