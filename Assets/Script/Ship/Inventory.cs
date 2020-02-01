using Script.Items;
using UnityEngine;

namespace Script.Ship
{
	public class Inventory : MonoBehaviour
	{
		public int[] ItemCounts { get; private set; } = new int[Item.ItemTypes.Length];

		public void UpdateCount()
		{
			foreach (Transform child in transform)
			{
				child.GetComponent<InventorySlot>()?.UpdateCount();
			}
		}
	}
}
