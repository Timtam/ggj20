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

		private Ship ship;

		private void Start()
		{
			part0 = transform.Find("Part0").GetComponent<ComponentPart>();
			part1 = transform.Find("Part1").GetComponent<ComponentPart>();
			lifebar = transform.Find("Lifebar").GetComponent<Image>();
			componentImage = transform.Find("Component").GetComponent<Image>();
			audioSource = GetComponent<AudioSource>();
			var canvas = FindObjectOfType<Canvas>();
			inventory = canvas.GetComponentInChildren<Inventory>();
			ship = canvas.GetComponentInChildren<Ship>();

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
					health -= 0.01f * Time.deltaTime;
					break;
			}
		}

		public void Damage(float damage)
		{
			if (componentType != ItemType.Shield && ship.Shield.health > 0f)
			{
				var shieldEfficiency = Mathf.Min(ship.Shield.health * 2f, 1f);
				var totalDamageMultiplier = (1 - shieldEfficiency) * 0.5f + 0.5f;
				var componentDamage = damage * totalDamageMultiplier * 0.4f;
				var shieldDamage = damage * totalDamageMultiplier * 0.6f;
				health = Math.Max(0f, health - componentDamage);
				ship.Shield.Damage(shieldDamage);
			}
			else
			{
				health = Math.Max(0f, health - damage);
			}

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
					audioSource.PlayOneShot(Resources.Load<AudioClip>("Sounds/repair/disable_cabin"));
					break;
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
					// cabling always heals 15%
					health = Mathf.Min(1f, health + 0.15f);
					return;
				}
				if (inventorySlot.itemType == ItemType.DuctTape)
				{
					// duct tape may heal or not. chance increases the more health is left
					var chance = health > 0.5f ? 1f : health * 2f;
					if (Random.Range(0f, 1f) <= chance)
					{
						health = Mathf.Min(1f, health + 0.2f);
					}
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
