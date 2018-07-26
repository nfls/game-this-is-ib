using System.Collections;
using UnityEngine;

public class SpawnPoint : DeviceController {

	public Material spawnMaterial;
	public float spawnTime;
	
	private Coroutine _spawnCoroutine;

	public void Spawn() {
		_spawnCoroutine = StartCoroutine(ExeSpawnCoroutine());
	}

	private IEnumerator ExeSpawnCoroutine() {
		float timeRemaining = spawnTime;
		float speed = 1f / spawnTime;
		float dissolveThreshold = 1;
		while (timeRemaining > 0) {
			yield return null;
			float delataTime = Time.time;
			timeRemaining -= delataTime;
			dissolveThreshold -= speed * delataTime;
			spawnMaterial.SetFloat(ShaderManager.DISSOLVE_THRESHOLD_KEYWORD, dissolveThreshold);
		}
		
		spawnMaterial.SetFloat(ShaderManager.DISSOLVE_THRESHOLD_KEYWORD, 0f);
	}
}