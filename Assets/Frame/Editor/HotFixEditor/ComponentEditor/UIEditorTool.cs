using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Linq;

[InitializeOnLoad]
[CanEditMultipleObjects]
public class UIEditorTool : UnityEditor.Editor
{
    private static GameObject CreateUI(string name, GameObject parent = null)
    {
        GameObject UIGo = null;
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/UIExample/" + name + ".prefab");
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return null;
        }
        if (parent == null)
        {
            UIGo = GameObject.Instantiate(go, targets[0].transform);
        }
        else
        {
            UIGo = GameObject.Instantiate(go, parent.transform);
        }
        UIGo.name = name;

        return UIGo;
    }
    [MenuItem("GameObject/拼界面/CreateUIDemo", false, 1)]
    public static void CreateDemo()
    {
        CreateUI("UIDemo");
    }
    [MenuItem("GameObject/拼界面/CreateUIPopDemo", false, 1)]
    public static void CreatePopDemo()
    {
        CreateUI("UIPopDemo");
    }
    [MenuItem("GameObject/拼界面/Image", false, 2)]
    public static void CreateImage()
    {
        CreateUI("image");
    }
    [MenuItem("GameObject/拼界面/Btn", false, 2)]
    public static void CreateBtn()
    {
        CreateUI("btn");
    }
    [MenuItem("GameObject/拼界面/BtnText", false, 2)]
    public static void CreateBtnText()
    {
        CreateUI("btn_txt");
    }
    [MenuItem("GameObject/拼界面/Text", false, 3)]
    public static void CreateText()
    {
        CreateUI("text");
    }
    [MenuItem("GameObject/拼界面/Input", false, 2)]
    public static void CreateInput()
    {
        CreateUI("input");  
    }
    [MenuItem("GameObject/拼界面/Slider", false, 4)]
    public static void CreateSlider()
    {
        CreateUI("slider");
    }
    [MenuItem("GameObject/拼界面/Scroll", false, 5)]
    public static void CreateScroll()
    {
        CreateUI("scroll");
    }
    [MenuItem("GameObject/拼界面/MenuH", false, 6)]
    public static void CreateMenuH()
    {
        CreateUI("menugridH");
    }
    [MenuItem("GameObject/拼界面/MenuV", false, 7)]
    public static void CreateMenuV()
    {
        CreateUI("menugridV");
    }
    [MenuItem("GameObject/UI/NewBg", false, 7)]
    public static void CreateTipState() {
        CreateUI("newbg");
    }
    [MenuItem("GameObject/拼界面/组件-SetImageChange", false, 7)]
    public static void CreateByImageChangeName()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return;
        }
        var target = targets[0];
        string imagePath = Application.dataPath + "/Res/Image/ChangeImage/" + target.name;
        bool ok = false;
        imagePath.GetAllFileName(null, file =>
        {
            if (!ok)
            {
                var imageName = file.Name;
                Transform tran = target.transform;
                var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Res/Image/ChangeImage/" + target.name + "/" + imageName);
                var image = tran.GetComponent<UnityEngine.UI.Image>();
                image.sprite = sprite;
                image.SetNativeSize();
                ok = true;
            }

        });

    }

    [MenuItem("GameObject/拼界面/组件-SetImageHalf", false, 7)]
    public static void SetImageHalf()
    {
        GameObject[] targets = Selection.gameObjects;
        Debug.Log(targets.Length);
        foreach (var item in targets)
        {
            var image = item.GetComponent<NewImage>();
            string result = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(image.sprite));
            var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Res/Image/All/" + result);
            DestroyImmediate(image);
            var half = item.AddComponent<HalfImage>();
            half.sprite = sprite;
            half.SetNativeSize();

        }
    }


    [MenuItem("GameObject/拼界面/git-导入最新5张图片", false, 7)]
    public static void ImportArtGit()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return;
        }
        var target = targets[0];
        ImageImport.ImportNewImage(target);
    }

    [MenuItem("GameObject/拼界面/git-导入最新5张图片", false, 7)]
    public static void ImportArtGitByDir()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return;
        }
        var target = targets[0];
        ImageImport.ImportNewImage(target);
    }



    [MenuItem("GameObject/拼界面/结构-复制名字", false, 7)]
    public static void CopyName()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length == 0)
            return;

        GUIUtility.systemCopyBuffer = string.Join("\n", targets.ToList().Select(r =>
        {
            Debug.Log(r);
            var image = r.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
            {
                return "tran.SetImage(\"\",\"name\" );".Replace("name", r.name);
            }
            return "tran.SetText(\"\",\"name\" );".Replace("name", r.name);
        }).ToArray());


    }


    [MenuItem("GameObject/拼界面/结构-调整路径到All", false, 7)]
    public static void SetImagePathAtAll()
    {
        GameObject[] targets = Selection.gameObjects;
        Debug.Log(targets.Length);
        foreach (var item in targets)
        {
            var image = item.GetComponent<NewImage>();
            if (image != null)
            {
                string result = System.IO.Path.GetFileName(AssetDatabase.GetAssetPath(image.sprite));
                var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Res/Image/All/" + result);
                image.sprite = sprite;
                image.SetNativeSize();
                Debug.Log(result);
            }

        }
    }
    [MenuItem("GameObject/拼界面/结构-CreateAll", false, 7)]
    public static void CreateByUIName()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            return;
        }
        var target = targets[0];
        // string imagePath = Application.dataPath + "/Res/Image/UIImage/" + target.name;
        string artPath = FrameConfig.ArtPath + "/Auto/prefabImages.txt";
        string[] fileNames = null;
        artPath.ReadStringByLine(line =>
         {
             var arr = line.Split('$');
             if (arr[0] == target.name)
             {
                 fileNames = arr[1].Split(',');
             }
         });

        Dictionary<string, GameObject> cMap = new Dictionary<string, GameObject>();
        Dictionary<string, int> cIndexMap = new Dictionary<string, int>();

        fileNames.Foreach((index, imagePath) =>
        {
            var entity = imagePath.Split('|');
            var group = entity[0];
            string dir = entity[1].Split('/')[0];
            var cName = entity[2];
            var imageName = entity[1].Split('/')[1];
            GameObject tGO;
            if (!cMap.TryGetValue(group, out tGO))
            {
                tGO = new GameObject(group);
                tGO.transform.parent = target.transform;

                tGO.name = group == "" ? "公用" : group;
                cMap[group] = tGO;
                cIndexMap[group] = cIndexMap.Count;
            }

            Create(dir, imageName, cName, tGO, index);
        });

    }

    public static void Create(string dir, string imageName, string name, GameObject target, int index)
    {
        var sprite = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Res/Image/" + dir + "/" + imageName);
        var args = imageName.Split('_');
        var componentType = args[0] == "Common" ? args[1] : args[0];
        Transform tran;
        switch (componentType)
        {
            //case "Btn":
            //    tran = CreateUI("btn").transform;
            //    tran.name = imageName.Replace(".png", "");
            //    var btnImage = tran.GetComponent<UnityEngine.UI.Image>();
            //    btnImage.sprite = sprite;
            //    btnImage.SetNativeSize();
            //    break;
            default:
                tran = CreateUI("image", target).transform;
                tran.name = name;
                var image = tran.GetComponent<UnityEngine.UI.Image>();
                image.sprite = sprite;
                image.SetNativeSize();
                break;
        }

        tran.localPosition = new Vector3(-500 + componentType.Length * 200, 0, 0);
    }
    [MenuItem("GameObject/获取路径", false, 100)]
    public static void GetPath()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length != 1)
        {
            Debug.LogError("请选中一个物体!");
            return;
        }
        var target = targets[0];
        var str = GetTargePath(target.transform).Substring(1);
        GUIUtility.systemCopyBuffer = str;
        Debug.LogError(str);
    }
    static string GetTargePath(Transform myTran)
    {
        if (myTran.parent == null || myTran.parent.name == "Canvas")
            return "";
        return GetTargePath(myTran.parent) + "/" + myTran.name;
    }
}
