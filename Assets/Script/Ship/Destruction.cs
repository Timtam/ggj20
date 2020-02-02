using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Destruction : MonoBehaviour
	{
		public Meteor meteorPrefab;
		public Explosion explosionPrefab;

		private Canvas canvas;
		private ShipComponent[] components;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			components = canvas.GetComponentsInChildren<ShipComponent>();
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
			} while (component.health <= 0f && loopCnt <= 10);
			if (component.health <= 0f) return;
			component.Damage(Random.Range(0.1f, 0.4f));
		}
	}
}
