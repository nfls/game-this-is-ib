using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleController : MonoBehaviour {

	public string identifierName;
	
	protected bool _started;
	protected ParticleSystem _system;

	protected virtual void Update() {
		if (_started && !_system.isPlaying) ParticlePool.Recycle(this);
	}
	
	public virtual void Init() {
		if (!_system) _system = GetComponent<ParticleSystem>();
		_started = true;
	}

	public virtual void Recycle() {
		
	}
}