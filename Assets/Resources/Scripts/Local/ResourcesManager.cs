using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class ResourcesManager {

	private const string SPLIT_MARK = "@";

	private const string ITEM_PREFIX = "item";
	private const string IB_SPRITE_PREFIX = "ibsprite";
	private const string PROJECTILE_PREFIX = "projectile";
	private const string TERRAIN_PREFIX = "terrain";
	private const string DEVICE_PREFIX = "device";
	private const string BODY_PREFIX = "body";
	private const string EYE_PREFIX = "eye";
	private const string ENEMY_PREFIX = "enemy";
	private const string BOSS_PREFIX = "boss";

	private const string VERSION_DATA_NAME = "versiondata";
	private const string PLAYER_ATTRIBUTES_DATA_NAME = "playerattributesdata";

#if UNITY_EDITOR
	public static readonly string ResourceStorageRoot = Application.dataPath + "/Hotassets/rs";
	public static readonly string TemperResourceStorageRoot = Application.dataPath + "/Hotassets/trs";
#else
	public static readonly string ResourceStorageRoot = Application.persistentDataPath + "/rs";
	public static readonly string TemperResourceStorageRoot = Application.persistentDataPath + "/trs";
#endif
	public static readonly string DataPath = ResourceStorageRoot + "/dt";
	public static readonly string PrefabPath = ResourceStorageRoot + "/pf";
	public static readonly string TemperDataPath = TemperResourceStorageRoot + "/tdt";
	public static readonly string TemperPrefabPath = ResourceStorageRoot + "/tpf";

	public static VersionData versionData;
	public static PlayerAttributesData playerAttributesData;

	public static bool IsCompleted {
		get { return File.Exists(DataPath) && File.Exists(PrefabPath); }
	}

	private static Dictionary<string, ItemAttributesData> itemDataDictionary;
	private static Dictionary<string, IBSpriteAttributesData> ibSpriteDataDictionary;

	private static Dictionary<string, GameObject> items;
	private static Dictionary<string, GameObject> ibSprites;
	private static Dictionary<string, GameObject> projectiles;
	private static Dictionary<string, GameObject> terrains;
	private static Dictionary<string, GameObject> devices;
	private static Dictionary<string, GameObject> bodies;
	private static Dictionary<string, GameObject> eyes;
	private static Dictionary<string, GameObject> enemies;
	private static Dictionary<string, GameObject> bosses;

	static ResourcesManager() {
		itemDataDictionary = new Dictionary<string, ItemAttributesData>(5);
		ibSpriteDataDictionary = new Dictionary<string, IBSpriteAttributesData>(5);

		items = new Dictionary<string, GameObject>(5);
		ibSprites = new Dictionary<string, GameObject>(5);
		projectiles = new Dictionary<string, GameObject>(5);
		terrains = new Dictionary<string, GameObject>(5);
		devices = new Dictionary<string, GameObject>(5);

		bodies = new Dictionary<string, GameObject>(5);
		eyes = new Dictionary<string, GameObject>(5);
		enemies = new Dictionary<string, GameObject>(5);
		bosses = new Dictionary<string, GameObject>(5);
	}

	public static void Init() {
		if (IsCompleted) {
			LoadData();
			LoadPrefabs();
		}
	}

	public static void LoadData() {
#if !UNITY_EDITOR
		AssetBundle bundle = AssetBundle.LoadFromFile(DataPath);
		versionData = bundle.LoadAsset<VersionData>(VERSION_DATA_NAME);
		playerAttributesData = bundle.LoadAsset<PlayerAttributesData>(PLAYER_ATTRIBUTES_DATA_NAME);
		string[] paths = bundle.GetAllAssetNames();
		foreach (string path in paths) {
			if (path.Contains(SPLIT_MARK)) {
				string p = path.Substring(path.LastIndexOf("/") + 1);
				int splitIndex = p.IndexOf(SPLIT_MARK);
				string prefix = p.Substring(0, splitIndex);
				string name = p.Substring(splitIndex + 1, p.IndexOf(".") - (splitIndex + 1));
				switch (prefix) {
					case ITEM_PREFIX: itemDataDictionary[name] = bundle.LoadAsset<ItemAttributesData>(path);
						break;
					case IB_SPRITE_PREFIX: ibSpriteDataDictionary[name] =
 bundle.LoadAsset<IBSpriteAttributesData>(path);
						break;
				}
			}
		}
#else
		string dataPath = "Assets/Hotassets/Data";
		versionData = AssetDatabase.LoadAssetAtPath<VersionData>(dataPath + "/" + VERSION_DATA_NAME + ".asset");
		playerAttributesData =
			AssetDatabase.LoadAssetAtPath<PlayerAttributesData>(
				dataPath + "/" + PLAYER_ATTRIBUTES_DATA_NAME + ".asset");
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Hotassets/Data");
		FileInfo[] files = info.GetFiles("*.asset", SearchOption.TopDirectoryOnly);
		foreach (var file in files) {
			string path = file.Name;
			if (path.Contains(SPLIT_MARK)) {
				int splitIndex = path.IndexOf(SPLIT_MARK);
				string prefix = path.Substring(0, splitIndex);
				string name = path.Substring(splitIndex + 1, path.IndexOf(".") - (splitIndex + 1));
				switch (prefix) {
					case ITEM_PREFIX:
						itemDataDictionary[name] =
							AssetDatabase.LoadAssetAtPath<ItemAttributesData>(dataPath + "/" + path);
						break;
					case IB_SPRITE_PREFIX:
						ibSpriteDataDictionary[name] =
							AssetDatabase.LoadAssetAtPath<IBSpriteAttributesData>(dataPath + "/" + path);
						break;
				}
			}
		}
#endif
	}

	public static void LoadPrefabs() {
#if !UNITY_EDITOR
		AssetBundle bundle = AssetBundle.LoadFromFile(PrefabPath);
		string[] paths = bundle.GetAllAssetNames();
		foreach (var path in paths) {
			string p = path.Substring(path.LastIndexOf("/") + 1);
			int splitIndex = p.IndexOf(SPLIT_MARK);
			string prefix = p.Substring(0, splitIndex);
			string name = p.Substring(splitIndex + 1, p.IndexOf(".") - (splitIndex + 1));
			switch (prefix) {
				case ITEM_PREFIX: items[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case IB_SPRITE_PREFIX: ibSprites[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case PROJECTILE_PREFIX: projectiles[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case BODY_PREFIX: bodies[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case EYE_PREFIX: eyes[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case ENEMY_PREFIX: enemies[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case BOSS_PREFIX: bosses[name] = bundle.LoadAsset<GameObject>(path);
					break;
			}
		}
#else
		string prefabPath = "Assets/Hotassets/Prefabs";
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Hotassets/Prefabs");
		FileInfo[] files = info.GetFiles("*.prefab", SearchOption.TopDirectoryOnly);
		foreach (var file in files) {
			string path = file.Name;
			if (path.Contains(SPLIT_MARK)) {
				int splitIndex = path.IndexOf(SPLIT_MARK);
				string prefix = path.Substring(0, splitIndex);
				string name = path.Substring(splitIndex + 1, path.IndexOf(".") - (splitIndex + 1));
				switch (prefix) {
					case ITEM_PREFIX:
						items[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case IB_SPRITE_PREFIX:
						ibSprites[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case PROJECTILE_PREFIX:
						projectiles[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case BODY_PREFIX:
						bodies[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case EYE_PREFIX:
						eyes[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case ENEMY_PREFIX:
						enemies[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
					case BOSS_PREFIX:
						bosses[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
				}
			}
		}
#endif
	}

	public static GameObject GetItem(string name) {
		return GameObject.Instantiate(items[name]);
	}

	public static GameObject GetIBSprite(string name) {
		return GameObject.Instantiate(ibSprites[name]);
	}

	public static GameObject GetProjectile(string name) {
		return GameObject.Instantiate(projectiles[name]);
	}

	public static GameObject GetTerrain(string name) {
		return GameObject.Instantiate(terrains[name]);
	}

	public static GameObject GetDevice(string name) {
		return GameObject.Instantiate(devices[name]);
	}

	public static GameObject GetBody(string name) {
		return GameObject.Instantiate(bodies[name]);
	}

	public static GameObject GetEye(string name) {
		return GameObject.Instantiate(eyes[name]);
	}

	public static GameObject GetEnemy(string name) {
		return GameObject.Instantiate(enemies[name]);
	}

	public static GameObject GetBoss(string name) {
		return GameObject.Instantiate(bosses[name]);
	}
}