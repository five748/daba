using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIMainAuto{
    public Transform Content;
    public Transform Navigate;
    public Transform Container_bg;
    public Transform Containers;
    public Transform Containers_left;
    public Transform Containers_right;
    public Transform Container_left_in;
    public Transform Container_left_out;
    public Transform Container_right_in;
    public Transform Container_right_out;
    public Transform Navigate_ship;
    public Transform Navigate_ship_in;
    public Transform Navigate_ship_out;
    public Transform Container_full;
    public Transform Channels;
    public Transform Channel_points;
    public Transform Zhuangshi_ad;
    public Transform Touch;
    public Image Bg;
    public Image Zhuangshi_zuoshang;
    public Image Zhuangshi_youxia;
    public Image Ad_0;
    public Image Ad_1;
    public Text Container_progress;
    public GameObject Navigate_back;
    public GameObject Navigate_send;
    private string _spriteBg;
    private string _spriteZhuangshi_zuoshang;
    private string _spriteZhuangshi_youxia;
    private string _spriteAd_0;
    private string _spriteAd_1;
    private string _stringContainer_progress;
    public void Init(Transform myTran, UIMain model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Content = myTran.Find("content");
        Navigate = myTran.Find("content/navigate");
        Container_bg = myTran.Find("content/navigate/container_bg");
        Containers = myTran.Find("content/navigate/container_bg/containers");
        Containers_left = myTran.Find("content/navigate/container_bg/containers/containers_left");
        Containers_right = myTran.Find("content/navigate/container_bg/containers/containers_right");
        Container_left_in = myTran.Find("content/navigate/container_bg/container_points/container_left_in");
        Container_left_out = myTran.Find("content/navigate/container_bg/container_points/container_left_out");
        Container_right_in = myTran.Find("content/navigate/container_bg/container_points/container_right_in");
        Container_right_out = myTran.Find("content/navigate/container_bg/container_points/container_right_out");
        Navigate_ship = myTran.Find("content/navigate/navigate_ship");
        Navigate_ship_in = myTran.Find("content/navigate/navigate_ship_in");
        Navigate_ship_out = myTran.Find("content/navigate/navigate_ship_out");
        Container_full = myTran.Find("content/navigate/container_full");
        Channels = myTran.Find("content/channels");
        Channel_points = myTran.Find("content/channel_points");
        Zhuangshi_ad = myTran.Find("content/zhuangshi_ad");
        Touch = myTran.Find("touch");
        Bg = myTran.Find("content/bg").GetComponent<Image>();
        Zhuangshi_zuoshang = myTran.Find("content/zhuangshi_zuoshang").GetComponent<Image>();
        Zhuangshi_youxia = myTran.Find("content/zhuangshi_youxia").GetComponent<Image>();
        Ad_0 = myTran.Find("content/zhuangshi_ad/ad_0").GetComponent<Image>();
        Ad_1 = myTran.Find("content/zhuangshi_ad/ad_1").GetComponent<Image>();
        Container_progress = myTran.Find("content/navigate/container_bg/container_progress").GetComponent<Text>();
        Navigate_back = myTran.Find("content/navigate/navigate_ship/navigate_back").gameObject;
        Navigate_send = myTran.Find("content/navigate/navigate_ship/navigate_send").gameObject;
    }
    void AddListen(UIMain model){
        EventTriggerListener.Get(Navigate_back).onClick = model.ClickNavigate_back;
        EventTriggerListener.Get(Navigate_send).onClick = model.ClickNavigate_send;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("content/navigate/container_bg/container_progress");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("content/channels/channel_0/channel_tip/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteBg{
        get { return _spriteBg; }
        set {
            if(_spriteBg == value)
                return;
            _spriteBg = value;
            SetUISpriteBg(_spriteBg);
        }
    }
    public string SpriteZhuangshi_zuoshang{
        get { return _spriteZhuangshi_zuoshang; }
        set {
            if(_spriteZhuangshi_zuoshang == value)
                return;
            _spriteZhuangshi_zuoshang = value;
            SetUISpriteZhuangshi_zuoshang(_spriteZhuangshi_zuoshang);
        }
    }
    public string SpriteZhuangshi_youxia{
        get { return _spriteZhuangshi_youxia; }
        set {
            if(_spriteZhuangshi_youxia == value)
                return;
            _spriteZhuangshi_youxia = value;
            SetUISpriteZhuangshi_youxia(_spriteZhuangshi_youxia);
        }
    }
    public string SpriteAd_0{
        get { return _spriteAd_0; }
        set {
            if(_spriteAd_0 == value)
                return;
            _spriteAd_0 = value;
            SetUISpriteAd_0(_spriteAd_0);
        }
    }
    public string SpriteAd_1{
        get { return _spriteAd_1; }
        set {
            if(_spriteAd_1 == value)
                return;
            _spriteAd_1 = value;
            SetUISpriteAd_1(_spriteAd_1);
        }
    }
    public string StringContainer_progress{
        get { return _stringContainer_progress; }
        set {
            if(_stringContainer_progress == value)
                return;
            _stringContainer_progress = value;
            SetUILabelContainer_progress(_stringContainer_progress);
        }
    }
    private void SetUISpriteBg(string spriteName){
        Bg.SetImage(spriteName);
    }
    private void SetUISpriteZhuangshi_zuoshang(string spriteName){
        Zhuangshi_zuoshang.SetImage(spriteName);
    }
    private void SetUISpriteZhuangshi_youxia(string spriteName){
        Zhuangshi_youxia.SetImage(spriteName);
    }
    private void SetUISpriteAd_0(string spriteName){
        Ad_0.SetImage(spriteName);
    }
    private void SetUISpriteAd_1(string spriteName){
        Ad_1.SetImage(spriteName);
    }
    private void SetUILabelContainer_progress(string text){
        Container_progress.text = text;
    }
}
