

using System;
using System.Collections.Generic;
using TreeData;
using UnityEngine;

public class NurtureMgr : Singleton<NurtureMgr>
{
    public DataNurture Data { get; }
    public static int MaxLv = 0;

    private Dictionary<string, int> _dicPassSpeed;
    private Dictionary<string, int> _dicShipFlow;
    
    private NurtureMgr()
    {
        Data = DataNurture.ReadFromFile();
        //GameProcess.Instance.SaveEvent += Data.SaveToFile;
        MaxLv = TableCache.Instance.equipmentLvupTable[1].cost.Length;
    }

    public bool CheckUpLevel(int tId)
    {
        var lv = Data.Nurtures[tId].lv;
        if (lv >= MaxLv)
        {
            Debug.LogError($"超过等级上限 检查代码");
            return false;
        }

        var nurture = Data.Nurtures[tId]; 
        var tNurture = TableCache.Instance.equipmentLvupTable[tId];

        if (!nurture.unlock)
        {
            if (PlayerMgr.Instance.data.score < tNurture.needScore)
            {
                Debug.LogError("积分不足");
                return false;
            }
        }
        else
        {
            if (!PlayerMgr.Instance.IsEnough(1, tNurture.cost[nurture.lv]))
            {
                Debug.LogError("道具不足");
                return false;
            }
        }

        return true;
    }

    public bool Uplevel(int tId)
    {
        if (!CheckUpLevel(tId))
        {
            return false;
        }
        var nurture = Data.Nurtures[tId];
        if (!nurture.unlock)
        {
            nurture.unlock = true;
        }
        else
        {
            nurture.lv += 1;
        }
        
        Data.SaveToFile();
        calculate_factor(tId);
        return true;
    }

    /// <summary>
    /// 主界面红点检查
    /// </summary>
    /// <returns></returns>
    public bool CheckRed()
    {
        var haveNum = PlayerMgr.Instance.GetItemNum(1);
        var score = PlayerMgr.Instance.data.score;

        foreach (var (_, data) in Data.Nurtures)
        {
            if (!data.unlock)
            {
                var tData = TableCache.Instance.equipmentLvupTable[data.tId];
                if (score >= tData.needScore)
                {
                    return true;
                }
            }
            else
            {
                if (data.lv >= MaxLv)
                {
                    continue;
                }
                
                var tData = TableCache.Instance.equipmentLvupTable[data.tId];
                if (tData.cost[data.lv] <= haveNum)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public int GetPassSpeedFactor(int damId, int channelId)
    {
        if (_dicShipFlow == null || _dicPassSpeed == null)
        {
            calculate_factor();
        }

        var factor = _dicPassSpeed.GetValueOrDefault($"{damId}_{channelId}", 0);
        return factor;
    }

    public int GetShipFlowFactor(int damId, int channelId)
    {
        if (_dicShipFlow == null || _dicPassSpeed == null)
        {
            calculate_factor();
        }

        var factor = _dicShipFlow.GetValueOrDefault($"{damId}_{channelId}", 0);
        return factor;
    }

    private void calculate_factor()
    {
        if (_dicShipFlow == null)
        {
            _dicShipFlow = new();
            _dicPassSpeed = new();
        }
        
        foreach (var (_, data) in Data.Nurtures)
        {
            var tData = TableCache.Instance.equipmentLvupTable[data.tId];
            _dicShipFlow[$"{tData.forDamId}_{tData.forChannelId}"] = tData.shipFlow * data.lv;
            _dicPassSpeed[$"{tData.forDamId}_{tData.forChannelId}"] = tData.passSpeed * data.lv;
        }        
    }
    
    private void calculate_factor(int tId)
    {
        if (_dicShipFlow == null || _dicPassSpeed == null)
        {
            calculate_factor();
            return;
        }
        
        var data = Data.Nurtures[tId];
        var tData = TableCache.Instance.equipmentLvupTable[data.tId];
        _dicShipFlow[$"{tData.forDamId}_{tData.forChannelId}"] = tData.shipFlow * data.lv;
        _dicPassSpeed[$"{tData.forDamId}_{tData.forChannelId}"] = tData.passSpeed * data.lv;
    }
}