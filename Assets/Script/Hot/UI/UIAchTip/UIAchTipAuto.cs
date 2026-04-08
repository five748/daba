using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIAchTipAuto{
    public Transform Scroll;
    public Transform Grid;
    public Text Msc;
    public GameObject Btn_close;
    private string _stringMsc;
    public void Init(Transform myTran, UIAchTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Msc = myTran.Find("middle/mscbg/msc").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UIAchTip model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
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
		trf= myTran.Find("middle/mscbg/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/addtime");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/max");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
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
    private void SetUILabelMsc(string text){
        Msc.text = text;
    }
}
