using UnityEngine;

public class AxisDetector {

	public static float doublePressThreshold = 0.5f;
	public static float chargeThreshold = 1f;

	string name;
	bool isPressed;
	bool isHeld;
	bool isReleased;

	float offset;

	public string Name {
		get {
			return name;
		}
	}



	public AxisDetector(string name) {
		this.name = name;
	}

	public void Refresh() {
		offset = Input.GetAxis(name);
	}
}
