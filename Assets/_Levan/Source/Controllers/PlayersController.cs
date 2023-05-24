using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Models;
using UnibusEvent;
using UnityEngine;

namespace LevanPangInterview.Controllers
{
    public class PlayersController : Controller
    {
        //////////////////////// Members
        
        public Models.PlayersCollection PlayersCollection;         // This will hold actual data in RUNTIME

        //////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log($"PlayerController.OnSystemInit()");
            this.PlayersCollection = new PlayersCollection();
        }

        
        public void Setup()
        {
            var gameplaySettings = Link.GetData<GameplayModel>();
            var playerCount      = gameplaySettings.PlayerCount;

            SetupPlayers(playerCount);
        }

        //////////////////////// Methods

        /// <summary>
        /// Initializas player model objects to hold player data.
        /// using presets we can easly have data ready, and its eaasy to edit if needed.
        /// </summary>
        /// <param name="playerCount"></param>
        public void SetupPlayers(int playerCount)
        {            
            Logger.Log($"PlayerController.InitializePlayers() playerCount={playerCount}");

            var presetPlayersCollection = Link.Data.GetPreset<Models.PlayersCollection>(); // collection of player data PRESETS - set in edit time
            
            this.PlayersCollection?.Clear();

            // Create player model objects up to the current player count
            for (int i = 0; i < playerCount; i++)
            {
                Logger.Log($"PlayerController.InitializePlayers() i={i} ");

                // Clone player model preset into player model object
                var preset  = presetPlayersCollection[i];
                var Player  = Instantiate(preset);

                this.PlayersCollection.Add( Player );
            }

            Logger.Log($"PlayerController.SettingModel");

            // Saving data in data service
            Link.Data.SetModelSingle(PlayersCollection);
        }
    }
}
