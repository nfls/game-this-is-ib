using System;
using UnityEngine;

public class IBSpriteAttributesData : ItemAttributesData {

	public string name;
	public string description;
	public int cost;

	public IBSpriteBoolAttribute doesDamage;
	public IBSpriteFloatAttribute damage;
	public IBSpriteBoolAttribute doesHit;
	public IBSpriteFloatAttribute velocityX;
	public IBSpriteFloatAttribute velocityY;
	public IBSpriteBoolAttribute doesStun;
	public IBSpriteFloatAttribute stunnedTime;

	[Serializable]
	public class IBSpriteFloatAttribute {
		
		public string name;
		public string description;
		public FloatAttribute[] attributes;
		
		[Serializable]
		public class FloatAttribute {

			public string name;
			public float value;
			public int cost;
		}
	}

	[Serializable]
	public class IBSpriteBoolAttribute {
		
		public string name;
		public string description;
		public BoolAttribute[] attributes;
		
		[Serializable]
		public class BoolAttribute {

			public string name;
			public bool value;
			public int cost;
		}
	}
}