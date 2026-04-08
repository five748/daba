using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIDYOutSideTipAuto{
    public Transform Grid;
    public GameObject Btn;
    public GameObject Btnaward;
    public void Init(Transform myTran, UIDYOutSideTip model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Grid = myTran.Find("middle/three/grid");
        Btn = myTran.Find("middle/btn").gameObject;
        Btnaward = myTran.Find("middle/btnaward").gameObject;
    }
    void AddListen(UIDYOutSideTip model){
        EventTriggerListener.Get(Btn).onClick = model.ClickBtn;
        EventTriggerListener.Get(Btnaward).onClick = model.ClickBtnaward;
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
		trf= myTran.Find("middle/two/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/three/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/three/grid/0/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btnaward/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
