using System;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public static class MeshTool {

	public static GameObject InstantiateMeshGameObject(Mesh mesh, Material material, bool insight = true) {
		GameObject go = new GameObject(mesh.name);
		go.AddComponent<MeshFilter>().mesh = mesh;
		MeshRenderer renderer = go.AddComponent<MeshRenderer>();
		renderer.material = material;
		renderer.lightProbeUsage = LightProbeUsage.Off;
		renderer.reflectionProbeUsage = ReflectionProbeUsage.Off;
		renderer.shadowCastingMode = ShadowCastingMode.Off;
		renderer.receiveShadows = false;

		if (insight) {
			Plane plane = new Plane(new Vector3(0f, 0f, -1f), Vector3.zero);
			Camera camera = SceneView.lastActiveSceneView.camera;
			Ray ray = camera.ViewportPointToRay(new Vector3(.5f, .5f));
			float enter;
			go.transform.position = plane.Raycast(ray, out enter) ? ray.GetPoint(enter) : new Vector3(camera.transform.position.x, camera.transform.position.y, 0f);
		}
		
		return go;
	}

	public static GameObject InstantiatePlane(PlaneGenerationData data, Material material) => InstantiateMeshGameObject(GeneratePlane(data), material);

	public static Mesh GeneratePlane(PlaneGenerationData data) {
		StringBuilder nameBuilder = new StringBuilder("Procedural Plane [", 28);
		nameBuilder.Append(data.length);
		nameBuilder.Append("x");
		nameBuilder.Append(data.width);
		nameBuilder.Append("][");
		nameBuilder.Append(data.verticesPerLength);
		nameBuilder.Append(",");
		nameBuilder.Append(data.verticesPerWidth);
		nameBuilder.Append("]");
		
		Vector3[] vertices = new Vector3[data.verticesPerLength * data.verticesPerWidth];
		Vector2[] uv = new Vector2[vertices.Length];
		Vector4[] tangents = new Vector4[vertices.Length];
		Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
		switch (data.direction) {
			case PlaneGenerationData.Direction.XY: tangent = new Vector4(1f, 0f, 0f, -1f); break;
			case PlaneGenerationData.Direction.XZ: tangent = new Vector4(1f, 0f, 0f, -1f); break;
			case PlaneGenerationData.Direction.YZ: tangent = new Vector4(0f, 0f, 1f, -1f); break;
			default: tangent = Vector4.zero; break;
		}
		
		for (int i = 0, w = 0; w < data.verticesPerWidth; w++)
			for (int l = 0; l < data.verticesPerLength; l++, i++) {
				float u = l / (float) (data.verticesPerLength - 1);
				float v = w / (float) (data.verticesPerWidth - 1);
				float x = -data.length / 2f + data.length * u;
				float y = -data.width / 2f + data.width * v;
				
				Vector3 position;
				switch (data.direction) {
					case PlaneGenerationData.Direction.XY: position = new Vector3(x, y, 0f); break;
					case PlaneGenerationData.Direction.XZ: position = new Vector3(x, 0f, y); break;
					case PlaneGenerationData.Direction.YZ: position = new Vector3(0f, y, x); break;
					default: position = Vector3.zero; break;
				}

				vertices[i] = position;
				uv[i] = new Vector2(u, v);
				tangents[i] = tangent;
			}
		
		int[] triangles = new int[(data.verticesPerLength - 1) * (data.verticesPerWidth - 1) * 6];
		for (int vi = 0, ti = 0, w = 0; w < data.verticesPerWidth - 1; w++, vi++)
			for (int l = 0; l < data.verticesPerLength - 1; l++, vi++, ti += 6) {
				triangles[ti] = vi;
				triangles[ti + 3] = triangles[ti + 2] = vi + 1;
				triangles[ti + 4] = triangles[ti + 1] = vi + data.verticesPerLength;
				triangles[ti + 5] = vi + data.verticesPerLength + 1;
			}

		Mesh mesh = new Mesh {
			name = nameBuilder.ToString(),
			vertices = vertices,
			triangles = triangles,
			uv = uv
		};
		
		mesh.RecalculateNormals();

		mesh.tangents = tangents;
		
		return mesh;
	}
}

public enum MeshGenerationType {
	Plane,
	Box,
	Slope,
	VolumetricFluid
}

[Serializable]
public class PlaneGenerationData {
	public Direction direction;
	public float length = 5f;
	public float width = 2f;
	public int verticesPerLength = 2;
	public int verticesPerWidth = 2;

	public enum Direction {
		XY,
		XZ,
		YZ
	}
}

[Serializable]
public class VolumetricFluidGenerationData {
	
}