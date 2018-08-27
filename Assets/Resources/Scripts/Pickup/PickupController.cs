using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupController : MonoBehaviour {

	public bool doFloating;
	public bool doRotating;
	public float amplitude;
	public float speed;
	public Vector3 rotation;
	public float angularSpeed;

	private Collider _collider;

	private Vector3 _originalPosition;
	private Quaternion _originalRotation;

	private void Start() {

		_collider = GetComponent<Collider>();
		_collider.isTrigger = true;

		_originalPosition = transform.position;
	}

	private void Update() {
		if (doFloating) {
			Vector3 upPosition = _originalPosition;
			upPosition.y += amplitude;
			transform.position = Vector3.LerpUnclamped(_originalPosition, upPosition, Mathf.Sin(speed * Time.time));
		}

		if (doRotating) transform.Rotate(rotation * angularSpeed * Time.time);
	}

	protected virtual void Disappear() {
		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
			
		}
	}
}