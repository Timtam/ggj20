using System;
using System.Collections.Generic;
using Script.Items;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Ship
{
	public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public ItemType itemType;

		private Canvas canvas;
		private Image image;
		private GameObject dragIcon;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			image = GetComponent<Image>();
			UpdateSprite();
		}

		public void UpdateSprite()
		{
			if (image == null) return;
			image.sprite = Item.GetSpriteForItem(itemType);
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			dragIcon = new GameObject("dragIcon");
			dragIcon.transform.SetParent(canvas.transform);
			dragIcon.transform.SetAsLastSibling();
			var iconImage = dragIcon.AddComponent<Image>();
			iconImage.sprite = image.sprite;
			var rectTransform = iconImage.GetComponent<RectTransform>();
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25f);
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform,
				eventData.position, eventData.pressEventCamera, out var position))
			{
				dragIcon.transform.position = position;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			Destroy(dragIcon);
			dragIcon = null;
		}
	}
}
