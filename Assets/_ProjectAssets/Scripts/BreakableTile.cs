// Maded by Pedro M Marangon
using UnityEngine;

public class BreakableTile : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.CompareTag("Player") && collision.transform.position.y < transform.position.y)
		{
			Destroy(gameObject);
		}
	}

}