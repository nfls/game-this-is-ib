using System;
using UnityEngine;

public static class Extensions {

	public static bool IsNullPath(this string path) {
		return path == CommonUtils.NULL_STRING;
	}

	public static KeyCode[] Resize(this KeyCode[] array, int length) {
		KeyCode[] newArray = new KeyCode[length];
		for (int i = 0, len = array.Length; i < length; i++) {
			if (i < array.Length) {
				newArray[i] = array[i];
			}
		}
		return newArray;
	}

	public static T ToEnum<T>(this string value) where T : struct {
		return (T)Enum.Parse(typeof(T), value);
	}

	public static void ResetToZero(this Vector3 vector3) {
		vector3.x = 0f;
		vector3.y = 0f;
		vector3.z = 0f;
	}

	public static Quaternion RotateTo(this Quaternion initialRotation, Quaternion rotation) {
		if (initialRotation.x == 0f && initialRotation.y == 0f && initialRotation.z == 0f) {
			return rotation;
		}
		return initialRotation * rotation;
	}

	public static Quaternion ToQuaternion(this Vector3 rotation) {
		return Quaternion.Euler(rotation);
	}
}