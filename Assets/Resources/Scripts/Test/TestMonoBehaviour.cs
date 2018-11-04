using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestMonoBehaviour : MonoBehaviour {

	public float speed = 10f;
	public float gravity;
	
	private Rigidbody _rigidbody;
	
	private void Awake() {
		// _rigidbody = GetComponent<Rigidbody>();
		GetComponent<ParticleController>().Debug();
	}

	private void Update() {
		/*
		Vector3 velocity;
		if (Input.GetKey(KeyCode.A)) velocity = Vector3.left * speed;
		else if (Input.GetKey(KeyCode.D)) velocity = Vector3.right * speed;
		else if (Input.GetKey(KeyCode.S)) velocity = Vector3.down * speed;
		else if (Input.GetKey(KeyCode.W)) velocity = Vector3.up * speed;
		else velocity = Vector3.zero;
		velocity.y -= gravity;
		_rigidbody.velocity = velocity;
		*/
	}
}