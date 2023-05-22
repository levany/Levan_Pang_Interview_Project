using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevanPangInterview
{
	public enum SnapDirection
	{
		Right,
		Left,
	}

	/// <summary>
	/// This script aligns an object to either the left or right edge of the screen
	/// </summary>
	public class WallSnap : MonoBehaviour
	{
		public SnapDirection SnapDirection;
		public Camera		 GameCamra;

		void Start()
		{
			if (!this.enabled)
				return;

			if (!Link.AppSettings.ShouldStrechWorldToFitScreen)
				return;

			// Align the object to the left edge of the screen, or to the right
			if		(SnapDirection == SnapDirection.Left) 
				transform.position = new Vector3(GameCamra.ScreenToWorldPoint(Vector3.zero).x
																			 ,transform.position.y
																			 ,transform.position.z);
			else if (SnapDirection == SnapDirection.Right) 
				transform.position = new Vector3(GameCamra.ScreenToWorldPoint(Vector3.right * Screen.width).x
																			 ,transform.position.y
																			 ,transform.position.z);
		}
	}

}