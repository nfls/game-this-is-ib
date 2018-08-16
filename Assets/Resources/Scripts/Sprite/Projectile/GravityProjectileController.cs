using UnityEngine;

public class GravityProjectileController : ProjectileController {

	public bool rotateWithVelocity;
	public float gravity;

	private Vector3 rotation = Vector3.zero;
	
	protected override void Move() {
		velocity.y -= gravity;
		base.Move();
		if (rotateWithVelocity) {
			rotation.z = Mathf.Atan(velocity.y / velocity.x) * Mathf.Rad2Deg;
			transform.rotation = rotation.ToQuaternion();	
		}
	}
}