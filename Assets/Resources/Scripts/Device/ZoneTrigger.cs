using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : DeviceController {

	[TagField]
	public List<string> detectedTags;

	public UnityEventWithVector3 enterEvent;
	public UnityEventWithVector3 exitEvent;

	private int _count;

	protected void OnTriggerEnter(Collider other) {
		foreach (var tag in detectedTags) {
			if (other.CompareTag(tag)) {
				_count += 1;
				if (_count == 1) enterEvent?.Invoke(other.transform.position);
				break;
			}
		}
	}
	
	protected void OnTriggerExit(Collider other) {
		foreach (var tag in detectedTags) {
			if (other.CompareTag(tag)) {
				_count -= 1;
				if (_count == 0) exitEvent?.Invoke(other.transform.position);
				break;
			}
		}
	}

	protected void OnDrawGizmos() {
		var pos = transform.position;
		var scale = transform.lossyScale;
		Color color = new Color(0, .8f, .1f, .5f);
		Gizmos.color = color;
		Gizmos.DrawCube(pos, scale);
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(pos, scale);
	}

	protected void OnDrawGizmosSelected() {
		var pos = transform.position;
		var scale = transform.lossyScale;
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(pos, scale);
	}
}