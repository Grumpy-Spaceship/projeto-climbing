// Maded by Pedro M Marangon
using DG.Tweening;
using Game.Health;
using Game.Player;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Enemies
{
	public class Enemy : MonoBehaviour, IDamageable
	{
		[TabGroup("Health"), SerializeField] private int maxHealth = 2;
		[TabGroup("Health"), ProgressBar(0,"maxHealth",r: 1, g: .2f, b: .3f), HideLabel, ReadOnly, SerializeField]private int health = 0;
		[TabGroup("Health"), AssetsOnly, SerializeField] private GameObject deathParticles;
		[TabGroup("Ground Check"), ChildGameObjectsOnly, SerializeField] private Transform groundCheckPos;
		[TabGroup("Ground Check"), SerializeField] private float groundCheckDist = 0.5f;
		[TabGroup("Ground Check"), SerializeField] private LayerMask whatIsGround = default;
		[SerializeField] private Animator anim;
		[SerializeField] private float moveSpeed = 5;
		[SerializeField] private float stopTime = 1;
		private Rigidbody2D _rb;
		private Vector3 baseScale;
		private float facingDir = 1;
		private float speed;

		public int HP => health;
		public int MaxHP => maxHealth;
		public float HP_Percent => (float)HP/(float)MaxHP;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			baseScale = transform.localScale;
			health = maxHealth;
			speed = anim.speed = moveSpeed;
			GetComponentInChildren<KillPlayerOnContact>().FindUI();
		}
		private void FixedUpdate()
		{
			_rb.velocity = new Vector2(speed * facingDir, _rb.velocity.y);

			bool hitWall = IsHittingWall();
			bool nearEdge = IsNearEdge();

			if (_rb.velocity.y>=0 && (hitWall || !nearEdge))
			{
				ChangeDirection();
			}

		}
		private void ChangeDirection()
		{
			bool rightToLeft = transform.localScale.x > 0;

			Vector3 scale = baseScale;
			scale.x = rightToLeft ? -1 : 1;

			transform.localScale = scale;
			facingDir = scale.x;
		}
		private bool IsHittingWall()
		{

			float castDist = groundCheckDist * facingDir;

			Vector3 targetPos = groundCheckPos.position;
			targetPos.x += castDist;
			Debug.DrawLine(groundCheckPos.position, targetPos);

			RaycastHit2D raycastHit2D = Physics2D.Linecast(groundCheckPos.position, targetPos, whatIsGround);
			Debug.DrawLine(groundCheckPos.position, targetPos, raycastHit2D ? Color.red : Color.green);
			return raycastHit2D;
		}
		private bool IsNearEdge()
		{
			float castDist = groundCheckDist;

			Vector3 targetPos = groundCheckPos.position;
			targetPos.y -= castDist;
			RaycastHit2D raycastHit2D = Physics2D.Linecast(groundCheckPos.position, targetPos, whatIsGround);
			Debug.DrawLine(groundCheckPos.position, targetPos,raycastHit2D?Color.green:Color.red);
			return raycastHit2D;
		}

		public void Damage(int amnt = 1)
		{
			SetHP(health - amnt);
			anim.Play("hit", 1);
			anim.Play("Stop", 0);
			speed = 0;
			Invoke(nameof(BackToMove), stopTime);

		}
		private void BackToMove()
		{
			speed = moveSpeed;
			anim.Play("Walk", 0);
		}

		public void Heal(int amnt = 1) => SetHP(health + amnt);

		public void SetHP(int value)
		{
			health = Mathf.Clamp(value, 0, maxHealth);
			if (health <= 0) Die();
		}

		public void Die()
		{
			if(deathParticles) Instantiate(deathParticles, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}