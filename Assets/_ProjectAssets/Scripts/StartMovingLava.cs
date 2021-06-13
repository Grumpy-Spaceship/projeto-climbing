// Maded by Pedro M Marangon
using Game.Movement;
using UnityEngine;

namespace Game
{
	public class StartMovingLava : MonoBehaviour
    {
		private Lava lava;

		private void Awake() => lava = FindObjectOfType<Lava>();

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				lava?.StartRising();
				Destroy(gameObject);
			}
		}

	}
}
