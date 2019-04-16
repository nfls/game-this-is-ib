using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Extensions {

	public static bool IsNullPath(this string path) => path == CommonUtils.NULL_STRING;

	public static KeyCode[] Resize(this KeyCode[] array, int length) {
		KeyCode[] newArray = new KeyCode[length];
		for (int i = 0, len = array.Length; i < length; i++)
			if (i < array.Length) newArray[i] = array[i];
		return newArray;
	}

	public static T ToEnum<T>(this string value) where T : struct => (T) Enum.Parse(typeof(T), value);

	public static void ResetToZero(this Vector3 vector3) {
		vector3.x = 0f;
		vector3.y = 0f;
		vector3.z = 0f;
	}

	public static Quaternion RotateTo(this Quaternion initialRotation, Quaternion rotation) {
		if (initialRotation.x == 0f && initialRotation.y == 0f && initialRotation.z == 0f) return rotation;
		return initialRotation * rotation;
	}

	public static Quaternion ToQuaternion(this Vector3 rotation) => Quaternion.Euler(rotation);

	public static FileInfo[] GetFiles(this DirectoryInfo directory, string[] searchPatterns, SearchOption option) {
		List<FileInfo> files = new List<FileInfo>();
		foreach (var pattern in searchPatterns)
			foreach (var file in directory.GetFiles(pattern, option)) files.Add(file);
		return files.ToArray();
	}

	public static string GetFaceCollisionDirection(this FaceDirection direction) => direction == FaceDirection.Right ? CharacterMotor.RIGHT_COLLISION : CharacterMotor.LEFT_COLLISION;
}

public class CameraBackgroundColorAttribute : PropertyAttribute { }

public class WaterQualitySettingAttribute : PropertyAttribute { }