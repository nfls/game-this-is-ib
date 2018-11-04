using UnityEngine;

public static class CharacterFactory {

	public static GameObject GenerateLocalPlayer() {
		GameObject player = GeneratePlayer(PlayerManager.LocalPlayerData);
		player.name = "Local Player";
		TagManager.SetAllTags(player, TagManager.LOCAL_PLAYER_TAG);
		return player;
	}

	public static GameObject GenerateRemotePlayer() {
		GameObject player = GeneratePlayer(PlayerManager.RemotePlayerData);
		player.name = "Remote Player";
		TagManager.SetAllTags(player, TagManager.REMOTE_PLAYER_TAG);
		return player;
	}
	
	private static GameObject GeneratePlayer(PlayerData data) {
		GameObject go = new GameObject();
		
		GameObject body = ResourcesManager.GetBody(data.bodyStyle);
		
		Renderer[] bodyRenderers = body.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in bodyRenderers) {
			renderer.material.SetColor(ShaderManager.OUTLINE_COLOR_KEYWORD, data.bodyOutlineColor);
			renderer.material.SetColor(ShaderManager.TOON_COLOR_KEYWORD, data.bodyColor);
		}

		body.transform.parent = go.transform;

		GameObject eye = ResourcesManager.GetEye(data.eyeStyle);
		
		Renderer[] eyeRenderers = eye.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in eyeRenderers) {
			renderer.material.SetColor(ShaderManager.OUTLINE_COLOR_KEYWORD, data.eyeOutlineColor);
			renderer.material.SetColor(ShaderManager.TOON_COLOR_KEYWORD, data.eyeColor);
		}

		eye.transform.parent = go.transform;

		go.AddComponent<CharacterMotor>();
		CharacterController controller = go.AddComponent<CharacterController>();
		controller.name = data.name;
		controller.health = data.health;
		controller.maxStamina = ResourcesManager.PlayerAttributesData.maxStamina[data.maxStaminaIndex].value;
		controller.stamina = controller.maxStamina;
		controller.staminaRecoveryDelay = ResourcesManager.PlayerAttributesData.staminaRecoveryDelay[data.staminaRecoveryDelayIndex].value;
		controller.staminaRecoveryRate = ResourcesManager.PlayerAttributesData.staminaRecoveryRate[data.staminaRecoveryRateIndex].value;
		controller.speed = ResourcesManager.PlayerAttributesData.speed[data.speedIndex].value;
		controller.acceleration = ResourcesManager.PlayerAttributesData.acceleration[data.accelerationIndex].value;
		controller.jumpPower = ResourcesManager.PlayerAttributesData.jumpPower[data.jumpPowerIndex].value;
		controller.jumpTimes = ResourcesManager.PlayerAttributesData.jumpTimes[data.jumpTimesIndex].value;
		controller.jumpPowerDecay = ResourcesManager.PlayerAttributesData.jumpPowerDecay[data.jumpPowerDecayIndex].value;
		controller.dodgeDistance = ResourcesManager.PlayerAttributesData.dodgeDistance[data.dodgeDistanceIndex].value;
		controller.dodgeInvincibilityTime = ResourcesManager.PlayerAttributesData.dodgeInvincibilityTime[data.dodgeInvincibilityTimeIndex].value;
		controller.dodgeCapacity = ResourcesManager.PlayerAttributesData.dodgeCapacity[data.dodgeCapacityIndex].value;
		controller.dodgeCooldown = ResourcesManager.PlayerAttributesData.dodgeCooldown[data.dodgeCooldownIndex].value;
		controller.carriedIBSpriteControllers = new IBSpriteController[ResourcesManager.PlayerAttributesData.ibSpriteCapacity[data.ibSpriteCapacityIndex].value];
		controller.hasAccelerationTrail = true;
		controller.accelerationTrailSettings = ResourcesManager.PlayerAttributesData.accelerationTrailSettings;
		controller.accelerationTrailSettings.color = data.accelerationTrailColor;
		controller.hasDodgeTrail = true;
		controller.dodgeTrailSettings = ResourcesManager.PlayerAttributesData.dodgeTrailSettings;
		controller.dodgeTrailSettings.color = data.dodgeTrailColor;
		controller.bloodColor = data.bloodColor;
		
		LayerManager.SetAllLayers(go, LayerManager.CharacterLayer);
		
		return go;
	}

	public static GameObject GenerateEnemy(string name) {

		return null;
	}
}