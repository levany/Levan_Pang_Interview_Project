using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Gameplay
{
    public class HarpoonOwner : MonoBehaviour
    {
        [Header("Setup")]
        public string    PlayerName;
        public Color     Color;
        public Transform SpwanPoint;
        public Transform ParticlesSpwanPoint;

        [Header("Particles")]
        public GameObject HarpoonParticles;

        [NonSerialized] public bool      IsHarpoonAvailable;
        [NonSerialized] public Harpoon   Harpoon;


        public GameObject SpwanHarpoonParticles(Vector3 targetPosition)
        {
            Logger.Log($"SpwanHarpoonParticles: target={targetPosition}");

            var ParticlesGO                = Instantiate(HarpoonParticles, this.transform.parent);
            ParticlesGO.transform.position = targetPosition;
            var particles                  = ParticlesGO.GetComponent<ParticleSystem>();
            
            // set color
            var gradient                   = particles.main.startColor.gradientMax;
            var colorKey                   = gradient.colorKeys;
            colorKey[0].color              = this.Color;
            colorKey[1].color              = Color.white;

            // apply color
            gradient.SetKeys(colorKey, gradient.alphaKeys);
            var main                       = particles.main;
            main.startColor                = gradient;

            particles.Play();

            return ParticlesGO;
        }

    }
}
