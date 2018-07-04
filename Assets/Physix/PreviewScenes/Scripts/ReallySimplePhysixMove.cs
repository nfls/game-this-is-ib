using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReallySimplePhysixMove : MonoBehaviour {

	//Really Simple Move//
	/*
	This Movement utilizes only one predefined physix movement, which simply sets 
	Velocity equal to three axis floats X, Y, and Z. This is the simplest form
	of movement, as each variable (X,Y,and Z) equates to the 'speed' of that axis.
	Sadly, this form of movement totally bipasses all benefits of using a rigidbody
	based character controller, so it is not recommended for platformers or action
	games.

	Movements:
	Global - All Axis set to equal. Will be called once in script.
	*/

	public Physix physix;
	public float Speed = 1;
	public float MaxSpeed = 10;
	public float JumpSpeed = 10;
	public float Gravity = 0.25f;
	private float X = 0;
	private float Y = 0;
	private float Z = 0;
	private bool fixedupdate = false;

	//We use fixed update and a bool to ensure physics acts the same at different framerates.
	void FixedUpdate () {
		fixedupdate = true;
	}

	void Update () {
		if(physix.IsColliding("Grounded")){ //are we on the ground?
			if(Y < 0){Y = 0;}
			if(Input.GetKeyDown("space")){Y = JumpSpeed;} //Jump on space press
		}
		if(fixedupdate){
			if(!physix.IsColliding("Grounded")){ //are we not on the ground?
				Y -= Gravity; //Apply Gravity
			}
			if(Input.GetKey("right") && X < MaxSpeed){X += Speed;} //Move right
			if(Input.GetKey("left") && X > -MaxSpeed){X -= Speed;} //Move left
			if(!Input.GetKey("right") && !Input.GetKey("left") && X != 0){X += (X > 0 ? -Speed : Speed);} //Decelerate X-Axis
			if(Input.GetKey("up") && Z < MaxSpeed){Z += Speed;} //Move Forwards
			if(Input.GetKey("down") && Z > -MaxSpeed){Z -= Speed;} //Move Backwards
			if(!Input.GetKey("up") && !Input.GetKey("down") && Z != 0){Z += (Z > 0 ? -Speed : Speed);} //Decelerate Z-Axis
			if(Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down")){ //should the character be moving?
				Vector3 look = (Input.GetKey("left") ? -Vector3.right : Vector3.zero) + 
						   		(Input.GetKey("right") ? Vector3.right : Vector3.zero) +
						   		(Input.GetKey("up") ? Vector3.forward : Vector3.zero) +
						   		(Input.GetKey("down") ? Vector3.back : Vector3.zero);
				if(look != Vector3.zero){transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look),0.25f);} //rotate character
			}

			//finally, apply movement by modifying "Global" with our 3 axis variables.
			physix.ApplyMovement("Global", X, AxisType.x, ValueType.value, Y, AxisType.y, ValueType.value, Z, AxisType.z, ValueType.value);
			fixedupdate = false;
		}
		transform.GetComponent<Renderer>().material.color = (physix.IsColliding("Grounded") ? new Color (1,1,1,1) : new Color (0.5f,0.5f,0.5f,1)); //change color if grounded
	}
}
