using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class BuildEditorWindow : EditorWindow
{
    static BuildEditorWindow MyWindow;
    [MenuItem("程序工具/打包工具")]
    static void BeginDrawWindow() {
        SDKConfigSet.SetGame();
        MyWindow = (BuildEditorWindow)EditorWindow.GetWindow(typeof(BuildEditorWindow), false, "打包工具", false);
        MyWindow.Init();
    }
    private void Init() {
        buildSet = null;
    }
    public GUISkin mySkyle;
    public Vector2 scrollMaxSize = new Vector2(10000, 10000);
    public Vector2 scrollPos = Vector2.zero;
    string androidNearingPath;
    string iosNearingPath;
    public void OnGUI() {
        if (mySkyle == null)
        {
            mySkyle = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Script/Editor/HotFixEditor/SceneEditor/SkillSkin.guiskin");
        }
        BeginWindows();
        ShowSet();
        EndWindows();
    }
    private BuildCache buildSet;
    private string[] altasSets = { "压缩图片", "已压过" };
    private string[] abNameSets = { "设置名字", "已设过" };
    private string[] hotSets = { "打新AB", "使用已有", "打变化"};
    private string[] buildSets = { "压缩图片", "设置名字", "单打资源", "打资源", "打apk", "上传Ab", "一键打包"};
    private bool isRuning = false;
    public void ShowSet()
    {
        InitBuildCache();
        GUILayout.Space(10);
        SetVersion();
        GUILayout.Space(10);
        SetAdId();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        buildSet.altasIndex = GUILayout.Toolbar(buildSet.altasIndex, altasSets, GUILayout.Width(120f));
        GUILayout.Space(10);
        buildSet.abNameIndex = GUILayout.Toolbar(buildSet.abNameIndex, abNameSets, GUILayout.Width(120f));
        GUILayout.Space(10);
        buildSet.hotIndex = GUILayout.Toolbar(buildSet.hotIndex, hotSets, GUILayout.Width(200f));
        if (buildSet.hotIndex == 2) {
            StyleCenter.ShowMenuButtonLayout(buildSet.difUseDefaltChannel, keys, (str) => {
                buildSet.difUseDefaltChannel = str;
            }, GUILayout.Width(100f));
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        buildSet.buildIndex = GUILayout.Toolbar(buildSet.buildIndex, buildSets, GUILayout.Width(500f));
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        SetChannel();
        GUILayout.Space(10);
        SetChannelSDK();
        GUILayout.Space(10);
        if (GUILayout.Button("运行")) {
            isRuning = true;
            SaveBuildSet();
            if (buildSet.buildIndex == 0) {
                SetImage(() => {
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 1)
            {
                SetAssetBundleName(() => {
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 2) {
                BuildAB(() => {
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 3)
            {
                OneKeyAb(() => {
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 4)
            {
                BuildAPK(() => {
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 5)
            {
                UpLoad(() =>{
                    isRuning = false;
                });
            }
            if (buildSet.buildIndex == 6)
            {
                OneKeyApk(() => {
                    isRuning = false;
                });
            }
        }
    }
    private List<string> keys;
    private List<string> androidKeys;
    private void InitBuildCache() {
        if (buildSet == null || buildSet.channels == null) {
            buildSet = BuildCache.ReadData();
            InitChannel();
            InitChannelSDK();
            androidNearingPath = AssetPath.ExportBundlePath(true);
            iosNearingPath = AssetPath.ExportBundlePath(false);
        }   
    }
    private void SaveBuildSet() {
        if (buildSet != null)
        {
            buildSet.WriteData();
        }
    }
    private Dictionary<string, bool> channnelOs = new Dictionary<string, bool>();
    private void InitChannel()
    {
        if (buildSet.channels == null)
        {
            buildSet.channels = new Dictionary<string, bool>();
        }
        channnelOs.Clear();
        var tab = TableRead.Instance.ReadTable<Table.channel>("channel", true);
        androidKeys = new List<string>();
        foreach (var item in tab)
        {
            var key = item.Key + "_" + item.Value.name;
            if (!buildSet.channels.ContainsKey(key))
            {
                buildSet.channels.Add(key, false);
            }
            channnelOs.Add(key, item.Value.OsType == "1");
            if (!channnelOs[key])
            {
                androidKeys.Add(key);
            }
        }
        keys = new List<string>(buildSet.channels.Keys);
    }
    public void SetChannel() {
        GUILayout.BeginHorizontal();
        for (int i = 0; i < keys.Count; i++)
        {
            if (i != 0 && i != keys.Count - 1 && i % 4 == 0)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
            }
            var key = keys[i];
            buildSet.channels[key] = GUILayout.Toggle(buildSet.channels[key], key, GUILayout.Width(200));
        }
        GUILayout.EndHorizontal();
    }
    private void InitChannelSDK()
    {
        var tab = TableRead.Instance.ReadTable<Table.channelTB>("channelTB", true);
        buildSet.channelSDKs = new string[tab.Count];

        int index = -1;
        foreach (var item in tab)
        {
            index++;
            var key = item.Key + "_" + item.Value.channel;
            buildSet.channelSDKs[index] = key;
        }
    }
    public void SetChannelSDK()
    {
        GUILayout.BeginHorizontal();
        buildSet.channelId = GUILayout.Toolbar(buildSet.channelId, buildSet.channelSDKs, GUILayout.Width(500f));
        GUILayout.EndHorizontal();
    }
    private void SetVersion() {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("apk版本:", GUILayout.Width(50f));
        StyleCenter.ShowMenuButtonLayout(buildSet.apkVersion, androidKeys, Event.current, GUI.skin.textField, (str) =>
        {
            buildSet.apkVersion = str;
        }, GUILayout.Width(150));
        EditorGUILayout.LabelField("资源版本:", GUILayout.Width(50f));
        buildSet.resVersion = EditorGUILayout.IntField(buildSet.resVersion, GUILayout.Width(60f));
        EditorGUILayout.LabelField("资源文件名:", GUILayout.Width(50f));
        buildSet.resName = EditorGUILayout.TextField(buildSet.resName, GUILayout.Width(60f));
        EditorGUILayout.LabelField("AB是否复制到unity:", GUILayout.Width(110f));
        buildSet.isNeedCopyToUnity = EditorGUILayout.Toggle(buildSet.isNeedCopyToUnity);
        GUILayout.EndHorizontal();
    }
    private void SetAdId()
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("抖音广告Id:", GUILayout.Width(50f));
        buildSet.DYAdId = EditorGUILayout.TextField(buildSet.DYAdId, GUILayout.Width(200f));
        EditorGUILayout.LabelField("微信广告Id:", GUILayout.Width(50f));
        buildSet.WXAdId = EditorGUILayout.TextField(buildSet.WXAdId, GUILayout.Width(200f));
        GUILayout.EndHorizontal();
    }
    public void SetImage(System.Action callback = null) {
        if (buildSet.altasIndex == 0)
        {
            Debug.LogError("设置图片需耗费很长时间");
            if (!EditorUtility.DisplayDialog("", "设置图片需耗费很长时间，是否继续?", "ok", "no"))
            {
                return;
            }
            AltasTool.SetAltasAndImage();
            if (callback != null)
                callback();
        }
        else {
            if (callback != null)
                callback();
        }
    }
    public void SetAssetBundleName(System.Action callback = null) {
        Build.SetAssetBundleName(false);
        if (callback != null)
            callback();
    }
    public void BuildAB(System.Action callback = null) {
        if (buildSet.hotIndex == 1) {
            return;
        }
        BuildAbBase(callback);
    }
    private void BuildAbBase(System.Action callback = null)
    {
        bool isHaveAndroid = false;
        bool isHaveIos = false;
        CreateDllEditor.CreateDll();
        Build.SetAssetBundleName(false);
        foreach (var item in buildSet.channels)
        {
            if (item.Value)
            {
                if (channnelOs.ContainsKey(item.Key))
                {
                    if (channnelOs[item.Key])
                    {
                        isHaveIos = true;
                    }
                    else
                    {
                        isHaveAndroid = true;
                    }
                }
            }
        }
        bool isUseDif = buildSet.hotIndex == 2;
        if (!isUseDif)
        {
            if (isHaveAndroid)
                ClearOnePath(androidNearingPath);
            if (isHaveIos)
                ClearOnePath(iosNearingPath);
        }
        else
        {
            if (!buildSet.difUseDefaltChannel.Contains("_"))
            {
                if (EditorUtility.DisplayDialog("", "请选择变化蓝本!", "ok"))
                {
                    return;
                }
                return;
            }
            else
            {
                CopyChannelToNearing(buildSet.difUseDefaltChannel, isHaveAndroid, isHaveIos);
            }
        }

            ClearPath(isHaveAndroid, isHaveIos);
            if (isHaveAndroid)
            {
                Build.BuildByUnSetName(buildSet.resVersion, false, true);
            }
            if (isHaveIos)
            {
                Build.BuildByUnSetName(buildSet.resVersion, false, false);
            }
            foreach (var item in buildSet.channels)
            {
                if (item.Value)
                {
                    if (channnelOs[item.Key])
                    {

                        AssetPath.CopyDirectory(iosNearingPath, iosNearingPath.Replace("nearing", item.Key));
                    }
                    else
                    {
                        AssetPath.CopyDirectory(androidNearingPath, androidNearingPath.Replace("nearing", item.Key));
                        ClearOnePath(androidNearingPath.Replace("nearing", "asset"));
                        AssetPath.CopyDirectory(androidNearingPath, androidNearingPath.Replace("nearing", "asset"), ".manifest");
                    }
                }
            }
        if (buildSet.isNeedCopyToUnity)
        {

            AssetPath.CopyDirectory(androidNearingPath, Application.streamingAssetsPath + "/asset/", ".manifest");
#if UNITY_IOS
                AssetPath.CopyDirectory(iosNearingPath, Application.streamingAssetsPath + "/asset/", ".manifest");
#endif

        }
            AssetDatabase.Refresh();
            if (callback != null)
                callback();
    }
    private void CopyChannelToNearing(string channel, bool isHaveAndorid, bool isHaveIOS) {
        if (isHaveAndorid) {
            ClearOnePath(androidNearingPath);
            AssetPath.CopyDirectory(androidNearingPath.Replace("nearing", channel), androidNearingPath);
        }
        if (isHaveIOS) {
            ClearOnePath(iosNearingPath);
            AssetPath.CopyDirectory(iosNearingPath.Replace("nearing", channel), iosNearingPath);
        }
    }
   
    private void ClearPath(bool isHaveAndroid, bool isHaveIOS) {
        if (buildSet.isNeedCopyToUnity)
            ClearOnePath(Application.streamingAssetsPath + "/asset/");
        foreach (var item in buildSet.channels)
        {
            if (item.Value)
            {
                if (isHaveAndroid)
                {
                    ClearOnePath(androidNearingPath.Replace("nearing", item.Key));
                }
                if (isHaveIOS) {
                    ClearOnePath(iosNearingPath.Replace("nearing", item.Key));
                }
            }
        }
    }
    private void ClearOnePath(string path) {
        if (Directory.Exists(path))
        {
            DeleteFolder(path);
        }
        else {
            Directory.CreateDirectory(path);
        }
    }
    public void DeleteFolder(string dir)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                File.Delete(d);//直接删除其中的文件 
            }
            else
            {
                DirectoryInfo d1 = new DirectoryInfo(d);
                Directory.Delete(d, true);
            }
        }
    }
    public void OneKeyAb(System.Action callback = null) {
        SetImage(() => {
            SetAssetBundleName(() => {
                BuildAB(callback);
            });
        });
    }
    public void OneKeyApk(System.Action callback = null) {
        SetImage(() => {
            SetAssetBundleName(() => {
                BuildAbBase(() => {
                    BuildAPK(callback);
                });
            });
        });
    }
    public void CopyToChannel() {
        
    }
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();
        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }
    public void BuildAPK(System.Action callback = null) {
        string apkName = buildSet.apkVersion + ".apk";
        string nearPath = FrameConfig.ClintFatherPath + "apk/Nearing1/";
        string apkVersionPath = FrameConfig.ClintFatherPath + "apk/" + buildSet.apkVersion + "/";
        ClearOnePath(nearPath);
        ClearOnePath(apkVersionPath);
        BuildPipeline.BuildPlayer(GetBuildScenes(), apkName, BuildTarget.Android, BuildOptions.None);
        Debug.LogError(Application.dataPath.Replace("Assets", apkName));
        File.Move(Application.dataPath.Replace("Assets", apkName), nearPath + apkName);
        File.Copy(nearPath + apkName, apkVersionPath + apkName);
        if (callback != null) {
            callback();
        }
    }
    public void UpLoad(System.Action callback = null) {
        foreach (var item in buildSet.channels)
        {
            if (item.Value)
            {
                UploadTool.Upload(buildSet.resVersion, androidNearingPath.Replace("nearing", item.Key), item.Key.Split('_')[0]);
            }
        }
        if (callback != null) {
            callback();
        }
    }
    void OnDestroy()
    {
        if (EditorUtility.DisplayDialog("", "是否保存设置?", "ok", "no"))
        {
            SaveBuildSet();
            return;
        }
    }
}
