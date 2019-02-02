
using System.Collections.Generic;
using UnityEngine;

public static class MaterialManager {
	
	private static Dictionary<string, Material> sharedMaterials = new Dictionary<string, Material>();

	public static Material GetMaterial(string shaderName) {
		if (sharedMaterials.ContainsKey(shaderName)) return sharedMaterials[shaderName];
		Material material = new Material(Shader.Find(shaderName));
		sharedMaterials.Add(shaderName, material);
		return material;
	}
}