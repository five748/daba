using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BuildCache : ReadAndSaveTool<BuildCache>
{
    public string difUseDefaltChannel = "变化蓝本";
    public bool isUseDiff;
    public bool isNeedCopyToUnity;
    public string apkVersion;
    public int resVersion;
    public string resName;
    public string DYAdId;
    public string WXAdId;
    [System.NonSerialized]
    public int scrVersion;
    public int altasIndex = 1;
    public int abNameIndex;
    public int hotIndex;
    public int buildIndex = 3;
    public bool isAndroid = true;
    public Dictionary<string, bool> channels;
    public string[] channelSDKs;
    public int channelId;
    public int SDKChannlID
    {
        get
        {
            return int.Parse(channelSDKs[channelId].Split('_')[0]);
        }
    }
    public static BuildCache ReadData()
    {
        return ReadByResources("BuildSet", () => {
            return new BuildCache();
        });
    }
    public void WriteData()
    {
        Save(Application.dataPath + "/Resources/BuildSet.bytes");
    }
}