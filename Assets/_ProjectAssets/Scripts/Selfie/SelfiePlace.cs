// Maded by Pedro M Marangon
using DG.Tweening;
using Game.Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Game.Selfie
{
    public class SelfiePlace : MonoBehaviour
	{

		[ChildGameObjectsOnly, SerializeField] private new Light2D light = null;
		[Range(1,2), SerializeField] private float intensity = 0.5f;
		[SuffixLabel("s"), SerializeField] private float duration = 0.2f;
		[SerializeField] private bool destroy = true;

		public bool DestroyOnExit => destroy;

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.TryGetComponent<PlayerScript>(out var player))
			{
				player.CanSelfie(true, this);
				DOVirtual.Float(1, intensity, duration, UpdateLightRadius);
			}
		}

		private void UpdateLightRadius(float value) => light.pointLightOuterRadius = value;

		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.TryGetComponent<PlayerScript>(out var player))
			{
				player.CanSelfie(false, null);
				DOVirtual.Float(intensity, 1, duration, UpdateLightRadius);
			}
		}

	}
}
