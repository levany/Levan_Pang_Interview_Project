using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevanPangInterview.Models;
using LevanPangInterview.Services;
using UnityEngine;

namespace LevanPangInterview
{
    public class DataService : Service
    {
        //////////////// Members

        public List<Model> Presets = new List<Model>(); // set at design time
        public List<Model> Models  = new List<Model>(); // usually set in runtime

        //////////////// API

        public void AddModel(Model model)
        {
            this.Models.Add(model);
        }
        
        public void SetModelSingle(Model model)
        {
            if (this.Models.Contains(model))
                this.Models.Remove(model);

            this.Models.Add(model);
        }
        
        public T GetModel<T>() where T : Model
        {
            return Models.FirstOrDefault(x => x is T) as T;
        }

        public T GetPreset<T>() where T : Model
        {
            var preset   = Presets.FirstOrDefault(x => x is T) as T;

            return preset;
        }

        public T ClonePreset<T>() where T : Model
        {
            var preset   = Presets.FirstOrDefault(x => x is T) as T;
            var instance = Instantiate(preset);

            return instance;
        }
    } 
}
