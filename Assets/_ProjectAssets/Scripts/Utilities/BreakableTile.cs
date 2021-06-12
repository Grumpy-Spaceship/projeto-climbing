// Maded by Pedro M Marangon
using Game.Health;
using Game.Score;
using Game.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class BreakableTile : MonoBehaviour, IDamageable
	{


		[FoldoutGroup("Health"), Min(0), MaxValue(10), SerializeField] private int maxHP;
		[FoldoutGroup("Health"), ProgressBar(0, "maxHP", r: 1, g: .2f, b: .3f), HideLabel, ReadOnly, SerializeField] private int hp;
		[MinValue(0), SerializeField] private int scoreAmnt = 1;
		[ChildGameObjectsOnly, SerializeField] private SpriteRenderer breakRend;
		[SerializeField] private SFX destroy;

		public int HP => hp;
		public int MaxHP => throw new System.NotImplementedException();
		public float HP_Percent => (float)hp / (float)maxHP;

		private void Awake() => SetHP(maxHP);

		public void Damage(int amnt = 1) => SetHP(hp - amnt);

		public void Die()
		{
			PlayerScore.AddScore(scoreAmnt);

			AudioSource s = destroy.Source;
			if (s)
			{
				s.transform.parent = null;
				Destroy(s, 1.5f);
			}
			destroy?.PlaySFX();

			Destroy(gameObject);
		}

		public void Heal(int amnt = 1) => SetHP(hp + amnt);

		public void SetHP(int value)
		{
			hp = Mathf.Clamp(value, 0, maxHP);

			breakRend.color = new Color(breakRend.color.r, breakRend.color.g, breakRend.color.b, 1-HP_Percent);

			if (hp <= 0) Die();
		}

	}
}