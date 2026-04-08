using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIStaffAuto{
    public Transform Scrollmenu;
    public Transform Scroll;
    public Transform Grid;
    public Transform Lveffect;
    public MenuGrid Menus;
    public GameObject Btn_close;
    public void Init(Transform myTran, UIStaff model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scrollmenu = myTran.Find("middle/scrollmenu");
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Lveffect = myTran.Find("middle/lveffect");
        Menus = myTran.Find("middle/scrollmenu/menus").GetComponent<MenuGrid>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UIStaff model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
        Menus.ClickEvent = model.ClickMenusMenus;
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
		trf= myTran.Find("middle/scrollmenu/menus/0/nor/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scrollmenu/menus/0/choose/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scrollmenu/menus/0/lock/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/channel");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/speed");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/open/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/open/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/lock/unenough/btn/text");
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
    
}
