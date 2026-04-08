using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using Debug = UnityEngine.Debug;

public class SDKSettingEditor : EditorWindow
{
    static SDKSettingEditor MyWindow;
    // [MenuItem("鱼子SDK/SDK设置")]
    static void BeginDrawWindow()
    {
        MyWindow = (SDKSettingEditor)EditorWindow.GetWindow(typeof(SDKSettingEditor), false, "SDK设置", false);
        MyWindow.Init();
    }
    private void Init()
    {
       
    }
    [HideInInspector]
    public GUISkin mySkyle;
    public Vector2 scrollMaxSize = new Vector2(10000, 10000);
    public Vector2 scrollPos = Vector2.zero;
    string androidNearingPath;
    string iosNearingPath;
    public SdkCache sdkCache;
    public string sdk_path;//sdk根目录
    public SdkConfig sdkConfig {
        get {
            return sdkCache.OnSDKConfig;
        }
    }
    public void OnGUI()
    {
        if (mySkyle == null)
        {
            mySkyle = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/BaseFrame/Frame/Editor/HotFixEditor/BuildEditor/SkillSkin.guiskin");
        }
        sdk_path = PlayerPrefs.GetString("SDK_ROOT_PATH");
        ReadData();
        BeginWindows();
        ShowSet();
        EndWindows();
    }
   
    private void ShowSet() {
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("当前Sdk:" + Enum.GetName(typeof(SDKType),sdkCache.ChooseSDK),GUILayout.Width(100f));
        if (GUILayout.Button("更换sdk"))
        {
            ShowSdkMenu();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("SDK根目录:" + sdk_path, GUILayout.Width(200f));
        if (GUILayout.Button("选择目录"))
        {
            SelectSDKRootPath();
        }
        if (GUILayout.Button("克隆git"))
        {
            CloneGit();
        }
        if (GUILayout.Button("更新git"))
        {
            PullSdk();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(40);

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("编辑器上报地址:", GUILayout.Width(100f));
        sdkConfig.Url_Report_Editor = GUILayout.TextArea(sdkConfig.Url_Report_Editor, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("手机上报地址:", GUILayout.Width(100f));
        sdkConfig.Url_Report_Phone = GUILayout.TextArea(sdkConfig.Url_Report_Phone, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("AppName:", GUILayout.Width(100f));
        sdkConfig.app_name = GUILayout.TextArea(sdkConfig.app_name, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("appId:", GUILayout.Width(100f));
        sdkConfig.appId = GUILayout.TextArea(sdkConfig.appId, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Ios广告id:", GUILayout.Width(100f));
        sdkConfig.ios_AdId = GUILayout.TextArea(sdkConfig.ios_AdId, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Andorid广告id:", GUILayout.Width(100f));
        sdkConfig.android_AdId = GUILayout.TextArea(sdkConfig.android_AdId, mySkyle.textArea);
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        if (GUILayout.Button("保存设置"))
        {
            SaveSdk();
        }
        GUILayout.Space(20);
        
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("应用sdk"))
        {
            AppalySdk();
        }
        GUILayout.EndHorizontal();

    }
    private void ShowSdkMenu()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Ohayoo"), false, ChangeSdk, SDKType.Ohayoo);
        menu.AddItem(new GUIContent("微信小游戏"), false, ChangeSdk, SDKType.WX);
        menu.ShowAsContext();
    }

    private void ChangeSdk(object sdkType)
    {
        SDKType sdk = (SDKType)sdkType;
        if (sdk == SDKType.Ohayoo)
        {
            Debug.Log("ohayoo");
        }else if (sdk == SDKType.WX)
        {
            Debug.Log("wx");
        }
        if (!sdkCache.allSdk.ContainsKey(sdk))
        {
            sdkCache.allSdk.Add(sdk,new SdkConfig());
        }
        sdkCache.ChooseSDK = sdk;
    }

    private void SaveSdk()
    {
        sdkCache.WriteData();
        SetDefine(sdkCache.ChooseSDK);
        Debug.Log("sdk设置保存成功");
    }

    private void SelectSDKRootPath()
    {
        string searchPath = EditorUtility.OpenFolderPanel("select path", sdk_path, "");

        if (searchPath != "")
        {
            // searchPath = ChangePath(searchPath);
            PlayerPrefs.SetString("SDK_ROOT_PATH", searchPath);
            PlayerPrefs.Save();
            sdk_path = searchPath;
            Debug.Log(searchPath);
        }
    }

    private void CloneGit()
    {
        var cmd = new CMDTool(sdk_path.Replace("/","\\"));
        cmd.Run("git", "clone ssh://git@git.yuzi-game.com:22422/frame/sdk.git YuZiSDK");
        Debug.Log("sdk克隆成功");
        //
        // string command = $"git clone ssh://git@git.yuzi-game.com:22422/frame/sdk.git YuZiSDK";
        // Debug.Log("执行:"+command);
        // var processInfo = new ProcessStartInfo()
        // {
        //     FileName = "cmd.exe",
        //     Arguments = $"/C {command}",
        //     CreateNoWindow = false,
        //     UseShellExecute = false,
        //     RedirectStandardOutput = true,
        //     WorkingDirectory = sdk_path
        // };
        //
        // using (var process = new Process())
        // {
        //     process.StartInfo = processInfo;
        //     process.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        //     process.Start();
        //     process.BeginOutputReadLine();
        //     process.WaitForExit();
        //     Debug.Log("sdk克隆成功");
        // }
    }
    //拉取sdk
    private void PullSdk()
    {
        var cmd = new CMDTool(sdk_path.Replace("/","\\") + "\\YuZiSDK");
        cmd.Run("git", "fetch --all");
        cmd.Run("git", "reset --hard origin/master");
        Debug.Log("sdk更新成功");

        // string command = $"pull";
        // Debug.Log("执行:"+command);
        // var psi = new ProcessStartInfo()
        // {
        //     FileName = "git.exe",
        //     Arguments = $"/C {command}",
        //     // Arguments = command,
        //     CreateNoWindow = false,
        //     UseShellExecute = false,
        //     RedirectStandardOutput = true,
        //     WorkingDirectory = sdk_path.Replace("/","\\") + "\\YuZiSDK"
        // };
        // psi.RedirectStandardInput = true;
        // psi.RedirectStandardOutput = true;
        // psi.RedirectStandardError = true;
        //
        //
        //
        // using (var p = new Process())
        // {
        //     p.StartInfo = psi;
        //     p.OutputDataReceived += (sender, e) => Console.WriteLine(e.Data);
        //     p.Start();
        //     p.BeginOutputReadLine();
        //     p.BeginErrorReadLine();
        //     p.WaitForExit();
        //     Debug.Log("sdk更新成功");
        // }
    }

    //应用sdk
    private void AppalySdk()
    {
        if(EditorUtility.DisplayDialog("", "是否应用SDK", "ok", "no"))
        {
            DeleteSdk();
            CopySdk();
            CopyXml();
            Close();
            SetDefine(sdkCache.ChooseSDK);
            AssetDatabase.Refresh();
            Debug.Log("Sdk应用成功");
        }
    }

    private void DeleteSdk()
    {
        //删除原sdk代码
        string dest = Application.dataPath + "\\YuZiSdk";
        // string dest = "D:\\Unity\\SDKTest\\YuZiSdk";
        dest = dest.Replace("/", "\\");

        if (!Directory.Exists(dest))
        {
            return;
        }
        new DirectoryInfo(dest).Delete(true);
        //删除ohayoo sdk包
        dest = Application.dataPath + "\\Plugins\\ByteGame";
        if (!Directory.Exists(dest))
        {
            return;
        }
        new DirectoryInfo(dest).Delete(true);
    }

    private void CopySdk()
    {
        string sdkName = Enum.GetName(typeof(SDKType), sdkCache.ChooseSDK);
        string target = $"{sdk_path}\\YuZiSdk\\Assets\\YuZiSdk\\{sdkName}";
        target = target.Replace("/", "\\");
        string dest = Application.dataPath + "\\YuZiSdk\\" + sdkName;
        // string dest = "D:\\Unity\\SDKTest\\YuZiSdk\\" + sdkName;
        CopyFolder(target, dest);

        if (sdkCache.ChooseSDK == SDKType.Ohayoo)
        {
            target = $"{sdk_path}\\YuZiSdk\\Assets\\Plugins\\ByteGame";
            dest = Application.dataPath + "\\Plugins\\ByteGame";
            CopyFolder(target, dest);
        }
        
    }

    private void CopyXml()
    {
        string sdkName = Enum.GetName(typeof(SDKType), sdkCache.ChooseSDK);
        string target = $"{sdk_path}\\YuZiSdk\\Assets\\YuZiSdk\\{sdkName}\\XML";
        if (!Directory.Exists(target))
        {
            return;
        }
        target = target.Replace("/", "\\");
        string dest = Application.dataPath + "\\Plugins\\Android";
        // string dest = "D:\\Unity\\SDKTest\\Plugins\\Android";
        CopyFolder(target, dest);
    }
    
    /// <summary>
    /// 复制文件夹中的所有文件夹与文件到另一个文件夹
    /// </summary>
    /// <param name="sourcePath">源文件夹</param>
    /// <param name="destPath">目标文件夹</param>
    public static void CopyFolder(string sourcePath,string destPath)
    {
        if (Directory.Exists(sourcePath))
        {
            if (!Directory.Exists(destPath))
            {
                //目标目录不存在则创建
                try
                {
                    Directory.CreateDirectory(destPath);
                }
                catch (Exception ex)
                {
                    throw new Exception("创建目标目录失败：" + ex.Message);
                }
            }
            //获得源文件下所有文件
            List<string> files = new List<string>(Directory.GetFiles(sourcePath));
            files.ForEach(c =>
            {
                string destFile = Path.Combine(new string[]{destPath,Path.GetFileName(c)});
                File.Copy(c, destFile,true);//覆盖模式
            });
            //获得源文件下所有目录文件
            List<string> folders = new List<string>(Directory.GetDirectories(sourcePath));
            folders.ForEach(c =>
            {
                string destDir = Path.Combine(new string[] { destPath, Path.GetFileName(c) });
                //采用递归的方法实现
                CopyFolder(c, destDir);
            });
        }
        else
        {
            throw new DirectoryNotFoundException("源目录不存在！");
        }
    }

    public void ReadData() {
        if (sdkCache != null) {
            return;
        }
        sdkCache = SdkCache.ReadData();
        if (sdkCache.allSdk.Count == 0) {
            sdkCache.allSdk.Add(SDKType.Ohayoo, new SdkConfig());
        }
    }
    
      //设置宏定义
    public static void SetDefine(SDKType sdk)
    {
        var currentTarget = EditorUserBuildSettings.selectedBuildTargetGroup;

        if (currentTarget == BuildTargetGroup.Unknown) return;

        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(currentTarget).Trim();
        if (definesString.Contains("SDK_"))
        {
            if (!definesString.EndsWith(";"))
            {
                definesString += ";";
            }
            Regex r = new Regex(@"\bSDK_.*;\b");
            definesString = r.Replace(definesString, ("SDK_" + Enum.GetName(typeof(SDKType), sdk) + ";").ToUpper());
        }
        else if(definesString == "")
        {
            definesString += ("SDK_" + Enum.GetName(typeof(SDKType), sdk) + ";").ToUpper();
        }
        else
        {
            definesString += (";SDK_" + Enum.GetName(typeof(SDKType), sdk) + ";").ToUpper();

        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(currentTarget, definesString);
    }

}
