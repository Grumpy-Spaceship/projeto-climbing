// Maded by Pedro M Marangon
using Game.Levels;
using UnityEngine;

public class CreateLevelPiece : MonoBehaviour
{

    private LevelGenerator levelGenerator;

	private void Awake() => levelGenerator = FindObjectOfType<LevelGenerator>();


	private void OnTriggerEnter2D(Collider2D other)
	{
		levelGenerator?.Generate(transform.position);
		Destroy(gameObject);
	}

}