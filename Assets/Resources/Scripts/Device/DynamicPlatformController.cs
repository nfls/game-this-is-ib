using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class DynamicPlatformController : DeviceController {

	public LoopType loopType;
	public float speed;
	public List<Vector3> destinations;

	private bool _isInverse;
	private int _currentDestinationIndex;
	private Vector3 _currentDestination;
	private Vector3 _lastPosition;
	private Coroutine _checkCoroutine;

	private void FixedUpdate() {
		if (_isEnabled) transform.position = Vector3.MoveTowards(transform.position, _currentDestination, speed * Time.deltaTime);
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
					if (_currentDestinationIndex != 0) _currentDestinationIndex -= 1; else {
						_currentDestinationIndex = 1;
						_isInverse = false;
						if (loopType == LoopType.DontLoop) Pause();
					}
				} else {
					if (_currentDestinationIndex < destinations.Count - 1) _currentDestinationIndex += 1;
					else {
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

	[Conditional("UNITY_EDITOR")]
	private void OnDrawGizmos() {
		Gizmos.color = Color.green;
		for (int i = 0; i < destinations.Count - 1; i++) Gizmos.DrawLine(destinations[i], destinations[i + 1]);
		if (loopType == LoopType.Close) Gizmos.DrawLine(destinations[destinations.Count - 1], destinations[0]);
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
		for (int i = 0; i < destinations.Count - 1; i++) Gizmos.DrawLine(destinations[i], destinations[i + 1]);
		Vector3 size = new Vector3(.3f, .3f, .3f);
		for (int i = 0; i < destinations.Count; i ++) Gizmos.DrawWireCube(destinations[i], size);
	}

	public enum LoopType {
		DontLoop,
		Inverse,
		Close
	}
}
