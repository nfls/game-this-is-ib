using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton {
	
	public static Camera MainCamera => mainCamera;

	private static Camera mainCamera;

	private void Start() {
		mainCamera = Camera.main;
	}
}
