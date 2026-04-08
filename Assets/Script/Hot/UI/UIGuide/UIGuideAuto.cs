using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIGuideAuto{
    public Transform Img_hollow;
    public Transform Img_hollow_1;
    public Transform Nd_hollow;
    public Transform Type_tip;
    public Transform Type_assembly_slip;
    public Transform Slip_finger;
    public Transform Dialogue;
    public Transform Type_indicator;
    public GameObject Btn_mask;
    public GameObject Btn_type_finger;
    public GameObject Indicator_finger;
    public void Init(Transform myTran, UIGuide model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Img_hollow = myTran.Find("middle/img_hollow");
        Img_hollow_1 = myTran.Find("middle/img_hollow_1");
        Nd_hollow = myTran.Find("middle/nd_hollow");
        Type_tip = myTran.Find("middle/type_tip");
        Type_assembly_slip = myTran.Find("middle/type_assembly_slip");
        Slip_finger = myTran.Find("middle/type_assembly_slip/slip_finger");
        Dialogue = myTran.Find("middle/dialogue");
        Type_indicator = myTran.Find("middle/type_indicator");
        Btn_mask = myTran.Find("btn_mask").gameObject;
        Btn_type_finger = myTran.Find("middle/type_indicator/btn_type_finger").gameObject;
        Indicator_finger = myTran.Find("middle/type_indicator/indicator_finger").gameObject;
    }
    void AddListen(UIGuide model){
        EventTriggerListener.Get(Btn_mask).onClick = model.ClickBtn_mask;
        EventTriggerListener.Get(Btn_type_finger).onClick = model.ClickBtn_type_finger;
        EventTriggerListener.Get(Indicator_finger).onClick = model.ClickIndicator_finger;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/type_tip/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/dialogue/nd_frame/txt_dialogue");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/dialogue/nd_frame/txt_continue");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
