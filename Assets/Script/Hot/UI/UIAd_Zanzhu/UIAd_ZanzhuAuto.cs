using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIAd_ZanzhuAuto{
    public Text Reward;
    public GameObject Btnad;
    public GameObject Btnclose;
    private string _stringReward;
    public void Init(Transform myTran, UIAd_Zanzhu model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Reward = myTran.Find("middle/reward").GetComponent<Text>();
        Btnad = myTran.Find("middle/btnad").gameObject;
        Btnclose = myTran.Find("middle/btnclose").gameObject;
    }
    void AddListen(UIAd_Zanzhu model){
        EventTriggerListener.Get(Btnad).onClick = model.ClickBtnad;
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
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
		trf= myTran.Find("middle/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btnad/text");
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
    private void SetUILabelReward(string text){
        Reward.text = text;
    }
}
