using UnityEngine;

public class RadialBlurController : MonoBehaviour {

	[Range(0f, .05f)]
	public float factor = .01f;
	public float duration = .1f;
	public Vector2 center = new Vector2(.5f, .5f);
	public Material material;

	private void OnRenderImage(RenderTexture src, RenderTexture dest) {
		if (material) {
			material.SetFloat("_BlurFactor", factor);
			material.SetVector("_BlurCenter", center);
			Graphics.Blit(src, dest, material);
		} else Graphics.Blit(src, dest);
	}
}