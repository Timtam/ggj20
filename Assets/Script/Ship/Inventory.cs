using System;
using Script.Items;
using UnityEngine;

namespace Script.Ship
{
	public class Inventory : MonoBehaviour
	{
		private static readonly float[] SlotXPositions = {-67f, 54f};
		private static readonly float[] SlotYPositions = {152f, 90f, 27f, -36f, -99f, -162f, -225f};

		public InventorySlot inventorySlotPrefab;

		public int[] ItemCounts { get; private set; } = new int[Item.ItemTypes.Length];

		private void Start()
		{
			ItemCounts = Globals.Instance.shipInventory;
			var typeIndex = 0;
			foreach (var y in SlotYPositions)
			{
				foreach (var x in SlotXPositions)
				{
					var slot = Instantiate(inventorySlotPrefab, Vector3.zero, Quaternion.identity, transform);
					slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
					slot.itemType = Item.ItemTypes[typeIndex++];
					slot.UpdateSprite();
				}
			}
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
