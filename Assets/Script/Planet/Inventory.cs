using System;
using Script.Items;
using UnityEngine;

namespace Script.Planet
{
	public class Inventory : MonoBehaviour
	{
		public GameObject spacePrefab;

		private const int SizeX = 4;
		private const int SizeY = 6;

		private struct InventorySlot
		{
			public bool IsFree { get; set; }
			public InventoryItem Item { get; set; }
		}

		private InventorySlot[,] inventory = new InventorySlot[SizeX, SizeY];

		private void Start()
		{
			for (var y = 0; y < SizeY; y++)
			{
				for (var x = 0; x < SizeX; x++)
				{
					var inst = Instantiate(spacePrefab, Vector3.zero, Quaternion.identity, transform);
					var rt = inst.transform as RectTransform;
					rt.anchoredPosition = new Vector3(50 + 50 * x, (50 + 50 * y) * -1, 0);
					rt.SetAsFirstSibling();
					inventory[x, y].IsFree = true;
					inventory[x, y].Item = inst.GetComponentInChildren<InventoryItem>();
					inventory[x, y].Item.Setup(this, x, y);
				}
			}
		}

		public bool StoreItem(ItemType type)
		{
			var size = Item.GetItemInventorySize(type);
			for (var y = 0; y < SizeY + 1 - size; y++)
			{
				for (var x = 0; x < SizeX + 1 - size; x++)
				{
					if (IsSpaceFree(x, y, size) && StoreItemAt(x, y, type))
					{
						return true;
					}
				}
			}

			return false;
		}

		public ItemType? RemoveItemFrom(int x, int y)
		{
			var type = inventory[x, y].Item.GetItemType();
			if (type == null) return null;

			inventory[x, y].Item.SetItemType(null);
			inventory[x, y].IsFree = true;
			if (Item.GetItemInventorySize(type.Value) == 2)
			{
				inventory[x + 1, y].IsFree = true;
				inventory[x, y + 1].IsFree = true;
				inventory[x + 1, y + 1].IsFree = true;
			}
			return type;
		}

		public bool StoreItemAt(int x, int y, ItemType type)
		{
			var size = Item.GetItemInventorySize(type);
			if (!IsSpaceFree(x, y, size)) return false;
			inventory[x, y].IsFree = false;
			inventory[x, y].Item.SetItemType(type);
			if (size == 2)
			{
				inventory[x + 1, y].IsFree = false;
				inventory[x, y + 1].IsFree = false;
				inventory[x + 1, y + 1].IsFree = false;
			}

			return true;
		}

		public bool IsSpaceFree(int x, int y, int size)
		{
			if (x < 0 || y < 0 || x >= SizeX + 1 - size || y >= SizeY + 1 - size)
			{
				return false;
			}

			if (size == 1)
			{
				return inventory[x, y].IsFree;
			}
			else
			{
				return inventory[x, y].IsFree && inventory[x + 1, y].IsFree && inventory[x, y + 1].IsFree &&
				       inventory[x + 1, y + 1].IsFree;
			}
		}

		public void MoveItemsToShip()
		{
			var globals = Globals.Instance;
			for (var y = 0; y < SizeY; y++)
			{
				for (var x = 0; x < SizeX; x++)
				{
					var type = RemoveItemFrom(x, y);
					if (type == null) continue;
					globals.shipInventory[Array.IndexOf(Item.ItemTypes, type.Value)]++;
				}
			}
		}
	}
}
