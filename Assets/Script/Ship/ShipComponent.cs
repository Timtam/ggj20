using System;
using System.Collections.Generic;
using System.Linq;
using Script.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Script.Ship
{
	public class ShipComponent : MonoBehaviour, IDropHandler
	{
		public float health;
		public ItemType componentType;
		public ItemType[] parts;
		public bool flipHorizontal;

		private ComponentPart part0;
		private ComponentPart part1;

		private void Start()
		{
			part0 = transform.Find("Part0").GetComponent<ComponentPart>();
			part1 = transform.Find("Part1").GetComponent<ComponentPart>();

			Flip();

			UpdatePart(ref part0, 0);
			UpdatePart(ref part1, 1);

			if (parts.Length > 2)
			{
				parts = parts.Take(2).ToArray();
			}
		}

		private void Flip()
		{
			if (!flipHorizontal) return;
			var rt = part0.transform as RectTransform;
			var anchor = rt.anchorMax;
			rt.anchorMax = new Vector2(1 - anchor.x, anchor.y);
			anchor = rt.anchorMin;
			rt.anchorMin = new Vector2(1 - anchor.x, anchor.y);
			anchor = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(anchor.x * -1, anchor.y);

			rt = part1.transform as RectTransform;
			anchor = rt.anchorMax;
			rt.anchorMax = new Vector2(1 - anchor.x, anchor.y);
			anchor = rt.anchorMin;
			rt.anchorMin = new Vector2(1 - anchor.x, anchor.y);
			anchor = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(anchor.x * -1, anchor.y);
		}

		private void UpdatePart(ref ComponentPart part, int index)
		{
			if (parts.Length > index)
			{
				part.itemType = parts[index];
				part.UpdateSprite();
			}
			else
			{
				Destroy(part.gameObject);
				part = null;
			}
		}

		public void OnDrop(PointerEventData eventData)
		{
			var inventorySlot = eventData.pointerDrag.GetComponent<InventorySlot>();
			if (inventorySlot == null || !inventorySlot.IsDragging) return;

			if (parts.Contains(inventorySlot.itemType) || inventorySlot.itemType == componentType ||
			    inventorySlot.itemType == ItemType.Cabling || inventorySlot.itemType == ItemType.DuctTape)
			{
				// correct part added
				inventorySlot.PlayDropSound();
				inventorySlot.ItemCount -= 1;
				// TODO: increase health
			}
		}
	}
}
