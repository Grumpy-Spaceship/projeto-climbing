//Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Health
{
	public interface IDamageable
	{

		int HP { get; }
		int MaxHP { get; }
		float HP_Percent { get; }

		void Damage(int amnt = 1);
		void Heal(int amnt = 1);
		void SetHP(int value);
		void Die();

	}
}