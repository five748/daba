

using System;
using System.Collections.Generic;
using TreeData;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShipMgr : Singleton<ShipMgr>
{
    public DataShip data { get; }

    public List<int> UnlockShipIds { get; private set; } = new List<int>();
    
    private ShipMgr()
    {
        data = DataShip.ReadFromFile();
        //GameProcess.Instance.SaveEvent += data.SaveToFile;
    }

    public void UnlockShip(int ship_id)
    {
        data.ships[ship_id].unlock = true;
        data.SaveToFile();
        FilterUnlockShip();
    }
    public void ShipLvUp(int shipId) {
        data.ships[shipId].lv++;
        data.SaveToFile();
    }

    public int NextLockShipId()
    {
        var id = data.ships.Count;
        foreach (var item in data.ships)
        {
            var ship = item.Value;
            if (!ship.unlock)
            {
                id = ship.ship_id;
                break;
            }
        }
        return Mathf.Min(id, data.ships.Count);
    }

    //收集已经解锁的船舶
    public void FilterUnlockShip()
    {
        UnlockShipIds.Clear();
        foreach (var item in data.ships)
        {
            var ship = item.Value;
            // 20240515 去除船舶检修与主界面的交互
            // if (ship.ship_id == 13 || ship.ship_id == 14)
            // {
            //     continue;
            // }
            if (ship.unlock)
            {
                UnlockShipIds.Add(ship.ship_id);
            }
        }
    }

    public int GetRandomUnlockShipId()
    {
        if (UnlockShipIds.Count == 0)
        {
            FilterUnlockShip();
        }
        return UnlockShipIds[Random.Range(0, UnlockShipIds.Count)];
    }
    
}