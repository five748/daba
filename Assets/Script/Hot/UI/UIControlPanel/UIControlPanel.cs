using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIControlPanel:BaseMonoBehaviour{
    private UIControlPanelAuto Auto = new UIControlPanelAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickBtn_guide_open(GameObject button){
        Debug.Log("click" + button.name);
        ControlPanelMgr.Instance.SetGuideShow(true);
        guide();
    }
    public void ClickBtn_guide_close(GameObject button){
        Debug.Log("click" + button.name);
        ControlPanelMgr.Instance.SetGuideShow(false);
        guide();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        guide();
    }

    private void guide()
    {
        var str = UIGuide.ShowGuide ? "开启" : "关闭";
        Auto.Txt_guide_state.text = $"新手引导状态 == {str}";
    }
}

