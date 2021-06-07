// Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Sounds
{
	public class SFXManager : MonoBehaviour
	{

		#region SINGLETON
		private static SFXManager _instance;
		public static SFXManager instance
		{
			get
			{
				if (_instance == null)
					_instance = FindObjectOfType<SFXManager>();
				return _instance;
			}
		}
		#endregion

		[HorizontalGroup("AudioSource"), SerializeField] private AudioSource defaultAudioSource;

		[TabGroup("UI")] public List<SFXClip> uiSFX;
		[TabGroup("Ambient")] public List<SFXClip> ambientSFX;
		[TabGroup("Weapons")] public List<SFXClip> weaponSFX;

		public static void PlaySFX(SFXClip sfx, bool waitForFinish = true, AudioSource audioSource = null)
		{
			#region Set Audio Source
			if (!audioSource)
				audioSource = SFXManager.instance.defaultAudioSource;

			if(!audioSource)
			{
				Debug.LogError("You forgot to add a default audio source!");
				return;
			}
			#endregion

			if(!audioSource.isPlaying || !waitForFinish)
			{
				audioSource.clip = sfx.GetClip();
				audioSource.volume = sfx.volume + Random.Range(-sfx.volumeVariation, sfx.volumeVariation);
				audioSource.pitch = sfx.pitch + Random.Range(-sfx.pitchVariation, sfx.pitchVariation);
				audioSource.Play();
			}

		}

		[HorizontalGroup("AudioSource")]
		[ShowIf("@defaultAudioSource == null")]
		[GUIColor(1,0.5f,0.5f,1)]
		[Button]
		private void AddAudioSource()
		{
			defaultAudioSource = this.gameObject.GetComponent<AudioSource>();
			if(!defaultAudioSource)
				defaultAudioSource = this.gameObject.AddComponent<AudioSource>();
		}

		public enum SFXType
		{
			UI,
			Ambient,
			Weapons
		}

	}
}