using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace LevanPangInterview
{
    /// <summary>
    /// DEBUG COMPONENT
    /// </summary>
    public class LoggerCanvas : MonoBehaviour
    {
        //////////// Members

        [Header("state")]
        public  bool            IsEnabled        = true;

        [Header("references")]
        public  TextMeshProUGUI textBox;
        public  KeyCode         toggleKeyCode    = KeyCode.Alpha0;
        public  GameObject      rig; 

        private string          tempString       = "";

        //////////// Lifecycle


        public LoggerCanvas()
        {
            Logger.SetLoggerCanvas(this);
        }

        private void Awake()
        {
            textBox      = GetComponentInChildren<TextMeshProUGUI>(includeInactive:true);
            textBox.text = tempString;

            this.SetEnabled(this.IsEnabled);
        }
        public void Update()
        {
            if (Input.GetKeyDown(toggleKeyCode))
            {
                SetEnabled(!this.IsEnabled);
            }
        }

        //////////// API
        
        public void SetEnabled(bool active)
        {
            this.rig.SetActive(active);
            this.IsEnabled = active;
        }

        //////////// API

        public void Log(object message)
        {
            if (this.textBox != null)
            {
                this.textBox.text += "\n" + message;
            }
            else
            {
                tempString += "\n" + message;
            }
        }
        public void LogError(object message)
        {
            if (this.textBox != null)
            {
                this.textBox.text += "\n" + $"<color=red>{message}</color>";
            }
            else
            {
                tempString += "\n" + $"<color=red>{message}</color>";
            }
        }
    }
}