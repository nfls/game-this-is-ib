using UnityEngine;
using Debug = UnityEngine.Debug;

public class TestMonoBehaviour : MonoBehaviour {

	private void Awake() {
		Debug.Log(name + " " + transform.right.x + ", " + transform.right.y + ", " + transform.right.z);
	}
}