using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class IBSpriteTrigger : MonoBehaviour {

	public DetectionSettings detectionSettings;

	public delegate void OnDetectHandler(IBSpriteTrigger trigger, Collider detectedcollider);

	public event OnDetectHandler OnDetectCharacterEnter;
	public event OnDetectHandler OnDetectDestructibleEnter;
	public event OnDetectHandler OnDetectCharacterExit;
	public event OnDetectHandler OnDetectDestructibleExit;

	public Collider Collider => _collider;

	protected Collider _collider;

	private void Start() {
		_collider = GetComponent<Collider>();
		_collider.isTrigger = true;
	}

	public void Enable() {
		_collider.enabled = true;
	}

	public void Disable() {
		_collider.enabled = false;
	}

	private void OnTriggerEnter(Collider other) {
		int layer = other.gameObject.layer;
		
		if (layer == LayerManager.CharacterLayer) {
			if (detectionSettings.detectsLocalPlayer && other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				OnDetectCharacterEnter?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsRemotePlayer && other.CompareTag(TagManager.REMOTE_PLAYER_TAG)) {
				OnDetectCharacterEnter?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsEnemy && other.CompareTag(TagManager.ENEMY_TAG)) {
				OnDetectCharacterEnter?.Invoke(this, other);
				return;
			}
		}
		
		if (layer == LayerManager.DeviceLayer) {
			if (detectionSettings.detectsDevice) {
				if (other.CompareTag(TagManager.DESTRUCTIBLE_TAG)) {
					OnDetectDestructibleEnter?.Invoke(this, other);
				}
			}
		}
	}

	private void OnTriggerExit(Collider other) {
		int layer = other.gameObject.layer;
		
		if (layer == LayerManager.CharacterLayer) {
			if (detectionSettings.detectsLocalPlayer && other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				OnDetectCharacterExit?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsRemotePlayer && other.CompareTag(TagManager.REMOTE_PLAYER_TAG)) {
				OnDetectCharacterExit?.Invoke(this, other);
				return;
			}

			if (detectionSettings.detectsEnemy && other.CompareTag(TagManager.ENEMY_TAG)) {
				OnDetectCharacterExit?.Invoke(this, other);
				return;
			}
		}
		
		if (layer == LayerManager.DeviceLayer) {
			if (detectionSettings.detectsDevice) {
				if (other.CompareTag(TagManager.DESTRUCTIBLE_TAG)) {
					OnDetectDestructibleExit?.Invoke(this, other);
				}
			}
		}
	}
}

[Serializable]
public class DetectionSettings {
	
	public bool detectsLocalPlayer;
	public bool detectsRemotePlayer;
	public bool detectsEnemy;
	public bool detectsDevice;
}