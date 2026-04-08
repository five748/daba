using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SelfComponent;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;
using YuZiSdk;

public class UIShip:BaseMonoBehaviour{
    private UIShipAuto Auto = new UIShipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Dictionary<int, ShipItem> ShipDatas {
        get {
            return ShipMgr.Instance.data.ships;
        }
    }
    private int Score {
        get {
            //return 10000;
            return PlayerMgr.Instance.data.score;
        }
    }
    private Dictionary<int, ShipItem> newShipDatas;
    private void Init(string param){
        UIManager.FadeOut();
        Show(); 
        
        bind_guide_btn();
    }
    public static bool IsHaveRedOut() {
        foreach (var item in ShipMgr.Instance.data.ships)
        {
            var tabOne = TableCache.Instance.shipTable[item.Key];
            if (item.Value.unlock)
            {
                //open
                if (item.Value.lv < tabOne.lvupCost.Length) {
                    if (PlayerMgr.Instance.IsEnough(2, tabOne.lvupCost[item.Value.lv]))
                    {
                        return true;
                    }
                }
            }
            else {
                if (tabOne.unlockType != 1)
                {
                    continue;
                }
                //close
                if (PlayerMgr.Instance.data.score >= tabOne.score) {
                    return true;
                }
            }
        }
        return false;
    }
    private void Show() {
        newShipDatas = new Dictionary<int, ShipItem>();
        ShipItem lockship = null;
        foreach (var item in ShipDatas)
        {
            //Debug.LogError(item.Key);
            if (!item.Value.unlock)
            {
                var tabOne = TableCache.Instance.shipTable[item.Key];
                if (tabOne.unlockType != 1)
                {
                    continue;
                }
                if (lockship == null)
                {
                    lockship = item.Value;
                    newShipDatas.Add(item.Key, item.Value);
                    continue;
                }
                else
                {
                    if (item.Key < lockship.ship_id)
                    {
                        lockship = item.Value;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
            newShipDatas.Add(item.Key, item.Value);
        }
        newShipDatas = newShipDatas.SortFirstMiddeDown((data) =>
        {
            if (!data.Value.unlock) {
                return -1;
            }
            if (data.Value.lv >= 5) {
                return 2;
            }
            return 0;
        });
        var loop = Auto.Scroll.GetComponent<LoopScroll>();
        var keys = new List<int>(newShipDatas.Keys);
        loop.InitItems(newShipDatas.Count, (tran, index, isOnlyGetSize) =>
        {
            if (!isOnlyGetSize)
            {
                tran.name = index.ToString();
                var k = keys[index];
                ShowOne(k, newShipDatas[k], index, tran);
            }
            return loop.ItemDefaultSize;
        });
    }
    private void ShowOne(int k, ShipItem v, int index, Transform tran) {
        var tabOne = TableCache.Instance.shipTable[k];
        tran.SetImage("shiptype/" + (tabOne.type - 1), "name/type");
        tran.SetText(tabOne.typeName, "name/type/text");
        tran.SetActive(v.unlock, "open");
        tran.SetActive(!v.unlock, "lock");
        tran.SetActive(v.lv != 0, "lvbg");
        tran.SetText("Lv." + v.lv, "lvbg/lv");
        tran.SetImage("ship/" + tabOne.icon, "icon");
        tran.SetText(tabOne.name, "name");
        tran.SetText(tabOne.toll, "money");
        tran.SetText((tabOne.passTime - v.cutPassTime) + "秒", "time");
        tran.SetActive(false, "max");
        if (!v.unlock)
        {
            int needScore = tabOne.score;
            tran.SetText("需要x评分".Replace("x", needScore.ToString().ChangeColor(GetNumColorStr(Score >= needScore))), "lock/msc");
            if (Score < needScore)
            {
                tran.SetActive(true, "lock/unenough");
                tran.SetActive(false, "lock/lock");
            }
            else
            {
                if (index == 0)
                {
                    GuideMgr.Instance.BindBtn(tran.Find("lock/lock/btn"), tableMenu.GuideWindownBtn.ship_first_unlock);
                }
                EventTriggerListener.Get(tran.Find("lock/lock/btn")).onClick = (btn) =>
                {
                    Debug.Log("open");
                    UIManager.OpenTipNoText("UIShipUnlock", k.ToString() + "_0", (str) =>
                    {
                        UIManager.CloseAllTip();
                        ShipMgr.Instance.UnlockShip(v.ship_id);
                        _CallBack?.Invoke(str);
                    });
                };
            }
        }
        else
        {
            bool isMax = v.lv == 5;
            tran.SetActive(isMax, "max");
            tran.SetActive(!isMax, "open");
            if (isMax)
            {
                return;
            }
            var cutTime = tabOne.lvupReduceTime[v.lv];
            var haveNum = PlayerMgr.Instance.GetItemNum(2);
            var needNum = tabOne.lvupCost[v.lv];
            tran.SetText("过闸x秒".Replace("x", ("-" + cutTime).ChangeColor("419ea6")), "open/msc");
            tran.SetText(needNum.ToString().ChangeColor(GetNumColorStr(haveNum >= needNum)), "open/money");
            if (index == 1)
            {
                GuideMgr.Instance.BindBtn(tran.Find("open/btn"), tableMenu.GuideWindownBtn.ship_first_uplevel);
            }
            EventTriggerListener.Get(tran.Find("open/btn")).onClick = (btn) =>
            {
                Debug.Log("uplv");
                if (PlayerMgr.Instance.AddItemNum(2, -needNum))
                {
                    ShipMgr.Instance.ShipLvUp(v.ship_id);
                    v.cutPassTime += cutTime;
                    Msg.Instance.Show("强化成功!");
                    MTaskData.Instance.AddTaskNum(MainTaskMenu.SumShipLvNum);
                    Auto.Lveffect.GetComponent<Image>().PlayOnce("lvupEffect");
                    Auto.Lveffect.SetParentOverride(tran);
                    Auto.Lveffect.SetActive(true);
                    ShowOne(k, v, index, tran);
                    YuziMgr.Instance.ReportNormal(3003,0,needNum);

                }
                else
                {
                    UIManager.OpenTip("UIAd_day", "", (str) => {
                        Show();
                    });
                }
            };
        }
    }
    private string GetNumColorStr(bool isEnough) {
        if (isEnough)
        {
            return "419ea6";
        }
        else {
            return "a64141";
        }
    }

    private void bind_guide_btn()
    {
        GuideMgr.Instance.BindBtn(transform.parent.Find("mask/text"), tableMenu.GuideWindownBtn.ship_close);
    }
}


