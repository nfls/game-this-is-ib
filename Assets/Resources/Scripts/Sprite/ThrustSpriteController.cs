using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(IBSpriteTrigger))]
public class ThrustSpriteController : IBSpriteController {

	public Vector3 hitShake;
	public TimeEffectRequest hitTimeEffect;
	public float thrustDuration;
	public Vector3 thrustOffset;
	public Vector3 thrustRotation;
	public ThrustBackMode thrustBackMode;
	public TrailSettings attackTrailSettings;

	public override DetectionSettings DetectionSettings {
		get { return _ibSpriteTrigger.detectionSettings; }
		set { _ibSpriteTrigger.detectionSettings = value; }
	}

	public Vector3 ThrustPosition {
		get {
			Vector3 offset = initialOffset + thrustOffset;
			offset.x *= (float) characterMotor.FaceDirection;
			return characterMotor.transform.position + offset;
		}
	}

	protected IBSpriteTrigger _ibSpriteTrigger;
	protected Coroutine _attackCoroutine;
	
	protected override void Awake() {
		base.Awake();
		
		_ibSpriteTrigger = GetComponent<IBSpriteTrigger>();
		_ibSpriteTrigger.onDetectCharacterEnter = OnDetectCharacterEnter;
		_ibSpriteTrigger.onDetectCharacterExit = OnDetectCharacterExit;
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
			
			ExitCharacterSyncState();
			DisableTrail();
			ResetPositionAndRotation();
		}
	}

	protected IEnumerator ExeAttackCoroutine() {
		_isFollowing = false;
		_isAttacking = true;
		ResetPositionAndRotation();
		EnableTrail(attackTrailSettings);
		EnterCharacterSyncState();
		while (_commandBufferCount > 0) {
			_ibSpriteTrigger.Enable();
			if (!string.IsNullOrEmpty(attackSound)) _audioSource.PlayOneShot(ResourcesManager.GetAudio(attackSound));
			float timeStart = Time.time;
			float timeDiff;
			float lerp;
			do {
				yield return null;
				timeDiff = Time.time - timeStart;
				lerp = timeDiff / thrustDuration;
				transform.position = Vector3.Lerp(InitialPosition, ThrustPosition, lerp);
			} while (timeDiff < thrustDuration);
			
			if (thrustBackMode == ThrustBackMode.Immediate) {
				yield return null;
				ResetPositionAndRotation();
			} else {
				timeStart = Time.time;
				do {
					yield return null;
					timeDiff = Time.time - timeStart;
					lerp = timeDiff / thrustDuration;
					transform.position = Vector3.Lerp(ThrustPosition, InitialPosition, lerp);
				} while (timeDiff < thrustDuration);
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

public enum ThrustBackMode {
	Immediate,
	Inverse
}