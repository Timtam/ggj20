using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Planet
{
	public class Player : MonoBehaviour
	{
		private GameObject playerSprite;

		// Start is called before the first frame update
		private void Start()
		{
			playerSprite = transform.Find("Robot").gameObject;
		}

		// Update is called once per frame
		private void Update()
		{
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");
			var move = new Vector3(x, y, 0);
			move.Normalize();
			transform.Translate(5 * Time.deltaTime * move);
			if (move.sqrMagnitude > 0)
			{
				playerSprite.transform.rotation =
					Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.right, move, Vector3.forward), Vector3.forward);
			}
		}
	}
}
