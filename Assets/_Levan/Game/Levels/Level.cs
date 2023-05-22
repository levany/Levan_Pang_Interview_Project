using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevanPangInterview.Models
{
    [CreateAssetMenu(fileName = "Level", menuName = "Levels/Level", order = 0)]
    public class Level : Model
    {
        [Header("Info")]
        public string LevelName;

        [Header("Stage")]
        public Color BackgroundColor;
        public Color WallsColor;
        public Color CameraBGColor;

        [Header("Audio")]
        public AudioClip LevelMusic;

        [Header("Content")]
        public GameObject[] Prefabs;
    }

}