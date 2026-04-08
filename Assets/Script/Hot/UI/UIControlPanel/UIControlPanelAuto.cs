using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIControlPanelAuto{
    public Text Txt_guide_state;
    public GameObject Btn_close;
    public GameObject Btn_guide_open;
    public GameObject Btn_guide_close;
    private string _stringTxt_guide_state;
    public void Init(Transform myTran, UIControlPanel model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Txt_guide_state = myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/txt_guide_state").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
        Btn_guide_open = myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/btn_guide_open").gameObject;
        Btn_guide_close = myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/btn_guide_close").gameObject;
    }
    void AddListen(UIControlPanel model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
        EventTriggerListener.Get(Btn_guide_open).onClick = model.ClickBtn_guide_open;
        EventTriggerListener.Get(Btn_guide_close).onClick = model.ClickBtn_guide_close;
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
		trf= myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/txt_guide_state");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/btn_guide_open/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/Scroll View/Viewport/Content/panel/引导/btn_guide_close/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringTxt_guide_state{
        get { return _stringTxt_guide_state; }
        set {
            if(_stringTxt_guide_state == value)
                return;
            _stringTxt_guide_state = value;
            SetUILabelTxt_guide_state(_stringTxt_guide_state);
        }
    }
    private void SetUILabelTxt_guide_state(string text){
        Txt_guide_state.text = text;
    }
}
