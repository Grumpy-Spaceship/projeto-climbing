// Maded by Pedro M Marangon
using UnityEngine;

namespace UnityEditor
{

	[AddComponentMenu("Physics 2D/Add Triggers/Box Trigger 2D")]
	public class BoxTrigger2D : MonoBehaviour
	{
		private void OnValidate()
		{
			gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
			if (!GetComponent<Rigidbody2D>()) gameObject.AddComponent<Rigidbody2D>();
			EditorApplication.delayCall += () => DestroyImmediate(this);
		}
	}

}