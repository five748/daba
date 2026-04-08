using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIFixShipSuccTipAuto{
    public Transform Top;
    public Transform Middle;
    public Transform ShipIcon;
    public Transform Down;
    public Text Price;
    public GameObject Btn;
    private string _stringPrice;
    public void Init(Transform myTran, UIFixShipSuccTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Top = myTran.Find("top");
        Middle = myTran.Find("middle");
        ShipIcon = myTran.Find("middle/shipIcon");
        Down = myTran.Find("down");
        Price = myTran.Find("middle/price").GetComponent<Text>();
        Btn = myTran.Find("middle/btn").gameObject;
    }
    void AddListen(UIFixShipSuccTip model){
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/price");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringPrice{
        get { return _stringPrice; }
        set {
            if(_stringPrice == value)
                return;
            _stringPrice = value;
            SetUILabelPrice(_stringPrice);
        }
    }
    private void SetUILabelPrice(string text){
        Price.text = text;
    }
}
