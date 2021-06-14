// Maded by Pedro M Marangon
using Game.Movement;
using Sirenix.OdinInspector;
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
		[SceneObjectsOnly, SerializeField] private TMP_Text updatedScoreCounter = null;
		[ReadOnly, SerializeField] private int score = 0;
		[ReadOnly, SerializeField] private int maxYPlayer = 0;
		[SerializeField] private Lava lava;
		[SerializeField] private int scrToIncreaseLavaSpeed = 40;
		[Range(1,2), SerializeField] private float lavaSpeedIncrease = 1.15f;

		public string ScoreText => "Score: " + (score*multiplier);
		public static int Score => instance.score;
		public int MaxPlayerY => maxYPlayer;

		public static void AddScore(int amnt = 1) => SetScore(instance.score+amnt);
		public static void SetScore(int value)
		{
			instance.score = value;
			instance.UpdateText();


			bool isDivisible = ((float)instance.score % (float)instance.scrToIncreaseLavaSpeed == 0);
			if (instance.lava.canMove && isDivisible && instance.lava.speed < 0.5f)
			{
				instance.lava.speed = Mathf.Clamp(instance.lava.speed * instance.lavaSpeedIncrease, 0, 0.5f);
			}

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