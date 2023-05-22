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
        public TextMeshProUGUI text;
        public KeyCode         toggleKeyCode;

        private void Reset()
        {
            text = GetComponentInChildren<TextMeshProUGUI>();
        }

        private void Awake()
        {
            Logger.SetLoggerCanvas(this);
        }

        public void Update()
        {
            if (Input.GetKeyDown(toggleKeyCode))
            {
                text.enabled = !text.enabled;
            }

        }

        public void Log(object message)
        {
            text.text += "\n" + message;
        }
        public void LogError(object message)
        {
            text.text += "\n" + $"<color=red>{message}</color>";
        }
    }
}