// Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Player
{

	public class AnimationSignalsToPlayer : MonoBehaviour
	{

		[SerializeField] private PlayerAtk atk;


		public void StartAirPunch() => atk.StartAirPunch();
		public void FinishAirPunch() => atk.FinishAirPunch();
		public void StartGroundPunch() => atk.StartGroundPunch();
		public void FinishGroundPunch() => atk.FinishGroundPunch();
		public void CanPunch() => atk.CanPunch();
		public void Punch() => atk.Punch();


	}

}