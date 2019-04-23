using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IBSpriteTrigger))]
public class PendulumSpriteController : IBSpriteController {

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
	}

	protected override void ExeAttackTask() {
		if (!_isAttacking) _attackCoroutine = StartCoroutine(ExeAttackCoroutine());
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
			
			_isCommandBufferFull = false;
			ExitCharacterSyncState();
			DisableTrail();
			_attackCoroutine = null;
			_isAttacking = false;
			characterController.StartStaminaRecovery();
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

	protected IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		EnableTrail(attackTrailSettings);
		EnterCharacterSyncState();
		transform.right = (characterController.transform.position - transform.position * (float) characterMotor.FaceDirection) * (float) characterMotor.FaceDirection;
		while (_commandBufferCount > 0) {
			_ibSpriteTrigger.Enable();
			if (attackSound) _audioSource.PlayOneShot(attackSound.Source);
			_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Clockwise));
			yield return _rotateCoroutine;
			_rotateCoroutine = null;
			if (rotateBackMode == RotateBackMode.Immediate) yield return null;
			else {
				_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Anticlockwise));
				yield return _rotateCoroutine;
			}

			ResetPositionAndRotation();
			_ibSpriteTrigger.Disable();
			_commandBufferCount--;
		}
		
		_isCommandBufferFull = false;
		ExitCharacterSyncState();
		DisableTrail();
		_attackCoroutine = null;
		_isAttacking = false;
		characterController.StartStaminaRecovery();
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