// Maded by Pedro M Marangon
using UnityEngine;

namespace UnityEditor
{
	[AddComponentMenu("Physics 2D/Add Triggers/Capsule Trigger 2D")]
	public class CapsuleTrigger2D : MonoBehaviour
	{
		private void OnValidate()
		{
			gameObject.AddComponent<CapsuleCollider2D>().isTrigger = true;
			if (!GetComponent<Rigidbody2D>()) gameObject.AddComponent<Rigidbody2D>();
			EditorApplication.delayCall += () => DestroyImmediate(this);
		}
	}

}