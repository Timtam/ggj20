using System;
using System.Linq;
using UnityEngine;

namespace Script.Items
{
	public enum ItemType
	{
		// parts
		Cabling,
		CircuitBoard,
		EnergyCrystal,
		Fuel,
		MetalPlate,
		Microchip,
		Oxygen,
		DuctTape,
		// components
		Cabin,
		Cargo,
		Navigation,
		PowerPlant,
		Shield,
		Thruster,
	}

	public class Item
	{
		public static ItemType[] ItemTypes { get; } = Enum.GetValues(typeof(ItemType)).Cast<ItemType>().ToArray();
		public static ItemType[] PartTypes { get; } = ItemTypes.Take(8).ToArray();
		public static ItemType[] ComponentTypes { get; } = ItemTypes.Skip(8).ToArray();

		public static string GetResourcePathForItem(ItemType type)
		{
			switch (type)
			{
				case ItemType.Cabling:
					return "Items/dotRed";
				case ItemType.CircuitBoard:
					return "Items/genericItem_color_075";
				case ItemType.EnergyCrystal:
				case ItemType.Fuel:
				case ItemType.Microchip:
				case ItemType.MetalPlate:
				case ItemType.Oxygen:
				case ItemType.DuctTape:
					return "Items/dotRed";
				case ItemType.Cabin:
				case ItemType.Cargo:
				case ItemType.Navigation:
				case ItemType.PowerPlant:
				case ItemType.Shield:
				case ItemType.Thruster:
					return "Items/dotRed";
				default:
					throw new Exception();
			}
		}

		public static Texture2D GetTextureForItem(ItemType type) =>
			Resources.Load<Texture2D>(GetResourcePathForItem(type));

		public static Sprite GetSpriteForItem(ItemType type) =>
			Resources.Load<Sprite>(GetResourcePathForItem(type));
	}
}
