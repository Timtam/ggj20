using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Ship
{
	public class Explosion : MonoBehaviour
	{
		public Sprite[] sprites;

		private Image image;

		private void Start()
		{
			image = GetComponent<Image>();
			StartCoroutine(ShowExplosion());
		}

		private IEnumerator ShowExplosion()
		{
			foreach (var sprite in sprites)
			{
				image.sprite = sprite;
				yield return new WaitForSeconds(0.1f);
			}
			Destroy(gameObject);
		}
	}
}
