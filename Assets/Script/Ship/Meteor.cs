using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Meteor : MonoBehaviour
	{
		private Image image;
		private new Rigidbody2D rigidbody2D;

		private void Start()
		{
			image = GetComponent<Image>();
			image.sprite = Resources.Load<Sprite>($"Space/meteor_{Random.Range(0, 8)}");
			image.SetNativeSize();
		}

		public Rigidbody2D GetBody()
		{
			if (rigidbody2D == null)
			{
				rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
			}
			return rigidbody2D;
		}
	}
}
