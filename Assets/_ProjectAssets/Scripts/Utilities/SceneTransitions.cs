// Maded by Pedro M Marangon
using NaughtyAttributes;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Player
{
	public class SceneTransitions : MonoBehaviour
	{
		[Scene,ShowNonSerializedField] private string currentScene = "";

		private void Awake() => currentScene = SceneManager.GetActiveScene().name;

		public void LoadScene(string scene) => SceneManager.LoadScene(scene);

		public void ReloadScene() => SceneManager.LoadScene(currentScene);

		public void ReloadScene(float delay) => StartCoroutine(LoadThisScene(delay, currentScene));
		public void LoadScene(string scene, float delay) => StartCoroutine(LoadThisScene(delay, scene));

		private IEnumerator LoadThisScene(float delay, string scene)
		{
			Time.timeScale = 0;
			yield return new WaitForSecondsRealtime(delay);
			SceneManager.LoadScene(scene);
		}

		public void QuitGame()
		{
			Application.Quit();
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#endif
		}


	}
}