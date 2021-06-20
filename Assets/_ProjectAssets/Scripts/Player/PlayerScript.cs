// Maded by Pedro M Marangon
using DG.Tweening;
using Game.Score;
using Game.Selfie;
using PedroUtilities;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Player
{

	public class PlayerScript : MonoBehaviour
	{
		[TabGroup("References"), ChildGameObjectsOnly, SerializeField] private Transform feetPos = null;
		[TabGroup("References"), ChildGameObjectsOnly, SerializeField] private Transform objToScale = null;
		[TabGroup("References"), ChildGameObjectsOnly, SerializeField] private Transform punchLPos = null;
		[TabGroup("References"), ChildGameObjectsOnly, SerializeField] private Transform punchRPos = null;
		[TabGroup("References"), ChildGameObjectsOnly, SerializeField] private Transform arm = null;
		[TabGroup("Settings"), HideLabel, SerializeField] private PlayerSettings settings = null;
		[TabGroup("Settings"), HideLabel, SerializeField] private AnimationHandler anim = null;
		[TabGroup("Settings"), HideLabel, SerializeField] private PlayerSFX sfx = null;
		[TabGroup("Settings"), SceneObjectsOnly, SerializeField] private AudioSource musicSource = null;
		[TabGroup("Settings"), SceneObjectsOnly, SerializeField] private AudioSource lavaSource = null;
		[TabGroup("Options"), SerializeField] private bool showDebugGizmos = false, useScore = true;
		private float _moveInput;
		private bool _canMove;
		private bool _canSelfie;
		private SelfiePlace _place;
		private Rigidbody2D _rb;

		//Freeze the rotation on Z axis and the X axis position (idle, specially on slope)
		private RigidbodyConstraints2D _FreezeRotationAndPosition => RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionX;
		//Freeze only the rotation on Z axis (moving)
		private RigidbodyConstraints2D _FreezeRotationOnly => RigidbodyConstraints2D.FreezeRotation;
		//Define if is in idle (i.e. not moving)
		private bool IsIdle => Mathf.Abs(_moveInput) <= 0;
		//Says what direction the player is facing (1 = Right, -1 = Left)
		public float FacingDirection => objToScale.localScale.x;
		
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
			if (settings.Jump.IsGrounded)
			{
				sfx?.PlayJump();
				GameObject particles = settings.Jump.GetParticles(feetPos);
				if(particles) Instantiate(particles, feetPos.position, Quaternion.identity);
			}

			settings.Jump?.JumpPress();
		}

		public void Kill()
		{
			sfx?.PlayDeath();
			musicSource?.Stop();
			lavaSource?.Stop();
			Destroy(gameObject);
		}

		public void CanSelfie(bool can, SelfiePlace place)
		{
			_canSelfie = can;
			_place = place;
		}

		public void OnSelfie()
		{
			if (!_canSelfie) return;

			SelfieManager.instance.TakeSelfie(_place);
		}

		public void OnAnyKey() => SelfieManager.instance.ExitSelfie();

		public void OnJumpRelease() => settings.Jump?.JumpRelease();
		public void OnMove(InputValue value)
		{
			_moveInput = _canMove ? value.Get<float>() : 0f;
		}

		public void StopYMovement() => _rb.velocity = new Vector2(_rb.velocity.x, 0);
		public void StopMovement()
		{
			_rb.velocity = Vector2.zero;
			_moveInput = 0;
		}

		public void Flip()
		{
			//Set correct direction
			objToScale.localScale = new Vector3(Swap(), objToScale.localScale.y, 1);
			if (FacingDirection == 1)
			{
				arm.localPosition = punchRPos.localPosition;
				arm.GetChild(0).localScale = Vector3.one * 1.15f;
				arm.GetChild(0).DOLocalRotate(Vector3.forward * -11.75f, 0);
			}
			else
			{
				arm.localPosition = punchLPos.localPosition;
				arm.GetChild(0).localScale = new Vector3(1.15f, -1.15f, 1.15f);
				arm.GetChild(0).DOLocalRotate(Vector3.forward * 11.75f, 0);
			}
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
		private float Swap() => -FacingDirection;
		private void UpdateAnimations()
		{

			//if there's no AnimationHandler or is playing a non-looping animation (like an attack), do nothing
			if (!anim || anim.IsPlayingNonLoopingAnimation) return;

			//JUMP/FALLING ANIMATION
			if (settings.Jump.IsJumping || _rb.velocity.y < -0.05f)
			{
				anim.PlayAnimation("Jump", true);
			}
			//IDLE
			else if (IsIdle) anim.PlayAnimation("Idle", true);
			//EVERYTHING ELSE
			else
			{
				//Walk
				if (!IsIdle)
				{
					bool runningTwrdsMouse = PlayerIsRunningTowardsMouse();
					string run = $"Run {(runningTwrdsMouse ? "Forward" : "Backward")}";
					anim.PlayAnimation(run, true);
				}
			}

		}

		private bool PlayerIsRunningTowardsMouse()
		{

			bool scaleEquals1 = objToScale.localScale.x == 1;
			bool goingToRight = _moveInput > 0;

			bool scaleEqualsMinus1 = objToScale.localScale.x == -1;
			bool goingToLeft = _moveInput < 0;


			bool result = (scaleEquals1 && goingToRight) || (scaleEqualsMinus1 && goingToLeft);

			return result;
		}

		private void Move() => _rb.velocity = new Vector2(_moveInput * settings.MoveSpeed * Time.deltaTime, _rb.velocity.y);

		private void Awake()
		{
			Time.timeScale = 1;
			_rb = GetComponent<Rigidbody2D>();
			objToScale = objToScale ? objToScale : transform;

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
			Move();
			settings.Jump?.JumpLogic(ref _rb, feetPos);



			if (useScore && transform.position.y >= PlayerScore.instance.MaxPlayerY)
			{
				int val = Mathf.Abs(Mathf.RoundToInt(transform.position.y) - PlayerScore.instance.MaxPlayerY);
				PlayerScore.SetMaxPlayerY(transform.position.y);
				PlayerScore.AddScore(val);
			}
		}
		private void OnDrawGizmos()
		{
			if (!showDebugGizmos) return;

			settings.Jump?.DrawGizmos(feetPos);
		}
		private void OnCollisionEnter2D(Collision2D other)
		{
			if (other.gameObject.IsInsideLayerMask(settings.Jump.WhatIsGround) &&
				other.transform.position.y < feetPos.position.y)
			{
				StopYMovement();
				OnJumpRelease();
				anim.PlayAnimation("Idle", true);
			}
		}
	}


	[Serializable]
	public class PlayerJumpSystem
	{
		
		[SerializeField] private LayerMask whatIsGround = default;
		[SerializeField] private GameObject particles = null;
		[SerializeField] private GameObject particles_Breakable = null;
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

		public GameObject GetParticles(Transform feetPos)
		{
			Collider2D hit = Physics2D.OverlapCircle(feetPos.position, groundCheckRadius, whatIsGround);


			if (!hit) return null;

			if (hit.TryGetComponent<BreakableTile>(out var _)) return particles_Breakable;
			else return particles;
		}

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
			_pressingJump = false;
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