using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System;
using System.Data;
using System.IO;

public class PrefabTool{
    [MenuItem("程序工具/修改所有预制")]
    static void LoadAllPrefabReplace() {
        LoopAllTran((tran) =>
        {
            tran.name = tran.name + 1;
        });
    }
    public static void LoopAllTran(System.Action<Transform> loop) {
        //string targetPath = Application.dataPath + "/Res/Prefab/UIPrefab/";
        string targetPath = Application.dataPath + "/Res/Prefab/TestUIPrefab/";
        AssetPath.GetAllFileName(targetPath, (fullpath) =>
        {
            if(fullpath.EndsWith(".prefab"))
            {
                string path = fullpath.GetUnityAssetPath();
                Debug.LogError(path);
                GameObject target = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                GameObject newprefab = PrefabUtility.InstantiatePrefab(target) as GameObject;
                loop(newprefab.transform);
                PrefabUtility.ReplacePrefab(target, newprefab, ReplacePrefabOptions.Default);
            }
        });
    }
}
