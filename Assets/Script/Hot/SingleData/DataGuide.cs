using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class DataGuide : ReadAndSaveTool<DataGuide>
{
    //存储地址
    private static string _savePath = "guide";
    
    //读取
    public static DataGuide ReadFromFile(Action call)
    {
        var data = ReadByPhone(_savePath, () =>
            {
                var data = new DataGuide();
                data.Init();
                return data;
            }
        );

        return data;
    }

    //保存
    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }

    #region data

    public int startIndex = -1;//当前这组引导的头一个下标
    public int index = -1;//当前做到的引导的下标
    
    public bool interrupt = false;//引导是否中断 决定了进入游戏的时候是否开启引导

    public List<int> listEnd = new List<int>();//已经完结的强制引导列表
    public List<int> listNotEnd = new List<int>();//还没完结的强制引导列表
    public Dictionary<int, int> DicSoftGuideCount = new Dictionary<int, int>();//软引导的次数统计

    public int guideBottomType = 0;//底部引导显示

    #endregion data

    #region init

    public DataGuide()
    {
       
        
    }
    private void Init() {
        //强制引导
        {
            var tables = TableCache.Instance.HardGuideTable;
            int group = 0;
            foreach (var table in tables)
            {
                if (group != table.Value.group)
                {
                    listNotEnd.Add(table.Value.id);
                    group = table.Value.group;
                }
            }

            listNotEnd.Sort((a, b) =>
            {
                var tableA = TableCache.Instance.HardGuideTable[a];
                var tableB = TableCache.Instance.HardGuideTable[b];

                return Math.Abs(tableA.group).CompareTo(Math.Abs(tableB.group));
            });
            Debug.Log("------为引导数量:" + listNotEnd.Count);
            Debug.Log("------引导表数据:" + tables.Count);
        }

        //软引导
        {
            var tables = TableCache.Instance.SoftGuideTable;
            foreach (var table in tables)
            {
                DicSoftGuideCount.Add(table.Value.id, table.Value.times);
            }
        }
    }
    #endregion init
}

/// <summary>
/// 引导底部按钮操作类型
/// </summary>
public enum EnumGuideBottomType
{
    eNone,
}