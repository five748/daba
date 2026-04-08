using GameVersionSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.WebRequestMethods;

public class ProssData : Single<ProssData>
{
    private EventSystem _eventSystem;
    public EventSystem eventSystem {
        get {
            if (_eventSystem == null) {
                _eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            }
            return _eventSystem;
        }
    } 
    public int NextDayRefreshTime {
        get {
            return TimeTool.GetTodayZeroTimeInt();
        }
    }
    public GUISkin mySkyle;
    public Event guiEvent;
    public bool FrameImageUseRes = true;
    public bool isGm = false;
    public bool isOpenGMTip;
    public bool IsHaveDebug = false;
    public bool isTestLogin;
    public bool IsUseILRunTime = false;
    public bool IsInEditor = true;
    public string sceneName;
    public string gotoSceneName;
    public GameObject SingleGo;
    public bool IsOpenHot;
    public Language language;
    public Platform platform;
    public int ServerChannel;
    public int channel;
    public int VersionId;
    public int Frame = 60;
    public int timeScale = 1;
    public string HttpAddress;
    public string Token = "";
    public bool IsReLogin;
    //UI=================
    public List<string> LstUI;
    public List<string> Tips;
    public string OnUI;
    public Vector2 CanvasSize;
    public Vector2 CanvasModifyTop;
    public Vector2 CanvasModifyDown;
    public bool isSetCanvasSize;
    public Vector2 halfCanvasSize;
    public int TipsCount {
        get {
            if (Tips == null) {
                return 0;
            }
            return Tips.Count;
        }
    }
    public Material grey;
    public Transform Root;
    public Transform BattleMask;
    public Camera UICamera;
    public Camera MainCamera;
    public List<Coroutine> Cos = new List<Coroutine>();
    public bool isDragScroll;
    public Vector4 ScreenRange;
    public Vector4 ScreenRangeBig;
    public Vector4 ScreenRangeSmall;
    public Vector2 ScreenBigWH;
    public Vector2 ScreenSmallWH;
    public float loginTime;
    public float LoginTime {
        get {
            return Time.time - loginTime;
        }
    }
    public void SetFrame() {
        Application.targetFrameRate = Frame;
    }
    public Coroutine WaitIE;
    public Coroutine OutIE;
    public Transform WaitingTran;
    public bool isWaitOpen;
    public bool isTimeOutOpen;
    private bool isGoingToClose;
    public void SetCallBegin() {
        ShowWaitTip();
        ShowTimeOutTip();
    }
    public void SetCallBeginInLogin()
    {
        ShowWaitTip();
        ShowTimeOutTipInLogin();
    }
    public void SetCallOver() {
        CloseWaitTip();
        CloseTimeOut();
    }
    private void ShowWaitTip() {
        if (isWaitOpen)
        {
            return;
        }
        WaitIE.Stop();
        WaitIE = MonoTool.Instance.Wait(1f, () => {
            isGoingToClose = false;
            isWaitOpen = true;
            WaitingTran.SetActive(true);
        });
    }
    public void CloseWaitTip() {
        WaitIE.Stop();
        if (isWaitOpen)
        {
            WaitingTran.SetActive(false);
            //isGoingToClose = true;
            //MonoTool.Instance.Wait(1, () =>
            //{
            //    if (isGoingToClose)
            //    {
            //        WaitingTran.SetActive(false);
            //    }
            //});
        }
        else {
            WaitingTran.SetActive(false);
        }
        isWaitOpen = false;
    }
    private float timeOutTime = 5;
    private void ShowTimeOutTip()
    {
        if (isTimeOutOpen)
        {
            return;
        }
        //Debug.LogError("WaitTimeOut");
        OutIE.Stop();
        OutIE = MonoTool.Instance.Wait(timeOutTime, () =>
        {
            OpenTimeOutTip();
        });
    }
    public void OpenTimeOutTip() {
        if (!Application.isPlaying) {
            return;
        }
        Debug.LogError("TimeOut");
        isTimeOutOpen = true;
        IsReLogin = true;
        EventTriggerListener.IsLock = false;
        EventTriggerListener.IsCallingServer = false;
        EventTriggerListener.IsUIChanging = false;
        GameProcess.Instance.StopData();
        CloseWaitTip();
        UIManager.OpenTip("UIWarnTip", "", (str) =>
        {
            GameProcess.Instance.ClearData();
            ReLogin();
            isTimeOutOpen = false;
        });
    }
    private void ShowTimeOutTipInLogin()
    {
        if (isTimeOutOpen)
        {
            return;
        }
        //Debug.LogError("WaitTimeOut");
        CloseWaitTip();
        OutIE.Stop();
        OutIE = MonoTool.Instance.Wait(timeOutTime, () =>
        {
            SerDisConnectInLogin();
        });
    }
    public void SerDisConnect() {
        UIManager.OpenTip("UIWarnTip1", "", (str) =>
        {
            GameProcess.Instance.ClearData();
            ReLogin();
            isTimeOutOpen = false;
        });
    }
    public void SerDisConnectInLogin()
    {
        UIManager.OpenTip("UIWarnTip1", "", (str) =>
        {
           
        });
    }
    public void CloseTimeOut() {
        //Debug.LogError("WaitTimeOutOver");
        OutIE.Stop();
    }
    private void ReLogin() {
        IsReLogin = true;
        EventTriggerListener.IsLock = false;
        EventTriggerListener.IsCallingServer = false;
        EventTriggerListener.IsUIChanging = false;
        Cos.Stop();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public bool IsShowMainBattleLog = false;
    public bool IsShowMainBattleRewardLog(string messageName) {
        if (IsShowMainBattleLog) {
            return true;
        }
        if (messageName == "item_change" || messageName == "Player/main_battle_reward")
        {
            return false;
        }
        else {
            return true;
        }
    }
    public void QucikReLogin() {
        if (!Application.isPlaying)
        {
            return;
        }
        isTimeOutOpen = true;
        IsReLogin = true;
        EventTriggerListener.IsLock = false;
        EventTriggerListener.IsCallingServer = false;
        EventTriggerListener.IsUIChanging = false;
        GameProcess.Instance.StopData();
        CloseWaitTip();
        GameProcess.Instance.ClearData();
        ReLogin();
        isTimeOutOpen = false;
    }
}
