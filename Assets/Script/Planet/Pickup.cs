using System;
using System.Collections.Generic;
using Script.Items;
using UnityEngine;

namespace Script.Planet
{
	public class Pickup : MonoBehaviour
	{
		public ItemType pickupType;

		private SpriteRenderer spriteRenderer;
		private PolygonCollider2D polygonCollider2D;

		public void Start()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
			polygonCollider2D = GetComponent<PolygonCollider2D>();
			UpdateSprite();
		}

		public void UpdateSprite()
		{
			if (spriteRenderer == null) return;
			spriteRenderer.sprite = Item.GetSpriteForItem(pickupType);
			var shape = new List<Vector2>();
			spriteRenderer.sprite.GetPhysicsShape(0, shape);
			polygonCollider2D.points = shape.ToArray();
		}
	}
}
