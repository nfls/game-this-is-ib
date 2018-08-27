using UnityEngine;

public static class PhysicsVisualizer {
	
	public static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color, float duration = 10f) {
		DrawBox(new Box(origin, halfExtents, orientation), color, duration);
	}
	
	public static void DrawBox(Box box, Color color, float duration) {
		Debug.DrawLine(box.frontTopLeft, box.frontTopRight, color, duration);
		Debug.DrawLine(box.frontTopRight, box.frontBottomRight, color, duration);
		Debug.DrawLine(box.frontBottomRight, box.frontBottomLeft, color, duration);
		Debug.DrawLine(box.frontBottomLeft, box.frontTopLeft, color, duration);
		
		Debug.DrawLine(box.backTopLeft, box.backTopRight, color, duration);
		Debug.DrawLine(box.backTopRight, box.backBottomRight, color, duration);
		Debug.DrawLine(box.backBottomRight, box.backBottomLeft, color, duration);
		Debug.DrawLine(box.backBottomLeft, box.backTopLeft, color, duration);
		
		Debug.DrawLine(box.frontTopLeft, box.backTopLeft, color, duration);
		Debug.DrawLine(box.frontTopRight, box.backTopRight, color, duration);
		Debug.DrawLine(box.frontBottomRight, box.backBottomRight, color, duration);
		Debug.DrawLine(box.frontBottomLeft, box.backBottomLeft, color, duration);
	}
	
	public struct Box {
		public Vector3 localFrontTopLeft { get; private set; }
		public Vector3 localFrontTopRight { get; private set; }
		public Vector3 localFrontBottomLeft { get; private set; }
		public Vector3 localFrontBottomRight { get; private set; }

		public Vector3 localBackTopLeft => -localFrontBottomRight;

		public Vector3 localBackTopRight => -localFrontBottomLeft;

		public Vector3 localBackBottomLeft => -localFrontTopRight;

		public Vector3 localBackBottomRight => -localFrontTopLeft;

		public Vector3 frontTopLeft => localFrontTopLeft + origin;

		public Vector3 frontTopRight => localFrontTopRight + origin;

		public Vector3 frontBottomLeft => localFrontBottomLeft + origin;

		public Vector3 frontBottomRight => localFrontBottomRight + origin;

		public Vector3 backTopLeft => localBackTopLeft + origin;

		public Vector3 backTopRight => localBackTopRight + origin;

		public Vector3 backBottomLeft => localBackBottomLeft + origin;

		public Vector3 backBottomRight => localBackBottomRight + origin;

		public Vector3 origin { get; private set; }

		public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents) {
			Rotate(orientation);
		}

		public Box(Vector3 origin, Vector3 halfExtents) {
			localFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
			localFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
			localFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
			localFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);

			this.origin = origin;
		}


		public void Rotate(Quaternion orientation) {
			localFrontTopLeft = RotatePointAroundPivot(localFrontTopLeft, Vector3.zero, orientation);
			localFrontTopRight = RotatePointAroundPivot(localFrontTopRight, Vector3.zero, orientation);
			localFrontBottomLeft = RotatePointAroundPivot(localFrontBottomLeft, Vector3.zero, orientation);
			localFrontBottomRight = RotatePointAroundPivot(localFrontBottomRight, Vector3.zero, orientation);
		}
	}

	static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation) {
		Vector3 direction = point - pivot;
		return pivot + rotation * direction;
	}
}