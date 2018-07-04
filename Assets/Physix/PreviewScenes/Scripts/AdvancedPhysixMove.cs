using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedPhysixMove : MonoBehaviour {

	//Advanced Move//
	/*
	This Movement utilizes a mixture of "Simple Move" and "Really Simple Move" to 
	create a perfect balance between script modifiers and preset Movement Options.
	Also uses an extra transform to rotate the Character according to the surface angle.
	Also uses "DownHit" Enter/Exit checks for a nice "Squish" animation effect.
	This form of movement is the most practical for complex movement systems, and can
	do pretty much anything you put your mind to.

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
	public Transform VisualCharacter;
	public float Speed = 1;
	public float MaxSpeed = 10;
	public float JumpSpeed = 10;
	public float Gravity = 0.5f;
	private bool fixedupdate = false;
	private int WallJumpWait = 0;

	void Awake () {
		Normal.parent = null; //Normal is used for rotation calculations, so it can't have a parent.
	}

	void FixedUpdate () {
		fixedupdate = true; //Used to maintain physics speed.
	}

	void Update () {
		//Squishy Effect
		VisualCharacter.localScale = Vector3.Lerp(VisualCharacter.localScale, Vector3.one, 0.1f);
		VisualCharacter.localPosition = Vector3.Lerp(VisualCharacter.localPosition, Vector3.zero, 0.1f);
		if(physix.IsEntering("DownHit")){
		VisualCharacter.localScale = new Vector3(1.5f,0.5f,1.5f);
		VisualCharacter.localPosition = new Vector3(0,-0.25f,0);
		}

		//RespawnIfHitDeathFloor
		if(physix.IsTriggerEntering("Death", TriggerType.name)){
			transform.position = new Vector3(0,4,-10); //Reset Position
			physix.ApplyMovement("GlobalEquals", 0, AxisType.xyz, ValueType.value); //Kill Velocity
		}
		
		//Jump and WallJump
		bool moving = Input.GetKey("left") || Input.GetKey("right") || Input.GetKey("up") || Input.GetKey("down"); //should we be moving?
		if(physix.IsColliding("DownHit") && Input.GetKeyDown("space")){physix.ApplyMovement("Global", JumpSpeed, AxisType.y, ValueType.value); VisualCharacter.localScale = new Vector3(0.5f,1.5f,0.5f);} //Jump
		if(physix.IsColliding("FrontHit") && !physix.IsColliding("DownHit") && Input.GetKeyDown("space")){physix.ApplyMovement("Local", JumpSpeed, AxisType.y, ValueType.value, -JumpSpeed, AxisType.z, ValueType.value);WallJumpWait = 10; VisualCharacter.localScale = new Vector3(0.5f,1.25f,0.75f);} //WallJump
		
		//Physics Stuff
		if(fixedupdate){
			if(physix.IsColliding("DownHit")){physix.ApplyMovement("Normal", -1, AxisType.y, ValueType.value);} //Simple way to keep character on the ground.
			Normal.rotation = Quaternion.FromToRotation (Vector3.up, physix.GetNormal("DownHit")); //RotateNormal based on surfaceangle (normal)
			VisualCharacter.localRotation = Quaternion.Lerp(VisualCharacter.localRotation, Quaternion.FromToRotation (Vector3.up, physix.GetLocalNormal("DownHit")), 0.25f); //Localrotation for visual effect

			if(!physix.IsColliding("DownHit")){physix.ApplyMovement("Global", -Gravity, AxisType.y, ValueType.value);} //Gravity
			if(WallJumpWait == 0){
			if(Input.GetKey("right")){physix.ApplyMovement("Normal", Speed, AxisType.x, ValueType.value, MaxSpeed, AxisType.x, ValueType.max);} //Move right
			if(Input.GetKey("left")){physix.ApplyMovement("Normal", -Speed, AxisType.x, ValueType.value, -MaxSpeed, AxisType.x, ValueType.min);} //Move left
			if(Input.GetKey("up")){physix.ApplyMovement("Normal", Speed, AxisType.z, ValueType.value, MaxSpeed, AxisType.z, ValueType.max);} //Move forward
			if(Input.GetKey("down")){physix.ApplyMovement("Normal", -Speed, AxisType.z, ValueType.value, -MaxSpeed, AxisType.z, ValueType.min);} //Move backwards
			if(!Input.GetKey("left") && !Input.GetKey("right")){physix.ApplyMovement("Decelerate", Speed, AxisType.x, ValueType.value);} //Decelerate if needed
			if(!Input.GetKey("up") && !Input.GetKey("down")){physix.ApplyMovement("Decelerate", Speed, AxisType.z, ValueType.value);} //Decelerate if needed
			}
			if(moving){
				Vector3 look = (Input.GetKey("left") ? -Vector3.right : Vector3.zero) +
						   		(Input.GetKey("right") ? Vector3.right : Vector3.zero) +
						   		(Input.GetKey("up") ? Vector3.forward : Vector3.zero) +
						   		(Input.GetKey("down") ? Vector3.back : Vector3.zero);
				if(look != Vector3.zero){transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(look),0.25f);} //rotate character
			}
			if(WallJumpWait > 0){WallJumpWait--;}
			fixedupdate = false;
		}
		VisualCharacter.GetComponent<Renderer>().material.color = (physix.IsColliding("DownHit") || physix.IsColliding("FrontHit") ? new Color (1,1,1,1) : new Color (0.5f,0.5f,0.5f,1)); //change color if grounded
	}
}
