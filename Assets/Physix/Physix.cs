using UnityEngine;
using System.Collections;

public enum AxisType {x,y,z,xy,xz,yz,xyz}
public enum ValueType {value, min, max, valuemin, valuemax, minmax, valueminmax}
public enum TriggerType {name, tag, layer}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Physix : MonoBehaviour {

static Collider[] SceneColliders;
private Component[] Colliders;
private bool pause = false;
private Vector3 PauseVelocity = Vector3.zero;
private Vector3 PauseAngularVelocity = Vector3.zero;
private Vector3 PauseVelocity2 = Vector3.zero;
private Vector3 PauseAngularVelocity2 = Vector3.zero;
[HideInInspector]
public Rigidbody rigidbody;
public PHYSIXCOLLISION[] Collisions;
public bool FixMovingPlatforms = true;
public bool PlatformsRetainVelocity = true;
public float PlatformsVelocityMultiplier = 25;
public LayerMask PlatformLayerMask = 0;
public bool TriggerCollision = false;
public float TriggerBounceMultiplier = 0.5f;
public LayerMask IgnoreCollision = 0;
public PHYSIXMOVE[] Movements;
//public Vector3 PhysicsRotation;
private float collisioncount = 0;
private float norm = 0.0f;
private bool RESET = false;
[HideInInspector]
public PHYSIXMOVE PHYSIXBUFFER;
private Rigidbody rb;
[HideInInspector]
public bool DisplayVisualSettings = false;
[HideInInspector]
public float HitboxScale = 1.0f;
[HideInInspector]
public Color HitBoxColor = new Color(1,1,1,0.25f);
[HideInInspector]
public bool UseDefualtInspector = false;
[HideInInspector]
public bool HideCollision = false;
[HideInInspector]
public bool HideMovement = false;
[HideInInspector]
public int SelectCollision = 0;
[HideInInspector]
public int SelectRange = 0;
[HideInInspector]
public bool HOLD = false;
[HideInInspector]
public Collider[] TriggerStays = new Collider[0];
[HideInInspector]
public Collider[] TriggerEnters = new Collider[0];
[HideInInspector]
public Collider[] TriggerExits = new Collider[0];

private float Buffer1Value = 0.0f;
private AxisType Buffer1Axis;
private ValueType Buffer1ValueType;
private float Buffer2Value = 0.0f;
private AxisType Buffer2Axis;
private ValueType Buffer2ValueType;
private float Buffer3Value = 0.0f;
private AxisType Buffer3Axis;
private ValueType Buffer3ValueType;
private float BufferValueCount = 0;

[System.Serializable]
public class PHYSIXCOLLISION {
	public string Name = "";
	[HideInInspector]
	public bool Enter = false;
	[HideInInspector]
	public bool Exit = false;
	[HideInInspector]
	public bool EnterB = false;
	[HideInInspector]
	public bool ExitB = false;
	[HideInInspector]
	public bool Active = false;
	[HideInInspector]
	public Vector3 Normal;
	[HideInInspector]
	public Vector3 LastNormal;
	[HideInInspector]
	public Vector3 Point;
	[HideInInspector]
	public Vector3 LastPoint;
	[HideInInspector]
	public float Count = 0;
	[HideInInspector]
	public Collision collision;
	[HideInInspector]
	public string tag = "";
	[HideInInspector]
	public bool Display = false;
	public Transform localTransform;
	public PHYSIXBOUNDS[] Ranges;
	public bool Snap = false;
	public float SnapOffset = 0.1f;
	public float SnapBreakVelocity = 10;
	public float ApplyMovementBreakVelocity = 10;
	public float SnapAngle = 60;
	[HideInInspector]
	public int SnapDelay;
	[HideInInspector]
	public Transform SnapTransform = null;
	public LayerMask IgnoreMask = 0;
	
	public PHYSIXCOLLISION () {
	Name = "New Collision";
	Enter = false;
	Exit = false;
	EnterB = false;
	ExitB = false;
	Active = false;
	SnapDelay = 0;
	Normal = new Vector3(0,0,0);
	LastNormal = new Vector3(0,0,0);
	LastPoint = new Vector3(0,0,0);
	Point = new Vector3(0,0,0);
	Count = 0;
	tag = "";
	Display = false;
	Snap = false;
	SnapOffset = 0.1f;
	SnapBreakVelocity = 10;
	SnapAngle = 60;
	Ranges = new PHYSIXBOUNDS[1];
	Ranges[0] = new PHYSIXBOUNDS();
	IgnoreMask = 0;
	}

	public void Equals (PHYSIXCOLLISION rhs) {
	Name = rhs.Name;
	Enter = false;
	Exit = false;
	EnterB = false;
	ExitB = false;
	Active = false;
	SnapDelay = 0;
	Normal = new Vector3(0,0,0);
	LastNormal = new Vector3(0,0,0);
	LastPoint = new Vector3(0,0,0);
	Point = new Vector3(0,0,0);
	Count = 0;
	tag = "";
	Display = false;
	Snap = rhs.Snap;
	SnapOffset = rhs.SnapOffset;
	SnapBreakVelocity = rhs.SnapBreakVelocity;
	SnapAngle = rhs.SnapAngle;
	localTransform = rhs.localTransform;
	}
};

[System.Serializable]
public class PHYSIXBOUNDS {
	[Tooltip ("Check collision on X axis.")]
	public bool x = false;
	[Tooltip ("Check collision on Y axis.")]
	public bool y = false;
	[Tooltip ("Check collision on Z axis.")]
	public bool z = false;
	[Tooltip ("Is normal*-90 less than value?")]
	public bool less = false;
	[Tooltip ("Is normal*-90 greater than value?")]
	public bool greater = false;
	[Tooltip ("Is normal*-90 equal to value?")]
	public bool equals = true;
	public float value = 0.0f;
	[HideInInspector]
	public bool Active = false;
	[HideInInspector]
	public bool SelectActive = false;

	public PHYSIXBOUNDS () {
	x = false;
	y = false;
	z = false;
	less = false;
	greater = false;
	equals = false;
	value = 0.0f;
	Active = false;
	}

	public void Equals (PHYSIXBOUNDS rhs) {
	x = rhs.x;
	y = rhs.y;
	z = rhs.z;
	less = rhs.less;
	greater = rhs.greater;
	equals = rhs.equals;
	value = rhs.value;
	Active = rhs.Active;
	}
};

[System.Serializable]
public class PHYSIXMOVE {
	public string Name = "";
	public PHYSIXMOVETYPE x;
	public PHYSIXMOVETYPE y;
	public PHYSIXMOVETYPE z;
	[HideInInspector]
	public bool Display = false;
	[Tooltip ("If filled, applies movement based on local rotation.")]
	public Transform localTransform;
	public PHYSIXMOVE () {
	Name = "New Movement";
	x = new PHYSIXMOVETYPE();
	y = new PHYSIXMOVETYPE();
	z = new PHYSIXMOVETYPE();
	Display = false;
	}

	public void Equals (PHYSIXMOVE rhs) {
	Name = rhs.Name;
	x.Equals(rhs.x);
	y.Equals(rhs.y);
	z.Equals(rhs.z);
	localTransform = rhs.localTransform;
	Display = false;
	}
};

[System.Serializable]
public class PHYSIXMOVETYPE {
[Tooltip ("Will this axis apply movement?")]
public bool active = false;
[Tooltip ("Value to apply, can be modified on-call through script.")]
public float value = 0.0f;
[Tooltip ("Keep this force from exceeding a min value?")]
public bool clampMin = false;
[Tooltip ("Keep this force from exceeding a max value?")]
public bool clampMax = false;
[Tooltip ("Forcefully set velocity to min if velocity exceeds min?")]
public bool forceMin = false;
[Tooltip ("Forcefully set velocity to max if velocity exceeds max?")]
public bool forceMax = false;
[Tooltip ("Smooths forced clamping, mostly used for deceleration.")]
public bool smoothForce = false;
[Tooltip ("Min value used in clamping, can be modified on-call through script.")]
public float min = 0.0f;
[Tooltip ("Max value used in clamping, can be modified on-call through script.")]
public float max = 0.0f;
[Tooltip ("Add value to velocity? Mostly used for acceleration.")]
public bool add = false;
[Tooltip ("Set velocity equal to value? Mostly used for quick actions, like jumping.")]
public bool equals = false;
[HideInInspector]
public bool Display = false;
[HideInInspector]
public bool DisplayMin = false;
[HideInInspector]
public bool DisplayMax = false;

public PHYSIXMOVETYPE () {
active = false;
value = 0.0f;
clampMin = false;
clampMax = false;
forceMin = false;
forceMax = false;
smoothForce = false;
min = 0.0f;
max = 0.0f;
add = false;
equals = false;
Display = false;
DisplayMin = false;
DisplayMax = false;
}

public void Equals (PHYSIXMOVETYPE rhs) {
active = rhs.active;
value = rhs.value;
clampMin = rhs.clampMin;
clampMax = rhs.clampMax;
forceMin = rhs.forceMin;
forceMax = rhs.forceMax;
smoothForce = rhs.smoothForce;
min = rhs.min;
max = rhs.max;
add = rhs.add;
equals = rhs.equals;
Display = false;
DisplayMin = false;
DisplayMax = false;
}
};

public void ApplyMovement ( string name  ){
int index = FindIndexM(name);
PHYSIXMOVE MOVE = Movements[index];
BufferValueCount = 0;
ApplyMovement(MOVE);
}
		
public void ApplyMovement ( string name ,   float VALUE ,   AxisType axis ,   ValueType value  ){
int index = FindIndexM(name);
PHYSIXMOVE MOVE= Movements[index];
BufferValueCount = 1;
Buffer1Value = VALUE;
Buffer1ValueType = value;
Buffer1Axis = axis;
ApplyMovement(MOVE);
}

public void ApplyMovement ( string name ,   float VALUE ,   AxisType axis ,   ValueType value ,   float VALUE2 ,   AxisType axis2 ,   ValueType value2  ){
int index= FindIndexM(name);
PHYSIXMOVE MOVE= Movements[index];
BufferValueCount = 2;
Buffer1Value = VALUE;
Buffer1ValueType = value;
Buffer1Axis = axis;
Buffer2Value = VALUE2;
Buffer2ValueType = value2;
Buffer2Axis = axis2;
ApplyMovement(MOVE);
}

public void ApplyMovement ( string name ,   float VALUE ,   AxisType axis ,   ValueType value ,   float VALUE2 ,   AxisType axis2 ,   ValueType value2 ,   float VALUE3 ,   AxisType axis3 ,   ValueType value3  ){
int index = FindIndexM(name);
PHYSIXMOVE MOVE = Movements[index];
BufferValueCount = 3;
Buffer1Value = VALUE;
Buffer1ValueType = value;
Buffer1Axis = axis;
Buffer2Value = VALUE2;
Buffer2ValueType = value2;
Buffer2Axis = axis2;
Buffer3Value = VALUE3;
Buffer3ValueType = value3;
Buffer3Axis = axis3;
ApplyMovement(MOVE);
}

public void ApplyMovement ( string name , Vector3 input, ValueType value){
int index = FindIndexM(name);
PHYSIXMOVE MOVE = Movements[index];
BufferValueCount = 3;
Buffer1Value = input.x;
Buffer1ValueType = value;
Buffer1Axis = AxisType.x;
Buffer2Value = input.y;
Buffer2ValueType = value;
Buffer2Axis = AxisType.y;
Buffer3Value = input.z;
Buffer3ValueType = value;
Buffer3Axis = AxisType.z;
ApplyMovement(MOVE);
}

public void ApplyMovement (PHYSIXMOVE MOVE){
if(!pause){
if(rigidbody == null){rigidbody = transform.GetComponent<Rigidbody>();}
bool localBool = (MOVE.localTransform != null);
Vector3 READ = (localBool ? MOVE.localTransform.InverseTransformDirection(rigidbody.velocity) : rigidbody.velocity);
Vector3 Velocity = READ;

bool  XACTIVE = false;
bool  YACTIVE = false;
bool  ZACTIVE = false;

float XVALUE = MOVE.x.value;
float XMIN = MOVE.x.min;
float XMAX = MOVE.x.max;

float YVALUE = MOVE.y.value;
float YMIN = MOVE.y.min;
float YMAX = MOVE.y.max;

float ZVALUE = MOVE.z.value;
float ZMAX = MOVE.z.max;
float ZMIN = MOVE.z.min;

if(BufferValueCount >= 1){
bool B1Value = (Buffer1ValueType == ValueType.value || Buffer1ValueType == ValueType.valuemin || Buffer1ValueType == ValueType.valuemax || Buffer1ValueType == ValueType.valueminmax);
bool B1Min = (Buffer1ValueType == ValueType.min || Buffer1ValueType == ValueType.minmax || Buffer1ValueType == ValueType.valuemin || Buffer1ValueType == ValueType.valueminmax);
bool B1Max = (Buffer1ValueType == ValueType.max || Buffer1ValueType == ValueType.minmax || Buffer1ValueType == ValueType.valuemax || Buffer1ValueType == ValueType.valueminmax);

if(Buffer1Axis == AxisType.x || Buffer1Axis == AxisType.xy || Buffer1Axis == AxisType.xz || Buffer1Axis == AxisType.xyz){
XACTIVE = true;
if(B1Value){XVALUE = Buffer1Value;}
if(B1Min){XMIN = Buffer1Value;}
if(B1Max){XMAX = Buffer1Value;}
}
if(Buffer1Axis == AxisType.y || Buffer1Axis == AxisType.xy || Buffer1Axis == AxisType.yz || Buffer1Axis == AxisType.xyz){
YACTIVE = true;
if(B1Value){YVALUE = Buffer1Value;}
if(B1Min){YMIN = Buffer1Value;}
if(B1Max){YMAX = Buffer1Value;}
}
if(Buffer1Axis == AxisType.z || Buffer1Axis == AxisType.xz || Buffer1Axis == AxisType.yz || Buffer1Axis == AxisType.xyz){
ZACTIVE = true;
if(B1Value){ZVALUE = Buffer1Value;}
if(B1Min){ZMIN = Buffer1Value;}
if(B1Max){ZMAX = Buffer1Value;}
}
}

if(BufferValueCount >= 2){
bool B2Value= (Buffer2ValueType == ValueType.value || Buffer2ValueType == ValueType.valuemin || Buffer2ValueType == ValueType.valuemax || Buffer2ValueType == ValueType.valueminmax);
bool B2Min= (Buffer2ValueType == ValueType.min || Buffer2ValueType == ValueType.minmax || Buffer2ValueType == ValueType.valuemin || Buffer2ValueType == ValueType.valueminmax);
bool B2Max= (Buffer2ValueType == ValueType.max || Buffer2ValueType == ValueType.minmax || Buffer2ValueType == ValueType.valuemax || Buffer2ValueType == ValueType.valueminmax);

if(Buffer2Axis == AxisType.x || Buffer2Axis == AxisType.xy || Buffer2Axis == AxisType.xz || Buffer2Axis == AxisType.xyz){
XACTIVE = true;
if(B2Value){XVALUE = Buffer2Value;}
if(B2Min){XMIN = Buffer2Value;}
if(B2Max){XMAX = Buffer2Value;}
}
if(Buffer2Axis == AxisType.y || Buffer2Axis == AxisType.xy || Buffer2Axis == AxisType.yz || Buffer2Axis == AxisType.xyz){
YACTIVE = true;
if(B2Value){YVALUE = Buffer2Value;}
if(B2Min){YMIN = Buffer2Value;}
if(B2Max){YMAX = Buffer2Value;}
}
if(Buffer2Axis == AxisType.z || Buffer2Axis == AxisType.xz || Buffer2Axis == AxisType.yz || Buffer2Axis == AxisType.xyz){
ZACTIVE = true;
if(B2Value){ZVALUE = Buffer2Value;}
if(B2Min){ZMIN = Buffer2Value;}
if(B2Max){ZMAX = Buffer2Value;}
}
}

if(BufferValueCount >= 3){
bool B3Value= (Buffer3ValueType == ValueType.value || Buffer3ValueType == ValueType.valuemin || Buffer3ValueType == ValueType.valuemax || Buffer3ValueType == ValueType.valueminmax);
bool B3Min= (Buffer3ValueType == ValueType.min || Buffer3ValueType == ValueType.minmax || Buffer3ValueType == ValueType.valuemin || Buffer3ValueType == ValueType.valueminmax);
bool B3Max= (Buffer3ValueType == ValueType.max || Buffer3ValueType == ValueType.minmax || Buffer3ValueType == ValueType.valuemax || Buffer3ValueType == ValueType.valueminmax);

if(Buffer3Axis == AxisType.x || Buffer3Axis == AxisType.xy || Buffer3Axis == AxisType.xz || Buffer3Axis == AxisType.xyz){
XACTIVE = true;
if(B3Value){XVALUE = Buffer3Value;}
if(B3Min){XMIN = Buffer3Value;}
if(B3Max){XMAX = Buffer3Value;}
}
if(Buffer3Axis == AxisType.y || Buffer3Axis == AxisType.xy || Buffer3Axis == AxisType.yz || Buffer3Axis == AxisType.xyz){
YACTIVE = true;
if(B3Value){YVALUE = Buffer3Value;}
if(B3Min){YMIN = Buffer3Value;}
if(B3Max){YMAX = Buffer3Value;}
}
if(Buffer3Axis == AxisType.z || Buffer3Axis == AxisType.xz || Buffer3Axis == AxisType.yz || Buffer3Axis == AxisType.xyz){
ZACTIVE = true;
if(B3Value){ZVALUE = Buffer3Value;}
if(B3Min){ZMIN = Buffer3Value;}
if(B3Max){ZMAX = Buffer3Value;}
}
}

if(MOVE.x.active || XACTIVE){
if(MOVE.x.add && (!MOVE.x.clampMin || Velocity.x >= XMIN || XVALUE > 0) && (!MOVE.x.clampMax || Velocity.x <= XMAX || XVALUE < 0)){Velocity.x += XVALUE;}
if(MOVE.x.equals){Velocity.x = XVALUE;}
if(MOVE.x.forceMin && Velocity.x < XMIN){Velocity.x = Mathf.MoveTowards(Velocity.x, XMIN, MOVE.x.smoothForce ? XVALUE : XMIN - Velocity.x);}
if(MOVE.x.forceMax && Velocity.x > XMAX){Velocity.x = Mathf.MoveTowards(Velocity.x, XMAX, MOVE.x.smoothForce ? XVALUE : Velocity.x - XMAX);}
}
if(MOVE.y.active || YACTIVE){
if(MOVE.y.add && (!MOVE.y.clampMin || Velocity.y >= YMIN || YVALUE > 0) && (!MOVE.y.clampMax || Velocity.y <= YMAX || YVALUE < 0)){Velocity.y += YVALUE;}
if(MOVE.y.equals){Velocity.y = YVALUE;}
if(MOVE.y.forceMin && Velocity.y < YMIN){Velocity.y = Mathf.MoveTowards(Velocity.y, YMIN, MOVE.y.smoothForce ? YVALUE : YMIN - Velocity.y);}
if(MOVE.y.forceMax && Velocity.y > YMAX){Velocity.y = Mathf.MoveTowards(Velocity.y, YMAX, MOVE.y.smoothForce ? YVALUE : Velocity.y - YMAX);}
}
if(MOVE.z.active || ZACTIVE){
if(MOVE.z.add && (!MOVE.z.clampMin || Velocity.z >= ZMIN || ZVALUE > 0) && (!MOVE.z.clampMax || Velocity.z <= ZMAX || ZVALUE < 0)){Velocity.z += ZVALUE;}
if(MOVE.z.equals){Velocity.z = ZVALUE;}
if(MOVE.z.forceMin && Velocity.z < ZMIN){Velocity.z = Mathf.MoveTowards(Velocity.z, ZMIN, MOVE.z.smoothForce ? ZVALUE : ZMIN - Velocity.z);}
if(MOVE.z.forceMax && Velocity.z > ZMAX){Velocity.z = Mathf.MoveTowards(Velocity.z, ZMAX, MOVE.z.smoothForce ? ZVALUE : Velocity.z - ZMAX);}
}
rigidbody.velocity = (localBool ? MOVE.localTransform.TransformDirection(Velocity) : Velocity);

for (int c = 0; c < Collisions.Length; c++) {
if(!Collisions[c].Snap || !Collisions[c].Active){continue;}
Vector3 Normal = Collisions[c].LastNormal;
float magnitude = Vector3.Project(rigidbody.velocity, Normal).magnitude;
if(magnitude >= Collisions[c].ApplyMovementBreakVelocity){Collisions[c].SnapDelay = Time.frameCount + 2;}

}

}
}

public void Pause () {
if(pause != true){pause = true; PauseVelocity = rigidbody.velocity; PauseAngularVelocity = rigidbody.angularVelocity; PauseVelocity2 = Vector3.zero; PauseAngularVelocity2 = Vector3.zero;}
rigidbody.velocity = PauseVelocity2;
rigidbody.angularVelocity = PauseAngularVelocity2;
}

public void Pause (float percent) {
if(pause != true){pause = true; PauseVelocity = rigidbody.velocity; PauseAngularVelocity = rigidbody.angularVelocity; PauseVelocity2 = rigidbody.velocity * percent; PauseAngularVelocity2 = rigidbody.angularVelocity * percent;}
rigidbody.velocity = PauseVelocity2;
rigidbody.angularVelocity = PauseAngularVelocity2;
}

public void Play () {
if(pause != false){pause = false; rigidbody.velocity = PauseVelocity; rigidbody.angularVelocity = PauseAngularVelocity;}
} 

public int FindIndexM ( string name  ){
for (int c = 0; c < Movements.Length; c++) {
if(Movements[c].Name == name){return c;}
}
Debug.LogError("Couldn't find Movement data for '"+name+".' Defaulting to '"+(Movements.Length > 0 ? Movements[0].Name : "Error")+".'");
return 0;
}

public int FindIndex ( string name  ){
for (int c = 0; c < Collisions.Length; c++) {
if(Collisions[c].Name == name){return c;}
}
Debug.LogError("Couldn't find Collision data for '"+name+".' Defaulting to '"+(Collisions.Length > 0 ? Collisions[0].Name : "Error")+".'");
return 0;
}

public PHYSIXMOVE GetMovementModule ( string name  ){
int index= FindIndexM(name);
return (Movements.Length == 0 ? null : Movements[index]);
}

public PHYSIXCOLLISION GetCollisionModule ( string name  ){
int index= FindIndex(name);
return (Collisions.Length == 0 ? null : Collisions[index]);
}

public Vector3 GetNormal ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? Vector3.zero : (Collisions[index].Normal / Collisions[index].Count).normalized);
}

public Collision GetCollision ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? null : Collisions[index].collision);
}

public string GetTag ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? null : Collisions[index].tag);
}

public Vector3 GetLocalNormal ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? Vector3.zero : Collisions[index].localTransform.InverseTransformDirection((Collisions[index].Normal / Collisions[index].Count).normalized));
}

public Vector3 GetPoint ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? Vector3.zero : (Collisions[index].Point / Collisions[index].Count));
}

public Vector3 GetLocalPoint ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Count == 0 ? Vector3.zero : Collisions[index].localTransform.InverseTransformPoint(Collisions[index].Point / Collisions[index].Count));
}

public void SetCollisionValue (string name, int rangeindex, float value){
int index= FindIndex(name);
Collisions[index].Ranges[rangeindex].value = value;
}

public void SetCollisionValue (string name, float value){
int index= FindIndex(name);
if(Collisions[index].Ranges.Length == 0){return;}
Collisions[index].Ranges[0].value = value;
}

public bool IsColliding ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Active && collisioncount != 0);
}

public bool IsExiting ( string name  ){
int index= FindIndex(name);
return (Collisions[index].Exit);
}

public bool IsEntering (string name){
int index= FindIndex(name);
return (Collisions[index].Enter);
}

public bool IsTriggering (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerStays.Length; i++) {
if(identifierType == TriggerType.name && TriggerStays[i].name == identifier){return true;}
if(identifierType == TriggerType.tag && TriggerStays[i].tag == identifier){return true;}
if(identifierType == TriggerType.layer && TriggerStays[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return true;}
}
return false;
}

public bool IsTriggerEntering (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerEnters.Length; i++) {
if(identifierType == TriggerType.name && TriggerEnters[i].name == identifier){return true;}
if(identifierType == TriggerType.tag && TriggerEnters[i].tag == identifier){return true;}
if(identifierType == TriggerType.layer && TriggerEnters[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return true;}
}
return false;
}

public bool IsTriggerExiting (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerExits.Length; i++) {
if(identifierType == TriggerType.name && TriggerExits[i].name == identifier){return true;}
if(identifierType == TriggerType.tag && TriggerExits[i].tag == identifier){return true;}
if(identifierType == TriggerType.layer && TriggerExits[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return true;}
}
return false;
}

public Collider GetTrigger (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerStays.Length; i++) {
if(identifierType == TriggerType.name && TriggerStays[i].name == identifier){return TriggerStays[i];}
if(identifierType == TriggerType.tag && TriggerStays[i].tag == identifier){return TriggerStays[i];}
if(identifierType == TriggerType.layer && TriggerStays[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return TriggerStays[i];}
}
return null;
}

public Collider GetTriggerEntering (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerEnters.Length; i++) {
if(identifierType == TriggerType.name && TriggerEnters[i].name == identifier){return TriggerEnters[i];}
if(identifierType == TriggerType.tag && TriggerEnters[i].tag == identifier){return TriggerEnters[i];}
if(identifierType == TriggerType.layer && TriggerEnters[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return TriggerEnters[i];}
}
return null;
}

public Collider GetTriggerExiting (string identifier, TriggerType identifierType){
for (int i = 0; i < TriggerExits.Length; i++) {
if(identifierType == TriggerType.name && TriggerExits[i].name == identifier){return TriggerExits[i];}
if(identifierType == TriggerType.tag && TriggerExits[i].tag == identifier){return TriggerExits[i];}
if(identifierType == TriggerType.layer && TriggerExits[i].gameObject.layer == LayerMask.NameToLayer(identifier)){return TriggerExits[i];}
}
return null;
}

public float AngleToNormal (float angle){
return -(Quaternion.Euler(0,angle,0) * Vector3.forward).x;
}

public Quaternion NormalToAngle (Vector3 normal){
return Quaternion.FromToRotation (Vector3.up, normal);
}

public float RoundToPower (float input, float power) {
		float powerMult = (Mathf.Pow(10, power));
		return Mathf.RoundToInt(input * powerMult) / powerMult;
}

void FixedUpdate () {
HandleMovingPlats();
collisioncount = 0;
Snap();
ResetTriggerBanks();
}

Vector3 ClosestPointOnTriangle (int index, MeshCollider mesh, Vector3 point) {
Vector3[] vertices = mesh.sharedMesh.vertices;
int[] triangles = mesh.sharedMesh.triangles;
Vector3 p0 = mesh.transform.TransformPoint(vertices[triangles[index * 3 + 0]]);
Vector3 p1 = mesh.transform.TransformPoint(vertices[triangles[index * 3 + 1]]);
Vector3 p2 = mesh.transform.TransformPoint(vertices[triangles[index * 3 + 2]]);

return ClosestPointOnTriangle(p0,p1,p2,point);
}

Vector3 ClosestPointOnTriangle (Vector3 p1,Vector3 p2,Vector3 p3, Vector3 point) {
Vector3 trianglepoint = (p1 + p2 + p3) / 3f;

Vector3 normal = Vector3.Cross((p2-p1).normalized, (p3-p1).normalized);
Vector3 returnvector = Vector3.ProjectOnPlane(point - trianglepoint, normal);
Vector3 endpoint = trianglepoint + returnvector;

Vector3 p1direct = (ClosestPointOnLine(p2, p3, trianglepoint, true) - trianglepoint);
Vector3 p2direct = (ClosestPointOnLine(p1, p3, trianglepoint, true) - trianglepoint);
Vector3 p3direct = (ClosestPointOnLine(p2, p1, trianglepoint, true) - trianglepoint);
if(Vector3.Project(returnvector, p1direct.normalized).magnitude > p1direct.magnitude && Vector3.Angle(returnvector, p1direct) < 90){endpoint = ClosestPointOnLine(p2, p3, endpoint);}
returnvector = Vector3.ProjectOnPlane(endpoint - trianglepoint, normal);
if(Vector3.Project(returnvector, p2direct.normalized).magnitude > p2direct.magnitude && Vector3.Angle(returnvector, p2direct) < 90){endpoint = ClosestPointOnLine(p1, p3, endpoint);}
returnvector = Vector3.ProjectOnPlane(endpoint - trianglepoint, normal);
if(Vector3.Project(returnvector, p3direct.normalized).magnitude > p3direct.magnitude && Vector3.Angle(returnvector, p3direct) < 90){endpoint = ClosestPointOnLine(p2, p1, endpoint);}

return endpoint;
}

Vector3 ClosestPointOnLine (Vector3 line1, Vector3 line2, Vector3 point) {
Vector3 min = line2 - line1;
Vector3 add = Vector3.ClampMagnitude(Vector3.Project(point - line1, min), min.magnitude);
		return line1 + (Vector3.Angle(add, min) < 90 ? add : Vector3.zero);
}

Vector3 ClosestPointOnLine (Vector3 line1, Vector3 line2, Vector3 point, bool Unclamped) {
Vector3 min = line2 - line1;
Vector3 add = Vector3.Project(point - line1, min);
		return line1 + add;
}

float GetColliderSize () {
		float count = 0;
		float value = 0;
		foreach (Collider collide in Colliders) {
			count++;
			value += collide.bounds.size.magnitude;
		}
		return (count == 0 ? 1 : value / count);
}

float GetColliderRadius () {
		float count = 0;
		float value = 0;
		foreach (SphereCollider collide in Colliders) {
			count++;
			value += collide.radius;
		}
		return (count == 0 ? 1 : value / count);
}

public void DelaySnap ( string name  ){
int index= FindIndex(name);
		Collisions[index].SnapDelay = Time.frameCount + 1;
}

public void DelaySnap (string name, int frames){
int index= FindIndex(name);
		Collisions[index].SnapDelay = Time.frameCount + 1 + frames;
}

public Collider GetClosestCollider (Vector3 startpos, float maxradius) {
	float radius = 0.0f;
	float change = (maxradius / 10f);
	bool updown = false;
	Collider[] colliders = Physics.OverlapSphere(startpos, radius);
	int length = colliders.Length + 1;
	int count = 100;
	while (colliders.Length != length && count > 0) {
		if(colliders.Length < length) {
			if(updown != false) {updown = false; change /= 2f;}
			radius += change;
			colliders = Physics.OverlapSphere(startpos, radius);
		}
		if(colliders.Length > length) {
			if(updown != true) {updown = true; change /= 2f;}
			radius -= change;
			colliders = Physics.OverlapSphere(startpos, radius);
		}
		count--;
	}
	Collider returncollider = null;
	for (int i = 0; i < colliders.Length; i++) {
		if(!IsOurCollider(colliders[i])){returncollider = colliders[i]; break;}
	}
	return returncollider;
}

public void Snap () {
RaycastHit hit;
RaycastHit hit2;
for (int i = 0; i < Collisions.Length; i++) {
if(!Collisions[i].Snap || !Collisions[i].Active || Collisions[i].SnapDelay >= Time.frameCount){continue;}
Collision collision = Collisions[i].collision;
if(collision == null || collision.collider == null){continue;}

Vector3 Normal = Collisions[i].LastNormal.normalized;
Vector3 center = GetColliderCenter();

float snapdistance = Mathf.Clamp(rigidbody.velocity.magnitude, 1, 100000) * Collisions[i].SnapOffset;
Collider snapto = Collisions[i].collision.collider;
if (Physics.Raycast(new Ray(center, -Normal), out hit2, snapdistance, ~(Collisions[i].IgnoreMask | IgnoreCollision))) {
	snapto = hit2.collider;
}
if(snapto == null) {continue;}

bool ismesh = snapto.GetType() == typeof(MeshCollider);

Vector3 direction;
float distance;
if(Colliders.Length <= 0 || Colliders[0] == null) {continue;}
Vector3 nowscale = transform.localScale;
transform.localScale += (Vector3.one);
bool overlapped = Physics.ComputePenetration(
(Collider)Colliders[0], transform.position, transform.rotation,
snapto, snapto.transform.position, snapto.transform.rotation,
out direction, out distance
);
transform.localScale = nowscale;

if (Physics.SphereCast(center + direction.normalized * transform.localScale.magnitude * 0.5f, GetColliderRadius(), -direction.normalized, out hit, snapdistance, ~(Collisions[i].IgnoreMask | IgnoreCollision))) {
float maxangle = Mathf.Abs(Vector3.Angle(hit.normal, Normal));
if (Collisions[i].IgnoreMask != (Collisions[i].IgnoreMask | (1 << snapto.gameObject.layer)) && IgnoreCollision != (IgnoreCollision | (1 << snapto.gameObject.layer)) && snapto.Raycast(new Ray(center, -(direction.normalized + direction.normalized + direction.normalized + Normal) * 0.25f), out hit2, snapdistance)) {
	maxangle = Mathf.Abs(Vector3.Angle(hit2.normal, Normal));
}

Quaternion torot = Quaternion.FromToRotation(Normal, hit.normal);
Vector3 newvelo = torot * rigidbody.velocity;
float magnitude = Vector3.Project(rigidbody.velocity, hit.normal).magnitude;
float magnitude2 = Vector3.Project(rigidbody.velocity, Normal).magnitude;
Vector3 colliderpoint = GetClosestColliderPoint(hit.point);
//if(magnitude >= Collisions[i].SnapBreakVelocity && magnitude2 >= Collisions[i].SnapBreakVelocity){Debug.Log(rigidbody.velocity);}
//if(maxangle > Collisions[i].SnapAngle){Debug.Log("ANGLE");}
//if(!NormalCollides(i, hit.normal)){Debug.Log("CANNOT");}
if((magnitude >= Collisions[i].SnapBreakVelocity && magnitude2 >= Collisions[i].SnapBreakVelocity) || maxangle > Collisions[i].SnapAngle || !NormalCollides(i, hit.normal)){continue;}

Vector3 tomove = hit.point - colliderpoint;

transform.position += tomove;
Vector3 newvelo2 = Vector3.ProjectOnPlane(newvelo, hit.normal);
rigidbody.velocity = newvelo2;
collisioncount++;
Collisions[i].LastNormal = hit.normal;
}
}
}

void Update (){
	RESET = true;
	if(collisioncount < 0){collisioncount = 0;}
}

void ResizeArray <T> (ref T[] array, int size) {
T[] Temp = array;
int oldlength = array.Length;
array = new T[size];
for (int i = 0; i < oldlength; i++) {
if(Temp[i] == null || i >= array.Length){break;}
array[i] = Temp[i];
}
}

public void RefreshColliders () {
Colliders = GetComponents(typeof(Collider));
}

public bool IsOurCollider (Collider collide) {
for (int i = 0; i < Colliders.Length; i++) {
if(collide == Colliders[i]) {return true;}
}
return false;
}

void Start () {
RefreshColliders();
if(rigidbody == null){rigidbody = transform.GetComponent<Rigidbody>();}
}

Vector3 GetClosestColliderPoint (Vector3 point) {
Vector3 returnpoint = Vector3.zero;
bool first = true;
foreach (Collider collide in Colliders) {
Vector3 closerpoint = collide.ClosestPoint(point);
if(!first && Vector3.Distance(point, returnpoint) > Vector3.Distance(point, closerpoint)){returnpoint = closerpoint;}
if(first){returnpoint = closerpoint; first = false;}
}
return returnpoint;
}
Vector3 GetColliderCenter () {
Vector3 returnpoint = Vector3.zero;
		float count = 0;
foreach (Collider collide in Colliders) {
			returnpoint += collide.bounds.center;
			count++;
}
		return returnpoint / count;
}

Transform Platform;
bool firstplat = true;
Vector3 platlastpos = Vector3.zero;
Vector3 platlastpos2 = Vector3.zero;
Vector3 platvelocity = Vector3.zero;
Quaternion platlastrot = Quaternion.identity;
void HandleMovingPlats () {
	if(collisioncount <= 0 && Platform != null) {Platform = null; if(PlatformsRetainVelocity) {rigidbody.velocity += platvelocity * PlatformsVelocityMultiplier;}}
	if(!FixMovingPlatforms || Platform == null || collisioncount <= 0){return;}
	if(!firstplat) {
		Vector3 tomove = Platform.position - platlastpos;
		Quaternion torotate = Platform.rotation * Quaternion.Inverse(platlastrot);
		Vector3 tomove2 = (torotate * platlastpos2) - platlastpos2;
		
		platvelocity = (tomove + tomove2);
		transform.position += platvelocity;
		//transform.eulerAngles += torotate;
	}
	platlastpos = Platform.position;
	platlastrot = Platform.rotation;
	platlastpos2 = transform.position - Platform.position;
	firstplat = false;
}

void LateUpdate (){
if(collisioncount == 0){
for (int c = 0; c < Collisions.Length; c++) {
Collisions[c].Normal = Vector3.zero;
Collisions[c].Point = Vector3.zero;
Collisions[c].Count = 0;
Collisions[c].Active = false;
for (int b2 = 0; b2 < Collisions[c].Ranges.Length; b2++) {
Collisions[c].Ranges[b2].Active = false;
}
}
}
for (int c2 = 0; c2 < Collisions.Length; c2++) {
Collisions[c2].Enter = false;
Collisions[c2].Exit = false;
}
for (int c3 = 0; c3 < Collisions.Length; c3++) {
	bool collide = (Collisions[c3].Active && collisioncount != 0);
	if(Collisions[c3].ExitB != !collide){Collisions[c3].Exit = !collide; Collisions[c3].ExitB = !collide;}
	if(Collisions[c3].EnterB != collide){Collisions[c3].Enter = collide; Collisions[c3].EnterB = collide;}
}
ResetTriggerEnterExit();
}

void ResetTriggerBanks () {
if(TriggerStays.Length != 0){ResizeArray(ref TriggerStays, 0);}
if(TriggerEnters.Length != 0){ResizeArray(ref TriggerEnters, 0);}
if(TriggerExits.Length != 0){ResizeArray(ref TriggerExits, 0);}
}

void ResetTriggerEnterExit () {
if(TriggerEnters.Length != 0){ResizeArray(ref TriggerEnters, 0);}
if(TriggerExits.Length != 0){ResizeArray(ref TriggerExits, 0);}
}

void OnTriggerStay (Collider collider) {
ResizeArray(ref TriggerStays, TriggerStays.Length + 1);
TriggerStays[TriggerStays.Length-1] = collider;
}

void OnTriggerEnter (Collider collider) {
ResizeArray(ref TriggerEnters, TriggerEnters.Length + 1);
TriggerEnters[TriggerEnters.Length-1] = collider;
}

void OnTriggerExit (Collider collider) {
ResizeArray(ref TriggerExits, TriggerExits.Length + 1);
TriggerExits[TriggerExits.Length-1] = collider;
}

void OnCollisionEnter ( Collision collider  ){HandleCollision(collider); collisioncount++;}

void OnCollisionExit ( Collision collider  ){if(collisioncount > 0){collisioncount--;}}

void OnCollisionStay ( Collision collider  ){
	bool PlatformLayer = PlatformLayerMask == (PlatformLayerMask | (1 << collider.gameObject.layer)) || IgnoreCollision == (IgnoreCollision | (1 << collider.gameObject.layer));
	if(!PlatformLayer && Platform != collider.transform) { Platform = collider.transform; firstplat = true;}
	collisioncount++;
	HandleCollision(collider);
}

bool NormalCollides (int index, Vector3 normal) {
bool BOOL = true;
Vector3 pointpoint = ((Collisions[index].localTransform != null) ? Collisions[index].localTransform.InverseTransformDirection(normal) : normal);

for (int b = 0; b < Collisions[index].Ranges.Length; b++) {
	if(Collisions[index].Ranges[b].x){norm = pointpoint.x;}
	if(Collisions[index].Ranges[b].y){norm = pointpoint.y;}
	if(Collisions[index].Ranges[b].z){norm = pointpoint.z;}
	float normround = RoundToPower(norm, 3);
	float round = RoundToPower(AngleToNormal(Collisions[index].Ranges[b].value), 3);
	bool BOOL2 = (Collisions[index].Ranges[b].greater && norm < AngleToNormal(Collisions[index].Ranges[b].value)) ||
	(Collisions[index].Ranges[b].less && norm > AngleToNormal(Collisions[index].Ranges[b].value)) ||
	(Collisions[index].Ranges[b].equals && normround == round);
	BOOL = (BOOL && BOOL2);
	}
	return BOOL;
}

Vector3 AverageNormal (ContactPoint[] array) {
		Vector3 end = Vector3.zero;
		for (int i = 0; i < array.Length; i++) {
			end += array[i].normal;
		}
		return end / array.Length;
}

Vector3 AveragePoint (ContactPoint[] array) {
		Vector3 end = Vector3.zero;
		for (int i = 0; i < array.Length; i++) {
			end += array[i].point;
		}
		return end / array.Length;
}

void HandleCollision ( Collision collider){
	if(RESET){
	for (int c = 0; c < Collisions.Length; c++) {
	Collisions[c].Normal = Vector3.zero;
	Collisions[c].Point = Vector3.zero;
	Collisions[c].Count = 0;
	Collisions[c].Active = false;
	for (int b2 = 0; b2 < Collisions[c].Ranges.Length; b2++) {
	Collisions[c].Ranges[b2].Active = false;
	}
	}
	RESET = false;
	}
	ContactPoint[] points = collider.contacts;
	
Vector3 poooooint = AveragePoint(points);
Vector3 nom = AverageNormal(points);

	for (int c2 = 0; c2 < Collisions.Length; c2++) {
	if(Collisions[c2].IgnoreMask == (Collisions[c2].IgnoreMask | (1 << collider.gameObject.layer))){continue;}
	if(IgnoreCollision == (IgnoreCollision | (1 << collider.gameObject.layer))){continue;}
	bool  localBool = (Collisions[c2].localTransform != null);
	Vector3 pointpoint = (localBool ? Collisions[c2].localTransform.InverseTransformDirection(nom) : nom);

	for (int b = 0; b < Collisions[c2].Ranges.Length; b++) {
	if(Collisions[c2].Ranges[b].x){norm = pointpoint.x;}
	if(Collisions[c2].Ranges[b].y){norm = pointpoint.y;}
	if(Collisions[c2].Ranges[b].z){norm = pointpoint.z;}
	if((Collisions[c2].Ranges[b].greater && norm < AngleToNormal(Collisions[c2].Ranges[b].value)) ||
	(Collisions[c2].Ranges[b].less && norm > AngleToNormal(Collisions[c2].Ranges[b].value)) ||
	(Collisions[c2].Ranges[b].equals && RoundToPower(norm, 3) == RoundToPower(AngleToNormal(Collisions[c2].Ranges[b].value), 3))){Collisions[c2].Ranges[b].Active = true; Collisions[c2].Normal += nom; Collisions[c2].LastNormal = nom; Collisions[c2].Point += poooooint; Collisions[c2].LastPoint = poooooint; Collisions[c2].Count++; Collisions[c2].collision = collider; if(Collisions[c2].tag == "" || !collider.transform.CompareTag(Collisions[c2].tag)){Collisions[c2].tag = collider.transform.tag;}}
	}
}
for (int c3 = 0; c3 < Collisions.Length; c3++) {
bool BOOL = true;
for (int b3 = 0; b3 < Collisions[c3].Ranges.Length; b3++) {
BOOL = (BOOL && Collisions[c3].Ranges[b3].Active);
}

Collisions[c3].Active = BOOL;
}

}

/*
void ApplyMovement ( Vector3 axis ,   float value ,   bool clampMin ,    bool clampMax ,    bool forceMin ,    bool forceMax ,    bool smoothForce ,    float min ,   float max ,   bool add ,    bool equals  ){
 if(axis.x != 0){
PHYSIXBUFFER.x.active = true;
PHYSIXBUFFER.x.value = value;
PHYSIXBUFFER.x.clampMin = clampMin;
PHYSIXBUFFER.x.clampMax = clampMax;
PHYSIXBUFFER.x.forceMin = forceMin;
PHYSIXBUFFER.x.forceMax = forceMax;
PHYSIXBUFFER.x.smoothForce = smoothForce;
PHYSIXBUFFER.x.min = min;
PHYSIXBUFFER.x.max = max;
PHYSIXBUFFER.x.add = add;
PHYSIXBUFFER.x.equals = equals;
}
if(axis.y != 0){
PHYSIXBUFFER.y.active = true;
PHYSIXBUFFER.y.value = value;
PHYSIXBUFFER.y.clampMin = clampMin;
PHYSIXBUFFER.y.clampMax = clampMax;
PHYSIXBUFFER.y.forceMin = forceMin;
PHYSIXBUFFER.y.forceMax = forceMax;
PHYSIXBUFFER.y.smoothForce = smoothForce;
PHYSIXBUFFER.y.min = min;
PHYSIXBUFFER.y.max = max;
PHYSIXBUFFER.y.add = add;
PHYSIXBUFFER.y.equals = equals;
}
if(axis.z != 0){
PHYSIXBUFFER.z.active = true;
PHYSIXBUFFER.z.value = value;
PHYSIXBUFFER.z.clampMin = clampMin;
PHYSIXBUFFER.z.clampMax = clampMax;
PHYSIXBUFFER.z.forceMin = forceMin;
PHYSIXBUFFER.z.forceMax = forceMax;
PHYSIXBUFFER.z.smoothForce = smoothForce;
PHYSIXBUFFER.z.min = min;
PHYSIXBUFFER.z.max = max;
PHYSIXBUFFER.z.add = add;
PHYSIXBUFFER.z.equals = equals;
}
ApplyMovement(PHYSIXBUFFER);
}

public void Snap () {
RaycastHit hit;
for (int i = 0; i < Collisions.Length; i++) {
if(!Collisions[i].Snap || !Collisions[i].Active){continue;}
Vector3 Normal = (Collisions[i].Count == 0 ? Vector3.zero : -(Collisions[i].Normal / Collisions[i].Count).normalized);
Vector3 ColliderPoint = Collisions[i].collision.collider.ClosestPoint(transform.position);
if (Physics.Raycast(new Ray(transform.position, ((ColliderPoint - transform.position).normalized + Normal) * 0.5f), out hit)) {
Vector3 rigidnormal = (hit.normal - Normal) * 0.5f;
float maxangle = Mathf.Abs(180 - Vector3.Angle(Normal, hit.normal));
float magnitude = Vector3.Project(rigidbody.velocity, -Normal).magnitude;
if(hit.distance > rigidbody.velocity.magnitude * Collisions[i].SnapOffset || magnitude >= Collisions[i].SnapBreakVelocity || maxangle > Collisions[i].SnapAngle || !NormalCollides(i, hit.normal)){continue;}
Vector3 tomove = ColliderPoint - GetClosestColliderPoint(ColliderPoint);
transform.position += tomove;
				rigidbody.velocity = rigidbody.velocity.magnitude * (Vector3.ProjectOnPlane(rigidbody.velocity, -tomove.normalized)).normalized;
}
}
}

OldSnap
public void Snap () {
RaycastHit hit;
for (int i = 0; i < Collisions.Length; i++) {
if(!Collisions[i].Snap || !Collisions[i].Active || Collisions[i].SnapDelay >= Time.frameCount){continue;}
bool mesh = Collisions[i].collision != null && Collisions[i].collision.collider != null && Collisions[i].collision.collider.GetType() == typeof(MeshCollider);
if(mesh){
if(Collisions[i].SnapTransform != Collisions[i].collision.collider.transform){
Collisions[i].SnapTransform = Collisions[i].collision.collider.transform;
PhysixMeshSnapping pms = Collisions[i].SnapTransform.GetComponent<PhysixMeshSnapping>();

if(pms == null){
pms = Collisions[i].collision.collider.gameObject.AddComponent(typeof(PhysixMeshSnapping)) as PhysixMeshSnapping;
}

Collisions[i].SnapMeshScript = pms;
}
MeshCollider meshcollider = (MeshCollider)Collisions[i].collision.collider;
mesh = !meshcollider.convex;
}
Vector3 Normal = -(Collisions[i].LastNormal).normalized;
if(Collisions[i].collision.collider == null){continue;}
Vector3 ColliderPoint = (mesh ? GetClosestMeshPoint((MeshCollider)Collisions[i].collision.collider, transform.position, i) : Collisions[i].collision.collider.ClosestPoint(transform.position));

if (Physics.Raycast(new Ray(transform.position, ((ColliderPoint - transform.position).normalized + Normal) * 0.5f), out hit)) {

float maxangle = Mathf.Abs(Vector3.Angle(-Normal, hit.normal));
				float magnitude = Vector3.Project(rigidbody.velocity, -Normal).magnitude;
				//if(Vector3.Distance(ColliderPoint, GetClosestColliderPoint(ColliderPoint)) > Mathf.Clamp(rigidbody.velocity.magnitude, 1, 100000) * Collisions[i].SnapOffset){Debug.Log("OFFSET");}
				//if(magnitude >= Collisions[i].SnapBreakVelocity){Debug.Log(magnitude);}
				//if(maxangle > Collisions[i].SnapAngle){Debug.Log("ANGLE");}
				//if(!NormalCollides(i, hit.normal)){Debug.Log("CANNOT");}
bool mesh2 = hit.collider.GetType() == typeof(MeshCollider);
if(mesh2){
MeshCollider meshcollider = (MeshCollider)hit.collider;
mesh2 = !meshcollider.convex;
}
ColliderPoint = (mesh2 ? GetClosestMeshPoint((MeshCollider)hit.collider, transform.position, i) : hit.collider.ClosestPoint(transform.position));
if(Vector3.Distance(ColliderPoint, GetClosestColliderPoint(ColliderPoint)) > Mathf.Clamp(rigidbody.velocity.magnitude, 1, 100000) * Collisions[i].SnapOffset || (magnitude >= Collisions[i].SnapBreakVelocity && (!mesh2 || maxangle == 0)) || maxangle > Collisions[i].SnapAngle || !NormalCollides(i, hit.normal)){continue;}

Vector3 tomove = Vector3.ClampMagnitude(ColliderPoint - GetClosestColliderPoint(ColliderPoint), GetColliderSize());
Vector3 todirect = -(ColliderPoint - GetColliderCenter()).normalized;
transform.position += tomove;
float mag = rigidbody.velocity.magnitude;
Vector3 newvelo = Vector3.ProjectOnPlane(rigidbody.velocity, todirect).normalized;
rigidbody.velocity = mag * newvelo;
collisioncount++;
}
}
}

float LargestTriangleSize (Vector3 p1,Vector3 p2,Vector3 p3) {
		float l1 = Vector3.Distance(p1,p2);
		float l2 = Vector3.Distance(p2,p3);
		float l3 = Vector3.Distance(p1,p3);
		return (l1 > l2 && l1 > l3 ? l1 : (l2 > l1 && l2 > l3 ? l2 : l3));
}
float SmallestTriangleSize (Vector3 p1,Vector3 p2,Vector3 p3) {
		float l1 = Vector3.Distance(p1,p2);
		float l2 = Vector3.Distance(p2,p3);
		float l3 = Vector3.Distance(p1,p3);
		return (l1 < l2 && l1 < l3 ? l1 : (l2 < l1 && l2 < l3 ? l2 : l3));
}

Vector3 GetClosestMeshPoint (MeshCollider collider, Vector3 Point, int collideindex) {
float collidersize = GetColliderSize();
Vector3 point = collider.transform.InverseTransformPoint(Point);
if(collider.sharedMesh.vertexCount < 3){return Vector3.zero;}
Vector3[] verts = collider.sharedMesh.vertices;
int[] tris = (Collisions[collideindex].SnapMeshScript == null ? collider.sharedMesh.triangles : Collisions[collideindex].SnapMeshScript.GetClosestBox(point).triangles);
int index1 = 0;
int index2 = 0;
int index3 = 0;
float dist1 = 0;
bool first = true;

int length = tris.Length;
for (int i = 0; i < length; i += 3) {
if(verts.Length <= tris[i] || verts.Length <= tris[i+1] || verts.Length <= tris[i+2]){continue;}
Vector3 POINT = (verts[tris[i]]+verts[tris[i+1]]+verts[tris[i+2]]) / 3f;
if(Vector3.Distance(point, POINT) > LargestTriangleSize(verts[tris[i]],verts[tris[i+1]],verts[tris[i+2]])){continue;}

Vector3 NORMAL = Vector3.Cross((verts[tris[i+1]]-verts[tris[i+0]]).normalized, (verts[tris[i+2]]-verts[tris[i+0]]).normalized);
double dot = Vector3.Dot(point - POINT,NORMAL);
if(dot <= 0){continue;}

Vector3 tripoint = ClosestPointOnTriangle(verts[tris[i]],verts[tris[i+1]],verts[tris[i+2]],point);
float DIST = Vector3.Distance(point, tripoint);
if(DIST == dist1){
Vector3 NORM = collider.transform.TransformDirection((point - tripoint).normalized);
Vector3 direct = Vector3.ProjectOnPlane(NORM, (point - tripoint).normalized) * collidersize;

Vector3 normcurrent = collider.transform.TransformDirection(Vector3.Cross((verts[index2]-verts[index1]).normalized, (verts[index3]-verts[index1]).normalized));
Vector3 normnew = collider.transform.TransformDirection(Vector3.Cross((verts[tris[i+1]]-verts[tris[i]]).normalized, (verts[tris[i+2]]-verts[tris[i]]).normalized));
float AngleCurrent = Vector3.Angle(NORM, normcurrent);
float AngleNew = Vector3.Angle(NORM, normnew);
if(AngleNew < AngleCurrent){first = true;}
}
if(DIST < dist1 || first){
index1 = tris[i];
index2 = tris[i+1];
index3 = tris[i+2];
dist1 = DIST;
first = false;
continue;
}
}
Vector3 p1 = collider.transform.TransformPoint(verts[index1]);
Vector3 p2 = collider.transform.TransformPoint(verts[index2]);
Vector3 p3 = collider.transform.TransformPoint(verts[index3]);

Vector3 endPoint = ClosestPointOnTriangle(p1,p2,p3,Point);
return endPoint;
}
*/
}