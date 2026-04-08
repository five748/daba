using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DG.Tweening.Plugins.Core.PathCore;
using TreeData;
using System;
using Unity.VisualScripting;
using Table;

[System.Serializable]
public class ReadAndSaveTool<T>
{
    private static string PhoneCachePath {
        get {
//#if DY && !UNITY_EDITOR
//            return StarkSDKSpace.StarkFileSystemManagerDefault.USER_DATA_PATH + "/DataCenter/";
//#endif
            return Application.persistentDataPath + "/DataCenter/";
        }
    }
    private static T ReadWXData(string readPath, System.Func<T> newT)
    {
        //
        string str = "";
#if WX
        str = PlayerPrefs.GetString(readPath);
        PlayerPrefs.Save();
#endif
#if DY
        str = StarkSDKSpace.StarkSDK.API.PlayerPrefs.GetString(readPath);
        //var fileSystem = StarkSDKSpace.StarkSDK.API.GetStarkFileSystemManager();
        //readPath = PhoneCachePath + readPath;
        //if (!fileSystem.AccessSync(readPath))
        //{
        //    return newT();
        //}
        //str = fileSystem.ReadFileSync(readPath, "utf8");
#endif
        //Debug.LogError(str);
        if (!string.IsNullOrEmpty(str))
        {
            return LitJson.JsonMapper.ToObject<T>(str);
        }
        return newT();
    }

    /// <summary>
    /// 写入文件
    /// </summary>
    private static void WriteWXData(T go, string readPath)
    {
        //Debug.LogError("WriteWXData:" + readPath);
        //Application.quitting
        //Debug.LogError(go);
        var str = LitJson.JsonMapper.ToJson(go);
#if WX
        PlayerPrefs.SetString(readPath, str);
#endif
#if DY
        StarkSDKSpace.StarkSDK.API.PlayerPrefs.SetString(readPath,str);
        StarkSDKSpace.StarkSDK.API.PlayerPrefs.Save();
#endif
    }
#if DY
    static void CreateFile(string filePath, string fileContext)
    {
        var fileSystem = StarkSDKSpace.StarkSDK.API.GetStarkFileSystemManager();

        if (!fileSystem.AccessSync(PhoneCachePath))
        {
            fileSystem.MkdirSync(PhoneCachePath);
        }
        string isSucc = fileSystem.WriteFileSync(filePath,  fileContext);
        Debug.Log(isSucc);
    }
#endif
    protected static void WriteInPhone(T go,string readPath)
    {
        if (ProssData.Instance.platform == Platform.WebGL)
        {
            WriteWXData(go, readPath);
            return;
        }
        string path = PhoneCachePath + readPath + ".bytes";
        //Debug.Log("保存数据:" + path);
        SerializeHelper.InstaceToFile(go, path);

    }
    protected static T ReadByPhone(string readPath, System.Func<T> newT)
    {
        if (ProssData.Instance.platform == Platform.WebGL)
        {
            return ReadWXData(readPath, newT);
        }
        string path = PhoneCachePath + readPath + ".bytes";
        if (!File.Exists(path))
        {
            Debug.Log("找不到:" + path);
            return newT();
        }
        return SerializeHelper.FileToInstance<T>(PhoneCachePath + readPath + ".bytes");
    }

    protected static T Read(string readPath, System.Func<T> newT)
    {
        if (ProssData.Instance.platform == Platform.WebGL)
        {
            return ReadWXData(readPath, newT);
        }
        else
        {
            TextAsset textAsset = null;
            AssetLoadOld.Instance.LoadAsset(readPath, ".bytes", (go) => {
                textAsset = go as TextAsset;
            });
            if (textAsset == null)
            {
                return newT();
            }
            MemoryStream ms = new MemoryStream(textAsset.bytes);
            return SerializeHelper.MemoryToInstance<T>(ms);
        }
    }
    protected static T ReadSkillBase(string readPath, System.Func<T> newT)
    {
        TextAsset textAsset = null;
        AssetLoadOld.Instance.LoadAssetResources(readPath, (go) => {
            textAsset = go as TextAsset;
        });
        if (textAsset == null)
        {
            return newT();
        }
        MemoryStream ms = new MemoryStream(textAsset.bytes);
        return SerializeHelper.MemoryToInstance<T>(ms);
    }
    protected static T ReadByResources(string readPath, System.Func<T> newT)
    {
        //Debug.LogError(readPath);
        TextAsset textAsset = Resources.Load<TextAsset>(readPath);
        if (textAsset == null)
        {
            return newT();
        }
        MemoryStream ms = new MemoryStream(textAsset.bytes);
        return SerializeHelper.MemoryToInstance<T>(ms);
    }
    protected void Save(string savePath) {
        SerializeHelper.InstaceToFile(this, savePath);
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }
    protected void Del(string path) {
        if (File.Exists(path))
        {
            File.Delete(path);
#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
        }
        else {
            Debug.LogError("找不到可删除的文件:" + path);
        }
    }
}

