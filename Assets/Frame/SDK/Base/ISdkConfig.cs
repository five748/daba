
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum SDKType {
    Ohayoo = 1,
    WX = 2,
}
[System.Serializable]
public class SdkConfig
{
    public string Url_Report {
        get {
#if UNITY_EDITOR
            return Url_Report_Editor;
#endif
            return Url_Report_Phone;
        }
    }
    //本地上报网址
    public string Url_Report_Editor = "http://192.168.3.12:9000/Api/";
    public string Url_Report_Phone = "https://report-s.huanlexiuxian.cn/Api/";
    public string app_name = Application.productName;
    public string deviceId = UnityEngine.SystemInfo.deviceUniqueIdentifier;
    
    public string Uid = "";//用户id

    public string appId = "";

    public string os = "window";//操作系统
    public string platform = "unity";//平台

    public string ios_AdId = "";//广告id
    public string android_AdId = "";
    public int videoPlayOrientation = 1;//广告方向 1 竖屏
}
[System.Serializable]
public class SdkCache : ReadAndSaveTool<SdkCache>
{
    public SDKType ChooseSDK = SDKType.Ohayoo;
    public Dictionary<SDKType, SdkConfig> allSdk = new Dictionary<SDKType, SdkConfig>();
    public SdkConfig OnSDKConfig {
        get {
            if (!allSdk.ContainsKey(ChooseSDK))
            {
                return  new SdkConfig();
            }
            return allSdk[ChooseSDK];
        }
    }
    public static SdkCache ReadData()
    {
        return ReadByResources("SDKCache", () => {
            return new SdkCache();
        });
    }
    public void WriteData()
    {
        Save(Application.dataPath + "/Resources/SDKCache.bytes");
    }
}
