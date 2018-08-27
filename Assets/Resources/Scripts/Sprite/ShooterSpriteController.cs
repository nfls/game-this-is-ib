using System.Collections;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class ShooterSpriteController : IBSpriteController {

	public string projectileType;
	public Vector3 fireOffset;
	public Vector3 fireRotation;
	public float firePower;
	public Vector3 fireShake;
	public string fireEffect;
	public string hitEffect;
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
	}

	protected virtual void Fire() {
		ProjectileController projectileController = ProjectileManager.Get(projectileType);
		LoadUpProjectile(projectileController);
		projectileController.Fire(new Vector3(firePower * (float) characterMotor.FaceDirection, 0, 0));
		if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
		if (fireShake != Vector3.zero) CameraManager.Shake(transform.position, fireShake);
		if (!string.IsNullOrEmpty(fireEffect)) {
			BurstParticleController explosion = ParticleManager.Get<BurstParticleController>(fireEffect);
			explosion.transform.position = transform.position + new Vector3(transform.lossyScale.x / 2f, 0, 0);
			explosion.Burst();
		}
	}

	protected virtual void LoadUpProjectile(ProjectileController projectileController) {
		projectileController.transform.position = FireOrigin;
		Vector3 rotation = fireRotation;
		rotation.z *= (float) characterMotor.FaceDirection;
		projectileController.transform.rotation = rotation.ToQuaternion();
		projectileController.hitSound = hitSound;
		projectileController.hitEffect = hitEffect;
		projectileController.lifespan = lifespan;
		projectileController.ownerCollider = characterMotor.BodyCollider;
		projectileController.OnDetectCharacterEnter = OnDetectCharacterEnter;
		projectileController.OnDetectCharacterExit = OnDetectCharacterExit;
		projectileController.DetectionSettings = DetectionSettings;
	}
}

public enum RecoilDirection {
	Backwards = -1,
	Forwards = 1
}