using System;
using System.Collections.Generic;
using System.Data;
using Table;

[Serializable]
public class DataNavigate:ReadAndSaveTool<DataNavigate>
{
    private static string _savePath = "navigate";

    public int capacityLv = 1;
    public int outputLv = 1;
    public int capacity = 0;
    public float output = 0;
    public float cargoCount = 0;
    public long fullCargoTime = 0;
    public Dictionary<int, CargoShipItem> ships;
    
    public static DataNavigate ReadFromFile()
    {
        var data = ReadByPhone(_savePath, () =>
            {
                DataNavigate data = new ()
                {
                    ships = new (),
                };

                var tData = TableCache.Instance.warehouseTable[1];
                data.capacity = tData.capacity;
                data.output = tData.output;

                var tCargoShips = TableCache.Instance.cargoShipTable;
                foreach (var (_, ship) in tCargoShips)
                {

                    CargoShipItem cargoShipItem = new ()
                    {
                        tId = ship.id
                    };

                    data.ships.TryAdd(ship.id, cargoShipItem);
                }
                
                return data;
            }
        );

        foreach (var (_, ship) in data.ships)
        {
            if (ship.lv > 0 && ship.sendTime == 0 && ship.cargoCount == 0)
            {
                if (ship.tOrderId == int.MinValue)
                {
                    var orderId = UnityEngine.Random.Range(1, TableCache.Instance.orderTable.Count + 1);
                    ship.tOrderId = orderId;
                }
            }
        }
        
        return data;
    }

    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }
}

[Serializable]
public class CargoShipItem
{
    public int tId = 0; //配置表id
    public int lv = 0; //等级 0级表示未解锁 解锁默认1级
    //组合 用于判定是不是在集装箱装货中
    public long sendTime = 0; //送达时间
    public int cargoCount = 0; //运送的货物量
    public int tOrderId = int.MinValue; //分配的订单id
    [LitJson.JsonIgnore]
    public int Capacity
    {
        get
        {
            var tShip = TableCache.Instance.cargoShipTable[tId];
            return tShip.capacity + tShip.capacityLvup * lv;
        }
    }
    [LitJson.JsonIgnore]
    public int Speed
    {
        get
        {
            var tShip = TableCache.Instance.cargoShipTable[tId];
            return tShip.speed;
        }
    }
}