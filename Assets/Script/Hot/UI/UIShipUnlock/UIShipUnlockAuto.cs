using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIShipUnlockAuto{
    public Transform Light;
    public Transform Ribbons;
    public Image Icon;
    public Image Img_item;
    public Text Title;
    public Text Des;
    public Text Money;
    public Text Name_lv;
    public GameObject Btn_sure;
    private string _spriteIcon;
    private string _spriteImg_item;
    private string _stringTitle;
    private string _stringDes;
    private string _stringMoney;
    private string _stringName_lv;
    public void Init(Transform myTran, UIShipUnlock model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Light = myTran.Find("middle/light");
        Ribbons = myTran.Find("middle/ribbons");
        Icon = myTran.Find("middle/icon").GetComponent<Image>();
        Img_item = myTran.Find("middle/collect/img_item").GetComponent<Image>();
        Title = myTran.Find("middle/bg_title/title").GetComponent<Text>();
        Des = myTran.Find("middle/collect/des").GetComponent<Text>();
        Money = myTran.Find("middle/collect/money").GetComponent<Text>();
        Name_lv = myTran.Find("middle/name_lv").GetComponent<Text>();
        Btn_sure = myTran.Find("middle/btn_sure").gameObject;
    }
    void AddListen(UIShipUnlock model){
        EventTriggerListener.Get(Btn_sure).onClick = model.ClickBtn_sure;
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
		trf= myTran.Find("middle/collect/des");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_sure/text");
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
    public string SpriteImg_item{
        get { return _spriteImg_item; }
        set {
            if(_spriteImg_item == value)
                return;
            _spriteImg_item = value;
            SetUISpriteImg_item(_spriteImg_item);
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
    public string StringDes{
        get { return _stringDes; }
        set {
            if(_stringDes == value)
                return;
            _stringDes = value;
            SetUILabelDes(_stringDes);
        }
    }
    public string StringMoney{
        get { return _stringMoney; }
        set {
            if(_stringMoney == value)
                return;
            _stringMoney = value;
            SetUILabelMoney(_stringMoney);
        }
    }
    public string StringName_lv{
        get { return _stringName_lv; }
        set {
            if(_stringName_lv == value)
                return;
            _stringName_lv = value;
            SetUILabelName_lv(_stringName_lv);
        }
    }
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUISpriteImg_item(string spriteName){
        Img_item.SetImage(spriteName);
    }
    private void SetUILabelTitle(string text){
        Title.text = text;
    }
    private void SetUILabelDes(string text){
        Des.text = text;
    }
    private void SetUILabelMoney(string text){
        Money.text = text;
    }
    private void SetUILabelName_lv(string text){
        Name_lv.text = text;
    }
}
