//Maded by Pedro M Marangon
using NaughtyAttributes;
using UnityEngine;

namespace Game.Movement
{
	public class VectorMovement : MonoBehaviour
	{
		protected enum Vector2Dir { Up, Down, Left, Right, Random, Custom }
		protected enum UpdateType { Update, FixedUpdate, LateUpdate }
		public bool IsCustomDir() => direction == Vector2Dir.Custom;
		public bool IsRandomDir() => direction == Vector2Dir.Random;

		public float speed;
		[SerializeField] protected UpdateType updateType = UpdateType.Update;
		[SerializeField] protected Vector2Dir direction = Vector2Dir.Up;
		[ShowIf("IsCustomDir"), SerializeField] protected Vector2 vectorDir = Vector2.zero;
		[ShowIf("IsRandomDir"), ShowNonSerializedField] protected Vector2 randomDir = Vector2.zero;
		[SerializeField] protected Space movementBasedOn = Space.Self;
		public bool canMove;

		protected Vector2 GetDir()
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
		protected void RandomizeDir() => randomDir = Random.insideUnitCircle;

		protected virtual void Awake() => RandomizeDir();

		protected void Update() => MoveIfCanTranslateAnd(updateType == UpdateType.Update);
		protected void FixedUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.FixedUpdate);
		protected void LateUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.LateUpdate);

		protected virtual void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (correctTypeOfUpdate && canMove)
			{
				Move();
			}
		}

		protected virtual void Move() { }
	}
}