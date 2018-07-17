using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;
using UG = UnityEditor.EditorGUILayout;

public class InterlocutionDataEditorWindow : EditorWindow {

	private const string DATA_SAVE_PATH = "InterlocutionDataPath";
	
	private Dictionary<string, List<InterlocutionData>> _data;
	private string _dataPath;
	private int _editingSubject;
	private int _editingInterlocution;
	private Vector2 _scrollPostion;

	[MenuItem("这就是IB/数据编辑/编辑问答数据")]
	public static void Open() {
		InterlocutionDataEditorWindow window = GetWindow<InterlocutionDataEditorWindow>(false, "问答编辑器", true);
		window.Init();
	}

	private void Init() {
		_dataPath = EditorPrefs.GetString(DATA_SAVE_PATH, CommonUtils.NULL_STRING);
		if (_dataPath.IsNullPath()) {
			_data = new Dictionary<string, List<InterlocutionData>>();
		} else {
			try {
				_data = JsonConvert.DeserializeObject<Dictionary<string, List<InterlocutionData>>>(File.ReadAllText(_dataPath));
			} catch (IOException) {
				EditorApplication.Beep();
				EditorUtility.DisplayDialog("异常捕获！", "文件" + _dataPath + "读取异常", "知道了");
				_data = new Dictionary<string, List<InterlocutionData>>();
				_dataPath = CommonUtils.NULL_STRING;
			}
		}
		_editingInterlocution = -1;
		_scrollPostion = Vector2.zero;
	}

	public void OnGUI() {
		UG.BeginVertical();

		UG.BeginHorizontal();
		UG.LabelField("当前数据源：" + (_dataPath.IsNullPath() ? "未在本地保存，已编辑内容随时可能丢失！" : _dataPath));
		if (GUILayout.Button("选择")) {
			string path = EditorUtility.OpenFilePanelWithFilters("选择数据源文件", Application.dataPath + "/Hotassets/Data", new []{ "text", "txt" });
			if (!string.IsNullOrEmpty(path)) {
				Dictionary<string, List<InterlocutionData>> temperData = null;
				try {
					temperData =
						JsonConvert.DeserializeObject<Dictionary<string, List<InterlocutionData>>>(
							File.ReadAllText(path));
				} catch (JsonException) {
					temperData = _data;
					path = _dataPath;
					EditorApplication.Beep();
					EditorUtility.DisplayDialog("异常捕获！", "数据源文件不能被正确加载，请确定.json文件的有效性", "知道了");
				} finally {
					_data = temperData;
					_dataPath = path;
				}
			}
		}
		
		UG.EndHorizontal();
		
		UG.BeginHorizontal();
		
		int subject = UG.Popup("所属学科", _editingSubject, InterlocutionData.Keys);
		if (subject != _editingSubject) _editingInterlocution = -1;
		_editingSubject = subject;
		string key = InterlocutionData.Keys[_editingSubject];
		UG.LabelField("分组", _editingSubject != InterlocutionData.Keys.Length - 1 ? Subject.ToSubject(InterlocutionData.Keys[_editingSubject]).group.ToString() : "无");

		UG.EndHorizontal();
		
		_scrollPostion = UG.BeginScrollView(_scrollPostion);

		if (!_data.ContainsKey(key)) {
			_data[key] = new List<InterlocutionData>();
		}

		List<InterlocutionData> interlocutions = _data[key];

		int _deletedInterlocution = -1;

		UG.LabelField("[已有 " + interlocutions.Count + " 道问答]");
		for (int i = 0, l = interlocutions.Count; i < l; i++) {
			
			UG.BeginHorizontal();
			
			UG.LabelField("[Q " + i + "] " + interlocutions[i].question);
			if (GUILayout.Button("编辑")) {
				_editingInterlocution = i;
			}

			if (GUILayout.Button("删除")) {
				EditorApplication.Beep();
				if (EditorUtility.DisplayDialog("危险操作警告⚠️", "即将删除问答 [Q" + i +"] （该操作不可逆）", "确认", "取消")) {
					_deletedInterlocution = i;
					if (_editingInterlocution == _deletedInterlocution) _editingInterlocution = -1;
				}
			}
			
			UG.EndHorizontal();
		}

		if (_deletedInterlocution != -1) {
			interlocutions.RemoveAt(_deletedInterlocution);
		}
		
		UG.EndScrollView();
		
		if (GUILayout.Button("添加问答")) {
			_editingInterlocution = interlocutions.Count;
			interlocutions.Add(new InterlocutionData());
		}
		
		UG.LabelField("问答编辑区");
		if (_editingInterlocution != -1) {
			UG.LabelField("Q " + _editingInterlocution);
			InterlocutionData interlocution = interlocutions[_editingInterlocution];
			interlocution.question = UG.TextArea(interlocution.question);
			interlocution.answer = (Option) UG.EnumPopup("正确选项", interlocution.answer);
			UG.LabelField("选项A");
			interlocution.optionA = UG.TextArea(interlocution.optionA);
			UG.LabelField("选项B");
			interlocution.optionB = UG.TextArea(interlocution.optionB);
			UG.LabelField("选项C");
			interlocution.optionC = UG.TextArea(interlocution.optionC);
			UG.LabelField("选项D");
			interlocution.optionD = UG.TextArea(interlocution.optionD);
			
		} else {
			UG.HelpBox("需要选择一个问答进行编辑！", MessageType.Warning);
		}

		if (GUILayout.Button("保存")) {
			bool toSave = true;
			bool toImport = false;
			if (_dataPath.IsNullPath()) {
				string path = EditorUtility.SaveFilePanel("保存问答数据", Application.dataPath + "/Hotassets/Data", "InterlocutionData", "txt");
				if (string.IsNullOrEmpty(path)) {
					toSave = false;
				} else {
					_dataPath = path;
					toImport = true;
				}
			}

			if (toSave) {
				File.WriteAllText(_dataPath, JsonConvert.SerializeObject(_data));
				EditorPrefs.SetString(DATA_SAVE_PATH, _dataPath);
			}

			if (toImport) {
				AssetDatabase.ImportAsset(_dataPath.Substring(_dataPath.IndexOf("Assets")));
			}
		}
		
		UG.EndVertical();
	}
}