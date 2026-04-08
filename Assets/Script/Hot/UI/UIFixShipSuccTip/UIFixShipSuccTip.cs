using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIFixShipSuccTip:BaseMonoBehaviour{
    private UIFixShipSuccTipAuto Auto = new UIFixShipSuccTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        Auto.StringPrice = "检修费用:x".Replace("x", Random.Range(20, 50).ToString().ChangeColor("419ea6"));
    }
}

