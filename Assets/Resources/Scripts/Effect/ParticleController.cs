using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public string identifierName;

	public ParticleSystem ParticleSystem => _system;
	
	protected bool _started;
	protected ParticleSystem _system;

	private void Awake() {
		_system = GetComponent<ParticleSystem>();
	}

	protected virtual void Update() {
		if (_started && !_system.isPlaying) {
			ParticleManager.Recycle(this);
			_recycled = true;
		}
	}

	public virtual void Recycle() {
		_started = false;
	}

	private bool _recycled;

	public void Debug() {
		UnityEngine.Debug.Log("_started = " + _started);
		UnityEngine.Debug.Log("isPlaying = " + _system.isPlaying);
		UnityEngine.Debug.Log("_recycled = " + _recycled);
	}
}