using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UICollectTipAuto{
    public Transform Nocan;
    public Transform Can;
    public Transform Award;
    public Transform Over;
    public Transform Scroll;
    public Transform Grid;
    public Transform Nohave;
    public Image Icon;
    public Text Num;
    public Text Time;
    public Text Name;
    public Text Msc;
    public GameObject Btn_close;
    public GameObject Btnad;
    public GameObject Btn;
    private string _spriteIcon;
    private string _stringNum;
    private string _stringTime;
    private string _stringName;
    private string _stringMsc;
    public void Init(Transform myTran, UICollectTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Nocan = myTran.Find("middle/one/nocan");
        Can = myTran.Find("middle/one/can");
        Award = myTran.Find("middle/one/can/award");
        Over = myTran.Find("middle/one/over");
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Nohave = myTran.Find("middle/nohave");
        Icon = myTran.Find("middle/one/can/award/icon").GetComponent<Image>();
        Num = myTran.Find("middle/one/num").GetComponent<Text>();
        Time = myTran.Find("middle/one/nocan/time").GetComponent<Text>();
        Name = myTran.Find("middle/one/can/award/name").GetComponent<Text>();
        Msc = myTran.Find("middle/mscbg/msc").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
        Btnad = myTran.Find("middle/one/nocan/btnad").gameObject;
        Btn = myTran.Find("middle/one/can/btn").gameObject;
    }
    void AddListen(UICollectTip model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
        EventTriggerListener.Get(Btnad).onClick = model.ClickBtnad;
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/nocan/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/nocan/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/nocan/btnad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/nocan/btnad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/can/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/one/over/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/mscbg/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/nohave");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/text");
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
    public string StringNum{
        get { return _stringNum; }
        set {
            if(_stringNum == value)
                return;
            _stringNum = value;
            SetUILabelNum(_stringNum);
        }
    }
    public string StringTime{
        get { return _stringTime; }
        set {
            if(_stringTime == value)
                return;
            _stringTime = value;
            SetUILabelTime(_stringTime);
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
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUILabelNum(string text){
        Num.text = text;
    }
    private void SetUILabelTime(string text){
        Time.text = text;
    }
    private void SetUILabelName(string text){
        Name.text = text;
    }
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
}
