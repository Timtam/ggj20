using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Destruction : MonoBehaviour
	{
		public Meteor meteorPrefab;
		public Explosion explosionPrefab;

		private Canvas canvas;
		private ShipComponent[] components;
		private AudioSource audioSource;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			components = canvas.GetComponentsInChildren<ShipComponent>();
			audioSource = GetComponent<AudioSource>();
			StartCoroutine(DestructionLoop());
		}

		private IEnumerator DestructionLoop()
		{
			while (true)
			{
				yield return new WaitForSeconds(Random.Range(5f, 10f));

				var meteor = Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity, transform);
				var rt = meteor.transform as RectTransform;
				var height = canvas.pixelRect.height * 1.2f;
				var width = canvas.pixelRect.width * 0.4f;
				rt.anchoredPosition = new Vector2(Random.Range(- width, width), (Random.Range(0, 2) * 2f - 1f) * height);
				var body = meteor.GetBody();
				body.angularVelocity = Random.Range(-1f, 1f) * 100f;
				var direction = transform.position - rt.position;
				body.AddForce(direction * Random.Range(0.1f, 0.5f), ForceMode2D.Impulse);
			}
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			var meteor = other.gameObject.GetComponent<Meteor>();
			if (meteor == null) return;

			Instantiate(explosionPrefab, meteor.transform.position, Quaternion.identity, transform);
			Destroy(meteor.gameObject);

			ShipComponent component;
			var loopCnt = 0;
			do
			{
				component = components[Random.Range(0, components.Length)];
				loopCnt++;
			} while (component.Health <= 0f && loopCnt <= 10);
			if (component.Health <= 0f) return;
			component.Damage(Random.Range(0.1f, 0.4f));
		}

		public void DestroyShip()
		{
			StopAllCoroutines();
			audioSource.Play();
			StartCoroutine(ShipDestructionLoop());
		}

		private IEnumerator ShipDestructionLoop()
		{
			var explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			var rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(-70, 10);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(30, 30);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(-30, 20);

			yield return new WaitForSeconds(2f);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(30, -30);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(60, 10);

			yield return new WaitForSeconds(2f);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(10, 100);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(-30, 20);

			yield return new WaitForSeconds(1.5f);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(30, -30);

			explosion = Instantiate(explosionPrefab, Vector3.zero, Quaternion.identity, transform);
			explosion.playHitSound = false;
			rt = explosion.transform as RectTransform;
			rt.anchoredPosition = new Vector2(100, 10);

			while (audioSource.isPlaying)
			{
				yield return new WaitForSeconds(0.2f);
			}

			SceneManager.LoadScene("MainMenu");
		}
	}
}
