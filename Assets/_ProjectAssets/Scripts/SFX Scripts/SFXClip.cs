// Maded by Pedro M Marangon
using PedroUtilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Sounds
{
    [CreateAssetMenu(menuName = "New SFX Clip", fileName = "NewSFXClip")]
    public class SFXClip : ScriptableObject
    {
        [Space, Title("Audio Clip")]
        public bool useListOfClips;
        [Required, HideIf("useListOfClips")] public AudioClip clip;
        [ShowIf("useListOfClips")] public List<AudioClip> clips;

        [Title("Clip Settings")]
        [Range(0, 1)] public float volume = 1;
        [Range(0, 0.2f)] public float volumeVariation = 0.05f;
        [Range(0, 2)] public float pitch = 1;
        [Range(0, 0.2f)] public float pitchVariation = 0.05f;

        public AudioClip GetClip()
		{
            if (useListOfClips) return clip;
            else return GetRandom.ElementInList(clips);
		}

    }
}
