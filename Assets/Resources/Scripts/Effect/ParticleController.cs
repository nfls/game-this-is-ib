using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public string identifierName;
	
	protected bool _started;
	protected ParticleSystem _system;

	private void Awake() {
		_system = GetComponent<ParticleSystem>();
	}

	protected virtual void Update() {
		if (_started && !_system.isPlaying) ParticleManager.Recycle(this);
	}
	
	public virtual void Init() {
		_started = true;
	}

	public virtual void Recycle() {
		
	}
}