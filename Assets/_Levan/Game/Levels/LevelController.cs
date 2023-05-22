using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using LevanPangInterview.Events.Game;
using LevanPangInterview.Gameplay;
using LevanPangInterview.Gameplay.Controllers;
using UnibusEvent;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace LevanPangInterview.Gameplay.Controllers
{
    public enum LevelResult
    {
        None,
        Esc,
        Win,
        Lose
    }

    public class LevelController : Controller
    {
        //////////////////////////////// Members 

        public  AudioSource      AudioSource;

        private StageController  Stage;
        
        private GameObject       LevelGO;
        public  bool             IsPlaying {get; private set;} = false;

        //////////////////////////////// Properties

        public Models.Level Level { get; set; }

        public LevelResult LevelResult {get; set; }= LevelResult.None;

        //////////////////////////////// lifecycke

        public override async Task OnSystemInit()
        {
            this.Stage = Link.GetController<Gameplay.Controllers.StageController>();   

            this.BindUntilDestroy<Events.Game.LevelLost>(OnLevelLost);
            this.BindUntilDestroy<Events.Game.LevelWon>(OnLevelWon);
        }

        //////////////////////////////// Main FLow


        public async Task Execute()
        {
            Logger.LogSeperator();
            Logger.Log($"Executing Level {Level.LevelName}");

            Setup();
            await Intro();
            await Play();
            await Outro();
            Dispose();
        }

        //////////////////////////////// Main Flow Sets

        public void Setup()
        {
            Logger.LogSeperator();
            Logger.Log("LevelController.Setup()");

            ///// Setup the Stage
            
            Logger.Log("LevelController.InitializeStage()");

            Stage.Enable();
            Stage.BackgroundColor       = Level.BackgroundColor;
            Stage.CameraBackgroundColor = Level.CameraBGColor;
            Stage.WallsColor            = Level.WallsColor;
            Stage.Title                 = Level.LevelName;
            Stage.Setup();

            
            ///// Spwan the Level GO

            Logger.Log("LevelController.CreateLevelGO()");

            this.LevelGO = new GameObject($"{Level.LevelName}_Content");
            this.LevelGO.transform.SetParent(Stage.transform, false);   

            
            ///// Populate with prefabs from Level Data
            
            Logger.Log("LevelController.PopulateLevel()");

            foreach (var prefab in Level.Prefabs)
            {
                var instance = Instantiate(original: prefab, parent: LevelGO.transform);
                instance.name = instance.name.Replace("(Clone)", "");
            }
        }

        private async Task Intro()
        {
            Logger.Log("Level Intro");

            ToggleLevelMusic(shouldTurnOn: true);
        }

        private async Task Play()
        {
            Logger.Log("Level Play");

            IsPlaying = true;

            while (IsPlaying)
            {
                await Task.Yield();
            }

            Logger.Log($"Level Ended with result {LevelResult}");
        }

        private async Task Outro()
        {
            Logger.Log($"Outro");

            ToggleLevelMusic(shouldTurnOn: false);

            #region CHEAT
            ////////////////////////////////////////// CHEAT
            /// this is a CHEAT(!) for easy debugging
            /// - enable\disable in app-settings
            /// - the app settings is referenced in the root gameobject of the scene
            if (LevelResult == LevelResult.Esc 
            &&  Link.AppSettings.CHEAT_ESC_KEY_WINS_LEVEL) 
            {
                LevelResult = LevelResult.Win;
            }
            //////////////////////////////////////////
            #endregion // CHEAT

            if ( LevelResult == LevelResult.Esc ) 
                return;

            Logger.Log("Level Outro");
        }

        public void Dispose()
        {
            Logger.Log("Level Dispose");

            Destroy(LevelGO);
            LevelGO = null;
        }

        //////////////////////////////// Lifecycle Events

        public void Update()
        {
            if (IsPlaying)
            {
                if (Keyboard.current[Link.AppSettings.EascapeKey].wasPressedThisFrame)
                {
                    Logger.Log("Escape!");
                    this.IsPlaying      = false;
                    this.LevelResult    = LevelResult.Esc;
                }
            }
        }

        //////////////////////////////// Helper Methods
        
        void ToggleLevelMusic(bool shouldTurnOn)
        {
            if (this.Level.LevelMusic == null)
                return;

            if (shouldTurnOn)
            {
                this.AudioSource.clip = this.Level.LevelMusic;
                this.AudioSource.loop = true;
                this.AudioSource.Play();
            }
            else
            {
                this.AudioSource.Stop();
            }
        }
        
        private void OnLevelWon(LevelWon action)
        {
            Logger.Log($"On Level Win");
            this.LevelResult = LevelResult.Win;
            IsPlaying = false;
        }

        private void OnLevelLost(LevelLost action)
        {
            Logger.Log($"On Level Lost");
            this.LevelResult = LevelResult.Lose;
            IsPlaying = false;
        }

    }
}
