using System.Diagnostics;
using UnityEngine;

public abstract class SignalReceiver : DeviceController {

	public SignalReceiverType type;
	public int activiationCount;
	
	public bool IsActivated => _isActivated;

	protected bool _isActivated;
	protected int _count;
	
	public void ReceiveActivationSignal() {
		_count += 1;
		if (type == SignalReceiverType.Additive) {
			if (_count == activiationCount) {
				_isActivated = true;
				if (!_isEnabled) return;
				OnActivate();
			}
		} else {
			_isActivated = _count % 2 == 1;
			if (!_isEnabled) return;
			if (_isActivated) OnActivate();
			else OnDeactivate();
		}
	}

	public void ReceiveDeactivationSignal() {
		_count -= 1;
		if (type == SignalReceiverType.Additive) {
			if (_count == activiationCount - 1) {
				_isActivated = false;
				if (!_isEnabled) return;
				OnDeactivate();
			}
		} else {
			_isActivated = _count % 2 == 0;
			if (!_isEnabled) return;
			if (_isActivated) OnActivate();
			else OnDeactivate();
		}
	}

	public abstract void OnActivate();

	public abstract void OnDeactivate();
	
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