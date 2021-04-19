// Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Player
{
	[CreateAssetMenu(fileName = "New Player Settings", menuName = "Game/Player Settings")]
	public class PlayerSettings : ScriptableObject
	{
		[Header("Jump Configurations")]
		[SerializeField] private PlayerJumpSystem jump = new PlayerJumpSystem();
		//[SerializeField] private AnimationHandler anim = null;
		[Space(20), SerializeField] private float knockbackDur = .2f;
		[SerializeField] private float knockForce = 3f;

		[Header("Movement Configurations")]
		[SerializeField] private float moveSpeed = 200;
		[SerializeField] private float horizontalAcceleration = 1;
		[Range(0, 1), SerializeField] private float horizontalDampingBasic = 0.5f;
		[Range(0, 1), SerializeField] private float horizontalDampingWhenStopping = 0.5f;
		[Range(0, 1), SerializeField] private float horizontalDampingWhenTurning = 0.5f;

		[Header("Punch Configurations")]
		[SerializeField] private LayerMask breakableTileMask =  1 << 1;
		[SerializeField] private float punchRadiusDetection = 0.5f;
		[SerializeField] private float punchGraphicShowTime = 0.1f;
		[SerializeField] private float punchInAirStop = .2f, punchCooldown = 1f;
		[SerializeField] private Vector2 upPos = default, normalPos = default;
		[Header("Cinemachine Configurations")]
		[Range(0,1), SerializeField] private float upScreenY = 0.5f;
		[Range(0,1), SerializeField] private float downScreenY = 0.5f;
		[Range(0,2), SerializeField] private float camSmoothness = 0.5f;
		[Range(0.3f,0.8f), SerializeField] private float normalScreenY = 0.5f;

		public PlayerJumpSystem Jump => jump;

		public float MoveSpeed => moveSpeed;
		public float H_Acceleration => horizontalAcceleration;
		public float BasicDamping => horizontalDampingBasic;
		public float StopDamping => horizontalDampingWhenStopping;
		public float TurnDamping => horizontalDampingWhenTurning;

		public float UpScreenY => upScreenY;
		public float DownScreenY => downScreenY;
		public float NormalScreenY => normalScreenY;
		public float CamSmoothness => camSmoothness;

		public float StopTimeWhenPunchingAir => punchInAirStop;
		public float PunchCooldown => punchCooldown;
		public float PunchGFXShowTime => punchGraphicShowTime;
		public Vector2 PunchUpPos => upPos;
		public Vector2 PunchNormalPos => normalPos;
		public float PunchRadiusDetection => punchRadiusDetection;
		public LayerMask BreakableTileMask => breakableTileMask;

		public float KnockbackDur => knockbackDur;
		public float KnockbackForce=> knockForce;
	}

}