using UnityEngine;
using YuZiSdk;

public class UIAd_Zanzhu:BaseMonoBehaviour{
    private UIAd_ZanzhuAuto Auto = new UIAd_ZanzhuAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        ADMgr.Instance.SetZanzhuTime(false);
        UIManager.CloseTip("UIAd_Zanzhu");
    }
    public void ClickBtnad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(1, (isSucc) =>
        {
            if (isSucc)
            {
                ADMgr.Instance.SetZanzhuTime(true);
                UIManager.CloseTip("UIAd_Zanzhu");
            }
        });
    }
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        ADMgr.Instance.SetZanzhuTime(false);
        UIManager.CloseTip("UIAd_Zanzhu");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        ADMgr.Instance.CheckNewDay();
        var score = PlayerMgr.Instance.data.score;
        foreach (var kv in TableCache.Instance.zanzhuTable)
        {
            if (kv.Value.score >= score)
            {
                Auto.Reward.text = "+" + kv.Value.reward.ToString();
                return;
            }
        }
        var last = TableCache.Instance.zanzhuTable[TableCache.Instance.zanzhuTable.Count];
        Auto.Reward.text = "+" + last.reward.ToString();
    }
}

