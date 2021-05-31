// Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game
{

	public class AnimationHandler : MonoBehaviour
	{

		[Required, SerializeField] private Animator animator = null;
		[HorizontalGroup("Prefix"), EnableIf("usePrefix"), SerializeField] private string prefix = "";
		[HideLabel, HorizontalGroup("Prefix"), SerializeField] private bool usePrefix = false;
		[HorizontalGroup("Sufix"), EnableIf("useSufix"), SerializeField] private string sufix = "";
		[HideLabel, HorizontalGroup("Sufix"), SerializeField] private bool useSufix = false;


		public bool IsPlayingNonLoopingAnimation { get; private set; }


		public void PlayAnimation(string animationName, bool loopable, int layer = 0)
		{
			//If the animator isn't assigned, do nothing
			if (!animator) return;

			/*
			 Set the IsPlayingNonLoopingAnimation to the inverse of the loopable parameter (since the property is asking if it's
			 NOT playing a loopable one ).
			 TL;DR if the animation is loopable, IsPlayingNonLoopingAnimation = false, and vice versa
			*/
			IsPlayingNonLoopingAnimation = !loopable;

			//Generates a composite name with the prefix and sufix setted in the inspector
			string name = (usePrefix ? prefix : "") + animationName + (useSufix ? sufix : "");

			//Only play the animation if a) the animator has the animation, and b) it isn't already the animation
			if (animator.HasState(layer, Animator.StringToHash(name)) && !animator.GetCurrentAnimatorStateInfo(0).IsName(name)) animator?.Play(name, layer);
		}

	}

}