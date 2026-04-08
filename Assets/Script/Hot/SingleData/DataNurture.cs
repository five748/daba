using System;
using System.Collections.Generic;

[Serializable]
public class DataNurture:ReadAndSaveTool<DataNurture>
{
    private static string _savePath = "nurture";

    public Dictionary<int, NurtureItem> Nurtures = new(); 
    
    public static DataNurture ReadFromFile()
    {
        return ReadByPhone(_savePath, () =>
            {
                var data = new DataNurture();
                //初始化数据
                foreach (var item in TableCache.Instance.equipmentLvupTable)
                {
                    var t = item.Value;
                    var nurture = new NurtureItem
                    {
                        tId = t.id
                    };
                    data.Nurtures.TryAdd(t.id, nurture);
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
public class NurtureItem
{
    public int tId = 0; //配置表id
    public int lv = 0; //解锁默认1级
    public bool unlock = false; //是否解锁
}