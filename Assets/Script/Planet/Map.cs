using System;
using System.Collections.Generic;
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
			ChooseMap();
			SpawnPickups();
		}

		private void ChooseMap()
		{
			var map = Random.Range(0, 5) + 1;
			var ways = transform.Find("Ways").GetComponent<SpriteRenderer>();
			var fg = transform.Find("FG").GetComponent<SpriteRenderer>();
			var fgCollider = fg.GetComponent<PolygonCollider2D>();
			ways.sprite = Resources.Load<Sprite>($"Map/Dorf {map}_Weg");
			fg.sprite = Resources.Load<Sprite>($"Map/Dorf {map}");

			var count = fg.sprite.GetPhysicsShapeCount();
			fgCollider.pathCount = count;
			for (var i = 0; i < count; i++)
			{
				var shape = new List<Vector2>();
				fg.sprite.GetPhysicsShape(i, shape);
				fgCollider.SetPath(i, shape);
			}
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
