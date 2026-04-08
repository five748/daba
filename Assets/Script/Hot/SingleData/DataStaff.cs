using System;
using System.Collections.Generic;

[Serializable]
public class DataStaff:ReadAndSaveTool<DataStaff>
{
    private static string save_path = "staff";

    public Dictionary<int, StaffItem> staffs = new(); 
    
    public static DataStaff ReadFromFile()
    {
        return ReadByPhone(save_path, () =>
            {
                var data = new DataStaff();
                //初始化数据
                foreach (var item in TableCache.Instance.tollCollectorTable)
                {
                    var t = item.Value;
                    var staff = new StaffItem();
                    staff.staff_id = t.id;
                    staff.unlock = false;
                    SetSumSpeed(staff);
                    data.staffs.TryAdd(t.id, staff);
                }
                
                return data;
            }
        );
    }
    private static void SetSumSpeed(StaffItem data)
    {
        if (data.oneLv == 0) { }
        {
            data.oneLv = 1;
        }
        if (data.twoLv == 0) { }
        {
            data.twoLv = 1;
        }
        if (data.threeLv == 0) { }
        {
            data.threeLv = 1;
        }
        if (data.fourLv == 0) { }
        {
            data.fourLv = 1;
        }
        var tab = TableCache.Instance.trainPropTable;
        data.SumAddSpeed = tab[data.oneLv].prop + tab[data.twoLv].prop + tab[data.threeLv].prop + tab[data.fourLv].prop;
    }

    public void SaveToFile()
    {
        WriteInPhone(this, save_path);
    }
}

[Serializable]
public class StaffItem
{
    public int staff_id = 0;
    public bool unlock = false;
    public int staff_lv = 0;
    public int oneLv;
    public int twoLv;
    public int threeLv;
    public int fourLv;
    public int SumAddSpeed; //过闸速度
    public int moneyspeed;  //收银速度
}