using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPhysixMove : MonoBehaviour {

	//Advanced Move//
	/*
	Necessary first person script since y'all just want to make 
	the next PUBG. Just kidding. Utilizes concepts shown in the
	"Advanced Movement" scene to make a simple first person 
	controller.

	Movements:
	Normal - All Axis set to add, with simple clamp activated. localTransform set to Normal object that will rotate with "DownHit" Normal.
	Global - All Axis set to Add.
	Local - Same as Global, but localTransform set to the character.
	GlobalEquals - All Axis set to equal.
	LocalEquals - Same as GlobalEquals, but localTransform set to the character.
	Decelerate - All axis approach zero, with a script defined speed.
	*/

	public Physix physix;
	public Transform Normal;
	public float Speed = 1;
	public float MaxSpeed = 10;
	public float JumpSpeed = 10;
	public float Gravity = 0.5f;
	private bool fixedupdate = false;

	void FixedUpdate () {
		fixedupdate = true; //Used to maintain physics speed.
	}

	void Update () {
		//RespawnIfHitDeathFloor
		if(physix.IsTriggerEntering("Death", TriggerType.name)){
			transform.position = new Vector3(0,4,-10); //Reset Position
			physix.ApplyMovement("GlobalEquals", 0, AxisType.xyz, ValueType.value); //Kill Velocity
		}
		
		//Jump
		if(physix.IsColliding("DownHit") && Input.GetKeyDown("space")){physix.ApplyMovement("Global", JumpSpeed, AxisType.y, ValueType.value);} //Jump

		if(fixedupdate){
			if(physix.IsColliding("DownHit")){physix.ApplyMovement("Normal", -1, AxisType.y, ValueType.value);} //Simple way to keep character on the ground. Can very well be more complex.
			Normal.localRotation = Quaternion.FromToRotation (Vector3.up, physix.GetLocalNormal("DownHit")); //RotateNormal based on surfaceangle (normal)
			if(!physix.IsColliding("DownHit")){physix.ApplyMovement("Global", -Gravity, AxisType.y, ValueType.value);} //Gravity

			if(Input.GetKey("d")){physix.ApplyMovement("Normal", Speed, AxisType.x, ValueType.value, MaxSpeed, AxisType.x, ValueType.max);} //Move right
			if(Input.GetKey("a")){physix.ApplyMovement("Normal", -Speed, AxisType.x, ValueType.value, -MaxSpeed, AxisType.x, ValueType.min);} //Move left
			if(Input.GetKey("w")){physix.ApplyMovement("Normal", Speed, AxisType.z, ValueType.value, MaxSpeed, AxisType.z, ValueType.max);} //Move forward
			if(Input.GetKey("s")){physix.ApplyMovement("Normal", -Speed, AxisType.z, ValueType.value, -MaxSpeed, AxisType.z, ValueType.min);} //Move backwards
			if(!Input.GetKey("a") && !Input.GetKey("d")){physix.ApplyMovement("Decelerate", Speed, AxisType.x, ValueType.value);} //Decelerate if needed
			if(!Input.GetKey("w") && !Input.GetKey("s")){physix.ApplyMovement("Decelerate", Speed, AxisType.z, ValueType.value);} //Decelerate if needed
			fixedupdate = false;
		}
	}
}
