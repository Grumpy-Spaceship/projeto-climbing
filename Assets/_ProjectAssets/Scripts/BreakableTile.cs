// Maded by Pedro M Marangon
using Game.Player;
using UnityEngine;

namespace Game
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class BreakableTile : MonoBehaviour
	{
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag("Player") && other.TryGetComponent<PlayerScript>(out var p))
			{
				p.StopYMovement();
				Destroy(gameObject);
			}
		}
	}
}