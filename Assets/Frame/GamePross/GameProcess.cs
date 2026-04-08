using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameVersionSpace;
using System;
using YuZiSdk;
using UnityEngine.Networking;
#if WX
using WeChatWASM;
#endif

public class GameProcess:SingleMono2<GameProcess> {
    public System.Action UpdateEvent;
    public List<GameObject> DonotDestoryGos = new List<GameObject>();
    public System.Action SaveEvent;
    public System.Action StopEvent;
    public System.Action CleanEvent;
    public System.Action<bool> OnFocusEvent;
    public void Init()
    {
        ProssData.Instance.loginTime = Time.time;
        CheckIsHaveConent();
        Application.quitting += () =>
        {
            Debug.LogError("游戏退出");
            SaveDataEvent();
#if TMSDK || DYTMSDK
            TMSDK.Instance.SDKSendGuideEvent("end", "游戏退出", "");
#endif
        };
#if DY
        StarkSDKSpace.StarkSDK.API.GetStarkAppLifeCycle().OnShowWithDict = (a) =>
        {
            Debug.LogError("OnShow");
            OnApplicationFocus(true);
        };
        StarkSDKSpace.StarkSDK.API.GetStarkAppLifeCycle().OnHide = () =>
        {
            Debug.LogError("OnHide");
            OnApplicationFocus(false);
        };
#endif
#if WX && !UNITY_EDITOR
        WX.OnShow(OnShow);
        WX.OnHide(OnHide);
#endif
    }
#if WX
    public void OnShow(OnShowListenerResult result)
    {
        // 输入法confirm回调
        Debug.LogError("OnShow");
        OnApplicationFocus(true);
    }
    public void OnHide(GeneralCallbackResult result)
    {
        // 输入法confirm回调
        Debug.LogError("OnHide");
        OnApplicationFocus(false);
    }
#endif
    public List<Transform> loadOverTrans = new List<Transform>();
    private Vector3 bigStep = new Vector3(0.1f, 0.1f, 0.1f);
    private List<Transform> needDelTran = new List<Transform>();
    public void AddLoadOverTran(Transform tran) {
        if (loadOverTrans.Contains(tran))
        {
            return;
        }
        //tran.localScale = Vector3.zero;
        //loadOverTrans.Add(tran);
        //tran.SetActive(true);
    }
    private void UpDateBig()
    {
        for (int i = loadOverTrans.Count - 1; i >= 0; i--)
        {
            var tran = loadOverTrans[i];
            if (tran == null)
            {
                loadOverTrans.RemoveAt(i);
                continue;
            }
            if (tran.localScale.x >= 1)
            {
                loadOverTrans.RemoveAt(i);
                continue;
            }
            tran.localScale += bigStep;
        }
    }
    private void Update()
    {
        
    }
    //本段代码退出
    void OnDestroy() {
        //Debug.LogError("SaveData"); 
        //if (SaveEvent != null)
        //{
        //    SaveEvent();
        //}
    }
    //暂停
    public void OnApplicationPause(bool pause)
    {

    }
    private int beginTime;
    public bool isInBg = false;
    //失去焦点
    public void OnApplicationFocus(bool focus)
    {
        //Debug.LogError("focus:" + focus);
        try
        {
#if !UNITY_EDITOR
        if (OnFocusEvent != null)
        {
            OnFocusEvent(focus);
        }
        if (!focus)
        {
            isInBg = true;
            SaveDataEvent();
        }
        else
        {
            isInBg = false;
            MusicMgr.Instance.RePlayBgm();
        }
#endif
        }
        catch { 
            
        }
    }
    //游戏退出
    public void OnApplicationQuit() {
        //Debug.LogError("OnApplicationQuit");

    }
    public void StopData() {
        if (StopEvent != null) {
            StopEvent();
            StopEvent = null;
        }
    }
    public void ClearData()
    {
        if (CleanEvent != null) {
            CleanEvent();
        }
    }
    public void ClearDonotDestory() {
        foreach(var go in DonotDestoryGos)
        {
            DestroyImmediate(go);
        }
        DonotDestoryGos.Clear();
    }
    public void SetDebug() {

    }
    public void SaveDataEvent() {
        //Debug.LogError("SaveDataEvent");
        if (SaveEvent != null)
        {
            PlayerPrefs.Save();
            SaveEvent();
        }
    }
    public bool IsHaveConnet {
        get {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
    }
    private Coroutine CheckConnetIE;
    public void CheckIsHaveConent()
    {
        //CheckConnetIE.Stop();
        //CheckConnetIE = MonoTool.Instance.UpdateCallHaveTime(1, () => {
        //    MonoTool.Instance.StartCor(CheckIsHaveConentIE());
        //});
    }
    //private IEnumerator CheckIsHaveConentIE()
    //{
    //    using (UnityWebRequest www = UnityWebRequest.Get("https://www.baidu.com/"))
    //    {
    //        yield return www.SendWebRequest();
    //        if (www.result != UnityWebRequest.Result.Success)
    //        {
    //            IsHaveConnet = false;
    //        }
    //        else
    //        {
    //            IsHaveConnet = true;
    //        }
    //    }
    //}
}