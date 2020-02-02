using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Explosion : MonoBehaviour
	{
		public Sprite[] sprites;

		private Image image;
		private AudioSource audioSource;

		private void Start()
		{
			image = GetComponent<Image>();
			audioSource = gameObject.AddComponent<AudioSource>();
			audioSource.PlayOneShot(Resources.Load<AudioClip>($"Sounds/repair/hit_{Random.Range(1, 6)}"));
			StartCoroutine(ShowExplosion());
		}

		private IEnumerator ShowExplosion()
		{
			foreach (var sprite in sprites)
			{
				image.sprite = sprite;
				yield return new WaitForSeconds(0.1f);
			}

			while (audioSource.isPlaying)
			{
				yield return  new WaitForSeconds(0.1f);
			}
			Destroy(gameObject);
		}
	}
}
