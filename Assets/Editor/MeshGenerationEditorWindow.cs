using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UG = UnityEditor.EditorGUILayout;

public class MeshGenerationEditorWindow : EditorWindow {

	private static Material defaultMaterial;
	private static MeshGenerationType chosenMeshGenerationType = MeshGenerationType.Plane;
	private static PlaneGenerationData planeGenerationData;
	private static VolumetricFluidGenerationData volumetricFluidGenerationData;
	
	[MenuItem("这就是IB/模型工具/编辑网格生成")]
	public static void Open() {
		MeshGenerationEditorWindow editorWindow = GetWindow<MeshGenerationEditorWindow>(false, "网格生成编辑器", true);
		editorWindow.Init();
	}

	private void Init() {
		
	}

	private void OnGUI() {
		UG.BeginVertical();
		
		chosenMeshGenerationType = (MeshGenerationType) UG.EnumPopup("网格生成类型", chosenMeshGenerationType);
		defaultMaterial = UG.ObjectField("默认材质", defaultMaterial, typeof(Material), false) as Material;
		
		switch (chosenMeshGenerationType) {
			case MeshGenerationType.Plane : OnPlaneGenerationGUI(); break;
			case MeshGenerationType.Box : OnBoxGenerationGUI(); break;
			case MeshGenerationType.Slope : OnSlopeGenerationGUI(); break;
			case MeshGenerationType.VolumetricFluid: OnVolumetricFluidGenerationGUI(); break;
			default: UG.HelpBox("需要选择一种网格生成类别进行编辑！", MessageType.Warning); break;
		}
		
		UG.EndVertical();
	}

	private void OnPlaneGenerationGUI() {
		planeGenerationData = planeGenerationData ?? new PlaneGenerationData();

		planeGenerationData.direction = (PlaneGenerationData.Direction) UG.EnumPopup("平面方向", planeGenerationData.direction);

		UG.BeginHorizontal();

		planeGenerationData.length = UG.DelayedFloatField("长", planeGenerationData.length);
		planeGenerationData.width = UG.DelayedFloatField("宽", planeGenerationData.width);
		
		UG.EndHorizontal();

		UG.BeginHorizontal();
		
		planeGenerationData.verticesPerLength = UG.DelayedIntField("长顶点数", planeGenerationData.verticesPerLength);
		planeGenerationData.verticesPerWidth = UG.DelayedIntField("宽顶点数", planeGenerationData.verticesPerWidth);
		
		UG.EndHorizontal();

		bool legal = true;
		string errorMessage = String.Empty;

		if (defaultMaterial == null) {
			legal = false;
			errorMessage += "默认材质不能为空";
		}

		if (planeGenerationData.length <= 0) {
			legal = false;
			errorMessage += "平面长必须大于0\n";
		}
		
		if (planeGenerationData.width <= 0) {
			legal = false;
			errorMessage += "平面宽必须大于0\n";
		}
		
		if (planeGenerationData.verticesPerLength <= 0) {
			legal = false;
			errorMessage += "平面长顶点数必须大于0\n";
		}
		
		if (planeGenerationData.verticesPerWidth <= 0) {
			legal = false;
			errorMessage += "平面宽顶点数必须大于0\n";
		}

		if (legal) {
			if (GUILayout.Button("生成")) MeshTool.InstantiatePlane(planeGenerationData, defaultMaterial);
		} else UG.HelpBox(errorMessage, MessageType.Error);
	}

	private void OnBoxGenerationGUI() {
		
	}

	private void OnSlopeGenerationGUI() {
		
	}

	private void OnVolumetricFluidGenerationGUI() {
		
	}
}