using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class SignalReceiver : DeviceController {

	public SignalReceiverType type;
	public int activiationCount;
	public UnityEvent activationEvent;
	public UnityEvent deactivationEvent;
	
	public bool IsActivated => _isActivated;

	protected bool _isActivated;
	protected int _count;
	
	public void ReceiveActivationSignal() {
		_count += 1;
		if (type == SignalReceiverType.Additive) {
			if (_count == activiationCount) {
				_isActivated = true;
				if (!_isEnabled) return;
				activationEvent?.Invoke();
			}
		} else {
			_isActivated = _count % 2 == 1;
			if (!_isEnabled) return;
			if (_isActivated) activationEvent?.Invoke();
			else deactivationEvent?.Invoke();
		}
	}

	public void ReceiveDeactivationSignal() {
		_count -= 1;
		if (type == SignalReceiverType.Additive) {
			if (_count == activiationCount - 1) {
				_isActivated = false;
				if (!_isEnabled) return;
				deactivationEvent?.Invoke();
			}
		} else {
			_isActivated = _count % 2 == 1;
			if (!_isEnabled) return;
			if (_isActivated) activationEvent?.Invoke();
			else deactivationEvent?.Invoke();
		}
	}
	
	[Conditional("UNITY_EDITOR")]
	protected virtual void OnDrawGizmos() {
		var pos = transform.position;
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(pos, .6f);
		Gizmos.color = new Color(0, .8f, .1f, .5f);
		Gizmos.DrawSphere(pos, .4f);
	}

	[Conditional("UNITY_EDITOR")]
	protected virtual void OnDrawGizmosSelected() {
		var pos = transform.position;
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(pos, .6f);
		Gizmos.DrawSphere(pos, .4f);
	}
}

public enum SignalReceiverType {
	Additive,
	Counter
}