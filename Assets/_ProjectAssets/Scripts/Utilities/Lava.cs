﻿//Maded by Pedro M Marangon
using Game.Player;
using Game.Score;
using UnityEngine;

namespace Game.Movement
{
	public class Lava : TranslateWithVector
	{
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

			canMove = PlayerScore.Score > 0;
		}

		protected override void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (Time.timeScale != 0 && correctTypeOfUpdate && canMove)
			{
				transform.Translate(speed * GetDistance() * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
			}
		}

		private float GetDistance() => Vector3.Distance(_player.transform.position, _t.position)*0.5f;
	}
}