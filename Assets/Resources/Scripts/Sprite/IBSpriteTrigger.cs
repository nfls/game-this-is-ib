using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IBSpriteTrigger : MonoBehaviour {

	public Transform detectionSource;
	public DetectionSettings detectionSettings;

	public Action<IBSpriteTrigger, Collider, Vector3> onDetectCharacterEnter;
	public Action<IBSpriteTrigger, Collider> onDetectCharacterExit;

	public Collider Collider => _collider;

	protected Collider _collider;
	protected Rigidbody _rigidbody;

	private void Awake() {
		_collider = GetComponent<Collider>();
		_collider.isTrigger = true;

		_rigidbody = gameObject.AddComponent<Rigidbody>();
		_rigidbody.isKinematic = true;
		
		Disable();
	}

	public void Enable() {
		_collider.enabled = true;
		_rigidbody.WakeUp();
	}

	public void Disable() {
		_collider.enabled = false;
		_rigidbody.Sleep();
	}

	private void OnTriggerEnter(Collider other) {
		int layer = other.gameObject.layer;
		
		if (layer == LayerManager.CharacterLayer) {
			detectionSource = detectionSource != null ? detectionSource : transform;
			Vector3 diff = other.transform.position - detectionSource.position;
			RaycastHit hitInfo;
			Vector3 contactPosition = other.Raycast(new Ray(detectionSource.position - diff * 2, diff), out hitInfo, diff.magnitude * 2) ? hitInfo.point : other.ClosestPoint(detectionSource.position);
			
			if (detectionSettings.detectsLocalPlayer && other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				onDetectCharacterEnter?.Invoke(this, other, contactPosition);
				return;
			}

			if (detectionSettings.detectsEnemy && other.CompareTag(TagManager.ENEMY_TAG)) onDetectCharacterEnter?.Invoke(this, other, contactPosition);
		}
	}

	private void OnTriggerExit(Collider other) {
		int layer = other.gameObject.layer;
		
		if (layer == LayerManager.CharacterLayer) {
			if (detectionSettings.detectsLocalPlayer && other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				onDetectCharacterExit?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsEnemy && other.CompareTag(TagManager.ENEMY_TAG)) onDetectCharacterExit?.Invoke(this, other);
		}
	}
}

[Serializable]
public class DetectionSettings {
	
	public bool detectsLocalPlayer;
	public bool detectsEnemy;
}