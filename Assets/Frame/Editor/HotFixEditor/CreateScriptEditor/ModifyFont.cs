using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
using System.Collections.Generic;
using System;
using System.Text;
using System.Reflection;
using UnityEngine.UI;

public class ModifyFont
{
    static string TargetName;
    static GameObject Target;
    static Font newFont;
    [MenuItem("程序工具/代码/自动修改所有图片字体")]
    static void modifyFont()
    {
        AssetLoadOld.Instance.LoadFont("FZY4JW", (font) =>
        {
            newFont = font;
        });
        GetFileNamesAndShort("", Application.dataPath + "/Res/Prefab/UIPrefab/", (fullpath, dirpath) =>
        {
            if (fullpath.EndsWith(".prefab"))
            {
                Debug.LogError(dirpath);
                Target = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Res/Prefab/UIPrefab/" + dirpath);
                SetList(Target.transform);
                EditorUtility.SetDirty(Target);
            }
        });
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public static void GetFileNamesAndShort(string begin, string targetPath, System.Action<string, string> finish)
    {
        targetPath = GetPlatformPath(targetPath);
        //获取路径下的所有文件
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        //获取所有子路径
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            GetFileNamesAndShort(begin + info.Name + "/", info.FullName, finish);
        }
        //获取所有子文件
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (finish != null)
            {
                if (info.FullName.IndexOf(".meta") != -1)
                    continue;
                finish(info.FullName, begin + info.Name);
            }
        }
    }
    public static string GetPlatformPath(string path)
    {
        return path.ToLower().Replace(@"\", @"/");
    }
    static void SetList(Transform go)
    {
        for (int i = 0; i < go.childCount; i++)
        {
            Transform target = go.GetChild(i);
            SetNewFont(target);
            //SetFontStyle(target);
            //SetImagePos(target);
            //SetFontSize(target);
            SetList(target);
        
        }
    }
    private static List<string> outRoleStrs = new List<string>() {
        "占领", "军衔", "攻占", "首冲", "血",
        "x","X","扫荡","黑市","殖民","掠夺","等级."
    };
    public static void SetFontSize(Transform myTran)
    {
        var input = myTran.GetComponent<InputField>();
        if (input)
        {
            myTran.GetOrAddComponent<Inputs>();
        }
    }
    [MenuItem("程序工具/代码/提取代码违规字段")]
    public static void LoopScript()
    {
        Application.dataPath.GetAllFileName(null, (file) => {
            if (file.Name.Contains(".meta"))
            {
                return;
            }
            if (!file.Name.Contains(".cs"))
            {
                return;
            }
            file.FullName.ReadStringByLine((line) =>
            {
                line = line.Split('/')[0];
                foreach (var str in outRoleStrs)
                {
                    if (str == "X" || str == "x") {
                        continue;
                    }
                    if (line.Contains(str))
                    {
                        Debug.LogError(file.Name + ":" + line);
                    }
                }

            });
        });
    }
    public static void SetNewFont(Transform myTran)
    {
        //if (myTran.GetComponent<Text>())
        //{
        //    myTran.GetComponent<Text>().font = newFont;
        //}
        GameObjectUtility.RemoveMonoBehavioursWithMissingScript(myTran.gameObject);
        if (myTran.GetComponent<InputField>())
        {
            myTran.GetOrAddComponent<Inputs>();
        }
    }
    static void SetFontStyle(Transform myTran)
    {
        if (myTran.GetComponent<NewText>())
        {
            ComponentMgr.Instance.SetStyle(myTran, myTran.GetComponent<NewText>().menuName);
        }
    }
    static void SetImagePos(Transform myTran)
    {
        var image = myTran.GetComponent<Image>();
        if (image != null && image.sprite == null)
        {
            Debug.LogError(image.name);
        }
    }
    static bool SetTarget()
    {
        Target = GetChooseGameObject();
        if (!Target)
            return false;
        TargetName = Target.name;
        return true;
    }
    static GameObject GetChooseGameObject()
    {
        GameObject[] targets = Selection.gameObjects;
        if (targets.Length == 0)
        {
            Debug.Log("请选择要生成代码的物体");
            return null;
        }
        if (targets.Length > 1)
        {
            Debug.Log("只能选中一个物体");
            return null;
        }
        return targets[0];
    }
}