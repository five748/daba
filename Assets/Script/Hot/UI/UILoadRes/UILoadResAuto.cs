using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UILoadResAuto{
    public Image Fg;
    public GameObject Logo;
    private string _spriteFg;
    public void Init(Transform myTran, UILoadRes model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Fg = myTran.Find("middle/fg").GetComponent<Image>();
        Logo = myTran.Find("logo").gameObject;
    }
    void AddListen(UILoadRes model){
        EventTriggerListener.Get(Logo).onClick = model.ClickLogo;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;

    }
    public string SpriteFg{
        get { return _spriteFg; }
        set {
            if(_spriteFg == value)
                return;
            _spriteFg = value;
            SetUISpriteFg(_spriteFg);
        }
    }
    private void SetUISpriteFg(string spriteName){
        Fg.SetImage(spriteName);
    }
}
