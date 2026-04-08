using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIPintuInfoAuto{
    public Transform List;
    public Transform Bag;
    public Transform View;
    public Transform Light;
    public Transform Block;
    public Transform Grid;
    public Transform Guide_target;
    public Text Name;
    public Text Reward1;
    public Text Reward2;
    public GameObject Btnclose;
    public GameObject Btn_sure;
    public GameObject Btn_ad;
    private string _stringName;
    private string _stringReward1;
    private string _stringReward2;
    public void Init(Transform myTran, UIPintuInfo model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        List = myTran.Find("middle/list");
        Bag = myTran.Find("middle/list/Viewport/bag");
        View = myTran.Find("middle/list/view");
        Light = myTran.Find("middle/grid/light");
        Block = myTran.Find("middle/grid/block");
        Grid = myTran.Find("middle/grid/grid");
        Guide_target = myTran.Find("middle/guide_target");
        Name = myTran.Find("title/name").GetComponent<Text>();
        Reward1 = myTran.Find("title/Image/reward1").GetComponent<Text>();
        Reward2 = myTran.Find("title/Image (1)/reward2").GetComponent<Text>();
        Btnclose = myTran.Find("title/btnclose").gameObject;
        Btn_sure = myTran.Find("down/btn_sure").gameObject;
        Btn_ad = myTran.Find("down/btn_ad").gameObject;
    }
    void AddListen(UIPintuInfo model){
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
        EventTriggerListener.Get(Btn_sure).onClick = model.ClickBtn_sure;
        EventTriggerListener.Get(Btn_ad).onClick = model.ClickBtn_ad;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("title/name");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("title/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("title/Image/reward1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("title/Image (1)/reward2");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_sure/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_ad/lb");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringName{
        get { return _stringName; }
        set {
            if(_stringName == value)
                return;
            _stringName = value;
            SetUILabelName(_stringName);
        }
    }
    public string StringReward1{
        get { return _stringReward1; }
        set {
            if(_stringReward1 == value)
                return;
            _stringReward1 = value;
            SetUILabelReward1(_stringReward1);
        }
    }
    public string StringReward2{
        get { return _stringReward2; }
        set {
            if(_stringReward2 == value)
                return;
            _stringReward2 = value;
            SetUILabelReward2(_stringReward2);
        }
    }
    private void SetUILabelName(string text){
        Name.text = text;
    }
    private void SetUILabelReward1(string text){
        Reward1.text = text;
    }
    private void SetUILabelReward2(string text){
        Reward2.text = text;
    }
}
