using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public static class ResourcesManager {

	private const string SPLIT_MARK = "@";

	private const string ITEM_PREFIX = "item";
	private const string IB_SPRITE_PREFIX = "ibsprite";
	private const string PROJECTILE_PREFIX = "projectile";
	private const string PARTICLE_PREFIX = "particle";
	private const string BODY_PREFIX = "body";
	private const string EYE_PREFIX = "eye";
	private const string ENEMY_PREFIX = "enemy";
	private const string BOSS_PREFIX = "boss";
	private const string UI_PREFIX = "ui";
	private const string SPRITE_PREFIX = "sprite";
	private const string AUDIO_PREFIX = "audio";

	private const string VERSION_DATA_NAME = "versiondata";
	private const string PLAYER_ATTRIBUTES_DATA_NAME = "playerattributesdata";
	private const string INTERLOCUTION_DATA_NAME = "interlocutiondata";

#if UNITY_EDITOR
	public static readonly string ResourceStorageRoot = Application.dataPath + "/Hotassets/rs";
	public static readonly string TemperResourceStorageRoot = Application.dataPath + "/Hotassets/trs";
#else
	public static readonly string ResourceStorageRoot = Application.persistentDataPath + "/rs";
	public static readonly string TemperResourceStorageRoot = Application.persistentDataPath + "/trs";
#endif
	public static readonly string DataPath = ResourceStorageRoot + "/dt";
	public static readonly string PrefabPath = ResourceStorageRoot + "/pf";
	public static readonly string SpritePath = ResourceStorageRoot + "/sp";
	public static readonly string AudioPath = ResourceStorageRoot + "/ad";
	public static readonly string TemperDataPath = TemperResourceStorageRoot + "/tdt";
	public static readonly string TemperPrefabPath = ResourceStorageRoot + "/tpf";
	public static readonly string TemperSpritePath = ResourceStorageRoot + "/tsp";
	public static readonly string TemperAudioPath = ResourceStorageRoot + "/tad";

	public static VersionData VersionData;
	public static PlayerAttributesData PlayerAttributesData;
	public static Dictionary<string, List<InterlocutionData>> InterlocutionData;

	public static bool IsCompleted => File.Exists(DataPath) && File.Exists(PrefabPath);

	private static Dictionary<string, ItemAttributesData> itemDataDictionary;
	private static Dictionary<string, IBSpriteAttributesData> ibSpriteDataDictionary;

	private static Dictionary<string, GameObject> items = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> ibSprites = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> projectiles = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> particles = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> bodies = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> eyes = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> enemies = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> bosses = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, GameObject> uis = new Dictionary<string, GameObject>(5);
	private static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>(5);
	private static Dictionary<string, AudioClip> audios = new Dictionary<string, AudioClip>(5);

	public static void Init() {
		if (IsCompleted) {
			LoadData();
			LoadPrefabs();
			LoadSprites();
			LoadAudios();
		}
	}

	private static void LoadData() {
#if !UNITY_EDITOR
		AssetBundle bundle = AssetBundle.LoadFromFile(DataPath);
		VersionData = bundle.LoadAsset<VersionData>(VERSION_DATA_NAME);
		PlayerAttributesData = bundle.LoadAsset<PlayerAttributesData>(PLAYER_ATTRIBUTES_DATA_NAME);
		InterlocutionData = JsonConvert.DeserializeObject<Dictionary<string, List<InterlocutionData>>>(bundle.LoadAsset<TextAsset>(INTERLOCUTION_DATA_NAME).text);
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
					case IB_SPRITE_PREFIX: ibSpriteDataDictionary[name] = bundle.LoadAsset<IBSpriteAttributesData>(path);
						break;
				}
			}
		}
#else
		string dataPath = "Assets/Hotassets/Data";
		VersionData = AssetDatabase.LoadAssetAtPath<VersionData>(dataPath + "/" + VERSION_DATA_NAME + ".asset");
		PlayerAttributesData = AssetDatabase.LoadAssetAtPath<PlayerAttributesData>(dataPath + "/" + PLAYER_ATTRIBUTES_DATA_NAME + ".asset");
		InterlocutionData = JsonConvert.DeserializeObject<Dictionary<string, List<InterlocutionData>>>(AssetDatabase.LoadAssetAtPath<TextAsset>(dataPath + "/" + INTERLOCUTION_DATA_NAME + ".txt").text);
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

	private static void LoadPrefabs() {
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
				case PARTICLE_PREFIX: particles[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case BODY_PREFIX: bodies[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case EYE_PREFIX: eyes[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case ENEMY_PREFIX: enemies[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case BOSS_PREFIX: bosses[name] = bundle.LoadAsset<GameObject>(path);
					break;
				case UI_PREFIX: uis[name] = bundle.LoadAsset<GameObject>(path);
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
					case PARTICLE_PREFIX:
						particles[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
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
					case UI_PREFIX:
						uis[name] = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath + "/" + path);
						break;
				}
			}
		}
#endif
	}
	
	private static void LoadSprites() {
#if !UNITY_EDITOR
		AssetBundle bundle = AssetBundle.LoadFromFile(SpritePath);
		string[] paths = bundle.GetAllAssetNames();
		foreach (var path in paths) {
			string p = path.Substring(path.LastIndexOf("/") + 1);
			int splitIndex = p.IndexOf(SPLIT_MARK);
			string prefix = p.Substring(0, splitIndex);
			string name = p.Substring(splitIndex + 1, p.IndexOf(".") - (splitIndex + 1));
			if (prefix == SPRITE_PREFIX) {
				sprites[name] = bundle.LoadAsset<Sprite>(path);
			}
		}
#else
		string spritePath = "Assets/Hotassets/Sprites";
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Hotassets/Sprites");
		FileInfo[] files = info.GetFiles(new[] { "*.png", "*.jpg" }, SearchOption.TopDirectoryOnly);
		foreach (var file in files) {
			string path = file.Name;
			if (path.Contains(SPLIT_MARK)) {
				int splitIndex = path.IndexOf(SPLIT_MARK);
				string prefix = path.Substring(0, splitIndex);
				string name = path.Substring(splitIndex + 1, path.IndexOf(".") - (splitIndex + 1));
				if (prefix == SPRITE_PREFIX) {
					audios[name] = AssetDatabase.LoadAssetAtPath<AudioClip>(spritePath + "/" + path);
				}
			}
		}
#endif
	}

	private static void LoadAudios() {
#if !UNITY_EDITOR
		AssetBundle bundle = AssetBundle.LoadFromFile(AudioPath);
		string[] paths = bundle.GetAllAssetNames();
		foreach (var path in paths) {
			string p = path.Substring(path.LastIndexOf("/") + 1);
			int splitIndex = p.IndexOf(SPLIT_MARK);
			string prefix = p.Substring(0, splitIndex);
			string name = p.Substring(splitIndex + 1, p.IndexOf(".") - (splitIndex + 1));
			if (prefix == AUDIO_PREFIX) {
				audios[name] = bundle.LoadAsset<AudioClip>(path);
			}
		}
#else
		string audioPath = "Assets/Hotassets/Audios";
		DirectoryInfo info = new DirectoryInfo(Application.dataPath + "/Hotassets/Audios");
		FileInfo[] files = info.GetFiles(new[] { "*.mp3", "*.wav", "*.aif" }, SearchOption.TopDirectoryOnly);
		foreach (var file in files) {
			string path = file.Name;
			if (path.Contains(SPLIT_MARK)) {
				int splitIndex = path.IndexOf(SPLIT_MARK);
				string prefix = path.Substring(0, splitIndex);
				string name = path.Substring(splitIndex + 1, path.IndexOf(".") - (splitIndex + 1));
				if (prefix == AUDIO_PREFIX) {
					audios[name] = AssetDatabase.LoadAssetAtPath<AudioClip>(audioPath + "/" + path);
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

	public static GameObject GetParticle(string name) {
		return GameObject.Instantiate(particles[name]);
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

	public static GameObject GetUI(string name) {
		return GameObject.Instantiate(uis[name]);
	}

	public static Sprite GetSprite(string name) {
		return sprites[name];
	}

	public static AudioClip GetAudio(string name) {
		return audios[name];
	}
}