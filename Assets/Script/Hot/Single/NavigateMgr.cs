

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeData;
using Unity.VisualScripting;
using UnityEngine;
using YuZiSdk;

public class NavigateMgr : Singleton<NavigateMgr>
{
    public DataNavigate Data { get; }

    public Action FunUpdateUICargo;

    public UIMainNavigateShip ComNavigateShip;

    #region NavigateShip 航运相关
    public Action FunNavigateSend; //航运 送货
    public Action FunNavigateBack; //航运 回港
    
    private List<int> _sendShipList = new(); //发送的船舶列表
    private List<int> _sendShipAniList = new(); //发送的船舶动画列表
    private List<int> _backShipAniList = new(); //返回的船舶动画列表
    
    public int NavigateShipId { get; private set; } = int.MinValue; //当前用于航行系统的船舶 用于界面表现
    public bool ShipStayAtPort = false;
    
    #endregion NavigateShip
    
    private NavigateMgr()
    {
        Data = DataNavigate.ReadFromFile();
        //GameProcess.Instance.SaveEvent += Data.SaveToFile;
    }

    public void FullCargoAd(Action<bool> call)
    {
        SdkMgr.Instance.ShowAd(10, b =>
        {
            if (b)
            {
                Data.cargoCount = Data.capacity;
                Data.SaveToFile();
            }
            call?.Invoke(b);
        });
    }

    public bool CapacityUplevel()
    {
        if (Data.capacityLv >= TableCache.Instance.warehouseTable.Count)
        {
            Msg.Instance.Show($"已满级");
            return false;
        }

        var cost = TableCache.Instance.warehouseTable[Data.capacityLv].capacityCost[0];
        if (PlayerMgr.Instance.AddItemNum(cost.id, -cost.num))
        {
            Data.capacityLv++;
            Data.capacity = TableCache.Instance.warehouseTable[Data.capacityLv].capacity;
            Data.SaveToFile();
            return true;
        }
        
        return false;
    }

    public bool OutputUplevel()
    {
        if (Data.outputLv >= TableCache.Instance.warehouseTable.Count)
        {
            Msg.Instance.Show($"已满级");
            return false;
        }
        
        var cost = TableCache.Instance.warehouseTable[Data.capacityLv].outputCost[0];
        if (PlayerMgr.Instance.AddItemNum(cost.id, -cost.num))
        {
            Data.outputLv++;
            Data.output = TableCache.Instance.warehouseTable[Data.outputLv].output;
            Data.SaveToFile();
            return true;
        }
        
        return false;
    }

    public void UplevelShip(int tId, Action<bool> call)
    {
        var ship = Data.ships[tId];
        var tShip = TableCache.Instance.cargoShipTable[tId];

        if (ship.lv + 1 > 999)
        {
            Msg.Instance.Show($"已满级");
            return;
        }
        
        if (ship.lv == 0)
        {
            if (tShip.unlockType == 1)
            {
                if (PlayerMgr.Instance.data.score >= tShip.score)
                {
                    ship.lv = 1;
                    call?.Invoke(true);
                    //分配订单
                    var orderId = UnityEngine.Random.Range(1, TableCache.Instance.orderTable.Count + 1);
                    ship.tOrderId = orderId;
                    Data.SaveToFile();
                }
                else
                {
                    call?.Invoke(false);
                }
            }
            else
            {
                SdkMgr.Instance.ShowAd(6, b =>
                {
                    if (b)
                    {
                        ship.lv = 1;
                        //分配订单
                        var orderId = UnityEngine.Random.Range(1, TableCache.Instance.orderTable.Count + 1);
                        ship.tOrderId = orderId;
                        Data.SaveToFile();
                    }
                    call?.Invoke(b);
                });
            }
        }
        else
        {
            if (PlayerMgr.Instance.AddItemNum(tShip.cost[0].id, -tShip.cost[0].num))
            {
                ship.lv++;
                call?.Invoke(true);
                Data.SaveToFile();
            }
            else
            {
                call?.Invoke(false);
            }
        }
    }

    public void SendOrder(int tShipId)
    {
        var ship = Data.ships[tShipId];
        if (ship.tOrderId == int.MinValue)
        {
            Debug.LogError($"发送订单失败 该船已经有订单了 检查代码");
            return;
        }

        if (Data.cargoCount < ship.Capacity)
        {
            Debug.LogError($"当前货物不足 不能发货 检查代码");
            return;
        }
        MTaskData.Instance.AddTaskNum(MainTaskMenu.SumShipSendNum);
        var tOrder = TableCache.Instance.orderTable[ship.tOrderId];
        
        var speedTime = tOrder.dis / ship.Speed;
        //todo 测试使用 缩短时间
        // speedTime /= 20;
        ship.sendTime = TimeTool.CurTimeSeconds + speedTime;
        
        ship.cargoCount = ship.Capacity;
        Data.cargoCount -= ship.Capacity;
        Data.SaveToFile();

        NavigateShipSendAdd(tShipId);
        _sendShipList.Add(tShipId);
        MonoTool.Instance.Wait(speedTime, () =>
        {
            _sendShipList.Remove(tShipId);
            NavigateShipBackAdd(tShipId);
        });
        
        FunUpdateUICargo?.Invoke();
    }

    public void GetOrderReward(int tShipId, int ratio, Action<bool> call)
    {
        if (ratio > 1)
        {
            SdkMgr.Instance.ShowAd(1, (b)=>
            {
                if (b)
                {
                    get_order_reward(tShipId, ratio);
                }
                call?.Invoke(b);
            });
        }
        else
        {
            get_order_reward(tShipId, ratio);
            call?.Invoke(true);
        }
    }

    public int GetRewardCount(Table.order tOrder, CargoShipItem dataShip)
    {
        return Mathf.FloorToInt(tOrder.rewardCoe * get_order_coe() * dataShip.Capacity);
    }

    private void get_order_reward(int tShipId, int ratio)
    {
        var ship = Data.ships[tShipId];
        var tOrder = TableCache.Instance.orderTable[ship.tOrderId];
        var orderId = UnityEngine.Random.Range(1, TableCache.Instance.orderTable.Count + 1);
        ship.tOrderId = orderId;
        ship.cargoCount = 0;
        ship.sendTime = 0;
        Data.SaveToFile();
        var coe = get_order_coe();
        // Debug.Log($"航运奖励数值 tOrder.rewardCoe {tOrder.rewardCoe}, coe {coe}, ship.Capacity {ship.Capacity}, isDouble {isDouble}");
        // PlayerMgr.Instance.AddItemNum(1,  Mathf.FloorToInt( tOrder.rewardCoe * get_order_coe() * ship.Capacity * (isDouble? 2: 1)));
        PlayerMgr.Instance.AddItemNum(1,  GetRewardCount(tOrder, ship) * ratio);        

        if (_backShipAniList.Contains(tShipId))
        {
            _backShipAniList.Remove(tShipId);
        }
    }

    private int get_order_coe()
    {
        var scoreDivTen = PlayerMgr.Instance.data.score / 10f;
        var scoreRound = Mathf.RoundToInt(scoreDivTen);
        int tId = 5;
        if (scoreRound > scoreDivTen)
        {
            tId = scoreRound * 10 - 5;
        }
        else if (scoreRound < scoreDivTen)
        {
            tId = scoreRound * 10 + 5;
        }
        else
        {
            tId = scoreRound * 10;
        }

        var tLast = TableCache.Instance.orderCoeTable.Last();
        if (tLast.Value.id <= tId)
        {
            return tLast.Value.coe;
        }
        
        TableCache.Instance.orderCoeTable.TryGetValue(tId, out var tOrderCoe);
        if (tOrderCoe != null)
        {
            return tOrderCoe.coe;
        }
        return 1;
    }

    /// <summary>
    /// 计算货物数量
    /// </summary>
    public void StartGameCalculate()
    {
        if (PlayerMgr.Instance.data.OutLineTime != 0)
        {
            var outTime = TimeTool.SerNowUtcTimeInt - PlayerMgr.Instance.data.OutLineTime;
            //20240521 分改为秒
            // int min = Mathf.CeilToInt(outTime / 60.0f);
            var output = outTime * Data.output;
            Data.cargoCount += output;
            Data.cargoCount = Mathf.Min(Data.cargoCount, Data.capacity);
        }
        MonoTool.Instance.StartCor(add_cargo());
    }

    private IEnumerator add_cargo()
    {
        while (true)
        {
            //20240521 生产的单位时间由分改为秒
            // yield return new WaitForSeconds(60);
            yield return new WaitForSeconds(1);
            Data.cargoCount += Data.output;
            Data.cargoCount = Math.Min(Data.cargoCount, Data.capacity);
            FunUpdateUICargo?.Invoke();
            Data.SaveToFile();
        }
    }

    public void StartCheckShipBack()
    {
        foreach (var (_, ship) in Data.ships)
        {
            if (ship.lv <= 0)
            {
                continue;
            }

            if (ship.sendTime <= 0 && ship.cargoCount <= 0)
            {
                continue;
            }

            if (ship.sendTime <= TimeTool.CurTimeSeconds)
            {
                NavigateShipBackAdd(ship.tId);
            }
            else
            {
                MonoTool.Instance.Wait(ship.sendTime - TimeTool.CurTimeSeconds, () =>
                {
                    NavigateShipBackAdd(ship.tId);
                });
            }
        }
        
        navigate_ship_back();
        // _coAni = MonoTool.Instance.UpdateCall(1f, () =>
        // {
        //     navigate_ship_send();
        //     navigate_ship_back();
        // });
    }

    public void NavigateShipSendAdd(int tShipId)
    {
        if (_sendShipAniList.Contains(tShipId))
        {
            _sendShipAniList.Remove(tShipId);
        }
        
        if (_backShipAniList.Contains(tShipId))
        {
            _backShipAniList.Remove(tShipId);
        }
        _sendShipAniList.Add(tShipId);
        navigate_ship_send();
    }

    public void NavigateShipBackAdd(int tShipId)
    {
        if (_backShipAniList.Contains(tShipId))
        {
            _backShipAniList.Remove(tShipId);
        }

        if (_sendShipAniList.Contains(tShipId))
        {
            _sendShipAniList.Remove(tShipId);
        }
        
        _backShipAniList.Add(tShipId);
        navigate_ship_back();
    }

    private void navigate_ship_send()
    {
        navigate_ship_check(true);
    }

    private void navigate_ship_back()
    {
        navigate_ship_check(false);
    }

    private void navigate_ship_check(bool isSend)
    {
        if (isSend)
        {
            //有船 且移动中
            if (NavigateShipId != int.MinValue)
            {
                // Debug.Log($"有船在港口中 不进行下一组 航运-发货-动作");
                Debug.Log($"有船在航行中或者港口中 不进行下一组 航运-发货-动作");
                return;
            }
            if (_sendShipAniList.Count > 0)
            {
                NavigateShipId = _sendShipAniList[0];
                ShipStayAtPort = true;
                _sendShipAniList.RemoveAt(0);
                ComNavigateShip.UpdateUI(NavigateShipId);
                Debug.Log($"发送的船舶id为{NavigateShipId}");
                FunNavigateSend?.Invoke();
            }
        }
        else
        {
            if (NavigateShipId != int.MinValue || ShipStayAtPort)
            {
                Debug.Log($"有船在航行中或者港口中 不进行下一组 航运-回港-动作");
                return;
            }
            if (_backShipAniList.Count > 0)
            {
                NavigateShipId = _backShipAniList[0];
                _backShipAniList.RemoveAt(0);
                ComNavigateShip.UpdateUI(NavigateShipId);
                FunNavigateBack?.Invoke();
            }
        }
    }

    public void NavigateEnd(bool isBack)
    {
        ShipStayAtPort = isBack;
        //重置船舶id
        NavigateShipId = int.MinValue;
        //优先检查发货船舶
        navigate_ship_send();
        navigate_ship_back();
    }

    public bool CheckRedMain()
    {
        //升级检查
        var tCapacity = TableCache.Instance.warehouseTable[Data.capacityLv];
        var tOutput = TableCache.Instance.warehouseTable[Data.outputLv];

        var hasNum = PlayerMgr.Instance.GetItemNum(2);
        if (tCapacity.capacityCost[0].num <= hasNum)
        {
            return true;
        }

        if (tOutput.outputCost[0].num <= hasNum)
        {
            return true;
        }

        if (CheckRedCompany())
        {
            return true;
        }
        
        return false;
    }

    public bool CheckRedOrder()
    {
        foreach (var (_, ship) in Data.ships)
        {
            if (ship.lv <= 0)
            {
                continue;
            }
            
            //非空船
            if (ship.cargoCount > 0 || ship.sendTime > 0)
            {
                continue;
            }

            if (Data.cargoCount >= ship.Capacity && ship.sendTime <= 0)
            {
                return true;
            }
        }
        
        return false;
    }

    public bool CheckRedCompany()
    {
        foreach (var (_, ship) in Data.ships)
        {
            var tShip = TableCache.Instance.cargoShipTable[ship.tId];

            if (ship.lv == 0)
            {
                if (tShip.unlockType == 1)
                {
                    if (tShip.score <= PlayerMgr.Instance.data.score)
                    {
                        return true;
                    }
                }
            }
            else
            {
                long hasNum = PlayerMgr.Instance.GetItemNum(tShip.cost[0].id);
                if (hasNum >= tShip.cost[0].num)
                {
                    return true;
                }
            }
        }
        
        return false;
    }
}