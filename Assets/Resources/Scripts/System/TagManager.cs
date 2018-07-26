using UnityEngine;

public class TagManager {

	public const string LOCAL_PLAYER_TAG = "Local Player";
	public const string REMOTE_PLAYER_TAG = "Remote Player";
	public const string ENEMY_TAG = "Enemy";
	public const string DESTRUCTIBLE_TAG = "Destructible";
	public const string INTERACTIVE_TAG = "Interactive";

	public static void SetAllTags(GameObject go, string tag) {
		go.tag = tag;
		int childCount = go.transform.childCount;
		for (int i = 0; i < childCount; i++) {
			SetAllTags(go.transform.GetChild(i).gameObject, tag);
		}
	}
}