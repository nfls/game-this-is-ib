using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerManager {
	
	public static readonly string PlayerDataPath = LocalDataManager.DataStorageRoot + "/pData";

	public static LocalPlayerData LocalPlayerData => localPlayerData;

	private static LocalPlayerData localPlayerData;

	private static LocalPlayerData localPlayerDataCache;

	static PlayerManager() => localPlayerDataCache = new LocalPlayerData();

	public static void Init() {
		if (!File.Exists(PlayerDataPath)) GeneratePlayerData();
		LoadLocalPlayerData();
	}

	public static void GeneratePlayerData() {
		Debug.Log("Generate Player Data");
		localPlayerDataCache = new LocalPlayerData();
		localPlayerDataCache.name = ResourcesManager.PlayerAttributesData.name;
		localPlayerDataCache.health = ResourcesManager.PlayerAttributesData.health;
		localPlayerDataCache.carriedIBSprites = new List<string>();
		localPlayerDataCache.ibSprites = new Dictionary<string, int>();
		localPlayerDataCache.bodyStyle = "default";
		localPlayerDataCache.bodyOutlineColor = Color.black;
		localPlayerDataCache.bodyColor = Color.blue;
		localPlayerDataCache.eyeStyle = "default";
		localPlayerDataCache.eyeOutlineColor = Color.black;
		localPlayerDataCache.eyeColor = Color.cyan;
		localPlayerDataCache.dodgeTrailColor = ResourcesManager.PlayerAttributesData.dodgeTrailSettings.color;
		localPlayerDataCache.accelerationTrailColor = ResourcesManager.PlayerAttributesData.accelerationTrailSettings.color;
		localPlayerDataCache.bloodColor = ResourcesManager.PlayerAttributesData.bloodColor;
		localPlayerDataCache.items = new List<string>();
		localPlayerDataCache.unlockedItems = new List<string>();
		
		#if UNITY_EDITOR
		localPlayerDataCache.jumpPowerDecayIndex = 3;
		localPlayerDataCache.jumpTimesIndex = 2;
		localPlayerDataCache.dodgeCapacityIndex = 2;
		localPlayerDataCache.dodgeCooldownIndex = 3;
		localPlayerDataCache.ibSpriteCapacityIndex = 3;
		#endif
		
		SaveLocalPlayerData();
	}

	public static void LoadLocalPlayerData() {
		localPlayerData = new LocalPlayerData(LocalDataManager.ReadDataFromFile<LocalPlayerDataProxy>(PlayerDataPath));
		RefreshLocalPlayerDataCache();
	}

	public static void SaveLocalPlayerData() => LocalDataManager.WriteDataToFile(PlayerDataPath, new LocalPlayerDataProxy(localPlayerDataCache));

	public static void RefreshLocalPlayerDataCache() => localPlayerDataCache.Cache(localPlayerData);
}