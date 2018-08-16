using System.Collections.Generic;
using UnityEngine;

public class ParticleDecalController : MonoBehaviour {

	[Range(0f, 1f)]
	public float decalRate = 1f;
	public float minSize;
	public float maxSize;
	public Gradient colorGradient;
	
	private ParticleSystem _particleSystem;
	private readonly List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>(4);

	private void Awake() {
		_particleSystem = GetComponent<ParticleSystem>();
	}
	
	private void OnParticleCollision(GameObject other) {
		int count = _particleSystem.GetCollisionEvents(other, _collisionEvents);

		for (int i = 0; i < count; i++) {
			float r = Random.Range(0f, 1f);
			if (r <= decalRate) ParticleDecalManager.OnParticleHit(_collisionEvents[i], Random.Range(minSize, maxSize), colorGradient.Evaluate(r));
		}
		
		ParticleDecalManager.DisplayParticles();
	}
}