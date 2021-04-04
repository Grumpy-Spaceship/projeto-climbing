﻿// Maded by Pedro M Marangon
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using System.Configuration;
using UnityEngine;

namespace Game.Player
{
	[RequireComponent(typeof(PlayerScript))]
	public class PlayerAtk : MonoBehaviour
	{
		[SerializeField] private PlayerSettings settings = null;
		[Required, SerializeField] private PlayerScript player;
		[SerializeField] private Transform punchPos = null;
		[Required, SerializeField] private CinemachineVirtualCamera cmCam;
		private bool isPunching = false, canPunch = true;
		private bool isUpwards = false;
		private bool isDownwards = false;
		private Rigidbody2D _rb;
		private CinemachineFramingTransposer transposer;
		private float _grScale;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_grScale = _rb.gravityScale;
			transposer = cmCam.GetCinemachineComponent<CinemachineFramingTransposer>();
		}

		public void OnUpwards()
		{
			isUpwards = !isUpwards;
			float value = isUpwards ? settings.UpScreenY : settings.NormalScreenY;
			DOVirtual.Float(transposer.m_ScreenY, value, settings.CamSmoothness, (float v) => transposer.m_ScreenY = v);
		}
		public void OnDownwards()
		{
			isDownwards = !isDownwards;
			float value = isDownwards ? settings.DownScreenY : settings.NormalScreenY;
			DOVirtual.Float(transposer.m_ScreenY, value, settings.CamSmoothness, (float v) => transposer.m_ScreenY = v);
		}

		public void OnPunch()
		{
			if (!canPunch) return;

			punchPos.localPosition = settings.PunchNormalPos;
			ProcessPunch();
		}
		public void OnPunchUp()
		{
			if (!canPunch) return;

			punchPos.localPosition = settings.PunchUpPos;
			ProcessPunch();
		}
		private void ProcessPunch()
		{
			if (!settings.Jump.IsGrounded && !isPunching) AirPunch();
			else GroundPunch();
		}

		private void GroundPunch()
		{
			Sequence s = DOTween.Sequence();
			s.AppendCallback(() =>
			{
				isPunching = true;
				canPunch = false;
				punchPos.GetChild(0).gameObject.SetActive(true);
			});
			s.AppendCallback(Punch);
			s.AppendInterval(settings.PunchGFXShowTime);
			s.AppendCallback(() =>
			{
				punchPos.GetChild(0).gameObject.SetActive(false);
				isPunching = false;
			});
			s.AppendInterval(settings.PunchCooldown);
			s.AppendCallback(() => canPunch = true);
		}

		private void AirPunch()
		{
			var vel = _rb.velocity;
			Sequence s = DOTween.Sequence();
			//Configuracoes iniciais
			s.AppendCallback(() =>
			{
				isPunching = true;
				canPunch = false;
				settings.Jump.DisableJump();
				_rb.gravityScale = 0.25f;
				_rb.velocity = Vector2.zero;
			});
			//FALCON PUNCH
			s.AppendCallback(() =>
			{
				punchPos.GetChild(0).gameObject.SetActive(true);
				Punch();
			});
			s.AppendInterval(settings.PunchGFXShowTime);
			s.AppendCallback(() => punchPos.GetChild(0).gameObject.SetActive(false));
			s.AppendInterval(settings.StopTimeWhenPunchingAir);
			s.AppendCallback(() =>
			{
				_rb.gravityScale = _grScale;
				_rb.velocity = vel;
				isPunching = false;
				settings.Jump.EnableJump();
			});
			//Cooldown
			s.AppendInterval(settings.PunchCooldown);
			s.AppendCallback(() => canPunch = true);
		}

		public void Punch()
		{
			if (!punchPos || !settings) return;
			Collider2D[] cols = Physics2D.OverlapCircleAll(punchPos.position, settings.PunchRadiusDetection, settings.BreakableTileMask);


			if (cols.Length > 0)
			{
				CapsuleCollider2D caps = GetComponent<CapsuleCollider2D>();
				Vector2 pos = transform.position;
				pos.x -= player.FacingDirection * caps.size.x;

				Collider2D[] colliderBehind = Physics2D.OverlapBoxAll(pos, Vector2.up + (Vector2.right*.5f), 0);

				if(colliderBehind.Length <= 0)
					transform.DOMoveX((transform.position.x) - player.FacingDirection * settings.KnockbackForce, settings.KnockbackDur);
			}

			foreach (var col in cols)
			{
				if (col.TryGetComponent<BreakableTile>(out var tile))
					tile.Damage();
			}

		}


		private void FixedUpdate()
		{
			if (settings.Jump.IsGrounded) _rb.gravityScale = _grScale;
		}
		private void OnDrawGizmos()
		{
			if (punchPos && settings)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(punchPos.position, settings.PunchRadiusDetection);
			}
		}

	}

}