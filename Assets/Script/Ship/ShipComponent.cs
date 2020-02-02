using System;
using System.Collections.Generic;
using System.Linq;
using Script.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Ship
{
	public class ShipComponent : MonoBehaviour, IDropHandler
	{
		[NonSerialized]
		public float health = 0.5f;
		public ItemType componentType;
		public ItemType[] parts;
		public bool flipHorizontal;

		private AudioSource audioSource;
		private ComponentPart part0;
		private ComponentPart part1;
		private Image lifebar;
		private Image componentImage;
		private float lifebarWidth;
		private Inventory inventory;

		private void Start()
		{
			part0 = transform.Find("Part0").GetComponent<ComponentPart>();
			part1 = transform.Find("Part1").GetComponent<ComponentPart>();
			lifebar = transform.Find("Lifebar").GetComponent<Image>();
			componentImage = transform.Find("Component").GetComponent<Image>();
			audioSource = GetComponent<AudioSource>();
			var canvas = FindObjectOfType<Canvas>();
			inventory = canvas.GetComponentInChildren<Inventory>();

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

			health = Globals.Instance.shipComponentHealth[Array.IndexOf(Item.ComponentTypes, componentType)];
		}

		private void OnDestroy()
		{
			Globals.Instance.shipComponentHealth[Array.IndexOf(Item.ComponentTypes, componentType)] = health;
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

			switch (componentType)
			{
				case ItemType.PowerPlant:
					health -= 0.015f * Time.deltaTime;
					break;
				case ItemType.Thruster:
					health -= 0.02f * Time.deltaTime;
					break;
			}
		}

		public void Damage(float damage)
		{
			health = Math.Max(0f, health - damage);
			if (health <= 0f)
			{
				PlayOfflineSound();
				if (componentType == ItemType.Cargo)
				{
					inventory.DestroyCargo();
				}
			}
		}

		private void PlayOfflineSound()
		{
			switch (componentType)
			{
				case ItemType.Cabin:
				case ItemType.Cargo:
				case ItemType.Navigation:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/disable_navigation"));
					break;
				case ItemType.PowerPlant:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/disable_generator"));
					break;
				case ItemType.Shield:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/disable_schild"));
					break;
				case ItemType.Thruster:
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/disable_triebwerk"));
					break;
				default:
					throw new Exception();
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

				if (inventorySlot.itemType == componentType)
				{
					// replacing the component gives full health
					health = 1f;
					return;
				}
				if (parts.Contains(inventorySlot.itemType))
				{
					// having the correct replacement part gives back 40% health
					health = Mathf.Min(1f, health + 0.4f);
					return;
				}
				if (inventorySlot.itemType == ItemType.Cabling)
				{
					// cabling always heals 10%
					health = Mathf.Min(1f, health + 0.1f);
					return;
				}
				if (inventorySlot.itemType == ItemType.DuctTape)
				{
					// duct tape is more efficient the more health is left
					var effiency = Mathf.Exp(health) - 1f;
					// some values:
					// 0 health -> 0
					// ~0.45 health -> 0.5
					// 0.5 health -> ~0.65
					// ~0.7 health -> 1
					var rand = Random.Range(0f, 0.5f);
					health = Mathf.Clamp(health + effiency - rand, 0f, 1f);
				}
			}
			else
			{
				// wrong part added
				inventorySlot.PlayWrongDropSound();
			}
		}
	}
}
