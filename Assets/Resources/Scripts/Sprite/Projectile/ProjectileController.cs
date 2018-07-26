using UnityEngine;

public abstract class ProjectileController : MonoBehaviour {

	public float lifespan;
	public TrailSettings trailSettings;

	protected bool _fired;
	protected float _startTime;
	protected TrailRenderer _trailRenderer;
	protected IBSpriteTrigger ibSpriteTrigger;
	
	private void Start() {
		trailSettings.Init();
		_trailRenderer = GetComponent<TrailRenderer>();
		
		ibSpriteTrigger = GetComponent<IBSpriteTrigger>();
		ibSpriteTrigger.Disable();
	}

	public void Update() {
		if (_fired) {
			Move();
			if (Time.time - _startTime >= lifespan) {
				Destroy(gameObject);
			}
		}
	}

	public void Fire() {
		_fired = true;
		_startTime = Time.time;
		trailSettings.InitRenderer(ref _trailRenderer);
		ibSpriteTrigger.Enable();
	}

	protected abstract void Move();
}