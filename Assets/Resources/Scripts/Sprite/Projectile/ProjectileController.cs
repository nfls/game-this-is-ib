using System;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IBSpriteTrigger))]
public class ProjectileController : MonoBehaviour {

	public string identifierName;
	public string hitSound;
	public string hitEffect;
	public float lifespan;
	public float destroyDelay;
	public Vector3 velocity;
	public Collider ownerCollider;
	public TrailSettings trailSettings;

	public Action<IBSpriteTrigger, Collider> OnDetectCharacterEnter {
		get { return _ibSpriteTrigger.onDetectCharacterEnter; }
		set { _ibSpriteTrigger.onDetectCharacterEnter = value; }
	}
	
	public Action<IBSpriteTrigger, Collider> OnDetectCharacterExit {
		get { return _ibSpriteTrigger.onDetectCharacterExit; }
		set { _ibSpriteTrigger.onDetectCharacterExit = value; }
	}

	public DetectionSettings DetectionSettings {
		get { return _ibSpriteTrigger.detectionSettings; }
		set { _ibSpriteTrigger.detectionSettings = value; }
	}

	protected bool _fired;
	protected float _startTime;
	protected TrailRenderer _trailRenderer;
	protected IBSpriteTrigger _ibSpriteTrigger;
	
	protected virtual void Awake() {
		_trailRenderer = GetComponent<TrailRenderer>();
		trailSettings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = false;
		
		_ibSpriteTrigger = GetComponent<IBSpriteTrigger>();
		_ibSpriteTrigger.Disable();
	}

	protected virtual void Update() {
		if (_fired) {
			Move();
			if (Time.time - _startTime >= lifespan) ProjectileManager.Recycle(this);
		}
	}

	public virtual void Fire(Vector3 velocity) {
		this.velocity = velocity;
		_fired = true;
		_startTime = Time.time;
		_trailRenderer.emitting = true;
		_ibSpriteTrigger.Enable();
	}

	protected virtual void Move() {
		transform.position += velocity * Time.deltaTime;
	}

	protected virtual void OnHit() {
		if (!string.IsNullOrEmpty(hitSound)) AudioManager.PlayAtPoint(hitSound, transform.position);
		if (!string.IsNullOrEmpty(hitEffect)) {
			BurstParticleController particleController = ParticleManager.Get<BurstParticleController>(hitEffect);
			particleController.transform.position = transform.position;
		}
	}

	public void Recycle() {
		_trailRenderer.Clear();
		_trailRenderer.emitting = false;
		ownerCollider = null;
		DetectionSettings = null;
		OnDetectCharacterEnter = null;
		OnDetectCharacterExit = null;
		_ibSpriteTrigger.Disable();
		_fired = false;
	}

	private void OnTriggerEnter(Collider other) {
		int layer = other.gameObject.layer;
		if (layer == LayerManager.CharacterLayer || layer == LayerManager.TerrainLayer || layer == LayerManager.DeviceLayer) {
			if (other == ownerCollider) return;
			OnHit();
			_startTime = Time.time;
			lifespan = destroyDelay;
		}
	}
}