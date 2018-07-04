using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PlayerManager {
	
	public static readonly string PlayerDataPath = LocalDataManager.DataStorageRoot + "/pData";

	public static bool HasRemotePlayer {
		get { return remotePlayerData != null; }
	}

	public static LocalPlayerData LocalPlayerData {
		get { return localPlayerData; }
	}

	public static RemotePlayerData RemotePlayerData {
		get { return remotePlayerData; }
	}

	private static LocalPlayerData localPlayerData;
	private static RemotePlayerData remotePlayerData;

	private static LocalPlayerData localPlayerDataCache;
	private static RemotePlayerData remotePlayerDataCache;

	static PlayerManager() {
		localPlayerDataCache = new LocalPlayerData();
		remotePlayerDataCache = new RemotePlayerData();
	}

	public static void Init() {
		if (!File.Exists(PlayerDataPath)) {
			GeneratePlayerData();
		}
		
		LoadLocalPlayerData();
	}

	public static void GeneratePlayerData() {
		Debug.Log("Generate Player Data");
		localPlayerDataCache = new LocalPlayerData();
		localPlayerDataCache.name = ResourcesManager.playerAttributesData.name;
		localPlayerDataCache.health = ResourcesManager.playerAttributesData.health;
		localPlayerDataCache.carriedIBSprites = new List<string>();
		localPlayerDataCache.ibSprites = new Dictionary<string, int>();
		localPlayerDataCache.bodyStyle = "default";
		localPlayerDataCache.bodyOutlineColor = Color.black;
		localPlayerDataCache.bodyColor = Color.blue;
		localPlayerDataCache.eyeStyle = "default";
		localPlayerDataCache.eyeOutlineColor = Color.black;
		localPlayerDataCache.eyeColor = Color.cyan;
		localPlayerDataCache.dodgeTrailColor = Color.blue;
		localPlayerDataCache.dodgeTrailOutlineColor = Color.blue;
		localPlayerDataCache.accelerationTrailColor = Color.cyan;
		localPlayerDataCache.accelerationTrailOutlineColor = Color.cyan;
		localPlayerDataCache.items = new List<string>();
		localPlayerDataCache.unlockedItems = new List<string>();
		SaveLocalPlayerData();
	}

	public static void LoadLocalPlayerData() {
		localPlayerData = new LocalPlayerData(LocalDataManager.ReadDataFromFile<LocalPlayerDataProxy>(PlayerDataPath));
		RefreshLocalPlayerDataCache();
	}

	public static void SaveLocalPlayerData() {
		LocalDataManager.WriteDataToFile(PlayerDataPath, new LocalPlayerDataProxy(localPlayerDataCache));
	}

	public static void RefreshLocalPlayerDataCache() {
		localPlayerDataCache.Cache(localPlayerData);
	}

	public static void RefreshRemotePlayerDataCache() {
		remotePlayerDataCache.Cache(remotePlayerData);
	}
}