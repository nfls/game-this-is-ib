using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public abstract class SignalSender : DeviceController {

	public bool isInitialActivated;
	public List<SignalReceiver> activationSignalReceivers;
	public List<SignalReceiver> deactivationSignalReceivers;
	
	public bool IsActivated {
		get { return _isActivated; }
		set {
			_isActivated = value;
			if (!_isEnabled) return;
			if (value) {
				SendActivationSignal();
			} else {
				SendDeactivationSignal();
			}
		}
	}

	protected bool _isActivated;

	public override void Replay() {
		base.Replay();
		if (isInitialActivated) {
			IsActivated = true;
		}
	}

	protected void SendActivationSignal() {
		foreach (var signalReceiver in activationSignalReceivers) {
			signalReceiver.ReceiveActivationSignal();
		}
	}

	protected void SendDeactivationSignal() {
		foreach (var signalReceiver in activationSignalReceivers) {
			signalReceiver.ReceiveDeactivationSignal();
		}
	}
	
	[Conditional("UNITY_EDITOR")]
	protected virtual void OnDrawGizmos() {
		var pos = transform.position;
		Gizmos.color = Color.black;
		foreach (var receiver in deactivationSignalReceivers) {
			Gizmos.DrawLine(pos, receiver.transform.position);
		}

		pos.y += .1f;
		Gizmos.color = Color.cyan;
		foreach (var receiver in activationSignalReceivers) {
			Gizmos.DrawLine(pos, receiver.transform.position + new Vector3(0, .1f, 0));
		}
	}

	[Conditional("UNITY_EDITOR")]
	protected virtual void OnDrawGizmosSelected() {
		var pos = transform.position;
		Gizmos.color = Color.black;
		foreach (var receiver in deactivationSignalReceivers) {
			Gizmos.DrawLine(pos, receiver.transform.position);
		}

		pos.y += .1f;
		Gizmos.color = Color.red;
		foreach (var receiver in activationSignalReceivers) {
			Gizmos.DrawLine(pos, receiver.transform.position + new Vector3(0, .1f, 0));
		}
	}
}