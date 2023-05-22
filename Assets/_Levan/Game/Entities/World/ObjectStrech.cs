using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevanPangInterview
{

	/// <summary>
	/// This script streches the width of an object to fit the game camera
	/// </summary>
	public class ObjectStrech : MonoBehaviour
	{
		public Camera GameCamera;

		void Start()
		{
			if (!this.enabled)
				return;

			if (!Link.AppSettings.ShouldStrechWorldToFitScreen)
				return;

			ResizeSpriteToScreen();
		}
		
		void ResizeSpriteToScreen() 
		{
			var sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;
     
			var width  = sr.sprite.bounds.size.x;
			var height = sr.sprite.bounds.size.y;
     
			var worldScreenHeight = GameCamera.orthographicSize * 2.0;
			var worldScreenWidth  = worldScreenHeight / Screen.height * Screen.width;
     
			var scaleX = worldScreenWidth  / width;
			var scaleY = worldScreenHeight / height;

			transform.localScale = new Vector3((float)scaleX, transform.localScale.y, transform.localScale.z);
		}

	}
}