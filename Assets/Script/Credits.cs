using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
	public class Credits : MonoBehaviour
	{
		private void Update()
		{
			if (Input.GetButton("Cancel"))
			{
				SceneManager.LoadScene("MainMenu");
			}
		}
	}
}
