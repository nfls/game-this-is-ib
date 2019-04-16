using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(IBSpriteTrigger))]
public class AreaDamagerController : MonoBehaviour {

	public AttackEffectSettings attackEffectSettings;
	public bool onlyDamageWhenEnter;
	public float damageInterval = .5f;

	private IBSpriteTrigger _trigger;
	private ObjectPool<CharacterDamageTimer> _timers = new ObjectPool<CharacterDamageTimer>();
	private List<CharacterDamageTimer> _characters = new List<CharacterDamageTimer>(2);

	private void Start() {
		_trigger = GetComponent<IBSpriteTrigger>();
		_trigger.onDetectCharacterEnter = OnCharacterEnter;
		_trigger.onDetectCharacterExit = OnCharacterExit;
		_trigger.Enable();
	}

	private void Update() {
		if (onlyDamageWhenEnter) return;
		float time = Time.time;
		foreach (var timer in _characters) {
			if (time >= timer.lastDamageTime + damageInterval) {
				Damage(timer.characterController);
				timer.lastDamageTime = time;
			}
		}
	}

	private void Damage(CharacterController character) {
		float direction = (float) character.FaceDirection;
		if (attackEffectSettings.doesHit) character.GetHit(attackEffectSettings.hitVelocityX * direction, attackEffectSettings.hitVelocityY);
		if (attackEffectSettings.doesStun) character.GetStunned(attackEffectSettings.stunAngle * direction, attackEffectSettings.stunTime);
		if (attackEffectSettings.doesDamage) character.GetDamaged(attackEffectSettings.damage);
	}

	private void Remove(CharacterController character) {
		int index = -1;
		for (int i = 0, l = _characters.Count; i < l; i++) {
			if (_characters[i].characterController == character) {
				index = i;
				break;
			}
		}

		if (index != -1) _characters.RemoveAt(index);
	}

	private void OnCharacterEnter(IBSpriteTrigger trigger, Collider collider, Vector3 contact) {
		CharacterController character = collider.GetComponentInParent<CharacterController>();
		_characters.Add(_timers.GetObject().Set(character, Time.time));
		Damage(character);
	}

	private void OnCharacterExit(IBSpriteTrigger trigger, Collider collider) => Remove(collider.GetComponentInParent<CharacterController>());

	public class CharacterDamageTimer {
		public CharacterController characterController;
		public float lastDamageTime;

		public CharacterDamageTimer Set(CharacterController character, float time) {
			characterController = character;
			lastDamageTime = time;
			return this;
		}
	}
}