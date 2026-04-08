using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class UISendEnd:BaseMonoBehaviour{
    private UISendEndAuto Auto = new UISendEndAuto();
    private int _tShipId = 0;
    
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickBtn_get(GameObject button){
        Debug.Log("click" + button.name);
        NavigateMgr.Instance.GetOrderReward(_tShipId, 1, b =>
        {
            if (b)
            {
                Debug.Log($"运货 领取订单奖励 {_tShipId} 成功");
                UIManager.CloseTip();
            }
        });
    }
    public void ClickBtn_double(GameObject button){
        Debug.Log("click" + button.name);
        NavigateMgr.Instance.GetOrderReward(_tShipId, TableCache.Instance.configTable[801].param.ToInt(), b =>
        {
            if (b)
            {
                Debug.Log($"运货 领取订单奖励 {_tShipId} 成功");
                UIManager.CloseTip();
            }
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    
    private void Init(string param){
        UIManager.FadeOut();
        _tShipId = param.ToInt();

        var ship = NavigateMgr.Instance.Data.ships[_tShipId];
        var tOrder = TableCache.Instance.orderTable[ship.tOrderId];
        Auto.Reward.text = $"+{NavigateMgr.Instance.GetRewardCount(tOrder, ship)}"; //系数暂且默认为1
        Auto.Ship.localPosition = Auto.Ship_pos0.localPosition;
        Auto.Ship.DOLocalMove(Auto.Ship_pos1.localPosition, 2f);
        Auto.Txt_multiple.text = $"X{TableCache.Instance.configTable[801].param.ToInt()}倍";
    }

    public override void Destory()
    {
        Auto.Ship.DOKill();
        base.Destory();
    }
}

