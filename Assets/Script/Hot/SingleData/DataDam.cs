using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using LitJson;

[Serializable]
public class DataDam:ReadAndSaveTool<DataDam>
{
    private static string _savePath = "dam";

    public Dictionary<int, DamItem> Dams = new Dictionary<int, DamItem>();
    public int finalDamId = 1;

    public  static int ChannelNum = 13;//航道数量
    
    public static DataDam ReadFromFile()
    {
        var data = ReadByPhone(_savePath, () =>
            {
                var dataDam = new DataDam();
                
                //初始化默认1号大坝
                dataDam.finalDamId = 1;
                
                return dataDam;
            }
        );

        //表更新时的数据补充
        var tDams = TableCache.Instance.damTable.ToValList();
        int damCount = tDams.Count;
        if (data.Dams.Count < damCount)
        {
            for (int i = data.Dams.Count; i < damCount; i++)
            {
                var tDam = tDams[i];
                DamItem damItem = new DamItem
                {
                    damId = tDam.id
                };

                init_channels_data(damItem);
                if (tDam.id is 1)
                {
                    damItem.unlock = true;
                    damItem.channels[0].shoufeizhan = true;
                    BuildMgr.Instance.Data.Builds[tDam.id].channels[0].builds[0].unlock = true;
                    BuildMgr.Instance.Data.SaveToFile();
                }
                
                //todo 测试 开放1和2
                // if (tDam.id is 2)
                // {
                //     damItem.unlock = true;
                //     damItem.channels[0].shoufeizhan = true;
                //     damItem.channels[1].shoufeizhan = true;
                //     
                //     BuildMgr.Instance.Data.Builds[tDam.id].channels[0].builds[0].unlock = true;
                //     BuildMgr.Instance.Data.Builds[tDam.id].channels[1].builds[0].unlock = true;
                // }
                data.Dams.TryAdd(tDam.id, damItem);
            }
        }
        return data;
    }

    private static void init_channels_data(DamItem damItem)
    {
        damItem.channels = new List<ChannelItem>();
        for (int i = 0; i < ChannelNum; i++)
        {
            ChannelItem item = new ChannelItem
            {
                channelId = i
            };
            damItem.channels.Add(item);
        }
    }
    
    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }
}

//大坝数据
[Serializable]
public class DamItem
{
    public int damId = 0;
    public bool unlock = false;//是否解锁
    public List<ChannelItem> channels = new List<ChannelItem>();
}

//航道数据
[Serializable]
public class ChannelItem
{
    public int channelId = 0;
    public int staffId = 0;//员工id
    public bool dengta = false;
    public bool diaoji = false;
    public bool shoufeizhan = false;
    public bool honglvdeng = false;
    public bool jiushengting = false;
    public bool luntai = false;
    public bool jiqi = false;
    public bool xiansu = false;//限速牌
    
    public bool showQianshui = false;//前水
    // public bool show_houshui = false;//后水
    public bool openQianmen = true;//前门打开
    public bool openHoumen = false;//后门打开
    public bool pushQianshui = false;//灌注前水 动画中
    public bool pullQianshui = false;//抽取前水 动画中
    public bool openingQianmen = false;//前门打开中
    public bool openingHoumen = false;//后门打开中
    public bool closingQianmen = false;//前门关闭中
    public bool closingHoumen = false;//后门关闭中

    public bool waitNext = true;
    public bool waitRepair = false;//等待船只修复
    public bool waitWash = false;//等待清洗航道

    public List<ChannelShip> ships = new List<ChannelShip>();//航道上的船舶数据
    public ChannelShip moveShip = null;//已经脱离队列 在进行操作的船舶
    [LitJson.JsonIgnore]
    public string IconQianmen => openQianmen ? $"qianmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/kai" : $"qianmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/guan";
    [LitJson.JsonIgnore]
    public string IconHoumen
    {
        get
        {
            if (openHoumen)
            {
                return $"houmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/kai";
            }

            if (pullQianshui || pushQianshui)
            {
                return $"houmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/guan2";
            }

            return showQianshui ? $"houmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/guan" : $"houmen{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/guan2";
        }
    }
    [LitJson.JsonIgnore]
    public string IconQianshui
    {
        get
        {
            if (!closingHoumen && !openingHoumen && (showQianshui || pushQianshui))
            {
                return $"qianshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/0";
            }

            return $"qianshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/1";
        }
    }
    [LitJson.JsonIgnore]
    public string IconHoushui => showQianshui ? $"houshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/man" : $"houshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/kong";
    [LitJson.JsonIgnore]
    public string IconPingTai => $"dampingtai/{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}";
    [LitJson.JsonIgnore]
    public string Iconzsx => $"damzsx/{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}";
    [LitJson.JsonIgnore]
    public string Iconyx => $"damyx/{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}";
    [LitJson.JsonIgnore]
    public string IconHouShuiMan => $"houshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/man";
    [LitJson.JsonIgnore]
    public string IconHouShuiKong => $"houshui{ChannelMgr.Instance.getCurDamIconId(ChannelMgr.Instance.Data.finalDamId)}/kong";
    [LitJson.JsonIgnore]
    public string staffAniId
    {
        get {
            var tStaff = TableCache.Instance.tollCollectorTable[staffId];
            return tStaff.aniId;
        }
    }
}

[Serializable]
public class ChannelShip
{
    public int id = int.MinValue;
    public int channelId = int.MinValue;
    public bool isRepair = false;

    [NonSerialized]
    [LitJson.JsonIgnore]
    public Transform tf = null;

    [NonSerialized]
    [LitJson.JsonIgnore]
    private Vector3 _pos = Vector3.zero;
    [LitJson.JsonIgnore]
    public Vector3 Pos
    {
        get
        {
            if (_pos is { x: 0, y: 0, z: 0 })
            {
                _pos = new Vector3(savePos[0], savePos[1], savePos[2]);
            }

            return _pos;
        }
        set
        {
            savePos[0] = value[0];
            savePos[1] = value[1];
            savePos[2] = value[2];
            _pos = value;
        }
    }
    public float[] savePos = new float[3];
    [LitJson.JsonIgnore]
    [NonSerialized]
    private Vector3 _aimQueuePos = Vector3.zero;
    [LitJson.JsonIgnore]
    public Vector3 AimQueuePos
    {
        get
        {
            if (_aimQueuePos is { x: 0, y: 0, z: 0 })
            {
                _aimQueuePos = new Vector3(saveAimQueuePos[0], saveAimQueuePos[1], saveAimQueuePos[2]);
            }

            return _aimQueuePos;
        }
        set
        {
            saveAimQueuePos[0] = value[0];
            saveAimQueuePos[1] = value[1];
            saveAimQueuePos[2] = value[2];
            _aimQueuePos = value;
        }
    }
    public float[] saveAimQueuePos = new float[3];
}