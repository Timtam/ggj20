using Script.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Planet
{
	public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
	{
		private Canvas canvas;
		private Image itemImage;
		private ItemType? type;
		private GameObject dragIcon;
		private Inventory inventory;
		private int x, y;

		public bool IsDragging { get; private set; }

		private void Start()
		{
			itemImage = GetComponent<Image>();
			canvas = GetComponentInParent<Canvas>();
		}

		public void Setup(Inventory inventory, int x, int y)
		{
			this.inventory = inventory;
			this.x = x;
			this.y = y;
		}

		public ItemType? GetItemType()
		{
			return type;
		}

		public void SetItemType(ItemType? newType)
		{
			type = newType;
			if (newType == null)
			{
				itemImage.color = new Color(1, 1, 1, 0);
			}
			else
			{
				var type = newType.Value;
				itemImage.color = Color.white;
				itemImage.sprite = Item.GetSpriteForItem(type);
				var size = Item.GetItemInventorySize(type);
				var rt = itemImage.rectTransform;
				if (size == 1)
				{
					rt.offsetMax = Vector2.zero;
					rt.offsetMin = Vector2.zero;
				}
				else
				{
					rt.offsetMax = new Vector2(50, 0);
					rt.offsetMin = new Vector2(0, -50);
				}
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			if (type == null) return;
			IsDragging = true;
			dragIcon = new GameObject("dragIcon");
			dragIcon.transform.SetParent(canvas.transform);
			dragIcon.transform.SetAsLastSibling();
			var iconImage = dragIcon.AddComponent<Image>();
			iconImage.sprite = itemImage.sprite;
			iconImage.raycastTarget = false;
			var rectTransform = iconImage.GetComponent<RectTransform>();
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 25f);
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 25f);

			itemImage.raycastTarget = false;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if (dragIcon == null) return;
			if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform,
				eventData.position, eventData.pressEventCamera, out var position))
			{
				dragIcon.transform.position = position;
			}
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			if (dragIcon == null) return;
			Destroy(dragIcon);
			dragIcon = null;
			IsDragging = false;

			itemImage.raycastTarget = true;
		}

		public void OnDrop(PointerEventData eventData)
		{
			if (eventData.pointerDrag == gameObject) return;
			var sourceSlot = eventData.pointerDrag.GetComponent<InventoryItem>();
			if (sourceSlot == null || !sourceSlot.IsDragging) return;

			var type = inventory.RemoveItemFrom(sourceSlot.x, sourceSlot.y);
			if (!inventory.StoreItemAt(x, y, type.Value))
			{
				inventory.StoreItemAt(sourceSlot.x, sourceSlot.y, type.Value);
			}
		}
	}
}
