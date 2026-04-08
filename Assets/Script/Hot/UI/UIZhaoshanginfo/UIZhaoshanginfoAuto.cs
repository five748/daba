using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIZhaoshanginfoAuto{
    public Transform List;
    public Text Lbtips;
    public GameObject Btnclose;
    public GameObject Btn_ad;
    private string _stringLbtips;
    public void Init(Transform myTran, UIZhaoshanginfo model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        List = myTran.Find("middle/list");
        Lbtips = myTran.Find("middle/lbtips").GetComponent<Text>();
        Btnclose = myTran.Find("middle/btnclose").gameObject;
        Btn_ad = myTran.Find("middle/btn_ad").gameObject;
    }
    void AddListen(UIZhaoshanginfo model){
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
        EventTriggerListener.Get(Btn_ad).onClick = model.ClickBtn_ad;
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
		trf= myTran.Find("middle/list/item/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/item/time");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/item/reward/reward");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/item/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_ad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/lbtips");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringLbtips{
        get { return _stringLbtips; }
        set {
            if(_stringLbtips == value)
                return;
            _stringLbtips = value;
            SetUILabelLbtips(_stringLbtips);
        }
    }
    private void SetUILabelLbtips(string text){
        Lbtips.text = text;
    }
}
