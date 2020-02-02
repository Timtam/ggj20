using System;
using System.Linq;
using Script.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class Ship : MonoBehaviour
	{
		private Text countdownText;
		private float remainingDistance, remainingTime, speed;
		private ShipComponent[] components;
		private Destruction destruction;
		private AudioSource music;

		public ShipComponent Cabin { get; private set; }
		public ShipComponent Cargo { get; private set; }
		public ShipComponent Navigation { get; private set; }
		public ShipComponent PowerPlant { get; private set; }
		public ShipComponent Shield { get; private set; }
		public ShipComponent Thruster { get; private set; }

		public bool IsDestroyed { get; private set; }

		private void Start()
		{
			countdownText = transform.Find("Countdown").GetComponent<Text>();
			remainingDistance = Random.Range(200f, 300f);
			speed = 1.5f;

			var canvas = GetComponentInParent<Canvas>();
			components = canvas.GetComponentsInChildren<ShipComponent>();
			Cabin = components.First(c => c.componentType == ItemType.Cabin);
			Cargo = components.First(c => c.componentType == ItemType.Cargo);
			Navigation = components.First(c => c.componentType == ItemType.Navigation);
			PowerPlant = components.First(c => c.componentType == ItemType.PowerPlant);
			Shield = components.First(c => c.componentType == ItemType.Shield);
			Thruster = components.First(c => c.componentType == ItemType.Thruster);

			destruction = canvas.GetComponentInChildren<Destruction>();
			music = GetComponent<AudioSource>();
			music.clip = Resources.Load<AudioClip>($"Music/Repair/{Random.Range(1, 5)}");
			music.Play();
		}

		private void Update()
		{
			if (!IsDestroyed)
			{
				Globals.Instance.timeAlive += Time.deltaTime;
			}
			// time/distance calculation
			if (speed <= Mathf.Epsilon)
			{
				countdownText.text = "∞";
			}
			else
			{
				remainingDistance -= Time.deltaTime * speed;
				remainingTime = remainingDistance / speed;
				var sec = Mathf.Max(0, Mathf.FloorToInt(remainingTime % 60f));
				var min = Mathf.Max(0, Mathf.FloorToInt((remainingTime - sec) / 60f));
				countdownText.text = Navigation.Health <= 0.0f ? "??:??" : $"{min:00}:{sec:00}";
			}

			if (remainingTime <= 0)
			{
				Globals.Instance.survivedFlights++;
				SceneManager.LoadScene("LandShipScene");
			}

			// component updates
			speed = 1.5f;
			if (PowerPlant.Health <= 0.5f)
			{
				speed -= (0.5f - PowerPlant.Health) * 0.2f;
			}
			if (Thruster.Health <= 0.0f)
			{
				speed = 0;
			}
			else if (Thruster.Health <= 0.5f)
			{
				speed -= 0.5f - Thruster.Health;
			}

			if (components.All(c => c.Health <= 0f) && !IsDestroyed)
			{
				// all components destroyed
				IsDestroyed = true;
				destruction.DestroyShip();
			}
		}
	}
}
