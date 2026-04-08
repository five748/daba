using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;

public static class ListTool
{  
    /// <summary>
     /// 随机排序
     /// </summary>
    public static List<T> RandomSortList<T>(this List<T> list)
    {
        System.Random random = new System.Random();
        List<T> nList = new List<T>();
        foreach (T temp in list)
        {
            nList.Insert(random.Next(nList.Count + 1), temp);
        }
        return nList;
    }
    public static T Remove<T>(this Queue<T> queue)
    {
        return queue.Dequeue();
    }
    public static void Add<T>(this Queue<T> queue, T t)
    {
        queue.Enqueue(t);
    }
    public static void Foreach<T>(this List<T> list, System.Action<T> callback)
    {
        for (int i = 0; i < list.Count; i++)
        {
            callback(list[i]);
        }
    }
    public static void ForeachBack<T>(this List<T> list, System.Action<T> callback)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            callback(list[i]);
        }
    }
    public static void Foreach<T>(this List<T> list, System.Action<int, T> callback)
    {
        if (list == null)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            callback(i, list[i]);
        }
    }
    public static List<T> Backward<T>(this List<T> list)
    {
        var newList = new List<T>();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            newList.Add(list[i]);
        }
        return newList;
    }
    public static void Foreach<T>(this T[] list, System.Action<int, T> callback)
    {
        for (int i = 0; i < list.Length; i++)
        {
            callback(i, list[i]);
        }
    }
    public static void For<T>(this T[] array, System.Action<T> callback)
    {
        for (int i = 0; i < array.Length; i++)
        {
            callback(array[i]);
        }
    }
    public static void For<T>(this T[] array, System.Action<int, T> callback)
    {
        for (int i = 0; i < array.Length; i++)
        {
            callback(i, array[i]);
        }
    }
    public static void ForByKey<T, V>(this Dictionary<T, V> dic, System.Action<T, V> callback)
    {
        if (dic == null) {
            return;
        }
        var keys = new List<T>(dic.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            callback(keys[i], dic[keys[i]]);
        }
    }
    public static void ForeachFor<T, V>(this Dictionary<T, V> dic, System.Action<T, V, int> callback)
    {
        int index = -1;
        foreach (var item in dic)
        {
            index++;
            callback(item.Key, item.Value, index);
        }
    }
    public static void ForeachForAddItems<T, V>(this Dictionary<T, V> dic, Transform grid, System.Action<T, V, int, Transform> callback)
    {
        int index = -1;
        var items = grid.AddChilds(dic.Count);
        foreach (var item in dic)
        {
            index++;
            callback(item.Key, item.Value, index, items[index]);
        }
    }

    public static void ForeachFor<T, V>(this Dictionary<T, V> dic, System.Action<KeyValuePair<T, V>, int> callback)
    {
        int index = -1;
        foreach (var item in dic)
        {
            index++;
            callback(item, index);
        }
    }
    public static V GetOne<T, V>(this Dictionary<T, V> dic, int index)
    {
        if (index >= dic.Count)
        {
            return default(V);
        }
        int i = -1;
        foreach (var item in dic)
        {
            i++;
            if (index == i)
            {
                return item.Value;
            }
        }
        return default(V);
    }
    public static Dictionary<T, V> Sort<T, V>(this Dictionary<T, V> datas, System.Comparison<KeyValuePair<T, V>> CompareTo)
    {
        List<KeyValuePair<T, V>> lst = new List<KeyValuePair<T, V>>(datas);
        lst.Sort(CompareTo);
        Dictionary<T, V> cache = new Dictionary<T, V>();
        for (int i = 0; i < lst.Count; i++)
        {
            cache.Add(lst[i].Key, datas[lst[i].Key]);
        }
        datas = cache;
        return cache;
    }
    public static Dictionary<T, V> SortByBeginIndex<T, V>(this Dictionary<T, V> datas, System.Comparison<KeyValuePair<T, V>> CompareTo, int beginIndex)
    {
        Dictionary<T, V> cache = new Dictionary<T, V>();
        List<KeyValuePair<T, V>> lst = new List<KeyValuePair<T, V>>(datas);
        var newLst = new List<KeyValuePair<T, V>>();
        for (int i = 0; i < beginIndex; i++)
        {
            cache.Add(lst[0].Key, datas[lst[0].Key]);
            lst.RemoveAt(0);
        }
        lst.Sort(CompareTo);
     
        for (int i = 0; i < lst.Count; i++)
        {
            cache.Add(lst[i].Key, datas[lst[i].Key]);
        }
        datas = cache;
        return cache;
    }
    public delegate int Compare<T>(T t);
    public delegate bool CompareIsHave<T>(T t);
    public static Dictionary<T, V> SortFirstMiddeDown<T, V>(this Dictionary<T, V> datas, Compare<KeyValuePair<T, V>> compare)
    {
        List<T> firsts = new List<T>();
        List<T> middles = new List<T>();
        List<T> lasts = new List<T>();
        List<T> lasts1 = new List<T>();
        foreach (var item in datas)
        {
            if (compare(item) == -1)
            {
                firsts.Add(item.Key);
            }
            if (compare(item) == 0)
            {
                middles.Add(item.Key);
            }
            if (compare(item) == 1)
            {
                lasts.Add(item.Key);
            }
            if (compare(item) == 2)
            {
                lasts1.Add(item.Key);
            }
        }
        var newData =new Dictionary<T, V>();
        for (int i = 0; i < firsts.Count; i++)
        {
            newData.Add(firsts[i], datas[firsts[i]]);
        }
        for (int i = 0; i < middles.Count; i++)
        {
            newData.Add(middles[i], datas[middles[i]]);
        }
        for (int i = 0; i < lasts.Count; i++)
        {
            newData.Add(lasts[i], datas[lasts[i]]);
        }
        for (int i = 0; i < lasts1.Count; i++)
        {
            newData.Add(lasts1[i], datas[lasts1[i]]);
        }
        datas.Clear();
        foreach (var item in newData)
        {
            datas.Add(item.Key, item.Value);
        }
        return newData;
    }
    public static List<T> SortFirstMiddeDown<T>(this List<T> datas, Compare<T> compare)
    {
        List<int> firsts = new List<int>();
        List<int> middles = new List<int>();
        List<int> lasts = new List<int>();
        List<int> lasts1 = new List<int>();
        int index = -1;
        foreach (var item in datas)
        {
            index++;
            if (compare(item) == -1)
            {
                firsts.Add(index);
            }
            if (compare(item) == 0)
            {
                middles.Add(index);
            }
            if (compare(item) == 1)
            {
                lasts.Add(index);
            }
            if (compare(item) == 2)
            {
                lasts1.Add(index);
            }
        }
        var newData = new List<T>();
        for (int i = 0; i < firsts.Count; i++)
        {
            newData.Add(datas[firsts[i]]);
        }
        for (int i = 0; i < middles.Count; i++)
        {
            newData.Add(datas[middles[i]]);
        }
        for (int i = 0; i < lasts.Count; i++)
        {
            newData.Add(datas[lasts[i]]);
        }
        for (int i = 0; i < lasts1.Count; i++)
        {
            newData.Add(datas[lasts1[i]]);
        }
        datas.Clear();
        foreach (var item in newData)
        {
            datas.Add(item);
        }
        return newData;
    }

    public static void Sort<T>(this T[] datas, System.Comparison<T> comparer)
    {
        List<T> newdatas = new List<T>();
        foreach (var item in datas)
        {
            newdatas.Add(item);
        }
        newdatas.Sort(comparer);
        for (int i = 0; i < newdatas.Count; i++)
        {
            datas[i] = newdatas[i];
        }
    }
    public static Sprite[] SortImage(this Sprite[] datas)
    {
        var newData = new Sprite[datas.Length];
        Dictionary<string, Sprite> dic = new Dictionary<string, Sprite>();
        for (int i = 0; i < datas.Length; i++)
        {
            dic.Add(datas[i].name, datas[i]);
        }
        for (int i = 0; i < datas.Length; i++)
        {
            newData[i] = dic[i.ToString()];
        }
        return newData;
    }
    public static void DelAll<T, V>(this Dictionary<T, V> dic)
    {
        if (dic == null || dic.Count == 0)
        {
            return;
        }
        T[] ts = new T[dic.Count];
        for (int i = 0; i < ts.Length; i++)
        {
            dic.Remove(ts[i]);
        }
    }
    public static void DelKes<T, V>(this Dictionary<T, V> dic, List<T> keys)
    {
        for (int i = 0; i < keys.Count; i++)
        {
            dic.Remove(keys[i]);
        }
    }
    public static List<T> GetNoHavesIn<T>(this T[] a, T[] b)
    {
        List<T> diffs = new List<T>();
        List<T> lsta = new List<T>();
        for (int i = 0; i < a.Length; i++)
        {
            lsta.Add(a[i]);
        }
        for (int i = 0; i < b.Length; i++)
        {
            if (!lsta.Contains(b[i]))
            {
                diffs.Add(b[i]);
            }
        }
        return diffs;
    }

    public static int Sum<T, T2>(this Dictionary<T2, T> dic, Func<T, int> func)
    {
        int sum = 0;
        foreach (var item in dic)
        {
            sum += func(item.Value);
        }
        return sum;
    }
    public static int Max<T, T2>(this Dictionary<T2, T> dic, Func<T, int> func)
    {
        int sum = 0;
        foreach (var item in dic)
        {
            var temp = func(item.Value);
            if (sum < temp)
            {
                sum = temp;
            }
        }
        return sum;
    }
    public static int Sum<T>(this List<T> dic, Func<T, int> func)
    {
        int sum = 0;
        foreach (var item in dic)
        {
            sum += func(item);
        }
        return sum;
    }

    public static int Sum<T, T2>(this Dictionary<T2, T> dic, Func<T, T2, int> func)
    {
        int sum = 0;
        foreach (var item in dic)
        {
            sum += func(item.Value, item.Key);
        }
        return sum;
    }

    public static Dictionary<string, T> ChangeKeyToString<T>(this Dictionary<int, T> datas)
    {
        var newDatas = new Dictionary<string, T>();
        foreach (var item in datas)
        {
            newDatas.Add(item.Key.ToString(), item.Value);
        }
        return newDatas;
    }
    public static Dictionary<string, T> ChangeLongKeyToString<T>(this Dictionary<long, T> datas)
    {
        var newDatas = new Dictionary<string, T>();
        foreach (var item in datas)
        {
            newDatas.Add(item.Key.ToString(), item.Value);
        }
        return newDatas;
    }
    public static Dictionary<int, T> ChangeKeyToInt<T>(this Dictionary<string, T> datas)
    {
        var newDatas = new Dictionary<int, T>();
        foreach (var item in datas)
        {
            newDatas.Add(int.Parse(item.Key), item.Value);
        }
        return newDatas;
    }
    public static Dictionary<long, T> ChangeKeyToLong<T>(this Dictionary<string, T> datas)
    {
        var newDatas = new Dictionary<long, T>();
        foreach (var item in datas)
        {
            newDatas.Add(long.Parse(item.Key), item.Value);
        }
        return newDatas;
    }

    public static Dictionary<string, T> Uniq<T>(this Dictionary<string, T> datas, Func<T, string> func)
    {
        var newDatas = new Dictionary<string, T>();
        foreach (var item in datas)
        {
            var key = func(item.Value);
            if (!newDatas.ContainsKey(key))
            {
                newDatas.Add(key, item.Value);

            }
        }
        return newDatas;
    }
    public static List<T> ToList<T>(this Dictionary<string, T> datas)
    {
        var newDatas = new List<T>();
        foreach (var item in datas)
        {
            newDatas.Add(item.Value);
        }
        return newDatas;
    }
    
    public static List<T2> ToValList<T1,T2>(this Dictionary<T1, T2> datas)
    {
        var newDatas = new List<T2>();
        foreach (var item in datas)
        {
            newDatas.Add(item.Value);
        }
        return newDatas;
    }
    
    
    public static List<T> DicToKeyList<T, T1>(this Dictionary<T, T1> datas, Func<T1, bool> func = null)
    {
        var newDatas = new List<T>();
        foreach (var item in datas)
        {
            if (func == null)
            {
                newDatas.Add(item.Key);
            }
            else if (func(item.Value))
            {
                newDatas.Add(item.Key);
            }
        }
        return newDatas;
    }

    public static List<int> ToKeyList<T>(this Dictionary<int, T> datas, System.Func<T, bool> func = null)
    {
        var newDatas = new List<int>(datas.Keys);
        if (func == null)
        {
            return newDatas;
        }
        var keys = new List<int>();
        for (int i = 0; i < newDatas.Count; i++)
        {
            if (func(datas[newDatas[i]]))
            {
                keys.Add(newDatas[i]);
            }
        }
        return keys;
    }
    public static bool IsExist<T>(this List<T> list, System.Func<T, bool> func)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (func(list[i]))
            {
                return true;
            }
        }

        return false;
    }

    public static T TryGetValue<T1, T2, T>(this Dictionary<T1, T2> datas, T1 key, Func<T2, T> func = null)
    {
        T2 times = default(T2);
        if (datas.TryGetValue(key, out times))
        {
            return func(times);
        }

        return default(T);
    }
    public static T First<T1, T>(this Dictionary<T1, T> datas, Func<T, bool> func)
    {
        foreach (var item in datas)
        {
            if (func(item.Value))
            {
                return item.Value;
            }
        }

        return default(T);
    }
    public static Dictionary<T, T1> ChangeValueToKey<T1, T>(this Dictionary<T1, T> datas)
    {
        Dictionary<T, T1> newdic = new Dictionary<T, T1>();
        foreach (var item in datas)
        {
            if (newdic.ContainsKey(item.Value))
            {
                Debug.LogError("有相同键值");
                return new Dictionary<T, T1>();
            }
            newdic.Add(item.Value, item.Key);
        }
        return newdic;
    }
    public static T GetByIndex<T1, T>(this Dictionary<T1, T> datas, int index)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("no have first");
            return default(T);
        }
        int index1 = -1;
        foreach (var item in datas)
        {
            index1++;
            if (index1 == index) { 
                return item.Value;
            }
        }
        return default(T);
    }
    public static T GetFirst<T1, T>(this Dictionary<T1, T> datas)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("no have first");
            return default(T);
        }
        return datas.First().Value;
    }
    public static T1 GetFirstKey<T1, T>(this Dictionary<T1, T> datas)
    {
        if (datas == null || datas.Count == 0)
        {
            throw new Exception("no have first");
        }
        return datas.First().Key;
    }
    public static T GetLast<T1, T>(this Dictionary<T1, T> datas)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("no have first");
            return default(T);
        }
        return datas.Last().Value;
    }
    public static T GetMin<T1, T>(this Dictionary<T1, T> datas, Func<T, int> func)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("no have first");
            return default(T);
        }
        datas = datas.Sort((a, b) =>
        {
            var one = func(a.Value);
            var two = func(b.Value);
            return one.CompareTo(two);
        });
        return datas.First().Value;
    }
    public static T GetMax<T1, T>(this Dictionary<T1, T> datas, Func<T, int> func)
    {
        if (datas == null || datas.Count == 0)
        {
            Debug.Log("no have first");
            return default(T);
        }
        datas = datas.Sort((a, b) =>
        {
            var one = func(a.Value);
            var two = func(b.Value);
            return -one.CompareTo(two);
        });
        return datas.First().Value;
    }
    public static List<T1> GetKeys<T1, T>(this Dictionary<T1, T> datas)
    {
        if (datas == null)
        {
            return null;
        }
        return new List<T1>(datas.Keys);
    }
    public static List<T> GeValues<T1, T>(this Dictionary<T1, T> datas)
    {
        if (datas == null)
        {
            return null;
        }
        return new List<T>(datas.Values);
    }

    public delegate T1 LstToDicBack<T1, T2>(T2 t);
    public static Dictionary<T1, T2> LstToDic<T1, T2>(this List<T2> lst, LstToDicBack<T1, T2> callback)
    {
        Dictionary<T1, T2> dic = new Dictionary<T1, T2>();
        foreach (var item in lst)
        {
            dic.Add(callback(item), item);
        }
        return dic;
    }

    public static void Foreach<T>(System.Action<T> calback)
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            calback(value);
        }
    }
    public static void ForeachEnmu<T>(System.Action<T> calback)
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            calback(value);
        }
    }
    public static bool IsSame<T>(this List<T> a, List<T> b)
    {
        if (a.Count != b.Count)
        {
            return false;
        }
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].Equals(b[i]))
            {
                return false;
            }
        }
        return true;
    }
    public static string LstToStrBySpilt(this List<string> lst, string chat)
    {
        var str = "";
        for (int i = 0; i < lst.Count; i++)
        {
            str += lst[i] + chat;
        }
        return str.CutLast();
    }
    public static List<Vector2> RemoveFirstSavePos(this List<Vector2> poses)
    {
        if (poses.Count < 2)
        {
            poses.RemoveAt(0);
            return poses;
        }
        Vector2 oldpos = poses[0];
        poses.RemoveAt(0);
        if (poses[0].x == oldpos.x && poses[0].y == oldpos.y)
        {
            poses.RemoveAt(0);
        }
        return poses;
    }
    public static void FunNearIndex(this int target, int len, System.Func<int, bool> AddOver, System.Func<int, bool> CutOver)
    {
        bool isAdd = true;
        int add = target;
        int cut = target - 1;
        bool isAddOver = false;
        bool isCutOver = false;
        while (true)
        {
            if (isAdd)
            {
                if (AddOver(add))
                {
                    isAddOver = true;
                }
                add++;
                len--;
            }
            else
            {
                if (CutOver(cut))
                {
                    isCutOver = true;
                }
                cut--;
                len--;
            }
            if (len == 0)
            {
                return;
            }
            isAdd = !isAdd;
            if (isAddOver)
            {
                isAdd = false;
            }
            if (isCutOver)
            {
                isAdd = true;
            }
            if (isAddOver && isCutOver)
            {
                return;
            }
        }
    }
    public static void Foreach<T>(this T[,] datas, System.Action<T, int, int> callback) {
        int len0 = datas.GetLength(0);
        int len1 = datas.GetLength(1);
        for (int i = 0; i < len0; i++)
        {
            for (int j = 0; j < len1; j++)
            {
                callback(datas[i, j], i, j);
            }
        }
    }
    
    public static List<T> OneDimensionArrayStringToList<T>(this string value)
    {
        string str_temp = value.Substring(1, value.Length - 2);
        string[] arr = str_temp.Split(",");
        List<T> list = new List<T>();
        for (int i = 0; i < arr.Length; i++)
        {
            string str = arr[i];
            T result = default(T);
            result = (T)Convert.ChangeType(str, typeof(T));
            list.Add(result);
        }

        return list;
    }
    public static T[] StrToArray<T>(this string value)
    {
        string str_temp = value.Substring(1, value.Length - 2);
        string[] arr = str_temp.Split(",");
        T[] arrays = new T[arr.Length];
        for (int i = 0; i < arr.Length; i++)
        {
            string str = arr[i];
            T result = default(T);
            result = (T)Convert.ChangeType(str, typeof(T));
            arrays[i] = result;
        }
        return arrays;
    }

    //字符串转2维数组
    public static  T[][] TwoStringToArray<T>(this string value)
    {
        if (value == "[]") { 
            return new T[0][];
        }
        string str_temp = value.Substring(2, value.Length - 4);
        string[] arr = str_temp.Split("],[");
        T[][] list = new T[arr.Length][];
        
        for (int i = 0; i < arr.Length; i++)
        {
            string[] arr2 = arr[i].Split(",");
            T[] list2 = new T[arr2.Length];
            for (int j = 0; j < arr2.Length; j++)
            {
                var s = arr2[j];
                T result = default(T);
                list2[j] = (T)Convert.ChangeType(s, typeof(T));
            }
            list[i] = list2;
        }
        return list;
    }
    public static Dictionary<int, int> ToDicIntInt(this string str)
    {
        var dic = new Dictionary<int, int>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToIntArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            if (dic.ContainsKey(strs[i])) {
                Debug.LogError("配置出错Key:" + strs[i]);
                continue;
            }
            dic.Add(strs[i], strs[i + 1]);
        }
        return dic;
    }
    public static Dictionary<int, string> ToDicIntString(this string str)
    {
        var dic = new Dictionary<int, string>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(int.Parse(strs[i]), strs[i + 1]);
        }
        return dic;
    }
    public static Dictionary<int, float> ToDicIntFloat(this string str)
    {
        var dic = new Dictionary<int, float>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(int.Parse(strs[i]), float.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<int, long> ToDicIntLong(this string str)
    {
        var dic = new Dictionary<int, long>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(int.Parse(strs[i]), long.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<string, string> ToDicStringString(this string str)
    {
        var dic = new Dictionary<string, string>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(strs[i], strs[i + 1]);
        }
        return dic;
    }
    public static Dictionary<string, int> ToDicStringInt(this string str)
    {
        var dic = new Dictionary<string, int>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(strs[i], int.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<string, float> ToDicStringFloat(this string str)
    {
        var dic = new Dictionary<string, float>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(strs[i], float.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<string, long> ToDicStringLong(this string str)
    {
        var dic = new Dictionary<string, long>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(strs[i], long.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<long, long> ToDicLongLong(this string str)
    {
        var dic = new Dictionary<long, long>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(long.Parse(strs[i]), long.Parse(strs[i + 1]));
        }
        return dic;
    }
    public static Dictionary<long, string> ToDicLongString(this string str)
    {
        var dic = new Dictionary<long, string>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(long.Parse(strs[i]), strs[i + 1]);
        }
        return dic;
    }
    public static Dictionary<long, float> ToDicLongFloat(this string str)
    {
        var dic = new Dictionary<long, float>();
        str = str.Substring(1, str.Length - 2);
        var strs = str.SplitToStringArray(',');
        for (int i = 0; i < strs.Length; i += 2)
        {
            dic.Add(long.Parse(strs[i]),float.Parse( strs[i + 1]));
        }
        return dic;
    }
    public static void Stop(this List<Coroutine> cos) {
        if (cos == null) {
            return;
        }
        for (int i = 0; i < cos.Count; i++)
        {
            cos[i].Stop();
        }
        cos.Clear();
    }
    public static string[] SplitToStringArray(this string str, char c)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        str = str.Replace("[", "").Replace("]", "");
        return str.Split(c);
    }
    public static int[] SplitToIntArray(this string str, char c)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        if (str == "[]") {
            return null;
        }
        str = str.Replace("[", "").Replace("]", "");
        string[] strs = str.Split(c);
        int[] arrays = new int[strs.Length];
        for (int i = 0; i < strs.Length; i++)
        {
            arrays[i] = int.Parse(strs[i]);
        }
        return arrays;
    }
    
    public static float[] SplitToFloatArray(this string str, char c)
    {
        if (string.IsNullOrEmpty(str))
        {
            return null;
        }
        str = str.Replace("[", "").Replace("]", "");
        string[] strs = str.Split(c);
        float[] arrays = new float[strs.Length];
        for (int i = 0; i < strs.Length; i++)
        {
            arrays[i] = float.Parse(strs[i]);
        }
        return arrays;
    }
    public static void For<T>(this T[,] datas, System.Action<T, int, int> callback) {
        int yLen = datas.GetLength(0);
        int xLen = datas.GetLength(1);
        for (int x = 0; x < xLen; x++)
        {
            for (int y = 0; y < yLen; y++)
            {
                callback(datas[x, y], x, y);
            }
        }
    }
    public static void Del<T>(this T[,] datas, List<Vec2Int> keys) {
        for (int i = 0; i < keys.Count; i++)
        {
            //Debug.LogError(default(T));
            datas[keys[i].y, keys[i].x] = default(T);
        }
    }
    public static int GetRandomLst(this int[] datas)
    {
        var index = UnityEngine.Random.Range(0, datas.Length);
        return datas[index];
    }
    public static int GetRandomLst(this List<int> datas)
    {
        var index = UnityEngine.Random.Range(0, datas.Count);
        return datas[index];
    }
    public static int GetRandom(this int[] datas) {
        return UnityEngine.Random.Range(datas[0], datas[1]);
    }
    public static float GetRandom(this float[] datas)
    {
        return UnityEngine.Random.Range(datas[0], datas[1]);
    }
    public static bool IsSameSortOverLst(this List<int> a, List<int> b) {
        if (a.Count != b.Count) {
            return false;
        }
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i])
            {
                return false; 
            }
        }
        return true;
    }
    //public static void AddKeyValue<K, T>(this Dictionary<K, T> datas, K k, T v) where T : new()
    //{
    //    if (!datas.ContainsKey(k))
    //    {
    //        datas.Add(k, v);
    //    }
    //    else
    //    {
    //        datas[k] = v;
    //    }
    //}

}





