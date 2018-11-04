using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoSingleton {

	public static int Count => idleAudioSources.Count;

	private static AudioManager instance;
	private static Transform audioRoot;
	private static readonly Queue<AudioSource> idleAudioSources = new Queue<AudioSource>(5);
	
	private void Start() {
		instance = this;
		GameObject go = new GameObject("Audio Root");
		DontDestroyOnLoad(go);
		audioRoot = go.transform;
	}

	public static void PlayAtPoint(AudioClip clip, Vector3 position, float volume = 1f) {
		AudioSource source = GetAudioSource();
		source.transform.position = position;
		source.clip = clip;
		source.volume = volume;
		source.Play();
		instance.StartCoroutine(ExeRecycleCoroutine(source));
	}

	public static void PlayAtPoint(AudioAsset audio, Vector3 position, float volume = 1f) {
		PlayAtPoint(audio.Source, position, volume);
	}

	public static void PlayAtPoint(string identifier, Vector3 position, float volume = 1f) {
		PlayAtPoint(ResourcesManager.GetAudio(identifier), position, volume);
	}

	private static AudioSource GetAudioSource() {
		AudioSource source;
		if (idleAudioSources.Count > 0) {
			source = idleAudioSources.Dequeue();
			source.gameObject.SetActive(true);
		} else {
			GameObject gameObject = new GameObject("Public Audio Source");
			gameObject.transform.parent = audioRoot;
			source = gameObject.AddComponent<AudioSource>();
			source.spatialBlend = 1f;
			source.loop = false;
		}

		return source;
	}

	private static IEnumerator ExeRecycleCoroutine(AudioSource source) {
		float time = source.clip.length;
		yield return new WaitForSeconds(time);
		source.Stop();
		source.gameObject.SetActive(false);
		idleAudioSources.Enqueue(source);
	}
}