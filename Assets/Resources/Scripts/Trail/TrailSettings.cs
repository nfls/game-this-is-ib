using System;
using UnityEngine;

[Serializable]
public class TrailSettings {
	
	public const string SHADER_NAME = "WilliamXie/UnlitOutlinedShader";
	public const string MAIN_COLOR_KEYWORD = "_MainColor";
	public const string OUTLINE_COLOR_KEYWORD = "_OutlineColor";
	public const string OUTLINE_WIDTH_KEYWORD = "_OutlineWidth";
	public const string OUTLINE_DISTANCE_KEYWORD = "_OutlineDistance";
	public const string BRIGHTNESS_KEYWORD = "_Brightness";

	public MotionVectorGenerationMode motionVectorMode;
	public Color mainColor;
	public Color outlineColor;
	public float outlineWidth;
	public float outlineDistance;
	public float brightness;
	public float lifespan;
	public float minDistance;
	public AnimationCurve width;
	public int cornerVertices;
	public int endCapVertices;
	public LineAlignment alignment;
	
	[HideInInspector] public Material material;

	public void Init() {
		material = new Material(Shader.Find(SHADER_NAME));
		material.SetColor(MAIN_COLOR_KEYWORD, mainColor);
		material.SetColor(OUTLINE_COLOR_KEYWORD, outlineColor);
		material.SetFloat(OUTLINE_WIDTH_KEYWORD, outlineWidth);
		material.SetFloat(OUTLINE_DISTANCE_KEYWORD, outlineDistance);
		material.SetFloat(BRIGHTNESS_KEYWORD, brightness);
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