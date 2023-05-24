using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace LevanPangInterview.Models
{
    [CreateAssetMenu(fileName = "LeaderboardsModel", menuName = "Models/LeaderboardsModel", order = 1)]
    [Serializable]
    public class Leaderboards : Model
    {
        public int              maxRecords = 10;
        public List<RecordInfo> Records    = new List<RecordInfo>();
    }

    [Serializable]
    public class RecordInfo
    {
        public string Name;
        public int    Score;

        public override string ToString() => $"{Name} : {Score}";
    }
}
