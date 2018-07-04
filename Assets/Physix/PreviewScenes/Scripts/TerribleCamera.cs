using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerribleCamera : MonoBehaviour {
	
	public Transform Target;
	public Vector3 Offset = Vector3.zero;

	void Update () {
		transform.position = Target.position + Offset;
		transform.LookAt(Target);
	}
}
