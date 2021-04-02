// Maded by Pedro M Marangon
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Game.Score
{
	public class PlayerScore : MonoBehaviour
	{
		#region SINGLETON
		public static PlayerScore instance;

		private void InitSingleton()
		{
			if (instance != null) Destroy(gameObject);

			instance = this;
		}
		#endregion

		[SerializeField] private int multiplier = 1;
		[ShowNonSerializedField] private int score = 0;
		[SerializeField] private TMP_Text updatedScoreCounter = null;
		[ShowNonSerializedField] private int maxYPlayer = 0;

		public string ScoreText => "Score: " + (score*multiplier);
		public int Score => score;
		public int MaxPlayerY => maxYPlayer;

		public static void AddScore(int amnt = 1) => SetScore(instance.score+amnt);
		public static void SetScore(int value)
		{
			instance.score = value;
			instance.UpdateText();
		}

		private void UpdateText() => updatedScoreCounter.text = ScoreText;

		public static void SetMaxPlayerY(float y) => instance.maxYPlayer = Mathf.RoundToInt(y);

		private void Awake()
		{
			InitSingleton();
			UpdateText();
			score = 0;
		}

	}
}