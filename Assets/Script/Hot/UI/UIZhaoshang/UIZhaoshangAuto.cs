using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIZhaoshangAuto{
    public Transform Toggle;
    public Transform List;
    public Transform Info;
    public Text Name;
    public Text Reward;
    public Text Time;
    public GameObject Btnclose;
    public GameObject Btn_look;
    private string _stringName;
    private string _stringReward;
    private string _stringTime;
    public void Init(Transform myTran, UIZhaoshang model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Toggle = myTran.Find("middle/top/Viewport/toggle");
        List = myTran.Find("middle/list");
        Info = myTran.Find("middle/info");
        Name = myTran.Find("middle/info/name/name").GetComponent<Text>();
        Reward = myTran.Find("middle/info/lb/reward").GetComponent<Text>();
        Time = myTran.Find("middle/info/time/time").GetComponent<Text>();
        Btnclose = myTran.Find("middle/bg/btnclose").gameObject;
        Btn_look = myTran.Find("middle/btn_look").gameObject;
    }
    void AddListen(UIZhaoshang model){
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
        EventTriggerListener.Get(Btn_look).onClick = model.ClickBtn_look;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/bg/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/Viewport/toggle/item/name1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/Viewport/toggle/item/check/name2");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/Viewport/toggle/item/unlock/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/item/unlock/need");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_look/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/info/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/info/name/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/info/lb/reward");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/info/time");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
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
    public string StringReward{
        get { return _stringReward; }
        set {
            if(_stringReward == value)
                return;
            _stringReward = value;
            SetUILabelReward(_stringReward);
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
    private void SetUILabelName(string text){
        Name.text = text;
    }
    private void SetUILabelReward(string text){
        Reward.text = text;
    }
    private void SetUILabelTime(string text){
        Time.text = text;
    }
}
