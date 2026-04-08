using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using System;
using LitJson;

public class TMSDK:MonoBehaviour
{
#if TMSDK || DYTMSDK
    private static TMSDK instance = null;

    public static TMSDK Instance
    {
        get
        {
            if (instance == null)
            {

                if (!Application.isPlaying)
                {
                    Debug.LogError("不支持在非播放模式下调用TMSDK接口");
                    return null;
                }
                instance = new GameObject(typeof(TMSDK).Name).AddComponent<TMSDK>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    public bool VideoAuto = false;
    private static bool progressStart = true;
  

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern int TM_SDK_GetJudgeConfig(int id);
#else
    private int TM_SDK_GetJudgeConfig(int id)
    {
        throw new NotImplementedException();
    }
#endif
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_Init();
#else
    private void TM_SDK_Init()
    {
        throw new NotImplementedException();
    }
#endif
    public void SDKInit()
    {
        Debug.Log("SDKInit");
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_Init();
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_sendGuideEvent(string isBeginOrOver, string guideName, string guideId);
#else
    private void TM_SDK_sendGuideEvent(string isBeginOrOver, string guideName, string guideId)
    {
        throw new NotImplementedException();
    }
#endif
    public void SDKSendGuideEvent(string isBeginOrOver, string guideName, string guideId)
    {
        Debug.Log("SDKSendGuideEvent:" + isBeginOrOver + ":" + guideId);
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_sendGuideEvent(isBeginOrOver, guideName, guideId);
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_syncPlayerInfo(int createTypeId, string guid, string g_nickname, string g_zone, string g_level, string g_vip_level);
#else
    private void TM_SDK_syncPlayerInfo(int createTypeId, string guid, string g_nickname, string g_zone, string g_level, string g_vip_level)
    {
        throw new NotImplementedException();
    }
#endif
    public void SyncPlayerInfo(int createTypeId, string guid, string g_nickname, string g_zone, string g_level, string g_vip_level)
    {
        Debug.Log("SyncPlayerInfo");
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_syncPlayerInfo(createTypeId, guid, g_nickname, g_zone, g_level, g_vip_level);
#endif
    }

    public int GetJudgeConfig(int id)
    {
        Debug.Log("TMSDK GetJudgeConfig" + id);
#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log($"TMSDK GetJudgeConfig === {TM_SDK_GetJudgeConfig(id)}");
            return TM_SDK_GetJudgeConfig(id);
#else
        return 0;
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_SENDLOADINGLOG(string appVersion);
#else
    private void TM_SDK_SENDLOADINGLOG(string appVersion)
    {
        throw new NotImplementedException();
    }
#endif

    public void sendLoadingLog(string str)
    {
        Debug.Log("TMSDK sendLoadingLog " + str);
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_SENDLOADINGLOG(str);
#endif
    }

    #region 原生模板
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_CreateCustomAd(int id, int left, int top, int right, int bottom);
#else
    private void TM_SDK_CreateCustomAd(int id, int left, int top, int right, int bottom)
    {
        throw new NotImplementedException();
    }
#endif
    public void CustomAd(int id, int left, int top, int right = 0, int bottom = 0)
    {
        Debug.Log("TMSDK CustomAd " + id);
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_CreateCustomAd(id, left, top, right, bottom);
#endif
    }
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_HideCustomAd();
#else
    private void TM_SDK_HideCustomAd()
    {
        throw new NotImplementedException();
    }
#endif
    public void hideCustomAd()
    {
        Debug.Log("TMSDK hideCustomAd ");
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_HideCustomAd();
#endif
    }
    #endregion

    #region 激励视频
    public class TMSDKOnRewardedVideoAd
    {
        public bool status;
        public string msg;
        public int id;
    }
    public Action<TMSDKOnRewardedVideoAd> ShowRewardedVideoAdCallBack;
    public bool _isVideoShow = false;
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_CreateRewardedVideoAd(string id);
#else
    private void TM_SDK_CreateRewardedVideoAd(string id)
    {
        throw new NotImplementedException();
    }
#endif
    public void CreateRewardedVideoAd()
    {
        Debug.Log("TMSDK CreateRewardedVideoAd");
#if UNITY_WEBGL && !UNITY_EDITOR
             TM_SDK_CreateRewardedVideoAd(GameVersionSpace.HotMgr.Instance.buildCache.WXAdId);
#endif
    }
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_ShowRewardedVideoAd(string id);
#else
    private void TM_SDK_ShowRewardedVideoAd(string id)
    {
        throw new NotImplementedException();
    }
#endif
    public void ShowRewardedVideoAd(int adId, string idstr, Action<TMSDKOnRewardedVideoAd> callback)
    {
        if (_isVideoShow)
        {
            Debug.Log("TMSDK ShowRewardedVideoAd _isVideoShow" + _isVideoShow);
            callback(null);
        }
        else
        {
            _isVideoShow = true;
            ShowRewardedVideoAdCallBack = callback;
            Debug.Log("TMSDK ShowRewardedVideoAd");
#if UNITY_WEBGL && !UNITY_EDITOR
                TM_SDK_ShowRewardedVideoAd(idstr);
#else
            _isVideoShow = false;
            callback(null);
#endif
        }
    }
    public void ShowRewardedVideoAdBack(string str) {
        if (ShowRewardedVideoAdCallBack != null) {
            Debug.LogError(str);
            var res = JsonUtility.FromJson<TMSDKOnRewardedVideoAd>(str);
            ShowRewardedVideoAdCallBack(res);
        }
    }
    public void onRewardedVideoAd(string msg)
    {
        _isVideoShow = false;
        var res = JsonUtility.FromJson<TMSDKOnRewardedVideoAd>(msg);
        Debug.Log("TMSDK onRewardedVideoAd " + msg);
        Debug.Log(res);
        ShowRewardedVideoAdCallBack(res);
    }
    #endregion

    #region banner
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_ShowBanner(int id);
#else
    private void TM_SDK_ShowBanner(int id)
    {
        throw new NotImplementedException();
    }
#endif
    public void ShowBanner(int id)
    {
        Debug.Log("TMSDK ShowBanner");
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_ShowBanner(id);
#endif
    }
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void TM_SDK_HideBanner();
#else
    private void TM_SDK_HideBanner()
    {
        throw new NotImplementedException();
    }
#endif
    public void HideBanner()
    {
        Debug.Log("TMSDK ShowBanner");
#if UNITY_WEBGL && !UNITY_EDITOR
            TM_SDK_HideBanner();
#endif
    }
    #endregion
#endif
}