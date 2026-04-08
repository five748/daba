using UnityEditor;
using UnityEngine;
using System.IO;

public class AndroidProject : EditorWindow
{
    public static EditorWindow window;

    string AndroidPluginPath
    {
        get
        {
            return "/Plugins/Android/";
        }
    }
    string AndroidSDKCopyPath
    {
        get
        {
            return "/YWSDK/File/";
        }
    }

    [MenuItem("程序工具/安卓SDK管理")]  //添加菜单选项
    public static void ShowWindow()
    {
        if (window != null)
            return;

        window = EditorWindow.GetWindow(typeof(AndroidProject), true, "安卓SDK管理", true);
        window.Show();
    }
    void OnGUI()
    {
        SetAndroidProject();
    }

    void SetAndroidProject()
    {
        EditorGUILayout.BeginHorizontal();  //开始水平布局
        if (GUILayout.Button("无SDK包".ToString()))
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.Mono2x);
            ClearOnePath(Application.dataPath + AndroidPluginPath);
            AssetPath.CopyDirectory(Application.dataPath.CutDir(1) + AndroidSDKCopyPath + "empty/", Application.dataPath + AndroidPluginPath);
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();  //开始水平布局
        EditorGUILayout.BeginHorizontal();  //开始水平布局
        if (GUILayout.Button("肆狼官方包".ToString()))
        {
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.All;
            ClearOnePath(Application.dataPath + AndroidPluginPath);
            AssetPath.CopyDirectory(Application.dataPath.CutDir(1) + AndroidSDKCopyPath + "silang3.0/", Application.dataPath + AndroidPluginPath);
            AssetDatabase.Refresh();
        }
        EditorGUILayout.EndHorizontal();
    }
    private void CopyToPlugin()
    {

    }
    private void ClearOnePath(string path)
    {
        if (Directory.Exists(path))
        {
            DeleteFolder(path);
        }
        else
        {
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
}