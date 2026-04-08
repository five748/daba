using System.Threading;
using UnityEngine;
using System.Threading.Tasks;
using DG.Tweening;
using YuZiSdk;
using System;
using Table;
using TreeData;
using LitJson;
using System.Collections.Generic;
using GameVersionSpace;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine.XR;

public class UILogin:BaseMonoBehaviour{
    private UILoginAuto Auto = new UILoginAuto();
    public override void BaseInit(){
        ProssData.Instance.isGm = true;
        MusicMgr.Instance.Init(TableCache.Instance.bgmTable, TableCache.Instance.soundTable, true);
        TableRunData.Instance.Init();
        
        Auto.Init(transform, this);
        Auto.Btn_control.SetActive(false);
        
#if CONTROL_PANEL
        //UIGuide.ShowGuide = ControlPanelMgr.Instance.Data.ShowGuide;        
        //Auto.Btn_control.SetActive(true);
#endif
        Init(param);
    }
    //------------------------------点击方法标记请勿删除---------------------------------

    public void ClickRightMenus(GameObject button, System.Action callback){
        Debug.Log("click" + button.name);
        callback();
    }
    public void ClickBtn_login(GameObject button){
        Debug.Log("click" + button.name);
        Auto.Btn_login.gameObject.SetActive(false);
        Auto.Sliderbg.SetActive(true);
        Auto.Lastser.SetActive(false);
        Auto.Slider.fillAmount = 0;
        YuziTable.Instance.InitTable(TableCache.Instance.reportTable, TableCache.Instance.reportInfoTable, TableCache.Instance.adTable);
        SdkMgr.Instance.Login(ClickLogin);
    }
    
  
    public void ClickShiling(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.Sdk.OpenAgeTip();
    }
    public void ClickLoginbtn(GameObject button){
        Debug.Log("click" + button.name);
        button.SetActive(false);
        Auto.Userinput.SetActive(false);
        Auto.Lastser.SetActive(true);
        Auto.Btn_login.gameObject.SetActive(true);
        ClickBtn_login(button);
    }
    public void ClickSerlstbtn(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBtn_control(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIControlPanel");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        //Debug.LogError(HotMgr.Instance.buildCache.SDKChannlID);
        ProssData.Instance.UICamera = Camera.main;
        InitSdk();
        ProssData.Instance.FrameImageUseRes = true;
        ProssData.Instance.IsInEditor = false;
        Auto.Loginbtn.SetActive(true);
        UIManager.FadeOut();
    }
    private void InitOtherUI(System.Action callback) {
        NavigateMgr.Instance.StartGameCalculate();
        var mainFather = GameObject.Find("UIMainCanvas");
        AssetLoadOld.Instance.LoadPrefab("UIPrefab/UIMain", (go) =>
        {
            var uiMain = GameObject.Instantiate(go);
            uiMain.transform.SetParentOverride(mainFather.transform);
            uiMain.GetComponent<RectTransform>().SetSiblingIndex(2);
            uiMain.SetActive(true);
            uiMain.name = "UIMain";
            callback();
        });
    }

    private void InitSdk()
    {
        Auto.Btn_login.gameObject.SetActive(false);
        Auto.Sliderbg.gameObject.SetActive(false);
        if (!CheckNetIsOk()) { return; }
        
       SdkMgr.Instance.Init(isSuccess =>
        {
            if (isSuccess)
            {
                //TMSDK.Instance.S
                Debug.Log("sdk初始化成功");
            }
        });
    }


    private void ClickLogin(bool isSuccess)
    {
        if (!CheckNetIsOk()) { return; }
        if (isSuccess)
        {
#if TMSDK || DYTMSDK
            var isNew = YuziMgr.Instance.isNew;
            TMSDK.Instance.SyncPlayerInfo(isNew ? 1 : 2, YuziMgr.Instance._account.userId, "测试", "1区", "1", "1");
            if (isNew)
            {
                var tab = TableCache.Instance.taskTable[1];
                TMSDK.Instance.SDKSendGuideEvent("begin", tab.desc0 + tab.desc1 + tab.desc2, "1");
            }
#endif
            Login();
        }
        else
        {
            //Msg.Instance.Show("登录失败,请重试!");
            Auto.Btn_login.gameObject.SetActive(true);
            Auto.Sliderbg.SetActive(false);
        }
    }

    private void Login()
    {
        
        Auto.Btn_login.gameObject.SetActive(false);
        Auto.Sliderbg.SetActive(true);
        Auto.Slider.fillAmount = 0;

        SdkMgr.Instance.Sdk.Report.ReportLogin("",TimeTool.SerNowTimeInt);
        Debug.Log("登录成功");

        //Auto.Loading.fillAmount = 0.74f;
        //Auto.Loading.DOFillAmount(1f, 0.3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        //if (PlayerInfo.Instance.FirstLogin)
        //{
        //    PlayerInfo.Instance.mData.SetFirstData();
        //}
        bool isOver = false;
        InitOtherUI(() => {
            DataSetting.Instance.Init();
            //Auto.Userinput.SetActive(true);
            //开启多指操作
            Input.multiTouchEnabled = true;
            if (isOver)
            {
                OverLogin();
            }
            else
            {
                isOver = true;
            }
        });
        Auto.Slider.DOFillAmount(1f, 1.5f).OnComplete(() =>
        {
            if (isOver)
            {
                OverLogin();
            }
            else {
                isOver = true;
            }
        });
    }
    private void OverLogin() {
        //Debug.LogError(".....");
        //UIManager.UICamera.clearFlags = CameraClearFlags.Depth;
        MusicMgr.Instance.PlayBgm(1);
        // UIManager.OpenUI("UIEmpty");

        UIManager.Root.Find("UIGuide").SetActive(true);
        MonoTool.Instance.WaitTwoFrame(() =>
        {
            UIGuide.InitGuide(() =>
            {
                UIManager.OpenUI("UIEmpty");
#if CONTROL_PANEL
                    if (!UIGuide.ShowGuide)
                    {
                        UIManager.Root.Find("UIGuide").SetActive(false);
                    }
#endif
            });
        });
    }
    
    

    private bool CheckNetIsOk()
    {
#if UNITY_EDITOR
        return true;
#endif
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Msg.Instance.Show("网络未连接,退出游戏");
            MonoTool.Instance.Wait(2f,() =>
            {
                Application.Quit();
            });
            return false;
        }
        return true;
    }
    public override void Update()
    {
        MoveUVTop();
    }
    private Vector3 rollTopVec = new Vector2(-1, 1);
    private ImageRoll imageRollTop;
    private void MoveUVTop()
    {
        //imageRollTop.SetUV(rollTopVec * 20 * Time.deltaTime * 0.0009259f);
    }
}












