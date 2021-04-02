//Maded by Pedro M Marangon
using UnityEngine;

namespace Game.Movement
{

	public class RotateWithVector : VectorMovement
	{
		protected override void Move() => transform.Rotate(speed * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
	}
}