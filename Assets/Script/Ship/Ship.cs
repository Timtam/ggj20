using System;
using System.Linq;
using Script.Items;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Ship : MonoBehaviour
	{
		private Text countdownText;
		private float remainingDistance, remainingTime, speed;

		private ShipComponent thruster;

		private void Start()
		{
			countdownText = transform.Find("Countdown").GetComponent<Text>();
			remainingDistance = Random.Range(200f, 300f);
			speed = 1.5f;

			var canvas = GetComponentInParent<Canvas>();
			var components = canvas.GetComponentsInChildren<ShipComponent>();
			thruster = components.First(c => c.componentType == ItemType.Thruster);
		}

		private void Update()
		{
			// time/distance calculation
			remainingDistance -= Time.deltaTime * speed;
			remainingTime = remainingDistance / speed;
			var sec = Mathf.Max(0, Mathf.FloorToInt(remainingTime % 60f));
			var min = Mathf.FloorToInt((remainingTime - sec) / 60f);
			countdownText.text = $"{min:00}:{sec:00}";

			// component updates
			speed = 1.5f;
			if (thruster.health <= 0.5f)
			{
				speed -= 0.5f - thruster.health;
			}
		}
	}
}
