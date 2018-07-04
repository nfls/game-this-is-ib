using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerribleFPSCamera : MonoBehaviour {
	
	public float RotateSpeed = 0.1f;
	public Transform RotateX;
	public Transform RotateY;
	private float X;

	void Update () {
		X = Mathf.Clamp(X - Input.GetAxis("Mouse Y") * RotateSpeed, -80, 80);
		RotateX.eulerAngles = new Vector3(X,RotateX.eulerAngles.y,RotateX.eulerAngles.z);
		RotateY.eulerAngles = new Vector3(RotateY.eulerAngles.x,RotateY.eulerAngles.y + Input.GetAxis("Mouse X") * RotateSpeed,RotateY.eulerAngles.z);
	}
}
