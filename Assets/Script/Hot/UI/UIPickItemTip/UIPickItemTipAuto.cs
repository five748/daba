using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIPickItemTipAuto{
    public Transform Top;
    public Transform Middle;
    public Transform Down;
    public Image Icon;
    public Image Money;
    public Text Tital;
    public Text Des;
    public Text Textbtn1;
    public Text Textbtn2;
    public GameObject Btn;
    public GameObject Btn1;
    private string _spriteIcon;
    private string _spriteMoney;
    private string _stringTital;
    private string _stringDes;
    private string _stringTextbtn1;
    private string _stringTextbtn2;
    public void Init(Transform myTran, UIPickItemTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Top = myTran.Find("top");
        Middle = myTran.Find("middle");
        Down = myTran.Find("down");
        Icon = myTran.Find("middle/icon").GetComponent<Image>();
        Money = myTran.Find("middle/grid/btn/textbtn1/money").GetComponent<Image>();
        Tital = myTran.Find("middle/tital").GetComponent<Text>();
        Des = myTran.Find("middle/des").GetComponent<Text>();
        Textbtn1 = myTran.Find("middle/grid/btn/textbtn1").GetComponent<Text>();
        Textbtn2 = myTran.Find("middle/grid/btn1/textbtn2").GetComponent<Text>();
        Btn = myTran.Find("middle/grid/btn").gameObject;
        Btn1 = myTran.Find("middle/grid/btn1").gameObject;
    }
    void AddListen(UIPickItemTip model){
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
        EventTriggerListener.Get(Btn1).onClick = model.ClickBtn1;
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
		trf= myTran.Find("middle/des");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/grid/btn/textbtn1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/grid/btn1/textbtn2");
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
    public string SpriteMoney{
        get { return _spriteMoney; }
        set {
            if(_spriteMoney == value)
                return;
            _spriteMoney = value;
            SetUISpriteMoney(_spriteMoney);
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
    public string StringDes{
        get { return _stringDes; }
        set {
            if(_stringDes == value)
                return;
            _stringDes = value;
            SetUILabelDes(_stringDes);
        }
    }
    public string StringTextbtn1{
        get { return _stringTextbtn1; }
        set {
            if(_stringTextbtn1 == value)
                return;
            _stringTextbtn1 = value;
            SetUILabelTextbtn1(_stringTextbtn1);
        }
    }
    public string StringTextbtn2{
        get { return _stringTextbtn2; }
        set {
            if(_stringTextbtn2 == value)
                return;
            _stringTextbtn2 = value;
            SetUILabelTextbtn2(_stringTextbtn2);
        }
    }
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUISpriteMoney(string spriteName){
        Money.SetImage(spriteName);
    }
    private void SetUILabelTital(string text){
        Tital.text = text;
    }
    private void SetUILabelDes(string text){
        Des.text = text;
    }
    private void SetUILabelTextbtn1(string text){
        Textbtn1.text = text;
    }
    private void SetUILabelTextbtn2(string text){
        Textbtn2.text = text;
    }
}
