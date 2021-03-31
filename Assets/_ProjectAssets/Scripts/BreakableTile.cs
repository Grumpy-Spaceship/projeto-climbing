// Maded by Pedro M Marangon
using Game.Health;
using NaughtyAttributes;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class BreakableTile : MonoBehaviour, IDamageable
	{

		[MinValue(1),MaxValue(10),SerializeField] private int maxHP;
		[SerializeField] private SpriteRenderer breakRend;
		private int hp;

		public int HP => hp;
		public int MaxHP => throw new System.NotImplementedException();
		public float HP_Percent => (float)hp / (float)maxHP;

		private void Awake() => SetHP(maxHP);

		public void Damage(int amnt = 1) => SetHP(hp - amnt);

		public void Die() => Destroy(gameObject);

		public void Heal(int amnt = 1) => SetHP(hp + amnt);

		public void SetHP(int value)
		{
			hp = Mathf.Clamp(value, 0, maxHP);

			breakRend.color = new Color(breakRend.color.r, breakRend.color.g, breakRend.color.b, 1-HP_Percent);

			if (hp <= 0) Die();
		}

	}
}