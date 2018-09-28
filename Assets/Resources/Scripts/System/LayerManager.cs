using UnityEngine;

public static class LayerManager {

	public static readonly int InteractionLayer = LayerMask.NameToLayer("Interaction");
	public static readonly int TerrainLayer = LayerMask.NameToLayer("Terrain");
	public static readonly int DeviceLayer = LayerMask.NameToLayer("Device");
	public static readonly int CharacterLayer = LayerMask.NameToLayer("Character");
	public static readonly int SpriteLayer = LayerMask.NameToLayer("Sprite");
	public static readonly int ProjectileLayer = LayerMask.NameToLayer("Projectile");
	public static readonly int DodgeLayer = LayerMask.NameToLayer("Dodge");
	public static readonly int PickupLayer = LayerMask.NameToLayer("Pickup");
	
	public static void SetAllLayers(GameObject go, int layer) {
		go.layer = layer;
		int childCount = go.transform.childCount;
		for (int i = 0; i < childCount; i++) SetAllLayers(go.transform.GetChild(i).gameObject, layer);
	}
}
