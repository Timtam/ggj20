using System;
using Script.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Ship
{
	public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public ItemType itemType;

		public int ItemCount
		{
			get => shipInventory.ItemCounts[typeIndex];
			set
			{
				shipInventory.ItemCounts[typeIndex] = value;
				UpdateCount();
			}
		}
		public bool IsDragging { get; private set; } = false;

		private Canvas canvas;
		private Image image;
		private Text countText;
		private Inventory shipInventory;
		private GameObject dragIcon;
		private AudioSource audioSource;
		private int typeIndex;

		private void Start()
		{
			canvas = GetComponentInParent<Canvas>();
			shipInventory = GetComponentInParent<Inventory>();
			image = GetComponent<Image>();
			countText = GetComponentInChildren<Text>();
			audioSource = GetComponent<AudioSource>();
			UpdateSprite();
			UpdateCount();
		}

		public void UpdateSprite()
		{
			if (image == null) return;
			image.sprite = Item.GetSpriteForItem(itemType);
			typeIndex = Array.IndexOf(Item.ItemTypes, itemType);
		}

		public void UpdateCount()
		{
			if (countText == null) return;
			countText.text = ItemCount.ToString();
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (ItemCount <= 0) return;
			IsDragging = true;
			dragIcon = new GameObject("dragIcon");
			dragIcon.transform.SetParent(canvas.transform);
			dragIcon.transform.SetAsLastSibling();
			var iconImage = dragIcon.AddComponent<Image>();
			iconImage.sprite = image.sprite;
			iconImage.raycastTarget = false;
			var rectTransform = iconImage.GetComponent<RectTransform>();
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25f);
			audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drag"));
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (dragIcon == null) return;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform,
				eventData.position, eventData.pressEventCamera, out var position))
			{
				dragIcon.transform.position = position;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (dragIcon == null) return;
			Destroy(dragIcon);
			dragIcon = null;
			IsDragging = false;
		}

		public void PlayDropSound()
		{
			switch (itemType)
			{
				case ItemType.Cabling:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_kabel"));
					break;
				case ItemType.CircuitBoard:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_platine"));
					break;
				case ItemType.DuctTape:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_tape"));
					break;
				case ItemType.EnergyCrystal:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_energiekristall"));
					break;
				case ItemType.Fuel:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_treibstoff"));
					break;
				case ItemType.MetalPlate:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_metallplatte"));
					break;
				case ItemType.Microchip:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_microchip"));
					break;
				case ItemType.Oxygen:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_sauerstoff"));
					break;
				case ItemType.Cabin:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_kabine"));
					break;
				case ItemType.Cargo:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_lagerraum"));
					break;
				case ItemType.Navigation:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_navigation"));
					break;
				case ItemType.PowerPlant:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_generator"));
					break;
				case ItemType.Shield:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_schild"));
					break;
				case ItemType.Thruster:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/drop_triebwerk"));
					break;
				default:
					throw new Exception();
			}
		}
	}
}
