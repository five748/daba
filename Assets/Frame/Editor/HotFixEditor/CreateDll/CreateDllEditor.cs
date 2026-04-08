using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class CreateDllEditor : AssetPostprocessor
{
	[MenuItem("程序工具/编译", false, 0)]
	public static void CreateDllMenuItem() {
		CreateDll();
	}
	public static void CreateDll(System.Action callback = null) {
		return;
		if (Application.isPlaying)
		{
			return;
		}
#if UNITY_IOS
		//int i = Application.dataPath.Length - 6;
		//BranchTool.CMDSHANDLoadAssetToUnity(Application.dataPath.Substring(0, i) + "build.sh", callback, false);
		//File.Copy(Config.ClientPath + "/Temp/bin/Debug/HotFix.Dll", Application.dataPath + "/Res/hotscript" + "/HotFixDll.bytes", true);
		//File.Copy(Config.ClientPath + "Temp/bin/Debug/HotFix.pdb", Application.dataPath + "/Res/hotscript" + "/HotFixPdb.bytes", true);
		//if (callback != null)
		//{
		//	callback();
		//}

#else
		int i = Application.dataPath.Length - 6;
		BranchTool.LoadAssetToUnity(Application.dataPath.Substring(0, i) + "build.bat", BranchTool.GetOnBranch(),  callback, false);
		File.Copy(FrameConfig.ClientPath + "/Temp/bin/Debug/HotFix.Dll", Application.dataPath + "/Res/hotscript" + "/HotFixDll.bytes", true);
		File.Copy(FrameConfig.ClientPath + "Temp/bin/Debug/HotFix.pdb", Application.dataPath + "/Res/hotscript" + "/HotFixPdb.bytes", true);
		if (callback != null)
		{
			callback();
		}
		Debug.Log("编译成功");
#endif
	}
	//static CreateDllEditor()
	//{
	//	EditorApplication.update += Update;
	//}

	//private static void Update()
	//{
	//	if (!EditorApplication.isCompiling)
	//	{
	//		CreateDll();
	//		EditorApplication.update -= Update;
	//	}
	//}
}
