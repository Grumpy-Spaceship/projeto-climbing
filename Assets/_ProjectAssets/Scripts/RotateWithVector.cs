//Maded by Pedro M Marangon
using NaughtyAttributes;
using UnityEngine;

namespace Game.Player
{
	public class RotateWithVector : MonoBehaviour
	{
		private enum Vector2Dir { Up, Down, Left, Right, Random, Custom }
		private enum UpdateType { Update, FixedUpdate, LateUpdate }
		public bool IsCustomDir() => direction == Vector2Dir.Custom;
		public bool IsRandomDir() => direction == Vector2Dir.Random;

		public float speed;
		[SerializeField] private UpdateType updateType = UpdateType.Update;
		[SerializeField] private Vector2Dir direction = Vector2Dir.Up;
		[ShowIf("IsCustomDir"), SerializeField] private Vector2 vectorDir = Vector2.zero;
		[ShowIf("IsRandomDir"), ShowNonSerializedField] private Vector2 randomDir = Vector2.zero;
		[SerializeField] private Space movementBasedOn = Space.Self;
		public bool canRotate;

		private Vector2 GetDir()
		{
			switch (direction)
			{
				case Vector2Dir.Up: return Vector2.up;
				case Vector2Dir.Down: return Vector2.down;
				case Vector2Dir.Left: return Vector2.left;
				case Vector2Dir.Right: return Vector2.right;
				case Vector2Dir.Random: return randomDir;
				case Vector2Dir.Custom: return vectorDir;
			}
			return Vector2.zero;
		}


		private void Awake() => randomDir = Random.insideUnitCircle;

		private void Update() => MoveIfCanTranslateAnd(updateType == UpdateType.Update);
		private void FixedUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.FixedUpdate);
		private void LateUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.LateUpdate);

		private void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (correctTypeOfUpdate && canRotate)
			{
				transform.Rotate(speed * Time.unscaledDeltaTime * GetDir(),movementBasedOn);
			}
		}
	}
}