using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using static UnityEngine.PlayerLoop.PreUpdate;

public class AutoCreateProject
{
    private static string Depot {
        get {
            return Application.dataPath + "/baseframewx/ConfigCache/"; 
        }
    }
    private static string FaherPath
    {
        get
        {
            var paths = Application.dataPath.Split('/');
            return paths[paths.Length - 3];
        }
    }
    [MenuItem("程序工具/生成项目")]
    private static void CreateProject() {
        CreatePlan();
        CreateArt();
        CreateScenes();
        CreateProjectSetting();
        CreateRes();
        (Application.dataPath + "/Script/").CreateDir();
        CopyBat();
        CopyGit("art");
        CopyGit("client");
        CopyGit("plan");
        AssetDatabase.Refresh();
    }
    private static void CreatePlan() {
        DirCopyToClintFather("plan");
    }
    private static void CreateArt() {
        DirCopyToClintFather("art");
    }
    private static void CreateRes()
    {
        DirCopyToAsset("Res");
    }
    private static void CreateScenes()
    {
        string path = Application.dataPath + "/Scenes/Runing/GameBegin.unity";
        DirCopyToAsset("Scenes");
        if(!File.Exists(path))
            File.Move(Application.dataPath + "/Scenes/Runing/GameBeginBase.unity", path);
        else
            File.Delete(Application.dataPath + "/Scenes/Runing/GameBeginBase.unity");
    }
    private static void CreateProjectSetting()
    {
        DirCopyToAssetFather("ProjectSettings");
    }
    private static void DirCopyToClintFather(string path) {
        var planPath = FrameConfig.ClintFatherPath + path + "/";
        if (!Directory.Exists(planPath))
        {
            Directory.CreateDirectory(planPath);
        }
        AssetPath.CopyDirectory(Depot + path + "/", planPath, ".meta", false);
    }
    private static void DirCopyToAsset(string path)
    {
        var planPath = Application.dataPath + "/" + path + "/";
        if (!Directory.Exists(planPath))
        {
            Directory.CreateDirectory(planPath);
        }
        AssetPath.CopyDirectory(Depot + path + "/", planPath, ".meta", false);
    }
    private static void DirCopyToAssetFather(string path)
    {
        var planPath = FrameConfig.ClientPath + path + "/";
        if (!Directory.Exists(planPath))
        {
            Directory.CreateDirectory(planPath);
        }
        AssetPath.CopyDirectory(Depot + path + "/", planPath, ".meta", false);
    }
    private static void CopyBat() {
        File.Copy(Depot + "Bat/gitupdate.bat", FrameConfig.ClientPath + "gitupdate.bat", true);
        File.Copy(Depot + "Bat/useserver.bat", FrameConfig.ClientPath + "useserver.bat", true);
        File.Copy(Depot + "Bat/.gitignore", FrameConfig.ClientPath + ".gitignore", true);
    }
    private static void CopyGit(string path) {
        var planPath = FrameConfig.ClintFatherPath + path + "/.git/";
        if (!Directory.Exists(planPath))
        {
            Directory.CreateDirectory(planPath);
        }
        AssetPath.CopyDirectory(Depot + "git/", planPath, ".meta", true);
        var configPath = planPath + "config";
        var str = configPath.ReadByUTF8();
        str = str.Replace("XXX", FaherPath + "/" + path);
        configPath.WriteByUTF8(str);
    }
}
