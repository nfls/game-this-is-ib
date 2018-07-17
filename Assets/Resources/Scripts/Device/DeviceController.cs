using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class DeviceController : MonoBehaviour {

	protected bool _isEnabled;

	protected virtual void Start() { }

	public virtual void Replay() {
		Play();
	}

	public virtual void Play() {
		_isEnabled = true;
	}

	public virtual void Pause() {
		_isEnabled = false;
	}
}