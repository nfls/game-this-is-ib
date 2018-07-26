using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IBSpriteTrigger))]
public class PendulumSpriteController : IBSpriteController {

	public float attackRotation;
	public float rotateSpeed;
	public RotateBackMode rotateBackMode;
	public TrailSettings attackTrailSettings;

	protected IBSpriteTrigger ibSpriteTrigger;
	protected Coroutine _attackCoroutine;
	protected Coroutine _rotateCoroutine;

	public override void Init() {
		base.Init();
		
		attackTrailSettings.Init();
		ibSpriteTrigger = GetComponent<IBSpriteTrigger>();
		ibSpriteTrigger.Disable();
		ibSpriteTrigger.detectionSettings = detectionSettings;
		ibSpriteTrigger.OnDetectCharacterEnter += OnDetectCharacterEnter;
		ibSpriteTrigger.OnDetectCharacterExit += OnDetectCharacterExit;
		ibSpriteTrigger.OnDetectDestructibleEnter += OnDetectDestrutibleEnter;
		ibSpriteTrigger.OnDetectCharacterExit += OnDetectDestrutibleExit;
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
		while (_commandBufferCount > 0) {
			ibSpriteTrigger.Enable();
			_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Clockwise));
			yield return _rotateCoroutine;
			_rotateCoroutine = null;
			ibSpriteTrigger.Disable();

			ibSpriteTrigger.Enable();
			if (rotateBackMode == RotateBackMode.Immediate) {
				yield return null;
				ResetPositionAndRotation();
			} else {
				_rotateCoroutine = StartCoroutine(ExeRotateCoroutine(RotationDirection.Anticlockwise));
				yield return _rotateCoroutine;
				ResetPositionAndRotation();
			}

			ibSpriteTrigger.Disable();
			_commandBufferCount--;
		}
		_isCommandBufferFull = false;
		ExitCharacterSyncState();
		DisableTrail();
		_attackCoroutine = null;
		_isAttacking = false;
	}

	protected override void OnDetectCharacterEnter(IBSpriteTrigger trigger, Collider detectedCollider) {
		CharacterController character = detectedCollider.GetComponentInParent<CharacterController>();
		
		if (attackEffectSettings.doesHit) {
			character.GetHit(attackEffectSettings.hitVelocityX, attackEffectSettings.hitVelocityY);
		}

		if (attackEffectSettings.doesStun) {
			character.GetStunned(GetStunAngle(attackEffectSettings.stunAngle, trigger.transform, detectedCollider.transform), attackEffectSettings.stunTime);
		}

		if (attackEffectSettings.doesDamage) {
			character.GetDamaged(attackEffectSettings.damage);
		}
	}

	protected override void OnDetectDestrutibleEnter(IBSpriteTrigger trigger, Collider detectedCollider) {
		
	}
	
	protected override void OnDetectCharacterExit(IBSpriteTrigger trigger, Collider detectedCollider) {
		
	}

	protected override void OnDetectDestrutibleExit(IBSpriteTrigger trigger, Collider detectedCollider) {
		
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