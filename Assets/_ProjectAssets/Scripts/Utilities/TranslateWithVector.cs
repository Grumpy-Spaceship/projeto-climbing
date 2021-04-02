//Maded by Pedro M Marangon
using NaughtyAttributes;
using UnityEngine;

namespace Game.Movement
{
	public class TranslateWithVector : VectorMovement
	{
		
		[ShowIf("IsRandomDir"), SerializeField] private bool updateRandomDir = false;
		[ShowIf(EConditionOperator.And, "IsRandomDir", "updateRandomDir"),Range(0.1f,5f), SerializeField] private float timeToChangeDir = 2f;

		protected override void Awake()
		{
			base.Awake();

			if(updateRandomDir) InvokeRepeating(nameof(RandomizeDir), timeToChangeDir, timeToChangeDir);
		}
		protected override void MoveIfCanTranslateAnd(bool correctTypeOfUpdate)
		{
			if (Time.timeScale != 0 && correctTypeOfUpdate && canMove)
			{
				transform.Translate(speed * Time.unscaledDeltaTime * GetDir(), movementBasedOn);
			}
		}
	}
}