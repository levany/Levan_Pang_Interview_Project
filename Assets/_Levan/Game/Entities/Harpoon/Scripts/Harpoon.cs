using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Events.Game.Harpoon;
using LevanPangInterview.Gameplay.Controllers;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Gameplay
{
    public class Harpoon : MonoBehaviour
    {
        //////////////// Members

        [Header("References")]
        [SerializeField] private GameObject     HarpoonBody;
        [SerializeField] private SpriteRenderer HarpoonRenderer;
                         private HarpoonManager HarpoonManager;

        [Header("Values")]
		public float          Speed = 2f;

        // private
        private float          initialHeight;
        private Vector3        initialPosition;
        private Vector3        initialScale;

        //////////////// Properties
        
        public HarpoonOwner   Owner       { get; set; }
        public Color          PlayerColor { get; set; }

        //////////////// Initialization

        public void Awake()
        {
            this.HarpoonManager = Link.GetController<HarpoonManager>();
            this.initialScale   = transform.localScale;
        }

        public void Setup()
        {
            BoxCollider2D box          = HarpoonBody.GetComponent<BoxCollider2D>();
            this.initialHeight         = box.size.y;
            this.initialPosition       = transform.position;
            this.HarpoonRenderer.color = PlayerColor;

            Logger.Log($"harpoonBody.LocalScale : {this.HarpoonBody.transform.localScale}");
        }

        public void Recycle()
        {
            HarpoonBody.transform.localScale = initialScale;
            HarpoonBody.transform.position   = initialPosition;
            Owner.Harpoon                    = null;
        }

        //////////////// Lifecycle

        private void OnEnable()
        {
            HarpoonManager.OnHarpoonEnabled(this);
        }

        private void OnDisable()
        {
            HarpoonManager.OnHarpoonDisabled(this);
        }

        void Update() 
		{
            // Grow the inner object of this gameobject 

			HarpoonBody.transform.localScale += Vector3.up * Time.deltaTime * Speed;
            HarpoonBody.transform.position    = initialPosition + new Vector3(0, (initialHeight / 2) * HarpoonBody.transform.localScale.y, 0) ;
		}

        //////////////// Events

        void OnTriggerEnter2D(Collider2D collider)
		{
            Logger.Log("Harpoon.TriggerEnter");

            if (collider.GetComponent<Impact>() is Impact impact)
            {
                Logger.Log($"harpoon colided with an 'Impact' GameObject : {impact.gameObject.name}!");
            }

            if (collider.GetComponent<Wall>() is Wall wall)
            {
                Logger.Log($"harpoon colided with an 'Wall' GameObject : {wall.gameObject.name}!");
                Unibus.Dispatch(new HarpoonHitWall() { Harpoon = this, wall = wall });
            }
		}
    }

}

namespace LevanPangInterview.Events.Game.Harpoon
{
    using LevanPangInterview.Gameplay;

    public struct HarpoonHitWall
    {
        public Harpoon Harpoon;
        public Wall    wall;
    }
}

