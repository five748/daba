
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
public class SDKChange
{
    public enum SDKChangType { 
        none = 0,
        wx = 1,
        dy = 2,
        ohayoo = 3,
        tm = 4,
        dytm = 5,
    }
    public static string WXPath = Application.dataPath;
    public static string DYPath = Application.dataPath + "/Plugins/";
    public static string OhyooPath = Application.dataPath + "/Ohayoo/";
    public static string AndroidXMLPath = Application.dataPath + "/Plugins/Android/";
    public static string PluginsPath = Application.dataPath + "/Plugins/";

    private static string _macroOhayoo = "SDK_OHAYOO";
    private static string _macroDy = "DY";
    private static string _macroWx = "WX";
    private static string _macroTm = "TMSDK";
    private static string _macroDYTm = "DYTMSDK";

    private static List<string> _macroSdks = new List<string>() { _macroOhayoo, _macroDy, _macroWx, _macroTm };
    
    public static string SDKCache {
        get {
            return SDKCacheRoot + "sdkcache/";
        }
    }
    public static string SDKCacheRoot
    {
        get
        {
            return Application.dataPath.CutDir(3);
        }
    }

    [MenuItem("程序工具/SDK切换/切换为微信")]
    public static void ChangeToWX() {
        if (!IsPullSDKCache()) {
            return;
        }
        set_sdk_macro(_macroWx);
        
        CleanAllPath();
        CopyChild(SDKCache + "wx/", WXPath);
        WriteSDKChangeType(SDKChangType.wx);
        Refresh();
    }
    [MenuItem("程序工具/SDK切换/切换为TMSDK")]
    public static void ChangeToTM()
    {
        if (!IsPullSDKCache())
        {
            return;
        }
        set_sdk_macro(_macroTm);

        CleanAllPath();
        CopyChild(SDKCache + "tm/", WXPath);
        WriteSDKChangeType(SDKChangType.tm);
        Refresh();
    }
    [MenuItem("程序工具/SDK切换/切换为DYTMSDK")]
    public static void ChangeToDYTM()
    {
        if (!IsPullSDKCache())
        {
            return;
        }
        set_sdk_macro(_macroDYTm);

        CleanAllPath();
        CopyChild(SDKCache + "dytm/", WXPath);
        WriteSDKChangeType(SDKChangType.dytm);
        Refresh();
    }
    [MenuItem("程序工具/SDK切换/切换为抖音")]
    public static void ChangeToDY()
    {
        if (!IsPullSDKCache())
        {
            return;
        }
        set_sdk_macro(_macroDy);
        
        CleanAllPath();
        CopyChild(SDKCache + "dy/", DYPath);
        WriteSDKChangeType(SDKChangType.dy);
        Refresh();
    }
    [MenuItem("程序工具/SDK切换/切换为Ohayoo")]
    public static void ChangeToOhayoo()
    {
        if (!IsPullSDKCache())
        {
            return;
        }
        set_sdk_macro(_macroOhayoo);

        CleanAllPath();
        CopyChild(SDKCache + "ohayoo/Script/", OhyooPath + "/Script/");
        CopyChild(SDKCache + "ohayoo/XML/", AndroidXMLPath);
        CopyChild(SDKCache + "ohayoo/Plugins/", PluginsPath);
        WriteSDKChangeType(SDKChangType.ohayoo);
        Refresh();
    }
    public static void Setsymol(string str) {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, str);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, str);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, str);
    }
    [MenuItem("程序工具/SDK切换/移除所有")]
    public static void RemoveAll() {
        if (!IsPullSDKCache())
        {
            return;
        }
        // BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        //获取当前平台已有的宏定义
        // var symbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
        // symbols = "SHIP_AIR";
        // PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, symbols);
        // Debug.Log("添加后的宏：" + PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup));
        set_sdk_macro("");
        CleanAllPath();
        WriteSDKChangeType(SDKChangType.none);
        Refresh();
    }
    public static void CleanAllPath() {
        var sdkType = GetChangeType();
        //wx
        DelPath(WXPath + "/WebGLTemplates/Template/");
        DelPath(WXPath + "/WebGLTemplates/WXTemplate/");
        DelPath(WXPath + "/WebGLTemplates/WXTemplate2020/");
        DelPath(WXPath + "/WebGLTemplates/WXTemplate2022/");
        DelPath(WXPath + "/WebGLTemplates/WXTemplate2022TJ/");
        DelPath(WXPath + "/WX-WASM-SDK-V2/");
        //dy
        DelPath(DYPath + "/ByteGame/");
        //删除ohyoo 代码
        DelPath(OhyooPath);
        //删除ohyoo sdk
        DelPath(PluginsPath + "/ByteGame/");
        File.Delete(PluginsPath + "/Android/AndroidManifest.xml");
        //switch (sdkType) {
        //    case SDKChangType.wx:
        //        DelPath(WXPath + "/WebGLTemplates/", SDKCache + "/wx/WebGLTemplates/");
        //        DelPath(WXPath + "/WX-WASM-SDK-V2/", SDKCache + "/wx/WX-WASM-SDK-V2/");
        //        break;
        //    case SDKChangType.dy:
        //        DelPath(DYPath + "/ByteGame/", SDKCache + "/dy/ByteGame/");
        //        break;
        //    case SDKChangType.ohayoo:
        //        //删除ohyoo 代码
        //        DelPath(OhyooPath, SDKCache + "/ohayoo/");
        //        //删除ohyoo sdk
        //        DelPath(PluginsPath + "/ByteGame/", SDKCache + "/ohayoo/Plugins/ByteGame/");
        //        File.Delete(PluginsPath + "/Android/AndroidManifest.xml");
        //        break;
        //}
    }
    private static void Refresh() {
        AssetDatabase.Refresh();
        PlayerSettings.WebGL.template = "PROJECT:Default";
    }
    private static void DelPath(string path) {
        if (Directory.Exists(path))
        {
            //CopyChild(path, targetPath);
            Directory.Delete(path, true);
        }
        if (File.Exists(path.CutLast() + ".meta")) {
            File.Delete(path.CutLast() + ".meta");
        }
    }
    public static void CopyChild(string sourcePath, string destinationPath, string filt = "", bool overwrite = true)
    {
        DirectoryInfo info = new DirectoryInfo(sourcePath);
        if (!Directory.Exists(destinationPath))
        {
            //目标目录不存在则创建
            try
            {
                Directory.CreateDirectory(destinationPath);
            }
            catch (Exception ex)
            {
                throw new Exception("创建目标目录失败：" + ex.Message);
            }
        }
        foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
        {
            string destName = destinationPath + "/" + fsi.Name;
            //UnityEngine.Debug.Log(destName);
            if (fsi is System.IO.FileInfo)          //如果是文件，复制文件
            {
                if (filt != "" && fsi.Name.Contains(filt))
                {
                    continue;
                }
                if (!overwrite)
                {
                    if (File.Exists(fsi.FullName))
                    {
                        File.Copy(fsi.FullName, destName, true);
                    }
                }
                else
                {
                    File.Copy(fsi.FullName, destName, true);
                }
            }
            else  //如果是文件夹，新建文件夹，递归
            {
                Directory.CreateDirectory(destName);
                CopyChild(fsi.FullName, destName, filt, overwrite);
            }
        }
    }
    private static SDKChangType GetChangeType()
    {
        var str = (SDKCache + "sdktype.txt").ReadByUTF8();
        return (SDKChangType)int.Parse(str);
    }
    private static void WriteSDKChangeType(SDKChangType skdType)
    {
        var str = ((int)skdType).ToString();
        (SDKCache + "sdktype.txt").WriteByUTF8(str);
    }
    private static bool IsPullSDKCache() {
        if (!Directory.Exists(SDKCache))
        {
            if (EditorUtility.DisplayDialog("", "SDK仓库为空, 是否拉取SDK仓库?", "ok", "no"))
            {
                var cmd = new CMDTool(SDKCacheRoot.Replace("/", "\\"));
                cmd.Run("git", "clone git@git.yuzi-game.com:frame/sdkcache.git");
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private static void set_sdk_macro(string str)
    {
        var result = new List<string>();
        foreach (var value in _macroSdks)
        {
            if (value == str)
            {
                continue;
            }
            result.Add(value);
        }

        MacroTool.RemoveMacros(result, BuildTargetGroup.Android);
        MacroTool.RemoveMacros(result, BuildTargetGroup.iOS);
        MacroTool.RemoveMacros(result, BuildTargetGroup.WebGL);
        MacroTool.RemoveMacros(result, BuildTargetGroup.Standalone);
        if (str != "")
        {
            MacroTool.AddMacro(str, BuildTargetGroup.Android);
            MacroTool.AddMacro(str, BuildTargetGroup.iOS);
            MacroTool.AddMacro(str, BuildTargetGroup.WebGL);
            MacroTool.AddMacro(str, BuildTargetGroup.Standalone);
        }
        if (str == "TMSDK") {
            MacroTool.AddMacro("WX", BuildTargetGroup.Android);
            MacroTool.AddMacro("WX", BuildTargetGroup.iOS);
            MacroTool.AddMacro("WX", BuildTargetGroup.WebGL);
            MacroTool.AddMacro("WX", BuildTargetGroup.Standalone);
        }
    }
}
