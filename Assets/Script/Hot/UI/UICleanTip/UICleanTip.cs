using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEditor;
using UnityEngine.UIElements;
using TMPro;
using Random = UnityEngine.Random;
using System.Linq;

public class UICleanTip:BaseMonoBehaviour{
    private UICleanTipAuto Auto = new UICleanTipAuto();
    
    private int channel_id = Int32.MinValue;
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickItemOne(GameObject btn) {
        Debug.Log("click:" + btn.name);
        int posIndex = int.Parse(btn.name);
        var collectId = PlayerMgr.Instance.data.cleanItems[posIndex];
        UIManager.OpenTipNoText("UIPickItemTip", collectId.ToString(), (str) => {
            PlayerMgr.Instance.data.cleanItems.Remove(posIndex);
            if (str == "1")
            {
                var pmMove = btn.GetComponent<PmMove>();
                pmMove.endTransform = items[0];
                EventTriggerListener.IsLock = true;
                pmMove.callback = () => {
                    ShowOne(collectId);
                    EventTriggerListener.IsLock = false;
                };
                var scr = Auto.Scrolldown.GetComponent<NewScrollRect>();
                scr.MoveToItemBase(items[0], () => {
                    pmMove.enabled = true;
                });
            }
            else {
                btn.SetActive(false);
                OverClean();
            }
        });
    }
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        try
        {
            //todo 需要知道清理的是哪个大坝 先写死1号
            ChannelMgr.Instance.WashEnd(1, channel_id);
        }
        catch { 
            
        }
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private NewScrollRect scroll;
    private EraseMask emask;
    ///private Dictionary<int, Table.collection> tabDatas;
    private void Init(string param){
        UIManager.FadeOut();
        channel_id = param.ToInt();
        InitWater();
        InitDatas();
        InitItems();
        ShowNum();
    }
    private void InitWater() {
        scroll = Auto.Scorllwater.GetComponent<NewScrollRect>();
        emask = Auto.Rawimage.GetComponent<EraseMask>();
        scroll.OnDragBegin = emask.OnPointerDown;
        scroll.OnDraging = emask.OnDrag;
        scroll.OnDragEnd = emask.OnPointerUp;
        emask.Over = OverWater;
    }
    private void OverWater() {
        Auto.Rawimage.SetActive(false);
        Auto.Scorllwater.SetActive(false);
        MusicMgr.Instance.PlaySound(4);
    }
    private void InitDatas() {
        var data = PlayerMgr.Instance.data;
        int index = -1;
        foreach (var item in data.cleanItems)
        {
            index++;
            var posIndex = item.Key;
            var collectId = item.Value;
 
            var tran = Auto.Items.GetChild(item.Key);
            if (collectId == 0)
            {
                tran.SetImage("icon/" + 1);
            }
            else {
                tran.SetImage("collect/" + collectId);
            }
            tran.SetActive(true);
            tran.name = posIndex.ToString();
            EventTriggerListener.Get(tran).onClick = ClickItemOne;
        }
        if (data.cleanItems.Count != 4) {
            OverWater();
        }
    }
    private  List<Transform> items;
    public void InitItems() {
        items = Auto.Grid.AddChilds(4).ToList();
        OverItems();
    }
    private void OverItems() {
        foreach (var key in PlayerMgr.Instance.data.cleanOverItems)
        {
            ShowOneBase(key);
        }
    }
    private void ShowOneBase(int key) {
        var tran = items[0];
        items.RemoveAt(0);
        if (key == 0)
        {
            tran.SetImage("icon/" + 1, "icon");
            tran.SetText("钞票", "text");
        }
        else {
            tran.SetImage("collect/" + key, "icon");
            tran.SetText(TableCache.Instance.collectionTable[key].name, "text");
        }
        tran.SetActive(true, "icon");
    }
    private void ShowOne(int collectId) {
        ShowOneBase(collectId);
        PlayerMgr.Instance.data.cleanOverItems.Add(collectId);
        ShowNum();
    }
    private int addNum {
        get {
            return PlayerMgr.Instance.data.cleanOverItems.Count;
        }
    }
    private void ShowNum() {
        Auto.StringOvernum = "古董收藏:(已收集x/4)".Replace("x", addNum.ToString());
        OverClean();
    }
    private static Coroutine WaitIE;
    public static bool CheckIsHaveClean(UIEmptyAuto auto) {
        var data = PlayerMgr.Instance.data;
        if (data.IsHaveCleanShip) {
            return true;
        }
        if (WaitIE == null) {
            var tabOne = TableCache.Instance.channelCleanTable[data.CleanIndex];
            //tabOne.time = 5;
            var endTime = TimeTool.SerNowUtcTimeInt + tabOne.time;
            WaitIE = auto.Cleantime.gameObject.CutTime(endTime, (str) => {
                auto.Cleantime.text = str;
            }, () => {
                auto.Cleantime.text = "";
                data.IsHaveCleanShip = true;
                UIEmpty.Instance.ShowCleanBtn();
                data.cleanItems.Clear();
                data.cleanOverItems.Clear();
                var lst = tabOne.pool.ToList();
                var lstPos = new List<int>()
                {
                    0, 1, 2, 3, 4, 5,
                };
                for (int i = 0; i < 4; i++)
                {
                    var index = Random.Range(0, lst.Count);
                    var posIndex = Random.Range(0, lstPos.Count);
                    data.cleanItems.Add(lstPos[posIndex], lst[index]);
                    lst.RemoveAt(index);
                    lstPos.RemoveAt(posIndex);
                    //Debug.LogError(index);
                }
            });
        }
        return false;
    }
    public void OverClean() {
        if (PlayerMgr.Instance.data.cleanItems.Count != 0)
        {
            return;
        }
        //清理完成
        var data = PlayerMgr.Instance.data;
        WaitIE.Stop();
        WaitIE = null;
        data.IsHaveCleanShip = false;
        data.cleanItems.Clear();
        data.cleanOverItems.Clear();
        data.CleanIndex++;
        UIEmpty.Instance.ShowCleanBtn();
        Msg.Instance.Show("清理完毕!");
        PlayerMgr.Instance.data.SumCleanShipNum++;
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumCleanNum);
        UIManager.CloseTip();
    }
}






