using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UICleanTipAuto{
    public Transform Top;
    public Transform Middle;
    public Transform Items;
    public Transform Rawimage;
    public Transform Scorllwater;
    public Transform Scrolldown;
    public Transform Grid;
    public Transform Down;
    public Text Overnum;
    public GameObject Close;
    private string _stringOvernum;
    public void Init(Transform myTran, UICleanTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Top = myTran.Find("top");
        Middle = myTran.Find("middle");
        Items = myTran.Find("middle/items");
        Rawimage = myTran.Find("middle/rawimage");
        Scorllwater = myTran.Find("middle/scorllwater");
        Scrolldown = myTran.Find("middle/scrolldown");
        Grid = myTran.Find("middle/scrolldown/grid");
        Down = myTran.Find("down");
        Overnum = myTran.Find("middle/overnum").GetComponent<Text>();
        Close = myTran.Find("middle/close").gameObject;
    }
    void AddListen(UICleanTip model){
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
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
