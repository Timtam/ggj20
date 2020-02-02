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
