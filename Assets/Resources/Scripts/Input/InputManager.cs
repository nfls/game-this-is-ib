using System.Collections.Generic;

public class InputManager : MonoSingleton {

	private static List<InputDetector> registeredDetectors = new List<InputDetector>(10);
	private static Dictionary<InputDetector, int> detectorRecords = new Dictionary<InputDetector, int>(10);

	private void Update() {
		foreach (InputDetector detector in registeredDetectors) detector.Refresh();
	}

	public static void NotifyDetectorOnBusy(InputDetector detector) {
		if (!registeredDetectors.Contains(detector)) {
			detectorRecords[detector] = 1;
			RegisterDetector(detector);
		} else detectorRecords[detector] += 1;
	}

	public static void NotifyDetectorOnIdle(InputDetector detector) {
		if (registeredDetectors.Contains(detector)) {
			detectorRecords[detector] -= 1;
			if (detectorRecords[detector] == 0) RemoveDetector(detector);
		}
	}

	private static void RegisterDetector(InputDetector inputDetector) {
		registeredDetectors.Add(inputDetector);
	}

	private static void RemoveDetector(InputDetector inputDetector) {
		inputDetector.Reset();
		registeredDetectors.Remove(inputDetector);
	}
}
