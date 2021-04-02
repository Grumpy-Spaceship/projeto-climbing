// Maded by Pedro M Marangon
using DG.Tweening;
using UnityEngine;

namespace Game.Movement
{
	public class FollowGameObject : MonoBehaviour
	{
		[SerializeField] private Transform objToFollow = null;
		[SerializeField] private float smoothingTime = 0.1f;
		[SerializeField] private bool followX = true, followY = true;

		private void Update()
		{
			Vector3 thisPos = transform.position, objPos = objToFollow.position;
			Vector3 pos = new Vector3(followX ? objPos.x : thisPos.x, followY ? objPos.y : thisPos.y);
			transform.DOMove(pos,smoothingTime);
		}

	}
}