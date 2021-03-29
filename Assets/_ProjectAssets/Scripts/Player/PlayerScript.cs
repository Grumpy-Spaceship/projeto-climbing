// Maded by Pedro M Marangon
using Cinemachine;
using DG.Tweening;
using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{

	public class PlayerScript : MonoBehaviour
	{
		[SerializeField] private Transform feetPos = null;
		[SerializeField] private Transform punchPos = null;
		[SerializeField] private PlayerSettings settings = null;
		[SerializeField] private CinemachineVirtualCamera cmCam;
		[SerializeField] private bool showDebugGizmos = false;
		[ShowNonSerializedField] private int score;
		private float _moveInput;
		private float _grScale;
		private bool _canMove;
		private bool isPunching = false, canPunch = true;

		private bool isUpwards = false;
		private bool isDownwards = false;

		private Rigidbody2D _rb;

		//Freeze the rotation on Z axis and the X axis position (idle, specially on slope)
		private RigidbodyConstraints2D _FreezeRotationAndPosition => RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		//Freeze only the rotation on Z axis (moving)
		private RigidbodyConstraints2D _FreezeRotationOnly => RigidbodyConstraints2D.FreezeRotation;
		//Define if is in idle (i.e. not moving)
		private bool IsIdle => Mathf.Abs(_moveInput) <= 0;
		//Says what direction the player is facing (1 = Right, -1 = Left)
		public float FacingDirection => transform.localScale.x;
		
		public void EnableInput()
		{
			settings.Jump.EnableJump();
			_canMove = true;
		}
		public void DisableInput()
		{
			settings.Jump.DisableJump();
			_canMove = false;
			_moveInput = 0;
		}
		public void OnJump() => settings.Jump?.JumpPress();
		public void OnJumpRelease() => settings.Jump?.JumpRelease();
		public void OnMove(InputValue value) => _moveInput = _canMove ? value.Get<float>() : 0f;


		public void OnUpwards()
		{
			isUpwards = !isUpwards;

			var transposer = cmCam.GetCinemachineComponent<CinemachineFramingTransposer>();
			float value = isUpwards ? settings.UpScreenY : settings.NormalScreenY;
			DOVirtual.Float(transposer.m_ScreenY, value, settings.CamSmoothness, (float v) => transposer.m_ScreenY = v);


			punchPos.localPosition = isUpwards ? settings.PunchUpPos : settings.PunchNormalPos;

		}

		public void OnDownwards()
		{
			isDownwards = !isDownwards;

			var transposer = cmCam.GetCinemachineComponent<CinemachineFramingTransposer>();
			float value = isDownwards ? settings.DownScreenY : settings.NormalScreenY;

			DOVirtual.Float(transposer.m_ScreenY, value, settings.CamSmoothness, (float v) => transposer.m_ScreenY = v);

		}


		public void OnPunch()
		{
			if (!canPunch) return;

			if (!settings.Jump.IsGrounded && !isPunching)
			{
				var vel = _rb.velocity;
				Sequence s = DOTween.Sequence();
				s.AppendCallback(() => {
					isPunching = true;
					canPunch = false;
					_rb.gravityScale = 0.25f;
					_rb.velocity = Vector2.zero;
					punchPos.GetChild(0).gameObject.SetActive(true);
				});
				s.AppendCallback(Punch);
				s.AppendInterval(settings.StopTimeWhenPunchingAir);
				s.AppendCallback(() => {
					_rb.gravityScale = _grScale;
					_rb.velocity = vel;
					punchPos.GetChild(0).gameObject.SetActive(false);
					isPunching = false;
				});
				s.AppendInterval(settings.PunchCooldown);
				s.AppendCallback(() => canPunch = true);
			}
			else
			{
				Sequence s = DOTween.Sequence();
				s.AppendCallback(() => {
					isPunching = true;
					canPunch = false;
					punchPos.GetChild(0).gameObject.SetActive(true);
				});
				s.AppendCallback(Punch);
				s.AppendInterval(settings.StopTimeWhenPunchingAir);
				s.AppendCallback(() => {
					punchPos.GetChild(0).gameObject.SetActive(false);
					isPunching = false;
				});
				s.AppendInterval(settings.PunchCooldown);
				s.AppendCallback(() => canPunch = true);
			}
		}

		public void Punch()
		{
			if (!punchPos || !settings) return;
			Collider2D[] cols = Physics2D.OverlapCircleAll(punchPos.position, settings.PunchRadiusDetection, settings.BreakableTileMask);
			foreach (var col in cols)
			{
				if (col.TryGetComponent<BreakableTile>(out var tile))
					Destroy(tile.gameObject);
			}

		}
		public void StopYMovement() => _rb.velocity = new Vector2(_rb.velocity.x, 0);

		private void FixSpiderManSyndrome()
		{
			//If it has a Physics Material 2D, then return (don't need to add one)
			if (_rb.sharedMaterial || GetComponent<Collider2D>().sharedMaterial) return;
			//Create the Physics Material 2D
			PhysicsMaterial2D playerMat = new PhysicsMaterial2D();
			playerMat.friction = 0;
			playerMat.bounciness = 0;
			//Applied it to rigidbody
			_rb.sharedMaterial = playerMat;
		}
		private float Swap()
		{
			if (_moveInput > 0) return 1;
			else if (_moveInput < 0) return -1;
			else return transform.localScale.x;
		}
		private void UpdateAnimations()
		{
			
		//	//if there's no AnimationHandler or is playing a non-looping animation (like an attack), do nothing
		//	if (!anim || anim.IsPlayingNonLoopingAnimation) return;

		//	//JUMP/FALLING ANIMATION
		//	if (jump.IsJumping || _rb.velocity.y < 0)
		//	{
		//		string jumpAnim;
		//		if (_rb.velocity.y > 0) jumpAnim = "Jumping";
		//		else jumpAnim = "Falling";

		//		anim.PlayAnimation(jumpAnim, true);
		//	}
		//	//IDLE
		//	else if (IsIdle) anim.PlayAnimation("Idle", true);
		//	//EVERYTHING ELSE
		//	else
		//	{
		//		//Walk
		//		if (!IsIdle) anim.PlayAnimation("Walk", true);
		//	}
			
		}

		private void Awake()
		{
			Time.timeScale = 1;
			_rb = GetComponent<Rigidbody2D>();
			_grScale = _rb.gravityScale;

			settings.Jump?.SetupJumps();

			FixSpiderManSyndrome();

			EnableInput();

		}
		private void Update()
		{
			if (Time.timeScale == 0) return;


			UpdateAnimations();
		}
		private void FixedUpdate()
		{
			//Stop rotating and sliding down slopes if idle; if not, stop rotating
			_rb.constraints = IsIdle ? _FreezeRotationAndPosition : _FreezeRotationOnly;

			//Return if game is paused
			if (Time.timeScale == 0) return;
			//Move the player
			_rb.velocity = new Vector2(_moveInput * settings.MoveSpeed * Time.deltaTime, _rb.velocity.y);

			//Set correct direction
			transform.localScale = new Vector3(Swap(), transform.localScale.y, 1);

			settings.Jump?.JumpLogic(ref _rb, feetPos);


			if (settings.Jump.IsGrounded)
			{
				_rb.gravityScale = _grScale;
			}

			if (transform.position.y >= score)
			{
				score = Mathf.RoundToInt(transform.position.y);
			}


		}
		private void OnDrawGizmos()
		{
			if (!showDebugGizmos) return;

			settings.Jump?.DrawGizmos(feetPos);
			if (punchPos && settings)
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(punchPos.position, settings.PunchRadiusDetection);
			}
		}

	}


	[Serializable]
	public class PlayerJumpSystem
	{
		
		[SerializeField] private LayerMask whatIsGround = default;
		[SerializeField] public float groundCheckRadius = .25f;
		[SerializeField] public float jumpForce = 4;
		[SerializeField] public float jumpTime = 0.35f;
		[SerializeField] public int totalJumps = 1;
		private float _jumpTimeCounter;
		private int _extraJumps;
		private bool _grounded;
		private bool _pressingJump;


		public bool CanJump { get; private set; }
		public int JumpCount => _extraJumps;
		public LayerMask WhatIsGround => whatIsGround;
		public bool IsJumping => _pressingJump;
		public bool IsGrounded => _grounded;

		public void DisableJump() => CanJump = false;
		public void EnableJump() => CanJump = true;
		public void JumpLogic(ref Rigidbody2D rb, Transform feetPos)
		{
			//if can't jump, do nothing
			if (!CanJump) return;

			//Define if is on ground
			_grounded = Physics2D.OverlapCircle(feetPos.position, groundCheckRadius, whatIsGround);

			// Set the amount of extra jumps
			if (_grounded && !_pressingJump)
				_extraJumps = totalJumps;

			//If is jumping
			if (_pressingJump && _extraJumps >= 0)
			{
				//Decrease timer; if timer is valid, then add force upwards
				_jumpTimeCounter -= Time.deltaTime;
				if (_jumpTimeCounter > 0)
				{
					rb.velocity = new Vector2(rb.velocity.x, jumpForce);
				}
			}
			else _jumpTimeCounter = jumpTime;
		}
		public void JumpPress()
		{
			if (!CanJump) return;
			//Start pressing jump button
			_pressingJump = true;
			//Remove an extra jump
			_extraJumps--;
			//Set timer
			_jumpTimeCounter = jumpTime;
		}
		public void JumpRelease()
		{
			if (!CanJump) return;
			_pressingJump = false;
		}
		public void SetupJumps()
		{
			_extraJumps = totalJumps;
		}
		public void StopJumpOnCeiling()
		{
			_pressingJump = false;
			_jumpTimeCounter = -20;
		}

		public void DrawGizmos(Transform feetPos)
		{
			Gizmos.color = Color.red;
			if (feetPos == null) return;
			Gizmos.DrawWireSphere(feetPos.position, groundCheckRadius);
		}

	}

}