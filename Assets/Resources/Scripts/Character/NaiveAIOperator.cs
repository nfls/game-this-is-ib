using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class NaiveAIOperator : CharacterOperator {

	public float thinkIntern = .1f;
	public float detectionDistance = 8f;
	public float maximumSight = 20f;
	public float jumpRequirementHeight = .5f;
	public float minAttackDistance = 1f;
	public float maxAttackDistance = 2f;
	public bool onlyAttackWithStamina = true;
	[Range(0f, 1f)]
	public float aggressiveness = .0f;

	protected CharacterController _target;
	protected Action _currerntAction;
	protected RaycastHit[] hitResults = new RaycastHit[5];

	protected bool _hasAttackedLastTime;
	protected float _lastThinkTime;

	protected override void Awake() {
		base.Awake();
		_characterController.SwitchOnIBSprite();
	}

	protected void Update() {
		if (Time.time - _lastThinkTime > thinkIntern) {
			_lastThinkTime = Time.time;
			Think();
		} 
		
		_currerntAction?.Invoke();
	}

	protected virtual void Think() {
		_currerntAction = null;
		bool _hasAttacked = false;
		if (_target) {
			Vector3 diff = _target.transform.position - transform.position;
			float distanceSqr = diff.sqrMagnitude;
			if (distanceSqr > maximumSight * maximumSight/* || Physics.Linecast(transform.position, _target.transform.position, 1 << LayerManager.TerrainLayer | 1 << LayerManager.DeviceLayer, QueryTriggerInteraction.Ignore)*/) {
				_target = null;
				return;
			}
			
			float absDiffX = Mathf.Abs(diff.x);
			if (absDiffX < minAttackDistance) {
				if (diff.x > 0) _currerntAction = _characterController.MoveLeft;
				else _currerntAction = _characterController.MoveRight;
			} else if (absDiffX < maxAttackDistance) {
				if (diff.x * (float) _characterController.FaceDirection < 0) _characterController.Turn();
				if (diff.y > jumpRequirementHeight) _characterController.Jump();
				if (onlyAttackWithStamina && _characterController.stamina > 0 || !onlyAttackWithStamina)
					if (Random.value < aggressiveness) {
						_hasAttacked = true;
						_characterController.OnReceiveAttackCommand();
					}
			} else {
				bool isFaceCollided = GetComponent<CharacterMotor>().IsFaceCollided;
				if (!isFaceCollided) {
					if (diff.x > 0) _currerntAction = _characterController.MoveRight;
					else _currerntAction = _characterController.MoveLeft;
				} else _characterController.Jump();
			}
		} else {
			int length = Physics.RaycastNonAlloc(transform.position, new Vector3((float) _characterController.FaceDirection, 0f, 0f), hitResults, detectionDistance, 1 << LayerManager.TerrainLayer | 1 << LayerManager.DeviceLayer | 1 << LayerManager.CharacterLayer, QueryTriggerInteraction.Ignore);
			Array.Sort(hitResults, (x, y) => x.transform == null ? 1 : y.transform == null ? -1 : x.distance.CompareTo(y.distance));
			for (int i = 0; i < length; i++) {
				Transform transform = hitResults[i].transform;
				int layer = transform.gameObject.layer;
				if (layer == LayerManager.TerrainLayer || layer == LayerManager.DeviceLayer) break;
				if (transform.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
					_target = transform.GetComponent<CharacterController>();
					break;
				}
			}
		}

		if (!_hasAttacked && _hasAttackedLastTime) {
			_hasAttackedLastTime = false;
			_characterController.OnFinishAttackCommand();
		}
	}
}