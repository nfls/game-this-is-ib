using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerData {

	public string name;
	public float health;
	public int maxStaminaIndex;
	public int staminaRecoveryDelayIndex;
	public int staminaRecoveryRateIndex;
	public int speedIndex;
	public int accelerationIndex;
	public int jumpPowerIndex;
	public int jumpTimesIndex;
	public int jumpPowerDecayIndex;
	public int dodgeSpeedIndex;
	public int dodgeDurationIndex;
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
	public Color accelerationTrailColor;
	public Gradient bloodColor;

	public PlayerData() { }

	public PlayerData(PlayerDataProxy data) {
		name = data.name;
		health = data.health;
		maxStaminaIndex = data.maxStaminaIndex;
		staminaRecoveryDelayIndex = data.staminaRecoveryDelayIndex;
		staminaRecoveryRateIndex = data.staminaRecoveryRateIndex;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeSpeedIndex = data.dodgeSpeedIndex;
		dodgeDurationIndex = data.dodgeDurationIndex;
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
		accelerationTrailColor = data.accelerationTrailColor;
		bloodColor = data.bloodColor;
	}

	public virtual void Cache(PlayerData data) {
		name = data.name;
		health = data.health;
		maxStaminaIndex = data.maxStaminaIndex;
		staminaRecoveryDelayIndex = data.staminaRecoveryDelayIndex;
		staminaRecoveryRateIndex = data.staminaRecoveryRateIndex;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeSpeedIndex = data.dodgeSpeedIndex;
		dodgeDurationIndex = data.dodgeDurationIndex;
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
		accelerationTrailColor = data.accelerationTrailColor;
		bloodColor = data.bloodColor;
	}
}

public class LocalPlayerData : PlayerData {
	
	public bool hasFinishedStarterLevel;
	public int numBronzeMeritCards;
	public int numSilverMeritCards;
	public int numGoldMeritCards;
	public List<string> items;
	public List<string> unlockedItems;

	public LocalPlayerData() { }

	public LocalPlayerData(LocalPlayerDataProxy data) : base(data) {
		hasFinishedStarterLevel = data.hasFinishedStarterLevel;
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
			hasFinishedStarterLevel = d.hasFinishedStarterLevel;
			numBronzeMeritCards = d.numBronzeMeritCards;
			numSilverMeritCards = d.numSilverMeritCards;
			numGoldMeritCards = d.numGoldMeritCards;
			items = new List<string>(d.items);
			unlockedItems = new List<string>(d.unlockedItems);
		}
	}
}

public class PlayerDataProxy {

	public string name;
	public float health;
	public int maxStaminaIndex;
	public int staminaRecoveryDelayIndex;
	public int staminaRecoveryRateIndex;
	public int speedIndex;
	public int accelerationIndex;
	public int jumpPowerIndex;
	public int jumpTimesIndex;
	public int jumpPowerDecayIndex;
	public int dodgeSpeedIndex;
	public int dodgeDurationIndex;
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
	public Gradient bloodColor;

	public PlayerDataProxy() { }

	public PlayerDataProxy(PlayerData data) {
		name = data.name;
		health = data.health;
		maxStaminaIndex = data.maxStaminaIndex;
		staminaRecoveryDelayIndex = data.staminaRecoveryDelayIndex;
		staminaRecoveryRateIndex = data.staminaRecoveryRateIndex;
		speedIndex = data.speedIndex;
		accelerationIndex = data.accelerationIndex;
		jumpPowerIndex = data.jumpPowerIndex;
		jumpTimesIndex = data.jumpTimesIndex;
		jumpPowerDecayIndex = data.jumpPowerDecayIndex;
		dodgeSpeedIndex = data.dodgeSpeedIndex;
		dodgeDurationIndex = data.dodgeDurationIndex;
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
		accelerationTrailColor = data.accelerationTrailColor;
		bloodColor = data.bloodColor;
	}
}

public class LocalPlayerDataProxy : PlayerDataProxy {
	
	public bool hasFinishedStarterLevel;
	public int numBronzeMeritCards;
	public int numSilverMeritCards;
	public int numGoldMeritCards;
	public List<string> items;
	public List<string> unlockedItems;
	
	public LocalPlayerDataProxy() { }

	public LocalPlayerDataProxy(LocalPlayerData data) : base(data) {
		hasFinishedStarterLevel = data.hasFinishedStarterLevel;
		numBronzeMeritCards = data.numBronzeMeritCards;
		numSilverMeritCards = data.numSilverMeritCards;
		numGoldMeritCards = data.numGoldMeritCards;
		items = new List<string>(data.items);
		unlockedItems = new List<string>(data.unlockedItems);
	}
}