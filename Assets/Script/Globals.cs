using Script.Items;
using UnityEngine;

namespace Script
{
	public class Globals
	{
		public static Globals Instance { get; }

		static Globals()
		{
			Instance = new Globals();
		}

		public int[] shipInventory = new int[Item.ItemTypes.Length];
	}
}
