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
			for (var i = 0; i < 20; i++)
			{
				var type = GetNewPickupType();
				var sprite = Item.GetSpriteForItem(type);
				var spriteSize = sprite.rect.size / sprite.pixelsPerUnit;
				while (true)
				{
					var x = Random.value * 50 - 25;
					var y = Random.value * 50 - 25;
					var hit = Physics2D.BoxCast(new Vector2(x, y), spriteSize, 0, Vector2.zero);
					if (hit.collider != null) continue;
					var pickup = Instantiate(pickupPrefab, new Vector3(x, y, 0), Quaternion.identity);
					pickup.pickupType = type;
					pickup.UpdateSprite();
					break;
				}
			}
		}

		private ItemType GetNewPickupType()
		{
			var t1 = Random.value;
			var t2 = Random.value;
			if (t1 > 0.8f)
			{
				// spawn component
				return Item.ComponentTypes[Mathf.FloorToInt(t2 * Item.ComponentTypes.Length)];
			}
			else
			{
				// spawn part
				return Item.PartTypes[Mathf.FloorToInt(t2 * Item.PartTypes.Length)];
			}
		}
	}
}
