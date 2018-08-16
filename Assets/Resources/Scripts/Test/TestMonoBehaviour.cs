using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestMonoBehaviour : MonoBehaviour {

	private int i;

	private void Awake() {
		PrintMethod("Awake");
	}

	private void OnEnable() {
		PrintMethod("On Enable");
	}

	private void OnDisable() {
		PrintMethod("On Disable");
	}

	private void Start() {
		PrintMethod("Start");
	}

	public void Init() {
		PrintMethod("Init");
	}

	private void PrintMethod(string method) {
		i++;
		Debug.Log(i + " | " + method + " | " + Time.time);
	}

	private void OnCollisionEnter() {
		Debug.Log("Enter " + Time.frameCount);
	}

	private void OnCollisionStay() {
		Debug.Log("Stay " + Time.frameCount);
	}

	private void OnCollisionExit() {
		Debug.Log("Exit " + Time.frameCount);
	}
}