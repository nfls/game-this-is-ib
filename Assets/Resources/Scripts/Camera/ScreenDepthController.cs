using UnityEngine;

public class ScreenDepthController : MonoBehaviour {

	public Material material;

	private void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (material != null) Graphics.Blit(src, dest, material);
	}
}