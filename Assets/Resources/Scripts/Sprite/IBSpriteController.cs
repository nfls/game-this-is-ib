using System;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class IBSpriteController : MonoBehaviour {

	public CharacterMotor characterMotor;
	public Vector3 initialOffset;
	public Vector3 initialRotation;
	public int commandBufferLength;

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

	protected bool _isInitiated;
	protected bool _isAttacking;
	protected bool _isFollowing;
	protected bool _isSyncing;
	protected bool _isCommandBufferFull;
	protected int _commandBufferCount;
	protected Transform _initialParent;
	protected TrailRenderer _trailRenderer;

	public virtual void OnSwitchOn() {
		gameObject.SetActive(true);
		ResetPositionAndRotation();
	}

	public virtual void OnSwitchOff() {
		if (_isAttacking) {
			CancelAttack();
		}
		
		gameObject.SetActive(false);
	}

	protected abstract void ExeAttackTask();

	protected abstract void CancelAttackTask();

	private void Start() {
		if (_isInitiated) return;
		
		Init();
	}

	private void Update() {
		if (!_isAttacking) {
			IdleUpdate();
		}
	}

	public virtual void Init() {
		_isInitiated = true;
		_initialParent = transform.parent;

		_trailRenderer = gameObject.AddComponent<TrailRenderer>();
		_trailRenderer.shadowCastingMode = ShadowCastingMode.Off;
		_trailRenderer.receiveShadows = false;
		_trailRenderer.allowOcclusionWhenDynamic = true;
		_trailRenderer.autodestruct = false;
		_trailRenderer.emitting = false;
		
		idleTrailSettings.Init();
	}

	protected void IdleUpdate() {
		Vector3 initialPosition = InitialPosition;
		Vector3 displacement = initialPosition - transform.position;
		float distance = displacement.magnitude;
		if (transform.lossyScale.x * (float) characterMotor.FaceDirection < 0) {
			Flip();
		}
		
		if (_isFollowing) {
			float offsetDistance = initialOffset.magnitude;
			transform.position = Vector3.MoveTowards(transform.position, initialPosition, Mathf.LerpUnclamped(idleMovementSettings.minFollowVelocity * Time.deltaTime, idleMovementSettings.maxFollowVelocity * Time.deltaTime, distance / (idleMovementSettings.maxFollowDistance - offsetDistance)));
			if (transform.position.Equals(initialPosition)) {
				_isFollowing = false;
				DisableTrail();
			}
		} else if (!_isFollowing) {
			if (distance > idleMovementSettings.maxFollowDistance) {
				ResetPositionAndRotation();
			} else if (distance > idleMovementSettings.minFollowDistance) {
				_isFollowing = true;
				EnableTrail(idleTrailSettings);
			} else {
				transform.Translate(0, Mathf.Sign(Mathf.Sin(Time.time)) * idleMovementSettings.floatingVelocity * Time.deltaTime, 0);
			}
		}
	}

	public void Attack() {
		if (!_isCommandBufferFull && _commandBufferCount < commandBufferLength) {
			_commandBufferCount++;
			if (_commandBufferCount == commandBufferLength) {
				_isCommandBufferFull = true;
			}
			ExeAttackTask();
		}
	}

	public void CancelAttack() {
		_commandBufferCount = 0;
		_isCommandBufferFull = false;
		_isAttacking = false;
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
		transform.parent = characterMotor.transform;
	}

	public void ExitCharacterSyncState() {
		/*
		Vector3 scale = transform.localScale;
		scale.x *= (float) characterMotor.FaceDirection;
		transform.localScale = scale;
		*/
		transform.parent = _initialParent;
	}

	public void EnableTrail(TrailSettings settings) {
		settings.InitRenderer(ref _trailRenderer);
		_trailRenderer.emitting = true;
	}

	public void DisableTrail() {
		_trailRenderer.emitting = false;
	}

	[Serializable]
	public class IdleMovementSettings {
		
		public float floatingVelocity;
		public float minFollowVelocity;
		public float maxFollowVelocity;
		public float minFollowDistance;
		public float maxFollowDistance;
	}
}