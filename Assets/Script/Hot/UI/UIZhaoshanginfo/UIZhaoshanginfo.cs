using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using YuZiSdk;

public class UIZhaoshanginfo:BaseMonoBehaviour{
    private UIZhaoshanginfoAuto Auto = new UIZhaoshanginfoAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIZhaoshanginfo");

    }
    public void ClickBtn_ad(GameObject button){
        Debug.Log("click" + button.name);
        SdkMgr.Instance.ShowAd(13, (b) =>
        {
            if (b)
            {
                PlayerMgr.Instance.RandomZhaoshangType(dam,true);
                UpdateUI();
            }

        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        dam = param.ToInt();
        UpdateUI();
        InitEffect();
        //下面是签约引导按钮
        // Auto.List.Find("btn")
    }

    private void InitEffect()
    {
        var items = Auto.List.AddChilds(3);
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var effect = item.Find("best");
            effect.localScale = new Vector3(1,1,1);
            effect.DOScale(new Vector3(0.85f, 0.85f, 0.85f), 0.6f).SetLoops(-1, LoopType.Yoyo);
        }
    }

    private int dam;
    public void UpdateUI()
    {
        var info = PlayerMgr.Instance.GetZhaoshanTypes(dam);
        var items = Auto.List.AddChilds(3);
        var coe = PlayerMgr.Instance.ProgressCoe();
        var count = PlayerMgr.Instance.GetZhaoshangInfo(dam)[0];
        int bestId = 0;
        int bestVal = 0;
        
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var type = info[i * 2];
            var quality = info[i * 2 + 1];

            var c1 = TableCache.Instance.attrackInvestmentTypeTable[type];
            var c2 = TableCache.Instance.attrackInvestmentTable[quality];
            ColorUtility.TryParseHtmlString("#" + c2.color, out var color);
            item.GetChild(0).GetComponent<Text>().text = c1.desc;
            item.GetChild(0).GetComponent<Text>().color = color;
            item.Find("best").gameObject.SetActive(false);
            item.GetChild(1).GetComponent<Text>().text = $"签约时间:{c2.minute}分钟";
            var lb_reward = item.Find("reward/lb");
            var shouyi = (int) (c2.minuteCoe * coe * c2.minute * count);
            if (shouyi > bestVal)
            {
                bestId = i;
                bestVal = shouyi;
            }
        
            lb_reward.GetComponent<Text>().text = $"签约总收益:{shouyi.ChangeNum()}";
            LayoutRebuilder.ForceRebuildLayoutImmediate(lb_reward.parent.GetComponent<RectTransform>());
            item.Find("icon").GetComponent<Image>().SetImage("zhaoshang/" + c1.id);
            item.GetChild(3).GetComponent<EventTriggerListener>().onClick = o =>
            {
                PlayerMgr.Instance.Qianyue(dam,type,quality);
                UIManager.CloseTip("UIZhaoshanginfo");
            };
            
            if (i == 0)
            {
                GuideMgr.Instance.BindBtn(item.GetChild(3), tableMenu.GuideWindownBtn.zhaoshang_info_sign);
            }
        }
        items[bestId].Find("best").gameObject.SetActive(true);

        var c3 = TableCache.Instance
            .attrackInvestmentTable[TableCache.Instance.attrackInvestmentTable.Count];
        var reward = count * c3.minuteCoe * c3.minute * coe;
        Auto.Lbtips.text = $"至少出现一个顶级广告主,签约可获得{((int)reward).ChangeNum()}";
    }
}

