using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UILoginTest:BaseMonoBehaviour{
    private UILoginTestAuto Auto = new UILoginTestAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickClick1(GameObject button){
        Debug.Log("click" + button.name);
   
    }
    public void ClickClick2(GameObject button){
        Debug.Log("click" + button.name);
        
    }
    
    //------------------------------点击方法标记请勿删除---------------------------------
    private RawImage rawImage;
    private Texture2D tex;
    private void Init(string param){
        UIManager.FadeOut();
    }
}
