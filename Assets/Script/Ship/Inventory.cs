using System;
using Script.Items;
using UnityEngine;

namespace Script.Ship
{
	public class Inventory : MonoBehaviour
	{
		public int[] ItemCounts { get; private set; } = new int[Item.ItemTypes.Length];

		private void Start()
		{
			var globals = Globals.Instance;
			ItemCounts = globals.shipInventory;
			UpdateCount();
		}

		private void OnDestroy()
		{
			Globals.Instance.shipInventory = ItemCounts;
		}

		public void UpdateCount()
		{
			foreach (Transform child in transform)
			{
				child.GetComponent<InventorySlot>()?.UpdateCount();
			}
		}
	}
}
