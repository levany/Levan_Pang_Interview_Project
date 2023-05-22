using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Models
{
    [CreateAssetMenu(fileName = "GameplayModel", menuName = "Models/GameplayModel", order = 1)]
    public class GameplayModel : Model
    {
        public int PlayerCount = 1;

        public Level[] Levels = new Level[0];
    }
}
