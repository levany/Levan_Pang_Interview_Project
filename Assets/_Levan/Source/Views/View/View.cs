using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Views
{
    public class View : MonoBehaviour
    {
        /// Properties

        public bool IsActive { get; protected set; }

        //// API

        public virtual async Task Execute() 
        { 
            Logger.Log($"View.Execute() : view = {this.name}");

            this.IsActive = true;

            this.Show();

            while (IsActive)
                await Task.Yield();

            Hide();
        }

        public virtual void CompleteExectiotion()
        {
            Logger.Log($"View.CompleteExectiotion : view = {this.name}");

            this.IsActive = false;
        }

        public void Show()
        {
            Logger.Log($"View.Show() : view = {this.name}");

            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            Logger.Log($"View.Hide() : view = {this.name}");

            this.gameObject.SetActive(false);
        }

        // Virtual Methods

        public virtual async Task OnSystemInit()
        {

        }
    }
}
