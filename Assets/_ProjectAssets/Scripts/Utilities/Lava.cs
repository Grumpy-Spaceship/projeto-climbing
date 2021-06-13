//Maded by Pedro M Marangon
using Game.Player;
using UnityEngine;

namespace Game.Movement
{
	public class Lava : TranslateWithVector
	{
		[Range(0,1), SerializeField] private float velocityDecreaser = 0.5f;
		private Transform _t;
		private PlayerScript _player;

		protected override void Awake()
		{
			_player = FindObjectOfType<PlayerScript>();
			_t = transform;

			GetComponent<KillPlayerOnContact>().FindUI();

			base.Awake();
		}

		protected override void Update()
		{
			base.Update();
		}

		public void StartRising() => canMove = true;

		protected override void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (Time.timeScale != 0 && correctTypeOfUpdate && canMove)
				transform.Translate(speed * GetDistance() * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
		}

		private float GetDistance() => (_player.transform.position.y - _t.position.y) * velocityDecreaser;
	}
}