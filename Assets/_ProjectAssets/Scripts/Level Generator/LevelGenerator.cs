// Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Levels
{
	public class LevelGenerator : MonoBehaviour
	{
		[SerializeField] private float distance;
		[SceneObjectsOnly, SerializeField] private Transform confiner;
		[SerializeField] private List<GameObject> levelPieces = new List<GameObject>();

		public void Generate(Vector3 pos)
		{
			GameObject levelP = levelPieces[Random.Range(0, levelPieces.Count-1)];
			Vector3 position = pos + (Vector3.up * distance);
			Instantiate(levelP, position, Quaternion.identity);
			confiner.position = position;
		}

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.yellow;

			Gizmos.DrawWireCube(transform.position, new Vector3(30, distance, 0));
		}

	}
}