-------------Physix Version 1.1--------------
---Copyright Matt "Melonhead" Sellers 2018---

############################################################################################
############################################################################################
############################################################################################

Physix is a rigidbody character controller that allows for perfect customization of any 
collision and/or movement based attributes; Allowing you to make the perfect controller for
any game object or project. Physix simplifies collision handling by doing any and all 
calculations you WOULD need for your game automatically, allowing you to simplify your code, and call
collision functions in Update and FixedUpdate. It is highly recommended to study the provided 
demo scenes to get a full understanding on how this character controller functions, so you too, 
can build awesome games with awesome Physics.

PS: Remember to set the rotation constraints on the Physix's rigidbody to freeze, as this 
will prevent your character from rotating uncontrollably.

############################################################################################
############################################################################################
############################################################################################

-Collisions-
Collisions utilize normal based angles to define hitboxes that can be called through script.

-Collision Variables-
Name : Name which is used when calling this collision in script.
LocalTransform : If filled, calculates collision based on LocalTransform's rotation.
Ranges : List of defined ranges which trigger collision. A range consists of an axis (X,Y, or Z),
			a form of logic (greater, less, and/or equal), and an angle (angle of collision face).
			if all ranges in a collision return true, then that collision will be triggered.

-Collision Functions-
Physix.IsColliding(String Name) //Returns true if collision "name" is currently colliding.
Physix.IsExiting(String Name) //Returns true if collision "name" is currently exiting collision.
Physix.IsEntering(String Name) //Returns true if collision "name" is currently entering collision.

-Getters-
Physix.GetNormal(String Name) //Returns the average normal (Vector3) of all normals in this collision.
Physix.GetLocalNormal(String Name) //same as above, but local to "name's" localTransform variable.
Physix.GetPoint(String Name) //Returns the average point (Vector3) of all points in this collision.
Physix.GetLocalPoint(String Name) //same as above, but local to "name's" localTransform variable.
Physix.GetCollision(String Name) //Returns the Collision type data for collision "Name", if you so need it.
Physix.GetTag(String Name) //Returns the tag of the collider "name" collided with.

-Setters-
Physix.SetCollisionValue(String Name, float value) //Sets the angle of the first range in collision "name" to value.
Physix.SetCollisionValue(String Name, int RangeIndex, float value) //same as above, except with a specific range index.

############################################################################################
############################################################################################
############################################################################################

-Triggers-
Simple way to check if Physix collides with a trigger, and getting data about said trigger.
A trigger is identified by using the TriggerType Enum; ie if TriggerType is set to TriggerType.name,
then Name will be checked to see if it equals the name of the trigger we hit.

enum TriggerType {name, tag, layer} //determines what to look for on the trigger we collided with.

-Trigger Functions-
Physix.IsTriggering(String Name, TriggerType type) //returns true if we are currently in defined trigger
Physix.IsTriggerEntering(String Name, TriggerType type) //returns true if we entered defined trigger
Physix.IsTriggerExiting(String Name, TriggerType type) //returns true if we exited defined trigger

-Getters-
Physix.GetTrigger(String Name, TriggerType type) //returns defined Collider we are currently in.
Physix.GetTriggerEntering(String Name, TriggerType type) //returns defined Collider we are currently entering.
Physix.GetTriggerExiting(String Name, TriggerType type) //returns defined Collider we are currently exiting.

-Examples-
Physix p = GetComponent<Physix>();
p.IsTriggering("Cube", TriggerType.name); //returns true if we are currently in a trigger named "Cube"
p.IsTriggerEntering("Wall", TriggerType.tag); //returns true if we are currently exiting a trigger with tag "Wall"
Collider exit = p.GetTriggerExiting("TransparentFX", TriggerType.layer); //exit variable is now the trigger we exited with layer "TransparentFX"

############################################################################################
############################################################################################
############################################################################################

-Movements-
Movements are predefined forms of moving the Physix Character Controller. They can have 
modifiers applied to them when they are called through script, so you can make them as
simple, or as complex as you want.

-Movement Variables-
Name : Name which is used when calling this movement in script.
LocalTransform : If filled, calculates movement based on LocalTransform's rotation.
X axis : How this movement will affect the X axis.
Y axis : How this movement will affect the Y axis.
Z axis : How this movement will affect the Z axis.

-Axis Variables-
Active : Will this axis apply movement? if axis is edited on script call, then active will be
			automatically set to true (for that one call).
Type : Will this movement add value to velocity, set velocity equal to value, or move velocity towards value?
Value : Value used by this axis to change velocity.
Minimum : Simple window to apply a lower cap to this axis.
Maximum : Simple window to apply a higher cap to this axis.

-Movement Functions-
When calling movement functions, you can apply modifiers using these two enums.

enum AxisType {x,y,z,xy,xz,yz,xyz} //used when applying movement modifiers.
enum ValueType {value, min, max, valuemin, valuemax, valueminmax} //used when applying movement modifiers.

Physix.ApplyMovement(String Name) //Applies movement option Name.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type) //Applies movement option Name, with Value applied to the Type on Axis.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type, float Value2, AxisType Axis2, ValueType Type2) //Applies movement option Name, but with two modifiers.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type, float Value2, AxisType Axis2, ValueType Type2, float Value3, AxisType Axis3, ValueType Type3) //Applies movement option Name, but with three modifiers.

Physix.ApplyMovement(String Name, Vector3 input, ValueType value) //Applies movement option Name, but with a vector3 modifier.

PS : Physix uses rigidbody.velocity to move the character, so to read the characters speed,
		you just need to call rigidbody.velocity. Use InverseTransformDirection(rigidbody.velocity)
		for local speed.

-Examples-
Physix p = GetComponent<Physix>();
p.ApplyMovement("Jump", 10, AxisType.y, ValueType.value); //Calls "Jump", but sets the y-axis value to 10
p.ApplyMovement("Walk", 10, AxisType.z, ValueType.max, -10, AxisType.z, ValueType.min); //Calls "Walk", but sets the z-axis min and max values to -10 and 10 respectively.
p.ApplyMovement("Strafe", 25, AxisType.xyz, ValueType.value); //Calls "Strafe", but sets the x, y, and z axis value to 25

############################################################################################
############################################################################################
############################################################################################

-Scene View Editing-
Collision angles can be edited by clicking and dragging the corners of the visible collision
cones seen in the scene view. Holding down the control key will snap the values to multiples of five, 
so you can easily select the perfect angle for your collision.

############################################################################################
############################################################################################
############################################################################################

-Scripting Reference-
An online scripting reference is available at https://sites.google.com/view/mattmelonheadsellers/scripting-reference/physix

-Scripting Reference-
enum AxisType {x,y,z,xy,xz,yz,xyz} //used when applying movement modifiers.
enum ValueType {value, min, max, valuemin, valuemax, valueminmax} //used when applying movement modifiers.
enum TriggerType {name, tag, layer} //determines what to look for on the trigger we collided with.

Physix.ApplyMovement(String Name) //Applies movement option Name.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type) //Applies movement option Name, with Value applied to the Type on Axis.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type, float Value2, AxisType Axis2, ValueType Type2) //Applies movement option Name, but with two modifiers.
Physix.ApplyMovement(String Name, float Value, AxisType Axis, ValueType Type, float Value2, AxisType Axis2, ValueType Type2, float Value3, AxisType Axis3, ValueType Type3) //Applies movement option Name, but with three modifiers.
Physix.ApplyMovement(String Name, float Value, Vector3 Value, ValueType Type) //Applies movement option Name, but with three modifiers of a vector3, and one value definition.

Physix.IsTriggering(String Name, TriggerType type) //returns true if we are currently in defined trigger
Physix.IsTriggerEntering(String Name, TriggerType type) //returns true if we entered defined trigger
Physix.IsTriggerExiting(String Name, TriggerType type) //returns true if we exited defined trigger
Physix.GetTrigger(String Name, TriggerType type) //returns defined Collider we are currently in.
Physix.GetTriggerEntering(String Name, TriggerType type) //returns defined Collider we are currently entering.
Physix.GetTriggerExiting(String Name, TriggerType type) //returns defined Collider we are currently exiting.

Physix.IsColliding(String Name) //Returns true if collision "name" is currently colliding.
Physix.IsExiting(String Name) //Returns true if collision "name" is currently exiting collision.
Physix.IsEntering(String Name) //Returns true if collision "name" is currently entering collision.
Physix.GetNormal(String Name) //Returns the average normal (Vector3) of all normals in this collision.
Physix.GetLocalNormal(String Name) //same as above, but local to "name's" localTransform variable.
Physix.GetPoint(String Name) //Returns the average point (Vector3) of all points in this collision.
Physix.GetLocalPoint(String Name) //same as above, but local to "name's" localTransform variable.
Physix.GetCollision(String Name) //Returns the Collision type data for collision "Name", if you so need it.
Physix.GetTag(String Name) //Returns the tag of the collider "name" collided with.
Physix.SetCollisionValue(String Name, float value) //Sets the angle of the first range in collision "name" to value.
Physix.SetCollisionValue(String Name, int RangeIndex, float value) //same as above, except with a specific range index.

-Getting Module Data-
You can get the raw module data from the inspector view by using these two functions. This isn't
reccomended as if you edit the data returned, it will change the default values of the module,
which might mess up other calculations if not utilized correctly.

Physix.GetCollisionModule(String Name); //Returns Collision class for collision module "Name"
Physix.GetMovementModule(String Name); //Returns Movement class for movement module "Name"

-Pausing and Playing-
Physix.Pause(); //Freezes physix until Play() is called. Perfect for pause menus.
Physix.Pause(float percent); //Same as pause, but physix retains it's velocity * percent. Great for bullet time or hitlag.
Physix.Play(); //Resumes physix if already paused.

############################################################################################
############################################################################################
############################################################################################

For technical help or for the reporting of bugs and glitches, please email me at business@mattmelonheadsellers.com.