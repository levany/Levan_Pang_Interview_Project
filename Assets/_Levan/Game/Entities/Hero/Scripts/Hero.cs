using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Events.Game.Hero;
using LevanPangInterview.Gameplay.Controllers;
using UnibusEvent;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevanPangInterview.Gameplay
{
	public class Hero : MonoBehaviour
	{
		//////////////////////////////// Members

		[Header("General")]
		public  float							speed = 4f;

		[Header("References")]
		public					 Transform		BottomPoint;
		public					 Transform		TopPoint;
		public					 GameObject		SquashParticles;
		[SerializeField] private Rigidbody2D	rigidbody;
		[SerializeField] private SpriteRenderer CapsuleSpriteRenderer;
		public					 AudioSource    AudioSource;


		[Header("PlayerInfo")]
		public  string							PlayerName;
		public  Color							PlayerColor;

		[Header("Input")]
		public InputAction						RightInput;
		public InputAction						LeftInput;
		public InputAction						FireInput;

		// General
		private float							velocity		 = 0f;
		public  float							MoveRotation	 = 10;

		private HeroesManager					heroesManager;

        //////////////////////////////// Initialization

        public void Awake()
        {
            this.heroesManager = Link.GetController<HeroesManager>();
        }

        public void SetUp()
        {
            CapsuleSpriteRenderer.color = PlayerColor;
			RightInput.Enable();
			LeftInput.Enable();
			FireInput.Enable();
        }


        //////////////////////////////// Lifecycle

        private void OnEnable()
        {
            heroesManager.OnHeroEnabled(this);
        }
        private void OnDisable()
        {
            heroesManager.OnHeroDisabled(this);
        }

        private void FixedUpdate()
		{			
			// set direction
			int direction = 0;
			if (RightInput.IsPressed()) direction =  1;
			if (LeftInput.IsPressed())  direction = -1;

			// move
			velocity = direction * speed;
			rigidbody.MovePosition(rigidbody.position + new Vector2 (velocity * Time.fixedDeltaTime, 0f));

			// Rotate a little -  ites cute
			transform.rotation = Quaternion.Euler(0,0, -direction * this.MoveRotation);
		}

		public void Update ()
		{
			if (FireInput.WasPerformedThisFrame())
			{
				Logger.Log($"Hero {this.gameObject.name} Pressed Fire!");

				Unibus.Dispatch(new HeroFireEvent() { Hero = this }); // Handled by PangMechanics class
			}
		}


        //////////////////////////////// Physics Events
		

        public void OnCollisionEnter2D(Collision2D Collision)
        {
			/// Make sure we hit an objet that is marked with "impact" component
			/// 
			/// for this small project there is only 1 type of impact objects - the fruits.
			/// but we want to make this generic and extendible.
			/// 
			/// that means the hero shouldnt know about fruits, or any specific types.
			/// so we use a generic utility "tag" component = "Impact"
			
			// only handle collider objects that are relevent to us ( = marked with "Impact" component)
			if (Collision.collider.attachedRigidbody != null 
            &&  Collision.collider.attachedRigidbody.transform.GetComponent<Impact>() is Impact impact)
			{
				// we keep the "Impact" component where the rigidbody is.
				// because the collider itself may be deeper in the prefab and not on always on the prefab root.

				Logger.Log($"Hero Got Hit");

				Unibus.Dispatch(new HeroGotHit() { Hero = this, Collider = Collision.collider });
			}
        }

		/// <summary>
		/// Cleans this object up 
		/// so it can be stored and retrived in an object pool 
		/// with out cuasing problems
		/// </summary>
        public void CleanUp()
        {
			// nothing else to clean up
			// if this object will be spwaned again,
			// everything will be overriden
        }
    } 
}


namespace LevanPangInterview.Events.Game.Hero
{
    using LevanPangInterview.Gameplay;
    
    public struct HeroGotHit
    {
        public Hero       Hero;
        public Collider2D Collider;
    }

    public struct HeroFireEvent
    {
        public Hero Hero;
    }
}