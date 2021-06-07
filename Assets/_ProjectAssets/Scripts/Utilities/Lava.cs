//Maded by Pedro M Marangon
using Game.Player;
using Game.Score;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Movement
{
	public class Lava : TranslateWithVector
	{
		[TabGroup("Lava"), Range(0,1), SerializeField] private float velocityDecreaser = 0.5f;
		[TabGroup("Lava"), SerializeField] private int minimumScoreToRise = 50;
		private Transform _t;
		private PlayerScript _player;

		protected override void Awake()
		{
			_player = FindObjectOfType<PlayerScript>();
			_t = transform;

			base.Awake();
		}

		protected override void Update()
		{
			base.Update();

			canMove = PlayerScore.Score > minimumScoreToRise;
		}

		protected override void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (Time.timeScale != 0 && correctTypeOfUpdate && canMove)
				transform.Translate(speed * GetDistance() * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
		}

		private float GetDistance() => (_player.transform.position.y - _t.position.y) * velocityDecreaser;
	}
}