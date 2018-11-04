using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class IBSpriteController : MonoBehaviour {

	public CharacterMotor characterMotor;
	public CharacterController characterController;
	public Vector3 initialOffset;
	public Vector3 initialRotation;
	public int commandBufferLength;
	public float staminaCost;
	public AudioAsset attackSound;
	public ParticleAsset hitEffect;
	public ParticleSystem.MinMaxGradient hitEffectColor;
	public AudioAsset hitSound;

	public AttackEffectSettings attackEffectSettings;
	public IdleMovementSettings idleMovementSettings;
	public TrailSettings idleTrailSettings;

	public bool IsAttacking => _isAttacking;
	public bool IsSyncing => _isSyncing;
	
	public Vector3 InitialPosition {
		get {
			Vector3 offset = initialOffset;
			offset.x *= (float) characterMotor.FaceDirection;
			return characterMotor.transform.position + offset;
		}
	}

	public Quaternion InitialRotation => initialRotation.ToQuaternion();

	public abstract DetectionSettings DetectionSettings {
		get;
		set;
	}

	protected bool _isAttacking;
	protected bool _isFollowing;
	protected bool _isSyncing;
	protected bool _isCommandBufferFull;
	protected int _commandBufferCount;
	protected Transform _initialParent;
	protected TrailRenderer _trailRenderer;
	protected AudioSource _audioSource;

	public virtual void OnSwitchOn() {
		gameObject.SetActive(true);
		ResetPositionAndRotation();
	}

	public virtual void OnSwitchOff() {
		if (_isAttacking) CancelAttack();
		
		gameObject.SetActive(false);
	}

	protected abstract void ExeAttackTask();

	protected abstract void CancelAttackTask();
	
	protected virtual void Awake() {
		_initialParent = transform.parent;

		_trailRenderer = gameObject.AddComponent<TrailRenderer>();
		_trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
		_trailRenderer.receiveShadows = false;
		_trailRenderer.allowOcclusionWhenDynamic = true;
		_trailRenderer.autodestruct = false;
		_trailRenderer.emitting = false;

		_audioSource = gameObject.AddComponent<AudioSource>();
		_audioSource.loop = false;
		_audioSource.spatialBlend = 1f;
		_audioSource.playOnAwake = false;
		_audioSource.volume = 1f;
	}
	
	protected virtual void Update() {
		if (!_isAttacking) {
			Vector3 initialPosition = InitialPosition;
			Vector3 displacement = initialPosition - transform.position;
			float distance = displacement.magnitude;
			if (transform.lossyScale.x * (float) characterMotor.FaceDirection < 0) Flip();
			if (_isFollowing) {
				float offsetDistance = initialOffset.magnitude;
				transform.position = Vector3.MoveTowards(transform.position, initialPosition, Mathf.LerpUnclamped(idleMovementSettings.minFollowVelocity * Time.deltaTime, idleMovementSettings.maxFollowVelocity * Time.deltaTime, distance / (idleMovementSettings.maxFollowDistance - offsetDistance)));
				if (transform.position.Equals(initialPosition)) {
					_isFollowing = false;
					DisableTrail();
				}
			} else if (!_isFollowing) {
				if (distance > idleMovementSettings.maxFollowDistance) ResetPositionAndRotation();
				else if (distance > idleMovementSettings.minFollowDistance) {
					_isFollowing = true;
					EnableTrail(idleTrailSettings);
				} else transform.Translate(0, Mathf.Sign(Mathf.Sin(Time.time)) * idleMovementSettings.floatingVelocity * Time.deltaTime, 0);
			}
		}
	}

	public virtual void OnReceiveAttackCommand() {
		if (!_isCommandBufferFull && _commandBufferCount < commandBufferLength && characterController.stamina > 0) {
			_commandBufferCount++;
			if (_commandBufferCount == commandBufferLength) _isCommandBufferFull = true;
			characterController.InterruptStaminaRecovery();
			characterController.CostStamina(staminaCost);
			ExeAttackTask();
		}
	}

	public virtual void OnFinishAttackCommand() {
		
	}

	public void CancelAttack() {
		_commandBufferCount = 0;
		_isCommandBufferFull = false;
		_isAttacking = false;
		characterController.StartStaminaRecovery();
		CancelAttackTask();
	}

	public void Flip() {
		Vector3 scale = transform.localScale;
		scale.x *= -1f;
		transform.localScale = scale;	
	}

	public void ResetPositionAndRotation() {
		transform.position = InitialPosition;
		transform.rotation = initialRotation.ToQuaternion();
	}

	public void EnterCharacterSyncState() {
		/*
		Vector3 scale = transform.localScale;
		scale.x *= (float) characterMotor.FaceDirection;
		transform.localScale = scale;
		*/
		_isSyncing = true;
		transform.parent = characterMotor.transform;
	}

	public void ExitCharacterSyncState() {
		/*
		Vector3 scale = transform.localScale;
		scale.x *= (float) characterMotor.FaceDirection;
		transform.localScale = scale;
		*/
		_isSyncing = false;
		transform.parent = _initialParent;
	}

	public void EnableTrail(TrailSettings settings) {
		settings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = true;
	}

	public void DisableTrail() {
		_trailRenderer.emitting = false;
	}

	protected virtual void OnDetectCharacterEnter(IBSpriteTrigger trigger, Collider detectedCollider) {
		CharacterController character = detectedCollider.GetComponentInParent<CharacterController>();
		float hitDirection = GetHitDirection(detectedCollider.transform, characterMotor.transform);
		if (attackEffectSettings.doesHit) character.GetHit(attackEffectSettings.hitVelocityX * hitDirection, attackEffectSettings.hitVelocityY);
		if (attackEffectSettings.doesStun) character.GetStunned(hitDirection * attackEffectSettings.stunAngle, attackEffectSettings.stunTime);
		if (attackEffectSettings.doesDamage) character.GetDamaged(attackEffectSettings.damage);
		if (hitEffect) {
			BurstParticleController particle = hitEffect.Get<BurstParticleController>();
			ParticleSystem.MainModule main = particle.ParticleSystem.main;
			main.startColor = hitEffectColor;
			particle.transform.position = trigger.transform.position;
			particle.Burst();
		}
		
		CameraManager.RadialBlur(CameraManager.MainCamera.WorldToViewportPoint(trigger.transform.position));
		FindObjectOfType<TestSceneController>().CombatClearShot(characterController.transform, detectedCollider.transform);
	}

	protected virtual void OnDetectCharacterExit(IBSpriteTrigger trigger, Collider detectedCollider) {
		
	}

	protected float GetHitDirection(Transform receiver, Transform attacker) {
		return receiver.position.x - attacker.position.x > 0 ? 1 : -1;
	}
}

[Serializable]
public class AttackEffectSettings {

	public bool doesHit;
	public bool doesStun;
	public bool doesDamage;
	public float hitVelocityX;
	public float hitVelocityY;
	public float stunTime;
	public float stunAngle;
	public float damage;
}

[Serializable]
public class IdleMovementSettings {
		
	public float floatingVelocity;
	public float minFollowVelocity;
	public float maxFollowVelocity;
	public float minFollowDistance;
	public float maxFollowDistance;
}