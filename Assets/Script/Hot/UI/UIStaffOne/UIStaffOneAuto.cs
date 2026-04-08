using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIStaffOneAuto{
    public Transform Choose;
    public Image Icon;
    public Text Name;
    public Text Msc;
    public Text Num;
    public Text Neednum;
    public GameObject Close;
    public GameObject One;
    public GameObject Two;
    public GameObject Three;
    public GameObject Four;
    public GameObject Btn;
    public GameObject Btnad;
    private string _spriteIcon;
    private string _stringName;
    private string _stringMsc;
    private string _stringNum;
    private string _stringNeednum;
    public void Init(Transform myTran, UIStaffOne model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Choose = myTran.Find("middle/choose");
        Icon = myTran.Find("middle/iconbg/icon").GetComponent<Image>();
        Name = myTran.Find("middle/name").GetComponent<Text>();
        Msc = myTran.Find("middle/msc").GetComponent<Text>();
        Num = myTran.Find("middle/choose/num").GetComponent<Text>();
        Neednum = myTran.Find("middle/btn/neednum").GetComponent<Text>();
        Close = myTran.Find("middle/close").gameObject;
        One = myTran.Find("middle/one").gameObject;
        Two = myTran.Find("middle/two").gameObject;
        Three = myTran.Find("middle/three").gameObject;
        Four = myTran.Find("middle/four").gameObject;
        Btn = myTran.Find("middle/btn").gameObject;
        Btnad = myTran.Find("middle/btnad").gameObject;
    }
    void AddListen(UIStaffOne model){
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
        EventTriggerListener.Get(One).onClick = model.ClickOne;
        EventTriggerListener.Get(Two).onClick = model.ClickTwo;
        EventTriggerListener.Get(Three).onClick = model.ClickThree;
        EventTriggerListener.Get(Four).onClick = model.ClickFour;
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
		trf= myTran.Find("middle/one");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/two");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/three");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/four");
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
    public string SpriteIcon{
        get { return _spriteIcon; }
        set {
            if(_spriteIcon == value)
                return;
            _spriteIcon = value;
            SetUISpriteIcon(_spriteIcon);
        }
    }
    public string StringName{
        get { return _stringName; }
        set {
            if(_stringName == value)
                return;
            _stringName = value;
            SetUILabelName(_stringName);
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
    public string StringNum{
        get { return _stringNum; }
        set {
            if(_stringNum == value)
                return;
            _stringNum = value;
            SetUILabelNum(_stringNum);
        }
    }
    public string StringNeednum{
        get { return _stringNeednum; }
        set {
            if(_stringNeednum == value)
                return;
            _stringNeednum = value;
            SetUILabelNeednum(_stringNeednum);
        }
    }
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUILabelName(string text){
        Name.text = text;
    }
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
    private void SetUILabelNum(string text){
        Num.text = text;
    }
    private void SetUILabelNeednum(string text){
        Neednum.text = text;
    }
}
