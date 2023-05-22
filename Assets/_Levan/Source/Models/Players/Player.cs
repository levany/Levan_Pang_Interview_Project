using System;
using System.Collections;
using System.Collections.Generic;
using LevanPangInterview.Models;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LevanPangInterview.Models
{
	[CreateAssetMenu(fileName="Player", menuName="Models/Player", order=0)]
	[Serializable]
	public class Player : Model
	{
		[Header("Info")]
		public string	  Name;
		public int		  Score;

		[Header("Gameplay")]
		public Color	  Color;

		[Header("Input")]
		public InputAction RightInput;
		public InputAction LeftInput;
		public InputAction FireInput;


		public void CopyTo(Player other)
		{
			other.name       = this.name;
			other.Score      = this.Score;
			other.Color      = this.Color;
			other.RightInput = this.RightInput;
			other.LeftInput  = this.LeftInput;
			other.FireInput  = this.FireInput;
		}
    } 
}
