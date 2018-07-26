using System.IO;
using UnityEditor;
using UnityEngine;

public static class AssetBundleTool {

	[MenuItem("这就是IB/资源工具/打包数据文件")]
	public static void BundleData() {
		string dataPath = Application.dataPath + "/Hotassets/Data";
		DirectoryInfo dataDir = new DirectoryInfo(dataPath);
		FileInfo[] dataFiles = dataDir.GetFiles(new[] {"*.asset", "*.txt"}, SearchOption.AllDirectories);
		
		string[] dataNames = new string[dataFiles.Length];
		EditorUtility.DisplayProgressBar("自动打包", "提取数据文件[0]", 0f);
		for (int i = 0, l = dataNames.Length; i < l; i++) {
			EditorUtility.DisplayProgressBar("自动打包", "提取数据文件[" + (i + 1) + "] " + dataFiles[i].Name, i + 1 / l);
			dataNames[i] =  "Assets/Hotassets/Data/" + dataFiles[i].Name;
			Debug.Log(dataFiles[i].Name);
		}
		EditorUtility.ClearProgressBar();
		
		AssetBundleBuild[] builds = { new AssetBundleBuild { assetBundleName = "dt", assetNames = dataNames } };
		EditorUtility.DisplayProgressBar("自动打包", "创建资源包......", .5f);
		BuildPipeline.BuildAssetBundles("Assets/Hotassets/rs", builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
		EditorUtility.DisplayProgressBar("自动打包", "打包完成", 1f);
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}
	
	[MenuItem("这就是IB/资源工具/打包预制体文件")]
	public static void BundlePrefabs() {
		string prefabPath = Application.dataPath + "/Hotassets/Prefabs";
		DirectoryInfo prefabDir = new DirectoryInfo(prefabPath);
		FileInfo[] prefabFiles = prefabDir.GetFiles("*.prefab", SearchOption.AllDirectories);
		string[] prefabNames = new string[prefabFiles.Length];
		EditorUtility.DisplayProgressBar("自动打包", "提取预制体文件[0]", 0f);
		for (int i = 0, l = prefabNames.Length; i < l; i++) {
			EditorUtility.DisplayProgressBar("自动打包", "提取预制体文件[" + (i + 1) + "] " + prefabFiles[i].Name, i + 1 / l);
			prefabNames[i] = "Assets/Hotassets/Prefabs/" + prefabFiles[i].Name;
			Debug.Log(prefabFiles[i].Name);
		}
		EditorUtility.ClearProgressBar();

		AssetBundleBuild[] builds = { new AssetBundleBuild { assetBundleName = "pf", assetNames = prefabNames } };
		EditorUtility.DisplayProgressBar("自动打包", "创建资源包......", .5f);
		BuildPipeline.BuildAssetBundles("Assets/Hotassets/rs", builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
		EditorUtility.DisplayProgressBar("自动打包", "打包完成", 1f);
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}

	[MenuItem("这就是IB/资源工具/自动打包")]
	public static void BundleAll() {
		string dataPath = Application.dataPath + "/Hotassets/Data";
		DirectoryInfo dataDir = new DirectoryInfo(dataPath);
		FileInfo[] dataFiles = dataDir.GetFiles(new[] {"*.asset", "*.txt"}, SearchOption.AllDirectories);
		string[] dataNames = new string[dataFiles.Length];
		EditorUtility.DisplayProgressBar("自动打包", "提取数据文件[0]", 0f);
		for (int i = 0, l = dataNames.Length; i < l; i++) {
			EditorUtility.DisplayProgressBar("自动打包", "提取数据文件[" + (i + 1) + "] " + dataFiles[i].Name, i + 1 / l);
			dataNames[i] =  "Assets/Hotassets/Data/" + dataFiles[i].Name;
			Debug.Log(dataFiles[i].Name);
		}
		EditorUtility.ClearProgressBar();

		string prefabPath = Application.dataPath + "/Hotassets/Prefabs";
		DirectoryInfo prefabDir = new DirectoryInfo(prefabPath);
		FileInfo[] prefabFiles = prefabDir.GetFiles("*.prefab", SearchOption.AllDirectories);
		string[] prefabNames = new string[prefabFiles.Length];
		EditorUtility.DisplayProgressBar("自动打包", "提取预制体文件[0]", 0f);
		for (int i = 0, l = prefabNames.Length; i < l; i++) {
			EditorUtility.DisplayProgressBar("自动打包", "提取预制体文件[" + (i + 1) + "] " + prefabFiles[i].Name, i + 1 / l);
			prefabNames[i] = "Assets/Hotassets/Prefabs/" + prefabFiles[i].Name;
			Debug.Log(prefabFiles[i].Name);
		}
		EditorUtility.ClearProgressBar();

		AssetBundleBuild[] builds = { new AssetBundleBuild { assetBundleName = "dt", assetNames = dataNames }, new AssetBundleBuild { assetBundleName = "pf", assetNames = prefabNames } };
		EditorUtility.DisplayProgressBar("自动打包", "创建资源包......", .5f);
		BuildPipeline.BuildAssetBundles("Assets/Hotassets/rs", builds, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
		EditorUtility.DisplayProgressBar("自动打包", "打包完成", 1f);
		AssetDatabase.Refresh();
		EditorUtility.ClearProgressBar();
	}
}