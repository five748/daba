using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIPintuResultAuto{
    public Transform Light;
    public Transform Lihua;
    public Transform Perfect;
    public Text Title;
    public Text Reward1;
    public Text Reward2;
    public Text Perreward;
    public GameObject Btn_get;
    public GameObject Btn_ad;
    private string _stringTitle;
    private string _stringReward1;
    private string _stringReward2;
    private string _stringPerreward;
    public void Init(Transform myTran, UIPintuResult model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Light = myTran.Find("middle/light");
        Lihua = myTran.Find("middle/lihua");
        Perfect = myTran.Find("middle/perfect");
        Title = myTran.Find("middle/title").GetComponent<Text>();
        Reward1 = myTran.Find("middle/img/reward1").GetComponent<Text>();
        Reward2 = myTran.Find("middle/img/reward2").GetComponent<Text>();
        Perreward = myTran.Find("middle/perfect/perreward").GetComponent<Text>();
        Btn_get = myTran.Find("middle/btn_get").gameObject;
        Btn_ad = myTran.Find("middle/btn_ad").gameObject;
    }
    void AddListen(UIPintuResult model){
        EventTriggerListener.Get(Btn_get).onClick = model.ClickBtn_get;
        EventTriggerListener.Get(Btn_ad).onClick = model.ClickBtn_ad;
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
		trf= myTran.Find("middle/img/reward1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_get/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_ad/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/perfect/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/perfect/Image/lb (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
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
    public string StringReward1{
        get { return _stringReward1; }
        set {
            if(_stringReward1 == value)
                return;
            _stringReward1 = value;
            SetUILabelReward1(_stringReward1);
        }
    }
    public string StringReward2{
        get { return _stringReward2; }
        set {
            if(_stringReward2 == value)
                return;
            _stringReward2 = value;
            SetUILabelReward2(_stringReward2);
        }
    }
    public string StringPerreward{
        get { return _stringPerreward; }
        set {
            if(_stringPerreward == value)
                return;
            _stringPerreward = value;
            SetUILabelPerreward(_stringPerreward);
        }
    }
    private void SetUILabelTitle(string text){
        Title.text = text;
    }
    private void SetUILabelReward1(string text){
        Reward1.text = text;
    }
    private void SetUILabelReward2(string text){
        Reward2.text = text;
    }
    private void SetUILabelPerreward(string text){
        Perreward.text = text;
    }
}
