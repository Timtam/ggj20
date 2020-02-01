using System;
using Script.Items;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Planet
{
	public class Map : MonoBehaviour
	{
		public Pickup pickupPrefab;

		public void Start()
		{
			SpawnPickups();
		}

		private void SpawnPickups()
		{
			for (int i = 0; i < 20; i++)
			{
				var x = Random.value * 50 - 25;
				var y = Random.value * 50 - 25;
				var pickup = Instantiate(pickupPrefab, new Vector3(x, y, 0), Quaternion.identity);
				var t1 = Random.value;
				var t2 = Random.value;
				if (t1 > 0.8f)
				{
					// spawn component
					pickup.pickupType = Item.ComponentTypes[Mathf.FloorToInt(t2 * Item.ComponentTypes.Length)];
				}
				else
				{
					// spawn part
					pickup.pickupType = Item.PartTypes[Mathf.FloorToInt(t2 * Item.PartTypes.Length)];
				}
				pickup.UpdateSprite();
			}
		}
	}
}
