using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageTrigger : MonoBehaviour {

	public string name;
	public bool detectsLocalPlayer;
	public bool detectsRemotePlayer;
	public bool detectsEnemy;
	public bool detectsDevice;
	public bool doesHit;
	public bool doesStun;
	public bool doesDamage;
	public float damage;
	public float hitVelocityX;
	public float hitVelocityY;
	public float stunnedTime;

	protected Collider collider;

	private void Start() {
		collider = GetComponent<Collider>();
		collider.isTrigger = true;
	}

	public void Enable() {
		collider.enabled = true;
	}

	public void Disable() {
		collider.enabled = false;
	}

	private void OnTriggerEnter(Collider other) {
		if (detectsLocalPlayer) {
			if (other.CompareTag(TagManager.LOCAL_PLAYER_TAG)) {
				DamageCharacter(other);
				return;
			}
		}

		if (detectsRemotePlayer) {
			if (other.CompareTag(TagManager.REMOTE_PLAYER_TAG)) {
				DamageCharacter(other);
				return;
			}
		}

		if (detectsEnemy) {
			if (other.CompareTag(TagManager.ENEMY_TAG)) {
				DamageCharacter(other);
				return;
			}
		}

		if (detectsDevice) {
			if (other.CompareTag(TagManager.DESTRUCTIBLE_TAG)) {
				DamageDevice(other);
			}
		}
	}

	private void DamageCharacter(Collider collider) {
		CharacterController characterController = collider.GetComponent<CharacterController>();
		if (doesHit) {
			characterController.GetHit(hitVelocityX, hitVelocityY);
		}

		if (doesStun) {
			characterController.GetStunned(stunnedTime);
		}
		
		if (doesDamage) {
			characterController.GetDamaged(damage);
		}
	}

	private void DamageDevice(Collider collider) {
		
	}
}