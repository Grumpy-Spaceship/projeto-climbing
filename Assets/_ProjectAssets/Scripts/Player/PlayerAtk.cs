// Maded by Pedro M Marangon
using Cinemachine;
using DG.Tweening;
using Game.Health;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{
	[RequireComponent(typeof(PlayerScript))]
	public class PlayerAtk : MonoBehaviour
	{
		[BoxGroup("Player"), SerializeField] private PlayerSettings settings = null;
		[ChildGameObjectsOnly, BoxGroup("Player"), Required, SerializeField] private PlayerScript player;
		[ChildGameObjectsOnly, FoldoutGroup("Punch transforms"), SerializeField] private Transform punchPos = null, punchRotator = null;
		[SceneObjectsOnly, FoldoutGroup("External Components"), Required, SerializeField] private CinemachineVirtualCamera cmCam;
		[ChildGameObjectsOnly, FoldoutGroup("External Components"), SerializeField] private CinemachineImpulseSource impulseSource = null;
		private Rigidbody2D _rb;
		private CinemachineFramingTransposer transposer;
		private PlayerInput _pInput;
		private bool isPunching = false, canPunch = true;
		private bool isUpwards = false;
		private Vector2 aimDir;
		private float _grScale;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			_grScale = _rb.gravityScale;
			transposer = cmCam.GetCinemachineComponent<CinemachineFramingTransposer>();
			_pInput = GetComponent<PlayerInput>();
		}

		public void OnUpwards()
		{
			isUpwards = !isUpwards;
			float value = isUpwards ? settings.UpScreenY : settings.NormalScreenY;
			DOVirtual.Float(transposer.m_ScreenY, value, settings.CamSmoothness, (float v) => transposer.m_ScreenY = v);
		}
		public void OnPunch()
		{
			if (!canPunch) return;

			punchPos.localPosition = settings.PunchNormalPos;
			ProcessPunch();
		}
		public void OnAimPunch(InputValue v)
		{
			//TODO: Get value
			aimDir = v.Get<Vector2>();
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

				if(colliderBehind.Length <= 0 && punchPos.position.y < transform.position.y + (settings.PunchRadiusDetection))
					transform.DOMoveX((transform.position.x) - player.FacingDirection * settings.KnockbackForce, settings.KnockbackDur);

				impulseSource.GenerateImpulse();

			}

			foreach (var col in cols)
			{
				if (col.TryGetComponent<IDamageable>(out var tile))
					tile.Damage();
			}

		}


		float angle;
		private void Update()
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(aimDir);
			Vector3 dir = pos - transform.position;
			angle = SetAngle(dir.normalized);
			punchRotator.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}

		private float SetAngle(Vector3 vector3)
		{
			float v = Mathf.Atan2(vector3.y, vector3.x) * Mathf.Rad2Deg;

			#region Flip the player if aiming behind
			bool facingLeftAndAimingRight = vector3.x < 0 && player.FacingDirection > 0;
			bool facingRightAndAimingLeft = vector3.x > 0 && player.FacingDirection < 0;
			if(facingLeftAndAimingRight || facingRightAndAimingLeft) player?.Flip();
			#endregion
			
			return v;
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