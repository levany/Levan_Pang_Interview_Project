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
        //////////////////////////////// consts
        
        public const string LEADERBOARDS_PERSISTANT_MODEL_KEY = "LEADERBOARDS_PERSISTANT_MODEL_KEY";

        //////////////////////////////// Members

        [SerializeField] private Models.Leaderboards LeaderboardsInitialPrestData;
        [NonSerialized ] public  Models.Leaderboards LeaderboardsData;

        //////////////////////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log($"LeaderboardsController.SystemInit");
            
            LoadLeaderboardsData();

            this.BindUntilDestroy<Events.Game.Score.NewRecord>(OnNewRecord);
        }

        //////////////////////////////// Api

        public bool IsNewRecord(int score)
        {
            Logger.Log($"score : {score}");

            var minTopScore  = this.LeaderboardsData.Records.Min(w => w.Score);

            bool isNewRecord = score >= minTopScore;
            
            Logger.Log($"minTopScore : {minTopScore}");
            Logger.Log($"IsNewRecord : {isNewRecord}");
            
            return isNewRecord;
        }

        //////////////////////////////// Events

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


        //////////////////////////////// Methods

        private void LoadLeaderboardsData()
        {
            Logger.Log($"Loading Leaderboards Data");

            // Try Online Storage
            Logger.Log($"loading online data");
            var onlineData = Link.SimpleCloudService.GetItem<Models.Leaderboards>();

            if (IsValidLEaderboardsData(onlineData))
            {
                Logger.Log($"Loaded Online Data");

                this.LeaderboardsData = onlineData;
                Link.Data.SetModelSingle(this.LeaderboardsData);
                return;
            }

            // Try Local storage
            Logger.Log($"loading local data");
            var localData = Link.Storage.LoadItem<Models.Leaderboards>(LEADERBOARDS_PERSISTANT_MODEL_KEY);

            if (IsValidLEaderboardsData(localData))
            {
                Logger.Log($"Loaded Local Data");
                
                this.LeaderboardsData = localData;
                Link.Data.SetModelSingle(this.LeaderboardsData);
                return;
            }
            

            // If No data available (errors, or first time playing)
            // Use Preset data
            Logger.Log($"loading preset (backup) data");
            this.LeaderboardsData = Link.Data.ClonePreset<Models.Leaderboards>();
            Link.Data.SetModelSingle(this.LeaderboardsData);

            // if we rech the back up - data is currupted - override it
            Logger.Log($"Resetting currupt local data to backup");
            Link.Storage.StoreItem(LEADERBOARDS_PERSISTANT_MODEL_KEY, this.LeaderboardsData);

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

        //////////////////////////////// Helper methods
        
        public bool IsValidLEaderboardsData(Models.Leaderboards data)
        {
            Logger.Log("checking leaderboard data validity");

            if (data == null)
            {
                Logger.LogError("data.records is null");
                return false;
            }

            if (data.Records == null)
            {
                Logger.LogError("data.records is null");
                return false;
            }

            if (data.Records.Count == 0) 
            {
                Logger.LogError("data.records.count is 0");
                return false;
            }

            if (data.Records.Any(r => r == null))
            {
                Logger.LogError("data contains a null record");
                return false;
            }

            return true;
        }
    }
}
