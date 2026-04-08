using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIStaffLvUpAuto{
    public Transform Scrollmenu;
    public Transform Choose;
    public Transform Scroll;
    public Transform Grid;
    public Transform Nohave;
    public Text Num;
    public MenuGrid Menus;
    public GameObject Btn_close;
    private string _stringNum;
    public void Init(Transform myTran, UIStaffLvUp model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scrollmenu = myTran.Find("middle/scrollmenu");
        Choose = myTran.Find("middle/choose");
        Scroll = myTran.Find("middle/scroll");
        Grid = myTran.Find("middle/scroll/grid");
        Nohave = myTran.Find("middle/nohave");
        Num = myTran.Find("middle/choose/num").GetComponent<Text>();
        Menus = myTran.Find("middle/scrollmenu/menus").GetComponent<MenuGrid>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
    }
    void AddListen(UIStaffLvUp model){
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
		trf= myTran.Find("middle/choose/num");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/0");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/2");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/3");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/max/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scroll/grid/0/max/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/nohave/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringNum{
        get { return _stringNum; }
        set {
            if(_stringNum == value)
                return;
            _stringNum = value;
            SetUILabelNum(_stringNum);
        }
    }
    private void SetUILabelNum(string text){
        Num.text = text;
    }
}
