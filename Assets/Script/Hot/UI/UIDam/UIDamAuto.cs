using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIDamAuto{
    public Transform Dams;
    public Transform DamViews;
    public GameObject Btn_close;
    public void Init(Transform myTran, UIDam model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Dams = myTran.Find("middle/dams");
        DamViews = myTran.Find("damViews");
        Btn_close = myTran.Find("bg_title/btn_close").gameObject;
    }
    void AddListen(UIDam model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("bg_title/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("bg_title/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("damViews/Viewport/Content/0/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("damViews/Viewport/Content/0/goto/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("damViews/Viewport/Content/0/current/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("damViews/Viewport/Content/0/score");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
