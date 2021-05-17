//Maded by Pedro M Marangon
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace PedroUtilities
{

	public class GetRandom
	{

		public static TObject ElementInArray<TObject>(TObject[] obj)
		{
			if (obj.Length <= 0) return default(TObject);
			int position = UnityEngine.Random.Range(0, obj.Length);
			return obj[position];
		}
		
		public static TObject ElementInList<TObject>(List<TObject> obj)
		{
			if (obj.Count <= 0) return default(TObject);
			int position = UnityEngine.Random.Range(0, obj.Count);
			return obj[position];
		}
		
		public static TObject SceneObjectOfType<TObject>() where TObject : Component
		{
			TObject[] obj = MonoBehaviour.FindObjectsOfType<TObject>();
			if (obj.Length <= 0 || obj == null) return default(TObject);
			int position = UnityEngine.Random.Range(0, obj.Length);
			return obj[position];
		}
		
		public static Quaternion Rotation2D(float min = 0, float max = 360)
		{
			Quaternion rot = Quaternion.Euler(0, 0, UnityEngine.Random.Range(min, max));
			return rot;
		}
		
		public static Quaternion Rotation3D(float minX = 0, float maxX = 360, float minY = 0, float maxY = 360, float minZ = 0, float maxZ = 360)
		{
			float X = UnityEngine.Random.Range(minX, maxX);
			float Y = UnityEngine.Random.Range(minY, maxY);
			float Z = UnityEngine.Random.Range(minZ, maxZ);
			Quaternion rot = Quaternion.Euler(X, Y, Z);
			return rot;
		}
		
		public static void AudioAndPlayIt(string audioName, AudioClip[] clips,AudioSource source = null, bool varPitch = false, AudioMixerGroup group = null,
										Vector3 pos = default(Vector3), float volume = 1, Transform parent = null, bool mute = false,
										bool loop = false, bool playAtStart = false, float spacialBlend = 0)
		{
			float randPitch = UnityEngine.Random.Range(0.5f, 1.5f);
			if (!source)
			{
				AudioSource s = new GameObject(audioName).AddComponent<AudioSource>();
				AudioClip clip = ElementInArray(clips);
				s.transform.parent = parent;
				s.pitch = varPitch ? randPitch : s.pitch;
				s.outputAudioMixerGroup = group;
				s.transform.position = pos;
				s.PlayOneShot(clip, volume);
				MonoBehaviour.Destroy(s.gameObject,clip.length+0.025f);
			}
			else
			{
				AudioClip clip = ElementInArray(clips);
				source.transform.parent = parent;
				source.pitch = varPitch ? randPitch : source.pitch;
				source.outputAudioMixerGroup = group;
				source.transform.position = pos;
				source.PlayOneShot(clip, volume);
			}

		}

	}

	public class GetAll {

		public static T[] ObjectsInSceneExceptThis<T>(T args, Transform parent = null) where T : Component {
			List<T> obj = parent.GetComponentsInChildren<T>().ToList();
			for (int o = 0; o < obj.Count;o++) {
				if (obj[o] == args) obj.RemoveAt(o);
				else if (args.GetComponent<T>() != null && args.GetComponent<T>() != obj[o]) obj.RemoveAt(o);
			}

			return obj.ToArray();
		}

		public static int TotalInScene<T>() where T:Component{
			T[] obj = MonoBehaviour.FindObjectsOfType<T>();
			if (obj == null) return 0;
			return obj.Length;
		}
	}

	public static class Extensions
	{
		public static float[] GenerateCurveArray(this AnimationCurve self, int size = 256)
		{
			float[] returnArray = new float[size];
			for (int j = 0; j < size; j++)
			{
				returnArray[j] = self.Evaluate(j / size);
			}
			return returnArray;
		}

		public static void StayInBounds(this Transform self, float minX, float minY, float maxX, float maxY)
		{
			float x, y;
			x = Mathf.Clamp(self.position.x, minX, maxX);
			y = Mathf.Clamp(self.position.y, minY, maxY);
			self.position = new Vector3(x, y, self.position.z);
		}
		
		public static void SuicideComponent(this Component gObj, float time = 0) => Component.Destroy(gObj, time);
		public static void Suicide(this Component gObj, float time = 0) => Component.Destroy(gObj.gameObject, time);

		public static float ChangeValueAndClamp(this float og, float min, float max, float value = 1, ValueChangeMode mode = ValueChangeMode.Add)
		{
			switch (mode)
			{
				case ValueChangeMode.Add: og += value; break;
				case ValueChangeMode.Subtract: og -= value; break;
				case ValueChangeMode.Multiply: og *= value; break;
				case ValueChangeMode.Divide: og /= value; break;
			}
			float result = Mathf.Clamp(og, min, max);
			return result;
		}
		public static float Remap(this float from, float fromMin, float fromMax, float toMin, float toMax)
		{

			var fromAbs = from - fromMin;
			var fromMaxAbs = fromMax - fromMin;
			var normal = fromAbs / fromMaxAbs;
			var toMaxAbs = toMax - toMin;
			var toAbs = toMaxAbs * normal;
			var to = toAbs + toMin;
			return to;
		}
		public static bool IsInsideInterval(this float val, float sVal, float eVal, bool sClosed, bool eClosed)
		{
			int count = 0;

			if (sClosed)
				count += (val >= sVal) ? 1 : 0;
			else
				count += (val > sVal) ? 1 : 0;

			if (eClosed)
				count += (val <= eVal) ? 1 : 0;
			else
				count += (val < eVal) ? 1 : 0;

			return count == 2;
		}

		public static int ChangeValueAndClamp(this int og, int min, int max, int value = 1, ValueChangeMode mode = ValueChangeMode.Add)
		{
			switch (mode)
			{
				case ValueChangeMode.Add: og += value; break;
				case ValueChangeMode.Subtract: og -= value; break;
				case ValueChangeMode.Multiply: og *= value; break;
				case ValueChangeMode.Divide: og /= value; break;
			}
			int result = Mathf.Clamp(og, min, max);
			return result;
		}

		public static float GetAngleFromVector2(this Vector2 vec, float amntToAdd = 0)
		{
			float angle = (Mathf.Atan2(vec.y, -vec.x) / Mathf.PI) * 180f;
			angle += amntToAdd;
			if (angle < 0)
			{
				angle += 360;
			}

			return angle;
		}

		public static void Activate(this GameObject go) => go.SetActive(true);
		public static void Deactivate(this GameObject go) => go.SetActive(false);
		public static bool IsInsideLayerMask(this GameObject go, LayerMask layerMask) => (layerMask.value & (1 << go.layer)) > 0;

	}

	public static class GeneralUtilities {
		public static Transform GetTransformFromScript<Script>() where Script : Component => MonoBehaviour.FindObjectOfType<Script>().transform;

		public static Vector3 ClampVector3(Vector3 baseVector, Vector3 min, Vector3 max)
		{
			float clampedX = Mathf.Clamp(baseVector.x, min.x, max.x);
			float clampedY = Mathf.Clamp(baseVector.y, min.y, max.y);
			float clampedZ = Mathf.Clamp(baseVector.z, min.z, max.z);
			return new Vector3(clampedX, clampedY, clampedZ);
		}
		public static Vector3 GetVector3FromAngle(float angle)
		{
			float angleRad = angle * (Mathf.PI / 180f);
			return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
		}
		public static Vector2 ClampVector2(Vector2 baseVector, Vector2 min, Vector2 max)
		{
			float clampedX = Mathf.Clamp(baseVector.x, min.x, max.x);
			float clampedY = Mathf.Clamp(baseVector.y, min.y, max.y);
			return new Vector2(clampedX, clampedY);
		}
		public static Vector2 GetVector2FromAngle(float angle)
		{
			float angleRad = angle * (Mathf.PI / 180f);
			return new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
		}

		public static float GetAngleFromVector(Vector3 dir, bool minus90 = false) {
			dir = dir.normalized;
			float n;
			n = minus90 ? Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg - 90f
				 : Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			if (n < 0) n += 360;
			return n;
		}

		/// <summary>
		/// Faz uma regra de 3 entre A, B e C. Se A equivale a B, C equivale ao resultado da equação
		/// </summary>
		/// <returns>o resultado da regra de 3</returns>
		public static float RegraDe3(float a, float b, float c) => (c * b) / a;

		public static bool IsNotNull(object obj) => obj != null;

	}

	[Serializable]
	public struct ValueRange
	{
		public float min;
		public float max;

		public ValueRange(float _min,float _max)
		{
			min = _min;
			max = _max;
		}
	}

	public enum ValueChangeMode { Add, Subtract, Multiply, Divide }

}