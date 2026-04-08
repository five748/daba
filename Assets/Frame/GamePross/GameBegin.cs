using GameVersionSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using YuZiSdk;
using UnityEngine.UI;
using TreeData;
using Unity.VisualScripting;
using System;
# if WX
using WeChatWASM;
#endif
#if DY
using StarkSDKSpace;
#endif

//游戏开始类
public class GameBegin : MonoBehaviour
{
    public Transform uiLogin;
    public bool IsReport = false;//是否上报
    public int maxH = 2340;
    private bool BigVersion(string a) {
        if (!a.Contains('.')) {
            return false;
        }
        string[] strs = a.Split('.');
        if (strs[0].IsInt()) {
            if (int.Parse(strs[0]) > 2) {
                return true;
            }
        }
        return false;
    }
    private void Awake()
    {
        Debug.Log("游戏开始");
        YuziMgr.isOpenReport = false;
        YuziMgr.isOpenYuziReport = false;
#if TMSDK || DYTMSDK
        //TMSDK.Instance.SDKInit();
        //TMSDK.Instance.SDKSendGuideEvent("begin", "游戏开始", "");
#endif
        //Debug.LogError(Application.version) ;
#if DY
        StarkSDKSpace.StarkSDK.API.GetStarkAppLifeCycle().OnShowWithDict = UIDYOutSideTip.OnShowOneParam;
#endif
        Loom.Instance.Init();
        SdkMgr.Instance.Init(Init);
    }
   
    public void Init(bool isSuccess)
    {
        // YuziMgr.isOpenReport = IsReport;
        ScreenTool.Init();
        Input.multiTouchEnabled = false;
        if (uiLogin != null)
        {
            UIManager.InitNavigations(uiLogin.gameObject);//初始化界面
        }
        else
        {
            UIManager.InitNavigations(gameObject);//初始化界面
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            ProssData.Instance.platform = Platform.Android;
        }
        else
        {
            ProssData.Instance.platform = Platform.Ios;
        }
#if WX || DY
        ProssData.Instance.platform = Platform.WebGL;
#endif
        GameProcess.Instance.SetDebug();
        ReLoginClearData();
        GameProcess.Instance.Init();
        EventTriggerListener.IsSceneChanging = false;
        Application.targetFrameRate = ProssData.Instance.Frame;
        SetFirstUI();
        MonoTool.Instance.WaitEndFrame(() =>
        {
#if TMSDK || DYTMSDK
            TMSDK.Instance.SDKSendGuideEvent("begin", "资源加载", "-1");
#endif
            LoadServerRes(() =>
            {
                AssetLoadInit(() =>
                {
                    PreLoadPrefab(() =>
                    {
#if TMSDK || DYTMSDK
                        TMSDK.Instance.SDKSendGuideEvent("end", "资源加载", "-1");
#endif
                        Begin();
                    });
                });
            });
        });
    }
    public void ReLoginClearData()
    {
        if (ProssData.Instance.IsReLogin)
        {
            ClearSingle();
            GameProcess.Instance.ClearDonotDestory();
        }
    }
    public void ClearSingle()
    {
       
    }
    //首次设置UI界面
    private void SetFirstUI()
    {
        if (!ProssData.Instance.SingleGo)
        {
            ProssData.Instance.SingleGo = GameObject.Find("Single");
        }
        var sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "GameBeginPhone")
        {
            ProssData.Instance.IsUseILRunTime = false;
            ProssData.Instance.IsOpenHot = true;
        }
#if UNITY_EDITOR
        if (sceneName == "GameBeginTest") {
            ProssData.Instance.isTestLogin = true;
        }
        Application.runInBackground = true;
#endif
#if !UNITY_EDITOR
        ProssData.Instance.IsOpenHot = true;
#endif
        MonoTool.Instance.WaitEndFrame(() =>
        {
            //Debug.LogError(Screen.width + ":" + Screen.height);
            //Debug.LogError("screen:" + Screen.safeArea);
            var Root = GameObject.Find("Canvas").transform;
            ProssData.Instance.ScreenRange = Root.GetRange();
            ProssData.Instance.ScreenRangeBig = ProssData.Instance.ScreenRange * 1.5f;
            ProssData.Instance.ScreenRangeSmall = ProssData.Instance.ScreenRange * 0.8f;
            ProssData.Instance.ScreenBigWH = new Vector2(Screen.width, Screen.height) * 1.5f;
            ProssData.Instance.ScreenSmallWH = new Vector2(Screen.width, Screen.height) * 0.8f;

            var size = Root.GetComponent<RectTransform>().sizeDelta;
            //Debug.LogError("SafeH:" + Screen.safeArea.height);
            //Debug.LogError("ScreenH:" + Screen.height);
            //Debug.LogError("SizeH:" + size.x);
            SetSafeArea(size);
            ScreenTool.DevelopMaxWidth = ProssData.Instance.CanvasSize.x;
            SetScreenOutMask(Root);
            ScreenTool.DefaultWidth = ProssData.Instance.CanvasSize.x;
            ProssData.Instance.halfCanvasSize = 0.5f * ProssData.Instance.CanvasSize;
            ProssData.Instance.isSetCanvasSize = true;
            if (uiLogin != null)
            {
                UITool.Instance.SetAnchor(uiLogin);
            }
#if !UNITY_EDITOR
		//if (ProssData.Instance.IsHaveDebug)
		//{
  //         var settingComponent = ProssData.Instance.SingleGo.GetOrAddComponent<UnityGameFramework.Runtime.SettingComponent>();
  //          var debuggcom = ProssData.Instance.SingleGo.GetOrAddComponent<UnityGameFramework.Runtime.DebuggerComponent>();
		//	Debug.unityLogger.logEnabled = true;
		//}
		//else
		//{
		//	Debug.unityLogger.logEnabled = false;
		//}
        Debug.unityLogger.logEnabled = false;
#endif


#if DY
            StarkSDK.EnableStarkSDKDebugToast = false;
#endif
        });

    }
    private void SetSafeArea(Vector2 size){
        Rect rect = new Rect(0, 0, 0, 0);
        Vector2 screenSize = Vector2.zero;
        float modify = 1;
#if DY && !UNITY_EDITOR
        var systemInfo = StarkSDKSpace.StarkSDK.API.GetSystemInfo();
        screenSize = new Vector2((float)systemInfo.screenWidth, (float)systemInfo.screenHeight);
        var safe = systemInfo.safeArea;
        rect = new Rect((float)safe.left, (float)safe.top, (float)safe.width, (float)safe.height);
#elif WX && !UNITY_EDITOR
        var systemInfo = WeChatWASM.WX.GetSystemInfoSync();
        screenSize = new Vector2((float)systemInfo.screenWidth, (float)systemInfo.screenHeight);
        if (systemInfo.safeArea != null)
        {
            var safe = systemInfo.safeArea;
            rect = new Rect((float)safe.left, (float)safe.top, (float)safe.width, (float)safe.height);
        }
        else {
            rect = new Rect(0, 0, screenSize.x, screenSize.y);
        }
#else
        rect = Screen.safeArea;
        screenSize = new Vector2(Screen.width, Screen.height);

#endif
        //Debug.LogError("screenSizeX:" + screenSize.x);
        modify = screenSize.x / 1080;
        var safeTop = (int)(rect.yMin / modify); 
        //Debug.LogError("safeTop:" + safeTop);
        ProssData.Instance.CanvasModifyTop = new Vector2(0, safeTop);
        var safeDown = (int)((screenSize.y - rect.yMax) / modify);
        ProssData.Instance.CanvasModifyDown = new Vector2(0, safeDown);
        if (size.y > maxH)
        {
            var cut = (size.y - maxH) * 0.5f;
            ProssData.Instance.CanvasModifyTop -= new Vector2(0, cut);
            ProssData.Instance.CanvasModifyDown -= new Vector2(0, cut);
            size.y = maxH;
        }
        ProssData.Instance.CanvasSize = new Vector2(size.x, size.y);
    }
    private void SetSafeAreaGame(int safeH, int bottom) { 
        
    }
    private int maskImageSizeY = 500;
    public void SetScreenOutMask(Transform father) {
        var top = NewImage("topImage", father);
        var down = NewImage("downImage", father);
        var y = maxH / 2 + maskImageSizeY * 0.5f;
        //var modify = ProssData.Instance.CanvasSize.y / 2.0f + maskImageSizeY * 0.5f - ProssData.Instance.CanvasModifyTop.y;
        top.anchoredPosition3D = new Vector3(0, y, 0);
        down.anchoredPosition3D = new Vector3(0, -y, 0);
    }
    private RectTransform NewImage(string name, Transform father) {
        var topImage = new GameObject(name).AddComponent<RectTransform>();
        topImage.sizeDelta = new Vector2(1080, maskImageSizeY);
        var image = topImage.AddComponent<Image>();
        image.color = Color.black;
        topImage.SetParent(father);
        topImage.localScale = Vector3.one;
        topImage.SetDepth(-10000);
        return topImage;
    } 
    //服务器设置
    public void GameSet()
    {
        Debug.Log("游戏设置中");
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
    }
    //下载服务器资源
    public void LoadServerRes(System.Action callback)
    {
        Debug.Log("资源同步中");
#if TMSDK || DYTMSDK
        TMSDK.Instance.sendLoadingLog("progressStart");
#endif

        HotMgr.Instance.Init(transform, () =>
        {
            Debug.Log("资源同步完成");
            callback();
#if TMSDK || DYTMSDK
            TMSDK.Instance.sendLoadingLog("progressEnd");
            return;
#endif
        });
        //GameVersion.Instance.InitVersion(callback);
    }
    //资源加载准备
    public void AssetLoadInit(System.Action callback)
    {
        ABData.Instance.PreLoad(callback);
    }
    //预加载资源
    public void PreLoadPrefab(System.Action callback)
    {
        if (!ProssData.Instance.IsOpenHot)
        {
            callback();
            return;
        }
        callback();
        //AssetLoadOld.Instance.LoadPrefab("UIPrefab/UIHeroMain", (go) => {
        //	callback();
        //});
    }
    //进入登陆界面
    public void Begin()
    {
        Debug.Log("开始游戏");
        ProssData.Instance.IsReLogin = false;
        Launch.Instance.Init();
        UIManager.GoingToName = "";
        HotMgr.Instance.DownLoadLater();
        //SdkMgr.Instance.OpenWXMes();
        SdkMgr.Instance.ActiveYuzi((b) =>
        {
            FirstLoadUI.Load();
        });

    }
}
