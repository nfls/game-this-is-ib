using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePhysixMove : MonoBehaviour {

	//Simple Move//
	/* 
	This Movement utilizes a few preset movement options such as "Walk", "Jump",
	and "Decelerate". Each preset's values are predefined for use in the Physix 
	Inspector, so we don't use any movement modifiers in this script. This form
	of movement keeps your code clean and easy to understand, but leads to a LONG list
	of movement options which "might" get a little overbearing when building 
	complex movement systems; As per the name "Simple".

	Movements:
	Walk - X axis approaches zero, while Z axis is added to (with max), localTransform set to this transform.
	Jump - Sets Y axis equal to a defined height
	Gravity - Subtracts from Y axis.
	Decelerate - Makes both X and Z axii approach zero.
	*/
	
	public Physix physix;
	private bool fixedupdate = false;

	//We use fixed update and a bool to ensure physics acts the same at different framerates.
	void FixedUpdate () {
		fixedupdate = true;
	}

	void Update () {
		bool moving = Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"); //should we be moving?
		if(physix.IsColliding("Grounded")){ //are we on the ground?
			if(Input.GetKeyDown("space")){physix.ApplyMovement("Jump");} //Jump on space press
		}
		if(fixedupdate){
			if(!moving){physix.ApplyMovement("Decelerate");}
			if(!physix.IsColliding("Grounded")){ //are we not on the ground?
				physix.ApplyMovement("Gravity"); //Apply Gravity
			}
			if(moving){
				physix.ApplyMovement("Walk"); //MoveCharacter
				Vector3 look = (Input.GetKey("left") ? -Vector3.right : Vector3.zero) + 
						   		(Input.GetKey("right") ? Vector3.right : Vector3.zero) +
						   		(Input.GetKey("up") ? Vector3.forward : Vector3.zero) +
						   		(Input.GetKey("down") ? Vector3.back : Vector3.zero);
				if(look != Vector3.zero){transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look),0.5f);} //rotate character
			}
			fixedupdate = false;
		}
		transform.GetComponent<Renderer>().material.color = (physix.IsColliding("Grounded") ? new Color (1,1,1,1) : new Color (0.5f,0.5f,0.5f,1)); //change color if grounded
	}
}
