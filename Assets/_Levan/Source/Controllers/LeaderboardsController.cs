using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevanPangInterview.Events.Game.Score;
using LevanPangInterview.Models;
using Newtonsoft.Json;
using UnibusEvent;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace LevanPangInterview.Controllers
{
    public class LeaderboardsController : Controller
    {
        // consts
        
        public const string LEADERBOARDS_PERSISTANT_MODEL_KEY = "LEADERBOARDS_PERSISTANT_MODEL_KEY";

        // Members

        [SerializeField] private Models.Leaderboards LeaderboardsInitialPrestData;
        [NonSerialized ] public  Models.Leaderboards LeaderboardsData;

        // Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log($"LeaderboardsController.SystemInit");
            
            LoadLeaderboardsData();

            this.BindUntilDestroy<Events.Game.Score.NewRecord>(OnNewRecord);
        }

        // Api

        public bool IsNewRecord(int score)
        {
            Logger.Log($"score : {score}");

            var minTopScore  = this.LeaderboardsData.Records.Min(w => w.Score);
            var maxTopScore  = this.LeaderboardsData.Records.Max(w => w.Score);

            bool isNewRecord = score > minTopScore && score <= maxTopScore;
            
            Logger.Log($"minTopScore : {minTopScore}");
            Logger.Log($"maxTopScore : {maxTopScore}");
            Logger.Log($"IsNewRecord : {isNewRecord}");
            
            return isNewRecord;
        }

        // Events

        private void OnNewRecord(NewRecord @event)
        {
            Logger.Log($"score : {@event.PlayerName}, {@event.Score}");

            // Add new record
            LeaderboardsData.Records.Add(new RecordInfo()
            {
                Name  = @event.PlayerName,
                Score = @event.Score,
            });

            // Sort by score
            LeaderboardsData.Records = LeaderboardsData.Records.OrderBy(r => r.Score).ToList();
            
            // Remove first item - no loger top record holder
            Logger.Log($"Removing old Leaderboards record holder {this.LeaderboardsData.Records[0].ToString()}");
            this.LeaderboardsData.Records.RemoveAt(0);

            // Save
            SaveLeaderboardsData();
        }


        // Methods

        private void LoadLeaderboardsData()
        {
            Logger.Log($"Loading Leaderboards Data");

            // Try Online Storage
            var onlineData = Link.SimpleCloudService.GetItem<Models.Leaderboards>();

            if (onlineData != null)
            {
                Logger.Log($"Loaded Online Data");

                this.LeaderboardsData = onlineData;
                Link.Data.SetModelSingle(this.LeaderboardsData);
                return;
            }


            // Try Local storage

            var localData = Link.Storage.LoadItem<Models.Leaderboards>(LEADERBOARDS_PERSISTANT_MODEL_KEY);

            if (localData != null )
            {
                Logger.Log($"Loaded Local Data");
                
                this.LeaderboardsData = localData;
                Link.Data.SetModelSingle(this.LeaderboardsData);
                return;
            }
            
            // If No data available (errors, or first time playing)
            // Use Preset data
            this.LeaderboardsData = Link.Data.ClonePreset<Models.Leaderboards>();
            Link.Data.SetModelSingle(this.LeaderboardsData);
        }

        private async Task SaveLeaderboardsData()
        {
            Logger.Log($"SaveLeaderboardsData");

            // Offline (for Backup)
            Link.Storage.StoreItem(LEADERBOARDS_PERSISTANT_MODEL_KEY, this.LeaderboardsData);

            // Online
            Link.SimpleCloudService.SetItem(this.LeaderboardsData);
            await Link.SimpleCloudService.Sync();
        }
    }
}
