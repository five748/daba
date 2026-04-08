using Unity.VisualScripting;
using UnityEngine;
using YuZiSdk;
using UnityEngine.UI;

public class UISetting : BaseMonoBehaviour
{
    private UISettingAuto Auto = new UISettingAuto();
    public override void BaseInit()
    {
        Auto.Init(transform, this);
        Init(param);
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_yinsi(GameObject button)
    {
        Debug.Log("click" + button.name);
        SdkMgr.Instance.Sdk.OpenPrivacyPolicySetting();
    }
    public void ClickBtn_music(GameObject button)
    {
        Debug.Log("click" + button.name);
        DataSetting.Instance.SetMusic();
        Auto.Btn_music.GetComponent<Toggle>().isOn = DataSetting.Instance.GetMusicVal() == 1;
    }
    public void ClickBtn_sound(GameObject button)
    {
        Debug.Log("click" + button.name);
        DataSetting.Instance.SetSound();
        Auto.Btn_sound.GetComponent<Toggle>().isOn = DataSetting.Instance.GetSoundVal() == 1;
    }
    public void ClickBtn_shake(GameObject button)
    {
        Debug.Log("click" + button.name);
        DataSetting.Instance.SetShake();
        Auto.Btn_shake.GetComponent<Toggle>().isOn = DataSetting.Instance.GetShakeVal() == 1;
    }
   
    public void ClickBtn_server(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.OpenTip("UIServer");
    }
    public void ClickBtn_copy(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBtn_gm(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBtn_libao(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBtn_luntan(GameObject button){
        Debug.Log("click" + button.name);
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param)
    {
        UIManager.FadeOut();

        Auto.Btn_music.GetComponent<Toggle>().isOn = DataSetting.Instance.GetMusicVal() == 1;
        Auto.Btn_sound.GetComponent<Toggle>().isOn = DataSetting.Instance.GetSoundVal() == 1;
        Auto.Btn_shake.GetComponent<Toggle>().isOn = DataSetting.Instance.GetShakeVal() == 1;
        UpdatePush();
    }

    
    private void UpdatePush()
    {
        var infos = DataSetting.Instance.GetPushInfos();
        for (int i = 0; i < Auto.Layout.childCount; i++)
        {
            var item = Auto.Layout.GetChild(i);
            int index = i;
            item.GetChild(0).GetChild(0).gameObject.SetActive(infos[i] == 1);
            item.GetOrAddComponent<EventTriggerListener>().onClick = (obj) =>
            {
                DataSetting.Instance.SetPushInfo(index);
                UpdatePush();
            };
        }
    }
}

