using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UILongShipAuto{
    public Transform Scroll;
    public Transform Grid;
    public Text Numtext;
    public GameObject Btn_close;
    private string _stringNumtext;
    public void Init(Transform myTran, UILongShip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Numtext = myTran.Find("middle/numbg/numtext").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UILongShip model){
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
		trf= myTran.Find("middle/mscbg/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/mscbg/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/numbg/numtext");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/text/num");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/adbtn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/noenough/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/des");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringNumtext{
        get { return _stringNumtext; }
        set {
            if(_stringNumtext == value)
                return;
            _stringNumtext = value;
            SetUILabelNumtext(_stringNumtext);
        }
    }
    private void SetUILabelNumtext(string text){
        Numtext.text = text;
    }
}
