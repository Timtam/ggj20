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
					return "Items/cabling";
				case ItemType.CircuitBoard:
					return "Items/circuitBoard";
				case ItemType.DuctTape:
					return "Items/ductTape";
				case ItemType.EnergyCrystal:
					return "Items/energyCrystal";
				case ItemType.Fuel:
					return "Items/fuel";
				case ItemType.Microchip:
					return "Items/microchip";
				case ItemType.MetalPlate:
					return "Items/metalPlate";
				case ItemType.Oxygen:
					return "Items/oxygen";
				case ItemType.Cabin:
				case ItemType.Cargo:
				case ItemType.Navigation:
					return "Items/navigation";
				case ItemType.PowerPlant:
				case ItemType.Shield:
				case ItemType.Thruster:
					return "Items/thruster";
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
