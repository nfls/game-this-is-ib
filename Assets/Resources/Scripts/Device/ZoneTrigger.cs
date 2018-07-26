using System.Collections.Generic;
using System.Diagnostics;
using Cinemachine;
using UnityEngine;

public class ZoneTrigger : SignalSender {

	[TagField]
	public List<string> detectedTags;

	private int _count;

	protected void OnTriggerEnter(Collider other) {
		foreach (var tag in detectedTags) {
			if (other.CompareTag(tag)) {
				_count += 1;
				if (_count == 1) {
					IsActivated = true;
				}
				break;
			}
		}
	}
	
	protected void OnTriggerExit(Collider other) {
		foreach (var tag in detectedTags) {
			if (other.CompareTag(tag)) {
				_count -= 1;
				if (_count == 0) {
					IsActivated = false;
				}
				break;
			}
		}
	}

#if UNITY_EDITOR
	protected override void OnDrawGizmos() {
		base.OnDrawGizmos();
		var pos = transform.position;
		var scale = transform.lossyScale;
		Color color = new Color(0, .8f, .1f, .5f);
		Gizmos.color = color;
		Gizmos.DrawCube(pos, scale);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(pos, scale);
	}

	protected override void OnDrawGizmosSelected() {
		base.OnDrawGizmosSelected();
		var pos = transform.position;
		var scale = transform.lossyScale;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(pos, scale);
	}
#endif
}