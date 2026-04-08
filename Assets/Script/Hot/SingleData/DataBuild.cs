using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class DataBuild:ReadAndSaveTool<DataBuild>
{
    private static string _savePath = "build";

    public Dictionary<int, DamBuildItem> Builds = new Dictionary<int, DamBuildItem>();
    
    public static DataBuild ReadFromFile()
    {
        return ReadByPhone(_savePath, () =>
            {
                var data = new DataBuild();
                foreach (var (_, dam) in TableCache.Instance.damTable)
                {
                    DamBuildItem damBuild = new()
                    {
                        damId = dam.id,
                        channels = new(),
                    };
                    for (int i = 0; i < DataDam.ChannelNum; i++)
                    {
                        ChannelBuildItem channelBuild = new ()
                        {
                            builds = new (),
                            channelId = i
                        };
                        damBuild.channels.Add(channelBuild);
                    }

                    data.Builds[dam.id] = damBuild;
                }
                
                foreach (var (_,value) in TableCache.Instance.buildingItemTable)
                {
                    var build = new BuildItem
                    {
                        tBuildId = value.id,
                        unlock = false,
                        buildId = data.Builds[value.forDamId].channels[value.forChannelId - 1].builds.Count, 
                    };

                    // if (value.forDamId is 1 && value.forChannelId is 0)
                    // {
                    //     build.unlock = true;
                    // }
                    //
                    // //todo 测试 开放1和2
                    // if (value.forDamId is 2  && value.forChannelId is 1 or 2)
                    // {
                    //     build.unlock = true;
                    // }
                    
                    data.Builds[value.forDamId].channels[value.forChannelId - 1].builds.Add(build);
                }

                return data;
            }
        );
    }
    
    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }
}

[Serializable]
public class DamBuildItem
{
    public int damId = -10000;
    //航道列表
    public List<ChannelBuildItem> channels;
}

[Serializable]
public class ChannelBuildItem
{
    public int channelId = -10000;
    //航道对应的建筑列表
    public List<BuildItem> builds;
}

[Serializable]
public class BuildItem
{
    public int tBuildId = 0; //建筑表中的id 对应的表中的数据 大坝id从1开始 航道id要从0开始 表数据的forChannelId需要减一
    public int buildId = 0; //第几个生成的就是几 方便做列表
    public bool unlock = false;
}