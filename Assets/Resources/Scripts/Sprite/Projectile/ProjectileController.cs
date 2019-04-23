using System;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IBSpriteTrigger))]
public class ProjectileController : MonoBehaviour {

	public string identifierName;
	public float lifespan;
	public float destroyDelay;
	public Vector3 velocity;
	public Collider ownerCollider;
	public ParticleAsset explosionEffect;
	public ParticleSystem.MinMaxGradient explosionEffectColor;
	public AudioAsset hitSound;
	public TrailSettings trailSettings;

	public Action<IBSpriteTrigger, Collider, Vector3> OnDetectCharacterEnter {
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
		if (layer == LayerManager.TerrainLayer || layer == LayerManager.DeviceLayer) {
			if (explosionEffect) {
				BurstParticleController particle = explosionEffect.Get<BurstParticleController>();
				ParticleSystem.MainModule main = particle.ParticleSystem.main;
				main.startColor = explosionEffectColor;
				particle.transform.position = transform.position;
				particle.Burst();
			}
			
			if (hitSound) AudioManager.PlayAtPoint(hitSound.Source, transform.position);
			
			_startTime = Time.time;
			lifespan = destroyDelay;
		} else if (layer == LayerManager.CharacterLayer) {
			if (other == ownerCollider) return;
			_startTime = Time.time;
			lifespan = destroyDelay;
		}
	}
}