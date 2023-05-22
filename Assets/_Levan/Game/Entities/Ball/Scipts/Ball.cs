using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Events.Game.Ball;
using LevanPangInterview.Gameplay.Controllers;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Gameplay
{
    public class Ball : MonoBehaviour
    {
        ////////// Members

        [Header("Ball Object")]
		public	int		    BallSize;
		public	Vector2	    StartForce;
        
        [Header("Points")]
        public int          ScorePoints;

        // Privae
		private Rigidbody2D rigidBody;
        private BallManager BallManager;

        ////////// Lifecycle

        private void Awake()
        {
            this.rigidBody   = GetComponent<Rigidbody2D>();
            this.BallManager = Link.GetController<BallManager>();
        }

        private void OnEnable()
        {
            BallManager.BallEnabled(this);
        }
        private void OnDisable()
        {
            BallManager.BallDisabled(this);
        }

        void Start() 
		{
			rigidBody.AddForce(StartForce, ForceMode2D.Impulse);
		}

        public void OnTriggerEnter2D(Collider2D otherCollider)
        {
            // Did we collider with an oject that is marked as "Impact?"
            var rigidBody = otherCollider.GetComponentInParent<Rigidbody2D>();
            if (rigidBody.GetComponent<Harpoon>() is Harpoon harpoon) 
            {
                Logger.Log($"Ball enter trigger with : {otherCollider.attachedRigidbody.gameObject.name}");

                Unibus.Dispatch(new BallGotHit() { Ball = this, Collider = otherCollider });
            }
        }

        public void OnRecycle()
        {
            rigidBody.AddForce(StartForce, ForceMode2D.Impulse);
        }

    }
}

namespace LevanPangInterview.Events.Game.Ball
{
    using LevanPangInterview.Gameplay;
    
    public struct BallGotHit
    {
        public Ball       Ball;
        public Collider2D Collider;
    }
}