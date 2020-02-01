using Script.Items;

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
