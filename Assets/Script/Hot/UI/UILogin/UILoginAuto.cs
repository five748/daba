using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UILoginAuto{
    public GameObject Left;
    public GameObject Sliderbg;
    public Transform Middle;
    public Transform Userinput;
    public Transform Username;
    public Transform Lastser;
    public Image Slider;
    public Image Loading;
    public Text Sername;
    public GameObject Btn_login;
    public GameObject Loginbtn;
    public GameObject Serlstbtn;
    public GameObject Shiling;
    public GameObject Btn_control;
    private string _spriteSlider;
    private string _spriteLoading;
    private string _stringSername;
    public void Init(Transform myTran, UILogin model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Left = myTran.Find("left").gameObject;
        Sliderbg = myTran.Find("middle/sliderbg").gameObject;
        Middle = myTran.Find("middle");
        Userinput = myTran.Find("middle/userinput");
        Username = myTran.Find("middle/userinput/username");
        Lastser = myTran.Find("middle/lastser");
        Slider = myTran.Find("middle/sliderbg/slider").GetComponent<Image>();
        Loading = myTran.Find("middle/sliderbg/loading").GetComponent<Image>();
        Sername = myTran.Find("middle/lastser/sername").GetComponent<Text>();
        Btn_login = myTran.Find("middle/btn_login").gameObject;
        Loginbtn = myTran.Find("middle/loginbtn").gameObject;
        Serlstbtn = myTran.Find("middle/lastser/serlstbtn").gameObject;
        Shiling = myTran.Find("middle/shiling").gameObject;
        Btn_control = myTran.Find("down/btn_control").gameObject;
    }
    void AddListen(UILogin model){
        EventTriggerListener.Get(Btn_login).onClick = model.ClickBtn_login;
        EventTriggerListener.Get(Loginbtn).onClick = model.ClickLoginbtn;
        EventTriggerListener.Get(Serlstbtn).onClick = model.ClickSerlstbtn;
        EventTriggerListener.Get(Shiling).onClick = model.ClickShiling;
        EventTriggerListener.Get(Btn_control).onClick = model.ClickBtn_control;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/userinput/username");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/loginbtn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/lastser/sername");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/lastser/serlstbtn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_control/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteSlider{
        get { return _spriteSlider; }
        set {
            if(_spriteSlider == value)
                return;
            _spriteSlider = value;
            SetUISpriteSlider(_spriteSlider);
        }
    }
    public string SpriteLoading{
        get { return _spriteLoading; }
        set {
            if(_spriteLoading == value)
                return;
            _spriteLoading = value;
            SetUISpriteLoading(_spriteLoading);
        }
    }
    public string StringSername{
        get { return _stringSername; }
        set {
            if(_stringSername == value)
                return;
            _stringSername = value;
            SetUILabelSername(_stringSername);
        }
    }
    private void SetUISpriteSlider(string spriteName){
        Slider.SetImage(spriteName);
    }
    private void SetUISpriteLoading(string spriteName){
        Loading.SetImage(spriteName);
    }
    private void SetUILabelSername(string text){
        Sername.text = text;
    }
}
