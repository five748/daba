using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using UnityEngine;

public class ChannelMgr:Singleton<ChannelMgr>
{
    public DataDam Data { get; }
    public int DamId { get; private set; } = int.MinValue;
    
    public Action<int, int, int, int> FunBuildUnlockAnimation;//大坝id 航道id 建筑id 建筑表的id
    public Action<int> FunUpdateChannelUI;//更新航道的ui
    public Action<int> FunStaffUnlock;//职员id 职员解锁
    public Action<int> FunShipUnlock;//船舶id 船舶解锁
    public Action<int> ChangeDamUpdate; //界面绑定数据
    public Action UpdateEmptyDamInfo; //更新主界面大坝信息
    public Action UpdateEmptyUnlockDamRed; //解锁大坝后 主界面相关的红点更新

    public Action<int, Transform> FlyGold;//主界面飞金币

    public int AlwaysOneID = int.MinValue;
    
    // private Stack<ChannelShip> _poolChannelShips;
    
    //一些公用的静态变量
    public static readonly int MaxShipCount = 10; //船舶最大数量
    public static float WaitNextShipTime => 1f;
    public static float ShipEnterCenterTime => 3f;
    public static float PushWaterTime => 1f;
    public static float DaoJiShiTime => 4f;
    public static float ShipLeaveFirstTime => 2f;
    public static float ShipLeaveSecondTime => 3f;
    public static float PullWaterTime => 1f;
    public static float ShipSpeed => 300f;
    public static float SpaceShip = 0f;//船舶间距 额外添加的 美观作用
    
    //一些界面位置数据
    private Dictionary<int, Vector3> _dicFirstShipLocalPos;//队列第一条船舶的位置 按航道来
    private Dictionary<int, Vector3> _dicShipWaitDirection;//船舶等待的方向 按航道来
    private Dictionary<int, Vector3> _dicShipInPos;//船舶进入闸门的位置
    
    public static bool CreateEvent = false;//是否创建副玩法 默认不创建

    //model类
    private Dictionary<int, List<ModelDamChannel>> _dicDamChannel;
    
    private Coroutine _coTurnEndSaveData = null;
    private long _nextSaveTime = long.MinValue;
    
    private ChannelMgr()
    {
        Data = DataDam.ReadFromFile();
        BuildMgr.Instance.FunBuildUnlock = OnBuildUnlock;
        // _poolChannelShips = new Stack<ChannelShip>();
        _dicFirstShipLocalPos = new ();
        _dicShipWaitDirection = new ();
        _dicShipInPos = new Dictionary<int, Vector3>();
        //GameProcess.Instance.SaveEvent += Data.SaveToFile;
        MonoTool.Instance.StartCor(co_save_dam());
    }

    public void RunData()
    {
        _dicDamChannel = new();
        foreach (var (_, dam) in Data.Dams)
        {
            if (!dam.unlock)
            {
                continue;
            }
            _dicDamChannel[dam.damId] = new List<ModelDamChannel>();
            var count = dam.channels.Count;
            for (int i = 0; i < count; i++)
            {
                var channel = dam.channels[i];
                var data = new ModelDamChannel(dam.damId, channel.channelId);
                _dicDamChannel[dam.damId].Add(data);
            }
        }
    }

    public void UnlockDam(int damId)
    {
        Data.Dams[damId].unlock = true;
        Data.SaveToFile();
        _dicDamChannel[damId] = new List<ModelDamChannel>();
        var dam = Data.Dams[damId];
        for (int i = 0; i < DataDam.ChannelNum; i++)
        {
            var channel = dam.channels[i];
            var data = new ModelDamChannel(dam.damId, channel.channelId);
            _dicDamChannel[damId].Add(data);
        }
        
        UpdateEmptyUnlockDamRed?.Invoke();
    }

    public void BindModelView(UIMainChannelItem view, int damId, int channelId)
    {
        var model = _dicDamChannel[damId][channelId];
        view.BindModel(model, damId);
        model.BindView(view);
    }

    public void ChangeDam(int damId)
    {
        if (damId == DamId)
        {
            Debug.LogError($"当前显示的就是 {damId} 号大坝");
            Msg.Instance.Show($"已处在该大坝");
            return;
        }

        var dataDam = GetDamData(damId);
        if (!dataDam.unlock)
        {
            Debug.LogError($"{damId} 号大坝尚未解锁");
            return;
        }
        
        var channelList = _dicDamChannel[DamId];
        foreach (var channel in channelList)
        {
            channel.UnbindView();
        }

        DamId = damId;
        Data.finalDamId = damId;
        ChangeDamUpdate?.Invoke(damId);
        UpdateEmptyDamInfo?.Invoke();
        Data.SaveToFile();
    }

    public void WaterPushStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.pushQianshui = true;
    }
    
    public void WaterPushEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.pushQianshui = false;
        item.showQianshui = true;
    }

    public void WaterPullStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.pullQianshui = true;
    }

    public void WaterPullEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.pullQianshui = false;
        item.showQianshui = false;
    }
    
    public void TurnStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitNext = false;
    }

    public void TurnEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitNext = true;
        turn_end_save();
    }

    public void DoorQianOpenStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.openingQianmen = true;
    }

    public void DoorQianOpenEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.openingQianmen = false;
        item.openQianmen = true;
    }
    
    public void DoorQianCloseStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.closingQianmen = true;
    }

    public void DoorQianCloseEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.closingQianmen = false;
        item.openQianmen = false;
    }

    public void DoorHouOpenStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.openingHoumen = true;
    }

    public void DoorHouOpenEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.openingHoumen = false;
        item.openHoumen = true;
    }
    
    public void DoorHouCloseStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.closingHoumen = true;
    }

    public void DoorHouCloseEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.closingHoumen = false;
        item.openHoumen = false;
    }

    /**
     * @param save 是否是保存的数据
     */
    private void OnBuildUnlock(int damId, int channelId, int buildId, int tBuildId)
    {
        //?Debug.LogError(buildId + ":" + tBuildId);
        MTaskData.Instance.AddTaskNum( MainTaskMenu.OpenSomeOneEquip, 1, tBuildId);
        //在这一层把对应的的id转换为字段
        var dataChannel = GetChannelData(damId, channelId);
        switch (buildId)
        {
            case 0:
                dataChannel.shoufeizhan = true;
                break;
            case 1:
                dataChannel.jiushengting = true;
                break;
            case 2:
                dataChannel.luntai = true;
                break;
            case 3:
                dataChannel.diaoji = true;
                break;
            case 4:
                dataChannel.dengta = true;
                break;
            case 5:
                dataChannel.jiqi = true;
                break;
            case 6:
                dataChannel.honglvdeng = true;
                break;
            case 7:
                dataChannel.xiansu = true;
                break;
        }

        Data.SaveToFile();

        // if (buildId > TableCache.Instance.configTable[901].param.ToInt())
        // {
            var tBuild = TableCache.Instance.buildingItemTable[tBuildId];
            var tip = $"评分+{tBuild.score}";
            Msg.Instance.Show(tip);
            // return;
        // }
        
        if (damId != DamId)
        {
            Debug.Log($"解锁设备 与当前大坝不同 切换大坝");
            ChangeDam(damId);
        }
        FunBuildUnlockAnimation?.Invoke(damId, channelId, buildId, tBuildId);
    }

    public void RepairShipStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitRepair = true;
    }

    public void RepairShipEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitRepair = false;
        if (item.ships.Count <= 0)
        {
            return;
        }

        item.ships[0].isRepair = false;
        item.ships[0].id = 14;
        item.ships[0].tf.GetComponent<UIMainShipItem>().RepairEnd();
    }

    public void WashStart(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitWash = true;
        FunUpdateChannelUI?.Invoke(channelId);
    }

    public void WashEnd(int damId, int channelId)
    {
        var item = GetChannelData(damId, channelId);
        item.waitWash = false;
        FunUpdateChannelUI?.Invoke(channelId);
    }

    public void UnlockStaff(int staffID)
    {
        var tStaff = TableCache.Instance.tollCollectorTable[staffID];
        var item = GetChannelData(tStaff.forDamId, tStaff.forChannelId - 1);
        item.staffId = staffID;
        Data.SaveToFile();
        if (tStaff.forDamId != DamId)
        {
            Debug.Log($"解锁设备 与当前大坝不同 切换大坝");
            ChangeDam(tStaff.forDamId);
        }
        
        FunStaffUnlock?.Invoke(staffID);
    }

    public void UnlockShip(int shipId)
    {
        FunShipUnlock?.Invoke(shipId);
    }

    public bool CheckUnlockShouFeiZhan(int damId, int channelId, int staffID)
    {
        return GetCurDamChannelData(staffID - 1).shoufeizhan;
    }

    public float CalcShipSpace(ship tShip0, ship tShip1)
    {
        var hyPre = Mathf.Sqrt(tShip0.ship_size[0] * tShip0.ship_size[0] * tShip0.scale_size * tShip0.scale_size + tShip0.ship_size[1] * tShip0.ship_size[1] * tShip0.scale_size * tShip0.scale_size);
        float hySelf = Mathf.Sqrt(tShip1.ship_size[0] * tShip1.ship_size[0] * tShip1.scale_size * tShip1.scale_size + tShip1.ship_size[1] * tShip1.ship_size[1] * tShip1.scale_size * tShip1.scale_size);
        float space = (hyPre + hySelf) / 2 + SpaceShip;
        return space;
    }

    public void OnCreateEvent(bool value)
    {
        var str = value ? "开启副玩法" : "关闭副玩法";
        Debug.Log(str);
        CreateEvent = value;
    }

    public void SetDamId(int damID)
    {
        DamId = damID;
    }

    public ChannelItem GetChannelData(int damId, int channelId)
    {
        if (damId == int.MinValue || channelId == int.MinValue)
        {
            Debug.LogError("没有进行初始化--需要先执行init传入对应的damId和channelId ---检查代码");
            return new ChannelItem();
        }

        Data.Dams.TryGetValue(damId, out var dam);
        if (dam == null)
        {
            Debug.LogError("不存在对应的大坝信息 ---检查代码");
            return new ChannelItem();
        }

        return dam.channels[channelId];
    }
    
    public DamItem GetDamData(int damId)
    {
        if (damId == int.MinValue)
        {
            Debug.LogError("damId错误 ---检查代码");
            return new DamItem();
        }

        Data.Dams.TryGetValue(damId, out var dam);
        if (dam == null)
        {
            Debug.LogError("不存在对应的大坝信息 ---检查代码");
            return new DamItem();
        }

        return dam;
    }

    //是否解锁大坝
    public bool IsLockDam(int damId)
    {
        var dataDam = GetDamData(damId);
        return dataDam.unlock;
    }
    
    
    /// <summary>
    /// 获取当前操作的大坝数据
    /// </summary>
    /// <returns></returns>
    public DamItem GetCurDamData()
    {
        if (DamId == int.MaxValue)
        {
            Debug.LogError("没有进行初始化--需要先执行SetDamId传入对应的damId ---检查代码");
            return new DamItem();
        }

        return GetDamData(DamId);
    }
    
    /// <summary>
    /// 获取前操作大坝的某个通道的信息
    /// </summary>
    /// <param name="channelId"></param>
    /// <returns></returns>
    public ChannelItem GetCurDamChannelData(int channelId)
    {
        if (DamId == int.MaxValue)
        {
            Debug.LogError("没有进行初始化--需要先执行SetDamId传入对应的damId ---检查代码");
            return new ChannelItem();
        }

        return GetChannelData(DamId, channelId);
    }

    // public void ReclaimChannelShip(ChannelShip ship)
    // {
    //     if (ship != null)
    //     {
    //         ship.tf.DOKill();
    //         ship.tf = null;
    //         ship.Pos = Vector3.zero;
    //         ship.AimQueuePos = Vector3.zero;
    //         ship.isRepair = false;
    //         _poolChannelShips.Push(ship);
    //     }
    // }

    // public ChannelShip GetChannelShip()
    // {
    //     ChannelShip ship = null;
    //     if (_poolChannelShips.Count > 0)
    //     {
    //         ship = _poolChannelShips.Pop();
    //     }
    //
    //     if (ship == null)
    //     {
    //         ship = new ChannelShip();
    //     }
    //     return ship;
    // }

    public void SetChannelRelativePos(int channelId, Vector3 firstShipPos, Vector3 directionPos, Vector3 shipInPos)
    {
        _dicFirstShipLocalPos.TryAdd(channelId, firstShipPos);
        _dicShipWaitDirection.TryAdd(channelId, directionPos);
        _dicShipInPos.TryAdd(channelId, shipInPos);
    }

    public Vector3 GetChannelFirstShipPos(int channelId)
    {
        _dicFirstShipLocalPos.TryGetValue(channelId, out var result);
        return result;
    }

    public Vector3 GetChannelWaitDirection(int channelId)
    {
        _dicShipWaitDirection.TryGetValue(channelId, out var result);
        return result;
    }
    
    public Vector3 GetShipInPos(int channelId)
    {
        _dicShipInPos.TryGetValue(channelId, out var result);
        return result;
    }

    public bool CheckShipInAimPos(ChannelShip ship)
    {
        var leftPos = ship.AimQueuePos - ship.Pos;
        if (leftPos.magnitude < 1f)
        {
            return true;
        }

        return false;
    }
    
    private void turn_end_save()
    {
        if (_nextSaveTime != long.MinValue)
        {
            return;
        }
        
        _nextSaveTime = TimeTool.CurTimeSeconds + 5;
        // Debug.Log($"下一次保存大坝数据的时间 _nextSaveTime {_nextSaveTime} TimeTool.CurTimeSeconds {TimeTool.CurTimeSeconds}");
    }

    private IEnumerator co_save_dam()
    {
        while (true)
        {
            if (_nextSaveTime == long.MinValue || Data == null)
            {
                yield return new WaitForSeconds(1f);
            }
            else
            {
                if (_nextSaveTime <= TimeTool.CurTimeSeconds)
                {
                    // Debug.Log($"保存大坝数据 TimeTool.CurTimeSeconds {TimeTool.CurTimeSeconds}");
                    Data.SaveToFile();
                    _nextSaveTime = long.MinValue;
                }
                else
                {
                    yield return new WaitForSeconds(_nextSaveTime - TimeTool.CurTimeSeconds);
                }
            }
        }
    }

    public float GetOverChannelTime(int damId, int channelId, int shipId)
    {
        var tShip = TableCache.Instance.shipTable[shipId];
        float result = tShip.passTime;

        var dataChannel = GetChannelData(damId, channelId);
        var dataShip = ShipMgr.Instance.data.ships[shipId];

        var staffFactory = 0;
        if (dataChannel.staffId != 0)
        {
            var dataStaff = StaffMgr.Instance.data.staffs[dataChannel.staffId];
            staffFactory = dataStaff.SumAddSpeed;
        }

        var adFactory = ADMgr.Instance.GetAttr(2);
        var nurtureFactory = NurtureMgr.Instance.GetPassSpeedFactor(damId, channelId);
        result = (tShip.passTime - dataShip.cutPassTime) / (float)(100 + dataShip.preLockageTime + staffFactory + nurtureFactory + adFactory) * 100f;
        
        return result;
    }

    public int GetOverChannelFee(int damId, int channelId, int shipId)
    {
        var tShip = TableCache.Instance.shipTable[shipId];
        int result = tShip.toll;
        var adFactory = ADMgr.Instance.GetAttr(1);
        result = Mathf.FloorToInt(tShip.toll * (float)(100 + adFactory) / 100f);
        return result;
    }

    public bool CheckRed()
    {
        foreach (var (key, dam) in Data.Dams)
        {
            if (!dam.unlock)
            {
                var tDam = TableCache.Instance.damTable[dam.damId];
                if (tDam.needScore <= PlayerMgr.Instance.data.score)
                {
                    return true;
                }

                return false;
            }
        }

        return false;
    }

    public void GMMakeShip(int channelId, int shipId)
    {
        var channelList = _dicDamChannel[DamId];
        channelList[channelId].GMMakeShip(shipId);
    }

    public int getCurDamIconId(int damId)
    {
        int iconId = damId % 6;
        if (iconId == 0)
        {
            iconId = 6; 
        } 
        return iconId;
    }
}