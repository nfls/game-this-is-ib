using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DamageTrigger))]
public class PendulumSpriteController : IBSpriteController {

	public float attackRotation;
	public float rotateSpeed;
	public RotateBackMode rotateBackMode;
	public TrailSettings attackTrailSettings;

	protected DamageTrigger _damageTrigger;
	protected Coroutine _attackCoroutine;
	protected Coroutine _rotateCoroutine;

	public override void Init() {
		base.Init();
		
		attackTrailSettings.Init();
		_damageTrigger = GetComponent<DamageTrigger>();
		_damageTrigger.Disable();
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
			if (rotation < 0) {
				break;
			}
		}
	}

	protected virtual IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		EnableTrail(attackTrailSettings);
		EnterCharacterSyncState();
		_damageTrigger.Enable();
		while (_commandBufferCount > 0) {
			_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Clockwise));
			yield return _rotateCoroutine;
			_rotateCoroutine = null;

			if (rotateBackMode == RotateBackMode.Immediate) {
				yield return null;
				ResetPositionAndRotation();
			} else {
				_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Anticlockwise));
				yield return _rotateCoroutine;
				ResetPositionAndRotation();
			}

			_commandBufferCount--;
		}
		_damageTrigger.Disable();
		_isCommandBufferFull = false;
		ExitCharacterSyncState();
		DisableTrail();
		_attackCoroutine = null;
		_isAttacking = false;
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