using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
	public class MainMenu : MonoBehaviour
	{
		public void NewGameClick()
		{
			SceneManager.LoadScene("PlanetScene");
		}
	}
}
