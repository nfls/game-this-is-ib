using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public static class MaterialTool {

	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 1X")]
	public static void GeneratePerlinNoiseMap1X() {
		GeneratePerlinNoiseMap(1);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 2X")]
	public static void GeneratePerlinNoiseMap2X() {
		GeneratePerlinNoiseMap(2);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 3X")]
	public static void GeneratePerlinNoiseMap3X() {
		GeneratePerlinNoiseMap(3);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 4X")]
	public static void GeneratePerlinNoiseMap4X() {
		GeneratePerlinNoiseMap(4);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 5X")]
	public static void GeneratePerlinNoiseMap5X() {
		GeneratePerlinNoiseMap(5);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 6X")]
	public static void GeneratePerlinNoiseMap6X() {
		GeneratePerlinNoiseMap(6);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 7X")]
	public static void GeneratePerlinNoiseMap7X() {
		GeneratePerlinNoiseMap(7);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 8X")]
	public static void GeneratePerlinNoiseMap8X() {
		GeneratePerlinNoiseMap(8);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 9X")]
	public static void GeneratePerlinNoiseMap9X() {
		GeneratePerlinNoiseMap(9);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 10X")]
	public static void GeneratePerlinNoiseMap10X() {
		GeneratePerlinNoiseMap(10);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 11X")]
	public static void GeneratePerlinNoiseMa11X() {
		GeneratePerlinNoiseMap(11);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 12X")]
	public static void GeneratePerlinNoiseMap12X() {
		GeneratePerlinNoiseMap(12);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 13X")]
	public static void GeneratePerlinNoiseMap13X() {
		GeneratePerlinNoiseMap(13);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 14X")]
	public static void GeneratePerlinNoiseMap14X() {
		GeneratePerlinNoiseMap(14);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 15X")]
	public static void GeneratePerlinNoiseMap15X() {
		GeneratePerlinNoiseMap(15);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 20X")]
	public static void GeneratePerlinNoiseMap20X() {
		GeneratePerlinNoiseMap(20);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 30X")]
	public static void GeneratePerlinNoiseMap30X() {
		GeneratePerlinNoiseMap(30);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 50X")]
	public static void GeneratePerlinNoiseMap50X() {
		GeneratePerlinNoiseMap(50);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/Perlin 100X")]
	public static void GeneratePerlinNoiseMap100X() {
		GeneratePerlinNoiseMap(100);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 1X")]
	public static void Generate3DPerlinNoiseMap1X() {
		Generate3DPerlinNoiseMap(1);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 2X")]
	public static void Generate3DPerlinNoiseMap2X() {
		Generate3DPerlinNoiseMap(2);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 3X")]
	public static void Generate3DPerlinNoiseMap3X() {
		Generate3DPerlinNoiseMap(3);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 4X")]
	public static void Generate3DPerlinNoiseMap4X() {
		Generate3DPerlinNoiseMap(4);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 5X")]
	public static void Generate3DPerlinNoiseMap5X() {
		Generate3DPerlinNoiseMap(5);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 6X")]
	public static void Generate3DPerlinNoiseMap6X() {
		Generate3DPerlinNoiseMap(6);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 7X")]
	public static void Generate3DPerlinNoiseMap7X() {
		Generate3DPerlinNoiseMap(7);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 8X")]
	public static void Generate3DPerlinNoiseMap8X() {
		Generate3DPerlinNoiseMap(8);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 9X")]
	public static void Generate3DPerlinNoiseMap9X() {
		Generate3DPerlinNoiseMap(9);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 10X")]
	public static void Generate3DPerlinNoiseMap10X() {
		Generate3DPerlinNoiseMap(10);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 11X")]
	public static void Generate3DPerlinNoiseMa11X() {
		Generate3DPerlinNoiseMap(11);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 12X")]
	public static void Generate3DPerlinNoiseMap12X() {
		Generate3DPerlinNoiseMap(12);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 13X")]
	public static void Generate3DPerlinNoiseMap13X() {
		Generate3DPerlinNoiseMap(13);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 14X")]
	public static void Generate3DPerlinNoiseMap14X() {
		Generate3DPerlinNoiseMap(14);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 15X")]
	public static void Generate3DPerlinNoiseMap15X() {
		Generate3DPerlinNoiseMap(15);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 20X")]
	public static void Generate3DPerlinNoiseMap20X() {
		Generate3DPerlinNoiseMap(20);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 30X")]
	public static void Generate3DPerlinNoiseMap30X() {
		Generate3DPerlinNoiseMap(30);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 50X")]
	public static void Generate3DPerlinNoiseMap50X() {
		Generate3DPerlinNoiseMap(50);
	}
	
	[MenuItem("这就是IB/材质工具/生成噪点图/3D Perlin 100X")]
	public static void Generate3DPerlinNoiseMap100X() {
		Generate3DPerlinNoiseMap(100);
	}
	
	public static void GeneratePerlinNoiseMap(float scale) {
		string path = EditorUtility.SaveFilePanelInProject("生成柏林噪点图", "perlin_noise_texture_" + scale + "x", "png", "保存", Application.dataPath + "/Resources/Textures");
		if (!string.IsNullOrEmpty(path)) {
			if (EditorUtility.DisplayCancelableProgressBar("生成柏林噪点图", "初始化", 0f)) {
				ClearProgressBar();
				return;
			}
			
			int size = 256;
			int sizeSqr = size * size;
			float oX = Random.value;
			float oY = Random.value;
			Texture2D texture2D = new Texture2D(size, size);
			for (int i = 0; i < size; i++) {
				for (int j = 0; j < size; j++) {
					float greyScale = Mathf.PerlinNoise(oX + ((float) i) / ((float) size) * scale, oY + ((float) j) / ((float) size) * scale);
					texture2D.SetPixel(i, j, new Color(greyScale, greyScale, greyScale));
					if (j % 100 == 0) {
						if (EditorUtility.DisplayCancelableProgressBar("生成柏林噪点图", string.Empty, (float) (size * i + j + 1) / sizeSqr)) {
							ClearProgressBar();
							return;
						}
					}
				}
			}
			
			texture2D.Apply();
			texture2D.filterMode = FilterMode.Bilinear;
			texture2D.wrapMode = TextureWrapMode.Repeat;
			File.WriteAllBytes(path, texture2D.EncodeToPNG());
			EditorUtility.ClearProgressBar();
			AssetDatabase.ImportAsset(path.Substring(path.IndexOf("Assets")));
		}
	}

	public static void Generate3DPerlinNoiseMap(float scale) {
		string path = EditorUtility.SaveFilePanelInProject("生成3D柏林噪点图", "perlin_noise_3d_texture_" + scale + "x", "asset", "保存", Application.dataPath + "/Resources/Textures");
		if (!string.IsNullOrEmpty(path)) {
			if (EditorUtility.DisplayCancelableProgressBar("生成3D柏林噪点图", "初始化", 0f)) {
				ClearProgressBar();
				return;
			}
			
			int sizeX = 256;
			int sizeY = 256;
			int sizeZ = 128;
			int sizeCubic = sizeX * sizeY * sizeZ;
			float oX = Random.value;
			float oY = Random.value;
			float oZ = Random.value;
			Texture3D texture3D = new Texture3D(sizeX, sizeY, sizeZ, TextureFormat.ARGB32, true);
			Color[] colors = new Color[sizeCubic];
			int index = 0;
			for (int i = 0; i < sizeX; i++) {
				for (int j = 0; j < sizeY; j++) {
					for (int k = 0; k < sizeZ; k++, index++) {
						float greyScale = PerlinNoise3D(oX + ((float) i) / ((float) sizeX) * scale, oY + ((float) j) / ((float) sizeY) * scale, oZ + ((float) k) / ((float) sizeZ) * scale);
						colors[index] = new Color(greyScale, greyScale, greyScale);
						if (j % 100 == 0) {
							if (EditorUtility.DisplayCancelableProgressBar("生成3D柏林噪点图", string.Empty, (float) (sizeX * i + sizeY * j + k + 1) / sizeCubic)) {
								ClearProgressBar();
								return;
							}
						}
					}
				}
			}
			
			texture3D.SetPixels(colors);
			texture3D.Apply();
			texture3D.filterMode = FilterMode.Bilinear;
			texture3D.wrapMode = TextureWrapMode.Repeat;
			EditorUtility.ClearProgressBar();
			AssetDatabase.CreateAsset(texture3D, path.Substring(path.IndexOf("Assets")));
		}
	}

	public static float PerlinNoise3D(float x, float y, float z) {
		float xy = Mathf.PerlinNoise(x, y);
		float yz = Mathf.PerlinNoise(y, z);
		float xz = Mathf.PerlinNoise(x, z);
		
		float yx = Mathf.PerlinNoise(y, x);
		float zy = Mathf.PerlinNoise(z, y);
		float zx = Mathf.PerlinNoise(z, x);

		return (xy + yz + xz + yx + zy + zx) / 6f;
	}

	[MenuItem("这就是IB/关闭进度条")]
	public static void ClearProgressBar() {
		EditorUtility.ClearProgressBar();
	}
}