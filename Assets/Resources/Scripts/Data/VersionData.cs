using System;
using UnityEngine;

[CreateAssetMenu(fileName = "VersionData", menuName = "This-Is-IB/Version Data")]
public class VersionData : ScriptableObject {

	public string versionCode;
	public string versionDescription;
	public int[] versionNumbers;
	public int[] supportVersionNumbers;
	public BuildMode buildMode;
	public AssetBundleLink[] assetBundleLinks;

	public bool NeedsReinstall(VersionData versionData) {
		return false;
	}

	public bool NeedsUpdate(VersionData versionData) {
		return false;
	}

	public enum BuildMode {
		Development,
		AlphaTest,
		BetaTest,
		Release
	}

	[Serializable]
	public class AssetBundleLink {
		
		public string name;
		public string url;
	}
}