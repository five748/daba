using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UICollectOneTipAuto{
    public Transform Light;
    public Transform Ribbons;
    public Image Icon;
    public Text Title;
    public Text Msc;
    public Text Name;
    public Text Price;
    public GameObject Btn_sure;
    public GameObject Btn_price;
    private string _spriteIcon;
    private string _stringTitle;
    private string _stringMsc;
    private string _stringName;
    private string _stringPrice;
    public void Init(Transform myTran, UICollectOneTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Light = myTran.Find("middle/light");
        Ribbons = myTran.Find("middle/ribbons");
        Icon = myTran.Find("middle/icon").GetComponent<Image>();
        Title = myTran.Find("middle/bg_title/title").GetComponent<Text>();
        Msc = myTran.Find("middle/msc").GetComponent<Text>();
        Name = myTran.Find("middle/name").GetComponent<Text>();
        Price = myTran.Find("middle/btn_price/price").GetComponent<Text>();
        Btn_sure = myTran.Find("middle/btn_sure").gameObject;
        Btn_price = myTran.Find("middle/btn_price").gameObject;
    }
    void AddListen(UICollectOneTip model){
        EventTriggerListener.Get(Btn_sure).onClick = model.ClickBtn_sure;
        EventTriggerListener.Get(Btn_price).onClick = model.ClickBtn_price;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/bg_title/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_sure/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_price/price");
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
    public string StringTitle{
        get { return _stringTitle; }
        set {
            if(_stringTitle == value)
                return;
            _stringTitle = value;
            SetUILabelTitle(_stringTitle);
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
    public string StringName{
        get { return _stringName; }
        set {
            if(_stringName == value)
                return;
            _stringName = value;
            SetUILabelName(_stringName);
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
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUILabelTitle(string text){
        Title.text = text;
    }
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
    private void SetUILabelName(string text){
        Name.text = text;
    }
    private void SetUILabelPrice(string text){
        Price.text = text;
    }
}
