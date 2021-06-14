// Maded by Pedro M Marangon
using DG.Tweening;
using Game.Score;
using Game.Sounds;
using PedroUtilities;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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
		[TabGroup("Poses"), SerializeField] private SpriteRenderer poseRend;
		[TabGroup("Poses"), SerializeField] private List<Sprite> poseSprites;
		[TabGroup("UI"), SerializeField] private CanvasGroup flash;
		[TabGroup("UI"), SerializeField] private GameObject selfieUI;
		[TabGroup("UI"), SerializeField] private CanvasGroup text;
		[SerializeField] private PlayerInput player;
		[TabGroup("Sound"), HideLabel, SerializeField] private SFX camSFX;
		[TabGroup("Sound"), SerializeField] private AudioMixerSnapshot normalMusic, lowpass;
		[TabGroup("Animation times"), SerializeField] private float textFadeTime = 1f;
		[TabGroup("Animation times"), SerializeField] private float flashTime = 0.1f;
		[TabGroup("Animation times"), SerializeField] private float waitForEnableInput = .1f;
		[TabGroup("Animation times"), SerializeField] private float waitForInput = 1.5f;
		[SerializeField] private int score;
		private SelfiePlace _place;

		public void TakeSelfie(SelfiePlace place)
		{
			_place = place;

			bgRend.sprite = GetRandom.ElementInList(bgSprites);
			poseRend.sprite = GetRandom.ElementInList(poseSprites);


			Sequence s = DOTween.Sequence();

			s.SetUpdate(true);
			s.AppendCallback(() =>
			{
				Time.timeScale = 0;
				player.GetComponent<Player.PlayerScript>().StopMovement();
				player.enabled = false;
				camSFX.PlaySFX();
				PlayerScore.AddScore(score);
				lowpass.TransitionTo(0.01f);
			});
			s.Append(flash.DOFade(1, flashTime));
			s.Append(text.DOFade(0, 0f));
			s.AppendCallback(() => selfieUI.Activate());
			s.Append(flash.DOFade(0, 0.1f));
			s.AppendInterval(waitForInput);
			s.AppendCallback(() =>
			{
				player.enabled = true;
				player.SwitchCurrentActionMap("AfterSelfie");
			});
			s.Append(text.DOFade(1, textFadeTime));
		}

		public void ExitSelfie()
		{
			Sequence s = DOTween.Sequence();

			s.SetUpdate(true);

			s.Append(text.DOFade(0, 0.1f));
			if (_place && _place.DestroyOnExit) s.AppendCallback(() => Destroy(_place.gameObject));
			s.AppendCallback(() => selfieUI.Deactivate());
			s.AppendInterval(waitForEnableInput);
			s.AppendCallback(() =>
			{
				Time.timeScale = 1;
				player.SwitchCurrentActionMap("Default");
				normalMusic.TransitionTo(0.01f);
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
			normalMusic.TransitionTo(0.01f);
			selfieUI.Deactivate();
			Time.timeScale = 1;
		}
	}
}
