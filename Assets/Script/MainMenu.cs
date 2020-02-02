using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
	public class MainMenu : MonoBehaviour
	{
		public void NewGameClick()
		{
			Globals.Instance.survivedFlights = 0;
			Globals.Instance.timeAlive = 0f;
			SceneManager.LoadScene("PlanetScene");
		}

		public void CreditClick()
		{
			SceneManager.LoadScene("CreditsScene");
		}

		public void ExitClick()
		{
			Application.Quit();
		}
	}
}
