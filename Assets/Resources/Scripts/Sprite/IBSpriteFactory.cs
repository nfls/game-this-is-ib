using UnityEngine;

public static class IBSpriteFactory {

	public static GameObject GenerateIBSprite(string name) {
		GameObject go = ResourcesManager.GetIBSprite(name);
		return go;
	}
}