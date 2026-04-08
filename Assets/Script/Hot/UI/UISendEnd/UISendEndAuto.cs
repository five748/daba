using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UISendEndAuto{
    public Transform Ship;
    public Transform Ship_pos0;
    public Transform Ship_pos1;
    public Text Reward;
    public Text Txt_multiple;
    public GameObject Btnclose;
    public GameObject Btn_get;
    public GameObject Btn_double;
    private string _stringReward;
    private string _stringTxt_multiple;
    public void Init(Transform myTran, UISendEnd model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Ship = myTran.Find("middle/ship");
        Ship_pos0 = myTran.Find("middle/ship_pos0");
        Ship_pos1 = myTran.Find("middle/ship_pos1");
        Reward = myTran.Find("middle/reward").GetComponent<Text>();
        Txt_multiple = myTran.Find("middle/btn_double/txt_multiple").GetComponent<Text>();
        Btnclose = myTran.Find("middle/btnclose").gameObject;
        Btn_get = myTran.Find("middle/btn_get").gameObject;
        Btn_double = myTran.Find("middle/btn_double").gameObject;
    }
    void AddListen(UISendEnd model){
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
        EventTriggerListener.Get(Btn_get).onClick = model.ClickBtn_get;
        EventTriggerListener.Get(Btn_double).onClick = model.ClickBtn_double;
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
		trf= myTran.Find("middle/btn_get/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_double/txt_multiple");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
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
    public string StringTxt_multiple{
        get { return _stringTxt_multiple; }
        set {
            if(_stringTxt_multiple == value)
                return;
            _stringTxt_multiple = value;
            SetUILabelTxt_multiple(_stringTxt_multiple);
        }
    }
    private void SetUILabelReward(string text){
        Reward.text = text;
    }
    private void SetUILabelTxt_multiple(string text){
        Txt_multiple.text = text;
    }
}
