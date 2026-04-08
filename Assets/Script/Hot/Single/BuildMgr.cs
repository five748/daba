

using System;
using System.Collections.Generic;
using TreeData;
using UnityEngine;
using YuZiSdk;

public class BuildMgr : Singleton<BuildMgr>
{
    public DataBuild Data { get; }
    
    public Action<int, int, int, int> FunBuildUnlock;//大坝id 航道id 建筑id 建筑表的id
    
    private BuildMgr()
    {
        Data = DataBuild.ReadFromFile();
    }

    /// <summary>
    /// 解锁设施
    /// </summary>
    /// <param name="damId">大坝id 从1开始</param>
    /// <param name="channelId">航道id 从0开始</param>
    /// <param name="buildId">建筑id</param>
    /// <param name="tBuildId">建筑配表id</param>
    public void Unlock(int damId, int channelId, int buildId, int tBuildId)
    {
        Data.Builds[damId].channels[channelId].builds[buildId].unlock = true;
        Data.SaveToFile();
        FunBuildUnlock?.Invoke(damId, channelId, buildId, tBuildId);
        var tBuild = TableCache.Instance.buildingItemTable[tBuildId];
        PlayerMgr.Instance.AddScore(tBuild.score);
        
        YuziMgr.Instance.ReportCustom(7002, new Dictionary<string, object>()
        {
            {"sheshi",tBuildId}
        });
    }
    
    public bool CheckRed()
    {
        foreach (var (_, dam) in Data.Builds)
        {
            var dataDam = ChannelMgr.Instance.GetDamData(dam.damId);
            if (!dataDam.unlock)
            {
                //大坝没解锁
                break;
            }

            foreach (var channel in dam.channels)
            {
                foreach (var build in channel.builds)
                {
                    if (!build.unlock)
                    {
                        var tBuild = TableCache.Instance.buildingItemTable[build.tBuildId];
                        return PlayerMgr.Instance.IsEnough(1, tBuild.unlock_cost);
                    }
                }
            }
        }

        return false;
    }
}