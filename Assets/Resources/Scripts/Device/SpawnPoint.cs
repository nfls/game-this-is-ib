using System.Collections;
using UnityEngine;

public class SpawnPoint : DeviceController {

	public float spawnYOffset = .5f;
	public float spawnTime = 3f;

	private ParticleSystem _particleSystem;
	private Coroutine spawnCoroutine;

	protected override void Awake() => _particleSystem = GetComponentInChildren<ParticleSystem>();

	public void Spawn(CharacterController character) {
		_particleSystem.Play();
		Vector3 pos = transform.position;
		pos.y += spawnYOffset;
		character.transform.position = pos;
		character.ComeAlive();
		if (spawnCoroutine != null) StopCoroutine(spawnCoroutine);
		spawnCoroutine = StartCoroutine(ExeSpawnCoroutine(character.GetComponent<InputOperator>()));
	}

	private IEnumerator ExeSpawnCoroutine(InputOperator input) {
		input.IsInControl = false;
		yield return new WaitForSeconds(spawnTime);
		input.IsInControl = true;
	}
}