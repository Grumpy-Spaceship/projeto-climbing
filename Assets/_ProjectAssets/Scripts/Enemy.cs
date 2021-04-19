// Maded by Pedro M Marangon
using Game.Health;
using Game.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Enemies
{
	public class Enemy : MonoBehaviour, IDamageable
	{
		[SerializeField] private int maxHealth = 2;
		[SerializeField] private Transform groundCheckPos;
		[SerializeField] private float groundCheckDist = 0.5f;
		[SerializeField] private float moveSpeed = 5;
		[SerializeField] private LayerMask whatIsGround = default;
		private Rigidbody2D _rb;
		private Vector3 baseScale;
		private float facingDir = 1;
		private int health = 0;

		public int HP => health;
		public int MaxHP => maxHealth;
		public float HP_Percent => (float)HP/(float)MaxHP;

		private void Awake()
		{
			_rb = GetComponent<Rigidbody2D>();
			baseScale = transform.localScale;
			health = maxHealth;
			GetComponentInChildren<KillPlayerOnContact>().FindUI();
		}
		private void FixedUpdate()
		{
			_rb.velocity = new Vector2(moveSpeed * facingDir, 0);

			if ((IsHittingWall() || !IsNearEdge()))
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
			Debug.Log(_rb.velocity.y >= 0,this);
		}
		private bool IsHittingWall()
		{

			float castDist = groundCheckDist * facingDir;

			Vector3 targetPos = groundCheckPos.position;
			targetPos.x += castDist;
			Debug.DrawLine(groundCheckPos.position, targetPos);

			return Physics2D.Linecast(groundCheckPos.position, targetPos, whatIsGround);
		}
		private bool IsNearEdge()
		{

			float castDist = groundCheckDist;

			Vector3 targetPos = groundCheckPos.position;
			targetPos.y -= castDist;
			Debug.DrawLine(groundCheckPos.position, targetPos);
			return Physics2D.Linecast(groundCheckPos.position, targetPos, whatIsGround);
		}

		public void Damage(int amnt = 1) => SetHP(health - amnt);
		public void Heal(int amnt = 1) => SetHP(health + amnt);

		public void SetHP(int value)
		{
			health = Mathf.Clamp(value, 0, maxHealth);
			if (health <= 0) Die();
		}

		public void Die()
		{
			Destroy(gameObject);
		}
	}
}