using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIMainTaskTipAuto{
    public Transform Top;
    public Transform Middle;
    public Transform Noover;
    public Transform Over;
    public Transform Down;
    public Image Light;
    public Image Icon;
    public Image Awardicon;
    public Text Tital;
    public Text Num;
    public Text Msc;
    public Text Noovernum;
    public Text Overnum;
    public GameObject Close;
    public GameObject Btngoto;
    public GameObject Btn;
    private string _spriteLight;
    private string _spriteIcon;
    private string _spriteAwardicon;
    private string _stringTital;
    private string _stringNum;
    private string _stringMsc;
    private string _stringNoovernum;
    private string _stringOvernum;
    public void Init(Transform myTran, UIMainTaskTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Top = myTran.Find("top");
        Middle = myTran.Find("middle");
        Noover = myTran.Find("middle/noover");
        Over = myTran.Find("middle/over");
        Down = myTran.Find("down");
        Light = myTran.Find("middle/light").GetComponent<Image>();
        Icon = myTran.Find("middle/icon").GetComponent<Image>();
        Awardicon = myTran.Find("middle/num/awardicon").GetComponent<Image>();
        Tital = myTran.Find("middle/tital").GetComponent<Text>();
        Num = myTran.Find("middle/num").GetComponent<Text>();
        Msc = myTran.Find("middle/msc").GetComponent<Text>();
        Noovernum = myTran.Find("middle/noover/noovernum").GetComponent<Text>();
        Overnum = myTran.Find("middle/over/overnum").GetComponent<Text>();
        Close = myTran.Find("middle/close").gameObject;
        Btngoto = myTran.Find("middle/noover/btngoto").gameObject;
        Btn = myTran.Find("middle/over/btn").gameObject;
    }
    void AddListen(UIMainTaskTip model){
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
        EventTriggerListener.Get(Btngoto).onClick = model.ClickBtngoto;
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
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
		trf= myTran.Find("middle/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/noover/btngoto/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/over/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteLight{
        get { return _spriteLight; }
        set {
            if(_spriteLight == value)
                return;
            _spriteLight = value;
            SetUISpriteLight(_spriteLight);
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
    public string SpriteAwardicon{
        get { return _spriteAwardicon; }
        set {
            if(_spriteAwardicon == value)
                return;
            _spriteAwardicon = value;
            SetUISpriteAwardicon(_spriteAwardicon);
        }
    }
    public string StringTital{
        get { return _stringTital; }
        set {
            if(_stringTital == value)
                return;
            _stringTital = value;
            SetUILabelTital(_stringTital);
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
    public string StringNoovernum{
        get { return _stringNoovernum; }
        set {
            if(_stringNoovernum == value)
                return;
            _stringNoovernum = value;
            SetUILabelNoovernum(_stringNoovernum);
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
    private void SetUISpriteLight(string spriteName){
        Light.SetImage(spriteName);
    }
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUISpriteAwardicon(string spriteName){
        Awardicon.SetImage(spriteName);
    }
    private void SetUILabelTital(string text){
        Tital.text = text;
    }
    private void SetUILabelNum(string text){
        Num.text = text;
    }
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
    private void SetUILabelNoovernum(string text){
        Noovernum.text = text;
    }
    private void SetUILabelOvernum(string text){
        Overnum.text = text;
    }
}
