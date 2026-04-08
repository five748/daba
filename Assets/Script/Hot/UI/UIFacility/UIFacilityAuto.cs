using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIFacilityAuto{
    public Transform Dam_btns_content;
    public Transform List;
    public GameObject Btn_close;
    public void Init(Transform myTran, UIFacility model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Dam_btns_content = myTran.Find("middle/dam_btns/btns/Viewport/dam_btns_content");
        List = myTran.Find("middle/list");
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UIFacility model){
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
		trf= myTran.Find("middle/dam_btns/btns/Viewport/dam_btns_content/dam_btn/bg_lock/ly/txt_lock_name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/dam_btns/btns/Viewport/dam_btns_content/dam_btn/txt_name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/Viewport/Content/item/bg_title/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/Viewport/Content/item/list_item/Viewport/list_item_content/detail/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/list/Viewport/Content/item/list_item/Viewport/list_item_content/detail/unlock/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
