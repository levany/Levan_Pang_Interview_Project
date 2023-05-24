using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevanPangInterview.Controllers;
using LevanPangInterview.Models;
using TMPro;
using UnibusEvent;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace LevanPangInterview.Views
{
    public class ScoreView : View
    {
        //////////////// Members

        [Header("Main PAnel")]
        public GameObject       MainPanel;
        public TextMeshProUGUI  NameText;
        public TextMeshProUGUI  ScoreText;

        [Header("Input panel")]
        public GameObject       NewRecordPanel;
        public TMP_InputField   InputField;
        public Button           SubmitButton;

        // Private

        private bool            isNewRecord = false;
        private Models.Player   PlayerData;

        //////////////// Properties

        public string ResultPlayerName = null;

        //////////////// API

        public async Task ExecuteScoreViewForPlayer(Models.Player player, bool IsNewRecord)
        {
            Logger.Log($"Showing score view for player : {player.name}");

            this.PlayerData       = player;
            this.ResultPlayerName = null;

            this.RefreshView();

            this.isNewRecord = IsNewRecord;

            await Execute();
        }

        //////////////// Lifecycle

        public override async Task OnSystemInit()
        {
            Logger.Log($"OnSystemInit()");
        }


        public void Update()
        {
            if (Keyboard.current[Link.AppSettings.EascapeKey].wasPressedThisFrame)
            {
                if (isNewRecord)
                {
                    if (MainPanel.gameObject.activeSelf)
                        OnOkButtonClicked();
                    else
                        this.CompleteExectiotion();        
                }
                else
                {
                    this.CompleteExectiotion();
                }
            }
        }

        //////////////// Main UI Event

        public void OnOkButtonClicked()
        {
            Logger.Log($"OnOkButtonClicked");

            if (!isNewRecord)
            {
                this.CompleteExectiotion();
                return;
            }
            else
            {
                MainPanel.SetActive(false);
                NewRecordPanel.SetActive(true);
            }
        }

        public void OnInputSubmitButtonClicked()
        {
            this.ResultPlayerName = InputField.text;
            this.CompleteExectiotion();
        }

        
        public void OnCloseButtonClicked()
        {
            this.CompleteExectiotion();
        }

        //////////////// Utility UI Event

        public void OnInputFieldTextChange(string @event)
        {
            var text = InputField.text;
            this.SubmitButton.interactable = (text != "");
        }

        //////////////// Methods

        public void RefreshView()
        {
            Logger.Log($"Refreshing View");

            var player = this.PlayerData;

            try
            {
                this.MainPanel.SetActive(true);
                this.NewRecordPanel.SetActive(false);

                this.NameText.text  = player.Name;
                this.NameText.color = player.Color;

                this.ScoreText.text = player.Score.ToString();

                this.SubmitButton.interactable = false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                throw;
            }
        }
    }

}