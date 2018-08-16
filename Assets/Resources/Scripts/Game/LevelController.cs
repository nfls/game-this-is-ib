using System;
using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour {
	
	[CameraBackgroundColor]
	public Color backgroundColor;
	
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
		
		#if UNITY_EDITOR
		Activate();
		#endif
	}

	public void Shift(Vector3 destination, float time, Action finishAction) {
		if (_shiftCoroutine != null) {
			StopCoroutine(_shiftCoroutine);
		}

		_shiftCoroutine = StartCoroutine(ExeShiftCoroutine(destination, time, finishAction));
	}

	public void Activate() {
		Camera.main.backgroundColor = backgroundColor;
		foreach (var device in devices) {
			device.Replay();
		}
	}

	public void Deactivate() {
		foreach (var device in devices) {
			device.Replay();
			device.Pause();
		}
	}

	public void Resume() {
		foreach (var device in devices) {
			device.Play();
		}
	}

	public void Pause() {
		foreach (var device in devices) {
			device.Pause();
		}
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
	
#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, new Vector3(1.01f, 1.01f, 2.02f));
	}
#endif
}