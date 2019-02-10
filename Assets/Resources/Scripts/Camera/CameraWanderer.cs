using UnityEngine;

public class CameraWanderer : MonoBehaviour {

	public float speed = 1f;
	public float sensitivity = 1f;
	public Vector3 lastPos;

	private void Awake() => lastPos = transform.position;

	private void Update() {
		if (Input.GetKey(KeyCode.W)) transform.Translate(Vector3.forward * speed * Time.time);
		if (Input.GetKey(KeyCode.S)) transform.Translate(Vector3.back * speed * Time.time);
		if (Input.GetKey(KeyCode.A)) transform.Translate(Vector3.left * speed * Time.time);
		if (Input.GetKey(KeyCode.D)) transform.Translate(Vector3.right * speed * Time.time);

		if (Input.GetKey(KeyCode.Mouse0)) {
			Vector3 dpos = Input.mousePosition - lastPos;
			transform.Rotate(new Vector3(-dpos.y * sensitivity, dpos.x * sensitivity, 0));
			float x = transform.rotation.eulerAngles.x;
			float y = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(x, y, 0);
		}

		lastPos = Input.mousePosition;
	}
}