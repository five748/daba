using System;
using System.Collections.Generic;

[Serializable]
public class DataShip:ReadAndSaveTool<DataShip>
{
    private static string save_path = "ship";

    public Dictionary<int, ShipItem> ships = new(); 
    
    public static DataShip ReadFromFile()
    {
        return ReadByPhone(save_path, () =>
            {
                var data = new DataShip();
                //初始化数据
                foreach (var item in TableCache.Instance.shipTable)
                {
                    var t = item.Value;
                    var ship = new ShipItem();
                    ship.ship_id = t.id;
                    ship.unlock = t.id == 1;
                    data.ships.TryAdd(t.id, ship);
                    if (ship.unlock)
                    {
                        MTaskData.Instance.AddTaskNum(MainTaskMenu.OpenSomeOneShip, 1, ship.ship_id);
                    }
                }
                return data;
            }
        );
    }
    
    public void SaveToFile()
    {
        WriteInPhone(this, save_path);
    }
}

[Serializable]
public class ShipItem
{
    public int ship_id = 0;
    public bool unlock = false;
    public int lv;
    public int cutPassTime;    //过闸减少时间
    public int preLockageTime; //过闸时间减少百分比
}