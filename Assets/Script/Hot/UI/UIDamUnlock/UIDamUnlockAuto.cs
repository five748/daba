using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIDamUnlockAuto{
    public Transform Light;
    public Transform Ribbons;
    public Image Icon;
    public Text Title;
    public GameObject Btn_sure;
    private string _spriteIcon;
    private string _stringTitle;
    public void Init(Transform myTran, UIDamUnlock model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Light = myTran.Find("middle/light");
        Ribbons = myTran.Find("middle/ribbons");
        Icon = myTran.Find("middle/icon").GetComponent<Image>();
        Title = myTran.Find("middle/bg_title/title").GetComponent<Text>();
        Btn_sure = myTran.Find("middle/btn_sure").gameObject;
    }
    void AddListen(UIDamUnlock model){
        EventTriggerListener.Get(Btn_sure).onClick = model.ClickBtn_sure;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/bg_title/title");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/btn_sure/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteIcon{
        get { return _spriteIcon; }
        set {
            if(_spriteIcon == value)
                return;
            _spriteIcon = value;
            SetUISpriteIcon(_spriteIcon);
        }
    }
    public string StringTitle{
        get { return _stringTitle; }
        set {
            if(_stringTitle == value)
                return;
            _stringTitle = value;
            SetUILabelTitle(_stringTitle);
        }
    }
    private void SetUISpriteIcon(string spriteName){
        Icon.SetImage(spriteName);
    }
    private void SetUILabelTitle(string text){
        Title.text = text;
    }
}
