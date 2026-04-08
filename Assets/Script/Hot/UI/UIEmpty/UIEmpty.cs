using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using Table;

public class UIEmpty:BaseMonoBehaviour{
    private UIEmptyAuto Auto = new UIEmptyAuto();
    
    private GameObjectPool _poolGood;

    private Canvas _mainCanvas;

    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_facility(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIFacility", "", (_) =>
        {
            red_facility();
        });
    }
    public void ClickBtn_staff(GameObject button){
        Debug.Log("click" + button.name);
        if (button.name != "task") {
            UIStaff.MainTaskGoToId = -1;
        }
        UIManager.OpenTip("UIStaff", "", (staffId) =>
        {
            UpDateAchRed();
            Auto.Redstaffup.SetActive(UIStaffLvUp.IsHaveRedOut());
            Auto.Redstaff.SetActive(UIStaff.IsHaveRedOut());
            if (staffId != "0")
            {
                StaffMgr.Instance.Unlock(int.Parse(staffId));
            }
        });
    }
    public void ClickScore(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIShip", "", (openShipId) =>
        {
            UIManager.CloseAllTip();
            if (openShipId != "0") {
                OpenNewShip(int.Parse(openShipId));
            }
            update_ship_btn();
            update_score(PlayerMgr.Instance.data.score);
            //todo 对应的船舶动作 在某一航道生成船舶
        });
    }
    //解锁的新船
    private void OpenNewShip(int shipId)
    {
        Debug.Log("ship:" + shipId);
        UpdateRedScoreAndDiamonds();
        UpDateAchRed();
        MTaskData.Instance.AddTaskNum(MainTaskMenu.OpenSomeOneShip, 1, shipId);
        ChannelMgr.Instance.UnlockShip(shipId);
        //航道中显示
    }
    public void ClickBtn_set(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UISettingNew");
    }
    public void ClickBtn_add_scale(GameObject button){
        Debug.Log("click" + button.name);
        MainMgr.Instance.ComCameraMove.Scale(1f);
    }
    public void ClickBtn_reduce_scale(GameObject button){
        Debug.Log("click" + button.name);
        MainMgr.Instance.ComCameraMove.Scale(-1f);
    }
    public void ClickBtn_mail(GameObject button){
        Debug.Log("click" + button.name);
        Msg.Instance.Show("敬请期待");
        //UIManager.OpenTip("UIGM");
    }
    public void ClickBtn_map(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIDam", "", (_) =>
        {
            update_map_name();
            red_map();
        });
    }
    public void ClickBtn_gm(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIGM", "", (str) =>
        {
            LockSystem.Instance.ShowSysIconActive(Auto);
            ShowOutLineAward();
        });
    }
    
    public void ClickBtn_staffup(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIStaffLvUp", "", (str) => {
            Auto.Redstaffup.SetActive(UIStaffLvUp.IsHaveRedOut());
        });
    }
    public void ClickBtn_adfuli(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIAd_Fuli");
    }
    public void ClickBtn_adday(GameObject button){
        Debug.Log("clickAdday");
        UIManager.OpenTip("UIAd_day");
    }
    public void ClickBtn_adzanzhu(GameObject button){
        Debug.Log("clickzhanzhu");
        UIManager.OpenTip("UIAd_Zanzhu","", s =>
        {
            UpdateAdRed();
        });
    }
    public void ClickBtn_longship(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UILongShip", "", (openShipId) =>
        {
            if (openShipId != "0")
            {
                OpenNewShip(int.Parse(openShipId));
            }
        });
    }
    public void ClickBtn_nurture(GameObject button){
        Debug.Log("click" + button.name);
        if (button.name != "task") {
            UINurture.MainTaskGoToId = -1;
        }
        UIManager.OpenTip("UINurture", "", (_) =>
        {
            red_nurture();
        });
    }
    public void ClickBtn_fix(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIFixShipMenu", "", (openShipId) => {
            if (openShipId != "0")
            {
                OpenNewShip(int.Parse(openShipId));
            }
            Auto.Redfix.SetActive(UIFixShipMenu.IsHaveRedOut());
        });
    }
    public void ClickBtn_clean(GameObject button){
        Debug.Log("clickclean");
        if (!PlayerMgr.Instance.data.IsHaveCleanShip) {
            Msg.Instance.Show("航道未有淤泥堵塞!");
            return;
        }
        UIManager.OpenTip("UICleanTip", "", str => {
            UpDateAchRed();
        });
    }
	public void ClickBtn_navigate(GameObject button){
        Debug.Log("click" + button.name);
        openNavigate();
    }
    public void ClickBtn_ach(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIAchTip", "", (str) => {
            UpDateAchRed();
        });
    }
    public void ClickBtn_collect(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UICollectTip", "", (str) => {
            UpDateCollectRed();
        });
    }
    public void ClickBtn_callbusiness(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIZhaoshang");
    }
    public void ClickTaskmsc(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickTaskbig(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIMainTaskTip", "", (str) => { 
            
        });
    }
    public void ClickBtn_jiaotong(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIJiaoTong","", s =>
        {
            jiaotong_red();
        });
    }
    public void ClickBtn_pintu(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIPintu","", s =>
        {
            jiaotong_red();
        });
    }
    public void ClickBtn_dy(GameObject button){
        Debug.Log("click" + button.name);
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public static UIEmpty Instance;
    private Coroutine UpDateCo;
    private void Init(string param){
        ShowTask();
        Instance = this;
        // MonoTool.Instance.StartCor(active_main());
        // ChannelMgr.Instance.FlyGold = fly_gold;
        _poolGood = new GameObjectPool("Other/FlyItem");
        Auto.Notice.gameObject.SetActive(false);
        _mainCanvas = GameObject.Find("UIMainCanvas").GetComponent<Canvas>();
        
        UIFly.Init(Auto.Fly_gold,Auto.Gold,Auto.Diamond,Auto.Notice);

        init_ui();
        update_ui();
        ShowOutLineAward();
        UpDateCo = MonoTool.Instance.UpdateCall(1, () => {
            UpDateCollectRed();
        });
        
        MonoTool.Instance.UpdateCall(1, UpdateAdRed);
        
        PlayerMgr.Instance.FunItemChange += OnItemChange;
        PlayerMgr.Instance.FunScoreChange += update_score;

        NavigateMgr.Instance.FunUpdateUICargo += update_cargo;

        ChannelMgr.Instance.UpdateEmptyDamInfo += update_map_name;
        ChannelMgr.Instance.UpdateEmptyDamInfo += red_map;
        
        //大坝解锁后 主界面相关的系统要更新红点的 在这里添加
        ChannelMgr.Instance.UpdateEmptyUnlockDamRed += red_facility;

        MainMgr.Instance.openNavigate = openNavigate;
# if DY || WX
        var goldRt = Auto.Gold.GetComponent<RectTransform>();
        goldRt.anchoredPosition = new Vector2(0, goldRt.anchoredPosition.y);
        var diamRt = Auto.Diamond.GetComponent<RectTransform>();
        diamRt.anchoredPosition = new Vector2(0, diamRt.anchoredPosition.y);
#endif

        UIManager.FadeOut();
    }
    private void ShowTask() {
        MTaskData.Instance.Init(Auto);
        LockSystem.Instance.ShowSysIconActive(Auto);
        ShowOutLineAward();
        MTaskData.Instance.TaskOverEvent += () =>
        {
            LockSystem.Instance.ShowSysIconActive(Auto);
            ShowOutLineAward();
            ShowCleanBtn();
        };
    }
    private bool isShowLineAwarded = false;
    private void ShowOutLineAward() {
        bool isOpen = LockSystem.Instance.GetSystemIsOpen(15);
        if (!isOpen) {
            return;
        }
        if (isShowLineAwarded) {
            return;
        }
        isShowLineAwarded = true;
        GameProcess.Instance.SaveEvent += () => {
            PlayerMgr.Instance.data.OutLineTime = TimeTool.SerNowUtcTimeInt;
            PlayerMgr.Instance.data.SaveToFile();
        };
        MonoTool.Instance.WaitTwoFrame(() =>
        {
            ShowOutLineAwardBase(false);
            GameProcess.Instance.OnFocusEvent = (isFocuse) =>
            {
                //Debug.LogError("焦点:" + isFocuse);
                if (isFocuse)
                {
                    //获得焦点
                    ShowOutLineAwardBase(true);
                }
                else {
                    //失去焦点
                    PlayerMgr.Instance.data.OutLineTime = TimeTool.SerNowUtcTimeInt;
                   // Debug.LogError(PlayerMgr.Instance.data.OutLineTime);
                   // Debug.LogError("DataTimeOut:" + System.DateTime.UtcNow);
                    PlayerMgr.Instance.data.SaveToFile();
                }
            };
        });
    }
    private void ShowOutLineAwardBase(bool isInFoce) {
        //Debug.LogError("DataTimeIn:" + System.DateTime.UtcNow);
        //Debug.LogError(TimeTool.SerNowUtcTimeInt  +":" + PlayerMgr.Instance.data.OutLineTime + "="  + (TimeTool.SerNowUtcTimeInt - PlayerMgr.Instance.data.OutLineTime));
        if (PlayerMgr.Instance.data.OutLineTime != 0)
        {
            if (TimeTool.SerNowUtcTimeInt - PlayerMgr.Instance.data.OutLineTime < 60)
            {
                //Debug.LogError("time:" + (TimeTool.SerNowUtcTimeInt - PlayerMgr.Instance.data.OutLineTime));
                return;
            }
            if (PlayerMgr.Instance.data.score == 0)
            {
                //Debug.LogError("Score:" + PlayerMgr.Instance.data.score);
                return;
            }
            if (isInFoce) {
                UIManager.CloseAllTip();
            }
            UIOutLineTip.outTime = TimeTool.SerNowUtcTimeInt - PlayerMgr.Instance.data.OutLineTime;
            MonoTool.Instance.WaitTwoFrame(() => {
                UIManager.OpenTipNoText("UIOutLineTip");
            });
        }
    }
    public void ShowCleanBtn()
    {
        var show = UICleanTip.CheckIsHaveClean(Auto);
        bool isOpen = LockSystem.Instance.GetSystemIsOpen(8);
        if (!isOpen)
        {
            Auto.Redclean.SetActive(false);
            return;
        }
        if (show) {
            Auto.Cleantime.text = "";
        }
        Auto.Redclean.SetActive(show);
        GuideMgr.Instance.BindBtn(show ? Auto.Btn_clean.transform.FindChildTransform("icon") : null, tableMenu.GuideWindownBtn.main_clean, Auto.Btn_clean.transform);
    }
    private void OnItemChange(int id, long num) {
        if (id == 1)
        {
            updateGoldRed();
        } 
        else if (id == 2)
        {
            UpdateRedScoreAndDiamonds();
        }
        MTaskData.Instance.ShowTask();
    }
    
    public override void Destory()
    {
        PlayerMgr.Instance.FunScoreChange -= update_score;
        PlayerMgr.Instance.FunItemChange -= OnItemChange;
        NavigateMgr.Instance.FunUpdateUICargo -= update_cargo;
        // ChannelMgr.Instance.FlyGold = null;
        if (UpDateCo != null)
        {
            UpDateCo.Stop();
            UpDateCo = null;
        }
        
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_task);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_task_get);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_ship);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_staff);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_nurture);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_call_business);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_fuli);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_clean);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_jiaotong);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_pintu);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_fix);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_navigate);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_staff_lv_up);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_dam);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_collect);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_ach);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_longzhou);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_adday);
        GuideMgr.Instance.BindBtn(null, tableMenu.GuideWindownBtn.main_facility);
        
        base.Destory();
    }
    
    //只初始化一次
    private void init_ui()
    {
        Auto.Gold.GetComponent<ItemValComponent>().SetAddCall(()=>{
            Debug.Log("Gold");
            //钞票
            UIManager.OpenTip("UIAd_Zanzhu");

        });
        Auto.Diamond.GetComponent<ItemValComponent>().SetAddCall(()=>{
            Debug.Log("Diamond");
            //钻石
            UIManager.OpenTip("UIAd_day");
        });
        update_map_name();
        update_cargo();
        ShowCleanBtn();
        bind_guide_btn();
        
        Debug.Log($"能否多指操作 {Input.multiTouchEnabled}");
    }
    
    //会被更新的内容
    private void update_ui()
    {
        update_score(PlayerMgr.Instance.data.score);
        update_ship_btn();
        UpdateAdRed();
        UpdateRedScoreAndDiamonds();
        red_facility();
        red_nurture();
        red_navigate();
        red_map();
        UpDateAchRed();
        UpDateCollectRed();
    }

    /// <summary>
    /// 金币相关的红点更新
    /// </summary>
    private void updateGoldRed()
    {
        red_nurture();
        red_facility();
    }
    
    public void UpdateRedScoreAndDiamonds() {
        Auto.Redstaffup.SetActive(UIStaffLvUp.IsHaveRedOut());
        Auto.Redship.SetActive(UIShip.IsHaveRedOut());
        Auto.Redlongship.SetActive(UILongShip.IsHaveRedOut());
        Auto.Redstaff.SetActive(UIStaff.IsHaveRedOut());
    }
    public void UpDateAchRed() {
        Auto.Redach.SetActive(UIAchTip.IsHaveRedOut());
    }
    public void UpDateCollectRed()
    {
        Auto.Redcollect.SetActive(UICollectTip.IsHaveRedOut());
    }   
    private void update_score(int num)
    {
        var shipId = ShipMgr.Instance.NextLockShipId();
        var tShip = TableCache.Instance.shipTable[shipId];
        Auto.Score_num.text = $"评分：{num.ChangeNum()}/{tShip.score}";
        // Auto.Score_num.text = $"{num.ChangeNum()}/{tShip.score}";
        Auto.Redfix.SetActive(UIFixShipMenu.IsHaveRedOut());
        jiaotong_red();
        UpdateRedScoreAndDiamonds();
        red_map();
    }
    
    private void update_ship_btn()
    {
        var shipId = ShipMgr.Instance.NextLockShipId();
        var tShip = TableCache.Instance.shipTable[shipId];
        Auto.Img_btn_ship.SetImage("ship/"+tShip.icon, false, 1, (_) =>
        {
            var tfBtnShip = Auto.Img_btn_ship.transform;
            //tfBtnShip.localScale = new Vector3(tShip.size, tShip.size);
            //var offset = new Vector3(tShip.btn_offset[0], tShip.btn_offset[1]);
            //tfBtnShip.localPosition = offset;
            //Auto.Img_btn_ship.transform.Rotate(0f, 0f, tShip.btn_rotate);
        });
    }

    // private void fly_gold(int num, Transform tfStart)
    // {
    //     int count = Random.Range(5, 6);
    //     int perNum = num / count;
    //     Vector3 randomPos = new Vector2(0, 0);
    //     
    //     //坐标转换
    //     var screenPoint = _mainCanvas.worldCamera.WorldToScreenPoint(tfStart.position);
    //     var worldPoint = UIManager.MainCamera.ScreenToWorldPoint(screenPoint);
    //     var startPosition = worldPoint;
    //     startPosition.z = UIManager.MainCamera.transform.position.z;
    //     
    //     for (int i = 0; i < count; i++)
    //     {
    //         int index = i;
    //         var go = _poolGood.GetOne();
    //         go.transform.SetParent(Auto.Fly_gold);
    //         go.transform.position = startPosition;
    //         go.SetActive(true);
    //         var position = Auto.Gold.GetComponent<ItemValComponent>().icon.transform.position;
    //         var dir = position - go.transform.position;
    //         var time = dir.magnitude / 800f + i * 0.1f;
    //         randomPos.x = Random.Range(-50f, 50f);
    //         randomPos.y = Random.Range(-50f, 50f);
    //         go.transform.DOMove(startPosition + randomPos, 0.2f).OnComplete(() =>
    //         {
    //             go.transform.DOMove(position, time).SetEase(Ease.InOutCubic).OnComplete(() =>
    //             {
    //                 if (index == 0)
    //                 {
    //                     MusicMgr.Instance.PlaySound(3);
    //                 }
    //                 _poolGood.RecOne(go);
    //                 PlayerMgr.Instance.AddItemNum(1, perNum);
    //             });
    //         });
    //     }
    // }

    private void red_facility()
    {
        Auto.Btn_facility.transform.FindChildTransform("red").SetActive(BuildMgr.Instance.CheckRed());
    }

    private void red_nurture()
    {
        Auto.Btn_nurture.transform.FindChildTransform("red").SetActive(NurtureMgr.Instance.CheckRed());
    }

    private void red_map()
    {
        Auto.Btn_map.transform.FindChildTransform("red").SetActive(ChannelMgr.Instance.CheckRed());
    }
    
    public void UpdateAdRed()
    {
        var ad = ADMgr.Instance;
        Auto.Redday.SetActive(ad.CheckDayRed());
        Auto.Redfuli.SetActive(ad.CheckFuliRed());
        Auto.Redzs.SetActive(ad.IsShowZanzhu());
        Auto.Ad_time.text = TimeTool.SerNowUtcTimeInt >= ad.data.day_time ? "" : TimeTool.GetMinSecTime(ad.data.day_time - TimeTool.SerNowUtcTimeInt);
        Auto.Redcallbusiness.SetActive(PlayerMgr.Instance.ZhaoshangRed());
    }

    private void update_map_name()
    {
        var tId = ChannelMgr.Instance.DamId == int.MinValue ? ChannelMgr.Instance.Data.finalDamId : ChannelMgr.Instance.DamId;
        var tDam = TableCache.Instance.damTable[tId];
        Auto.Btn_map.transform.FindChildTransform("text").GetComponent<Text>().text = tDam.name;
    }

    private void jiaotong_red()
    {
        // Auto.Btn_callbusiness.transform.GetChild(0).gameObject.SetActive(PlayerMgr.Instance.ZhaoshangRed());
        Auto.Redpt.SetActive(PlayerMgr.Instance.PintuRed());
        Auto.Redjt.SetActive(PlayerMgr.Instance.JiaotongRed());
    }

    private void update_cargo()
    {
        Auto.Navigate_back.SetActive(false);
        Auto.Navigate_full.SetActive(false);
        Auto.Navigate_send.SetActive(false);
        
        red_navigate();
        
        var curTime = TimeTool.CurTimeSeconds;
        foreach (var (_, ship) in NavigateMgr.Instance.Data.ships)
        {
            if (ship.lv > 0 && ship.cargoCount > 0 && ship.sendTime > 0 && ship.sendTime <= curTime)
            {
                Auto.Navigate_back.SetActive(true);
                return;
            }
        }
        
        foreach (var (_, ship) in NavigateMgr.Instance.Data.ships)
        {
            if (ship.lv > 0 && ship.cargoCount <= 0 && ship.sendTime <= 0)
            {
                if (NavigateMgr.Instance.Data.cargoCount >= ship.Capacity)
                {
                    Auto.Navigate_send.SetActive(true);
                    return;
                }
            }
        }

        if (NavigateMgr.Instance.Data.cargoCount >= NavigateMgr.Instance.Data.capacity)
        {
            Auto.Navigate_full.SetActive(true);
        }
    }

    private void red_navigate()
    {
        Auto.Btn_navigate.transform.FindChildTransform("red").SetActive(NavigateMgr.Instance.CheckRedMain());
    }

    private void openNavigate()
    {
        if (Auto.Navigate_full.gameObject.activeSelf || Auto.Btn_navigate.transform.FindChildTransform("red").gameObject.activeSelf)
        {
            UINavigate.IsGotoOrder = false;
        }

        if (Auto.Navigate_send.gameObject.activeSelf || Auto.Navigate_back.gameObject.activeSelf)
        {
            UINavigate.IsGotoOrder = true;
        }
        
        UIManager.OpenTip("UINavigate", "", (_) =>
        {
            NavigateMgr.Instance.ComNavigateShip.Reload();
            NavigateMgr.Instance.FunUpdateUICargo?.Invoke();
            red_navigate();
        });
    }

    private void bind_guide_btn()
    {
        GuideMgr.Instance.BindBtn(Auto.Task.FindChildTransform("noover/tasknum"), tableMenu.GuideWindownBtn.main_task, Auto.Task);
        GuideMgr.Instance.BindBtn(Auto.Task.FindChildTransform("over/overstr"), tableMenu.GuideWindownBtn.main_task_get, Auto.Task);
        GuideMgr.Instance.BindBtn(Auto.Score.transform, tableMenu.GuideWindownBtn.main_ship);
        GuideMgr.Instance.BindBtn(Auto.Btn_staff.transform, tableMenu.GuideWindownBtn.main_staff);
        GuideMgr.Instance.BindBtn(Auto.Btn_nurture.transform, tableMenu.GuideWindownBtn.main_nurture);
        GuideMgr.Instance.BindBtn(Auto.Btn_callbusiness.transform, tableMenu.GuideWindownBtn.main_call_business);
        GuideMgr.Instance.BindBtn(Auto.Btn_adfuli.transform, tableMenu.GuideWindownBtn.main_fuli);
        GuideMgr.Instance.BindBtn(Auto.Btn_jiaotong.transform.FindChildTransform("icon"), tableMenu.GuideWindownBtn.main_jiaotong, Auto.Btn_jiaotong.transform);
        GuideMgr.Instance.BindBtn(Auto.Btn_pintu.transform.FindChildTransform("icon"), tableMenu.GuideWindownBtn.main_pintu, Auto.Btn_pintu.transform);
        GuideMgr.Instance.BindBtn(Auto.Btn_fix.transform.FindChildTransform("icon"), tableMenu.GuideWindownBtn.main_fix, Auto.Btn_fix.transform);
        GuideMgr.Instance.BindBtn(Auto.Btn_navigate.transform, tableMenu.GuideWindownBtn.main_navigate);
        GuideMgr.Instance.BindBtn(Auto.Btn_staffup.transform, tableMenu.GuideWindownBtn.main_staff_lv_up);
        GuideMgr.Instance.BindBtn(Auto.Btn_map.transform, tableMenu.GuideWindownBtn.main_dam);
        GuideMgr.Instance.BindBtn(Auto.Btn_collect.transform, tableMenu.GuideWindownBtn.main_collect);
        GuideMgr.Instance.BindBtn(Auto.Btn_ach.transform, tableMenu.GuideWindownBtn.main_ach);
        GuideMgr.Instance.BindBtn(Auto.Btn_longship.transform, tableMenu.GuideWindownBtn.main_longzhou);
        GuideMgr.Instance.BindBtn(Auto.Btn_adday.transform, tableMenu.GuideWindownBtn.main_adday);
        GuideMgr.Instance.BindBtn(Auto.Btn_facility.transform, tableMenu.GuideWindownBtn.main_facility);
    }
}









