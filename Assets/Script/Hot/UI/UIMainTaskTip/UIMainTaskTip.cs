using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.XR;

public class UIMainTaskTip:BaseMonoBehaviour{
    private UIMainTaskTipAuto Auto = new UIMainTaskTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtngoto(GameObject button){
        Debug.Log("click" + button.name);
        MTaskData.Instance.GoToUI();
    }
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
        MTaskData.Instance.ClickTask(button);
    }
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        Show();
    }
    public void Show() {
        Auto.SpriteAwardicon = "icon/" + MTaskData.Instance.tabOne.reward[0].id;
        Auto.StringNum = "X" + MTaskData.Instance.tabOne.reward[0].num;
        var tabOne = MTaskData.Instance.tabOne;
        string desc2 = tabOne.desc2;
        if (desc2 == "\"\"")
        {
            desc2 = "";
        }
        Auto.StringMsc = tabOne.desc0 + tabOne.desc1 + desc2;
        Auto.StringOvernum = "(" + MTaskData.Instance.data.OnTaskNum + "/" + MTaskData.Instance.tabOne.targetNum + ")";
        Auto.StringNoovernum = "(" + MTaskData.Instance.data.OnTaskNum + "/" + MTaskData.Instance.tabOne.targetNum + ")";
        Auto.Noover.SetActive(!MTaskData.Instance.IsOver);
        Auto.Over.SetActive(MTaskData.Instance.IsOver);
    }
}


