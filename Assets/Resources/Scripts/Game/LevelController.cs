using System.Collections;
using UnityEngine;

public class LevelController : MonoBehaviour {
	
	public int Width {
		get { return _width; }
	}
	
	public int Height {
		get { return _height; }
	}

	public CharacterController LocalPlayer {
		get { return _localPlayer; }
	}

	public CharacterController RemotePlayer {
		get { return _remotePlayer; }
	}

	[SerializeField]
	protected int _width;
	[SerializeField]
	protected int _height;
	protected bool _isShifiting;

	protected CharacterController _localPlayer;
	protected CharacterController _remotePlayer;

	protected Transform terrainRoot;
	protected Transform deviceRoot;

	protected TerrainController[] terrains;
	protected DeviceController[] devices;

	protected Coroutine _shiftCoroutine;

	private void Start() {
		terrainRoot = transform.Find("Terrains");
		deviceRoot = transform.Find("Devices");

		terrains = terrainRoot.GetComponentsInChildren<TerrainController>();
		devices = deviceRoot.GetComponentsInChildren<DeviceController>();
	}

	public void Shift(Vector3 destination, float time) {
		
	}

	public void Activate() {
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

	protected IEnumerator ExeShiftCoroutine(Vector3 destination, float time) {
		float speed = (destination - transform.position).magnitude / time;
		
		while (time > 0) {
			yield return null;
			time -= Time.time;
			
		}
	}
	
	#if UNITY_EDITOR
	private void OnDrawGizmos() {
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, new Vector3(1.01f, 1.01f, 2.02f));
	}
	#endif
}