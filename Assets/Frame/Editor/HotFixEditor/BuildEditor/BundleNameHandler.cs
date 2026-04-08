using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BundleNameHandler
{
    static string dataPath = (Application.dataPath + "/").Replace("\\", "/");
    static bool IsClearBundleName = false;


    //[MenuItem("Test/清除选择目录bundle name")]
    public static void ChooseObject()
    {
        UnityEngine.Object[] arr = Selection.GetFiltered(typeof(UnityEditor.DefaultAsset), SelectionMode.Unfiltered);
        foreach (UnityEngine.Object obj in arr)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            string root = Application.dataPath.Replace("Assets", path);
            SetDirectoryAssetBundleName(root, true);
        }
    }

    /// <summary>
    /// [MenuItem("Test/SetName")]
    /// </summary>
    static public void SetName()
    {
        string root = Application.dataPath + @"\Res\table\";
        SetDirectoryAssetBundleName(root, false);
    }

    //[MenuItem("Test/Empty")]
    static public void Empty()
    {
        string root = Application.dataPath + @"\Res\table\";
        SetDirectoryAssetBundleName(root, true);
    }

    static public void SetDirectoryAssetBundleName(string root, bool isClearBundleName = false, string searchPattern = "")
    {
        if (!Directory.Exists(root))
        {
            Debug.LogError("目录不存在");
            return;
        }

        IsClearBundleName = isClearBundleName;
        string[] files = Directory.GetFiles(root, "*" + searchPattern, SearchOption.AllDirectories);
        foreach (string file in files)
        {
            SetAssetBundleName(file);
        }

        AssetDatabase.RemoveUnusedAssetBundleNames();
        AssetDatabase.Refresh();
        Debug.Log("Set name finish");
    }

    public static void SetFileBundleName(string file, bool isClearBundleName)
    {
        if (!File.Exists(file))
        {
            Debug.LogError("文件不存在:" + file);
            return;
        }

        IsClearBundleName = isClearBundleName;
        SetAssetBundleName(file);
    }

    static void SetAssetBundleName(string file)
    {
        if (file.EndsWith(".meta")) return;
        PathFormat(ref file);

        string unity_path = file.Replace(dataPath, "Assets/");
        string bundle_path = IsClearBundleName ? string.Empty : GetBundlePath(file.Replace(dataPath, ""));

        AssetImporter assetImporter = AssetImporter.GetAtPath(unity_path);
        if (assetImporter.assetBundleName == bundle_path) return;

        assetImporter.assetBundleName = bundle_path;
        //assetImporter.SetAssetBundleNameAndVariant(bundle_path, string.Empty);
    }

    static string GetBundlePath(string path)
    {
        path = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
        PathFormat(ref path);
        return path.ToLower();
    }

    static void PathFormat(ref string path)
    {
        path = path.Replace("\\", "/");
    }

    public static void SetLuaBundleName()
    {
        string root = Application.dataPath + "/Lua/";
        string[] paths = Directory.GetDirectories(root, "*", SearchOption.TopDirectoryOnly);

        foreach(string path in paths)
        {
            string unity_path = path.Replace(dataPath, "Assets/");
            string bundle_path = "res/" + GetBundlePath(path.Replace(dataPath, ""));

            AssetImporter assetImporter = AssetImporter.GetAtPath(unity_path);
            if (assetImporter.assetBundleName == bundle_path) continue;

            assetImporter.assetBundleName = bundle_path;
        }
    }

}
