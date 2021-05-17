// Maded by Pedro M Marangon
using UnityEngine;

namespace Game
{
	public class ParallaxBackground : MonoBehaviour
    {

		[SerializeField] private Vector2 parallaxEffectMultiplier = new Vector2(0.5f,0.5f);
		[SerializeField] private bool infiniteHorizontal = false, infiniteVertical = true;

        private Transform cam;
        private Vector3 lastCamPos;
        private float texUnitSizeX, texUnitSizeY;

		void Start()
        {
            cam = Camera.main.transform;
            lastCamPos = cam.position;

            Sprite spr = GetComponent<SpriteRenderer>().sprite;
            Texture2D tex = spr.texture;
            texUnitSizeX = tex.width / spr.pixelsPerUnit;
            texUnitSizeY = tex.height / spr.pixelsPerUnit;
        }
        
        void LateUpdate()
        {
            Vector3 delta = cam.position - lastCamPos;
            transform.position += new Vector3(delta.x * parallaxEffectMultiplier.x, delta.y * parallaxEffectMultiplier.y);
            lastCamPos = cam.position;

			float distanceX = cam.position.x - transform.position.x;
			float distanceY = cam.position.y - transform.position.y;

            if (infiniteHorizontal && Mathf.Abs(distanceX) >= texUnitSizeX)
            {
                float offsetX = distanceX % texUnitSizeX;
                transform.position = new Vector3(cam.position.x + offsetX, transform.position.y);
            }

            if (infiniteVertical && Mathf.Abs(distanceY) >= texUnitSizeY)
            {
                float offsetY = distanceY % texUnitSizeY;
                transform.position = new Vector3(transform.position.x, cam.position.y + offsetY);
            }

        }
    }
}
