using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
	public class SwitchScene : MonoBehaviour
	{
		public string nextSceneName;
		public AudioClip clip;
		public float duration;

		private void Start()
		{
			var source = GetComponent<AudioSource>();
			source.PlayOneShot(clip);
			StartCoroutine(NextScene());
		}

		private IEnumerator NextScene()
		{
			yield return new WaitForSeconds(duration);
			SceneManager.LoadScene(nextSceneName);
		}
	}
}
