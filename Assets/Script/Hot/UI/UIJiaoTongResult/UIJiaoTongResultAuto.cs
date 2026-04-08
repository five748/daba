using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIJiaoTongResultAuto{
    public Transform Success;
    public Transform Light;
    public Transform Lihua;
    public Transform Fail;
    public Text Rate;
    public Text Reward;
    public GameObject Btn_get;
    public GameObject Btn_ad;
    public GameObject Btn_close2;
    public GameObject Btn_failad;
    public GameObject Btn_close;
    private string _stringRate;
    private string _stringReward;
    public void Init(Transform myTran, UIJiaoTongResult model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Success = myTran.Find("success");
        Light = myTran.Find("success/light");
        Lihua = myTran.Find("success/lihua");
        Fail = myTran.Find("fail");
        Rate = myTran.Find("success/btn_ad/rate").GetComponent<Text>();
        Reward = myTran.Find("img/reward").GetComponent<Text>();
        Btn_get = myTran.Find("success/btn_get").gameObject;
        Btn_ad = myTran.Find("success/btn_ad").gameObject;
        Btn_close2 = myTran.Find("fail/btn_close2").gameObject;
        Btn_failad = myTran.Find("fail/btn_failad").gameObject;
        Btn_close = myTran.Find("fail/btn_close").gameObject;
    }
    void AddListen(UIJiaoTongResult model){
        EventTriggerListener.Get(Btn_get).onClick = model.ClickBtn_get;
        EventTriggerListener.Get(Btn_ad).onClick = model.ClickBtn_ad;
        EventTriggerListener.Get(Btn_close2).onClick = model.ClickBtn_close2;
        EventTriggerListener.Get(Btn_failad).onClick = model.ClickBtn_failad;
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("success/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("success/btn_get/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("success/btn_ad/rate");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("fail/btn_close2/rate (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("fail/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("fail/btn_failad/rate");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("fail/btn_failad/rate (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("img/reward");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringRate{
        get { return _stringRate; }
        set {
            if(_stringRate == value)
                return;
            _stringRate = value;
            SetUILabelRate(_stringRate);
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
    private void SetUILabelRate(string text){
        Rate.text = text;
    }
    private void SetUILabelReward(string text){
        Reward.text = text;
    }
}
