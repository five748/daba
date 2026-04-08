using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UILoginTestAuto{
    public GameObject Click1;
    public GameObject Click2;
    public void Init(Transform myTran, UILoginTest model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Click1 = myTran.Find("middle/click1").gameObject;
        Click2 = myTran.Find("middle/click2").gameObject;
    }
    void AddListen(UILoginTest model){
        EventTriggerListener.Get(Click1).onClick = model.ClickClick1;
        EventTriggerListener.Get(Click2).onClick = model.ClickClick2;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;

    }
    
}
