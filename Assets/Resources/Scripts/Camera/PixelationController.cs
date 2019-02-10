using UnityEngine;

public class PixelationController : MonoBehaviour {

	public float pixelsPerRow;
	public float ratio;
	public Material material;

	private void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (material) {
			material.SetFloat("_PixelsPerRow", pixelsPerRow);
			material.SetFloat("_Ratio", ratio);
			Graphics.Blit(src, dest, material);
		} else Graphics.Blit(src, dest);
	}
}
