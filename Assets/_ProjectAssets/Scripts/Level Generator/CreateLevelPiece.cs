// Maded by Pedro M Marangon
using Game.Levels;
using UnityEngine;

public class CreateLevelPiece : MonoBehaviour
{

	[SerializeField] private GameObject obj = null;
	private LevelGenerator levelGenerator;
	private bool destroyObject = false;

	private void Awake() => levelGenerator = FindObjectOfType<LevelGenerator>();


	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!destroyObject)
		{
			levelGenerator?.Generate(transform.position);
			destroyObject = true;
			return;
		}

		if (other.CompareTag("Destroy Level Piece"))
			Destroy(obj);
	}

}