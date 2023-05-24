using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevanPangInterview.Models;
using LevanPangInterview.Views;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Controllers
{
    public class ScoreController : Controller
    {
        // Members

        private ScoreView              scoreView;
        private LeaderboardsController leaderboards;

        // Lifecycle

        public override async Task OnSystemInit()
        {
            this.scoreView    = Link.GetView<ScoreView>();
            this.leaderboards = Link.GetController<LeaderboardsController>();
        }

        public void Setup()
        {
            ResetScores();
        }

        // API

        public async Task RunScoreFlow()
        {
            Logger.Log("Running Score Flow");

            var players = Link.GetData<PlayersCollection>();

            Logger.Log($"player count : {players.Count}");

            foreach (var player in players)
            {
                var isNewRecord = this.leaderboards.IsNewRecord(player.Score);
                
                // Show score view
                await this.scoreView.ExecuteScoreViewForPlayer(player, isNewRecord);

                if (isNewRecord)
                {
                    var playerNameForLeaderboards = scoreView.ResultPlayerName;

                    if (playerNameForLeaderboards == null)
                        continue;

                    Unibus.Dispatch(new Events.Game.Score.NewRecord() { PlayerName = playerNameForLeaderboards, Score = player.Score } );
                }
            }
        }

        /// <summary>
        /// resets the scores for a new game
        /// </summary>
        public void ResetScores()
        {
            Logger.Log($"Reseting Player Score");
            
            var playersCollection = Link.GetData<PlayersCollection>();

            foreach (var player in playersCollection)
            {
                player.Score = 0;
            }
        }


        // Methods

        /// <summary>
        /// Updates score and returns the updated score for the given player
        /// </summary>
        public int AddScoreToPlayer(string playerName, int Score)
        {
            Logger.Log($"PlayerController.AddScoreToPlayer()");

            var playersCollection = Link.GetData<PlayersCollection>();

            var player      = playersCollection.First(p => p.Name == playerName);
            var playerIndex = playersCollection.IndexOf(player);
            
            player.Score   += Score;

            Unibus.Dispatch(new Events.Game.Score.PlayerScoreUpdated() { PlayerIndex = playerIndex
                                                                       , PlayerName  = playerName
                                                                       , PlayerScore = player.Score });

            return player.Score;
        }
    }
}

namespace LevanPangInterview.Events.Game.Score
{
    public struct PlayerScoreUpdated
    {
        public string PlayerName;
        public int    PlayerIndex;
        public int    PlayerScore;
    }
}

namespace LevanPangInterview.Events.Game.Score
{
    public struct NewRecord
    {
        public string PlayerName;
        public int    Score;
    }
}
