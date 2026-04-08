using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIFixShipMenu:BaseMonoBehaviour{
    private UIFixShipMenuAuto Auto = new UIFixShipMenuAuto();
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
    public static UIFixShipMenu Instance;
    public int Score
    {
        get
        {
            return PlayerMgr.Instance.data.score;
        }
    }
    public Dictionary<int, List<int>> FixItems
    {
        get
        {
            return PlayerMgr.Instance.data.fixItems;
        }
    }
    private void Init(string param){
        UIManager.FadeOut();
        Instance = this;
        //PlayerMgr.Instance.data.score = 100;
        Show();
    }
    public static bool IsHaveRedOut() {
        foreach (var item in TableCache.Instance.repairDungeonTable)
        {
            var overNum = GetFixNum(item.Key);
            bool isover = overNum == 7;
            if (isover) {
                continue;
            }
            bool isOpen = PlayerMgr.Instance.data.score >= item.Value.score;
            if (isOpen)
            {
                return true;
            }
        }
        return false;
    }
    private void Show() {
        var tab = TableCache.Instance.repairDungeonTable;
        tab.ForeachForAddItems(Auto.Grid, (k, v, index, tran) =>{
            var shipTabOne = TableCache.Instance.shipTable[v.shipId];
            tran.SetImage("shipmenu/" + index, "bg");
            tran.SetText("通关解锁".ChangeWhiteColor() + shipTabOne.name, "msc");
            tran.SetText(v.title, "name");
            bool isOpen = Score >= v.score;
            tran.SetActive(!isOpen, "lock");
            tran.SetActive(isOpen, "btn");
            tran.SetText($"需{v.score.ToString().ChangeColor("f13f3f")}评分", "lock/lockstr");
            var overNum = GetFixNum(k);
            bool isover = overNum == 7; 
            tran.SetActive(isover, "over");
            tran.SetText($"进度:{overNum.ToString().ChangeColor("f13f3f")}/7","btn/numstr");
            tran.SetText(shipTabOne.toll, "money");
            if (isover) {
                tran.SetActive(false, "btn");
            }
            EventTriggerListener.Get(tran.Find("btn")).onClick = (btn) =>
            {
                Debug.Log("btn");
                UIManager.OpenTip("UIFixShipTip", k.ToString(), (str) =>{
                    Show();
                });
            };

            if (index == 0)
            {
                GuideMgr.Instance.BindBtn(tran.Find("btn"), tableMenu.GuideWindownBtn.fix_challenge);
            }
        });
    }
    private static int GetFixNum(int k) {
        if (!PlayerMgr.Instance.data.fixItems.ContainsKey(k)) {
            PlayerMgr.Instance.data.fixItems.Add(k, new List<int>());
        }
        return PlayerMgr.Instance.data.fixItems[k].Count;
    }
}
