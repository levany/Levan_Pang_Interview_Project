using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LevanPangInterview.Views
{
	public class ScoreHUDViewItem : MonoBehaviour
	{
		public Models.Player   Player;

		public TextMeshProUGUI PlayerNameText;
		public TextMeshProUGUI PlayerScoreText;
		public Image           BackgroundImage;
	} 
}
