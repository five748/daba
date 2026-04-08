using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIFixShipTipAuto{
    public Transform Top;
    public Transform Middle;
    public Transform Shipdown;
    public Transform Shipdtop;
    public Transform Pos1;
    public Transform Pos2;
    public Transform Pos3;
    public Transform Scrolldown;
    public Transform Grid;
    public Transform Down;
    public Text Overnum;
    public GameObject Close;
    public GameObject Bigger;
    public GameObject Mask;
    private string _stringOvernum;
    public void Init(Transform myTran, UIFixShipTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Top = myTran.Find("top");
        Middle = myTran.Find("middle");
        Shipdown = myTran.Find("middle/shipdown");
        Shipdtop = myTran.Find("middle/scroll/bigger/mask/shipdtop");
        Pos1 = myTran.Find("middle/scroll/bigger/mask/pos1");
        Pos2 = myTran.Find("middle/scroll/bigger/mask/pos2");
        Pos3 = myTran.Find("middle/scroll/bigger/mask/pos3");
        Scrolldown = myTran.Find("middle/scrolldown");
        Grid = myTran.Find("middle/scrolldown/grid");
        Down = myTran.Find("down");
        Overnum = myTran.Find("middle/overnum").GetComponent<Text>();
        Close = myTran.Find("middle/close").gameObject;
        Bigger = myTran.Find("middle/scroll/bigger").gameObject;
        Mask = myTran.Find("middle/scroll/bigger/mask").gameObject;
    }
    void AddListen(UIFixShipTip model){
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
        EventTriggerListener.Get(Bigger).onClick = model.ClickBigger;
        EventTriggerListener.Get(Mask).onClick = model.ClickMask;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/tital");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/overnum");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringOvernum{
        get { return _stringOvernum; }
        set {
            if(_stringOvernum == value)
                return;
            _stringOvernum = value;
            SetUILabelOvernum(_stringOvernum);
        }
    }
    private void SetUILabelOvernum(string text){
        Overnum.text = text;
    }
}
