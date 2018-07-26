using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton {

	public static float minimumTimeScale;
	public static float maximumTimeScale;

	public static float TimeScale {
		get { return timeScale; }
		private set {
			timeScale = value;
			Time.timeScale = value;
		}
	}

	private static bool isPaused;
	private static float timeScale;
	private static readonly List<TimeEffectRequest> requests = new List<TimeEffectRequest>(4);

	private void Start() {
		timeScale = Time.timeScale;
	}

	private void Update() {
		if (!isPaused) {
			foreach (var request in requests) {
				if (request.duration > 0) {
					request.lifeRemained -= Time.unscaledDeltaTime;
					if (request.lifeRemained <= 0) {
						DestroyRequest(request);
					}
				}
			}
		}
	}

	public static void Play() {
		isPaused = false;
		CalculateTimeScale();
	}

	public static void Pause() {
		isPaused = true;
		TimeScale = 0;
	}

	public static void HandleRequest(TimeEffectRequest request) {
		request.lifeRemained = request.duration;
		requests.Add(request);
		CalculateTimeScale();
	}

	private static void DestroyRequest(TimeEffectRequest request) {
		requests.Remove(request);
	}

	private static void CalculateTimeScale() {
		TimeScale = 0;
		foreach (var request in requests) {
			CalculateTimeScale(request);
		}
	}

	private static void CalculateTimeScale(TimeEffectRequest request) {
		float scale;
		if (request.type == TimeEffectRequestType.Add) {
			scale = timeScale + request.value;
		} else {
			scale = timeScale * request.value;
		}

		if (scale < minimumTimeScale) {
			scale = minimumTimeScale;
		}

		if (scale > maximumTimeScale) {
			scale = maximumTimeScale;
		}

		TimeScale = scale;
	}
}

public class TimeEffectRequest {

	public TimeEffectRequestType type;
	public float value;
	public float duration;
	public float lifeRemained;
	public float max = -1f;
	public float min = -1f;
}

public enum TimeEffectRequestType {
	Add,
	Multiply
}