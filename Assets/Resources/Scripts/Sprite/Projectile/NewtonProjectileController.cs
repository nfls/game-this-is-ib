using UnityEngine;

[RequireComponent(typeof(ConstantForce))]
public class NewtonProjectileController : GravityProjectileController {

	public bool limitedByBounceTime;
	public bool limitedByLifeSpan;
	public int maxBounceTime;
	public float bounciness = 1f;
	public Collider collisionCollider;

	private int _bounceTime;
	private Rigidbody _rigidbody;
	private PhysicMaterial _physicMaterial;
	private ConstantForce _gravityField;
	
	protected override void Awake() {
		base.Awake();
		
		_rigidbody = GetComponent<Rigidbody>();
		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = false;
		_rigidbody.angularDrag = 0;
		_rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		_rigidbody.constraints = RigidbodyConstraints.FreezePositionZ;

		_physicMaterial = new PhysicMaterial {
			bounceCombine = PhysicMaterialCombine.Maximum,
			staticFriction = 0f,
			dynamicFriction = 0f,
			frictionCombine = PhysicMaterialCombine.Minimum
		};

		collisionCollider.material = _physicMaterial;

		_gravityField = gameObject.AddComponent<ConstantForce>();
	}

	public override void Fire(Vector3 velocity) {
		base.Fire(velocity);
		Physics.IgnoreCollision(collisionCollider, ownerCollider, true);
		_bounceTime = 0;
		_rigidbody.freezeRotation = !rotateWithVelocity;
		_rigidbody.velocity = velocity;
		collisionCollider.enabled = true;
		collisionCollider.material.bounciness = bounciness;
		_gravityField.force = new Vector3(0f, -gravity, 0f);
	}

	protected override void Update() {
		if (_fired) {
			if (limitedByLifeSpan)
				if (Time.time - _startTime >= lifespan) ProjectileManager.Recycle(this);
		}
	}

	private void OnCollisionEnter() {
		_bounceTime++;
		if (limitedByBounceTime) {
			if (_bounceTime > maxBounceTime) {
				_startTime = Time.time;
				lifespan = destroyDelay;
				limitedByBounceTime = false;
				limitedByLifeSpan = true;
				collisionCollider.enabled = false;
			}
		}
	}
}