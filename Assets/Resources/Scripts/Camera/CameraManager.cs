using Cinemachine;
using UnityEngine;

public class CameraManager : MonoSingleton {
	
	public static Camera MainCamera => mainCamera;

	private static CinemachineVirtualCamera virtualMainCamera;
	private static Camera mainCamera;
	private static CinemachineImpulseSource mainImpulseSource;

	private void Awake() {
		mainCamera = Camera.main;
		mainImpulseSource = mainCamera.GetComponent<CinemachineImpulseSource>();
		virtualMainCamera = mainCamera.transform.parent.GetComponent<CinemachineVirtualCamera>();
		DontDestroyOnLoad(virtualMainCamera);
	}

	public static void Shake(Vector3 position, Vector3 velocity, float duration = .2f) {
		mainImpulseSource.m_ImpulseDefinition.m_TimeEnvelope.m_SustainTime = duration;
		mainImpulseSource.GenerateImpulseAt(position, velocity);
		JoystickUtil.Rumble(1, 200);
	}

	private void OnApplicationQuit() {
		JoystickUtil.Quit();
	}
}