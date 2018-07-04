using UnityEngine;

public class DynamicPlatformController : MonoBehaviour {

	public float speedX;
	public float speedY;

	public float SpeedX {
		get { return c * speedX; }
	}

	public float SpeedY {
		get { return c * speedY; }
	}

	private float c;
	private Rigidbody _rigidbody;

	private void Start() {
		_rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate () {
		float direction;
		direction = Mathf.Sin(Time.time) > 0 ? 1f : -1f;
		_rigidbody.velocity = new Vector3(direction * speedX, direction * speedY);
	}

	private void OnDisable() {
		c = 0f;
	}
}
