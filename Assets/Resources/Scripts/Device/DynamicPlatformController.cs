using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DynamicPlatformController : DeviceController {

	public LoopType loopType;
	public float speed;
	public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;
	public List<Vector3> destinations;

	public Vector3 DeltaMovement => _deltaMovement;

	private bool _isInverse;
	private int _currentDestinationIndex;
	private Vector3 _currentDestination;
	private Vector3 _deltaMovement;
	private Vector3 _lastPosition;
	private List<CharacterController> _characters;
	private Rigidbody _rigidbody;
	private Coroutine _checkCoroutine;

	protected override void Awake() {
		base.Awake();
		
		_characters = new List<CharacterController>(2);
		
		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
		_rigidbody.interpolation = interpolation;
	}

	private void FixedUpdate() {
		if (_isEnabled) _rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime));
	}

	private void Update() {
		if (_isEnabled) {
			Vector3 pos = transform.position;
			_deltaMovement = pos - _lastPosition;
			_lastPosition = pos;
			foreach (var character in _characters) character.transform.position += _deltaMovement;	
		}
	}

	public override void Replay() {
		transform.position = destinations[0];
		if (destinations.Count > 0) {
			_currentDestinationIndex = 1;
			_currentDestination = destinations[1];
		}
		
		Play();
	}

	public override void Play() {
		base.Play();
		_checkCoroutine = StartCoroutine(ExeCheckCoroutine());
	}

	public override void Pause() {
		base.Pause();
		StopCoroutine(_checkCoroutine);
		_checkCoroutine = null;
	}

	private IEnumerator ExeCheckCoroutine() {
		while (true) {
			yield return new WaitForEndOfFrame();
			if (transform.position == _currentDestination) {
				if (_isInverse) {
					if (_currentDestinationIndex != 0) {
						_currentDestinationIndex -= 1;
					} else {
						_currentDestinationIndex = 1;
						_isInverse = false;
						if (loopType == LoopType.DontLoop) {
							Pause();
						}
					}
				} else {
					if (_currentDestinationIndex < destinations.Count - 1) {
						_currentDestinationIndex += 1;
					} else {
						switch (loopType) {
							case LoopType.DontLoop: Pause();
								break;
							case LoopType.Inverse:
								_currentDestinationIndex -= 1;
								_isInverse = true;
								break;
							case LoopType.Close: _currentDestinationIndex = 0;
								break;
						}
					}
				}

				_currentDestination = destinations[_currentDestinationIndex];
			}
		}
	}

	private void AddCharacter(CharacterController controller) {
		_characters.Add(controller);
	}

	private void RemoveCharacter(CharacterController controller) {
		_characters.Remove(controller);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == LayerManager.CharacterLayer) {
			CharacterController controller = other.transform.GetComponent<CharacterController>();
			if (controller) {
				AddCharacter(controller);
			}
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.layer == LayerManager.CharacterLayer) {
			CharacterController controller = other.transform.GetComponent<CharacterController>();
			if (controller) {
				RemoveCharacter(controller);
			}
		}
	}

	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		for (int i = 0; i < destinations.Count - 1; i++) {
			Gizmos.DrawLine(destinations[i], destinations[i + 1]);
		}

		if (loopType == LoopType.Close) {
			Gizmos.DrawLine(destinations[destinations.Count - 1], destinations[0]);
		}
		
		Vector3 size = new Vector3(.3f, .3f, .3f);
		Color color = new Color(0, .7f, .3f, .4f);
		for (int i = 0; i < destinations.Count; i ++) {
			Gizmos.color = color;
			Gizmos.DrawCube(destinations[i], size);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(destinations[i], size);
		}
	}
	
	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		for (int i = 0; i < destinations.Count - 1; i++) {
			Gizmos.DrawLine(destinations[i], destinations[i + 1]);
		}
		
		Vector3 size = new Vector3(.3f, .3f, .3f);

		for (int i = 0; i < destinations.Count; i ++) {
			Gizmos.DrawWireCube(destinations[i], size);
		}
	}

	public enum LoopType {
		DontLoop,
		Inverse,
		Close
	}
}
