using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using UnibusEvent;
using UnityEngine;
using UnityEngine.Pool;

namespace LevanPangInterview.Gameplay.Controllers
{
    public class BallManager : Controller
    {
        //////////////////////////////// Members

        public GameObject[] BallPrefabs;
        
        [Header("Audio effect")]
        public  AudioClip   PopSound;
        public  AudioSource AudioSource;
        public  float       PitchTimeoutSeconds = 5;
        public  float       PitchRaisAmount     = 1;
        
        private float       lastPopTime;
        private float       originalPitch;

        public List<Ball>   ActiveBalls;

        //List<ObjectPool<Ball>> BallPools;
        
        //////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            this.ActiveBalls = new List<Ball>();

            // Initializing ObjectPool(s) for all ball sizes
            //this.BallPools = new List<ObjectPool<Ball>>();

            //foreach (var ballPrefab in BallPrefabs)
            //{
            //    BallPools.Add(new ObjectPool<Ball>
            //    (
            //        createFunc      : () => Instantiate(ballPrefab).GetComponent<Ball>()
            //       ,actionOnGet     : b  => { b.gameObject.SetActive(true); b.OnRecycle(); }
            //       ,actionOnRelease : b  => b.gameObject.SetActive(false)                          
            //    ));
            //}
        }

        internal void Setup()
        {
            // Setup object pool
            this.AudioSource.clip = this.PopSound;
            originalPitch         = AudioSource.pitch;
            lastPopTime           = Time.realtimeSinceStartup;
        }
        
        public void CleanUp()
        {
            Logger.Log($"Cleanup - {ActiveBalls.Count} Active Balls remaining.");

            //for (int i = ActiveBallObjects.Count - 1; i >= 0; i--)
            //{
            //    var ball = ActiveBallObjects[i];
            //    this.BallPools[ball.BallSize-1].Release(ball); 
            //}

            for (int i = ActiveBalls.Count - 1; i >= 0; i--)
            {
                var ball = ActiveBalls[i];
                Destroy(ball.gameObject);
            }

            ActiveBalls.Clear();

            Logger.Log($"Ball CleanUp done. count = {ActiveBalls.Count} balls left");
        }

        //////////////////////////////// API

        public void SplitBall(Ball ball)
        {
            Logger.Log($"BallManager.SplitBall : {ball.gameObject.name}");

            PlayPopSound();
            Logger.Log($"Balllllll : {ball.gameObject.name}");
            Debug.Log(ball.gameObject, ball.gameObject);

            if (ball.BallSize > 1)
            {
                // Definitions                          
                var ballPosition                        = (Vector2)ball.transform.position;
                                                        
                // Get next ball prefab                 
                var ballPrefabIndex                     = ball.BallSize - 1;
                var nextBallPrefabIndex                 = (ballPrefabIndex - 1);
                var nextBallPrefab                      = BallPrefabs[nextBallPrefabIndex];
                                                        
                // Right ball                           
                //var rightBall                         = BallPools[nextBallPrefabIndex].Get(); // from object pool
                var rightBall                           = Instantiate(this.BallPrefabs[nextBallPrefabIndex]).GetComponent<Ball>();
                rightBall.gameObject.transform.position = ballPosition + (Vector2.right / 4f);
                rightBall.gameObject.transform.rotation = Quaternion.identity;
                                                        
                // Left Ball                            
                //var leftBall                            = BallPools[nextBallPrefabIndex].Get(); // from object pool
                var leftBall                            = Instantiate(this.BallPrefabs[nextBallPrefabIndex]).GetComponent<Ball>();
                leftBall.gameObject.transform.position  = ballPosition - (Vector2.right / 4f);
                leftBall.gameObject.transform.rotation  = Quaternion.identity;
                                                        
                // Set new balls Speed                  
                rightBall.StartForce                    = new Vector2(2f,  5f);
                leftBall .StartForce                    = new Vector2(-2f, 5f);
            }

            // Destroy this ball
            DestroyBall(ball);
        }

        public void DestroyBall(Ball ball)
        {
            Destroy(ball.gameObject);
            //this.BallPools[ball.BallSize-1].Release(ball);
        }
    
        public void BallEnabled(Ball ball)
        {
            this.ActiveBalls.Add(ball);

            Logger.Log($"Ball object count = {ActiveBalls.Count}");
        }
        public void BallDisabled(Ball ball)
        {
            if (!ActiveBalls.Contains(ball))
                return;

            this.ActiveBalls.Remove(ball);

            Logger.Log($"Ball object count = {ActiveBalls.Count}");

            if (ActiveBalls.Count == 0)
                Unibus.Dispatch(new Events.Game.Ball.LastBallKilled());
        }

        //////////////////////////////// HelperMethods
        
        public void PlayPopSound()
        {
            if (Time.realtimeSinceStartup - lastPopTime < this.PitchTimeoutSeconds)
            {
                AudioSource.pitch += PitchRaisAmount;
            }
            else
            {
                AudioSource.pitch = originalPitch;
            }

            this.AudioSource.Play();

            lastPopTime = Time.realtimeSinceStartup;
        }
    }
}


namespace LevanPangInterview.Events.Game.Ball
{
    public struct LastBallKilled {}
}
