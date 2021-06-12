// Maded by Pedro M Marangon
using DG.Tweening;
using Game.Score;
using Game.Sounds;
using PedroUtilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Selfie
{
	public class SelfieManager : MonoBehaviour
	{

		#region Singleton Setup
		public static SelfieManager instance;

		private void InitSingleton()
		{
			if (instance != null) Destroy(gameObject);

			instance = this;
		}
		#endregion

		[TabGroup("Background"), SerializeField] private SpriteRenderer bgRend;
		[TabGroup("Background"), SerializeField] private List<Sprite> bgSprites;
		[TabGroup("UI"), SerializeField] private CanvasGroup flash;
		[TabGroup("UI"), SerializeField] private GameObject selfieUI;
		[TabGroup("UI"), SerializeField] private CanvasGroup text;
		[SerializeField] private PlayerInput player;
		[TabGroup("Sound"), HideLabel, SerializeField] private SFX camSFX;
		[SerializeField] private int score;
		private SelfiePlace _place;

		public void TakeSelfie(SelfiePlace place)
		{
			_place = place;

			bgRend.sprite = GetRandom.ElementInList(bgSprites);

			Sequence s = DOTween.Sequence();

			s.SetUpdate(true);
			s.AppendCallback(() =>
			{
				Time.timeScale = 0;
				player.enabled = false;
				camSFX.PlaySFX();
				PlayerScore.AddScore(score);
			});
			s.Append(flash.DOFade(1, 0.1f));
			s.Append(text.DOFade(0, 0f));
			s.AppendCallback(() => selfieUI.Activate());
			s.Append(flash.DOFade(0, 0.1f));
			s.AppendInterval(1.5f);
			s.Append(text.DOFade(1, 1f));
			s.AppendCallback(() =>
			{
				player.enabled = true;
				player.SwitchCurrentActionMap("AfterSelfie");
			});
		}

		public void ExitSelfie()
		{
			Sequence s = DOTween.Sequence();

			s.SetUpdate(true);

			s.Append(text.DOFade(0, 0.1f));
			if (_place) s.AppendCallback(() => Destroy(_place.gameObject));
			s.AppendCallback(() => selfieUI.Deactivate());
			s.AppendInterval(.1f);
			s.AppendCallback(() =>
			{
				Time.timeScale = 1;
				player.SwitchCurrentActionMap("Default");
			});
		}

		private void Awake()
		{
			InitSingleton();

			InitialSetup();

		}

		private void InitialSetup()
		{
			flash.alpha = text.alpha = 0;
			selfieUI.Deactivate();
			Time.timeScale = 1;
		}
	}
}
