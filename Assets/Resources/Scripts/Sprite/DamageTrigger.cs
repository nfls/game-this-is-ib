using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DamageTrigger : MonoBehaviour {

	public AudioClip hitSound;

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

	protected Collider _collider;
	protected AudioSource _audioSource;

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
		/*
		BurstParticleController burstParticle = ParticlePool.Get<BurstParticleController>("cubeblood");
		burstParticle.transform.position = transform.position;
		burstParticle.Spray();
		*/
		if (hitSound) {
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}
		
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