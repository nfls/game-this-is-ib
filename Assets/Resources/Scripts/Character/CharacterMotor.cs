using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CharacterMotor : MonoBehaviour {
	
	private const string BOTTOM_COLLISION = "Bottom";
	private const string LEFT_COLLISION = "Left";
	private const string RIGHT_COLLISION = "Right";
	private const string GLOBAL_MOVEMENT = "Global";
	
	private static readonly Vector3 ZeroVector3 = Vector3.zero;
	private static readonly Quaternion IdentityQuaternion = Quaternion.identity;

	public bool useGravity = true;
	public float gravity = 1f;
	public float mass = 1f;
	public float brakePower = 2f;
	public float freeBodyDrag = 1f;

	public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;
	public CollisionDetectionMode collisionDetectionMode;
	
	public delegate void OnGroundEnterHandler();
	public delegate void OnGroundExitHandler();
	public delegate void OnFreeBodyExitHandler();

	public event OnGroundEnterHandler OnGroundEnter;
	public event OnGroundExitHandler OnGroundExit;
	public event OnFreeBodyExitHandler OnFreeBodyExit;

	public bool IsGrounded => _isGrounded;

	public FaceDirection FaceDirection => _faceDirection;

	public float VelocityX => _freeVelocityX + _moveVelocityX;

	public float VelocityY => _velocityY;

	private BoxCollider _bodyCollider;
	private Rigidbody _rigidbody;
	private Physix _physix;
	private FaceDirection _faceDirection = FaceDirection.Right;

	private bool _isGrounded;

	private bool _hasMoved;
	private bool _hasDodged;

	private float _freeVelocityX;
	private float _moveVelocityX;
	private float _dodgeDistance;
	private float _velocityY;
	
	private void Start() {
		_bodyCollider = GetComponent<BoxCollider>();
		
		// Config Rigidbody
		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.useGravity = false;
		_rigidbody.mass = mass;
		_rigidbody.drag = 0f;
		_rigidbody.angularDrag = 0f;
		_rigidbody.collisionDetectionMode = collisionDetectionMode;
		_rigidbody.constraints = (int) RigidbodyConstraints.FreezePositionZ + (int) RigidbodyConstraints.FreezeRotationX + (int) RigidbodyConstraints.FreezeRotationY + RigidbodyConstraints.FreezeRotationZ;
		_rigidbody.interpolation = interpolation;
		
		// Condig Physix
		_physix = gameObject.AddComponent<Physix>();
		_physix.Collisions = new Physix.PHYSIXCOLLISION[3];
		_physix.Collisions[0] = new Physix.PHYSIXCOLLISION {
			Name = BOTTOM_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { y = true, less = true, equals = true, value = -45f } }
		};
		_physix.Collisions[1] = new Physix.PHYSIXCOLLISION {
			Name = LEFT_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { x = true, greater = true, equals = true, value = 45f } }
		};
		_physix.Collisions[2] = new Physix.PHYSIXCOLLISION {
			Name = RIGHT_COLLISION,
			Active = true,
			Ranges = new[] { new Physix.PHYSIXBOUNDS { x = true, less = true, equals = true, value = -45f } }
		};
		_physix.Movements = new[] { new Physix.PHYSIXMOVE { Name = GLOBAL_MOVEMENT } };
		_physix.Movements[0].x.equals = true;
		_physix.Movements[0].y.equals = true;
		_physix.Movements[0].z.equals = true;
	}

	private void FixedUpdate() {
		if (_physix.IsColliding(BOTTOM_COLLISION)) {
			if (!_isGrounded) {
				_velocityY = 0f;
				_isGrounded = true;
				OnGroundEnter?.Invoke();
			}
			
		} else {
			if (useGravity) {
				_velocityY -= gravity;
			}
			
			if (_isGrounded) {
				_isGrounded = false;

				OnGroundExit?.Invoke();
			}
		}

		if (!_hasMoved) {
			if (_moveVelocityX != 0f) {
				if (_moveVelocityX * (float) _faceDirection > brakePower) {
					_moveVelocityX += -brakePower * (float) _faceDirection;
				} else {
					_moveVelocityX = 0f;
				}
			}
		} else {
			_hasMoved = false;
		}

		if (_freeVelocityX != 0f) {
			int direction = _freeVelocityX > 0 ? 1 : -1;
			if (_freeVelocityX * direction > freeBodyDrag) {
				_freeVelocityX += -freeBodyDrag * direction;
			} else {
				_freeVelocityX = 0f;
				OnFreeBodyExit?.Invoke();
			}
		}
		
		_physix.ApplyMovement(GLOBAL_MOVEMENT, _freeVelocityX + _moveVelocityX, AxisType.x, ValueType.value, _velocityY, AxisType.y, ValueType.value);
		
		if (_hasDodged) {
			_hasDodged = false;
			Teleport(_dodgeDistance);
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
		transform.localRotation = (transform.localRotation.eulerAngles + new Vector3(0f, 180f, 0f)).ToQuaternion();
	}

	public void Move(float velocity) {
		/*
		if ((float) _faceDirection * velocity <= 0f) {
			Flip();
		}
		*/
		_hasMoved = true;
		_moveVelocityX = velocity;
	}

	public void Dodge(float distance) {
		_hasDodged = true;
		_dodgeDistance = distance;
	}

	private void Teleport(float distance) {
		float sign;
		string collision;
		if (distance > 0) {
			sign = 1;
			collision = LEFT_COLLISION;
		} else {
			sign = -1;
			collision = RIGHT_COLLISION;
		}
		if (_physix.IsColliding(collision)) return;
		
		RaycastHit hitInfo;
		
		if (_rigidbody.SweepTest(new Vector3(sign, 0, 0), out hitInfo, distance * sign, QueryTriggerInteraction.Ignore)) {
			distance = hitInfo.point.x - transform.position.x - transform.lossyScale.x / 2;
		}
		
		transform.position += new Vector3(distance, 0, 0);
	}

	public void Jump(float jumpPower) {
		_velocityY = jumpPower;
		
	}

	public void GetHit(float velocityX, float velocityY) {
		_freeVelocityX += velocityX;
		_velocityY += velocityY;
	}
}

public enum FaceDirection {
	Left = -1,
	Right = 1
}
