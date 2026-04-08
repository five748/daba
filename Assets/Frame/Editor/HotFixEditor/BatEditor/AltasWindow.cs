using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
[Serializable]
public class AsestData
{
    public string name;
    public string path;
    public bool isCHoose;
    [NonSerialized]
    private Texture _icon;
    public Texture icon
    {
        get
        {
            if (_icon == null)
            {
                _icon = AssetDatabase.GetCachedIcon(path);
            }
            return _icon;
        }
    }
}
[Serializable]
public class AltasDatas : ReadAndSaveTool<AltasDatas>
{
    public static AltasDatas ReadData()
    {
        return Read(tempPath, () => {
            return new AltasDatas();
        });
    }
    public void WriteData()
    {
        Save(Application.dataPath + "/" + tempPath + ".bytes");
    }
    private static string tempPath
    {
        get
        {
            return "EditorData/AltasText";
        }
    }
    public Dictionary<string, Dictionary<string, AsestData>> imageData = new Dictionary<string, Dictionary<string, AsestData>>();
}

public class AltasWindow : EditorWindow
{
    static AltasWindow MyWindow;
    [MenuItem("自定义工具/图集工具")]
    static void BeginDrawWindow()
    {
        MyWindow = (AltasWindow)EditorWindow.GetWindow(typeof(AltasWindow), false, targetName, false);
        MyWindow.Init();
    }
    private static AltasDatas altasDatas;
    private static string targetName = "";
    private AsestData ChoosePrefabData;
    private List<AsestData> prefabDatas = new List<AsestData>();
    private Dictionary<string, AsestData> imageDatas
    {
        get
        {
            if (!altasDatas.imageData.ContainsKey(targetName))
            {
                return new Dictionary<string, AsestData>();
            }
            return altasDatas.imageData[targetName];
        }
    }
    private void Init()
    {

    }
    private void ReadData()
    {
        altasDatas = AltasDatas.ReadData();
    }
    private void SaveData()
    {
        Debug.Log("保存成功");
        altasDatas.WriteData();
    }
    private void InitPrefabName()
    {
        ReadData();
        prefabDatas.Clear();
        (Application.dataPath + "/Res/Prefab/UIPrefab/").GetAllFileName(null, file => {
            if (file.Name.Contains(".prefab"))
            {
                AsestData data = new AsestData();
                data.path = "Assets/Res/Prefab/UIPrefab/" + file.Name;
                data.name = file.Name.Replace(".prefab", "");
                prefabDatas.Add(data);
            }
        });
    }
    private void SetImageDatas()
    {
        if (!altasDatas.imageData.ContainsKey(targetName))
        {
            altasDatas.imageData.Add(targetName, new Dictionary<string, AsestData>());
        }
        var deps = AssetDatabase.GetDependencies(ChoosePrefabData.path);
        foreach (var path in deps)
        {
            if (path.Contains(".png"))
            {
                if (!altasDatas.imageData[targetName].ContainsKey(path))
                {
                    AsestData data = new AsestData();
                    data.path = path;
                    imageDatas.Add(path, data);
                }
            }
        }
    }
    public void OnGUI()
    {
        if (prefabDatas.Count == 0)
        {
            InitPrefabName();
        }
        ShowScroll();
    }
    private void ShowScroll()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width), GUILayout.Height(position.height));
        {
            GUILayout.Space(2f);
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.2f - 16f));
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("PrefabList", EditorStyles.boldLabel);
                if (GUILayout.Button("保存"))
                {
                    SaveData();
                }
                EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                {
                    ShowPrefab();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.78f - 16f));
            {
                GUILayout.Space(5f);
                string prefabName = "xx";
                if (ChoosePrefabData != null)
                {
                    prefabName = ChoosePrefabData.name;
                }
                EditorGUILayout.LabelField(prefabName, EditorStyles.boldLabel);
                EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                {
                    ShowImages();
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            GUILayout.Space(5f);
        }
        EditorGUILayout.EndHorizontal();
    }
    private Vector2 prefabScroll = Vector2.zero;
    private void ShowPrefab()
    {
        prefabScroll = EditorGUILayout.BeginScrollView(prefabScroll);
        {
            int index = -1;
            foreach (var data in prefabDatas)
            {
                index++;
                EditorGUILayout.BeginHorizontal();
                {
                    data.isCHoose = GUI.Toggle(new Rect(5f, 20f * index + 1f, 16f, 16f), data.isCHoose, "");
                    if (data.isCHoose)
                    {
                        if (ChoosePrefabData != null)
                        {
                            if (ChoosePrefabData != data)
                            {
                                ChoosePrefabData.isCHoose = false;
                                ChoosePrefabData = data;
                                targetName = ChoosePrefabData.path;
                                SetImageDatas();
                            }
                        }
                        else
                        {
                            ChoosePrefabData = data;
                            targetName = ChoosePrefabData.path;
                            SetImageDatas();
                        }
                    }
                    GUI.DrawTexture(new Rect(25f, 20f * index + 1f, 16f, 16f), data.icon);
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(36f), GUILayout.Height(18f));
                    EditorGUILayout.LabelField(data.name);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
    }
    private Vector2 imageScroll = Vector2.zero;
    private void ShowImages()
    {
        imageScroll = EditorGUILayout.BeginScrollView(imageScroll);
        {
            int index = -1;
            foreach (var data in imageDatas)
            {
                index++;
                EditorGUILayout.BeginHorizontal();
                {
                    data.Value.isCHoose = GUI.Toggle(new Rect(5f, 20f * index + 1f, 16f, 16f), data.Value.isCHoose, "");
                    GUI.DrawTexture(new Rect(25f, 20f * index + 1f, 16f, 16f), data.Value.icon);
                    EditorGUILayout.LabelField(string.Empty, GUILayout.Width(36f), GUILayout.Height(18f));
                    EditorGUILayout.LabelField(data.Value.path);
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
    }
    void OnDestroy()
    {
        if (EditorUtility.DisplayDialog("", "是否保存修改", "ok", "no"))
        {
            Debug.Log("sure");
            SaveData();
        }
    }
}
