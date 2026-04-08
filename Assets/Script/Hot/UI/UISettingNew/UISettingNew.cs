using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YuZiSdk;

public class UISettingNew:BaseMonoBehaviour{
    private UISettingNewAuto Auto = new UISettingNewAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_music(GameObject button){
        Debug.Log("click" + button.name);
        DataSetting.Instance.SetMusic();
        Auto.Btn_music.GetComponent<Toggle>().isOn = DataSetting.Instance.GetMusicVal() == 1;
    }
    public void ClickBtn_sound(GameObject button){
        Debug.Log("click" + button.name);
        DataSetting.Instance.SetSound();
        Auto.Btn_sound.GetComponent<Toggle>().isOn = DataSetting.Instance.GetSoundVal() == 1;
    }
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickYinsi(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.Sdk.OpenPrivacyPolicySetting();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        Auto.Btn_music.GetComponent<Toggle>().isOn = DataSetting.Instance.GetMusicVal() == 1;
        Auto.Btn_sound.GetComponent<Toggle>().isOn = DataSetting.Instance.GetSoundVal() == 1;
    }
}
