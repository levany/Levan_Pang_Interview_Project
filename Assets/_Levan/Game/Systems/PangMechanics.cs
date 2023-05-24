using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Controllers;
using LevanPangInterview.Gameplay;
using LevanPangInterview.Gameplay.Controllers;
using LevanPangInterview.Views;
using UnibusEvent;
using UnityEngine;
using LevanPangInterview.Events.Game.Hero;
using LevanPangInterview.Events.Game.Harpoon;
using LevanPangInterview.Events.Game.Ball;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LevanPangInterview.Events.Game;

namespace LevanPangInterview.Gameplay.Controllers
{
    public class PangMechanics : Controller
    {
        //////////////////////////////////////// Members
        
        [Header("Managers")]
        private HeroesManager     HeroManager;
        private HarpoonManager    HarpoonManager;
        private BallManager       BallManager;
        
        [Header("General")]
        public StageController    Stage;

        private int               delaySaftyCheck = 0;

        //////////////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            // Caching references
            this.HeroManager      = Link.GetController<Gameplay.Controllers.HeroesManager>(); 
            this.HarpoonManager   = Link.GetController<Gameplay.Controllers.HarpoonManager>();
            this.BallManager      = Link.GetController<Gameplay.Controllers.BallManager>();   

            Logger.Log($"Subscribing to game events");
            this.BindUntilDestroy<BallGotHit>    (OnBallGotHit);
            this.BindUntilDestroy<HeroGotHit>    (OnHeroGotHit);
            this.BindUntilDestroy<HeroFireEvent> (OnHeroFire);
            this.BindUntilDestroy<HarpoonHitWall>(OnHarpoonHitWall);
            this.BindUntilDestroy<LastBallKilled>(OnLastBallKilled);
            this.BindUntilDestroy<LastHeroKilled>(OnLastHeroKilled);
        }

        public void Setup()
        {
            Logger.LogSeperator();
            Logger.Log($"Setup");

            this.delaySaftyCheck++;

            //// Enabling Stage

            this.Stage.Enable();

            //// Setup Controllers and managers
            
            Logger.Log($"Setting up managers");
            this.Stage.Setup();
            this.HeroManager.Setup();
            this.HarpoonManager.Setup();
            this.BallManager.Setup();

            //// Hero Manager

            Logger.Log($"SettingUp Hero Prefabs");

            this.HeroManager.SpwanHeroePrefabs();

            //// Harpoon Manager

            Logger.Log($"Setting Harpoon Owners");

            foreach (var hero in HeroManager.ActiveHeroes)
            {
                HarpoonManager.SetupHarpoonOwner(hero.gameObject
                                                ,hero.PlayerName
                                                ,hero.PlayerColor
                                                ,hero.BottomPoint
                                                ,hero.TopPoint);
            }
        }
        
        public void CleanUp()
        {
            Logger.LogSeperator();
            Logger.Log($"CleanUp");

            this.delaySaftyCheck++;

            this.BallManager.CleanUp(); 
            this.HeroManager.Cleanup();
            this.HarpoonManager.CleanUp();
            this.Stage.CleanUP();
            this.Stage.Disable();
        }
        
        //////////////////////////////////////// Events

        public void OnHeroGotHit(HeroGotHit @event)
        {
            if (Link.AppSettings.CHEAT_HERO_CANT_DIE)
                return;

            // Check if what hit the hero is a Ball
            if (@event.Collider.attachedRigidbody != null
            &&  @event.Collider.attachedRigidbody.transform.GetComponent<Ball>() is Ball ball)
			{
				Logger.Log($"PangMechanics.OnHeroGotHit.Ball");
                
                /// (in this small project - its the only thing that CAN hit the hero,
                ///  but we are acting as if its a big project that may have other types of colling objects)

                HeroManager.DestroyHero(@event.Hero);
                BallManager.SplitBall(ball);
			}
        }

        public void OnBallGotHit(BallGotHit @event)
        {
            BallManager.SplitBall(@event.Ball);

            if (@event.Collider.attachedRigidbody != null
            &&  @event.Collider.attachedRigidbody.gameObject.GetComponent<Harpoon>() is Harpoon harpoon)
            {
                AddScoreToPlayer(harpoon.Owner.PlayerName, @event.Ball.ScorePoints);

                this.HarpoonManager.DestroyHarpoon(harpoon);
            }
        }

        public void OnHeroFire(HeroFireEvent @event)
        {
            HarpoonManager.SpwanHarpoon(@event.Hero.GetComponent<HarpoonOwner>());
        }
        
        private void OnHarpoonHitWall(HarpoonHitWall @event)
        {
            HarpoonManager.DestroyHarpoon(@event.Harpoon);
        }
        
        private void OnLastHeroKilled(LastHeroKilled action)
        {
            Logger.Log("Level Lost!");
            LoseLevel();
        }
        
        private void OnLastBallKilled(LastBallKilled action)
        {
            Logger.Log("Last Ball Killed !");
            CheckForWin();
        }
        
        //////////////////////////////////////// Methods

        private async Task CheckForWin()
        {
            Logger.Log("Waiting to check for win");

            bool isSafe = await this.SmallDelay();

            if (!isSafe) 
                return;

            Logger.Log("Checking for win");

            // might be a ball that pops late ?
            // might be that the hero died with the last ball...
            if (this.BallManager.ActiveBalls.Count  >  0
            ||  this.HeroManager.ActiveHeroes.Count == 0)
            {
                Logger.Log($"No Win balls:{BallManager.ActiveBalls.Count}. heroes:{this.HeroManager.ActiveHeroes.Count}");
                return; 
            }

            Logger.Log("Level Won!");

            // If all is legit - Win the level !
            Unibus.Dispatch(new LevelWon());            
        }

        private async Task LoseLevel()
        {            
            bool isSafe = await this.SmallDelay();

            if (!isSafe) 
                return;

            Logger.Log("Checking for Lose");

            // If all is legit - Win the level !
            Unibus.Dispatch(new LevelLost());
        }

        private void AddScoreToPlayer(string playerName, int score)
        {
            Link.GetController<ScoreController>().AddScoreToPlayer(playerName, score);
        }

        /// <summary>
        /// Delays for a small amount of time 
        /// returns TRUE if safty flag is ok,
        /// returns FALSE if safty flag is wrong 
        ///  - witch means that we restarted the level while waiting
        /// </summary>
        public async Task<bool> SmallDelay()
        {
            var SaftyValueBeforDelay = this.delaySaftyCheck;
            
            await Task.Delay(1000);
            
            /// If this is false
            /// The level must have restarted manually.
            /// We are no longer in the same context !
            /// 
            bool isSafe =  this.delaySaftyCheck == SaftyValueBeforDelay;

            return isSafe;
        }

    } 
}

namespace LevanPangInterview.Events.Game
{
    public struct LevelWon  {}
    public struct LevelLost {}
}
