using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.IO;

public class BatTool
{
    //[MenuItem("Tools/Apply")]
    //public static void ApplyPrefab()
    //{
    //    foreach (var go in Selection.gameObjects)
    //    {
    //        if (go == null) return;
    //        PrefabType type = PrefabUtility.GetPrefabType(go);
    //        if (type == PrefabType.PrefabInstance)
    //        {
    //            Object target = PrefabUtility.GetCorrespondingObjectFromSource(go);
    //            PrefabUtility.ReplacePrefab(go, target, ReplacePrefabOptions.ConnectToPrefab);
    //        }
    //    }
    //}
    [MenuItem("自定义工具/修改文件名字")]
    public static void ChangeFileName()
    {
        //1星冰祭司卡#309905.png
        Dictionary<int, string> propInt = new Dictionary<int, string>();
        (FrameConfig.ArtPath + "Image/ChangeImage_变化/prop_道具表").GetAllFileName(null, flie =>
        {
            if (flie.Name.Contains("#"))
            {
                var strs = flie.Name.Replace(".png", "").Split('#');

                flie.MoveTo(flie.FullName.Replace(strs[0], strs[1]).Replace(strs[1] + ".png", strs[0] + ".png"));
                if (strs[0].IsInt())
                {
                    if (!propInt.ContainsKey(int.Parse(strs[0])))
                    {
                        propInt.Add(int.Parse(strs[0]), strs[1]);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(flie.Name);
                    }
                }
                if (strs[1].IsInt())
                {
                    if (!propInt.ContainsKey(int.Parse(strs[1])))
                    {
                        propInt.Add(int.Parse(strs[1]), strs[0]);
                    }
                    else
                    {
                        UnityEngine.Debug.LogError(flie.Name);
                    }
                }
            }
        });
    }
    [MenuItem("策划工具/清除本地缓存")]
    public static void ClearAllLocalData()
    {
        PlayerPrefs.DeleteAll();
#if DY
        StarkSDKSpace.StarkSDK.API.PlayerPrefs.DeleteAll();
#endif
        Directory.Delete(Application.persistentDataPath, true);
    }
    //[MenuItem("场景/导入模型")]
    public static void ImportSenceModel()
    {

        string srcPath = FrameConfig.ArtPath + "/Sence";
        string destPath = Application.dataPath + "/SceneRes/";
        CopySence(srcPath, destPath);

        AssetDatabase.Refresh();
    }
    public static void CopySence(string srcPath, string destPath)
    {
        DirectoryInfo dir = new DirectoryInfo(srcPath);
        FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //获取目录下（不包含子目录）的文件和子目录
        foreach (FileSystemInfo i in fileinfo)
        {
            string outName = destPath + "\\" + i.Name;
            if (i is DirectoryInfo)     //判断是否文件夹
            {
                if (!Directory.Exists(outName))
                {
                    Directory.CreateDirectory(outName);   //目标目录下不存在此文件夹即创建子文件夹
                }
                CopySence(i.FullName, outName);    //递归调用复制子文件夹
            }
            else
            {
                File.Copy(i.FullName, outName, true);     //不是文件夹即复制文件，true表示可以覆盖同名文件
            }
        }
    }

    /// <summary>
    /// [MenuItem("原画/导入图片")]
    /// </summary>
    public static void ImportPng()
    {
        string srcPath = "D:/Test";
        string destPath = Application.dataPath + "/Test/";
        CopySence(srcPath, destPath);
        AssetDatabase.Refresh();
        var index = 0;
        destPath.GetAllFileName(null, (path) =>
        {
            if (!path.FullName.EndsWith(".png"))
            {
                return;
            }
            var pathName = path.FullName.Replace(@"\", @"/").Replace(destPath, "");
            AssetImporter assetImporter = AssetImporter.GetAtPath("Assets/Test/" + pathName);
            TextureImporter textureImporter = (TextureImporter)assetImporter;
            //UnityEngine.Debug.Log( pathName);
            if (textureImporter.textureType != TextureImporterType.Sprite)
            {
                textureImporter.textureType = TextureImporterType.Sprite;
                AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(assetImporter));
            }
            GameObject tGO = new GameObject();
            tGO.name = pathName;
            tGO.transform.parent = GameObject.Find("UICanvas").transform;
            tGO.AddComponent<UnityEngine.UI.Image>();
            var rect = tGO.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.3f, 0.3f, 1f);
            rect.localPosition = new Vector3(-500 + index * 250, -125, 0);
            if (index == 1)
            {
                index += 2;
            }
            else
            {
                index++;

            }
            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Test/" + pathName);
            var image = tGO.GetComponent<UnityEngine.UI.Image>();
            image.sprite = sprite;
            image.SetNativeSize();
            UnityEngine.Debug.Log(pathName);
        });
    }
    static void SetList(Transform go)
    {
        for (int i = 0; i < go.childCount; i++)
        {
            if (go.gameObject.layer != LayerMask.NameToLayer("UI"))
            {
                go.gameObject.layer = LayerMask.NameToLayer("UI");
            }
            Transform target = go.GetChild(i);
            SetLayer(target);
            SetList(target);
        }
    }
    static void SetLayer(Transform target)
    {
        if (target.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            target.gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }


    //[MenuItem("美术工具/修改特效层级")]
    public static void ChangeEffectAllMat()
    {
        string path = Application.dataPath + "/Res/effect/texture";
        path.GetAllFileName(null, (flie) =>
        {
            if (!flie.Name.Contains(".mat"))
            {
                return;
            }
            //UnityEngine.Debug.LogError(flie.Name);
            string effectPath = flie.FullName.Replace(@"\", @"/").Replace(Application.dataPath, "Assets");
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(effectPath);
            if (mat == null)
            {
                UnityEngine.Debug.LogError("找不到:" + effectPath);
                return;
            }
            mat.renderQueue = 3001;
            var assetImporter = AssetImporter.GetAtPath(effectPath);
            if (assetImporter)
            {
                assetImporter.SaveAndReimport();
            }
        });
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    [MenuItem("程序工具/设置无限制帧数")]
    public static void SetFramFree()
    {
        Application.targetFrameRate = -1;
    }
}
