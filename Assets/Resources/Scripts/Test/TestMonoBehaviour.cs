using System.Collections.Generic;
using UnityEngine;
using Physics = RotaryHeart.Lib.PhysicsExtension.Physics;

public class TestMonoBehaviour : MonoBehaviour {

	public Collider collider;
	public float distance = 10f;

	public void Update() {
		if (Input.GetKeyUp(KeyCode.T)) {
			RaycastHit hitInfo;
			Vector3 diff = collider.transform.position - transform.position;
			Ray ray = new Ray(transform.position - diff, diff);
			Physics.Raycast(ray, Physics.PreviewCondition.Editor, 10f, Color.green, Color.red);
			if (collider.Raycast(ray, out hitInfo, distance)) {
				Debug.Log(Time.time + " Hit " + hitInfo.normal.ToString("F4") + " !");
			} else Debug.Log(Time.time + " Miss !");
		}
	}
}