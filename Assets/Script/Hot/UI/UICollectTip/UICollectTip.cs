using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;
using YuZiSdk;
using UnityEditor;
using TMPro;

public class UICollectTip:BaseMonoBehaviour{
    private UICollectTipAuto Auto = new UICollectTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickBtnad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(11, (isSucc) => {
            if (isSucc) {
                SetByAd();
                ShowAwarTip();
            }
        });
    }
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        ShowAwarTip();
    }
    private void ShowAwarTip() {
        UICollectOneTip.IsShowAttri = false;
        UIManager.OpenTipNoText("UICollectOneTip", data.collectId.ToString(), (str) => {
            data.dayCollectIndex++;
            data.SaveToFile();
            OverAward();
            ShowOne();
            Show();
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private List<int> collectLst {
        get {
            return PlayerMgr.Instance.data.collectLst;
        }
    }
    public DataPlayer data {
        get {
            return PlayerMgr.Instance.data;
        }
    }
    private int maxNum;
    private void Init(string param){
        UIManager.FadeOut();
        ShowOne();
        Show();
    }
    private void SetByAd() {
        var threeDatas = new List<int>();
        foreach (var item in TableCache.Instance.collectionTable)
        {
            if (item.Value.quality == 3) {
               
                if (!data.collectLst.Contains(item.Key))
                {
                    threeDatas.Add(item.Key);
                }
            }
        }
        if (threeDatas.Count == 0) {
            foreach (var item in TableCache.Instance.collectionTable)
            {
                if (item.Value.quality == 3)
                {
                    threeDatas.Add(item.Key);
                }
            }
        }
        data.collectId = threeDatas.GetRandomLst();
        //Debug.LogError(data.collectId);
        SetAwarNum();
    }
    private static void SetAwarNum() {
        if (PlayerMgr.Instance.data.collectId == -1)
        {
            PlayerMgr.Instance.data.collectCount = TableCache.Instance.configTable[501].param.SplitToIntArray(',').GetRandom();
        }
    }
    public static bool IsHaveRedOut() {
        if (TimeTool.SerNowUtcTimeInt > PlayerMgr.Instance.data.collectTodayTime)
        {
            //隔天
            PlayerMgr.Instance.data.collectTodayTime = TimeTool.GetTodayZeroTimeInt();
            PlayerMgr.Instance.data.dayCollectIndex = 0;
            OverAward();
            return false;
        }
        if (PlayerMgr.Instance.data.dayCollectIndex >= TableCache.Instance.collectionOutputTable.Count) {
            return false;
        }
        if (PlayerMgr.Instance.data.collectTodayTime == 0)
        {
            PlayerMgr.Instance.data.collectTodayTime = TimeTool.GetTodayZeroTimeInt();
            OverAward();
            return false;
        }
        if (TimeTool.SerNowUtcTimeInt > PlayerMgr.Instance.data.collectAwardEndTime)
        {
            return true;
        }
        return false;
    }
    private void ShowOne() {
        maxNum = TableCache.Instance.collectionOutputTable.Count;
        if (data.collectTodayTime == 0) {
            data.collectTodayTime = TimeTool.GetTodayZeroTimeInt();
            OverAward();
        }
        if (TimeTool.SerNowUtcTimeInt > data.collectTodayTime) {
            //隔天
            data.collectTodayTime = TimeTool.GetTodayZeroTimeInt();
            data.dayCollectIndex = 0;
            OverAward();
        }
        if (TimeTool.SerNowUtcTimeInt > data.collectAwardEndTime)
        {
            data.isCollectCanGet = true;
        }
        ShowOneState();
    }
    private static void OverAward() {
        int index = PlayerMgr.Instance.data.dayCollectIndex + 1;
        if (PlayerMgr.Instance.data.dayCollectIndex >= TableCache.Instance.collectionOutputTable.Count)
        {
            index = TableCache.Instance.collectionOutputTable.Count - 1;
        }
        var tabOne = TableCache.Instance.collectionOutputTable[index];
        PlayerMgr.Instance.data.collectId = tabOne.pool.GetRandomLst();
        SetAwarNum();
        PlayerMgr.Instance.data.isCollectCanGet = false;
        PlayerMgr.Instance.data.collectAwardEndTime = TimeTool.SerNowUtcTimeInt + tabOne.time;
    }
    private Coroutine Co;
    private void ShowOneState() {
        bool isOver = data.dayCollectIndex == maxNum;
        Auto.Over.SetActive(false);
        Auto.Nocan.SetActive(false);
        Auto.Can.SetActive(false);
        if (isOver) {
            Auto.Num.text = $"{data.dayCollectIndex}/{maxNum}".ChangeColor("a64141");
            Auto.Over.SetActive(true);
            return;
        }
        Auto.Num.text = $"{data.dayCollectIndex}/{maxNum}";
        if (data.isCollectCanGet)
        {
            Auto.Can.SetActive(true);
            if (data.collectId == -1)
            {
                Auto.SpriteIcon = "icon/2";
                Auto.StringName = $"钻石x{PlayerMgr.Instance.data.collectCount}".ChangeColor("a64141");
            }
            else {
                Auto.SpriteIcon = "collect/" + data.collectId;
                Auto.StringName = TableCache.Instance.collectionTable[data.collectId].name;
            }
          
        }
        else {
            Co.Stop();
            Auto.Nocan.SetActive(true);
            Co = transform.gameObject.CutTime(data.collectAwardEndTime, (str) => {
                Auto.Time.text = str;
            },
            () => {
                data.isCollectCanGet = true;
                ShowOne();
            });
        }
    }
    private void Show() {
        Auto.Nohave.SetActive(collectLst.Count == 0);
        Auto.StringMsc = $"已收藏藏品 {collectLst.Count}/{TableCache.Instance.collectionTable.Count}";
        var items = Auto.Grid.AddChilds(collectLst.Count);
        int index = -1;
        foreach (var tran in items)
        {
            index++;
            int id = collectLst[index];
            tran.SetImage("collect/" + id, "icon");
            tran.SetText(TableCache.Instance.collectionTable[id].name, "name");
            EventTriggerListener.Get(tran).onClick = (btn) =>
            {
                UICollectOneTip.IsShowAttri = true;
                UIManager.OpenTip("UICollectOneTip", id.ToString(), (str) => {
                    
                });
            };
        }
    }
    
}






