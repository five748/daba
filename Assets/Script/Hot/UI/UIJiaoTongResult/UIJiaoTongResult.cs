using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using UnityEngine.UI;
using YuZiSdk;

public class UIJiaoTongResult:BaseMonoBehaviour{
    private UIJiaoTongResultAuto Auto = new UIJiaoTongResultAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_get(GameObject button){
        Debug.Log("click" + button.name);
        PlayerMgr.Instance.RecivedJiaotongReward(isSuccess,config.id,false);
        UIManager.CloseTip("UIJiaoTongResult");
        UIManager.CloseTip("UIJiaoTongInfo");
    }
    public void ClickBtn_ad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(8, (b) =>
        {
            if (b)
            {
                PlayerMgr.Instance.RecivedJiaotongReward(isSuccess,config.id,true);
                UIManager.CloseTip("UIJiaoTongResult");
                UIManager.CloseTip("UIJiaoTongInfo");

            }
        });
    }
    public void ClickBtn_close2(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIJiaoTongResult");
        UIManager.CloseTip("UIJiaoTongInfo");

    }
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIJiaoTongResult");
        UIManager.CloseTip("UIJiaoTongInfo");

    }
    public void ClickBtn_failad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(8, b =>
        {
            if (b)
            {
                PlayerMgr.Instance.RecivedJiaotongReward(isSuccess,config.id,true);
                UIManager.CloseTip("UIJiaoTongResult");
                UIManager.CloseTip("UIJiaoTongInfo");
            }
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private directTraffic config;
    private bool isSuccess;
    private void Init(string param){
        UIManager.FadeOut();
        var str = param.Split("-");
        config = TableCache.Instance.directTrafficTable[str[0].ToInt()];
        isSuccess = str[1] == "1";
        UpdateUI();
        
    }

    

    private void UpdateUI()
    {
        Auto.Success.gameObject.SetActive(isSuccess);
        Auto.Fail.gameObject.SetActive(!isSuccess);
        Auto.Reward.text = $"奖励: <color=#1ba37b>{config.reward[0].num}</color>";
        Auto.Rate.text = $"{TableCache.Instance.configTable[301].param}倍领取";
        if (isSuccess)
        {
            ShowEffect();
        }
        
    }

    public override void Destory()
    {
        base.Destory();
        DOTween.Kill(Auto.Lihua);
    }

    private void ShowEffect()
    {
        Auto.Lihua.DOLocalMoveY(-300, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Auto.Lihua.gameObject.SetActive(false);
        });
    }
}


