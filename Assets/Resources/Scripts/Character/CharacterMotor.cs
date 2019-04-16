using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class CharacterMotor : MonoBehaviour {
	
	private const string BOTTOM_COLLISION = "Bottom";
	private const string FORWARD_COLLISION = "Forward";
	private const string BACKWARD_COLLISION = "Backward";
	private const string GLOBAL_MOVEMENT = "Global";
	
	private static readonly Vector3 ZeroVector3 = Vector3.zero;
	private static readonly Quaternion IdentityQuaternion = Quaternion.identity;

	public bool usesGravity = true;
	public float gravity = 1f;
	public float mass = 1f;
	public float brakePower = 2f;
	public float freeBodyDrag = 1f;

	public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;
	public CollisionDetectionMode collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

	public event Action onGroundEnter;
	public event Action onGroundExit;
	public event Action onFreeBodyExit;
	public event Action onDodgeExit;

	public bool IsGrounded => _isGrounded;

	public bool IsFaceCollided => _physix.IsColliding(FORWARD_COLLISION);

	public Collider BodyCollider => _bodyCollider;

	public FaceDirection FaceDirection => _faceDirection;

	public float VelocityX => _freeVelocityX + _moveVelocityX;

	public float VelocityY => _velocityY;

	private Collider _bodyCollider;
	private Rigidbody _rigidbody;
	private Physix _physix;
	private FaceDirection _faceDirection = FaceDirection.Right;

	private bool _needsUpdate;
	private bool _isGrounded;

	private bool _hasMoved;
	private bool _isDodging;

	private float _freeVelocityX;
	private float _freeVelocityY;
	private float _moveVelocityX;
	private float _dodgeVelocity;
	private float _dodgeDuration;
	private float _dodgeTime;
	private float _velocityY;
	
	private void Awake() {
		_bodyCollider = GetComponent<Collider>();
		
		// Config Rigidbody
		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.useGravity = false;
		_rigidbody.mass = mass;
		_rigidbody.drag = 0f;
		_rigidbody.angularDrag = 0f;
		_rigidbody.collisionDetectionMode = collisionDetectionMode;
		_rigidbody.constraints = (int) RigidbodyConstraints.FreezePositionZ + (int) RigidbodyConstraints.FreezeRotationX + (int) RigidbodyConstraints.FreezeRotationY + RigidbodyConstraints.FreezeRotationZ;
		_rigidbody.interpolation = interpolation;
		_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		
		// Config Physix
		_physix = gameObject.AddComponent<Physix>();
		_physix.Collisions = new Physix.PHYSIXCOLLISION[3];
		_physix.Collisions[0] = new Physix.PHYSIXCOLLISION {
			Name = BOTTOM_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { y = true, less = true, equals = true, value = -45f } },
			Snap = true
		};
		
		_physix.Collisions[1] = new Physix.PHYSIXCOLLISION {
			Name = FORWARD_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { x = true, greater = true, equals = true, value = 45f } }
		};
		
		_physix.Collisions[2] = new Physix.PHYSIXCOLLISION {
			Name = BACKWARD_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { x = true, less = true, equals = true, value = -45f } }
		};
		
		_physix.Movements = new[] { new Physix.PHYSIXMOVE { Name = GLOBAL_MOVEMENT } };
		_physix.Movements[0].x.equals = true;
		_physix.Movements[0].y.equals = true;
		_physix.Movements[0].z.equals = true;

		_physix.PlatformsRetainVelocity = false;
		_physix.PlatformsVelocityMultiplier = 25f;
	}

	private void FixedUpdate() => _needsUpdate = true;

	private void Update() {
		if (!_needsUpdate) return;
		_needsUpdate = false;
		if (_isDodging) {
			if (Time.time - _dodgeTime >= _dodgeDuration) {
				_isDodging = false;
				gameObject.layer = LayerManager.CharacterLayer;
				onDodgeExit?.Invoke();
			} else {
				Vector3 dodgeVelocity = new Vector3(_dodgeVelocity, 0f, 0f);
				if (_physix.IsColliding(BOTTOM_COLLISION)) {
					dodgeVelocity = Vector3.ProjectOnPlane(dodgeVelocity, _physix.GetNormal(BOTTOM_COLLISION));
					if (!_isGrounded) {
						_velocityY = 0f;
						_isGrounded = true;
						onGroundEnter?.Invoke();	
					}
				} else {
					if (_isGrounded) {
						_isGrounded = false;
						onGroundExit?.Invoke();
					}
				}
				
				_physix.ApplyMovement(GLOBAL_MOVEMENT, dodgeVelocity.x, AxisType.x, ValueType.value, dodgeVelocity.y, AxisType.y, ValueType.value);
			}
		} else {
			if (_physix.IsColliding(BOTTOM_COLLISION)) {
				if (!_isGrounded) {
					_velocityY = 0f;
					_isGrounded = true;
					onGroundEnter?.Invoke();
				}
			} else {
				if (usesGravity) _velocityY -= gravity;
				if (_isGrounded) {
					_isGrounded = false;
					onGroundExit?.Invoke();
				}
			}

			if (!_hasMoved) {
				if (_moveVelocityX != 0f)
					if (_moveVelocityX * (float) _faceDirection > brakePower) _moveVelocityX += -brakePower * (float) _faceDirection;
					else _moveVelocityX = 0f;
			} else _hasMoved = false;

			if (_freeVelocityX != 0f) {
				int direction = _freeVelocityX > 0 ? 1 : -1;
				if (_freeVelocityX * direction > freeBodyDrag) _freeVelocityX += -freeBodyDrag * direction;
				else {
					_freeVelocityX = 0f;
					onFreeBodyExit?.Invoke();
				}
			}
			
			_physix.ApplyMovement(GLOBAL_MOVEMENT, _freeVelocityX + _moveVelocityX, AxisType.x, ValueType.value, _velocityY, AxisType.y, ValueType.value);
		}
	}
	
	public void Flip() {
		_faceDirection = (FaceDirection) ((int) _faceDirection * -1);
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
		/*
		scale = _bodyCollider.size;
		scale.x *= -1;
		_bodyCollider.size = scale;
		*/
	}

	public void Turn() {
		_faceDirection = (FaceDirection) ((int) _faceDirection * -1);
		transform.eulerAngles = _faceDirection == FaceDirection.Left ? new Vector3(0f, 180f, 0f) : Vector3.zero;
	}

	public void Move(float velocity) {
		/*
		if ((float) _faceDirection * velocity <= 0f) Flip();
		*/
		_hasMoved = true;
		_moveVelocityX = velocity;
	}

	public void Dodge(float velocity, float duration) {
		_dodgeVelocity = velocity;
		_dodgeDuration = duration;
		_dodgeTime = Time.time;
		gameObject.layer = LayerManager.DodgeLayer;
		_isDodging = true;
		float portion = Mathf.Sign(velocity) * .5f;
		ushort leftStrength = Convert.ToUInt16((.5f - portion) * 3000);
		ushort rightStrength = Convert.ToUInt16((.5f + portion) * 3000);
		JoystickUtil.RumbleJoystick(leftStrength, rightStrength, 200);
	}

	private void Teleport(float distance) {

		if (_physix.IsColliding(FORWARD_COLLISION))
			if (_physix.GetCollision(FORWARD_COLLISION).gameObject.layer != LayerManager.CharacterLayer) return;

		gameObject.layer = LayerManager.DodgeLayer;
		RaycastHit hitInfo;
		if (_rigidbody.SweepTest(new Vector3(1, 0, 0), out hitInfo, distance, QueryTriggerInteraction.Ignore)) distance = hitInfo.point.x - transform.position.x - transform.localScale.x / 2;
		gameObject.layer = LayerManager.CharacterLayer;
		
		transform.position = transform.position + new Vector3(distance, 0, 0);
	}

	public void Jump(float jumpPower) => _velocityY = jumpPower;

	public void GetHit(float velocityX, float velocityY) {
		_freeVelocityX += velocityX;
		_velocityY = velocityY;
	}
}

public enum FaceDirection {
	Left = -1,
	Right = 1
}
