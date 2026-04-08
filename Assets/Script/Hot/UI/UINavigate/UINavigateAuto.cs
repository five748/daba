using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UINavigateAuto{
    public Transform Scrollmenu;
    public Transform Company;
    public Transform Un_up_capacity;
    public Transform Un_up_output;
    public Transform Scroll_company;
    public Transform Grid_company;
    public Transform Order;
    public Transform Scroll_order;
    public Transform Grid_order;
    public Image Img_full;
    public Image Progress;
    public Text Capacity_money;
    public Text Output_money;
    public Text Progress_num;
    public Text Txt_capacity;
    public Text Txt_output;
    public Text Txt_cargo_count;
    public Text Txt_left_ship;
    public GameObject Btn_close;
    public GameObject Btn_company;
    public GameObject Btn_order;
    public GameObject Can_up_capacity;
    public GameObject Can_up_output;
    public GameObject Btn_full_ad;
    private string _spriteImg_full;
    private string _spriteProgress;
    private string _stringCapacity_money;
    private string _stringOutput_money;
    private string _stringProgress_num;
    private string _stringTxt_capacity;
    private string _stringTxt_output;
    private string _stringTxt_cargo_count;
    private string _stringTxt_left_ship;
    public void Init(Transform myTran, UINavigate model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Scrollmenu = myTran.Find("middle/scrollmenu");
        Company = myTran.Find("middle/company");
        Un_up_capacity = myTran.Find("middle/company/up_capacity/un_up_capacity");
        Un_up_output = myTran.Find("middle/company/up_output/un_up_output");
        Scroll_company = myTran.Find("middle/company/scroll_company");
        Grid_company = myTran.Find("middle/company/scroll_company/grid_company");
        Order = myTran.Find("middle/order");
        Scroll_order = myTran.Find("middle/order/scroll_order");
        Grid_order = myTran.Find("middle/order/scroll_order/grid_order");
        Img_full = myTran.Find("middle/company/img_full").GetComponent<Image>();
        Progress = myTran.Find("middle/company/progress_cargo/progress").GetComponent<Image>();
        Capacity_money = myTran.Find("middle/company/up_capacity/capacity_money").GetComponent<Text>();
        Output_money = myTran.Find("middle/company/up_output/output_money").GetComponent<Text>();
        Progress_num = myTran.Find("middle/company/progress_cargo/progress_num").GetComponent<Text>();
        Txt_capacity = myTran.Find("middle/company/txt_capacity").GetComponent<Text>();
        Txt_output = myTran.Find("middle/company/txt_output").GetComponent<Text>();
        Txt_cargo_count = myTran.Find("middle/order/txt_cargo_count").GetComponent<Text>();
        Txt_left_ship = myTran.Find("middle/order/txt_left_ship").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
        Btn_company = myTran.Find("middle/scrollmenu/btn_company").gameObject;
        Btn_order = myTran.Find("middle/scrollmenu/btn_order").gameObject;
        Can_up_capacity = myTran.Find("middle/company/up_capacity/can_up_capacity").gameObject;
        Can_up_output = myTran.Find("middle/company/up_output/can_up_output").gameObject;
        Btn_full_ad = myTran.Find("middle/company/btn_full_ad").gameObject;
    }
    void AddListen(UINavigate model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
        EventTriggerListener.Get(Btn_company).onClick = model.ClickBtn_company;
        EventTriggerListener.Get(Btn_order).onClick = model.ClickBtn_order;
        EventTriggerListener.Get(Can_up_capacity).onClick = model.ClickCan_up_capacity;
        EventTriggerListener.Get(Can_up_output).onClick = model.ClickCan_up_output;
        EventTriggerListener.Get(Btn_full_ad).onClick = model.ClickBtn_full_ad;
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
		trf= myTran.Find("middle/scrollmenu/btn_company/nor/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scrollmenu/btn_company/choose/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scrollmenu/btn_order/nor/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/scrollmenu/btn_order/choose/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/up_capacity/un_up_capacity/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/up_capacity/can_up_capacity/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/up_output/un_up_output/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/up_output/can_up_output/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/btn_full_ad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/progress_cargo/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/txt_capacity");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/txt_output");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/prop0");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/prop1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/lock/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/lock/lock/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/lock/lock/btn_unlock_ad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/lock/unenough/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/open/msc");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/open/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/company/scroll_company/grid_company/0/max");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/txt_cargo_count");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/txt_left_ship");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/prop0");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/prop1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/send/enough/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/send/unenough/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/get/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/shipping/time");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/order/scroll_order/grid_order/0/shipping/btn/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteImg_full{
        get { return _spriteImg_full; }
        set {
            if(_spriteImg_full == value)
                return;
            _spriteImg_full = value;
            SetUISpriteImg_full(_spriteImg_full);
        }
    }
    public string SpriteProgress{
        get { return _spriteProgress; }
        set {
            if(_spriteProgress == value)
                return;
            _spriteProgress = value;
            SetUISpriteProgress(_spriteProgress);
        }
    }
    public string StringCapacity_money{
        get { return _stringCapacity_money; }
        set {
            if(_stringCapacity_money == value)
                return;
            _stringCapacity_money = value;
            SetUILabelCapacity_money(_stringCapacity_money);
        }
    }
    public string StringOutput_money{
        get { return _stringOutput_money; }
        set {
            if(_stringOutput_money == value)
                return;
            _stringOutput_money = value;
            SetUILabelOutput_money(_stringOutput_money);
        }
    }
    public string StringProgress_num{
        get { return _stringProgress_num; }
        set {
            if(_stringProgress_num == value)
                return;
            _stringProgress_num = value;
            SetUILabelProgress_num(_stringProgress_num);
        }
    }
    public string StringTxt_capacity{
        get { return _stringTxt_capacity; }
        set {
            if(_stringTxt_capacity == value)
                return;
            _stringTxt_capacity = value;
            SetUILabelTxt_capacity(_stringTxt_capacity);
        }
    }
    public string StringTxt_output{
        get { return _stringTxt_output; }
        set {
            if(_stringTxt_output == value)
                return;
            _stringTxt_output = value;
            SetUILabelTxt_output(_stringTxt_output);
        }
    }
    public string StringTxt_cargo_count{
        get { return _stringTxt_cargo_count; }
        set {
            if(_stringTxt_cargo_count == value)
                return;
            _stringTxt_cargo_count = value;
            SetUILabelTxt_cargo_count(_stringTxt_cargo_count);
        }
    }
    public string StringTxt_left_ship{
        get { return _stringTxt_left_ship; }
        set {
            if(_stringTxt_left_ship == value)
                return;
            _stringTxt_left_ship = value;
            SetUILabelTxt_left_ship(_stringTxt_left_ship);
        }
    }
    private void SetUISpriteImg_full(string spriteName){
        Img_full.SetImage(spriteName);
    }
    private void SetUISpriteProgress(string spriteName){
        Progress.SetImage(spriteName);
    }
    private void SetUILabelCapacity_money(string text){
        Capacity_money.text = text;
    }
    private void SetUILabelOutput_money(string text){
        Output_money.text = text;
    }
    private void SetUILabelProgress_num(string text){
        Progress_num.text = text;
    }
    private void SetUILabelTxt_capacity(string text){
        Txt_capacity.text = text;
    }
    private void SetUILabelTxt_output(string text){
        Txt_output.text = text;
    }
    private void SetUILabelTxt_cargo_count(string text){
        Txt_cargo_count.text = text;
    }
    private void SetUILabelTxt_left_ship(string text){
        Txt_left_ship.text = text;
    }
}
