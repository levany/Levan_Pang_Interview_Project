using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnibusEvent;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LevanPangInterview.Views
{
    public class LeaderboardsView : View
    {
        //// Members

        [Header("Main PAnel")]
        public GameObject                  MainPanel;
        public GameObject                  ContainerGO;
        public GameObject                  ItemPrefab;

        // private
        private Models.Leaderboards        LeaderboardsData;

        private List<LeaderboardsViewItem> Items;
        private bool                       IsInitialized = false;

        //// Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log($"OnSystemInit()");

            InitializeItemStubs();
            this.RefreshItems();
        }

        private void OnEnable()
        {
            if (!IsInitialized)
                return;

            RefreshItems();
        }

        public void Update()
        {
            if (Keyboard.current[Link.AppSettings.EascapeKey].wasPressedThisFrame)
            {
                this.CompleteExectiotion();
            }
        }

        //// UI Event

        public void OnCloseButtonClicked()
        {
            Logger.Log($"OnCloseButtonClicked");
            
            Unibus.Dispatch(new Events.App.LeaderboardsFinished());
            this.CompleteExectiotion();
        }

        //// Methods

        
        public void InitializeItemStubs()
        {
            Logger.Log($"Initializing Items");

            this.Items = new List<LeaderboardsViewItem>();
            
            try
            {
                var LeaderboardsPresets = Link.Data.GetPreset<Models.Leaderboards>();
                var presets             = LeaderboardsPresets.Records;
                var count               = LeaderboardsPresets.maxRecords;

                for (int i = 0; i < count; i++)
                {
                    var itemGO       = Instantiate(ItemPrefab, parent: ContainerGO.transform);
                    var item         = itemGO.GetComponent<LeaderboardsViewItem>();

                    item.Name        = presets[count - i -1].Name;
                    item.Score       = presets[count - i -1].Score.ToString();
                    item.PlaceNumber = (i + 1).ToString();

                    this.Items.Add(item);

                    IsInitialized = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }

        public void RefreshItems()
        {
            Logger.Log($"Refreshing Items");

            try
            {
                this.LeaderboardsData = Link.GetData<Models.Leaderboards>();
                var count             = LeaderboardsData.maxRecords;

                Logger.Log($"LoadedData is {LeaderboardsData}");

                for (int i = 0; i < count; i++)
                {
                    var winner = LeaderboardsData.Records[i];

                    Logger.Log($"Loaded Winner : {winner.Name} -> {winner.Score}");

                    var item         = this.Items[count - i - 1];

                    item.Name        = winner.Name;
                    item.Score       = winner.Score.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }     
        }
    }
}