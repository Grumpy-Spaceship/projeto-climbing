// Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Sounds
{
	public class SFXButton : MonoBehaviour
    {

		[TabGroup("Select"), HideLabel, SerializeField] private SFX selectSFX;
		[TabGroup("Click"), HideLabel, SerializeField] private SFX clickSFX;

		public void PlaySelect() => selectSFX.PlaySFX();
		public void PlayClick() => clickSFX.PlaySFX();

	}
}
