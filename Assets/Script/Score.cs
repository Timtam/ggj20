using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
	public class Score : MonoBehaviour
	{
		private void Start()
		{
			var text = GetComponent<Text>();
			var s = Globals.Instance.timeAlive % 60f;
			var min = Mathf.RoundToInt((Globals.Instance.timeAlive - s) / 60f);
			var sec = Mathf.FloorToInt(s);
			text.text = $"Survived Flights\n{Globals.Instance.survivedFlights}\n\nTotal Time Alive\n{min:00}:{sec:00}";
		}
	}
}
