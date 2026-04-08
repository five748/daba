using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UISettingNewAuto{
    public GameObject Btn_music;
    public GameObject Btn_sound;
    public GameObject Close;
    public GameObject Yinsi;
    public void Init(Transform myTran, UISettingNew model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Btn_music = myTran.Find("middle/music/music/btn_music").gameObject;
        Btn_sound = myTran.Find("middle/music/sound/btn_sound").gameObject;
        Close = myTran.Find("middle/close").gameObject;
        Yinsi = myTran.Find("middle/yinsi").gameObject;
    }
    void AddListen(UISettingNew model){
        EventTriggerListener.Get(Btn_music).onClick = model.ClickBtn_music;
        EventTriggerListener.Get(Btn_sound).onClick = model.ClickBtn_sound;
        EventTriggerListener.Get(Close).onClick = model.ClickClose;
        EventTriggerListener.Get(Yinsi).onClick = model.ClickYinsi;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/music/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/music/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/sound/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    
}
