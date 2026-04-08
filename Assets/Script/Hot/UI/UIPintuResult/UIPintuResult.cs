using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using UnityEngine.UI;
using YuZiSdk;

public class UIPintuResult:BaseMonoBehaviour{
    private UIPintuResultAuto Auto = new UIPintuResultAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_get(GameObject button){
        Debug.Log("click" + button.name);
        PlayerMgr.Instance.RecivedPintuReward(config.id, count, false);
        UIManager.CloseTip("UIPintuResult");
        UIManager.CloseTip("UIPintuInfo");
    }
    public void ClickBtn_ad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(7, b =>
        {
            if (b)
            {
                PlayerMgr.Instance.RecivedPintuReward(config.id, count, true);
                UIManager.CloseTip("UIPintuResult");
                UIManager.CloseTip("UIPintuInfo"); 
            }
          
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private loadingOfCargo config;
    private int count;

    private void Init(string param){
        UIManager.FadeOut();
        var ps = param.Split("-");
        var id = ps[0].ToInt();
        var count = ps[1].ToInt();
        this.count = count;
        config = TableCache.Instance.loadingOfCargoTable[id];
        UpdateUI();
        ShowEffect();
    }

    private void UpdateUI()
    {
        var isFull = count >= config.itemId.Length;
        Auto.Btn_ad.gameObject.SetActive(!isFull);
        Auto.Btn_get.transform.localPosition = new Vector3(isFull ? 0 : -250,Auto.Btn_get.transform.localPosition.y,0);
        Auto.Perfect.gameObject.SetActive(isFull);

        Auto.Reward1.text = $"总共装载<color=#1ba37b>{count}</color>件物资";
        Auto.Reward2.text = count.ToString();
        // Auto.Perreward.text = "+" + TableCache.Instance.configTable[201].param;
        Auto.Perreward.text = "+" + config.reward;
        
    }

    private void ShowEffect()
    {
        Auto.Lihua.DOLocalMoveY(-300, 1f).SetEase(Ease.Linear).OnComplete(() =>
        {
            Auto.Lihua.gameObject.SetActive(false);
        });
    }
    
}

