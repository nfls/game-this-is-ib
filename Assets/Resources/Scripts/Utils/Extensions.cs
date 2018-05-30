using System;
using UnityEngine;

public static class Extensions {

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
}