//Maded by Pedro M Marangon
using NaughtyAttributes;
using UnityEngine;

namespace Game.Player
{
	public class TranslateWithVector : MonoBehaviour
	{
		private enum Vector2Dir { Up, Down, Left, Right, Random, Custom }
		private enum UpdateType { Update, FixedUpdate, LateUpdate }
		public bool IsCustomDir() => direction == Vector2Dir.Custom;
		public bool IsRandomDir() => direction == Vector2Dir.Random;

		public float speed;
		[SerializeField] private UpdateType updateType = UpdateType.Update;
		[SerializeField] private Vector2Dir direction = Vector2Dir.Up;
		[SerializeField] private Space movementBasedOn = Space.World;
		[ShowIf("IsCustomDir"), SerializeField] private Vector2 vectorDir = Vector2.zero;
		[ShowIf("IsRandomDir"), ShowNonSerializedField] private Vector2 randomDir = Vector2.zero;
		[ShowIf("IsRandomDir"), SerializeField] private bool updateRandomDir = false;
		[ShowIf(EConditionOperator.And, "IsRandomDir", "updateRandomDir"),Range(0.1f,5f), SerializeField] private float timeToChangeDir = 2f;
		public bool canTranslate;

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
		private void RandomizeDir() => randomDir = Random.insideUnitCircle;


		private void Awake()
		{
			RandomizeDir();

			if(updateRandomDir) InvokeRepeating(nameof(RandomizeDir), timeToChangeDir, timeToChangeDir);
		}


		private void Update() => MoveIfCanTranslateAnd(updateType == UpdateType.Update);
		private void FixedUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.FixedUpdate);
		private void LateUpdate() => MoveIfCanTranslateAnd(updateType == UpdateType.LateUpdate);

		private void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (Time.timeScale != 0 && correctTypeOfUpdate && canTranslate)
			{
				transform.Translate(speed * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
			}
		}
	}
}