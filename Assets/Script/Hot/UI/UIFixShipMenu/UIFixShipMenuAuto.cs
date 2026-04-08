using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIFixShipMenuAuto{
    public Transform Scroll;
    public Transform Grid;
    public GameObject Btn_close;
    public void Init(Transform myTran, UIFixShipMenu model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UIFixShipMenu model){
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
		trf= myTran.Find("middle/scroll/grid/0/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/btn/numstr");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/lockstr");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
