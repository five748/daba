using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIOutLineTipAuto{
    public Text Num;
    public Text Msc;
    public Text Rote;
    public GameObject Close;
    public GameObject Btn;
    public GameObject Btnad;
    private string _stringNum;
    private string _stringMsc;
    private string _stringRote;
    public void Init(Transform myTran, UIOutLineTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Num = myTran.Find("middle/num").GetComponent<Text>();
        Msc = myTran.Find("middle/msc").GetComponent<Text>();
        Rote = myTran.Find("middle/rote").GetComponent<Text>();
        Close = myTran.Find("middle/close").gameObject;
        Btn = myTran.Find("middle/btn").gameObject;
        Btnad = myTran.Find("middle/btnad").gameObject;
    }
    void AddListen(UIOutLineTip model){
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
        EventTriggerListener.Get(Btnad).onClick = model.ClickBtnad;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/rote");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/choose/num");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btnad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringNum{
        get { return _stringNum; }
        set {
            if(_stringNum == value)
                return;
            _stringNum = value;
            SetUILabelNum(_stringNum);
        }
    }
    public string StringMsc{
        get { return _stringMsc; }
        set {
            if(_stringMsc == value)
                return;
            _stringMsc = value;
            SetUILabelMsc(_stringMsc);
        }
    }
    public string StringRote{
        get { return _stringRote; }
        set {
            if(_stringRote == value)
                return;
            _stringRote = value;
            SetUILabelRote(_stringRote);
        }
    }
    private void SetUILabelNum(string text){
        Num.text = text;
    }
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
    private void SetUILabelRote(string text){
        Rote.text = text;
    }
}
