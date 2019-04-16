using UnityEngine;

public class TestMonoBehaviour : MonoBehaviour {
	private void OnCollisionEnter(Collision other) {
		Debug.Log(name + " Collides !");
	}
}