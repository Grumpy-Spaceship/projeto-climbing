// Maded by Pedro M Marangon
using Game.Sounds;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
	public class PlayerSFX : MonoBehaviour
	{

		[TabGroup("Footsteps"), HideLabel, SerializeField] private SFX footstep;
		[TabGroup("Jump"), HideLabel, SerializeField] private SFX jump;
		[TabGroup("Death"), HideLabel, SerializeField] private SFX death;
		[TabGroup("Breaking"), HideLabel, SerializeField] private SFX breaking;

		public void PlayFS() => footstep?.PlaySFX();
		public void PlayJump() => jump?.PlaySFX();
		public void PlayBreak() => breaking?.PlaySFX();
		public void PlayDeath()
		{
			AudioSource s = death.Source;
			if (s)
			{
				s.transform.parent = null;
				Destroy(s, 1.5f);
			}
			death?.PlaySFX();
		}
	}
}