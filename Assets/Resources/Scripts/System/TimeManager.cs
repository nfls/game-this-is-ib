using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton {

	public static float MinimumTimeScale = 0;
	public static float MaximumTimeScale = 100f;
	public static TimeManager Instance;

	public static float TimeScale {
		get { return timeScale; }
		private set {
			timeScale = value;
			Time.timeScale = value;
		}
	}

	private static bool isPaused;
	private static float timeScale;
	
	private static readonly List<TimeEffectRequest> globalRequests = new List<TimeEffectRequest>();
	private static readonly List<TimeEffectRequest> storyRequests = new List<TimeEffectRequest>();
	private static readonly List<TimeEffectRequest> actionRequests = new List<TimeEffectRequest>();

	private void Awake() {
		timeScale = Time.timeScale;
		Instance = this;
	}

	private void Update() {
		if (!isPaused) {
			bool flag = false;
			float timeDiff = Time.unscaledDeltaTime;
			for (int i = 0; i < globalRequests.Count; i++) {
				TimeEffectRequest request = globalRequests[i];
				request.lifeRemained -= timeDiff;
				if (request.lifeRemained <= 0f) {
					globalRequests.RemoveAt(i);
					flag = true;
				}
			}
			
			for (int i = 0; i < storyRequests.Count; i++) {
				TimeEffectRequest request = storyRequests[i];
				request.lifeRemained -= timeDiff;
				if (request.lifeRemained <= 0f) {
					storyRequests.RemoveAt(i);
					flag = true;
				}
			}
			
			for (int i = 0; i < actionRequests.Count; i++) {
				TimeEffectRequest request = actionRequests[i];
				request.lifeRemained -= timeDiff;
				if (request.lifeRemained <= 0f) {
					actionRequests.RemoveAt(i);
					flag = true;
				}
			}
			
			if (flag) CalculateTimeScale();
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
		switch (request.layer) {
			case TimeEffectLayer.Globe: globalRequests.Add(request);
				break;
			case TimeEffectLayer.Story: storyRequests.Add(request);
				break;
			case TimeEffectLayer.Action: actionRequests.Add(request);
				break;
		}
		
		CalculateTimeScale();
	}

	private static void CalculateTimeScale() {
		float scale = CalculateGlobalScale() * CalculateStoryScale() * CalculateActionScale();
		if (scale < MinimumTimeScale) scale = MinimumTimeScale;
		else if (scale > MaximumTimeScale) scale = MaximumTimeScale;
		TimeScale = scale;
	}

	private static float CalculateGlobalScale() {
		float globalScale = 1f;
		int priority = -1;
		foreach (var request in globalRequests)
			if (request.priority >= priority) globalScale = request.value;
		return globalScale;
	}

	private static float CalculateStoryScale() {
		float storyScale = 1f;
		int priority = -1;
		foreach (var request in storyRequests)
			if (request.priority >= priority) storyScale = request.value;
		return storyScale;
	}

	private static float CalculateActionScale() {
		float actionScale = 1f;
		foreach (var request in actionRequests)
			if (request.value < actionScale) actionScale = request.value;
		return actionScale;
	}
}

[Serializable]
public class TimeEffectRequest {

	public TimeEffectLayer layer;
	public int priority;
	public float value;
	public float duration;
	public float lifeRemained;
}

public enum TimeEffectLayer {
	Globe,
	Story,
	Action
}