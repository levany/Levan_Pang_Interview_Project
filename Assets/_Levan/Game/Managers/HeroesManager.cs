using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using LevanPangInterview.Events.Game.Hero;
using UnibusEvent;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering;

namespace LevanPangInterview.Gameplay.Controllers
{
    public class HeroesManager : Controller
    {
        //////////////////////////////// Members

        [Header("References")]
        public StageController         Stage;
                                       
        [Header("Scene")]              
        public GameObject              HeroPrefab;
        public List<Transform>         SpwanPoints;
                                       
        [Header("Audio")]              
        public AudioClip               SquashSound;
        public AudioSource             AudioSource;
                                       
        [NonSerialized]                
        public List<Hero>              ActiveHeroes = new();

        // private ObjectPool<GameObject> ObjectPool;

        //////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            this.Stage = Link.GetController<StageController>();

            //this.ObjectPool = new ObjectPool<GameObject>
            //(
            //    createFunc      : ()   => { return Instantiate(original: HeroPrefab, parent: Stage.transform); }
            //   , actionOnGet    : (go) => { go.SetActive(true); }
            //   , actionOnRelease: (go) => { go.gameObject.SetActive(false); gameObject.GetComponent<Hero>().CleanUp(); }
            //);
        }

        public void Setup()
        {
            if (this.ActiveHeroes.Count > 0)
                Logger.LogError("There should be no objects in the ActiveHeroes list during HeroManagers' setup ");
        }

        public void Cleanup()
        {
            this.AudioSource.clip = null;

            // Dispose of  all active hero objcts
            for (int i = this.ActiveHeroes.Count - 1; i >= 0; i--)
            {
                var hero = this.ActiveHeroes[i];
                DestroyHero(hero);
            }

            ActiveHeroes.Clear();

            Logger.Log($"Hero ClleanUp done");
        }

        //////////////////////////////// API

        public void SpwanHeroePrefabs()
        {
            Logger.Log("PopulateHeroes");
            try
            {
                var gameplay  = Link.GetData<Models.GameplayModel>();
                var heroes    = Link.GetController<HeroesManager>();
                var Players   = Link.GetController<PlayersController>();

                Logger.Log($"current playerCount is   : {gameplay.PlayerCount}");
                Logger.Log($"Players Count is         : {Players.PlayersCollection.Count}");

                for (int i = 0; i < gameplay.PlayerCount; i++)
                {
                    var player = Players.PlayersCollection[i];

                    // Spwan the hero
                    var hero = heroes.SpwanHero(i+1, player);

                    // object will automaticly be added to the ActiveHeros list
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }
        }


        public Hero SpwanHero(int playerNumber, Models.Player player)
        {
            Logger.Log($"HeroManager.SpwanHero: P#={playerNumber} - P={player.Name}");

            /// Using object pool instend of Instantiating and Destroying objects

            var heroGO                = InstantiateHeroGameObject();
            var hero                  = heroGO.GetComponent<Hero>();

            heroGO.name               = $"Hero_{playerNumber}";
            heroGO.transform.position = this.SpwanPoints[playerNumber-1].position;
            hero.PlayerName           = player.Name;
            hero.PlayerColor          = player.Color;
            
            hero.RightInput           = player.RightInput;
            hero.LeftInput            = player.LeftInput;
            hero.FireInput            = player.FireInput;

            hero.SetUp();

            return hero;
        }

        public void DestroyHero(Hero hero)
        {
            Logger.Log($"GO={hero.gameObject.name} - P={hero.PlayerName}");
            
            // Spwan thouse beautiful babies !
            SpwanSuqshParticles(hero);
            
            // Play Squash sound
            AudioSource.clip = this.SquashSound;
            AudioSource.Play();

            // Destroy hero object
            DestroyHeroObject(hero);
        }

        public void OnHeroEnabled(Hero hero)
        {
            // Register a newly active hero object

            this.ActiveHeroes.Add(hero);

            Logger.Log($"Ball object cound = {ActiveHeroes.Count}");
        }
        public void OnHeroDisabled(Hero hero)
        {
            // Unregister a newly Disabled hero object

            this.ActiveHeroes.Remove(hero);

            Logger.Log($"Ball object cound = {ActiveHeroes.Count}");

            // inform that the last hero object was just disabled 
            if (this.ActiveHeroes.Count == 0)
                Unibus.Dispatch(new LastHeroKilled());
        }

        //////////////////////////////// ObjectPool
        
        private GameObject InstantiateHeroGameObject()
        {
            // return this.ObjectPool.Get().GetComponent<Hero>(); // BUG
            return Instantiate(original: HeroPrefab, parent: Stage.transform);
        }

        private void DestroyHeroObject(Hero hero)
        {
            // this.ObjectPool.Release(hero.gameObject); // BUG
            Destroy(hero.gameObject);
        }

        //////////////////////////////// Herlper methods
        
        private void SpwanSuqshParticles(Hero hero)
        {
            Logger.Log($"HeroManager.KillHero: GO={hero.gameObject.name} - P={hero.PlayerName}");

            // Spwan particles
            var ParticlesGO                = Instantiate(original: hero.SquashParticles, parent : this.Stage.transform);
            ParticlesGO.transform.position = hero.transform.position;
            var particles                  = ParticlesGO.GetComponent<ParticleSystem>();
            
            // set color
            var gradient                   = particles.main.startColor.gradientMax;
            var colorKey                   = gradient.colorKeys;
            colorKey[0].color              = hero.PlayerColor;
            colorKey[1].color              = Color.white;

            // apply color
            gradient.SetKeys(colorKey, gradient.alphaKeys);
            var main                       = particles.main;
            main.startColor                = gradient;

            particles.Play();
        }
    }
}

namespace LevanPangInterview.Events.Game.Hero
{
    public struct LastHeroKilled {}
}
