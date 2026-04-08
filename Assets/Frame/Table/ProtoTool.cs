using System.Collections;
using System.Collections.Generic;
using TreeData;
using UnityEngine;

public static class ProtoTool
{
    public static List<TreeData.item> ToItems(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        str = str.Replace("[", "").Replace("]", "");
        string[] strs = str.Split(',');
        var items = new List<TreeData.item>();
        for (int i = 0; i < strs.Length; i += 2)
        {
            TreeData.item data = new TreeData.item()
            {
                id = int.Parse(strs[i]),
                num = int.Parse(strs[i + 1]),
            };
            items.Add(data);
        }
        return items;
    }
    public static TreeData.item ToItemOne(this string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        str = str.Replace("[", "").Replace("]", "");
        string[] strs = str.Split(',');
        TreeData.item data = new TreeData.item()
        {
            id = int.Parse(strs[0]),
            num = int.Parse(strs[1]),
        };
        return data;
    }
    
    public static List<TreeData.item> ToItems(this int[] arr)
    {
        if (arr == null)
        {
            return new List<item>();
        }
        
        var items = new List<TreeData.item>();
        for (int i = 0; i < arr.Length; i += 2)
        {
            TreeData.item data = new TreeData.item()
            {
                id = arr[i],
                num = arr[i + 1],
            };
            items.Add(data);
        }
        return items;
    }
}
