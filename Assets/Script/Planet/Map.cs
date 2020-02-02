using System;
using System.Collections.Generic;
using Script.Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Script.Planet
{
	public class Map : MonoBehaviour
	{
		public Pickup pickupPrefab;

		private Inventory inventory;
		private Text countdowmText;
		private float remainingTime;
		private AudioSource timerBeep;
		private AudioSource music;

		public void Start()
		{
			ChooseMap();
			SpawnPickups();
			RepairShip();

			var canvas = FindObjectOfType<Canvas>();
			countdowmText = canvas.transform.Find("Countdown").GetComponent<Text>();
			remainingTime = 120f; // 2 min
			inventory = canvas.GetComponentInChildren<Inventory>();

			var sources = GetComponents<AudioSource>();
			music = sources[0];
			timerBeep = sources[1];

			music.clip = Resources.Load<AudioClip>($"Music/Planet/{Random.Range(1, 5)}");
			music.Play();
		}

		private void ChooseMap()
		{
			var map = Random.Range(0, 5) + 1;
			var ways = transform.Find("Ways").GetComponent<SpriteRenderer>();
			var fg = transform.Find("FG").GetComponent<SpriteRenderer>();
			var fgCollider = fg.GetComponent<PolygonCollider2D>();
			ways.sprite = Resources.Load<Sprite>($"Map/Dorf {map}_Weg");
			fg.sprite = Resources.Load<Sprite>($"Map/Dorf {map}");

			var count = fg.sprite.GetPhysicsShapeCount();
			fgCollider.pathCount = count;
			for (var i = 0; i < count; i++)
			{
				var shape = new List<Vector2>();
				fg.sprite.GetPhysicsShape(i, shape);
				fgCollider.SetPath(i, shape);
			}
		}

		private void SpawnPickups()
		{
			var numPickups = Random.Range(25, 50);
			for (var i = 0; i < numPickups; i++)
			{
				var type = GetNewPickupType();
				var sprite = Item.GetSpriteForItem(type);
				var spriteSize = sprite.rect.size / sprite.pixelsPerUnit;
				while (true)
				{
					var x = Random.Range(-45f, 45f);
					var y = Random.Range(-25f, 25f);
					var hit = Physics2D.BoxCast(new Vector2(x, y), spriteSize, 0, Vector2.zero);
					if (hit.collider != null) continue;
					var pickup = Instantiate(pickupPrefab, new Vector3(x, y, 0), Quaternion.identity);
					pickup.pickupType = type;
					pickup.UpdateSprite();
					break;
				}
			}
		}

		private void RepairShip()
		{
			var globals = Globals.Instance;
			for (var i = 0; i < globals.shipComponentHealth.Length; i++)
			{
				globals.shipComponentHealth[i] = Mathf.Max(0.1f, globals.shipComponentHealth[i]);
			}
		}

		private ItemType GetNewPickupType()
		{
			var t1 = Random.value;
			var t2 = Random.value;
			if (t1 > 0.9f)
			{
				// spawn component
				return Item.ComponentTypes[Mathf.FloorToInt(t2 * Item.ComponentTypes.Length)];
			}
			else
			{
				// spawn part
				return Item.PartTypes[Mathf.FloorToInt(t2 * Item.PartTypes.Length)];
			}
		}

		private void Update()
		{
			var lastTime = remainingTime;
			remainingTime -= Time.deltaTime;
			Globals.Instance.timeAlive += Time.deltaTime;
			var sec = Mathf.Max(0, Mathf.FloorToInt(remainingTime % 60f));
			var min = Mathf.Max(0, Mathf.FloorToInt((remainingTime - sec) / 60f));
			countdowmText.text = $"Time until ship leaves\n{min:00}:{sec:00}";

			if ((lastTime >= 30f && remainingTime < 30f) ||
			    (lastTime >= 15f && remainingTime < 15f))
			{
				timerBeep.Play();
			}
			if (remainingTime <= 0 || Input.GetKeyDown(KeyCode.F12))
			{
				inventory.MoveItemsToShip();
				SceneManager.LoadScene("StartShipScene");
			}
		}
	}
}
