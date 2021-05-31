// Maded by Pedro M Marangon
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Player
{
	[CreateAssetMenu(fileName = "New Player Settings", menuName = "Game/Player Settings")]
	public class PlayerSettings : ScriptableObject
	{
		[Space, SerializeField] private float moveSpeed = 200;
		[TabGroup("Jump"), HideLabel, SerializeField] private PlayerJumpSystem jump = new PlayerJumpSystem();
		[TabGroup("Knockback"), SerializeField] private float knockbackDur = .2f;
		[TabGroup("Knockback"), SerializeField] private float knockForce = 3f;
		[TabGroup("New Group","Punch"), SerializeField] private LayerMask breakableTileMask =  1 << 1;
		[TabGroup("New Group","Punch"), SerializeField] private float punchRadiusDetection = 0.5f;
		[TabGroup("New Group","Punch"), SerializeField] private float punchGraphicShowTime = 0.1f;
		[TabGroup("New Group","Punch"), SerializeField] private float punchInAirStop = .2f, punchCooldown = 1f;
		[TabGroup("New Group","Punch"), SerializeField] private Vector2 upPos = default, normalPos = default;
		[TabGroup("New Group", "Cinemachine"), Range(0,1), SerializeField] private float upScreenY = 0.5f;
		[TabGroup("New Group","Cinemachine"), Range(0,1), SerializeField] private float downScreenY = 0.5f;
		[TabGroup("New Group","Cinemachine"), Range(0,2), SerializeField] private float camSmoothness = 0.5f;
		[TabGroup("New Group","Cinemachine"), Range(0.3f,0.8f), SerializeField] private float normalScreenY = 0.5f;

		public PlayerJumpSystem Jump => jump;

		public float MoveSpeed => moveSpeed;

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