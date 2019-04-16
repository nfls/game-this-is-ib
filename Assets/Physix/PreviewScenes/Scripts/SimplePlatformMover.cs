using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlatformMover : MonoBehaviour {

	public Vector3 startPoint;
	public Vector3 endPoint;
	public float speed;

	private bool _isForward;
	private Vector3 _target;

	private void Awake() {
		startPoint = transform.position;
		_isForward = true;
		_target = endPoint;
	}

	private void Update() {
		transform.position = Vector3.MoveTowards(transform.position, _target, speed * Time.deltaTime);
		if (transform.position != _target) return;
		_isForward = !_isForward;
		_target = _isForward ? endPoint : startPoint;
	}
}
