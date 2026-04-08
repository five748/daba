using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SceneCheckoutEditor
{
    [MenuItem("程序工具/检查/检查场景")]
    static void CreateAllUIScript()
    {
        if (!Application.isPlaying) {
           // EditorApplication.ExecuteMenuItem("Edit/Play");
            EditorUtility.DisplayDialog("", "运行中才能检查!", "ok");
            return;
        }
    }
    private static List<GameObject> GetFilePrefab(string path, string assetPath) {
        List<GameObject> gos = new List<GameObject>();
        CreateScrite.GetFileNamesAndShort("", path, (fullpath, dirpath) => {
            if (dirpath.Contains(".prefab"))
            {
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath + dirpath);
                gos.Add(go);
            }
        });
        return gos;
    }
}
