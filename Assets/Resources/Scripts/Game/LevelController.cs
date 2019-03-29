using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class LevelController : MonoBehaviour {
	
	[CameraBackgroundColor]
	public Color backgroundColor;
	public float scanTime = 10f;

	public bool changeMaterial;
	public Material terrainMaterial;
	public Material deviceMaterial;
	
	public int Width => _width;
	public int Height => _height;

	public CharacterController LocalPlayer => _localPlayer;
	public CharacterController RemotePlayer => _remotePlayer;

	protected bool _isShifiting;
	[SerializeField]
	protected int _width;
	[SerializeField]
	protected int _height;

	protected CharacterController _localPlayer;
	protected CharacterController _remotePlayer;

	protected Transform terrainRoot;
	protected Transform deviceRoot;

	protected DeviceController[] devices;

	protected Coroutine _shiftCoroutine;

	private void Awake() {
		terrainRoot = transform.Find("Terrains");
		deviceRoot = transform.Find("Devices");

		devices = deviceRoot.GetComponentsInChildren<DeviceController>();
		
		StaticBatchingUtility.Combine(terrainRoot.gameObject);
		if (changeMaterial) {
			foreach (MeshRenderer renderer in terrainRoot.GetComponentsInChildren<MeshRenderer>()) renderer.material = terrainMaterial;
			foreach (MeshRenderer renderer in deviceRoot.GetComponentsInChildren<MeshRenderer>()) renderer.material = deviceMaterial;
		}
	}

	private void Start() {
		StartCoroutine(ExeScanCoroutine(scanTime));
	}

	public void Shift(Vector3 destination, float time, Action finishAction) {
		if (_shiftCoroutine != null) {
			StopCoroutine(_shiftCoroutine);
			_shiftCoroutine = null;
		}

		_shiftCoroutine = StartCoroutine(ExeShiftCoroutine(destination, time, finishAction));
	}

	public void Activate() {
		Camera.main.backgroundColor = backgroundColor;
		foreach (var device in devices) device.Replay();
	}

	public void Deactivate() {
		foreach (var device in devices) {
			device.Replay();
			device.Pause();
		}
	}

	public void Resume() {
		foreach (var device in devices) device.Play();
	}

	public void Pause() {
		foreach (var device in devices) device.Pause();
	}

	protected IEnumerator ExeScanCoroutine(float time) {
		SceneScanEffectController effectController = CameraManager.MainCamera.GetComponent<SceneScanEffectController>();
		effectController.distance = 0;
		effectController.enabled = true;
		float maxDistance = Mathf.Sqrt(_width * _width + _height * _height);
		float originalTime = Time.time;
		float targetTime = originalTime + time;
		float currentTime;
		do {
			yield return null;
			currentTime = Time.time;
			effectController.distance = (currentTime - originalTime) / time * maxDistance;
		} while (currentTime < targetTime);

		effectController.enabled = false;
	}

	protected IEnumerator ExeShiftCoroutine(Vector3 destination, float time, Action finishAction) {
		_isShifiting = true;
		Vector3 originalPos = transform.position;
		float originalTime = Time.time;
		float targetTime = originalTime + time;
		float currentTime;
		do {
			yield return null;
			currentTime = Time.time;
			transform.position = Vector3.LerpUnclamped(originalPos, destination, (currentTime - originalTime) / time);
		} while (currentTime < targetTime);

		transform.position = destination;
		_shiftCoroutine = null;
		_isShifiting = false;
		
		finishAction();
	}
	
	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, new Vector3(1.01f, 1.01f, 2.02f));
	}
}