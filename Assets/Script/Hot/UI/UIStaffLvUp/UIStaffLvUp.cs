using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;

public class UIStaffLvUp:BaseMonoBehaviour{
    private UIStaffLvUpAuto Auto = new UIStaffLvUpAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickMenusMenus(GameObject button, System.Action callback){
        Debug.Log("click" + button.name);
        var chooseId = int.Parse(button.name);
        bool isLock = !ChannelMgr.Instance.Data.Dams[chooseId].unlock;
        if (isLock)
        {
            return;
        }
        ChooseDamId = chooseId;
        Show();
        callback();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Dictionary<int, StaffItem> OutData {
        get {
            return StaffMgr.Instance.data.staffs;
        }
    }
    public int Score {
        get {
            return PlayerMgr.Instance.data.score;
        }
    }
    public Dictionary<int, Dictionary<int, StaffItem>> OurDatas = new Dictionary<int, Dictionary<int, StaffItem>>();
    private int ChooseDamId
    {
        get {
            return PlayerMgr.Instance.data.ChooseMemberMenuId;
        }
        set {
            PlayerMgr.Instance.data.ChooseMemberMenuId = value;
        }
    }
    public Dictionary<int, DamItem> DamDatas
    {
        get
        {
            return ChannelMgr.Instance.Data.Dams;
        }
    }
    private void Init(string param){
        UIManager.FadeOut();
        ChooseDamId = ChannelMgr.Instance.DamId;
        SetData();
        ShowMenu();
        //Show();
    }
    public static bool IsHaveRedOut() {
        var datas = StaffMgr.Instance.data.staffs;
        foreach (var item in datas)
        {
            var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
            bool isLock = !ChannelMgr.Instance.Data.Dams[tabOne.forDamId].unlock;
            if (isLock)
            {
                continue;
            }
            var data = item.Value;
            if (!data.unlock)
            {
                continue;
            }
            bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
            if (isMax)
            {
                continue;
            }
            var needNum = int.Parse(TableCache.Instance.configTable[101].param);
            if (PlayerMgr.Instance.IsEnough(2, needNum))
            {
                return true;
            }
        }
        return false;
    }
    private void SetData() {
        foreach (var item in OutData)
        {
            var data = item.Value;
            if (!data.unlock)
            {
                continue;
            }
            var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
            if (!OurDatas.ContainsKey(tabOne.forDamId)) {
                OurDatas.Add(tabOne.forDamId, new Dictionary<int, StaffItem>());
            }
            OurDatas[tabOne.forDamId].Add(item.Key, item.Value);
        }
    }
    private void ShowMenu()
    {
        var tab = TableCache.Instance.damTable;
        bool chooseHaveRed = IsHaveRed(ChooseDamId);
        if (!chooseHaveRed)
        {
            bool isHaveRed = false;
            tab.ForeachForAddItems(Auto.Menus.transform, (k, v, index, tran) =>
            {
                tran.name = k.ToString();
                tran.SetText(v.name, "nor/text");
                tran.SetText(v.name, "choose/text");
                tran.SetText(v.name, "lock/text");
                bool isLock = !DamDatas[k].unlock;
                tran.SetActive(isLock, "lock");
                bool _isHaveRed = IsHaveRed(k);
                tran.SetActive(_isHaveRed, "red");
                if (_isHaveRed && !isHaveRed)
                {
                    isHaveRed = true;
                    ChooseDamId = k;
                }
            });
        }
        else
        {
            tab.ForeachForAddItems(Auto.Menus.transform, (k, v, index, tran) =>
            {
                tran.name = k.ToString();
                tran.SetText(v.name, "nor/text");
                tran.SetText(v.name, "choose/text");
                tran.SetText(v.name, "lock/text");
                bool isLock = !DamDatas[k].unlock;
                tran.SetActive(isLock, "lock");
            });
        }
        Auto.Menus.ClickMenu(ChooseDamId - 1);
        Auto.Scrollmenu.GetComponent<NewScrollRect>().MoveToItem(Auto.Menus.transform.GetChild(ChooseDamId - 1));
    }
    public void ShowMenuRed()
    {
        var tab = TableCache.Instance.damTable;
        tab.ForeachForAddItems(Auto.Menus.transform, (k, v, index, tran) => {
            tran.name = k.ToString();
            bool isHaveRed = IsHaveRed(k);
            tran.SetActive(isHaveRed, "red");
        });
    }
    private bool IsHaveRed(int damId) {
        if (!OurDatas.ContainsKey(damId))
        {
            return false;
        }
        var datas = OurDatas[damId];
        bool isLock = !DamDatas[damId].unlock;
        if (isLock)
        {
            return false;
        }
        foreach (var item in datas)
        {
            var data = item.Value;
            bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
            if (isMax)
            {
                continue;
            }
            var needNum = int.Parse(TableCache.Instance.configTable[101].param);
            if (PlayerMgr.Instance.IsEnough(2, needNum))
            {
                return true;
            }
        }
        return false;
    }
    private void Show() {
        ShowMenuRed();
        if (!OurDatas.ContainsKey(ChooseDamId)) {
            Auto.Scroll.SetActive(false);
            Auto.Nohave.SetActive(true);
            return;
        }
        Auto.Scroll.SetActive(true);
        Auto.Nohave.SetActive(false);
        var datas = OurDatas[ChooseDamId];
        //Debug.LogError(datas.Count);
        datas.SortFirstMiddeDown((data) => {
            bool isMax = (data.Value.oneLv + data.Value.twoLv + data.Value.threeLv + data.Value.fourLv) == 20;
            if (isMax)
            {
                return 2;
            }
            var needNum = int.Parse(TableCache.Instance.configTable[101].param);
            var isEnough = PlayerMgr.Instance.IsEnough(2, needNum);
            if (isEnough) {
                return 1;
            }
            return 0;
        });
        //Debug.LogError(datas.Count);
        datas.ForeachForAddItems(Auto.Grid, (k, v, index, tran) => {
            ShowOne(tran, v, k, index);
        });
    }
    private void ShowOne(Transform tran, StaffItem data, int k, int index) {
        //Debug.LogError(tran.name);
        var tab = TableCache.Instance.trainPropTable;
        var tabOne = TableCache.Instance.tollCollectorTable[k];
        bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
        tran.SetText(tabOne.name, "name");
        tran.SetText(tabOne.forChannelId.ToString().ChangeColor("419ea6") + "号通道", "name/channel");
        tran.SetActive(isMax, "max");
        tran.SetActive(!isMax, "btn");
        var icon = tran.Find("icon");
        icon.SetImage("staff/" + tabOne.aniId);
        icon.GetComponent<Image>().PlayLoop($"staffWait{tabOne.aniId}");
        tran.SetText($"微笑[{tab[data.oneLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.oneLv].color), "0");
        tran.SetText($"礼貌[{tab[data.twoLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.twoLv].color), "1");
        tran.SetText($"着装[{tab[data.threeLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.threeLv].color), "2");
        tran.SetText($"热情[{tab[data.fourLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.fourLv].color), "3");
        SetFatherTextColor(tran.Find("0"), TableCache.Instance.trainPropTable[data.oneLv].color);
        SetFatherTextColor(tran.Find("1"), TableCache.Instance.trainPropTable[data.twoLv].color);
        SetFatherTextColor(tran.Find("2"), TableCache.Instance.trainPropTable[data.threeLv].color);
        SetFatherTextColor(tran.Find("3"), TableCache.Instance.trainPropTable[data.fourLv].color);
        EventTriggerListener.Get(tran.Find("0")).onClick = (btn) =>
        {
            Debug.Log("0");
            ShowChoose(btn.transform, data.oneLv);
        };
        EventTriggerListener.Get(tran.Find("1")).onClick = (btn) =>
        {
            Debug.Log("1");
            ShowChoose(btn.transform, data.twoLv);
        };
        EventTriggerListener.Get(tran.Find("2")).onClick = (btn) =>
        {
            Debug.Log("2");
            ShowChoose(btn.transform, data.threeLv);
        };
        EventTriggerListener.Get(tran.Find("3")).onClick = (btn) =>
        {
            Debug.Log("3");
            ShowChoose(btn.transform, data.fourLv);
        };

        if (index == 0)
        {
            GuideMgr.Instance.BindBtn(tran.Find("btn"), tableMenu.GuideWindownBtn.staff_lv_up_first_train);
        }
        EventTriggerListener.Get(tran.Find("btn")).onClick = (btn) =>
        {
            Debug.Log("btn");
            UIManager.OpenTip("UIStaffOne", k.ToString(), (str) => {
                ShowMenuRed();
                ShowOne(tran, data, k, index);
            });
        };
    }
    private Coroutine Co;
    private void ShowChoose(Transform parent, int lv)
    {
        Auto.Choose.SetActive(true);
        Auto.Choose.SetParentOverride(parent);
        Auto.StringNum = "过闸速度" + $"+{TableCache.Instance.trainPropTable[lv].prop}%".ChangeColor("1ba37b");
        Co.Stop();
        Co = MonoTool.Instance.Wait(2, () => {
            if (this == null || Auto.Choose == null)
            {
                return;
            }
            Auto.Choose.SetActive(false);
        });
    }
    private void SetFatherTextColor(Transform tran, string color) {
        var child = tran.GetChild(0).GetComponent<Text>();
        child.text = "__________".ChangeColor(color);
    }
   
}


