using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;

public class UICollectOneTip:BaseMonoBehaviour{
    private UICollectOneTipAuto Auto = new UICollectOneTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_sure(GameObject button){
        Debug.Log("click" + button.name);
        if (!IsShowAttri)
        {
            PlayerMgr.Instance.AddCollectId(collectId);
        }
        UIManager.CloseTip();
    }
    public void ClickBtn_price(GameObject button){
        Debug.Log("click" + button.name);
        if (collectId == -1)
        {
            PlayerMgr.Instance.AddItemNum(2, PlayerMgr.Instance.data.collectCount);
        }
        else {
            PlayerMgr.Instance.AddCollectId(collectId);
        }
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private int collectId;
    public static bool IsShowAttri;
    private void Init(string param){
        UIManager.FadeOut();
        collectId = int.Parse(param);
        if (collectId == -1)
        {
            var tabOne = TableCache.Instance.itemTable[2];
            Auto.SpriteIcon = "icon/2";
            Auto.StringName = tabOne.initCost;
            Auto.StringMsc = tabOne.desc;
            Auto.Btn_sure.SetActive(false);
            Auto.Btn_price.SetActive(true);
            Auto.StringPrice = $"获得x{PlayerMgr.Instance.data.collectCount}";

        }
        else {
            var tabOne = TableCache.Instance.collectionTable[collectId];
            Auto.SpriteIcon = "collect/" + collectId;
            Auto.StringName = tabOne.name;
            Auto.StringMsc = tabOne.desc;
            bool isHave = PlayerMgr.Instance.data.collectLst.Contains(collectId);
            Auto.Btn_sure.SetActive(!isHave);
            Auto.Btn_price.SetActive(isHave);
            Auto.StringPrice = $"出售+{tabOne.salePrice[0].num}";
        }
        if (IsShowAttri) {
            Auto.Btn_sure.SetActive(true);
            Auto.Btn_price.SetActive(false);
            Auto.Title.text = "藏品属性";
        }
    }
}


