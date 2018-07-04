using System.Collections;
using UnityEngine;

public class ShooterSpriteController : IBSpriteController {

	public Vector3 fireOrigin;
	public float firePower;
	public float recoilDistance;
	public float recoilRotation;
	public float recoilSpeed;
	public float recoilAngularSpeed;

	public Vector3 FireOrigin {
		get {
			Vector3 offset = fireOrigin;
			offset.x *= (float) characterMotor.FaceDirection;
			return characterMotor.transform.position + offset;
		}
	}
	
	protected Coroutine _attackCoroutine;
	protected Coroutine _recoilCoroutine;

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

	protected virtual IEnumerator ExeRecoilCoroutine(RecoilDirection direction) {
		float distance = recoilDistance;
		float rotation = recoilRotation;
		bool finishD = false;
		bool finishR = false;
		
		while (true) {
			yield return null;
			if (!finishD) {
				float d = recoilSpeed * Time.deltaTime;
				transform.position = transform.position + new Vector3(d * (float) direction * (float) characterMotor.FaceDirection, 0, 0);
				distance -= d;
				if (distance < 0) {
					finishD = true;
				}
			}

			if (!finishR) {
				float r = recoilAngularSpeed * Time.deltaTime;
				transform.Rotate(0, 0, -r * (float) direction);
				rotation -= r;
				if (rotation < 0) {
					finishR = true;
				}
			}

			if (finishD && finishR) {
				break;
			}
		}
	}

	protected virtual IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		DisableTrail();
		EnterCharacterSyncState();
		while (_commandBufferCount > 0) {
			// todo shoot bullets
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
}

public enum RecoilDirection {
	Backwards = -1,
	Forwards = 1
}