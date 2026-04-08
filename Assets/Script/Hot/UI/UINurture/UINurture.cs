using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using Table;

public class UINurture:BaseMonoBehaviour{
    private UINurtureAuto Auto = new UINurtureAuto();

    private int _choseDamId;
    private Dictionary<int, NurtureItem> OutData => NurtureMgr.Instance.Data.Nurtures;
    public Dictionary<int, Dictionary<int, NurtureItem>> OurDatas = new ();
    
    private int Score {
        get {
            //return 10000;
            return PlayerMgr.Instance.data.score;
        }
    }

    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickMenusMenus(GameObject button, System.Action callback){
        Debug.Log("click" + button.name);
        _choseDamId = int.Parse(button.name);
        if (!ChannelMgr.Instance.GetDamData(_choseDamId).unlock)
        {
            return;
        }
        Show();
        callback();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        SetData();
        ShowMenu(false);
        Show();

        PlayerMgr.Instance.FunItemChange += OnItemChange;
        bind_guide_btn();
    }

    public override void Destory()
    {
        PlayerMgr.Instance.FunItemChange -= OnItemChange;
        base.Destory();
    }

    private void SetData() {
        foreach (var item in OutData)
        {
            var tabOne = TableCache.Instance.equipmentLvupTable[item.Key];
            if (!OurDatas.ContainsKey(tabOne.forDamId)) {
                OurDatas.Add(tabOne.forDamId, new Dictionary<int, NurtureItem>());
            }
            OurDatas[tabOne.forDamId].Add(item.Key, item.Value);
        }
    }
    public static int MainTaskGoToId = -1;
    private void ShowMenu(bool updateRed) {
        var tab = TableCache.Instance.damTable;
        var redDamIds = new List<int>();
        tab.ForeachForAddItems(Auto.Menus.transform, (k,v, index, tran) => {
            tran.name = k.ToString();
            tran.SetText(v.name, "nor/text");
            tran.SetText(v.name, "choose/text");
            tran.SetText(v.name, "lock/text");
            tran.SetActive(!ChannelMgr.Instance.GetDamData(v.id).unlock, "lock");
            bool isHaveRed = IsHaveRed(k);
            tran.SetActive(isHaveRed, "red");
            if (isHaveRed)
            {
                redDamIds.Add(v.id);
            }
        });

        if (updateRed)
        {
            return;
        }

        if (redDamIds.Count <= 0)
        {
            _choseDamId = ChannelMgr.Instance.DamId; 
        }
        else
        {
            _choseDamId = redDamIds[0];
            for (int i = 0; i < redDamIds.Count; i++)
            {
                if (redDamIds[i] == ChannelMgr.Instance.DamId)
                {
                    _choseDamId = ChannelMgr.Instance.DamId;
                    break;
                }
            }
        }
        
        _choseDamId = Mathf.Max(1, _choseDamId);
        if (MainTaskGoToId != -1)
        {
            _choseDamId = TableCache.Instance.equipmentLvupTable[MainTaskGoToId].forDamId;
        }
        Auto.Scrollmenu.GetComponent<NewScrollRect>().MoveToItem(Auto.Menus.transform.GetChild(_choseDamId - 1));
        Auto.Menus.ClickMenu(_choseDamId - 1);
    }
    
    private bool IsHaveRed(int damId) {
        if (!OurDatas.ContainsKey(damId))
        {
            return false;
        }

        if (!ChannelMgr.Instance.GetDamData(damId).unlock)
        {
            return false;
        }
        
        var datas = OurDatas[damId];
        var haveNum = PlayerMgr.Instance.GetItemNum(1);
        foreach (var item in datas)
        {
            var data = item.Value;
            bool isMax = data.lv >= NurtureMgr.MaxLv;
            if (isMax)
            {
                continue;
            }

            var tData = TableCache.Instance.equipmentLvupTable[data.tId];
            if (!data.unlock)
            {
                if (tData.needScore <= Score)
                {
                    return true;
                }
            }
            else
            {
                if (haveNum >= tData.cost[data.lv])
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    private void Show(int effectId = int.MinValue) {
        var datas = OurDatas[_choseDamId];

        List<int> firstLock = new ();
        List<int> unlock = new();
        List<int> otherLock = new ();
        List<int> lackLock = new ();
        List<int> max = new ();

        var findUnlock = false;
        ListTool.Compare<KeyValuePair<int, NurtureItem>> compare = (data) =>
        {
            var tData = TableCache.Instance.equipmentLvupTable[data.Value.tId];
            if (!data.Value.unlock)
            {
                if (!findUnlock && tData.needScore <= Score)
                {
                    findUnlock = true;
                    return -1;
                }
                
                return tData.needScore <= Score ? 4 : 5;
            }
            
            if (data.Value.lv >= NurtureMgr.MaxLv)
            {
                return 6;
            }

            return 2;
        };
        
        foreach (var item in datas)
        {
            if (compare(item) == -1)
            {
                firstLock.Add(item.Key);
                continue;
            }
            if (compare(item) == 2)
            {
                unlock.Add(item.Key);
                continue;
            }
            if (compare(item) == 4)
            {
                otherLock.Add(item.Key);
                continue;
            }
            if (compare(item) == 5)
            {
                lackLock.Add(item.Key);
                continue;
            }
            if (compare(item) == 6)
            {
                max.Add(item.Key);
                continue;
            }
        }
        
        // unlock.Sort((x, y) =>
        // {
        //     var dataX = datas[x];
        //     var dataY = datas[y];
        //     var tX = TableCache.Instance.equipmentLvupTable[x];
        //     var tY = TableCache.Instance.equipmentLvupTable[y];
        //     if (tX.cost[dataX.lv] <= tY.cost[dataY.lv])
        //     {
        //         return -1;
        //     }
        //
        //     return 1;
        // });
        
        var newData =new Dictionary<int, NurtureItem>();
        for (int i = 0; i < firstLock.Count; i++)
        {
            newData.Add(firstLock[i], datas[firstLock[i]]);
        }
        for (int i = 0; i < unlock.Count; i++)
        {
            newData.Add(unlock[i], datas[unlock[i]]);
        }
        for (int i = 0; i < otherLock.Count; i++)
        {
            newData.Add(otherLock[i], datas[otherLock[i]]);
        }
        for (int i = 0; i < lackLock.Count; i++)
        {
            newData.Add(lackLock[i], datas[lackLock[i]]);
        }
        for (int i = 0; i < max.Count; i++)
        {
            newData.Add(max[i], datas[max[i]]);
        }
        datas.Clear();
        foreach (var item in newData)
        {
            datas.Add(item.Key, item.Value);
        }
        var items = new Dictionary<int, Transform>();
        datas.ForeachForAddItems(Auto.Grid, (k, v, index, tran) => {
            items.Add(k, tran);
            ShowOne(tran, v, k, index, effectId);
        });
        if (MainTaskGoToId != -1)
        {
            var scroll = Auto.Scroll.GetComponent<NewScrollRect>();
            scroll.MoveToItem(items[MainTaskGoToId]);
            MainTaskGoToId = -1;
        }
    }
    
    private void ShowOne(Transform tran, NurtureItem data, int k, int index, int effectId) {
        var tabOne = TableCache.Instance.equipmentLvupTable[data.tId];
        tran.SetActive(data.unlock, "open");
        tran.SetActive(!data.unlock, "lock");
        tran.SetActive(data.unlock, "lvbg");
        tran.SetText("Lv." + data.lv, "lvbg/lv");
        tran.SetImage("nurture/" + tabOne.iconId, "icon");
        tran.SetText($"{tabOne.name} <size=38><color=#419ea6>{tabOne.forChannelId}</color>号航道</size>", "name");
        var showLv = Mathf.Max(1, data.lv);
        tran.SetText($"所有船只过闸速度 <color=#419ea6>+{tabOne.passSpeed * showLv}%</color>", "prop0");
        tran.SetText($"船流量 <color=#419ea6>+{tabOne.passSpeed * showLv}%</color>", "prop1");
        tran.SetActive(false, "max");
        if (!data.unlock)
        {
            int needScore = tabOne.needScore;
            tran.SetText("需要x评分".Replace("x", needScore.ToString().ChangeColor(GetNumColorStr(Score >= needScore))), "lock/msc");
            if (Score < needScore)
            {
                tran.SetActive(true, "lock/unenough");
                tran.SetActive(false, "lock/lock");
            }
            else {
                if (index == 0)
                {
                    GuideMgr.Instance.BindBtn(tran.Find("lock/lock/btn"), tableMenu.GuideWindownBtn.nurture_first_unlock);
                }
                EventTriggerListener.Get(tran.Find("lock/lock/btn")).onClick = (btn) =>
                {
                    Debug.Log($"养护 设备解锁 {tabOne.id}");
                    if (NurtureMgr.Instance.Uplevel(tabOne.id))
                    {
                        Msg.Instance.Show("解锁成功!");
                        Show();
                    }
                };
            }
        }
        else {
            bool isMax = NurtureMgr.MaxLv <= data.lv;
            tran.SetActive(isMax, "max");
            tran.SetActive(!isMax, "open");
            if (!isMax) {
                var haveNum = PlayerMgr.Instance.GetItemNum(1);
                var needNum = tabOne.cost[data.lv];
                tran.SetText(needNum.ToString().ChangeColor(GetNumColorStr(haveNum >= needNum)), "open/money");
                EventTriggerListener.Get(tran.Find("open/btn")).onClick = (btn) =>
                {
                    Debug.Log("养护 设备升级");
                    if (NurtureMgr.Instance.Uplevel(tabOne.id))
                    {
                        PlayerMgr.Instance.AddItemNum(1, -needNum);
                        Msg.Instance.Show("升级成功!");
                        MTaskData.Instance.AddTaskNum(MainTaskMenu.FixEquipLvNum, 1, tabOne.id);
                        Show(data.tId);
                        ShowMenu(true);
                    }
                    else {
                        UIManager.OpenTip("UIAd_Zanzhu", "", s =>
                        {
                            ShowOne(tran, data, k, index, int.MinValue);
                            UIEmpty.Instance.UpdateAdRed();
                        });
                    }
                };
            }
        }

        if (effectId != int.MinValue && effectId == data.tId)
        {
            var tfEffect = tran.FindChildTransform("lveffect"); 
            tfEffect.GetComponent<Image>().PlayOnce("lvupEffect");
            tfEffect.SetParentOverride(tran);
            tfEffect.SetActive(true);
        }
    }
    
    private string GetNumColorStr(bool isEnough) {
        if (isEnough)
        {
            return "419ea6";
        }
        return "a64141";
    }
    
    private void bind_guide_btn()
    {
        GuideMgr.Instance.BindBtn(transform.parent.Find("mask/text"), tableMenu.GuideWindownBtn.nurture_close);
    }

    private void OnItemChange(int id, long num)
    {
        if (id == 1)
        {
            if (IsHaveRed(_choseDamId))
            {
                Show();
            }
        }
    }
}


