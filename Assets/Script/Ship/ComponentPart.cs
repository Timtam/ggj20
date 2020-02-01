using Script.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Ship
{
	public class ComponentPart : MonoBehaviour
	{
		public ItemType itemType;

		private Image image;

		private void Start()
		{
			image = GetComponent<Image>();
			UpdateSprite();
		}

		public void UpdateSprite()
		{
			if (image == null) return;
			image.sprite = Item.GetSpriteForItem(itemType);
		}
	}
}
