using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IBSpriteTrigger : MonoBehaviour {

	public DetectionSettings detectionSettings;

	public Action<IBSpriteTrigger, Collider> onDetectCharacterEnter;
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
			if (detectionSettings.detectsLocalPlayer && other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				onDetectCharacterEnter?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsEnemy && other.CompareTag(TagManager.ENEMY_TAG)) onDetectCharacterEnter?.Invoke(this, other);
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