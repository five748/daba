using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIJiaoTongInfoAuto{
    public Transform Grid;
    public Text Name;
    public Text Layer;
    public Text Reward2;
    public GameObject Btnclose;
    private string _stringName;
    private string _stringLayer;
    private string _stringReward2;
    public void Init(Transform myTran, UIJiaoTongInfo model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Grid = myTran.Find("middle/grid");
        Name = myTran.Find("title/name").GetComponent<Text>();
        Layer = myTran.Find("title/layer").GetComponent<Text>();
        Reward2 = myTran.Find("title/Image (1)/reward2").GetComponent<Text>();
        Btnclose = myTran.Find("title/btnclose").gameObject;
    }
    void AddListen(UIJiaoTongInfo model){
        EventTriggerListener.Get(Btnclose).onClick = model.ClickBtnclose;
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
		trf= myTran.Find("title/layer");
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
    public string StringLayer{
        get { return _stringLayer; }
        set {
            if(_stringLayer == value)
                return;
            _stringLayer = value;
            SetUILabelLayer(_stringLayer);
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
    private void SetUILabelLayer(string text){
        Layer.text = text;
    }
    private void SetUILabelReward2(string text){
        Reward2.text = text;
    }
}
