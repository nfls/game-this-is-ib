#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum CollisionXYZEnum
{
        X = 0, // Custom name for "Nothing" option
        Y = 1,
        Z = 2,
}

public enum CollisionLogicEnum
{
		Everything = ~0,
        Less = 1 << 0, // Custom name for "Nothing" option
        Greater = 1 << 1,
        Equal = 1 << 2,
}

public enum MovementTypeInspector
{
        Add = 0, // Custom name for "Nothing" option
        Equals = 1,
        MoveTowards = 2,
}
public enum MovementTypeClamp
{
        NoClamp = 0, // Custom name for "Nothing" option
        SimpleClamp = 1,
        ForcefulClamp = 2,
}

public class PhysixInspector : MonoBehaviour {
	[CustomEditor(typeof(Physix))]
	public class PhysixEditor : Editor 
	{
		int deleteCollision = -1;
		int duplicateCollision = -1;
		int deleteMovement = -1;
		int duplicateMovement = -1;
		bool newThang = false;

void ResizeArray <T> (ref T[] array, int size) {
T[] Temp = array;
int oldlength = array.Length;
array = new T[size];
for (int i = 0; i < oldlength; i++) {
if(Temp[i] == null || i >= array.Length){break;}
array[i] = Temp[i];
}
}

void RemoveAt <T> (ref T[] array, int index) {
for (int i = index + 1; i < array.Length; i++) {
array[i - 1] = array[i];
}
		ResizeArray(ref array, array.Length - 1);
}

void DuplicateAtM (ref Physix.PHYSIXMOVE[] array, int index) {
ResizeArray(ref array, array.Length+1);
array[array.Length - 1] = new Physix.PHYSIXMOVE();
for (int i = array.Length - 1; i > index; i--) {
array[i].Equals(array[i - 1]);
}
}

void DuplicateAtC (ref Physix.PHYSIXCOLLISION[] array, int index) {
ResizeArray(ref array, array.Length+1);
array[array.Length - 1] = new Physix.PHYSIXCOLLISION();
for (int i = array.Length - 1; i > index; i--) {
array[i].Equals(array[i - 1]);
ResizeArray(ref array[i].Ranges, array[i - 1].Ranges.Length);
for(int ii = 0; ii < array[i].Ranges.Length; ii++){array[i].Ranges[ii].Equals(array[i-1].Ranges[ii]);}
}
}

void displayMovementAxis(ref Physix.PHYSIXMOVETYPE axis) {
GUILayout.BeginHorizontal();
GUILayout.Space(25);
GUILayout.BeginVertical();

GUILayout.BeginHorizontal();axis.active = GUILayout.Toggle(axis.active, "Active", "Button", GUILayout.MaxWidth(75));EditorGUILayout.HelpBox("Will this axis apply movement?", MessageType.None);GUILayout.EndHorizontal();
MovementTypeInspector ENUM = MovementTypeInspector.Add;
bool switchtoadd = false;
if(axis.add){ENUM = MovementTypeInspector.Add;}
if(axis.equals){ENUM = MovementTypeInspector.Equals;}
if(axis.smoothForce){ENUM = MovementTypeInspector.MoveTowards;}
string typestring = (ENUM == MovementTypeInspector.Add ? "Adds value to velocity." : (ENUM == MovementTypeInspector.Equals ? "Sets velocity equal to value." : "Moves velocity towards target value."));
if(ENUM != MovementTypeInspector.Add){switchtoadd = true;}
GUILayout.BeginHorizontal();ENUM = (MovementTypeInspector)EditorGUILayout.EnumPopup(ENUM, GUILayout.MaxWidth(100));EditorGUILayout.HelpBox(typestring, MessageType.None);GUILayout.EndHorizontal();
if(ENUM == MovementTypeInspector.Add && switchtoadd){axis.clampMin = false; axis.forceMin = false;axis.clampMax = false; axis.forceMax = false;}
if(ENUM == MovementTypeInspector.Add){axis.add = true; axis.equals = false; axis.smoothForce = false;}
if(ENUM == MovementTypeInspector.Equals){axis.add = false; axis.equals = true; axis.smoothForce = false;}
if(ENUM == MovementTypeInspector.MoveTowards){axis.add = false; axis.equals = false; axis.smoothForce = true; axis.forceMax = true; axis.forceMin = true;}

if(!axis.smoothForce){axis.value = EditorGUILayout.DelayedFloatField("Value: ", axis.value);}
if(axis.add){
axis.DisplayMin = EditorGUILayout.Foldout(axis.DisplayMin, "Minimum", true);
if(axis.DisplayMin){
MovementTypeClamp minclamp = MovementTypeClamp.NoClamp;
if(!axis.clampMin && !axis.forceMin){minclamp = MovementTypeClamp.NoClamp;}
if(axis.clampMin && !axis.forceMin){minclamp = MovementTypeClamp.SimpleClamp;}
if(!axis.clampMin && axis.forceMin){minclamp = MovementTypeClamp.ForcefulClamp;}
if(axis.clampMin && axis.forceMin){minclamp = MovementTypeClamp.ForcefulClamp;}
GUILayout.BeginHorizontal();
GUILayout.Space(25);
GUILayout.BeginVertical();
minclamp = (MovementTypeClamp)EditorGUILayout.EnumPopup(minclamp, GUILayout.MaxWidth(100));
if(minclamp == MovementTypeClamp.NoClamp){axis.clampMin = false; axis.forceMin = false;}
if(minclamp == MovementTypeClamp.SimpleClamp){axis.clampMin = true; axis.forceMin = false;}
if(minclamp == MovementTypeClamp.ForcefulClamp){axis.clampMin = false; axis.forceMin = true;}
if(minclamp != MovementTypeClamp.NoClamp){axis.min = EditorGUILayout.DelayedFloatField("Minimum: ", axis.min);}
EditorGUILayout.HelpBox((minclamp == MovementTypeClamp.NoClamp ? "Velocity will not have a minimum value." : (minclamp == MovementTypeClamp.SimpleClamp ? "Velocity will not subtract lower than minimum." : "If below minimum, velocity will be set to minimum.")), MessageType.None);
GUILayout.EndVertical();
GUILayout.EndHorizontal();
}

axis.DisplayMax = EditorGUILayout.Foldout(axis.DisplayMax, "Maximum", true);
if(axis.DisplayMax){
MovementTypeClamp maxclamp = MovementTypeClamp.NoClamp;
if(!axis.clampMax && !axis.forceMax){maxclamp = MovementTypeClamp.NoClamp;}
if(axis.clampMax && !axis.forceMax){maxclamp = MovementTypeClamp.SimpleClamp;}
if(!axis.clampMax && axis.forceMax){maxclamp = MovementTypeClamp.ForcefulClamp;}
if(axis.clampMax && axis.forceMax){maxclamp = MovementTypeClamp.ForcefulClamp;}
GUILayout.BeginHorizontal();
GUILayout.Space(25);
GUILayout.BeginVertical();
maxclamp = (MovementTypeClamp)EditorGUILayout.EnumPopup(maxclamp, GUILayout.MaxWidth(100));
if(maxclamp == MovementTypeClamp.NoClamp){axis.clampMax = false; axis.forceMax = false;}
if(maxclamp == MovementTypeClamp.SimpleClamp){axis.clampMax = true; axis.forceMax = false;}
if(maxclamp == MovementTypeClamp.ForcefulClamp){axis.clampMax = false; axis.forceMax = true;}
if(maxclamp != MovementTypeClamp.NoClamp){axis.max = EditorGUILayout.DelayedFloatField("Maximum: ", axis.max);}
EditorGUILayout.HelpBox((maxclamp == MovementTypeClamp.NoClamp ? "Velocity will not have a maximum value." : (maxclamp == MovementTypeClamp.SimpleClamp ? "Velocity will not add greater than maximum." : "If above maximum, velocity will be set to maximum.")), MessageType.None);
GUILayout.EndVertical();
GUILayout.EndHorizontal();
}
}

if(axis.smoothForce){
axis.min = EditorGUILayout.DelayedFloatField("Target Value: ", axis.min);
axis.max = axis.min;
axis.value = EditorGUILayout.DelayedFloatField("Speed: ", axis.value);
}

GUILayout.EndVertical();
GUILayout.EndHorizontal();
}

static LayerMask LayerMaskField( string label, LayerMask layerMask) {
     List<string> layers = new List<string>();
     List<int> layerNumbers = new List<int>();
 
     for (int i = 0; i < 32; i++) {
         string layerName = LayerMask.LayerToName(i);
         if (layerName != "") {
             layers.Add(layerName);
             layerNumbers.Add(i);
         }
     }
     int maskWithoutEmpty = 0;
	 int count = 0;
     for (int i = 0; i < layerNumbers.Count; i++) {
         if (((1 << layerNumbers[i]) & layerMask.value) > 0){
             maskWithoutEmpty |= (1 << i);
				count++;}
     }
			if(count >= layers.Count){maskWithoutEmpty = ~0;}
     maskWithoutEmpty = EditorGUILayout.MaskField( label, maskWithoutEmpty, layers.ToArray());
     int mask = 0;
     for (int i = 0; i < layerNumbers.Count; i++) {
         if ((maskWithoutEmpty & (1 << i)) > 0)
             mask |= (1 << layerNumbers[i]);
     }
     layerMask.value = mask;
     return layerMask;
 }

		public override void OnInspectorGUI (){
			if(newThang){newThang = false;}
			Physix PHYSIX = (Physix)target;
			if(PHYSIX.Collisions == null){PHYSIX.Collisions = new Physix.PHYSIXCOLLISION[0];}
			if(PHYSIX.Movements == null){PHYSIX.Movements = new Physix.PHYSIXMOVE[0];}

			if(!PHYSIX.UseDefualtInspector){
			GUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("Collision", MessageType.None);
			PHYSIX.HideCollision = GUILayout.Toggle(PHYSIX.HideCollision, "Hide", "Button", GUILayout.MaxWidth(50));
			GUILayout.EndHorizontal();
			bool deleteOrDuplicate = false;
			if(!PHYSIX.HideCollision){
			GUILayout.BeginHorizontal();
			GUILayout.Space(25);
			GUILayout.BeginVertical();
			
			//DisplayCollisions
			for (int i = 0; i < PHYSIX.Collisions.Length; i++) {
			GUILayout.BeginHorizontal();
			PHYSIX.Collisions[i].Display = GUILayout.Toggle(PHYSIX.Collisions[i].Display, PHYSIX.Collisions[i].Name, "Button");
				if (GUILayout.Button("■|■", GUILayout.MaxWidth(35))) {deleteCollision = -1; duplicateCollision = i;PHYSIX.Collisions[i].Display = false;}
				if (GUILayout.Button("X", GUILayout.MaxWidth(25))) {deleteCollision = i; duplicateCollision = -1;PHYSIX.Collisions[i].Display = false;}
			GUILayout.EndHorizontal();
			if(deleteCollision == i || duplicateCollision == i){
			bool WC = (deleteCollision == i);
			GUILayout.BeginHorizontal();
			if(GUILayout.Button("yes", GUILayout.MaxWidth(50))) {if(WC){RemoveAt(ref PHYSIX.Collisions, deleteCollision);}if(!WC){DuplicateAtC(ref PHYSIX.Collisions, duplicateCollision);}deleteCollision = -1; duplicateCollision = -1;deleteOrDuplicate = true;}
			if(!deleteOrDuplicate){EditorGUILayout.HelpBox(""+(WC ? "Delete" :  "Duplicate")+" "+PHYSIX.Collisions[i].Name+"?", MessageType.None);}
			if(GUILayout.Button("no", GUILayout.MaxWidth(50))) {deleteCollision = -1; duplicateCollision = -1;}
			GUILayout.EndHorizontal();
			}
			if(deleteOrDuplicate){break;}
			if(PHYSIX.Collisions[i].Display){
				GUILayout.BeginHorizontal();
				GUILayout.Space(25);
				GUILayout.BeginVertical();
				PHYSIX.Collisions[i].Name = EditorGUILayout.DelayedTextField("Name: ", PHYSIX.Collisions[i].Name);
				EditorGUILayout.HelpBox("Ranges", MessageType.None);
				GUILayout.BeginHorizontal();
				GUILayout.Space(25);
				GUILayout.BeginVertical();
				for (int ii = 0; ii < PHYSIX.Collisions[i].Ranges.Length; ii++) {
						if(ii > 0){EditorGUILayout.HelpBox("-and-", MessageType.None);}
					GUILayout.BeginHorizontal();
					//EditorGUILayout.LabelField("Range #"+(i+1), GUILayout.MaxWidth(75));
					EditorGUILayout.HelpBox("Axis", MessageType.None, true);
					CollisionXYZEnum tempCollision = CollisionXYZEnum.X;
					if(PHYSIX.Collisions[i].Ranges[ii].x){tempCollision = CollisionXYZEnum.X;}
					if(PHYSIX.Collisions[i].Ranges[ii].y){tempCollision = CollisionXYZEnum.Y;}
					if(PHYSIX.Collisions[i].Ranges[ii].z){tempCollision = CollisionXYZEnum.Z;}
					tempCollision = (CollisionXYZEnum)EditorGUILayout.EnumPopup(tempCollision);
					PHYSIX.Collisions[i].Ranges[ii].x = (tempCollision == CollisionXYZEnum.X);
					PHYSIX.Collisions[i].Ranges[ii].y = (tempCollision == CollisionXYZEnum.Y);
					PHYSIX.Collisions[i].Ranges[ii].z = (tempCollision == CollisionXYZEnum.Z);
					EditorGUILayout.HelpBox("is", MessageType.None, true);
					CollisionLogicEnum tempCollisionLogic;
					tempCollisionLogic = (PHYSIX.Collisions[i].Ranges[ii].less ? CollisionLogicEnum.Less : 0) | 
									(PHYSIX.Collisions[i].Ranges[ii].greater ? CollisionLogicEnum.Greater : 0) | 
									(PHYSIX.Collisions[i].Ranges[ii].equals ? CollisionLogicEnum.Equal : 0);
					if(PHYSIX.Collisions[i].Ranges[ii].less && PHYSIX.Collisions[i].Ranges[ii].greater && PHYSIX.Collisions[i].Ranges[ii].equals){tempCollisionLogic = CollisionLogicEnum.Everything;}
					tempCollisionLogic = (CollisionLogicEnum)EditorGUILayout.EnumFlagsField(tempCollisionLogic);
					PHYSIX.Collisions[i].Ranges[ii].less = ((tempCollisionLogic & CollisionLogicEnum.Less) != 0);
					PHYSIX.Collisions[i].Ranges[ii].greater = ((tempCollisionLogic & CollisionLogicEnum.Greater) != 0);
					PHYSIX.Collisions[i].Ranges[ii].equals = ((tempCollisionLogic & CollisionLogicEnum.Equal) != 0);
					EditorGUILayout.HelpBox((PHYSIX.Collisions[i].Ranges[ii].equals ? "to" : "than"), MessageType.None, true);
						PHYSIX.Collisions[i].Ranges[ii].value = Mathf.Clamp(EditorGUILayout.DelayedFloatField(PHYSIX.Collisions[i].Ranges[ii].value), -90,90);
						if(GUILayout.Button("X", GUILayout.MaxWidth(25))){RemoveAt(ref PHYSIX.Collisions[i].Ranges, ii); ii--;}
					GUILayout.EndHorizontal();
					
				}
					if(GUILayout.Button("+", GUILayout.MaxWidth(25))){ResizeArray(ref PHYSIX.Collisions[i].Ranges, PHYSIX.Collisions[i].Ranges.Length + 1); newThang = true;}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
					PHYSIX.Collisions[i].localTransform = (Transform)EditorGUILayout.ObjectField("Local Transform", (Transform)PHYSIX.Collisions[i].localTransform, typeof(Transform), true);
					PHYSIX.Collisions[i].Snap = GUILayout.Toggle(PHYSIX.Collisions[i].Snap, new GUIContent("Snap", "Snaps Physix onto a surface if this collision throws true."));
							if(PHYSIX.Collisions[i].Snap){
								GUILayout.BeginHorizontal();
								GUILayout.Space(25);
								GUILayout.BeginVertical();
								PHYSIX.Collisions[i].SnapOffset = EditorGUILayout.FloatField(new GUIContent("Snap Offset: ", "Minimum distance ground has to be to snap onto it. Scales with speed."), PHYSIX.Collisions[i].SnapOffset);
								PHYSIX.Collisions[i].SnapBreakVelocity = EditorGUILayout.FloatField(new GUIContent("Snap Break Velocity", "Tangential force needed to Un-snap Physix from the surface."), PHYSIX.Collisions[i].SnapBreakVelocity);
								PHYSIX.Collisions[i].ApplyMovementBreakVelocity = EditorGUILayout.FloatField(new GUIContent("ApplyMovement() Break Velocity", "Tangential force needed to Un-snap Physix from the surface, if the force is applied through ApplyMovement()."), PHYSIX.Collisions[i].ApplyMovementBreakVelocity);
								PHYSIX.Collisions[i].SnapAngle = EditorGUILayout.FloatField(new GUIContent("Snap Angle", "Max surface angle change allowed."), PHYSIX.Collisions[i].SnapAngle);								GUILayout.EndVertical();
								GUILayout.EndHorizontal();
							}
					PHYSIX.Collisions[i].IgnoreMask = LayerMaskField("IgnoreLayers", PHYSIX.Collisions[i].IgnoreMask);
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}
			if(GUILayout.Button("-New Collision-", GUILayout.MaxWidth(100))){ResizeArray(ref PHYSIX.Collisions, PHYSIX.Collisions.Length + 1);newThang = true;}
			EditorGUILayout.HelpBox("Collision Options", MessageType.None);
			GUILayout.BeginHorizontal();
			GUILayout.Space(25);
			GUILayout.BeginVertical();

			PHYSIX.FixMovingPlatforms = GUILayout.Toggle(PHYSIX.FixMovingPlatforms, new GUIContent("Automatic Moving Platforms", "Physix will automatically move with any collider it is colliding with."));
			if(PHYSIX.FixMovingPlatforms){
				GUILayout.BeginHorizontal();
				GUILayout.Space(25);
				GUILayout.BeginVertical();
				PHYSIX.PlatformLayerMask = LayerMaskField("Ignore platforms", PHYSIX.PlatformLayerMask);
				PHYSIX.PlatformsRetainVelocity = GUILayout.Toggle(PHYSIX.PlatformsRetainVelocity, new GUIContent("Retain Platform Velocity", "When exiting a collider, retains any collider velocity."));
				if(PHYSIX.PlatformsRetainVelocity) {PHYSIX.PlatformsVelocityMultiplier = EditorGUILayout.FloatField(new GUIContent("Platform Velocity Multiplier: ", "Multiplier to decide how much velocity is retained after exiting a moving platform."), PHYSIX.PlatformsVelocityMultiplier);}
				GUILayout.EndVertical();
				GUILayout.EndHorizontal();
			}
			PHYSIX.IgnoreCollision = LayerMaskField("Ignore Layers", PHYSIX.IgnoreCollision);

			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			}

			//Display Movement//////////////////////////////////////////////////////////////////////////////////////////////
			GUILayout.BeginHorizontal();
			EditorGUILayout.HelpBox("Movement", MessageType.None);
			PHYSIX.HideMovement = GUILayout.Toggle(PHYSIX.HideMovement, "Hide", "Button", GUILayout.MaxWidth(50));
			GUILayout.EndHorizontal();
			if(!PHYSIX.HideMovement){
			GUILayout.BeginHorizontal();
			GUILayout.Space(25);
			GUILayout.BeginVertical();
			for (int i = 0; i < PHYSIX.Movements.Length; i++) {
				GUILayout.BeginHorizontal();
				PHYSIX.Movements[i].Display = GUILayout.Toggle(PHYSIX.Movements[i].Display, PHYSIX.Movements[i].Name, "Button");
				if (GUILayout.Button("■|■", GUILayout.MaxWidth(35))) {deleteMovement = -1; duplicateMovement = i;PHYSIX.Movements[i].Display = false;}
				if (GUILayout.Button("X", GUILayout.MaxWidth(25))) {deleteMovement = i; duplicateMovement = -1;PHYSIX.Movements[i].Display = false;}
				GUILayout.EndHorizontal();
				if(deleteMovement == i || duplicateMovement == i){
				bool WC = (deleteMovement == i);
				GUILayout.BeginHorizontal();
				if (GUILayout.Button("yes", GUILayout.MaxWidth(50))) {if(WC){RemoveAt(ref PHYSIX.Movements, deleteMovement);}if(!WC){DuplicateAtM(ref PHYSIX.Movements, duplicateMovement);}deleteMovement = -1; duplicateMovement = -1;deleteOrDuplicate = true;}
				if(!deleteOrDuplicate){EditorGUILayout.HelpBox(""+(WC ? "Delete" :  "Duplicate")+" "+PHYSIX.Movements[i].Name+"?", MessageType.None);}
				if (GUILayout.Button("no", GUILayout.MaxWidth(50))) {deleteMovement = -1; duplicateMovement = -1;}
				GUILayout.EndHorizontal();
				}
				if(deleteOrDuplicate){break;}

				if(PHYSIX.Movements[i].Display){
					GUILayout.BeginHorizontal();
					GUILayout.Space(25);
					GUILayout.BeginVertical();
					PHYSIX.Movements[i].Name = EditorGUILayout.DelayedTextField("Name: ", PHYSIX.Movements[i].Name);

					PHYSIX.Movements[i].x.Display = EditorGUILayout.Foldout(PHYSIX.Movements[i].x.Display, "X Axis", true);
					if(PHYSIX.Movements[i].x.Display){displayMovementAxis(ref PHYSIX.Movements[i].x);}
					PHYSIX.Movements[i].y.Display = EditorGUILayout.Foldout(PHYSIX.Movements[i].y.Display, "Y Axis", true);
					if(PHYSIX.Movements[i].y.Display){displayMovementAxis(ref PHYSIX.Movements[i].y);}
					PHYSIX.Movements[i].z.Display = EditorGUILayout.Foldout(PHYSIX.Movements[i].z.Display, "Z Axis", true);
					if(PHYSIX.Movements[i].z.Display){displayMovementAxis(ref PHYSIX.Movements[i].z);}

					PHYSIX.Movements[i].localTransform = (Transform)EditorGUILayout.ObjectField("Local Transform", (Transform)PHYSIX.Movements[i].localTransform, typeof(Transform), true);
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}
			if(GUILayout.Button("-New Movement-", GUILayout.MaxWidth(110))){ResizeArray(ref PHYSIX.Movements, PHYSIX.Movements.Length + 1);newThang = true;}
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			}
			}
			if(PHYSIX.UseDefualtInspector){
			//EditorGUILayout.HelpBox("Defualt Inspector", MessageType.None);
			DrawDefaultInspector();
			}
			//PHYSIX.PhysicsRotation = EditorGUILayout.Vector3Field(new GUIContent("Physics Rotation: ", "Rotates the direction in which Physix calculates data. Great for changing gravitational pull."), PHYSIX.PhysicsRotation);
			GUILayout.BeginHorizontal();GUILayout.Space(10);PHYSIX.DisplayVisualSettings = EditorGUILayout.Foldout(PHYSIX.DisplayVisualSettings, "Settings", true);GUILayout.EndHorizontal();
			if(PHYSIX.DisplayVisualSettings){
			GUILayout.BeginHorizontal();
			GUILayout.Space(30);
			GUILayout.BeginVertical();
				PHYSIX.HitboxScale = EditorGUILayout.DelayedFloatField("Hitbox Scale: ", PHYSIX.HitboxScale);
				PHYSIX.HitBoxColor = EditorGUILayout.ColorField("Hitbox Color: ", PHYSIX.HitBoxColor);
				PHYSIX.UseDefualtInspector = EditorGUILayout.Toggle("Use Default Inspector: ", PHYSIX.UseDefualtInspector);
				GUILayout.BeginHorizontal();
				EditorGUILayout.HelpBox("Physix Version 1.6", MessageType.None);
				EditorGUILayout.HelpBox("© Matt \"Melonhead\" Sellers 2018", MessageType.None);
				GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndHorizontal();
			}
			if(!newThang){SceneView.RepaintAll();}
		}
		float roundUp(float numToRound, float multiple)
		{
			if(multiple == 0){return 0;}
    		return ((numToRound + multiple - 1) / multiple) * multiple;
		}

		//SceneView
		void OnSceneGUI () {
		Physix PHYSIX = (Physix)target;
			if(PHYSIX.Collisions == null){PHYSIX.Collisions = new Physix.PHYSIXCOLLISION[0];}
			if(PHYSIX.Movements == null){PHYSIX.Movements = new Physix.PHYSIXMOVE[0];}
			if(!newThang && !PHYSIX.HideCollision){
			Event e = Event.current;
			float CCPH = (Camera.current != null ? Camera.current.pixelRect.height : Screen.height);
			float CCPHX = (Camera.current != null ? Camera.current.pixelRect.width : Screen.width);
			bool mouseoutside = e.mousePosition.y > CCPH || e.mousePosition.y < 0 || e.mousePosition.x > CCPHX || e.mousePosition.x < 0;
			if(e.button == 0 && e.isMouse && PHYSIX.SelectCollision != -1){
			int controlID = GUIUtility.GetControlID(FocusType.Passive);
			EventType eventType = e.GetTypeForControl(controlID);
        	if (eventType == EventType.MouseUp)
        	{
				PHYSIX.HOLD = false;
            	GUIUtility.hotControl = controlID;
        	}
        	else if (eventType == EventType.MouseDown)
        	{
				PHYSIX.HOLD = true;
            	GUIUtility.hotControl = 0;
        	}
			}
			if(!PHYSIX.HOLD){PHYSIX.SelectCollision = -1; PHYSIX.SelectRange = -1;}
			for (int i = 0; i < PHYSIX.Collisions.Length; i++) {
				if(PHYSIX.Collisions[i].Display){
					Transform trans = (PHYSIX.Collisions[i].localTransform != null ? PHYSIX.Collisions[i].localTransform : PHYSIX.transform);
					for (int ii = 0; ii < PHYSIX.Collisions[i].Ranges.Length; ii++) {
						float iiirot = 0.0f;
						Vector3 iiivect = new Vector3(0,0,0);
						Vector3 iiivect2 = new Vector3(0,0,0);
						Vector3 iiipos = new Vector3(0,0,0);
						Quaternion quateuler = Quaternion.Euler(trans.eulerAngles);
						if(Camera.current != null){
						iiipos = Camera.current.transform.position;
								iiivect = Camera.current.transform.eulerAngles;
								iiivect2 = (Quaternion.Inverse(trans.rotation) * Camera.current.transform.rotation).eulerAngles;
						if(PHYSIX.Collisions[i].Ranges[ii].x){iiirot = (trans.InverseTransformPoint(Camera.current.transform.position).z < 0 ? iiivect2.x : 180 - iiivect2.x);}
						if(PHYSIX.Collisions[i].Ranges[ii].y){iiirot = iiivect2.y;}
						if(PHYSIX.Collisions[i].Ranges[ii].z){iiirot = (trans.InverseTransformPoint(Camera.current.transform.position).x < 0 ? iiivect2.x : 180 - iiivect2.x);}
						}
						Quaternion rot = quateuler * Quaternion.Euler(new Vector3(0,0,PHYSIX.Collisions[i].Ranges[ii].value * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * (PHYSIX.Collisions[i].Ranges[ii].less ? -1 : 1)) + (PHYSIX.Collisions[i].Ranges[ii].less ? 180 : 0)) + (PHYSIX.Collisions[i].Ranges[ii].x ? new Vector3(0,0,-90) : new Vector3(0,0,0)) + new Vector3((PHYSIX.Collisions[i].Ranges[ii].x ? iiirot : 0),(PHYSIX.Collisions[i].Ranges[ii].y ? iiirot : 0),0) + (PHYSIX.Collisions[i].Ranges[ii].z ? new Vector3(iiirot,90,90) : new Vector3(0,0,0)));
						Quaternion rotTip = quateuler * Quaternion.Euler(new Vector3(0,0,90 * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * (PHYSIX.Collisions[i].Ranges[ii].less ? -1 : 1)) + (PHYSIX.Collisions[i].Ranges[ii].less ? 180 : 0)) + (PHYSIX.Collisions[i].Ranges[ii].x ? new Vector3(0,0,-90) : new Vector3(0,0,0)) + new Vector3((PHYSIX.Collisions[i].Ranges[ii].x ? iiirot : 0),(PHYSIX.Collisions[i].Ranges[ii].y ? iiirot : 0),0) + (PHYSIX.Collisions[i].Ranges[ii].z ? new Vector3(iiirot,90,90) : new Vector3(0,0,0)));
						Handles.color = new Color(PHYSIX.HitBoxColor.r,PHYSIX.HitBoxColor.g,PHYSIX.HitBoxColor.b,0.25f);
							float size = PHYSIX.HitboxScale * 0.5f * ((PHYSIX.transform.lossyScale.x + PHYSIX.transform.lossyScale.y + PHYSIX.transform.lossyScale.z) / 3f);
							float ArcLength = (PHYSIX.Collisions[i].Ranges[ii].less && PHYSIX.Collisions[i].Ranges[ii].greater ? 360 : (180 - (PHYSIX.Collisions[i].Ranges[ii].value * 2)*(PHYSIX.Collisions[i].Ranges[ii].less ? -1 : 1)) * (!PHYSIX.Collisions[i].Ranges[ii].less && !PHYSIX.Collisions[i].Ranges[ii].greater ? 0 : 1));
							Vector3 point1 = trans.position + (rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.right))*size;
							Vector3 point2 = trans.position + (rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * (Quaternion.Euler(0,0,(PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * ArcLength) * Vector3.right)))*size;
							Vector3 pointT = trans.position + (rotTip * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.right))*size;
						Handles.DrawSolidArc(trans.position,
							rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.forward),
							rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.right),
								ArcLength,
                			size);
						Handles.color = new Color(PHYSIX.HitBoxColor.r,PHYSIX.HitBoxColor.g,PHYSIX.HitBoxColor.b,1);
							if(!(PHYSIX.Collisions[i].Ranges[ii].less && PHYSIX.Collisions[i].Ranges[ii].greater)){
							if(PHYSIX.Collisions[i].Ranges[ii].equals){
							Handles.DrawLine(point1, trans.position);
							Handles.DrawLine(point2, trans.position);
							}
							if(!PHYSIX.Collisions[i].Ranges[ii].equals){
							Handles.DrawDottedLine(point1, trans.position, 10);
							Handles.DrawDottedLine(point2, trans.position, 10);
							}
							}
							float dist1 = Vector3.Distance(new Vector3(e.mousePosition.x,CCPH - e.mousePosition.y, 0), Vector3.Scale(Camera.current.WorldToScreenPoint(point1),new Vector3(1,1,0)));
							float dist2 = Vector3.Distance(new Vector3(e.mousePosition.x,CCPH - e.mousePosition.y, 0), Vector3.Scale(Camera.current.WorldToScreenPoint(point2),new Vector3(1,1,0)));
							float neededDist1 = Mathf.Clamp((size / Vector3.Distance(iiipos, point1)) * 100, 20, 1000);
							float neededDist2 = Mathf.Clamp((size / Vector3.Distance(iiipos, point2)) * 100, 20, 1000);
							if((dist1 < neededDist1 || dist2 < neededDist2) && !PHYSIX.HOLD){PHYSIX.SelectCollision = i; PHYSIX.SelectRange = ii;}
							Vector3 facenormal = Quaternion.Euler(iiivect) * Vector3.forward;
							if(PHYSIX.SelectCollision == i && PHYSIX.SelectRange == ii){
							if(!PHYSIX.HOLD){
							Handles.DrawWireDisc(point1, facenormal,size * 0.1f);
							Handles.DrawWireDisc(point2, facenormal,size * 0.1f);
							}
							if(PHYSIX.HOLD){
									if(mouseoutside){PHYSIX.HOLD = false;}
							Handles.color = new Color(PHYSIX.HitBoxColor.r,PHYSIX.HitBoxColor.g,PHYSIX.HitBoxColor.b,0.5f);
							Handles.DrawSolidDisc(point1, facenormal,size * 0.1f);
							Handles.DrawSolidDisc(point2, facenormal,size * 0.1f);
									Vector3 transScreen = Camera.current.WorldToScreenPoint(trans.position);
									float rotvalue = (PHYSIX.Collisions[i].Ranges[ii].y ? rotTip.eulerAngles.z - 90 : (PHYSIX.Collisions[i].Ranges[ii].z ? rotTip.eulerAngles.z - rotTip.eulerAngles.x : rotTip.eulerAngles.z - rotTip.eulerAngles.y + trans.eulerAngles.y));
									float valangle = (iiivect.z > 90 ? -1 : 1) * (PHYSIX.Collisions[i].Ranges[ii].x && iiivect2.y > 90 && iiivect2.y < 270 ? 1 : -1) * (!PHYSIX.Collisions[i].Ranges[ii].x ? -1 : 1) * (90 - Vector3.Angle(Quaternion.Inverse(Quaternion.Euler(0,0,rotvalue)) * (new Vector3(e.mousePosition.x,CCPH - e.mousePosition.y, 0) - Vector3.Scale(transScreen,new Vector3(1,1,0))),(PHYSIX.Collisions[i].Ranges[ii].y ? Vector3.up : -Vector3.right)));
									float rounded10 = PHYSIX.RoundToPower(valangle,-1);
									float diff10 = valangle - rounded10;
									float control10 = rounded10 + (diff10 > 0 ? (Mathf.Abs(diff10) > 2.5 ? 5 : 0) : (Mathf.Abs(diff10) > 2.5 ? -5 : 0));
									PHYSIX.Collisions[i].Ranges[ii].value = (e.control ? control10 : PHYSIX.RoundToPower(valangle,0));
									GUIStyle labelstyle = new GUIStyle();
									labelstyle.normal.textColor = new Color(PHYSIX.HitBoxColor.r,PHYSIX.HitBoxColor.g,PHYSIX.HitBoxColor.b,1);
									if(dist1 < dist2){Handles.Label(point1 + Quaternion.LookRotation(point1 - trans.position) * new Vector3 (0,0,size * 0.2f), ""+PHYSIX.Collisions[i].Ranges[ii].value+"º", labelstyle);}
									if(dist2 <= dist1){Handles.Label(point2 + Quaternion.LookRotation(point2 - trans.position) * new Vector3 (0,0,size * 0.2f), ""+PHYSIX.Collisions[i].Ranges[ii].value+"º", labelstyle);}

							}
							}
						Handles.color = new Color(PHYSIX.HitBoxColor.r,PHYSIX.HitBoxColor.g,PHYSIX.HitBoxColor.b,1);
						Handles.DrawWireArc(trans.position,
							rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.forward),
							rot * ((PHYSIX.Collisions[i].Ranges[ii].z ? -1 : 1) * Vector3.right),
							ArcLength,
							size);
					}
				}
			}
}			
			if(PHYSIX.SelectCollision != -1){SceneView.RepaintAll();}
   		}
	}
}

#endif
