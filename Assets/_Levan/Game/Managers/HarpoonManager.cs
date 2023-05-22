using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEngine.UI.GridLayoutGroup;

namespace LevanPangInterview.Gameplay.Controllers
{
    public class HarpoonManager : Controller
    {
        //////////////////////////////// Members

        public GameObject           HarpoonPrefab;
        public GameObject           HarpoonParticlesPrefab;

        public AudioClip            HarpoonSound;
        public AudioSource          AudioSource;

        public List<Harpoon>        ActiveHarpoons;

      //private ObjectPool<Harpoon> HarpoonPool;

        //////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            ActiveHarpoons = new List<Harpoon>();

            //this.HarpoonPool = new ObjectPool<Harpoon>
            //(
            //    createFunc      : ()  => Instantiate(HarpoonPrefab).GetComponent<Harpoon>()
            //   ,actionOnGet     : (h) => h.gameObject.SetActive(true)
            //   ,actionOnRelease : (h) => { h.Recycle(); h.gameObject.SetActive(false); }
            //);
        }

        public void Setup()
        {
            Logger.Log("Setup");
        }

        internal void CleanUp()
        {
            Logger.Log($"Hero ClleanUp done");

            // Dispose of  all active hero objcts
            for (int i = this.ActiveHarpoons.Count - 1; i >= 0; i--)
            {
                var harpoon = this.ActiveHarpoons[i];
              
                DestroyHarpoon( harpoon );
            }

            ActiveHarpoons.Clear();

        }

        //////////////////////////////// API

        public void SetupHarpoonOwner(GameObject owner, string PlayerName, Color color, Transform SpwanPoint, Transform ParticlesSpwanPoint)
        {
            Logger.Log($"HarpoonManager.SetupHarpoonOwner : {owner.gameObject.name}, {PlayerName}, {color}, {transform.position}");

            var hOwner                  = owner.AddComponent<HarpoonOwner>();
            hOwner.PlayerName           = PlayerName;
            hOwner.Color                = color;
            hOwner.SpwanPoint           = SpwanPoint;
            hOwner.ParticlesSpwanPoint  = SpwanPoint;
            hOwner.HarpoonParticles     = HarpoonParticlesPrefab;

            hOwner.IsHarpoonAvailable   = true;
        }

        public void SpwanHarpoon(HarpoonOwner owner)
        {
            Logger.Log($"HarpoonManager.SpwanHarpoon: {owner.gameObject.name}");

            if (!owner.IsHarpoonAvailable)
            {
                Logger.Log($"Harpoon not available for {owner.gameObject.name}!");
                return;
            }

            // Instantiate

            //var harpoon                    = this.HarpoonPool.Get();
            //var harpoonGO                  = harpoon.gameObject;
            var harpoonGO                  = Instantiate(HarpoonPrefab);
            var harpoon                    = harpoonGO.GetComponent<Harpoon>();

            harpoonGO.transform.parent     = owner.transform.parent;

            // Setup
            harpoon.Owner                  = owner;
            harpoonGO.transform.position   = owner.SpwanPoint.position;
            owner.IsHarpoonAvailable       = false;
            harpoon.PlayerColor            = owner.Color;

            SpwanHarpoonParticles(owner);

            harpoon.Setup();

            // Play Sound
            AudioSource.clip = HarpoonSound;
            AudioSource.Play();

        }

        public void DestroyHarpoon(Harpoon harpoon)
        {
            Logger.Log($"HarpoonManager.DestroyHarpoon: {harpoon.gameObject.name}");

            harpoon.Owner.IsHarpoonAvailable = true;
            
          //this.HarpoonPool.Release(harpoon);
            Destroy(harpoon.gameObject);
        }

        private void SpwanHarpoonParticles(HarpoonOwner owner)
        {
            Logger.Log($"SpwanHarpoonParticles: owner={owner.gameObject.name}");

            owner.SpwanHarpoonParticles(owner.ParticlesSpwanPoint.position);
        }

        public void OnHarpoonEnabled(Harpoon harpoon)
        {
            this.ActiveHarpoons.Add(harpoon);
        }

        public void OnHarpoonDisabled(Harpoon harpoon)
        {
            this.ActiveHarpoons.Remove(harpoon);
        }
    }
}
