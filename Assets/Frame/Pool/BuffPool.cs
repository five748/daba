using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuffPool<T>
{
    List<T> lst = new List<T>();
    List<int> freeIds = new List<int>();
    public delegate T CreateHandle();
    private CreateHandle CreateEvent;
    private System.Action<T> FreeEvent;
    private System.Action<T> getOnEvent;
    public BuffPool(CreateHandle create, System.Action<T> getone, System.Action<T> free)
    {
        lst.Clear();
        CreateEvent = create;
        getOnEvent = getone;
        FreeEvent = free;
    }
    int adddindex = -1;
    public T GetOne()
    {
        if (freeIds.Count == 0)
        {
            T t = CreateEvent();
            //Debug.LogError("create:" + lst.Count);
            lst.Add(t);
            getOnEvent(t);
            return t;
        }
        int one = freeIds[0];
        freeIds.RemoveAt(0);
        if (lst[one] == null) {
            return GetOne();
        }
        getOnEvent(lst[one]);
        return lst[one];
    }
    int index;
    public void Free(T t)
    {
        //Debug.Log(t);
        index = lst.IndexOf(t);
        //Debug.LogError("free:" + index);
        if (index == -1)
        {
            Debug.LogError("缓存池释放失败!");
            return;
        }
        if (index != -1)
        {
            freeIds.Add(index);
            FreeEvent(t);
        }
    }
    public void FreeAll()
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (!freeIds.Contains(i))
            {
                Free(lst[i]);
            }
        }
        lst.Clear();
        freeIds.Clear();
    }
    public void ForEach(System.Action<T> callback)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (freeIds.Contains(i))
            {
                continue;
            }
            callback(lst[i]);
        }
    }
}



















































