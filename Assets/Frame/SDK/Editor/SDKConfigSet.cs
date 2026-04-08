using GameVersionSpace;
#if DY
using StarkSDKSpace;
using StarkSDKTool;
#endif
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEditor;
using UnityEngine;
#if WX
using WeChatWASM;
#endif

public class SDKConfigSet
{
#if WX
    [MenuItem("微信小游戏/转换小游戏", false, 1)]
    static void OpenWXWindow() {
        SetGame();
        WeChatWASM.WXEditorWin.Open();
    }
#endif
    [MenuItem("程序工具/设置打包参数")]
    public static void SetGame()
    {
        var buildcache = BuildCache.ReadData();
        var tab = TableRead.Instance.ReadTable<GameSet>("GameSet", true);
#if UNITY_IOS
    PlayerSettings.productName = tab[2].strvalue; ; //游戏名字
#else
        PlayerSettings.productName = tab[1].strvalue; ; //游戏名字
#endif
        PlayerSettings.companyName = "yuzi";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, tab[12].strvalue);
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, tab[13].strvalue);
        buildcache.resName = tab[14].strvalue;
        buildcache.WXAdId = tab[7].strvalue;
        buildcache.DYAdId = tab[8].strvalue;
#if WX
        SetWX(buildcache.resName);
#endif
#if DY
        SetDY(buildcache.resName);
#endif
        buildcache.WriteData();
        AssetDatabase.Refresh();
    }
#if WX
    private static void SetWX(string projectNameEnglish)
    {
        var tab = TableRead.Instance.ReadTable<GameSet>("GameSet");
        var config = UnityUtil.GetEditorConf();
        PlayerSettings.productName = tab[3].strvalue; ; //游戏名字
        config.ProjectConf.projectName = tab[3].strvalue;
        config.ProjectConf.Appid = tab[5].strvalue;
#if TMSDK
        config.ProjectConf.CDN = tab[9].strvalue + $"/Xjfc/Webgl/StreamingAssets/tm/{projectNameEnglish}/";
#elif P8SDK 
        config.ProjectConf.CDN = tab[9].strvalue + $"/Xjfc/Webgl/StreamingAssets/p8wx/{projectNameEnglish}/";
        config.ProjectConf.Appid = "wxdbbe37aa45f44f4b";
#else
        config.ProjectConf.CDN = tab[9].strvalue + $"/Xjfc/Webgl/StreamingAssets/wx/{projectNameEnglish}/";
#endif
        config.ProjectConf.DST = FrameConfig.ClintFatherPath + "wx/";
        config.ProjectConf.disableHighPerformanceFallback = tab[11].strvalue == "1"; //是否开始IOS高性能模式
    }
#endif
#if DY
    private static void SetDY(string projectNameEnglish)
    {
        //Debug.LogError(StarkBuilderSettings.Instance.appId);
        var tab = TableRead.Instance.ReadTable<GameSet>("GameSet");
        PlayerSettings.productName = tab[4].strvalue; ; //游戏名字
        StarkBuilderSettings.Instance.appId = tab[6].strvalue;
        StarkBuilderSettings.Instance.urlCacheList = new string[] { tab[10].strvalue};
        StarkBuilderSettings.Instance.webGLOutputDir = FrameConfig.ClintFatherPath + "dy/";
        StarkBuilderSettings.Instance.Save();
    }
#endif
    }
