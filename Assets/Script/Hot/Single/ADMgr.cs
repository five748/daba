




using YuZiSdk;

public class ADMgr : Singleton<ADMgr>
{

    public DataAD data;

    public ADMgr()
    {
        data = DataAD.Read();
        CheckNewDay();
    }
    

    //type=1 过闸费用, 2过闸速度
    public float GetAttr(int type)
    {
        var time = data.attrEndtime[type - 1];
        if (TimeTool.SerNowUtcTimeInt < time)
        {
            return TableCache.Instance.meirifuliTable[type].prop;
        }
        return 0;
    }
    
    public void CheckNewDay(){ data.CheckNewDay();}

    public bool CheckFuliRed()
    {
        var now = TimeTool.SerNowUtcTimeInt;
        var times = data.attrEndtime;
        if (now > times[0] && now > times[1])
        {
            return true;
        }

        for (int i = 0; i < 3; i++)
        {
            var config = TableCache.Instance.meirifuliBoxTable[i + 1];
            if (data.fuli_adnum >= config.time && !data.fuli_reward.Contains(config.id))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckDayRed()
    {
        if (TimeTool.SerNowUtcTimeInt >= data.day_time && data.day_index < 6)
        {
            return true;
        }
        for (int i = 0; i < 3; i++)
        {
            var config = TableCache.Instance.lingjiangBoxTable[i + 1];
            if (data.day_adnum >= config.time && !data.day_reward2.Contains(config.id))
            {
                return true;
            }
        }

        for (int i = 0; i < data.day_index; i++)
        {
            var config = TableCache.Instance.lingjiangTable[i + 1];
            if (!data.day_reward1.Contains(config.id))
            {
                return true;
            }
        }
        return false;
    }
    
    
    //是否显示赞助商
    public bool IsShowZanzhu()
    {
        var now = TimeTool.SerNowUtcTimeInt;
        return now > data.zanzhuTime;
    }
    //设置赞助显示时间
    public void SetZanzhuTime(bool isLookAd)
    {
        var time = TableCache.Instance.configTable[isLookAd ? 1 : 2].param.ToInt();
        data.zanzhuTime = TimeTool.SerNowUtcTimeInt + time;
        data.Save();
        if (isLookAd)
        {
            //todo 获取奖励
            var score = PlayerMgr.Instance.data.score;
            foreach (var kv in TableCache.Instance.zanzhuTable)
            {
                if (kv.Value.score >= score)
                {
                    PlayerMgr.Instance.AddItemNum(1, kv.Value.reward);
                    Msg.Instance.Show("钞票+" + kv.Value.reward);
                    return;
                }
            }
            var last = TableCache.Instance.zanzhuTable[TableCache.Instance.zanzhuTable.Count];
            PlayerMgr.Instance.AddItemNum(1, last.reward);
            Msg.Instance.Show("钞票+" + last.reward);

        }
    }

    public void LookDayAd(int tid)
    {
        data.day_adnum++;
        data.day_index = tid;
        data.day_time = TimeTool.SerNowUtcTimeInt + TableCache.Instance.lingjiangTable[tid].cd;

        data.Save();
    }

    //看广告领取奖励
    public void RecivedDayAd(int tid)
    {
        if (!TableCache.Instance.lingjiangTable.ContainsKey(tid))
        {
            return;
        }
        if (data.day_reward1.Contains(tid))
        {
            Msg.Instance.Show("重复领取");
            return;
        }
        data.day_reward1.Add(tid);
        var reward= TableCache.Instance.lingjiangTable[tid].reward;
        PlayerMgr.Instance.AddItemNum(2, reward);
        Msg.Instance.Show("钻石+" + reward);
        YuziMgr.Instance.ReportNormal(3001,0,reward);

        data.Save();
    }

    //领取每天看广告次数奖励
    public void ReciveDayReward(int tidi)
    {
        if (!TableCache.Instance.lingjiangBoxTable.ContainsKey(tidi))
        {
            return;
        }
        var config = TableCache.Instance.lingjiangBoxTable[tidi];
        if (config.time > data.day_adnum)
        {
            Msg.Instance.Show("条件不满足");
            return;
        }
        if (data.day_reward2.Contains(tidi))
        {
            Msg.Instance.Show("重复领取");
            return;
        }
        data.day_reward2.Add(tidi);
        PlayerMgr.Instance.AddItemNum(2, config.reward);
        Msg.Instance.Show("钻石+" + config.reward);
        YuziMgr.Instance.ReportNormal(3002,0,config.reward);

        data.Save();
    }


    public void LookFuliAd(int tid)
    {
        data.fuli_adnum++;
        var time = data.attrEndtime[tid - 1];
        data.attrEndtime[tid - 1] = TimeTool.SerNowUtcTimeInt + TableCache.Instance.meirifuliTable[tid].time;
        Msg.Instance.Show("激活成功!");
        data.Save();
    }


    //每日福利广告次数奖励领取
    public void RecivedFuliReward(int tid)
    {
        if (!TableCache.Instance.meirifuliBoxTable.ContainsKey(tid))
        {
            return;
        }
        var config = TableCache.Instance.meirifuliBoxTable[tid];
        if (config.time > data.fuli_adnum)
        {
            Msg.Instance.Show("条件不满足");
            return;
        }
        if (data.fuli_reward.Contains(tid))
        {
            Msg.Instance.Show("重复领取");
            return;
        }
        data.fuli_reward.Add(tid);
        PlayerMgr.Instance.AddItemNum(2, config.reward);
        Msg.Instance.Show("钻石+" + config.reward);
        data.Save();
    }

    public bool CheckHaveBuff()
    {
        var now = TimeTool.SerNowUtcTimeInt;
        foreach (var time in data.attrEndtime)
        {
            if (time > now)
            {
                return true;
            }
        }

        return false;
    }
}