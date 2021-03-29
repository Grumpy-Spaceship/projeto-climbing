// Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Player
{
	[CreateAssetMenu(fileName = "New Player Settings", menuName = "Game/Player Settings")]
	public class PlayerSettings : ScriptableObject
	{
		[SerializeField] private PlayerJumpSystem jump = new PlayerJumpSystem();
		//[SerializeField] private AnimationHandler anim = null;
		[Space,SerializeField] public float moveSpeed = 200;
		[SerializeField] private LayerMask breakableTileMask =  1 << 1;
		[SerializeField] private float punchRadiusDetection = 0.5f;
		[SerializeField] private float punchInAirStop = .2f, punchCooldown = 1f;
		[SerializeField] private Vector2 upPos = default, normalPos = default;
		[Header("Cinemachine Configurations")]
		[Range(0,1), SerializeField] private float upScreenY = 0.5f;
		[Range(0,1), SerializeField] private float downScreenY = 0.5f;
		[Range(0,2), SerializeField] private float camSmoothness = 0.5f;
		[Range(0.3f,0.8f), SerializeField] private float normalScreenY = 0.5f;

		public PlayerJumpSystem Jump => jump;
		public float MoveSpeed => moveSpeed;

		public float UpScreenY => upScreenY;
		public float DownScreenY => downScreenY;
		public float NormalScreenY => normalScreenY;
		public float CamSmoothness => camSmoothness;

		public float StopTimeWhenPunchingAir => punchInAirStop;
		public float PunchCooldown => punchCooldown;
		public Vector2 PunchUpPos => upPos;
		public Vector2 PunchNormalPos => normalPos;
		public float PunchRadiusDetection => punchRadiusDetection;
		public LayerMask BreakableTileMask => breakableTileMask;


	}

}