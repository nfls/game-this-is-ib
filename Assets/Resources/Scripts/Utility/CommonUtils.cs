﻿using System;
using UnityEngine;

public static class CommonUtils {

	public const string NULL_STRING = "NULL";

	public static void ResetToZero(ref Vector3 vector3) {
		vector3.x = 0f;
		vector3.y = 0f;
		vector3.z = 0f;
	}
}
