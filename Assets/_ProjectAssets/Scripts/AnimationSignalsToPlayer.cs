// Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Player
{

	public class AnimationSignalsToPlayer : MonoBehaviour
	{

		[SerializeField] private PlayerScript playerControls = null;
		[SerializeField] private PlayerAtk playerCombat = null;
		[SerializeField] private AnimationHandler animationHandler = null;
		//	[SerializeField] private PlayerHealth playerHealth = null;


		public void PlayFS()
		{

		}

		public void EnableInput()
		{
			playerControls?.EnableInput();
			animationHandler?.PlayAnimation("Idle", true);
		}

		public void DisableInput()
		{
			playerControls.DisableInput();
		}

	}

}