using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(MakeGuide))]
[ExecuteInEditMode]
public class MakeGuideEditor : Editor
{

    string filePath;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("----------------------------------------------------------");
        EditorGUILayout.LabelField("文件路径：");
        EditorGUILayout.TextField(filePath);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存"))
        {
            Save();
        }
        EditorGUILayout.EndHorizontal();
    }

    string hierarchyPath;
    string GetHierarchyPath(Transform t, bool initPath = true)
    {
        if (initPath) hierarchyPath = "";
        hierarchyPath = t.name + hierarchyPath;
        if (t.parent.name != "UICanvas")
        {
            Transform parent = t.parent;
            hierarchyPath = "/" + hierarchyPath;
            GetHierarchyPath(parent, false);
        }
        return hierarchyPath;
    }

    void Save()
    {
        if (string.IsNullOrEmpty(filePath))//创建一个新文件并保存
        {
            string path = EditorUtility.SaveFilePanel("Save", Application.dataPath, "", "txt");
            File.WriteAllText(path, GetInspectorInfo());
        }
        else//直接保存在读取的文件
        {
            File.WriteAllText(filePath, GetInspectorInfo());
        }
        AssetDatabase.Refresh();
        Debug.Log("保存成功");
    }

    string GetInspectorInfo()
    {
        string content = "";
        MakeGuide makeGuide = (MakeGuide)target;
        List<GuideUI> guideList = makeGuide.guideList;
        foreach (GuideUI guideUI in guideList)
        {
            content += guideUI.desc + "_" + guideUI.descPosition + "_" + guideUI.HandPosition + "_" + guideUI.HandRotation + "_" + guideUI.ArrowNum + "_"
                + guideUI.ArrowPosition + "_" + guideUI.ArrowRotation + "\r\n";
        }
        return content;
    }

}
