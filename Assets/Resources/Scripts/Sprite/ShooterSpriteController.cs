using System;
using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ShooterSpriteController : IBSpriteController {

	public ProjectileAsset projectileType;
	public Vector3 fireOffset;
	public Vector3 fireRotation;
	public float firePower;
	public Vector3 fireShake;
	public ParticleAsset fireEffect;
	public ParticleAsset explosionEffect;
	public ParticleSystem.MinMaxGradient explosionEffectColor;
	public float lifespan;
	public float recoilTime;
	public float recoilDistance;
	public float recoilRotation;

	public Vector3 FireOrigin {
		get {
			Vector3 offset = fireOffset;
			offset.x *= (float) characterMotor.FaceDirection;
			return characterMotor.transform.position + offset;
		}
	}

	public override DetectionSettings DetectionSettings { get; set; }
	
	protected Coroutine _attackCoroutine;
	protected Coroutine _recoilCoroutine;

	protected override void ExeAttackTask() {
		if (!_isAttacking) _attackCoroutine = StartCoroutine(ExeAttackCoroutine());
	}

	protected override void CancelAttackTask() {
		if (_isAttacking) {
			if (_attackCoroutine != null) {
				StopCoroutine(_attackCoroutine);
				_attackCoroutine = null;
			}
			
			if (_recoilCoroutine != null) {
				StopCoroutine(_recoilCoroutine);
				_recoilCoroutine = null;
			}
			
			ExitCharacterSyncState();
			ResetPositionAndRotation();
			_commandBufferCount = 0;
			_isCommandBufferFull = false;
			_isAttacking = false;
		}
	}

	private IEnumerator ExeRecoilCoroutine(RecoilDirection direction) {
		float startTime = Time.time;
		Vector3 originalPos = transform.localPosition;
		Quaternion originalRot = transform.localRotation;
		float directionC = (float) direction;
		Vector3 targetPos = originalPos + new Vector3(recoilDistance * directionC, 0, 0);
		Quaternion targetRot = originalRot * new Vector3(0, 0, -recoilRotation * directionC).ToQuaternion();
		
		while (true) {
			yield return null;
			float timeDiff = Time.time - startTime;
			float lerp = timeDiff / recoilTime;
			transform.localPosition = Vector3.Lerp(originalPos, targetPos, lerp);
			transform.localRotation = Quaternion.Lerp(originalRot, targetRot, lerp);
			if (timeDiff >= recoilTime) break;
		}
	}

	private IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		DisableTrail();
		EnterCharacterSyncState();
		while (_commandBufferCount > 0) {
			Fire();
			_recoilCoroutine = StartCoroutine(ExeRecoilCoroutine(RecoilDirection.Backwards));
			yield return _recoilCoroutine;
			_recoilCoroutine = StartCoroutine(ExeRecoilCoroutine(RecoilDirection.Forwards));
			yield return _recoilCoroutine;
			_recoilCoroutine = null;
			ResetPositionAndRotation();
			_commandBufferCount--;
		}
		
		_isCommandBufferFull = false;
		ExitCharacterSyncState();
		_attackCoroutine = null;
		_isAttacking = false;
		
		characterController.StartStaminaRecovery();
	}

	protected virtual void Fire() {
		ProjectileController projectileController = projectileType.Get<ProjectileController>();
		LoadUpProjectile(projectileController);
		projectileController.Fire(new Vector3(firePower * (float) characterMotor.FaceDirection, 0, 0));
		if (attackSound) _audioSource.PlayOneShot(attackSound.Source);
		if (fireShake != Vector3.zero) CameraManager.Shake(transform.position, fireShake);
		if (fireEffect) {
			BurstParticleController explosion = fireEffect.Get<BurstParticleController>();
			explosion.transform.position = transform.position + new Vector3(transform.lossyScale.x / 2f, 0, 0);
			explosion.Burst();
		}
	}

	protected virtual void LoadUpProjectile(ProjectileController projectileController) {
		projectileController.transform.position = FireOrigin;
		Vector3 rotation = fireRotation;
		rotation.z *= (float) characterMotor.FaceDirection;
		projectileController.transform.rotation = rotation.ToQuaternion();
		projectileController.lifespan = lifespan;
		projectileController.ownerCollider = characterMotor.BodyCollider;
		projectileController.explosionEffect = explosionEffect;
		projectileController.explosionEffectColor = explosionEffectColor;
		projectileController.hitSound = hitSound;
		projectileController.OnDetectCharacterEnter = OnDetectCharacterEnter;
		projectileController.OnDetectCharacterExit = OnDetectCharacterExit;
		projectileController.DetectionSettings = DetectionSettings;
	}

	protected override void DisplayPlayerDamageEffect(float distance, float hitDirection, Vector3 contactPosition) => DamageRumbleEffect(distance, hitDirection, 5000);
}

public enum RecoilDirection {
	Backwards = -1,
	Forwards = 1
}