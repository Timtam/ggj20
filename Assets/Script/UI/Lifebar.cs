using System;
using UnityEngine;

namespace Script.UI
{
	public class Lifebar : MonoBehaviour
	{
		public Sprite greenBar;
		public Sprite yellowBar;
		public Sprite redBar;

		private float value = 1f;
		private GameObject bar;
		private SpriteRenderer barSpriteRenderer;
		private GameObject shadow;
		private SpriteRenderer shadowSpriteRenderer;

		private void Start()
		{
			bar = transform.Find("Bar").gameObject;
			barSpriteRenderer = bar.GetComponent<SpriteRenderer>();
			shadow = transform.Find("Shadow").gameObject;
			shadowSpriteRenderer = shadow.GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			var scale = transform.lossyScale;
			bar.transform.localScale = new Vector3(1f / scale.x, 1f / scale.y, 1f);
			barSpriteRenderer.size = new Vector2(value * scale.x, 0.26f * scale.y);
			shadow.transform.localScale = bar.transform.localScale;
			shadowSpriteRenderer.size = new Vector2(scale.x, 0.26f * scale.y);
			bar.transform.localPosition = new Vector3((value - 1) * 0.5f, 0f, 0f);
			if (value > 0.5f)
			{
				barSpriteRenderer.sprite = greenBar;
			}
			else if (value > 0.2f)
			{
				barSpriteRenderer.sprite = yellowBar;
			}
			else
			{
				barSpriteRenderer.sprite = redBar;
			}
		}

		public void SetValue(float newValue)
		{
			value = Math.Min(1f, Math.Max(0f, newValue));
		}
	}
}
