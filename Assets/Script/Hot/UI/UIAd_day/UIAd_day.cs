using UnityEngine;
using YuZiSdk;

public class UIAd_day:BaseMonoBehaviour{
    private UIAd_dayAuto Auto = new UIAd_dayAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIAd_day");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Coroutine _coroutine;
    private void Init(string param){
        UIManager.FadeOut();
        _coroutine?.Stop();
        ADMgr.Instance.CheckNewDay();

        UpdateList();
        UpdateReward();
        _coroutine = MonoTool.Instance.UpdateCall(0.5f, UpdateTime);
    }
    public override void Destory()
    {
        _coroutine?.Stop();
        _coroutine = null;
    }

    private Transform timeNode = null;
    public void UpdateList()
    {
        var items = Auto.List.AddChilds(6);
        var data = ADMgr.Instance.data;
        var now = TimeTool.SerNowUtcTimeInt;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var config = TableCache.Instance.lingjiangTable[i + 1];
            item.Find("count").SetText($"第{i+1}次");
            item.Find("num").SetText("x" + config.reward);
            var btn_get = item.Find("btn").GetComponent<EventTriggerListener>();
            var btnad = item.Find("btnad").GetComponent<EventTriggerListener>();
            var nd_time = item.Find("time");
            var nd_hasget = item.Find("hasget");
            btn_get.gameObject.SetActive(data.day_index >= config.id && !data.day_reward1.Contains(config.id));
            nd_hasget.gameObject.SetActive(data.day_index >= config.id && data.day_reward1.Contains(config.id));
            nd_time.gameObject.SetActive(data.day_index == config.id-1 && now < data.day_time);
            btnad.gameObject.SetActive(data.day_index == config.id-1 && now >= data.day_time);
            // if (config.id == 1)//第一个免费领取
            // {
            //     btnad.gameObject.SetActive(false);
            //     btn_get.gameObject.SetActive(!data.day_reward1.Contains(config.id));
            // }
            if (nd_time.gameObject.activeSelf)
            {
                timeNode = nd_time;
                UpdateTime();
            }
            btn_get.onClick = o =>
            {
                ADMgr.Instance.RecivedDayAd(config.id);
                UpdateList();
            };

            btnad.onClick = o =>
            {
                SdkMgr.Instance.ShowAd(2,(iSucc) =>
                {
                    if (iSucc)
                    {
                        ADMgr.Instance.LookDayAd(config.id);
                        UpdateList();
                        UpdateReward();
                    }
                 
                });
            };
        }
    }

    public void UpdateReward()
    {
        var items = Auto.Reward.AddChilds(3);
        var data = ADMgr.Instance.data;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var config = TableCache.Instance.lingjiangBoxTable[i + 1];
            var count = Mathf.Min(config.time, data.day_adnum);
            item.Find("count").SetText($"满{count}/{config.time}次");
            item.Find("num").SetText("x" + config.reward);
            var btn_get = item.Find("btn").GetComponent<EventTriggerListener>();
            var nd_no = item.Find("no");
            var nd_hasget = item.Find("hasget");
            
            btn_get.gameObject.SetActive(data.day_adnum >= config.time && !data.day_reward2.Contains(config.id));
            nd_no.gameObject.SetActive(data.day_adnum < config.time);
            nd_hasget.gameObject.SetActive(data.day_reward2.Contains(config.id));

            btn_get.onClick = o =>
            {
                ADMgr.Instance.ReciveDayReward(config.id);
                UpdateReward();
            };
        }
    }


    public void UpdateTime()
    {
        if (timeNode != null && timeNode.gameObject.activeSelf)
        {
            var end = ADMgr.Instance.data.day_time;
            timeNode.SetText(TimeTool.GetMinSecTime(end - TimeTool.SerNowUtcTimeInt));
            if (TimeTool.SerNowUtcTimeInt >= end)
            {
                timeNode = null;
                UpdateList();
            }
        }
        
    }
}



