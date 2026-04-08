

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Table;
using TreeData;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerMgr : Singleton<PlayerMgr>
{
    public DataPlayer data { get; }
    
    public Action<int> FunScoreChange;
    public Action<int, long> FunItemChange;//item_id 和 item_num 

    private Coroutine _coSaveData = null;
    private long _nextSaveTime = long.MinValue;
    
    private long _goldUpdateTime = 0;

    private PlayerMgr()
    {
        data = DataPlayer.ReadFromFile();
        //GameProcess.Instance.SaveEvent += data.SaveToFile;
        MonoTool.Instance.UpdateCall(1, OnUpdate);
        MonoTool.Instance.StartCor(co_save_item());
    }

    //fly_type:1 飞图标    2不飞图标只飘增加数值   其他,只加道具
    public bool AddItemNum(int itemId, int num,int fly_type = 1)
    {
        if (!data.items.ContainsKey(itemId))
        {
            Debug.LogError($"{itemId} 不包含在道具表内 或道具表更新了");
            data.items.TryAdd(itemId, 0);
        }

        var item = data.items[itemId];
        if (num < 0 && item < Mathf.Abs(num))
        {
            Msg.Instance.Show($"{TableCache.Instance.itemTable[itemId].initCost}不足");
            return false;
        }
        data.items[itemId] += num;
        if (itemId == 1)
        {
            MTaskData.Instance.AddTaskNum(MainTaskMenu.GetMoneyNum, num, 1);
        }
        item_change_save();
        // FunItemChange?.Invoke(itemId, data.items[itemId]);
        onItemChange(itemId, data.items[itemId]);
      
        if (num > 0)
        {
            if (fly_type == 1)
            {                
                UIFly.FlyItem(itemId,num,true);
            }
            else if(fly_type == 2)
            {
                UIFly.ShowRewardTip(itemId,num);
            }
        }
        return true;
    }

    private void onItemChange(int id, long num)
    {
        if (id == 1)
        {
            if (_goldUpdateTime > TimeTool.CurTimeSeconds)
            {
                MonoTool.Instance.Wait(_goldUpdateTime - TimeTool.CurTimeSeconds, () =>
                {
                    FunItemChange?.Invoke(1, GetItemNum(1));
                });
                return;
            } 
            
            FunItemChange?.Invoke(id, num);
            _goldUpdateTime = TimeTool.CurTimeSeconds + 1;
            return;
        }        

        FunItemChange?.Invoke(id, num);
    }
    
    public bool IsEnough(int id, long num) {
        long haveNum = 0;
        if (data.items.TryGetValue(id, out var item))
        {
            haveNum = item;
        }
        if (haveNum >= num) {
            return true;
        }
        return false;
    }
    public long GetItemNum(int id)
    {
        return data.items.GetValueOrDefault(id, 0);
    }

    /**
     * 增加评分
     */
    public bool AddScore(int num)
    {
        if (num < 0 && data.score < Mathf.Abs(num))
        {
            Msg.Instance.Show("积分不足");
            return false;
        }
        
        
        var score = data.score;
        score += num;
        if (score < 5)
        {
            score = 5;
        }
        //20240605 测试版 特殊处理
        // else if (score > 1500)
        // {
        //     score = 1500;
        // }
        else
        {
            int ratio = score / 5;
            score = ratio * 5;
        }
        data.score = score;
        data.SaveToFile();
        FunScoreChange?.Invoke(data.score);
        return true;
    }

    //养成系数
    public int ProgressCoe()
    {
        var key = data.score - data.score % 5;
        if (TableCache.Instance.attrackInvestmentCoeTable.ContainsKey(key))
        {
            return TableCache.Instance.attrackInvestmentCoeTable[key].coe;
        }
        else
        {
            Debug.LogError("养成系数不存在");
            return TableCache.Instance.attrackInvestmentCoeTable[5].coe;
        }
        
    }
    
    
    private void OnUpdate()
    {
        UpdateZhaoshangTime();
    }



    /**
     * *************招商***************
     * *************招商***************
     */

    public bool ZhaoshangRed()
    {
        for (int i = 1; i <= 6; i++)
        {
            if (ChannelMgr.Instance.IsLockDam(i))
            {
                var info = GetZhaoshangInfo(i);
                if (info[0] > 0 && info[1] == 0 && info[3] <= 0)
                {
                    return true;
                }

                var haslock = info[0] < 6 && PlayerMgr.Instance.data.score >= TableCache.Instance.attrackInvestmentUnlockTable[(i-1) * 6 + info[0] + 1].needScore;
                if (haslock)
                {
                    return true;
                }
            }            
        }

        return false;
    }

    public bool JiaotongRed()
    {
        if (data.jiaotong.Count == TableCache.Instance.directTrafficTable.Count)
        {
            return false;
        }
        var score = data.score;
        foreach (var kv in TableCache.Instance.directTrafficTable)
        {
            if (!data.jiaotong.Contains(kv.Key) && score >= kv.Value.score)
            {
                return true;
            }
        }
        return false;
    }

    public bool PintuRed()
    {
        if (data.pintu.Count == TableCache.Instance.loadingOfCargoTable.Count)
        {
            return false;
        }
        var score = data.score;
        foreach (var kv in TableCache.Instance.loadingOfCargoTable)
        {
            if (!data.pintu.Contains(kv.Key) && score >= kv.Value.score)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateZhaoshangTime()
    {
        var hasChange = false;
        var coe = ProgressCoe();
        foreach (var kv in data.zhaoshang)
        {
            var info = kv.Value;
            if (info[0] > 0 && info[1] > 0)
            {
                info[3]--;
                if (info[3] >= 0 && info[3] % 60 == 0)
                {
                    hasChange = true;
                    var reward = (int)(TableCache.Instance.attrackInvestmentTable[info[2]].minuteCoe * coe * info[0]);
                    Debug.Log("招商位" + kv.Key + "获得收益" + reward);
                    AddItemNum(1, reward,2);
                    var notice = TableCache.Instance.attrackInvestmentTypeTable[info[1]].tips;
                    UIFly.PlayNotice(notice,1,reward);
                    UIFly.ShowRewardTip(1,reward);
                }

                if (info[3] <= 0)
                {
                    info[1] = 0;
                    info[2] = 0;
                    info[3] = 0;
                }
            }
        }

        if (hasChange)
        {
            data.SaveToFile();
        }
    }
    
    
    public int[] GetZhaoshangInfo(int id)
    {
        if (!data.zhaoshang.ContainsKey(id))
        {
            if (ChannelMgr.Instance.IsLockDam(id))
            {
                data.zhaoshang.Add(id,new []{0,0,0,0});
                return data.zhaoshang[id];
            }
            return null;
        }
        else
        {
            return data.zhaoshang[id];
        }
    }


    public int[] GetZhaoshanTypes(int dam)
    {
        if (!data.zhaoshangtype.ContainsKey(dam))
        {
            if (dam == 1)
            {    //引导第一个必出最高品质脑黄金
                data.zhaoshangtype[dam] = new[] {1, 6, 2, 3, 3, 2};
            }
            else
            {
                RandomZhaoshangType(dam,false);                
            }
        }

        return data.zhaoshangtype[dam];
    }

    
    //招商签约
    public void Qianyue(int dam,int type,int quality)
    {
        var info = GetZhaoshangInfo(dam);
        if (info == null)
        {
            Msg.Instance.Show("未解锁");
            return;
        }
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumSignShipNum, 1);
        var time = TableCache.Instance.attrackInvestmentTable[quality].minute;
        info[1] = type;
        info[2] = quality;
        info[3] = time * 60 - 1;
        data.zhaoshang[dam] = info;
        RandomZhaoshangType(dam,false);
        data.SaveToFile();
    }

    public void RandomZhaoshangType(int dam,bool isAd)
    {
        List<int> list = new List<int>();
        foreach (var kv in TableCache.Instance.attrackInvestmentTypeTable)
        {
            list.Add(kv.Key);
        }
        var types = new int[6];
        for (int i = 0; i < 3; i++)
        {
            var index = Random.Range(0, list.Count);
            types[i * 2] = list[index];
            list.RemoveAt(index);
            var quality = Random.Range(isAd ? 4 : 1, isAd ? TableCache.Instance.attrackInvestmentTable.Count + 1 : 6);
            types[i * 2 + 1] = quality;
        }
        data.zhaoshangtype[dam] = types;
        if (isAd)
        {
            for (int i = 1; i < types.Length; i+=2)
            {
                if (types[i] >= 6)//广告必出6星
                {
                    data.SaveToFile();
                    return;
                }   
            }
            types[Random.Range(0, 3) * 2 + 1] = 6;
        }
        data.SaveToFile();
    }
    
    public void LockZhaoshang(int checkDam)
    {
        var info = data.zhaoshang[checkDam];
        if (info == null || info[0] >= 6)
        {
            return;
        }
        var config = TableCache.Instance.attrackInvestmentUnlockTable[(checkDam - 1) * 6 + info[0] + 1];
        if (data.score < config.needScore)
        {
            Msg.Instance.Show("积分不足");
            return;
        }
        data.zhaoshang[checkDam][0] = info[0] + 1;
        data.SaveToFile();
    }


    public void RecivedPintuReward(int id, int count,bool isAd)
    {
        if (data.pintu.Contains(id))
        {
            return;
        }
        var config = TableCache.Instance.loadingOfCargoTable[id];
        int reward = 0;
        if (count >= config.itemId.Length)
        {
            reward = count + config.reward;
        }
        else
        {
            reward = isAd ? count * 2 : count;
        }

        AddItemNum(2, reward);
        Msg.Instance.Show("钻石+" + reward);
        data.pintu.Add(id);
        data.SaveToFile();
    }

    public void RecivedJiaotongReward(bool isSuccess,int id,bool isAd)
    {
        if (data.jiaotong.Contains(id))
        {
            return;
        }
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumDirTrafNum);
        var config = TableCache.Instance.directTrafficTable[id];
        var reward = config.reward[0].num;
        if (isSuccess && isAd)
        {
            reward *= TableCache.Instance.configTable[301].param.ToInt();
        }

        AddItemNum(2, reward);
        Msg.Instance.Show("钻石+" + reward);
        data.jiaotong.Add(id);
        data.SaveToFile();
    }
    
    /**
     * *********招商end************
     */
    
    
    public void AddCollectId(int id)
    {
        var tabOne = TableCache.Instance.collectionTable[id];
        if (data.collectLst.Contains(id))
        {
            var awards = tabOne.salePrice;
            PlayerMgr.Instance.AddItemNum(awards[0].id, awards[0].num);
            return;
        }
        data.SumCollectNum++;
        data.collectLst.Add(id);
        var shipType = tabOne.shipType;
        foreach (var item in ShipMgr.Instance.data.ships)
        {
            var shipTabOne = TableCache.Instance.shipTable[item.Key];
            if (shipTabOne.type == shipType) {
                item.Value.preLockageTime += tabOne.prop;
            }
        }
        data.SaveToFile();
    }

    private void item_change_save()
    {
        if (_nextSaveTime != long.MinValue)
        {
            return;
        }
        
        _nextSaveTime = TimeTool.CurTimeSeconds + 5;
        // Debug.Log($"下一次保存道具信息的时间 _nextSaveTime {_nextSaveTime} TimeTool.CurTimeSeconds {TimeTool.CurTimeSeconds}");
    }

    private IEnumerator co_save_item()
    {
        while (true)
        {
            if (_nextSaveTime == long.MinValue || data == null)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                if (_nextSaveTime <= TimeTool.CurTimeSeconds)
                {
                    //Debug.Log($"保存道具信息 TimeTool.CurTimeSeconds {TimeTool.CurTimeSeconds}");
                    data.SaveToFile();
                    _nextSaveTime = long.MinValue;
                }
                else
                {
                    yield return new WaitForSeconds(_nextSaveTime - TimeTool.CurTimeSeconds);
                }
            }
        }
    }
}