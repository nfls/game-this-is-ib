using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton {
	
	public static Camera MainCamera => mainCamera;

	private static Camera mainCamera;
	private static CinemachineImpulseSource mainImpulseSource;
	private static RadialBlurController radialBlurController;
	private static CinemachineVirtualCamera virtualMainCamera;

	private static float radialBlurEndTime = -1f;

	private void Awake() {
		mainCamera = Camera.main;
		mainImpulseSource = mainCamera.GetComponent<CinemachineImpulseSource>();
		radialBlurController = mainCamera.GetComponent<RadialBlurController>();
		virtualMainCamera = FindObjectOfType<CinemachineVirtualCamera>();
		DontDestroyOnLoad(virtualMainCamera);
	}

	private void Update() {
		if (radialBlurEndTime > 0f)
			if (radialBlurEndTime <= Time.time) {
				radialBlurEndTime = -1f;
				radialBlurController.enabled = false;
			}
	}

	public static void Shake(Vector3 position, Vector3 velocity, float duration = .2f) {
		mainImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration;
		mainImpulseSource.GenerateImpulseAt(position, velocity);
		JoystickUtil.Rumble(1, 200);
	}

	public static void RadialBlur(Vector3 center) {
		radialBlurController.enabled = true;
		radialBlurController.center = center;
		radialBlurEndTime = Time.time + radialBlurController.duration;
	}

	private void OnApplicationQuit() {
		JoystickUtil.Quit();
	}
}