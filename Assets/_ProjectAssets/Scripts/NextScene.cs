//Maded by Pedro M Marangon
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
	public class NextScene : MonoBehaviour
	{
		[Scene] public string sceneName = "SampleScene";

		public void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				StartCoroutine(nameof(LoadNextScene));

				IEnumerator LoadNextScene()
				{
					Time.timeScale = 0;
					yield return new WaitForSecondsRealtime(1.5f);
					SceneManager.LoadScene(sceneName);
				}

			}
		}



	}
}