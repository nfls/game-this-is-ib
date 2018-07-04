using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerData {

	public string name;
	public float health;
	public int speedIndex;
	public int accelerationIndex;
	public int jumpPowerIndex;
	public int jumpTimesIndex;
	public int jumpPowerDecayIndex;
	public int dodgeDistanceIndex;
	public int dodgeInvincibilityTimeIndex;
	public int dodgeCapacityIndex;
	public int dodgeCooldownIndex;
	public int ibSpriteCapacityIndex;
	public List<string> carriedIBSprites;
	public Dictionary<string, int> ibSprites;
	public string bodyStyle;
	public Color bodyOutlineColor;
	public Color bodyColor;
	public string eyeStyle;
	public Color eyeOutlineColor;
	public Color eyeColor;
	public Color dodgeTrailColor;
	public Color dodgeTrailOutlineColor;
	public Color accelerationTrailColor;
	public Color accelerationTrailOutlineColor;

	public PlayerData() { }

	public PlayerData(PlayerDataProxy data) {
		name = data.name;
		health = data.health;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeDistanceIndex = data.dodgeDistanceIndex;
		dodgeInvincibilityTimeIndex = data.dodgeInvincibilityTimeIndex;
		dodgeCapacityIndex = data.dodgeCapacityIndex;
		dodgeCooldownIndex = data.dodgeCooldownIndex;
		ibSpriteCapacityIndex = data.ibSpriteCapacityIndex;
		carriedIBSprites = new List<string>(data.carriedIBSprites);
		ibSprites = new Dictionary<string, int>(data.ibSprites);
		bodyStyle = data.bodyStyle;
		bodyOutlineColor = new Color(data.bodyOutlineColor[0], data.bodyOutlineColor[1], data.bodyOutlineColor[2], data.bodyOutlineColor[3]);
		bodyColor = new Color(data.bodyColor[0], data.bodyColor[1], data.bodyColor[2], data.bodyColor[3]);
		eyeStyle = data.eyeStyle;
		eyeOutlineColor = new Color(data.eyeOutlineColor[0], data.eyeOutlineColor[1], data.eyeOutlineColor[2], data.eyeOutlineColor[3]);
		eyeColor = new Color(data.eyeColor[0], data.eyeColor[1], data.eyeColor[2], data.eyeColor[3]);
		dodgeTrailColor = new Color(data.dodgeTrailColor[0], data.dodgeTrailColor[1], data.dodgeTrailColor[2], data.dodgeTrailColor[3]);
		dodgeTrailOutlineColor = new Color(data.dodgeTrailOutlineColor[0], data.dodgeTrailOutlineColor[1], data.dodgeTrailOutlineColor[2], data.dodgeTrailOutlineColor[3]);
		accelerationTrailColor = new Color(data.accelerationTrailColor[0], data.accelerationTrailColor[1], data.accelerationTrailColor[2], data.accelerationTrailColor[3]);
		accelerationTrailOutlineColor = new Color(data.accelerationTrailOutlineColor[0], data.accelerationTrailOutlineColor[1], data.accelerationTrailOutlineColor[2], data.accelerationTrailOutlineColor[3]);
	}

	public virtual void Cache(PlayerData data) {
		name = data.name;
		health = data.health;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeDistanceIndex = data.dodgeDistanceIndex;
		dodgeInvincibilityTimeIndex = data.dodgeInvincibilityTimeIndex;
		dodgeCapacityIndex = data.dodgeCapacityIndex;
		dodgeCooldownIndex = data.dodgeCooldownIndex;
		ibSpriteCapacityIndex = data.ibSpriteCapacityIndex;
		carriedIBSprites = new List<string>(data.carriedIBSprites);
		ibSprites = new Dictionary<string, int>(data.ibSprites);
		bodyStyle = data.bodyStyle;
		bodyOutlineColor = data.bodyOutlineColor;
		bodyColor = data.bodyColor;
		eyeStyle = data.eyeStyle;
		eyeOutlineColor = data.eyeOutlineColor;
		eyeColor = data.eyeColor;
		dodgeTrailColor = data.dodgeTrailColor;
		dodgeTrailOutlineColor = data.dodgeTrailOutlineColor;
		accelerationTrailColor = data.accelerationTrailColor;
		accelerationTrailOutlineColor = data.accelerationTrailOutlineColor;
	}
}

public class LocalPlayerData : PlayerData {
	
	public int numBronzeMeritCards;
	public int numSilverMeritCards;
	public int numGoldMeritCards;
	public List<string> items;
	public List<string> unlockedItems;

	public LocalPlayerData() { }

	public LocalPlayerData(LocalPlayerDataProxy data) : base(data) {
		numBronzeMeritCards = data.numBronzeMeritCards;
		numSilverMeritCards = data.numSilverMeritCards;
		numGoldMeritCards = data.numGoldMeritCards;
		items = new List<string>(data.items);
		unlockedItems = new List<string>(data.unlockedItems);
	}

	public override void Cache(PlayerData data) {
		base.Cache(data);
		var d = data as LocalPlayerData;
		if (d != null) {
			numBronzeMeritCards = d.numBronzeMeritCards;
			numSilverMeritCards = d.numSilverMeritCards;
			numGoldMeritCards = d.numGoldMeritCards;
			items = new List<string>(d.items);
			unlockedItems = new List<string>(d.unlockedItems);
		}
	}
}

public class RemotePlayerData : PlayerData {
	
	public int reward;
	
	public RemotePlayerData() { }

	public RemotePlayerData(RemotePlayerDataProxy data) : base(data) {
		reward = data.reward;
	}
	
	public override void Cache(PlayerData data) {
		base.Cache(data);
		var d = data as RemotePlayerData;
		if (d != null) {
			reward = d.reward;
		}
	}
}

public class PlayerDataProxy {
	
	public string name;
	public float health;
	public int speedIndex;
	public int accelerationIndex;
	public int jumpPowerIndex;
	public int jumpTimesIndex;
	public int jumpPowerDecayIndex;
	public int dodgeDistanceIndex;
	public int dodgeInvincibilityTimeIndex;
	public int dodgeCapacityIndex;
	public int dodgeCooldownIndex;
	public int ibSpriteCapacityIndex;
	public List<string> carriedIBSprites;
	public Dictionary<string, int> ibSprites;
	public string bodyStyle;
	public float[] bodyOutlineColor;
	public float[] bodyColor;
	public string eyeStyle;
	public float[] eyeOutlineColor;
	public float[] eyeColor;
	public float[] dodgeTrailColor;
	public float[] dodgeTrailOutlineColor;
	public float[] accelerationTrailColor;
	public float[] accelerationTrailOutlineColor;

	public PlayerDataProxy() { }

	public PlayerDataProxy(PlayerData data) {
		name = data.name;
		health = data.health;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeDistanceIndex = data.dodgeDistanceIndex;
		dodgeInvincibilityTimeIndex = data.dodgeInvincibilityTimeIndex;
		dodgeCapacityIndex = data.dodgeCapacityIndex;
		dodgeCooldownIndex = data.dodgeCooldownIndex;
		ibSpriteCapacityIndex = data.ibSpriteCapacityIndex;
		carriedIBSprites = new List<string>(data.carriedIBSprites);
		ibSprites = new Dictionary<string, int>(data.ibSprites);
		bodyStyle = data.bodyStyle;
		bodyOutlineColor = new [] { data.bodyOutlineColor.r, data.bodyOutlineColor.g, data.bodyOutlineColor.b, data.bodyOutlineColor.a };
		bodyColor = new [] { data.bodyColor.r, data.bodyColor.g, data.bodyColor.b, data.bodyColor.a };
		eyeStyle = data.eyeStyle;
		eyeOutlineColor = new [] { data.eyeOutlineColor.r, data.eyeOutlineColor.g, data.eyeOutlineColor.b, data.eyeOutlineColor.a };
		eyeColor = new [] { data.eyeColor.r, data.eyeColor.g, data.eyeColor.b, data.eyeColor.a };
		dodgeTrailColor = new[] { data.dodgeTrailColor.r, data.dodgeTrailColor.g, data.dodgeTrailColor.b, data.dodgeTrailColor.a };
		dodgeTrailOutlineColor = new[] { data.dodgeTrailOutlineColor.r, data.dodgeTrailOutlineColor.g, data.dodgeTrailOutlineColor.b, data.dodgeTrailOutlineColor.a };
		accelerationTrailColor = new[] { data.accelerationTrailColor.r, data.accelerationTrailColor.g, data.accelerationTrailColor.b, data.accelerationTrailColor.a };
		accelerationTrailOutlineColor = new[] { data.accelerationTrailOutlineColor.r, data.accelerationTrailOutlineColor.g, data.accelerationTrailOutlineColor.b, data.accelerationTrailOutlineColor.a };
	}
}

public class LocalPlayerDataProxy : PlayerDataProxy {
	
	public int numBronzeMeritCards;
	public int numSilverMeritCards;
	public int numGoldMeritCards;
	public List<string> items;
	public List<string> unlockedItems;
	
	public LocalPlayerDataProxy() { }

	public LocalPlayerDataProxy(LocalPlayerData data) : base(data) {
		numBronzeMeritCards = data.numBronzeMeritCards;
		numSilverMeritCards = data.numSilverMeritCards;
		numGoldMeritCards = data.numGoldMeritCards;
		items = new List<string>(data.items);
		unlockedItems = new List<string>(data.unlockedItems);
	}
}

public class RemotePlayerDataProxy : PlayerDataProxy {

	public int reward;
	
	public RemotePlayerDataProxy() { }

	public RemotePlayerDataProxy(RemotePlayerData data) : base(data) {
		reward = data.reward;
	}
}