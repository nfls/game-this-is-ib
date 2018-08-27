using UnityEngine;

public class RocketProjectileController : ProjectileController {

	public Transform target;
	public Vector3 targetPos;
	public float correctionPower;
	public float power;

	private bool _towardsRight;

	public override void Fire(Vector3 velocity) {
		base.Fire(velocity);
		_towardsRight = velocity.x > 0f;
	}

	protected override void Move() {
		if (target != null) targetPos = target.position;
		Vector3 diff = targetPos - transform.position;
		Vector3 dir = diff.normalized;
		if (_towardsRight) {
			transform.right = Vector3.MoveTowards(transform.right, dir, correctionPower * Time.deltaTime);
			velocity = transform.right * power;
		} else {
			transform.rotation = Quaternion.FromToRotation(Vector3.left, Vector3.MoveTowards(transform.rotation * Vector3.left, dir, correctionPower * Time.deltaTime));
			velocity = transform.rotation * Vector3.left * power;
		}
		
		base.Move();
	}
}