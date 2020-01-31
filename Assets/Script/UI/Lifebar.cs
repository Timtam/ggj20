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

		private void Start()
		{
			bar = transform.Find("Bar").gameObject;
			barSpriteRenderer = bar.GetComponent<SpriteRenderer>();
		}

		private void Update()
		{
			barSpriteRenderer.size = new Vector2(value, 0.26f);
			bar.transform.localPosition = new Vector3(-0.5f + value / 2f, 0f, 0f);
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
