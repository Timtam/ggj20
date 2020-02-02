using Script.Items;
using UnityEngine;

namespace Script
{
	public class Globals
	{
		public static Globals Instance { get; }

		static Globals()
		{
			Instance = new Globals();
		}

		private Globals()
		{
			shipComponentHealth = new float[Item.ComponentTypes.Length];
			for (var i = 0; i < shipComponentHealth.Length; i++)
			{
				shipComponentHealth[i] = Mathf.Sqrt(Random.Range(0.4f, 1f));
			}
		}

		public int[] shipInventory = new int[Item.ItemTypes.Length];
		public float[] shipComponentHealth;
		public int survivedFlights = 0;
		public float timeAlive = 0f;
	}
}
