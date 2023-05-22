using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LevanPangInterview.Models
{
    [CreateAssetMenu(fileName="Players", menuName="Models/Players", order=0)]
    [Serializable]
    public class PlayersCollection : Models.Collection<Models.Player>
    {
    }
}
