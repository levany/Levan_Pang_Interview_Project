using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Views;
using TMPro;
using UnityEngine;

namespace LevanPangInterview.Views
{
    public class LeaderboardsViewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI PlaceNumberText;
        [SerializeField] private TextMeshProUGUI NameText;
        [SerializeField] private TextMeshProUGUI ScoreText;

        public string PlaceNumber
        {
            get => PlaceNumberText.text;
            set => PlaceNumberText.text = value;
        }

        public string Name
        {
            get => NameText.text;
            set => NameText.text = value;
        }

        public string Score
        {
            get => ScoreText.text;
            set => ScoreText.text = value;
        }
    }
}