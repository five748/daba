using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIAchTip:BaseMonoBehaviour{
    private UIAchTipAuto Auto = new UIAchTipAuto();
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
    private DataPlayer data
    {
        get {
            return PlayerMgr.Instance.data;
        }
    }
    private void Init(string param){
        UIManager.FadeOut();
        Show();
    }
    public static bool IsHaveRedOut() {
        foreach (var item in PlayerMgr.Instance.data.OnAchId)
        {
            int haveNum = GetAchNum(item.Key);
            int overNum = TableCache.Instance.achievementTable[item.Value].targetNum;
            if (haveNum >= overNum) {
                return true;
            }
        }
        return false;
    }
    private void Show() {
        Auto.StringMsc = $"最多可获得 {Mathf.CeilToInt(data.MaxOutLineTime/60.0f) + "分钟"} 离线收益";
        var lst = data.OnAchId;
        lst.ForeachForAddItems(Auto.Grid, (k, v, index, tran) =>{
            var tabOne = TableCache.Instance.achievementTable[v];
            tran.SetImage("ach/" + k, "icon");
            tran.SetText(tabOne.desc, "msc");
            var addtime = Mathf.CeilToInt(tabOne.offLine / 60.0f);
            tran.SetText($"+{addtime}分钟", "addtime");
            int haveNum = GetAchNum(k);
            int overNum = tabOne.targetNum;
            bool isCan = haveNum >= overNum;
            tran.SetActive(isCan, "btn");
            tran.SetActive(!isCan, "lock");
            tran.SetText($"{haveNum.ToString().ChangeColor(GetNumColorStr(isCan))}/{overNum}", "num");
            EventTriggerListener.Get(tran.Find("btn")).onClick = (btn) =>
            {
                Debug.Log("btn");
                Msg.Instance.Show("离线收益上限" + $"+{addtime}分钟".ChangeColor("419EA6"));
                data.OnAchId[k]++;
                data.MaxOutLineTime += tabOne.offLine;
                data.SaveToFile();
                Show();
            };
        });
    }
    public static int GetAchNum(int achType)
    {
        switch(achType)
        {
            case 1:
                return PlayerMgr.Instance.data.SumOpenShipNum;
            case 2: 
                return PlayerMgr.Instance.data.SumCleanShipNum;
            case 3:
                return PlayerMgr.Instance.data.SumCollectNum;
            case 4:
                return PlayerMgr.Instance.data.SumOverDamNum;
            case 5:
                return PlayerMgr.Instance.data.SumStaffLvNum;
        }
        return 100;
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
