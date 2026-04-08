using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YuZiSdk;

public class UIOutLineTip:BaseMonoBehaviour{
    private UIOutLineTipAuto Auto = new UIOutLineTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        PlayerMgr.Instance.AddItemNum(1, AddNum);
        Msg.Instance.Show("钞票:" + AddNum);
        CloseTip();
    }
    public void ClickBtnad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(12, (isSucc) => {
            if (isSucc) {
                PlayerMgr.Instance.AddItemNum(1, AddNum * 2);
                Msg.Instance.Show("钞票:" + AddNum * 2);
                CloseTip();
            }
        });
    }
    private void CloseTip() {
        UIManager.CloseTip();
        SetGuideScale(Vector3.one);
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public static int outTime;
    private int AddNum = 100;
    private int min;
    private void Init(string param){
        UIManager.FadeOut();
        SetGuideScale(Vector3.zero);
        SetData();
        string str = min.ToString().ChangeColor("1ba37b");
        Auto.Msc.text = $"本次获得{str}分钟离线收益";
        Auto.StringNum = "+" + AddNum;
        Auto.StringRote = $"( 最多可获得{Mathf.FloorToInt(PlayerMgr.Instance.data.MaxOutLineTime / 60.0f)}分钟离线收益 )";
    }
    private void SetGuideScale(Vector3 scale) {
        UIManager.Root.Find("UIGuide").transform.localScale = scale;
    }
    private void SetData() {
        if (outTime > PlayerMgr.Instance.data.MaxOutLineTime)
        {
            outTime = PlayerMgr.Instance.data.MaxOutLineTime;
        }
        min = Mathf.FloorToInt(outTime / 60.0f);
        var configValue = float.Parse(TableCache.Instance.configTable[601].param);
        if (!TableCache.Instance.progressCoeTable.ContainsKey(PlayerMgr.Instance.data.score))
        {
            Debug.LogError("分数必须为5的倍数|分数:" + PlayerMgr.Instance.data.score);
        }
        else {
            var scoreValue = TableCache.Instance.progressCoeTable[PlayerMgr.Instance.data.score].coe;
            AddNum = (int)(configValue * scoreValue * min);
        }
    }
}

