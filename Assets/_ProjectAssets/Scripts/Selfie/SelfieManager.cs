using PedroUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game
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

		[SerializeField] private Camera cam;
		[SerializeField] private SpriteRenderer bgRend;
		[SerializeField] private List<Sprite> bgSprites;
		[SerializeField] private Image img;
		private bool takeSelfie;
		private Sprite resultSprite;

		public void TakeSelfie()
		{
			cam.targetTexture = RenderTexture.GetTemporary(Screen.width, Screen.height, 16);
			takeSelfie = true;
		}

		private void OnPostRender()
		{
			if (takeSelfie)
			{
				takeSelfie = false;

				bgRend.sprite = GetRandom.ElementInList(bgSprites);

				Texture2D rendResult = GetRect(out Rect rect);

				resultSprite = Sprite.Create(rendResult, rect, Vector3.one * 0.5f);

				img.sprite = resultSprite;
				img.color = Color.white;
			}
		}

		private Texture2D GetRect(out Rect rect)
		{
			RenderTexture renderTexture = cam.targetTexture;

			var rendResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
			rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
			rendResult.ReadPixels(rect, 0, 0);

			return rendResult;
		}

		private void Awake()
		{
			InitSingleton();
		}

	}
}
