using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Controllers
{
    public class Controller : MonoBehaviour
    {
        // API

        public virtual void Enable()
        {
            Logger.Log($"Controller.Enable() : name = {this.name}");
            this.gameObject.SetActive(true);
        }

        public virtual void Disable()
        {   
            Logger.Log($"Controller.Disable() : name = {this.name}");
            this.gameObject.SetActive(false);
        }

        // Virtual Methods

        public virtual async Task OnSystemInit()
        {

        }
    }
}
