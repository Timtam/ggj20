using System;
using System.Collections.Generic;
using System.Linq;
using Script.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Ship
{
	public class ShipComponent : MonoBehaviour, IDropHandler
	{
		[NonSerialized]
		public float health = 0.5f;
		public ItemType componentType;
		public ItemType[] parts;
		public bool flipHorizontal;

		private ComponentPart part0;
		private ComponentPart part1;
		private Image lifebar;
		private Image componentImage;
		private float lifebarWidth;

		private void Start()
		{
			part0 = transform.Find("Part0").GetComponent<ComponentPart>();
			part1 = transform.Find("Part1").GetComponent<ComponentPart>();
			lifebar = transform.Find("Lifebar").GetComponent<Image>();
			componentImage = transform.Find("Component").GetComponent<Image>();

			Flip();

			UpdatePart(ref part0, 0);
			UpdatePart(ref part1, 1);

			if (parts.Length > 2)
			{
				parts = parts.Take(2).ToArray();
			}

			componentImage.sprite = Item.GetSpriteForItem(componentType);
			var rt = lifebar.transform as RectTransform;
			lifebarWidth = rt.rect.width;
		}

		private void Flip()
		{
			if (!flipHorizontal) return;
			var rt = part0.transform as RectTransform;
			var anchorMax = rt.anchorMax;
			var anchorMin = rt.anchorMin;
			rt.anchorMax = new Vector2(1 - anchorMin.x, anchorMax.y);
			rt.anchorMin = new Vector2(1 - anchorMax.x, anchorMin.y);
			var position = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(position.x * -1, position.y);

			rt = part1.transform as RectTransform;
			anchorMax = rt.anchorMax;
			anchorMin = rt.anchorMin;
			rt.anchorMax = new Vector2(1 - anchorMin.x, anchorMax.y);
			rt.anchorMin = new Vector2(1 - anchorMax.x, anchorMin.y);
			position = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(position.x * -1, position.y);

			rt = lifebar.transform as RectTransform;
			anchorMax = rt.anchorMax;
			anchorMin = rt.anchorMin;
			rt.anchorMax = new Vector2(1 - anchorMin.x, anchorMax.y);
			rt.anchorMin = new Vector2(1 - anchorMax.x, anchorMin.y);
			position = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(position.x * -1, position.y);
			var pivot = rt.pivot;
			rt.pivot = new Vector2(1 - pivot.x, pivot.y);

			rt = componentImage.transform as RectTransform;
			anchorMax = rt.anchorMax;
			anchorMin = rt.anchorMin;
			rt.anchorMax = new Vector2(1 - anchorMin.x, anchorMax.y);
			rt.anchorMin = new Vector2(1 - anchorMax.x, anchorMin.y);
			position = rt.anchoredPosition;
			rt.anchoredPosition = new Vector2(position.x * -1, position.y);
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

		private void Update()
		{
			var rt = lifebar.transform as RectTransform;
			rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, lifebarWidth * health);
			if (health > 0.5f)
			{
				lifebar.color = Color.green;
			}
			else if (health > 0.2f)
			{
				lifebar.color = Color.yellow;
			}
			else
			{
				lifebar.color = Color.red;
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
