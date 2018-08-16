using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IBSpriteTrigger))]
public class PendulumSpriteController : IBSpriteController {

	public Vector3 hitShake;
	public TimeEffectRequest hitTimeEffect;
	public float attackRotation;
	public float rotateSpeed;
	public RotateBackMode rotateBackMode;
	public TrailSettings attackTrailSettings;

	public override DetectionSettings DetectionSettings {
		get { return _ibSpriteTrigger.detectionSettings; }
		set { _ibSpriteTrigger.detectionSettings = value; }
	}

	protected IBSpriteTrigger _ibSpriteTrigger;
	protected Coroutine _attackCoroutine;
	protected Coroutine _rotateCoroutine;

	protected override void Awake() {
		base.Awake();
		
		_ibSpriteTrigger = GetComponent<IBSpriteTrigger>();
		_ibSpriteTrigger.onDetectCharacterEnter = OnDetectCharacterEnter;
		_ibSpriteTrigger.onDetectCharacterExit = OnDetectCharacterExit;
		_ibSpriteTrigger.onDetectDestructibleEnter = OnDetectDestructibleEnter;
		_ibSpriteTrigger.onDetectDestructibleExit = OnDetectDestructibleExit;
	}

	protected override void ExeAttackTask() {
		if (!_isAttacking) {
			_attackCoroutine = StartCoroutine(ExeAttackCoroutine());
		}
	}

	protected override void CancelAttackTask() {
		if (_isAttacking) {
			if (_attackCoroutine != null) {
				StopCoroutine(_attackCoroutine);
				_attackCoroutine = null;
			}
			if (_rotateCoroutine != null) {
				StopCoroutine(_rotateCoroutine);
				_rotateCoroutine = null;
			}
			ExitCharacterSyncState();
			DisableTrail();
			ResetPositionAndRotation();
		}
	}
	
	public IEnumerator ExeRotateCoroutine(RotationDirection direction) {
		float rotation = attackRotation;
		while (true) {
			yield return null;
			float r = rotateSpeed * Time.deltaTime;
			transform.RotateAround(characterMotor.transform.position, Vector3.back * (float) characterMotor.FaceDirection, r * (float) direction);
			rotation -= r;
			if (rotation < 0) break;
		}
	}

	protected virtual IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		EnableTrail(attackTrailSettings);
		EnterCharacterSyncState();
		while (_commandBufferCount > 0) {
			_ibSpriteTrigger.Enable();
			if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
			_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Clockwise));
			yield return _rotateCoroutine;
			_rotateCoroutine = null;
			_ibSpriteTrigger.Disable();

			_ibSpriteTrigger.Enable();
			if (rotateBackMode == RotateBackMode.Immediate) {
				yield return null;
				ResetPositionAndRotation();
			} else {
				_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Anticlockwise));
				yield return _rotateCoroutine;
				ResetPositionAndRotation();
			}

			_ibSpriteTrigger.Disable();
			_commandBufferCount--;
		}
		_isCommandBufferFull = false;
		ExitCharacterSyncState();
		DisableTrail();
		_attackCoroutine = null;
		_isAttacking = false;
	}

	protected override void OnDetectCharacterEnter(IBSpriteTrigger trigger, Collider detectedCollider) {
		base.OnDetectCharacterEnter(trigger, detectedCollider);
		if (!string.IsNullOrEmpty(hitSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(hitSound));
		CameraManager.Shake(trigger.transform.position, hitShake);
		TimeManager.HandleRequest(hitTimeEffect);
	}
}

public enum RotationDirection {
	Anticlockwise = -1,
	Clockwise = 1
}

public enum RotateBackMode {
	Immediate,
	Inverse
}