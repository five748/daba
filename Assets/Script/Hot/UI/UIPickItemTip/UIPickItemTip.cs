using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIPickItemTip:BaseMonoBehaviour{
    private UIPickItemTipAuto Auto = new UIPickItemTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        if(id == 0)
        {
            PlayerMgr.Instance.AddItemNum(1, AddMoneyNum);
            Msg.Instance.Show($"钞票+{AddMoneyNum}");
        }
        else {
            var tabOne = TableCache.Instance.collectionTable[id];
            PlayerMgr.Instance.AddItemNum(tabOne.salePrice[0].id, tabOne.salePrice[0].num);
            Msg.Instance.Show($"{TableCache.Instance.itemTable[tabOne.salePrice[0].id].initCost}+{tabOne.salePrice[0].num}");
        }
        MusicMgr.Instance.PlaySound(3);
        UIManager.CloseTip("0");
    }
    public void ClickBtn1(GameObject button){
        Debug.Log("click" + button.name);
        if (id == 0) {
            return;
        }
        if (PlayerMgr.Instance.data.collectLst.Contains(id)) {
            Msg.Instance.Show($"{TableCache.Instance.collectionTable[id].name}已收藏，不能重复收藏!");
            return;
        }
        
        PlayerMgr.Instance.AddCollectId(id);
        Msg.Instance.Show(TableCache.Instance.collectionTable[id].desc);
        UIManager.CloseTip("1");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private int id;
    private int AddMoneyNum = 100;
    private void Init(string param){
        UIManager.FadeOut();
        id = int.Parse(param);
        var score = PlayerMgr.Instance.data.score;
        if (TableCache.Instance.channelCleanCoeTable.ContainsKey(score))
        {
            AddMoneyNum = Mathf.FloorToInt(TableCache.Instance.channelCleanCoeTable[PlayerMgr.Instance.data.score].coe * float.Parse(TableCache.Instance.configTable[701].param));
        }
        else {
            Debug.LogError("找不到养成积分:" + score);
        }
     
        Show();
    }
    private void Show() {
   
        if (id == 0)
        {
            var tabOne = TableCache.Instance.itemTable[1];
            Auto.StringTital = tabOne.initCost;
            Auto.SpriteIcon = "icon/" + 1;
            Auto.StringDes = tabOne.initCost + "+" + AddMoneyNum;
            Auto.StringTextbtn1 = $"获得+{AddMoneyNum}";
            Auto.SpriteMoney = "icon/1";
            Auto.Btn1.SetActive(false);
        }
        else {
            var tabOne = TableCache.Instance.collectionTable[id];
            Auto.StringTital = tabOne.name;
            Auto.SpriteIcon = "collect/" + id;
            Auto.StringDes = tabOne.desc;
            Auto.StringTextbtn1 = $"出售+{tabOne.salePrice[0].num}";
            bool isHave = PlayerMgr.Instance.data.collectLst.Contains(id);
            Auto.Btn1.transform.SetGrey(isHave);
            //if (isHave) {
            //    EventTriggerListener.Get(Auto.Btn1).onClick = null;
            //}
        }
       
    }
}



