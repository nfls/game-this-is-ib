using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAttributesData", menuName = "This-Is-IB/Player Attributes Data")]
public class PlayerAttributesData : ScriptableObject {

	public string name;
	public float health;
	public TrailSettings dodgeTrailSettings;
	public TrailSettings accelerationTrailSettings;
	public PlayerFloatAttribute maxStamina;
	public PlayerFloatAttribute staminaRecoveryRate;
	public PlayerFloatAttribute speed;
	public PlayerFloatAttribute acceleration;
	public PlayerFloatAttribute jumpPower;
	public PlayerIntAttribute jumpTimes;
	public PlayerFloatAttribute jumpPowerDecay;
	public PlayerFloatAttribute dodgeDistance;
	public PlayerFloatAttribute dodgeInvincibilityTime;
	public PlayerIntAttribute dodgeCapacity;
	public PlayerFloatAttribute dodgeCooldown;
	public PlayerIntAttribute ibSpriteCapacity;

	[Serializable]
	public class PlayerIntAttribute {
		
		public string name;
		public string description;
		public IntAttribute[] attributes;
		
		[Serializable]
		public class IntAttribute {
			
			public string name;
			public int value;
			public int cost;
		}

		public IntAttribute this[int i] {
			get { return attributes[i]; }
		}
	}
	
	[Serializable]
	public class PlayerFloatAttribute {
		
		public string name;
		public string description;
		public FloatAttribute[] attributes;
		
		[Serializable]
		public class FloatAttribute {
		
			public string name;
			public float value;
			public int cost;
		}
		
		public FloatAttribute this[int i] {
			get { return attributes[i]; }
		}
	}
}