// Maded by Pedro M Marangon
using Game.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game
{
	public class EndTutorial : MonoBehaviour
    {

		[SerializeField] private GameObject ui;
		[SerializeField] private PlayerInput input;
		[SerializeField] private Lava lava;

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.CompareTag("Player"))
			{
				ui.SetActive(true);
				lava.enabled = false;
				Destroy(input.gameObject);
				EventSystem.current.SetSelectedGameObject(ui.transform.GetChild(0).Find("PlayGame").gameObject);
			}
		}
	}
}
