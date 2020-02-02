using System;
using System.Collections;
using System.Collections.Generic;
using Script.Items;
using UnityEngine;

namespace Script.Planet
{
	public class Player : MonoBehaviour
	{
		private GameObject playerSprite;
		private AudioSource robotMove;
		private AudioSource robotStart;
		private AudioSource robotStop;
		private AudioSource collectAudio;
		private Rigidbody2D body;
		private Inventory inventory;
		private Vector2 move = Vector2.zero;
		private bool isMoving;
		private const float MoveSpeed = 5f;

		private void Start()
		{
			playerSprite = transform.Find("Robot").gameObject;
			var sources = playerSprite.GetComponents<AudioSource>();
			robotMove = sources[0];
			robotStart = sources[1];
			robotStop = sources[2];
			body = GetComponent<Rigidbody2D>();
			inventory = FindObjectOfType<Inventory>();
			collectAudio = GetComponent<AudioSource>();
		}

		private void Update()
		{
			var x = Input.GetAxisRaw("Horizontal");
			var y = Input.GetAxisRaw("Vertical");
			move = new Vector2(x, y);
			move.Normalize();
			if (move.sqrMagnitude > 0)
			{
				playerSprite.transform.rotation =
					Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.up, move, Vector3.forward), Vector3.forward);
				if (!isMoving)
				{
					robotStart.Play();
					robotMove.PlayDelayed(0.12f);
				}
				isMoving = true;
			}
			else if (isMoving)
			{
				robotStop.Play();
				robotMove.Stop();
				robotStart.Stop();
				isMoving = false;
			}
		}

		private void FixedUpdate()
		{
			body.velocity = MoveSpeed * move;
		}

		private void OnCollisionEnter2D(Collision2D other)
		{
			var pickup = other.gameObject.GetComponent<Pickup>();
			if (pickup != null)
			{
				if (inventory.StoreItem(pickup.pickupType))
				{
					if (Item.GetItemInventorySize(pickup.pickupType) == 1)
					{
						collectAudio.PlayOneShot(Resources.Load<AudioClip>("Sounds/Planet/collect"));
					}
					else
					{
						collectAudio.PlayOneShot(Resources.Load<AudioClip>("Sounds/Planet/collect_big"));
					}
					Destroy(pickup.gameObject);
				}
				return;
			}

			if (other.gameObject.name == "Ship")
			{
				if (!inventory.IsEmpty)
				{
					collectAudio.PlayOneShot(Resources.Load<AudioClip>("Sounds/Planet/transfer"));
					inventory.MoveItemsToShip();
				}
			}
		}
	}
}
