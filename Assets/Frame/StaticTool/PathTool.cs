using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using LitJson;




public static class PathTool
{
    public static void GetAllFileName(this string targetPath, System.Action<DirectoryInfo> callbackDir = null, System.Action<FileInfo> callbackFile = null)
    {
        if (!Directory.Exists(targetPath)) {
            Debug.Log("找不到:" + targetPath);
            return;
        }
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            if (callbackFile != null)
                callbackFile(info);
        }
        //获取所有子路径
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".svn") != -1)
                continue;
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            if (callbackDir != null)
                callbackDir(info);
            info.FullName.GetAllFileName(callbackDir, callbackFile);
        }
    }
    public static void GetAllFileNameOnlyThis(this string targetPath, System.Action<FileInfo> callbackFile = null)
    {
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        foreach (FileInfo info in Dir.GetFiles())
        {
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            if (callbackFile != null)
                callbackFile(info);
        }
    }
    public static void GetAllDirectoriesNameOnlyThis(this string targetPath, System.Action<DirectoryInfo> callbackDir = null)
    {
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        foreach (System.IO.DirectoryInfo info in Dir.GetDirectories())
        {
            if (info.FullName.IndexOf(".meta") != -1)
                continue;
            if (callbackDir != null)
                callbackDir(info);
        }
    }
    public static void GetDirOnlyThis(this string targetPath, System.Action<DirectoryInfo> callback = null)
    {
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            if (callback != null)
                callback(info);
        }
    }
    public static List<string> GetLstDirOnlyThis(this string targetPath)
    {
        List<string> paths = new List<string>();
        System.IO.DirectoryInfo Dir = new System.IO.DirectoryInfo(targetPath);
        foreach (DirectoryInfo info in Dir.GetDirectories())
        {
            paths.Add(info.Name);
        }
        return paths;
    }
    public static void CreateDir(this string fileName)
    {
        string dirPath = Path.GetDirectoryName(fileName);
        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }
    }
    public static void CreateDirAndFile(this string fileName)
    {
        fileName.CreateDir();
        if (!File.Exists(fileName))
        {
            File.Create(fileName).Dispose();
        }
    }
    public static string GetImagePath(this string filePath)
    {
        var dirPath = Path.GetDirectoryName(filePath);
        return dirPath.Replace("Assets/Res/Image/ChangeImage/", "");
    }
    public static string GetSpinePath(this string filePath)
    {
        var dirPath = Path.GetDirectoryName(filePath).CutPathByLast(1);
        return dirPath.Replace("Assets/Res/Spine/", "");
    }
    public static Dictionary<int, int> ReadToDic(this string path)
    {
        Dictionary<int, int> datas = new Dictionary<int, int>();
        var str = path.ReadByUTF8();
        if (string.IsNullOrEmpty(str))
        {
            return datas;
        }
        var strs = str.Split('|');
        foreach (var one in strs)
        {
            var strss = one.Split('_');
            datas.Add(int.Parse(strss[0]), int.Parse(strss[1]));
        }
        return datas;
    }
    public static Dictionary<int, List<int>> ReadToDicChangeByValue(this string path)
    {
        Dictionary<int, List<int>> datas = new Dictionary<int, List<int>>();
        //AssetLoadOld.Instance.LoadText(path, (str) =>
        //{
        //    var strs = str.Split('|');
        //    foreach (var one in strs)
        //    {
        //        var strss = one.Split('_');
        //        int key = int.Parse(strss[1]);
        //        if (!datas.ContainsKey(key))
        //        {
        //            datas.Add(key, new List<int>());
        //        }
        //        datas[key].Add(int.Parse(strss[0]));
        //    }
        //});
        return datas;
    }
    public static void WriteByDic(this string path, Dictionary<int, int> datas)
    {
        var str = "";
        foreach (var item in datas)
        {
            str += item.Key + "_" + item.Value + "|";
        }
        path.WriteByUTF8(str.CutLast());
    }
    public static Dictionary<string, string> ReadToDicStr(this string path)
    {
        Dictionary<string, string> datas = new Dictionary<string, string>();
        //AssetLoadOld.Instance.LoadText(path, (str) =>
        //{
        //    if (string.IsNullOrEmpty(str))
        //    {
        //        Debug.Log("找不到:" + path);
        //        return;
        //    }
        //    var strs = str.Split('|');
        //    foreach (var one in strs)
        //    {
        //        var strss = one.Split('_');
        //        try
        //        {

        //            datas.Add(strss[0], strss[1]);
        //        }
        //        catch
        //        {
        //            Debug.LogError(strss);
        //        }
        //    }
        //});
        return datas;
    }
    public static void WriteByDicStr(this string path, Dictionary<string, string> datas)
    {
        //Debug.LogError(path);
        var str = "";
        foreach (var item in datas)
        {
            str += item.Key + "_" + item.Value + "|";
        }
        path.WriteByUTF8(str.CutLast());
    }
}
