using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class UIZhaoshang:BaseMonoBehaviour{
    private UIZhaoshangAuto Auto = new UIZhaoshangAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIZhaoshang");
        MainMgr.Instance.updateZhaoShang?.Invoke(ChannelMgr.Instance.GetCurDamData().damId);
    }
    public void ClickBtn_look(GameObject button){
        Debug.Log("click" + button.name);
        if (checkDam < 0)
        {
            Msg.Instance.Show("未解锁");
            return;
        }
        UIManager.OpenTip("UIZhaoshanginfo",checkDam.ToString(), (s) =>
        {
            UpdateToggle();
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Coroutine cor;
    private void Init(string param){
        UIManager.FadeOut();
        checkDam = -1;
        UpdateToggle(true);
        cor = MonoTool.Instance.UpdateCall(1, UpdateTime);
        bind_guide_btn();
    }

    private int checkDam;
   
    private void UpdateToggle(bool isInit = false)
    {
        var count = TableCache.Instance.damTable.Count;
        var items = Auto.Toggle.AddChilds(count);
        var score = PlayerMgr.Instance.data.score;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var dam = i + 1;
            var name = TableCache.Instance.damTable[dam].name;
            item.GetChild(0).GetComponent<Text>().text = name;
            item.GetChild(1).GetChild(0).GetComponent<Text>().text = name;
            item.GetChild(2).GetChild(1).GetComponent<Text>().text = name;
            var isLock = ChannelMgr.Instance.IsLockDam(dam);
            if (!isLock)
            {
                item.GetChild(2).gameObject.SetActive(true);
                item.GetChild(3).gameObject.SetActive(false);
                item.GetChild(2).GetComponent<EventTriggerListener>().onClick = o =>
                {
                    Msg.Instance.Show(name + "未解锁");
                };
                continue;
            }
            item.GetChild(2).gameObject.SetActive(false);
            var info = PlayerMgr.Instance.GetZhaoshangInfo(dam);
            var lockNum = info[0];
            var hasCanlock = lockNum < 6 &&
                             score >= TableCache.Instance.attrackInvestmentUnlockTable[i * 6 + lockNum + 1].needScore;
            var hasad = info[0] > 0 && info[3] <= 0;

            var hasRed = hasCanlock || hasad;
            if (hasRed && checkDam < 0 && isInit)
            {
                checkDam = dam;
            }
            if (hasRed && isInit && dam == ChannelMgr.Instance.DamId)
            {
                checkDam = dam;
            }
            item.GetChild(3).gameObject.SetActive(hasRed);
            // item.GetChild(1).gameObject.SetActive(checkDam == dam);                
            item.GetComponent<EventTriggerListener>().onClick = o =>
            {
                checkDam = dam;
                UpdateToggle();
            };
        }

        if (checkDam <= 0)
        {
            checkDam = ChannelMgr.Instance.DamId;
            // items[checkDam].GetChild(1).gameObject.SetActive(true);
        }

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var dam = i + 1;
            item.GetChild(1).gameObject.SetActive(checkDam == dam);                
        }

        UpdateList();
    }

    public override void Destory()
    {
        cor.Stop();
        for (int i = 0; i < Auto.List.childCount; i++)
        {
            var item = Auto.List.GetChild(i);
            item.Find("unlock/suo").DOKill();
        }
    }

    private void UpdateList()
    {
        var info = PlayerMgr.Instance.GetZhaoshangInfo(checkDam);
        var score = PlayerMgr.Instance.data.score;
        var lockNum = info[0];
        var count = Mathf.Min(6, lockNum + 1);
        var items = Auto.List.AddChilds(count);

        for (int i = 0; i < count; i++)
        {
            var item = items[i];
            var icon = item.GetChild(1).GetComponent<Image>();
            bool isshowicon = info[1] > 0 && info[3] > 0 && lockNum > i;
            icon.gameObject.SetActive(isshowicon);
            if (isshowicon)
            {
                icon.SetImage("zhaoshang/" + info[1]);
            }

            var nd_lock = item.GetChild(2);
            var suo = nd_lock.GetChild(0);
            nd_lock.gameObject.SetActive(lockNum <= i);
            nd_lock.GetComponent<EventTriggerListener>().onClick = o =>
            {
                PlayerMgr.Instance.LockZhaoshang(checkDam);
                UpdateToggle();
            };
            if (lockNum <= i)
            {
                var need = TableCache.Instance.attrackInvestmentUnlockTable[(checkDam - 1) * 6 + i + 1].needScore;
                var canlock = score >= need;
                suo.GetChild(0).gameObject.SetActive(canlock);
                nd_lock.GetChild(1).GetComponent<Text>().text = $"需<color=#{(canlock ? "24d39f" : "f13f3f")}>{need}</color>积分";
                suo.DOKill(true);
                if (canlock)
                {
                    suo.eulerAngles = new Vector3(0,0,10);
                    suo.DORotate(new Vector3(0, 0, -10), 0.2f).SetLoops(-1, LoopType.Yoyo);                    
                }
                else
                {
                    suo.eulerAngles = new Vector3(0,0,0);
                }
            }

            if (i == 0)
            {
                GuideMgr.Instance.BindBtn(nd_lock, tableMenu.GuideWindownBtn.zhaoshang_first_unlock);
            }
        }

        var hasqianyue = info[1] > 0 && info[3] > 0;
        Auto.Btn_look.gameObject.SetActive(info[0] > 0 && !hasqianyue);
        Auto.Info.gameObject.SetActive(hasqianyue && lockNum > 0);
        var coe = PlayerMgr.Instance.ProgressCoe();
        if (hasqianyue)
        {
            var config = TableCache.Instance.attrackInvestmentTable[info[2]];
            Auto.Name.text = TableCache.Instance.attrackInvestmentTypeTable[info[1]].desc;
            ColorUtility.TryParseHtmlString("#" + config.color, out var color);
            Auto.Name.color = color;
            Auto.Reward.text = $"签约收益:<color=#1BA37B>每分钟{((int)(lockNum * config.minuteCoe * coe)).ChangeNum()}</color>";
            LayoutRebuilder.ForceRebuildLayoutImmediate(Auto.Reward.transform.parent.GetComponent<RectTransform>());
            Auto.Time.text = TimeTool.GetMinSecTime(info[3]);
        }
        
    }

    public void UpdateTime()
    {
        if (checkDam > 0)
        {
            var info = PlayerMgr.Instance.GetZhaoshangInfo(checkDam);
            if (info != null)
            {
                Auto.Time.text = TimeTool.GetMinSecTime(info[3]);
                if (info[3] <= 0 && Auto.Time.transform.parent.gameObject.activeInHierarchy)
                {
                    UpdateToggle();
                }

                // if (info[1] > 0 && info[3] % 60 == 0)
                // {
                //     UIFly.FlyItem(1,50,false);
                // }
            }
        }
    }
    
    private void bind_guide_btn()
    {
        GuideMgr.Instance.BindBtn(Auto.Btn_look.transform, tableMenu.GuideWindownBtn.zhaoshang_info);
        GuideMgr.Instance.BindBtn(transform.parent.Find("mask/text"), tableMenu.GuideWindownBtn.zhaoshang_close);
    }
}


