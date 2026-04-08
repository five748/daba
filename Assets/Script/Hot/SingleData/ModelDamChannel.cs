//纯数据类 航道数据 演化航道数据变化 操作航道界面

using System.Collections;
using UnityEngine;

public class ModelDamChannel
{
    private int _damId;
    private int _channelId;
    private bool _isShow;

    private ChannelItem _dataChannel;//数据
    private UIMainChannelItem _channelItem;//界面

    private Coroutine _coCreateShip;
    private Coroutine _coNextShip;
    private Coroutine _coPipeline;
    private Coroutine _coFixUpdatePos;
    
    private long _turnCount = 0;
    
    public ModelDamChannel(int damId, int channelId)
    {
        _damId = damId;
        _channelId = channelId;
        _isShow = false;
        _dataChannel = ChannelMgr.Instance.GetChannelData(damId, channelId);
        recovery_channel();
        run();
    }

    ~ModelDamChannel()
    {
        Stop();
    }

    public ModelDamChannel BindView(UIMainChannelItem mainChannelItem)
    {
        //重新绑定 初始化的时候大概率不会绑定
        //只有在操作界面的时候才需要用到 可以延迟绑定
        _channelItem = mainChannelItem;
        _isShow = true;
        stop_pipeline();
        recovery_channel();
        log($"航道 {mainChannelItem.name} 更换完毕");
        return this;
    }

    public ModelDamChannel UnbindView()
    {
        _isShow = false;
        _channelItem.ClearALlEvent();
        _channelItem = null;
        stop_pipeline();
        recovery_channel();
        return this;
    }
    
    public void Stop()
    {
        if (_coCreateShip != null)
        {
            MonoTool.Instance.StopCoroutine(_coCreateShip);
            _coCreateShip = null;
        }

        if (_coNextShip != null)
        {
            MonoTool.Instance.StopCoroutine(_coNextShip);
            _coNextShip = null;
        }

        if (_coFixUpdatePos != null)
        {
            MonoTool.Instance.StopCoroutine(_coFixUpdatePos);
            _coFixUpdatePos = null;
        }
        
        stop_pipeline();
    }

    private void stop_pipeline()
    {
        if (_coPipeline != null)
        {
            MonoTool.Instance.StopCoroutine(_coPipeline);
            _coPipeline = null;
        }
    }

    private void run()
    {
        //开始跑数据
        _coCreateShip = MonoTool.Instance.StartCor(create_ship());
        _coNextShip = MonoTool.Instance.StartCor(check_next_ship());
        _coFixUpdatePos = MonoTool.Instance.StartCor(fix_update_pos());
    }

    private IEnumerator create_ship()
    {
        while (true)
        {
            // if (!_dataChannel.shoufeizhan)
            // {
            //     ChannelMgr.Instance.FunUpdateChannelUI(_channelId);
            // }
            
            float timeWait;
            if (_dataChannel.ships.Count == 0)
            {
                timeWait = 1f;
            }
            else if (_dataChannel.ships.Count >= 1)
            {
                // timeWait = Random.Range(5f, 8f);
                timeWait = 5f;
            }
            else
            {
                // timeWait = Random.Range(6f, 9f);
                timeWait = 5f;
            }

            yield return new WaitForSeconds(timeWait);
            
            //太多就不创建了
            if (_dataChannel.ships.Count < ChannelMgr.MaxShipCount && _dataChannel.shoufeizhan)
            {
                make_ship();
            }
        }
    }

    public void GMMakeShip(int shipId)
    {
        make_ship(false, shipId);
    }

    private void make_ship(bool needRepair = false, int shipId = 0)
    {
        //没指定船舶id
        if (shipId == 0)
        {
            if (ChannelMgr.Instance.AlwaysOneID != int.MinValue)
            {
                shipId = ChannelMgr.Instance.AlwaysOneID;
            }
            else
            {
                shipId = ShipMgr.Instance.GetRandomUnlockShipId();
            }
        }
        
        // 20240515 去除船舶检修与主界面的交互
        // if (_channelId == 1)
        // {
        //     bool hasRepair = false;
        //     
        //     for (int i = 0; i < _dataChannel.ships.Count; i++)
        //     {
        //         var ship = _dataChannel.ships[i];
        //         if (ship.isRepair)
        //         {
        //             hasRepair = true;
        //             break;
        //         }
        //     }
        //     
        //     //当前没有需要修的船
        //     if (!hasRepair)
        //     {
        //         //默认不修的情况下 本条船舶随机修不修
        //         if (!needRepair)
        //         {
        //             needRepair = Random.Range(0, 101) > 95;
        //         }
        //     }
        // }

        ChannelShip dataShip = new ();// ChannelMgr.Instance.GetChannelShip();
        dataShip.channelId = _dataChannel.channelId;
        dataShip.isRepair = needRepair;
        dataShip.Pos = ChannelMgr.Instance.GetChannelFirstShipPos(_channelId) + ChannelMgr.Instance.GetChannelWaitDirection(_channelId) * ChannelMgr.MaxShipCount;
        dataShip.id = needRepair ? 13 : shipId;//修船的船舶id固定为13
        _dataChannel.ships.Add(dataShip);
        calculate_aim_pos(_dataChannel.ships.Count - 1);
        if (_isShow)
        {
            _channelItem.MakeNewShip(dataShip);
        }
    }
    
    private IEnumerator check_next_ship()
    {
        while (true)
        {
            //有移动的船舶时不能进入下一条 主要是恢复状态时
            if (!_dataChannel.waitNext || _dataChannel.moveShip != null)
            {
                yield return new WaitForSeconds(ChannelMgr.WaitNextShipTime);
            }
            else
            {
                if (!_dataChannel.waitRepair && !_dataChannel.waitWash)
                {
                    if (_dataChannel.ships.Count > 0)
                    {
                        if (ChannelMgr.Instance.CheckShipInAimPos(_dataChannel.ships[0]))
                        {
                            next_ship();
                        }
                    }
                }
                yield return new WaitForSeconds(ChannelMgr.WaitNextShipTime);
            }
        }
    }
    
    private void next_ship()
    {
        // return;
        if (_dataChannel.ships.Count <= 0)
        {
            return;
        }

        var firstShip = _dataChannel.ships[0]; 
        if (firstShip.isRepair)
        {
            ChannelMgr.Instance.RepairShipStart(_damId, _channelId);
            return;
        }
        
        //航道清洗事件 2024.05.14 独立出来 不与主界面交互
        // create_wash_event();

        if (_dataChannel.waitWash)
        {
            return;
        }
        
        //如果第一条船的目的地还不是排队中的第一个位置 直接返回
        if (firstShip.AimQueuePos != ChannelMgr.Instance.GetChannelFirstShipPos(_channelId))
        {
            log("第一条船尚未就位 不能开启下一轮");
            return;
        }

        ChannelMgr.Instance.TurnStart(_damId, _channelId);
        
        _turnCount++;
        log($"第 {_turnCount} 轮 下一艘船");
        
        if (ChannelMgr.Instance.CheckShipInAimPos(firstShip) && _dataChannel.shoufeizhan)
        {
            _dataChannel.moveShip = firstShip;
            if (_isShow)
            {
                _dataChannel.ships[0].tf.GetComponent<UIMainShipItem>().StopMove();
            }
            _dataChannel.ships.RemoveAt(0);
            calculate_all_ship_pos();
            
            stop_pipeline();
            _coPipeline = MonoTool.Instance.StartCor(ship_move_to_center());
        }
    }
    
    private void create_wash_event()
    {
        if (!ChannelMgr.CreateEvent)
        {
            return;
        }
        
        if (_dataChannel.waitWash)
        {
            return;
        }

        //todo 目前只在第一和第二条航道进行
        if (_channelId != 1 && _channelId != 0)
        {
            return;
        }

        if (Random.Range(0, 101) > 95)
        {
            log("触发清理航道事件");
            ChannelMgr.Instance.WashStart(_damId, _channelId);
        }
    }

    private void calculate_all_ship_pos()
    {
        var count = _dataChannel.ships.Count;
        for (int i = 0; i < count; i++)
        {
            calculate_aim_pos(i);
        }
    }
    
    private Vector3 calculate_aim_pos(int index)
    {
        var pos = ChannelMgr.Instance.GetChannelFirstShipPos(_channelId);
        var dataShip = _dataChannel.ships[index];
        if (index == 0)
        {
            dataShip.AimQueuePos = pos;
            return pos;
        }

        var dataPreShip = _dataChannel.ships[index - 1];
        var tShip = TableCache.Instance.shipTable[dataShip.id];
        var tPreShip = TableCache.Instance.shipTable[dataPreShip.id];
        var space = ChannelMgr.Instance.CalcShipSpace(tPreShip, tShip);
        dataShip.AimQueuePos = dataPreShip.AimQueuePos + ChannelMgr.Instance.GetChannelWaitDirection(_channelId).normalized * space;
        return dataShip.Pos;
    }
    
    private IEnumerator ship_move_to_center()
    {
        log("ship_move_to_center");

        if (!_isShow)
        {
            yield return new WaitForSeconds(ChannelMgr.ShipEnterCenterTime);
            
            _coPipeline = MonoTool.Instance.StartCor(close_qianmen());
            yield break;
        }
        
        _channelItem.ShipMoveToCenter();
    }

    public void CloseQianMen()
    {
        //由外部调用 当UI层存在的时候会被调用
        _coPipeline = MonoTool.Instance.StartCor(close_qianmen());
    }

    private IEnumerator close_qianmen()
    {
        log("close_qianmen");
        if (!_isShow)
        {
            ChannelMgr.Instance.DoorQianCloseStart(_damId, _channelId);
            var tFrame = TableRunData.Instance.frameNumAndSpeed[$"closeDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_1".ToString()];
            float costTime = Time.fixedDeltaTime * tFrame.sfNum * tFrame.frameRate;
            yield return new WaitForSeconds(costTime);
            ChannelMgr.Instance.DoorQianCloseEnd(_damId, _channelId);
            
            _coPipeline = MonoTool.Instance.StartCor(push_qianshui());
            yield break;
        }
        
        _channelItem.CloseQianMen();
    }

    public void PushQianShui()
    {
        _coPipeline = MonoTool.Instance.StartCor(push_qianshui());
    }
    private IEnumerator push_qianshui()
    {
        log("push_qianshui");

        if (_dataChannel.moveShip == null)
        {
            logError("push_qianshui时 moveShip 没有数据");
            yield break;
        }
        
        if (!_isShow)
        {
            ChannelMgr.Instance.WaterPushStart(_damId, _channelId);
            yield return new WaitForSeconds(ChannelMgr.PushWaterTime);
            ChannelMgr.Instance.WaterPushEnd(_damId, _channelId);
            
            _coPipeline = MonoTool.Instance.StartCor(daojishi());
            yield break;
        }
        
        _channelItem.PushQianShui();
    }

    public void DaoJiShi()
    {
        _coPipeline = MonoTool.Instance.StartCor(daojishi());
    }
    private IEnumerator daojishi()
    {
        log("daojishi");

        if (!_isShow)
        {
            yield return new WaitForSeconds(ChannelMgr.Instance.GetOverChannelTime(_damId, _channelId, _dataChannel.moveShip.id));
            var tShip = TableCache.Instance.shipTable[_dataChannel.moveShip.id];
            PlayerMgr.Instance.AddItemNum(1, ChannelMgr.Instance.GetOverChannelFee(_damId, _channelId, _dataChannel.moveShip.id),2);//增加金币
            
            _coPipeline = MonoTool.Instance.StartCor(open_houmen());
            yield break;
        }
        
        _channelItem.DaoJiShi();
    }

    public void OpenHouMen()
    {
        _coPipeline = MonoTool.Instance.StartCor(open_houmen());
    }
    private IEnumerator open_houmen()
    {
        log("open_houmen");

        if (!_isShow)
        {
            ChannelMgr.Instance.DoorHouOpenStart(_damId, _channelId);
            var tFrame = TableRunData.Instance.frameNumAndSpeed[$"openDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_2"];
            float costTime = Time.fixedDeltaTime * tFrame.sfNum * tFrame.frameRate;
            yield return new WaitForSeconds(costTime);
            ChannelMgr.Instance.DoorHouOpenEnd(_damId, _channelId);

            _coPipeline = MonoTool.Instance.StartCor(ship_leave());
            yield break;
        }
        _channelItem.OpenHouMen();
    }

    public void ShipLeave()
    {
        _coPipeline = MonoTool.Instance.StartCor(ship_leave());
    }
    private IEnumerator ship_leave()
    {
        if (_dataChannel.moveShip == null)
        {
            logError("ship_leave时 moveShip 没有数据数据");
            yield break;
        }

        log("ship_leave");
        //增加累计过闸数量
        PlayerMgr.Instance.data.SumOverDamNum += 1;
        if (UIEmpty.Instance != null) { 
            UIEmpty.Instance.UpDateAchRed();
        }
        if (!_isShow)
        {
            yield return new WaitForSeconds(ChannelMgr.ShipLeaveFirstTime);
            //船舶离开 回收船舶数据
            // ChannelMgr.Instance.ReclaimChannelShip(_dataChannel.moveShip);
            _dataChannel.moveShip = null;
            
            _coPipeline = MonoTool.Instance.StartCor(close_houmen());
            yield break;
        }
        
        _channelItem.ShipLeave();
    }

    public void CloseHouMen()
    {
        _coPipeline = MonoTool.Instance.StartCor(close_houmen());
    }
    private IEnumerator close_houmen()
    {
        log("close_houmen");
        if (!_isShow)
        {
            ChannelMgr.Instance.DoorHouCloseStart(_damId, _channelId);
            var tFrame = TableRunData.Instance.frameNumAndSpeed[$"closeDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_2"];
            float costTime = Time.fixedDeltaTime * tFrame.sfNum * tFrame.frameRate;
            yield return new WaitForSeconds(costTime);
            ChannelMgr.Instance.DoorHouCloseEnd(_damId, _channelId);

            _coPipeline = MonoTool.Instance.StartCor(pull_qianshui());
            yield break;
        }
        
        _channelItem.CloseHouMen();
    }

    public void PullQianShui()
    {
        _coPipeline = MonoTool.Instance.StartCor(pull_qianshui());
    }
    private IEnumerator pull_qianshui()
    {
        log("pull_qianshui");
        if (!_isShow)
        {
            ChannelMgr.Instance.WaterPullStart(_damId, _channelId);
            yield return new WaitForSeconds(ChannelMgr.PullWaterTime);
            ChannelMgr.Instance.WaterPullEnd(_damId, _channelId);
            
            _coPipeline = MonoTool.Instance.StartCor(open_qianmen());
            yield break;
        }
        
        _channelItem.PullQianShui();
    }
    
    public void OpenQianMen()
    {
        _coPipeline = MonoTool.Instance.StartCor(open_qianmen());
    }
    private IEnumerator open_qianmen()
    {
        log("open_qianmen");
        if (!_isShow)
        {
            ChannelMgr.Instance.DoorQianOpenStart(_damId, _channelId);
            var tFrame = TableRunData.Instance.frameNumAndSpeed[$"openDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_1"];
            float costTime = Time.fixedDeltaTime * tFrame.sfNum * tFrame.frameRate;
            yield return new WaitForSeconds(costTime);
            ChannelMgr.Instance.DoorQianOpenEnd(_damId, _channelId);
            ChannelMgr.Instance.TurnEnd(_damId, _channelId);
            yield break;
        }
        
        _channelItem.OpenQianMen();
    }

    private IEnumerator fix_update_pos()
    {
        //更新队列中的船舶位置数据
        while (true)
        {
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            int count = _dataChannel.ships.Count;
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var ship = _dataChannel.ships[i];
                    if (!ChannelMgr.Instance.CheckShipInAimPos(ship))
                    {
                        ship.Pos = Vector2.MoveTowards(ship.Pos, ship.AimQueuePos, ChannelMgr.ShipSpeed * Time.fixedDeltaTime);
                    }
                    else
                    {
                        ship.Pos = ship.AimQueuePos;
                    }
                }
            }

            //船舶向闸门内移动
            if (_dataChannel.moveShip != null && _dataChannel.openQianmen)
            {
                var shipInPos = ChannelMgr.Instance.GetShipInPos(_channelId);
                var firstShipPos = ChannelMgr.Instance.GetChannelFirstShipPos(_channelId);
                var distance = Time.fixedDeltaTime / ChannelMgr.ShipEnterCenterTime * (shipInPos - firstShipPos).magnitude; 
                _dataChannel.moveShip.Pos = Vector2.MoveTowards(
                    _dataChannel.moveShip.Pos, 
                    shipInPos, 
                    distance
                );
            }
        }
    }
    
    //重置航道
    private void recovery_channel()
    {
        if (_isShow && _channelItem == null)
        {
            logError("没有绑定对应的物体实例 需要先执行 BindView");
        }
        
        if (_channelItem != null)
        {
            _channelItem.ClearALlEvent();
            _channelItem.RecoveryShips();//船舶恢复
        }
        
        if (!_dataChannel.shoufeizhan)
        {
            log("还没解锁 不恢复");
            ChannelMgr.Instance.FunUpdateChannelUI(_channelId);
            return;
        }

        if (_dataChannel.ships.Count == 0 && _dataChannel.moveShip == null)
        {
            log("没有排队和移动中的船舶 不恢复");
            return;
        }
        
        //流程
        if (_dataChannel.openingQianmen)
        {
            log("恢复状态 打开前门");
            // 前门打开
            OpenQianMen();
        }
        else if (_dataChannel.openQianmen && !_dataChannel.pushQianshui && !_dataChannel.showQianshui && _dataChannel.moveShip != null)
        {
            log("恢复状态 开往闸道中心");
            // 船舶开到闸门中的指定位置
            _coPipeline = MonoTool.Instance.StartCor(ship_move_to_center());
        }
        else if (_dataChannel.closingQianmen)
        {
            log("恢复状态 关闭前门");
            // 前门关闭
            CloseQianMen();
        }
        else if (_dataChannel.pushQianshui)
        {
            log("恢复状态 注水");
            // 注水
            PushQianShui();
        }
        else if (_dataChannel.showQianshui && !_dataChannel.openingHoumen && !_dataChannel.openHoumen && !_dataChannel.pullQianshui)
        {
            log("恢复状态 倒计时");
            //倒计时中
            DaoJiShi();
        }
        else if (_dataChannel.openingHoumen)
        {
            log("恢复状态 后门打开");
            // 后门打开
            OpenHouMen();
        }
        else if (_dataChannel.openHoumen && !_dataChannel.closingHoumen && _dataChannel.moveShip != null)
        {
            log("恢复状态 船舶离开");
            // 船舶离开
            ShipLeave();
        }
        else if (_dataChannel.closingHoumen || (_dataChannel.openHoumen && !_dataChannel.closingHoumen))
        {
            log("恢复状态 后门关闭");
            // 后门关闭
            CloseHouMen();
        }
        else if (_dataChannel.pullQianshui)
        {
            log("恢复状态 排水");
            // 排水
            PullQianShui();
        }
        else if (_dataChannel.openingQianmen)
        {
            log("恢复状态 打开前门");
            // 打开前门
            OpenQianMen();
        }
    }

    private void log(string msg)
    {
        return;
        string strMsg = $"大坝 {_damId} 航道 {_dataChannel.channelId} {msg}";
        strMsg += _isShow ? "显示" : "不显示";
        strMsg = $"<color=#21A91F> {strMsg} </color>";
        Debug.Log(strMsg);
    }

    private void logError(string msg)
    {
        string strMsg = $"大坝 {_damId} 航道 {_dataChannel.channelId} {msg} ";
        strMsg += _isShow ? "显示" : "不显示";
        Debug.LogError(strMsg);
    }
}