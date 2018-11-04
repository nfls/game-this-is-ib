using System.IO;
using UnityEditor;

public static class DataTool {

	[MenuItem("这就是IB/本地数据/删除玩家存档")]
	public static void ClearLocalPlayerData() {
		EditorApplication.Beep();
		if (!EditorUtility.DisplayDialog("危险操作警告⚠️", "即将删除本地玩家存档（该操作不可逆）", "确认", "取消")) return;
		if (File.Exists(PlayerManager.PlayerDataPath)) File.Delete(PlayerManager.PlayerDataPath);
	}
}