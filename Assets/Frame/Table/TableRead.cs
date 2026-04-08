using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableRead : Single<TableRead>
{
    public Dictionary<int, T> ReadTable<T>(string name, bool isUseEditor = false) where T : ITable, new()
    {
        Dictionary<int, T> tableData = new Dictionary<int, T>();
        AssetLoadOld.Instance.LoadTableText(name, (str) =>
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            str = str.Replace(GameSign.ColumnsFlag, GameSign.ColumnsOne);
            string[] lines = str.Split(GameSign.ColumnsChar);
            for (int i = 0; i < lines.Length; i++)
            {
                T t = new T();
                var key = t.Init(lines[i]);
                tableData.Add(key, t);
            }
        }, isUseEditor);
        return tableData;
    }
}
