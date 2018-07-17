using System;
using UnityEngine;

[Serializable]
public class TrailSettings {

	public MotionVectorGenerationMode motionVectorMode;
	public Color color;
	public float brightness;
	public float lifespan;
	public float minDistance;
	public AnimationCurve width;
	public int cornerVertices;
	public int endCapVertices;
	public LineAlignment alignment;
	
	[HideInInspector] public Material material;

	public void Init() {
		material = new Material(Shader.Find(MaterialManager.SINGLE_COLOR_SHADER_NAME));
		material.SetColor(MaterialManager.TOON_COLOR_KEYWORD, color);
		material.SetFloat(MaterialManager.BRIGHTNESS_KEYWORD, brightness);
	}

	public void InitRenderer(ref TrailRenderer renderer) {
		renderer.motionVectorGenerationMode = motionVectorMode;
		renderer.material = material;
		renderer.time = lifespan;
		renderer.minVertexDistance = minDistance;
		renderer.widthCurve = width;
		renderer.numCornerVertices = cornerVertices;
		renderer.numCapVertices = endCapVertices;
		renderer.alignment = alignment;
	}
}