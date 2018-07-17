using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicPlatformController : DeviceController {

	public LoopType loopType;
	public float speed;
	public RigidbodyInterpolation interpolation = RigidbodyInterpolation.Interpolate;
	public List<Vector3> destinations;

	public Vector3 DeltaMovement {
		get { return _deltaMovement; }
	}
	
	protected bool _isInverse;
	protected int _currentDestinationIndex;
	protected Vector3 _currentDestination;
	protected Vector3 _deltaMovement;
	protected Vector3 _lastPosition;
	protected List<CharacterMotor> _characters;
	protected Rigidbody _rigidbody;
	protected Coroutine _checkCoroutine;

	protected override void Start() {
		base.Start();
		
		_characters = new List<CharacterMotor>(2);
		
		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
		_rigidbody.interpolation = interpolation;
	}

	private void Update() {
		Vector3 pos = transform.position;
		_deltaMovement = pos - _lastPosition;
		_lastPosition = pos;
		foreach (var motor in _characters) {
			motor.transform.position += _deltaMovement;
		}
	}

	private void FixedUpdate() {
		if (_isEnabled) {
			_rigidbody.MovePosition(Vector3.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime));
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
		_checkCoroutine = StartCoroutine(ExeCheckTask());
	}

	public override void Pause() {
		base.Pause();
		if (_checkCoroutine != null) StopCoroutine(_checkCoroutine);
	}

	protected IEnumerator ExeCheckTask() {
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

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.layer == LayerManager.CharacterLayer) {
			CharacterMotor motor = other.transform.GetComponent<CharacterMotor>();
			if (motor) {
				_characters.Add(motor);
			}
		}
	}

	private void OnCollisionExit(Collision other) {
		if (other.gameObject.layer == LayerManager.CharacterLayer) {
			CharacterMotor motor = other.transform.GetComponent<CharacterMotor>();
			if (motor) {
				_characters.Remove(motor);
			}
		}
	}

#if UNITY_EDITOR
	private Color color = new Color(0, .7f, .3f, .4f);
	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		for (int i = 0; i < destinations.Count - 1; i++) {
			Gizmos.DrawLine(destinations[i], destinations[i + 1]);
		}

		if (loopType == LoopType.Close) {
			Gizmos.DrawLine(destinations[destinations.Count - 1], destinations[0]);
		}
		
		Vector3 size = new Vector3(.3f, .3f, .3f);

		for (int i = 0; i < destinations.Count; i ++) {
			Gizmos.color = color;
			Gizmos.DrawCube(destinations[i], size);
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(destinations[i], size);
		}
	}
	
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
#endif

	public enum LoopType {
		DontLoop,
		Inverse,
		Close
	}
}
