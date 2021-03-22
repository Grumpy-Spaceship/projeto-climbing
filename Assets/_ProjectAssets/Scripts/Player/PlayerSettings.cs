// Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Player
{
	[CreateAssetMenu(fileName = "New Player Settings", menuName = "Game/Player Settings")]
	public class PlayerSettings : ScriptableObject
	{
		[SerializeField] private PlayerJumpSystem jump;
		//[SerializeField] private AnimationHandler anim = null;
		[SerializeField] public float moveSpeed = 200;
		[SerializeField, Range(0f, 0.9f)] private float moveThreshold = 0.125f;
		[SerializeField] private LayerMask breakableTileMask =  1 << 1;
		[SerializeField] private float punchRadiusDetection = 0.5f;

		public PlayerJumpSystem Jump => jump;
		public float MoveSpeed => moveSpeed;
		public float MoveThreshold => moveThreshold;

		public float PunchRadiusDetection => punchRadiusDetection;
		public LayerMask BreakableTileMask => breakableTileMask;
	}

}