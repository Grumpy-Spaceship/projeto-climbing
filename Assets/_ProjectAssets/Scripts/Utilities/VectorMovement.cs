//Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Movement
{
	public class VectorMovement : MonoBehaviour
	{
		protected enum Vector2Dir { Up, Down, Left, Right, Random, Custom }
		protected enum UpdateType { Update, FixedUpdate, LateUpdate }
		public bool IsCustomDir() => direction == Vector2Dir.Custom;
		public bool IsRandomDir() => direction == Vector2Dir.Random;

		[TabGroup("Movement"), EnumToggleButtons, SerializeField] protected UpdateType updateType = UpdateType.Update;
		[TabGroup("Movement"), EnumPaging, SerializeField] protected Vector2Dir direction = Vector2Dir.Up;
		[TabGroup("Movement"), ShowIf("@direction == Vector2Dir.Custom"), SerializeField] protected Vector2 vectorDir = Vector2.zero;
		[TabGroup("Movement"), ShowIf("@direction == Vector2Dir.Random"), ReadOnly, SerializeField] protected Vector2 randomDir = Vector2.zero;
		[TabGroup("Movement"), EnumToggleButtons, SerializeField] protected Space movementBasedOn = Space.Self;
		[TabGroup("Movement")] public float speed;
		[TabGroup("Movement")] public bool canMove;

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

		protected virtual void Update() => MoveIfCanTranslateAnd(updateType == UpdateType.Update);
		protected virtual void FixedUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.FixedUpdate);
		protected virtual void LateUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.LateUpdate);

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