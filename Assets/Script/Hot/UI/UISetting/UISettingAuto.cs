using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UISettingAuto{
    public Transform Layout;
    public Transform Toggle;
    public Text Uid;
    public Text Server;
    public GameObject Btn_server;
    public GameObject Btn_copy;
    public GameObject Btn_music;
    public GameObject Btn_sound;
    public GameObject Btn_shake;
    public GameObject Btn_gm;
    public GameObject Btn_libao;
    public GameObject Btn_yinsi;
    public GameObject Btn_luntan;
    private string _stringUid;
    private string _stringServer;
    public void Init(Transform myTran, UISetting model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Layout = myTran.Find("middle/message/layout");
        Toggle = myTran.Find("middle/luanage/toggle");
        Uid = myTran.Find("middle/top/Uid").GetComponent<Text>();
        Server = myTran.Find("middle/top/server").GetComponent<Text>();
        Btn_server = myTran.Find("middle/top/btn_server").gameObject;
        Btn_copy = myTran.Find("middle/top/btn_copy").gameObject;
        Btn_music = myTran.Find("middle/music/music/btn_music").gameObject;
        Btn_sound = myTran.Find("middle/music/sound/btn_sound").gameObject;
        Btn_shake = myTran.Find("middle/music/shake/btn_shake").gameObject;
        Btn_gm = myTran.Find("middle/other/toggle/btn_gm").gameObject;
        Btn_libao = myTran.Find("middle/other/toggle/btn_libao").gameObject;
        Btn_yinsi = myTran.Find("middle/other/toggle/btn_yinsi").gameObject;
        Btn_luntan = myTran.Find("middle/other/toggle/btn_luntan").gameObject;
    }
    void AddListen(UISetting model){
        EventTriggerListener.Get(Btn_server).onClick = model.ClickBtn_server;
        EventTriggerListener.Get(Btn_copy).onClick = model.ClickBtn_copy;
        EventTriggerListener.Get(Btn_music).onClick = model.ClickBtn_music;
        EventTriggerListener.Get(Btn_sound).onClick = model.ClickBtn_sound;
        EventTriggerListener.Get(Btn_shake).onClick = model.ClickBtn_shake;
        EventTriggerListener.Get(Btn_gm).onClick = model.ClickBtn_gm;
        EventTriggerListener.Get(Btn_libao).onClick = model.ClickBtn_libao;
        EventTriggerListener.Get(Btn_yinsi).onClick = model.ClickBtn_yinsi;
        EventTriggerListener.Get(Btn_luntan).onClick = model.ClickBtn_luntan;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/server");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/btn_server/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/top/btn_copy/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/music/icon/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/sound/icon/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/music/shake/icon/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item (1)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item (2)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item (3)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item (4)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/message/layout/item (5)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/luanage/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/luanage/toggle/1/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/luanage/toggle/1/Image (3)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/luanage/toggle/2/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/luanage/toggle/2/Image (3)/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/other/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/other/toggle/btn_gm/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/other/toggle/btn_libao/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/other/toggle/btn_yinsi/text (1)");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/other/toggle/btn_luntan/text (1)");
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
    public string StringUid{
        get { return _stringUid; }
        set {
            if(_stringUid == value)
                return;
            _stringUid = value;
            SetUILabelUid(_stringUid);
        }
    }
    public string StringServer{
        get { return _stringServer; }
        set {
            if(_stringServer == value)
                return;
            _stringServer = value;
            SetUILabelServer(_stringServer);
        }
    }
    private void SetUILabelUid(string text){
        Uid.text = text;
    }
    private void SetUILabelServer(string text){
        Server.text = text;
    }
}
