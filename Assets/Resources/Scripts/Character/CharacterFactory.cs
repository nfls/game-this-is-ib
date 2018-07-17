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
			renderer.material.SetColor(MaterialManager.OUTLINE_COLOR_KEYWORD, data.bodyOutlineColor);
			renderer.material.SetColor(MaterialManager.TOON_COLOR_KEYWORD, data.bodyColor);
		}

		body.transform.parent = go.transform;

		GameObject eye = ResourcesManager.GetEye(data.eyeStyle);
		
		Renderer[] eyeRenderers = eye.GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in eyeRenderers) {
			renderer.material.SetColor(MaterialManager.OUTLINE_COLOR_KEYWORD, data.eyeOutlineColor);
			renderer.material.SetColor(MaterialManager.TOON_COLOR_KEYWORD, data.eyeColor);
		}

		eye.transform.parent = go.transform;

		go.AddComponent<CharacterMotor>();
		CharacterController controller = go.AddComponent<CharacterController>();
		controller.name = data.name;
		controller.health = data.health;
		controller.speed = ResourcesManager.playerAttributesData.speed[data.speedIndex].value;
		controller.acceleration = ResourcesManager.playerAttributesData.acceleration[data.accelerationIndex].value;
		controller.jumpPower = ResourcesManager.playerAttributesData.jumpPower[data.jumpPowerIndex].value;
		controller.jumpTimes = ResourcesManager.playerAttributesData.jumpTimes[data.jumpTimesIndex].value;
		controller.jumpPowerDecay = ResourcesManager.playerAttributesData.jumpPowerDecay[data.jumpPowerDecayIndex].value;
		controller.dodgeDistance = ResourcesManager.playerAttributesData.dodgeDistance[data.dodgeDistanceIndex].value;
		controller.dodgeInvincibilityTime = ResourcesManager.playerAttributesData.dodgeInvincibilityTime[data.dodgeInvincibilityTimeIndex].value;
		controller.dodgeCapacity = ResourcesManager.playerAttributesData.dodgeCapacity[data.dodgeCapacityIndex].value;
		controller.dodgeCooldown = ResourcesManager.playerAttributesData.dodgeCooldown[data.dodgeCooldownIndex].value;
		controller.carriedIBSpriteControllers = new IBSpriteController[ResourcesManager.playerAttributesData.ibSpriteCapacity[data.ibSpriteCapacityIndex].value];
		controller.hasAccelerationTrail = true;
		controller.accelerationTrailSettings = ResourcesManager.playerAttributesData.accelerationTrailSettings;
		controller.accelerationTrailSettings.color = data.accelerationTrailColor;
		controller.hasDodgeTrail = true;
		controller.dodgeTrailSettings = ResourcesManager.playerAttributesData.dodgeTrailSettings;
		controller.dodgeTrailSettings.color = data.dodgeTrailColor;
		
		LayerManager.SetAllLayers(go, LayerManager.CharacterLayer);
		
		return go;
	}

	public static GameObject GenerateEnemy(string name) {

		return null;
	}
}