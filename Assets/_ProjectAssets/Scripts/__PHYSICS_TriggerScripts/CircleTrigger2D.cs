// Maded by Pedro M Marangon
using UnityEngine;

namespace UnityEditor
{
	[AddComponentMenu("Physics 2D/Add Triggers/Circle Trigger 2D")]
	public class CircleTrigger2D : MonoBehaviour
	{
		private void OnValidate()
		{
			gameObject.AddComponent<CircleCollider2D>().isTrigger = true;
			if (!GetComponent<Rigidbody2D>()) gameObject.AddComponent<Rigidbody2D>();
			EditorApplication.delayCall += () => DestroyImmediate(this);
		}
	}

}