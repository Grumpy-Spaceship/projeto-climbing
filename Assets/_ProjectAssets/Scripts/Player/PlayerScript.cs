// Maded by Pedro M Marangon
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
		[SerializeField] private bool showDebugGizmos = false;
		private float _moveInput;
		private bool _canMove;
		private Rigidbody2D _rb;

		//Freeze the rotation on Z axis and the X axis position (idle, specially on slope)
		private RigidbodyConstraints2D _FreezeRotationAndPosition => RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		//Freeze only the rotation on Z axis (moving)
		private RigidbodyConstraints2D _FreezeRotationOnly => RigidbodyConstraints2D.FreezeRotation;
		//Define if is in idle (i.e. not moving)
		private bool IsIdle => Mathf.Abs(_moveInput) <= settings.MoveThreshold;
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
		public void OnJump()
		{
			if (settings.Jump.CanJump && settings.Jump.IsGrounded)
				_rb.AddForce(Vector2.up * settings.Jump.jumpForce, ForceMode2D.Impulse);
		}
		public void OnMove(InputValue value) => _moveInput = _canMove ? value.Get<float>() : 0f;
		public void OnPunch() => Punch();

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
		public void StopYMovement()
		{
			_rb.velocity = new Vector2(_rb.velocity.x,0);
		}

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
			if (_moveInput > settings.MoveThreshold) return 1;
			else if (_moveInput < -settings.MoveThreshold) return -1;
			else return transform.localScale.x;
		}
		private void UpdateAnimations()
		{
			/*
			//if there's no AnimationHandler or is playing a non-looping animation (like an attack), do nothing
			if (!anim || anim.IsPlayingNonLoopingAnimation) return;

			// JUMP/FALLING ANIMATION
			if (jump.IsJumping || _rb.velocity.y < 0)
			{
				string jumpAnim;
				if (_rb.velocity.y > 0) jumpAnim = "Jumping";
				else jumpAnim = "Falling";

				anim.PlayAnimation(jumpAnim, true);
			}
			//IDLE
			else if (IsIdle) anim.PlayAnimation("Idle", true);
			//EVERYTHING ELSE
			else
			{
				//Walk
				if (!IsIdle) anim.PlayAnimation("Walk", true);
			}
			*/
		}

		private void Awake()
		{
			Time.timeScale = 1;
			_rb = GetComponent<Rigidbody2D>();

			FixSpiderManSyndrome();

			EnableInput();
		}
		private void Update()
		{
			if (Time.timeScale == 0) return;

			settings.Jump?.JumpLogic(feetPos);

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
		private bool _grounded;

		// Get if can jump
		public bool CanJump { get; private set; }
		// Get if can jump
		public bool IsGrounded => _grounded;
		// Get the layermask of what is ground
		public LayerMask WhatIsGround => whatIsGround;


		public void DisableJump() => CanJump = false;
		public void EnableJump() => CanJump = true;
		public void JumpLogic(Transform feetPos)
		{
			//if can't jump, do nothing
			if (!CanJump) return;

			//Define if is on ground
			_grounded = Physics2D.OverlapCircle(feetPos.position, groundCheckRadius, whatIsGround);
		}

		public void DrawGizmos(Transform feetPos)
		{
			Gizmos.color = Color.red;
			if (feetPos == null) return;
			Gizmos.DrawWireSphere(feetPos.position, groundCheckRadius);
		}

	}

}