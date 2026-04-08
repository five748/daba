using UnityEngine;
using YuZiSdk;

public class UIAd_Fuli:BaseMonoBehaviour{
    private UIAd_FuliAuto Auto = new UIAd_FuliAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIAd_Fuli");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    
    private Coroutine _coroutine;
    private void Init(string param){
        UIManager.FadeOut();
        _coroutine?.Stop();

        ADMgr.Instance.CheckNewDay();
        UpdateUI();
        _coroutine = MonoTool.Instance.UpdateCall(0.5f, UpdateTime);
        
        //下面这是引导按钮--
        // Auto.Attr2.Find("btn");
    }

    public override void Destory()
    {
        _coroutine?.Stop();
        _coroutine = null;

    }

    private void UpdateUI()
    {
        UpdateAttrNode(1);
        UpdateAttrNode(2);
        UpdateReward();
    }

    private void UpdateAttrNode(int type)
    {
        var item = type == 1 ? Auto.Attr1 : Auto.Attr2;
        var config = TableCache.Instance.meirifuliTable[type];
        var endTime = ADMgr.Instance.data.attrEndtime[type - 1];
        var tips = type == 1
            ? $"所有船只过闸费用<color=#1ba37b>+{config.prop}%</color>,持续<color=#1ba37b>{config.time / 60}分钟</color>!"
            : $"所有船只过闸速度<color=#1ba37b>+{config.prop}%</color>,持续<color=#1ba37b>{config.time / 60}分钟</color>!";
        item.GetChild(1).SetText(tips);
        var btn_ad = item.Find("btn").GetComponent<EventTriggerListener>();
        var nd_time = item.Find("time");
        var now = TimeTool.SerNowUtcTimeInt;
        btn_ad.gameObject.SetActive(now > endTime);
        nd_time.gameObject.SetActive(now < endTime);
        nd_time.SetText("剩余:" + TimeTool.GetMinSecTime2(endTime - now));
        btn_ad.onClick = o =>
        {
            SdkMgr.Instance.ShowAd(type == 1 ? 4 : 3, (isSucc) =>
            {
                if (isSucc)
                {
                    ADMgr.Instance.LookFuliAd(type);
                }
                UpdateUI();
            }, GuideMgr.Instance.InGuide);
        };

        if (type == 2)
        {
            GuideMgr.Instance.BindBtn(btn_ad.transform, tableMenu.GuideWindownBtn.fuli_upspeed);
        }
    }

    private void UpdateReward()
    {
        var items = Auto.Reward.AddChilds(3);
        var data = ADMgr.Instance.data;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var config = TableCache.Instance.meirifuliBoxTable[i + 1];
            var count = Mathf.Min(config.time, data.fuli_adnum);
            item.Find("count").SetText($"满{count}/{config.time}次");
            item.Find("num").SetText("x" + config.reward);
            var btn_get = item.Find("btn").GetComponent<EventTriggerListener>();
            var nd_no = item.Find("no");
            var nd_hasget = item.Find("hasget");
            
            btn_get.gameObject.SetActive(data.fuli_adnum >= config.time && !data.fuli_reward.Contains(config.id));
            nd_no.gameObject.SetActive(data.fuli_adnum < config.time);
            nd_hasget.gameObject.SetActive(data.fuli_reward.Contains(config.id));

            btn_get.onClick = o =>
            {
                ADMgr.Instance.RecivedFuliReward(config.id);
                UpdateReward();
            };
        }
    }

    private void UpdateTime()
    {
        if (Auto.Attr1 == null)
        {
            return;
        }
        var times = ADMgr.Instance.data.attrEndtime;
        var now = TimeTool.SerNowUtcTimeInt;
        var item1 = Auto.Attr1.Find("time");
        var item2 = Auto.Attr2.Find("time");
        if (item1.gameObject.activeInHierarchy)
        {
            item1.SetText("剩余:" + TimeTool.GetMinSecTime2(times[0] - now));
            if (times[0] < now)
            {
                UpdateAttrNode(1);
            }
        }
        if (item2.gameObject.activeInHierarchy )
        {
            item2.SetText("剩余:" + TimeTool.GetMinSecTime2(times[1] - now));
            if (times[1] < now)
            {
                UpdateAttrNode(2);
            }
        }

    }
}
