using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SelfComponent;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;
using System.Data;
using Unity.VisualScripting;
using System;
using YuZiSdk;

public class UIStaff:BaseMonoBehaviour{
    private UIStaffAuto Auto = new UIStaffAuto();

    private ListView list;
    
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
        int chooseId = int.Parse(button.name);
        bool isLock = !ChannelMgr.Instance.Data.Dams[chooseId].unlock;
        if (isLock)
        {
            return;
        }
        ChooseDamId = chooseId;
        Show(false);
        callback();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Dictionary<int, StaffItem> newDatas;
    private Dictionary<int, StaffItem> OutData
    {
        get
        {
            return StaffMgr.Instance.data.staffs;
        }
    }
    private int ChooseDamId
    {
        get
        {
            return PlayerMgr.Instance.data.ChooseStaffMenuId;
        }
        set
        {
            PlayerMgr.Instance.data.ChooseStaffMenuId = value;
        }
    }
    private int Score
    {
        get
        {
            //return 10000;
            return PlayerMgr.Instance.data.score;
        }
    }
    private int maxLv = 5;
    public Dictionary<int, Dictionary<int, StaffItem>> OurDatas = new Dictionary<int, Dictionary<int, StaffItem>>();
    public Dictionary<int, DamItem> DamDatas {
        get {
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
        foreach (var item in StaffMgr.Instance.data.staffs)
        {
            var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
            bool isLock = !ChannelMgr.Instance.Data.Dams[tabOne.forDamId].unlock;
            if (isLock) {
                continue;
            }
            if (!item.Value.unlock)
            {
                int needScore = tabOne.score;
                if (PlayerMgr.Instance.data.score >= needScore)
                {
                    return true;
                }
            }
            else {
                var haveNum = PlayerMgr.Instance.GetItemNum(2);
                if (item.Value.staff_lv < tabOne.lvupCost.Length) {
                    var needNum = tabOne.lvupCost[item.Value.staff_lv];
                    if (haveNum >= needNum)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
    private void SetData()
    {
        foreach (var item in OutData)
        {
            var data = item.Value;
            var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
            if (!OurDatas.ContainsKey(tabOne.forDamId))
            {
                OurDatas.Add(tabOne.forDamId, new Dictionary<int, StaffItem>());
            }
            OurDatas[tabOne.forDamId].Add(item.Key, item.Value);
        }
    }
    public static int MainTaskGoToId = -1;
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
        if (MainTaskGoToId != -1)
        {
            ChooseDamId = TableCache.Instance.tollCollectorTable[MainTaskGoToId].forDamId;
        }
        Auto.Scrollmenu.GetComponent<NewScrollRect>().MoveToItem(Auto.Menus.transform.GetChild(ChooseDamId - 1));
        Auto.Menus.ClickMenu(ChooseDamId - 1);
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
    private bool IsHaveRed(int damId)
    {
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
            var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
            if (!data.unlock) {
                if (PlayerMgr.Instance.data.score >= tabOne.score)
                {
                    return true;
                }
                continue;
            }
            bool isMax = data.staff_lv == maxLv;
            if (isMax)
            {
                continue;
            }
            var haveNum = PlayerMgr.Instance.GetItemNum(2);
            var needNum = tabOne.lvupCost[data.staff_lv];
            if (haveNum >= needNum)
            {
                return true;
            }
        }
        return false;
    }
    private Dictionary<int, Transform> items;
    private void Show(bool isLvUp) {
        ShowMenuRed();
        newDatas = new Dictionary<int, StaffItem>();
        StaffItem lockData = null;
        foreach (var item in OurDatas[ChooseDamId])
        {
            if (!item.Value.unlock)
            {
                var tabOne = TableCache.Instance.tollCollectorTable[item.Key];
                if (lockData == null)
                {
                    lockData = item.Value;
                    newDatas.Add(item.Key, item.Value);
                    continue;
                }
                else
                {
                    if (item.Key < lockData.staff_id)
                    {
                        lockData = item.Value;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            newDatas.Add(item.Key, item.Value);
        }
        newDatas = newDatas.SortFirstMiddeDown((data) =>
        {
            if (!data.Value.unlock)
            {
                return -1;
            }
            if (data.Value.staff_lv >= maxLv)
            {
                return 2;
            }
            return 0;
        });
        if (isLvUp) {
            int index = -1;
            foreach (var item in items)
            {
                index++;
                ShowOne(item.Key, newDatas[item.Key], index, item.Value, true);
            }
        }
        else
        {
            items = new Dictionary<int, Transform>();
            newDatas.ForeachForAddItems(Auto.Grid, (k, v, index, tran) => {
                items.Add(k, tran);
                ShowOne(k, v, index, tran, false);
            });
            if (MainTaskGoToId != -1)
            {
                var scroll = Auto.Scroll.GetComponent<NewScrollRect>();
                scroll.MoveToItem(items[MainTaskGoToId]);
                MainTaskGoToId = -1;
            }
        }
       
    }
    private void ShowOne(int k, StaffItem v, int index, Transform tran, bool isShowLv) {
        var tab = TableCache.Instance.trainPropTable;
        var tabOne = TableCache.Instance.tollCollectorTable[k];
        bool isMax = v.staff_lv == maxLv;
        tran.SetText(tabOne.name, "name");
        tran.SetActive(v.staff_lv != 0, "lvbg");
        tran.SetText("Lv." + v.staff_lv, "lvbg/lv");
        tran.SetText($"岗位{tabOne.forChannelId.ToString().ChangeColor("419ea6")}号通道", "channel");
        tran.SetActive(isMax, "max");
        tran.SetActive(!isMax, "open");
        if (!isShowLv)
        {
            var icon = tran.Find("icon");
            icon.SetImage("staff/" + tabOne.aniId);
            icon.GetComponent<Image>().PlayLoop($"staffWait{tabOne.aniId}");
        }
        tran.SetActive(v.unlock, "open");
        tran.SetActive(!v.unlock, "lock");


        tran.SetText($"收银速度{("+" + v.moneyspeed + "%").ChangeColor("419ea6")}", "speed");
        if (!v.unlock)
        {
            int needScore = tabOne.score;
            tran.SetText("需要x评分".Replace("x", needScore.ToString().ChangeColor(GetNumColorStr(Score >= needScore))), "lock/msc");
            if (Score < needScore)
            {
                tran.SetActive(true, "lock/unenough");
                tran.SetActive(false, "lock/btn");
            }
            else
            {
                tran.SetActive(false, "lock/unenough");
                tran.SetActive(true, "lock/btn");
                if (index == 0)
                {
                    GuideMgr.Instance.BindBtn(tran.Find("lock/btn"), tableMenu.GuideWindownBtn.staff_first_unlock);
                }
                EventTriggerListener.Get(tran.Find("lock/btn")).onClick = (btn) =>
                {
                    Debug.Log("open");
                    v.unlock = true;
                    StaffMgr.Instance.data.SaveToFile();
                    UIManager.CloseTip(k.ToString());
                
                };
            }
        }
        else
        {
            if (isMax)
            {
                tran.SetActive(false, "open");
                return;
            }
            var cutTime = tabOne.lvupRate[v.staff_lv];
            var haveNum = PlayerMgr.Instance.GetItemNum(2);
            var needNum = tabOne.lvupCost[v.staff_lv];
            tran.SetText($"收银速度{("+" + cutTime + "%").ChangeColor("419ea6")}", "open/msc");
            tran.SetText(needNum.ToString().ChangeColor(GetNumColorStr(haveNum >= needNum)), "open/money");
            EventTriggerListener.Get(tran.Find("open/btn")).onClick = (btn) =>
            {
                Debug.Log("uplv");
                //PlayerMgr.Instance.AddItemNum(2, needNum);
                if (PlayerMgr.Instance.AddItemNum(2, -needNum))
                {
                    v.staff_lv++;
                    v.moneyspeed += cutTime;
                    PlayerMgr.Instance.data.SumStaffLvNum++;
                    Msg.Instance.Show("升级成功!");
                    Auto.Lveffect.GetComponent<Image>().PlayOnce("lvupEffect");
                    Auto.Lveffect.SetParentOverride(tran);
                    Auto.Lveffect.SetActive(true);
                    StaffMgr.Instance.data.SaveToFile();
                    Show(true);
                    MTaskData.Instance.AddTaskNum(MainTaskMenu.SomeOneStaffMaxLv, 1, v.staff_id);
                    YuziMgr.Instance.ReportNormal(3004,0,needNum);

                }
                else
                {
                    UIManager.OpenTip("UIAd_day", "", (str) => {
                        Show(false);
                    });
                }
            };
        }
    }
    private string GetNumColorStr(bool isEnough)
    {
        if (isEnough)
        {
            return "419ea6";
        }
        else
        {
            return "a64141";
        }
    }

}



