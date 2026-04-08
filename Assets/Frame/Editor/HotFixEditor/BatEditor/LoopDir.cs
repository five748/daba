using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Assertions.Must;
using System.IO;
public class LoopDir{

	[MenuItem("Assets/ChangeFileName")]
	static void ChangeFileName() {

		string[] strs = Selection.assetGUIDs;
		string path = AssetDatabase.GUIDToAssetPath(strs[0]);
		Debug.LogError(path);
		(Application.dataPath.Replace("Assets", "") + path).GetAllFileName(null, file => {
			string oldName = file.Name.Replace(".png", "");
			string newName = (int.Parse(oldName) + 100000).ToString();
			file.MoveTo(file.FullName.Replace(oldName, newName));
		});
	}
	[MenuItem("Assets/ChangeFileBinToByte")]
	static void ChangeFileBinToByte() {
		string[] strs = Selection.assetGUIDs;
		string path = AssetDatabase.GUIDToAssetPath(strs[0]);
		Debug.LogError(path);
		(Application.dataPath.Replace("Assets", "") + path).GetAllFileName(null, file => {
			file.MoveTo(file.FullName.Replace(".bin",".bytes"));
		});
	}
}
