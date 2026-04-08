using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using LitJson;
using System.Reflection;
using GameVersionSpace;

public class UpdateData
{
    public UpdateData(Transform _tran, Action _update)
    {
        tran = _tran;
        update = _update;
    }
    public Transform tran;
    public Action update;
}

public class Launch : SingleMono2<Launch>
{
    static List<UpdateData> DllUIUpdateList = new List<UpdateData>();
    private bool isInited = false;
    public void Init()
    {
        //ILTypeInstance
        if (isInited)
        {
            return;
        }
        hotAllTypes = new Dictionary<string, Type>();
        OnUIs = new Dictionary<string, BaseMonoBehaviour>();
       
        isInited = true;
    }
    public ABOne CreateAbOne(string path)
    {
        var abOne = new ABOne();
        abOne.basePath = path;
        abOne.isIn = true;
        abOne.isNeedUrlDownLoad = false;
        return abOne;
    }

    void Update()
    {
        if (DllUIUpdateList.Count > 0)
        {
            for (int i = 0; i < DllUIUpdateList.Count; i++)
            {
                if (i < DllUIUpdateList.Count && DllUIUpdateList[i].tran != null)
                {
                    DllUIUpdateList[i].update();
                }
            }
            // foreach (var data in DllUIUpdateList)
            // {
            //     if (data.tran != null)
            //         data.update();
            // }
        }
    }
    public static Assembly hotFixAssembly;
    public static Assembly mainAssembly;
    public static Dictionary<string, Type> hotAllTypes;
    public static Dictionary<string, BaseMonoBehaviour> OnUIs = new Dictionary<string, BaseMonoBehaviour>();
    public static bool IsUseHotFix(string tranName)
    {
        return ProssData.Instance.IsUseILRunTime;
    }
    public static BaseMonoBehaviour InitUI(Transform tran, string tranName = "", string prarm = "", System.Action openCallback = null)
    {
        // if (ProssData.Instance.IsUseILRunTime)
        //Debug.LogError("OpenUIName:" + tran.name);
        if (true)
        {
            if (mainAssembly == null)
            {
                mainAssembly = Assembly.GetExecutingAssembly(); // 获取当前程序集 
            }
        }
        else
        {
            if (hotFixAssembly == null)
            {
                // hotFixAssembly = Assembly.Load("HotFix"); // 获取当前程序集 
            }
        }
        if (string.IsNullOrEmpty(tranName))
        {
            tranName = tran.name.Replace("(Clone)", "");
        }
        //Debug.LogError(tranName);
        if (IsUseHotFix(tranName))
        {
            if (hotAllTypes.ContainsKey(tranName))
            {
                //生成实例
                AddOnUI(tranName);
                //调用接口方法
                OnUIs[tranName].BaseInitIL(tran, prarm, openCallback);
                OnUIs[tranName].BaseInit();
                AddUpdate(tran, OnUIs[tranName].Update);
                return OnUIs[tranName];
            }
        }
        else
        {
            AddOnUI(tranName);
            if (OnUIs[tranName] != null)
            {
                OnUIs[tranName].transform = tran;
                OnUIs[tranName].param = prarm;
                OnUIs[tranName].OpenCallBack = openCallback;
                OnUIs[tranName].BaseInit();
            }
            else
            {
                Debug.LogError("UI BaseMonoBehaviour is no exist");
            }
            try
            {
                AddUpdate(tran, OnUIs[tranName].Update);
            }
            catch
            {
                UnityEngine.Debug.LogError("打开界面失败:" + tranName);
            }
            return OnUIs[tranName];
        }
        return null;
    }
  
    private static void AddOnUI(string tranName)
    {
        List<string> keys = new List<string>(OnUIs.Keys);
        for (int i = 0; i < keys.Count; i++)
        {
            if (OnUIs[keys[i]] == null)
            {
                OnUIs.Remove(keys[i]);
                continue;
            }
            if (OnUIs[keys[i]].transform == null)
            {
                OnUIs[keys[i]].Destory();
                OnUIs.Remove(keys[i]);
                OnUIs[keys[i]] = null;
            }
        }
        object obj = hotFixAssembly?.CreateInstance(tranName);
        if (obj == null)
        {
            obj = mainAssembly.CreateInstance(tranName);
        }
        BaseMonoBehaviour ui = (BaseMonoBehaviour)obj;
        if (!OnUIs.ContainsKey(tranName))
        {
            OnUIs.Add(tranName, ui);
        }
        else
        {
            OnUIs[tranName] = ui;
        }
    }
    private static void AddUpdate(Transform tran, System.Action update)
    {
        DllUIUpdateList.Add(new UpdateData(tran, update));
        List<UpdateData> needDels = new List<UpdateData>();
        int index = -1;
        foreach (var item in DllUIUpdateList)
        {
            index++;
            if (item.tran == null)
            {
                needDels.Add(item);
            }
        }
        foreach (var data in needDels)
        {
            DllUIUpdateList.Remove(data);
        }
    }
    public static void CloseUI(bool isTip, GameObject go, string name, string str, bool isUseCloseBack = false, System.Action callback = null)
    {
        if (isUseCloseBack)
            OnUIs[name].CloseBack(str);
        if (OnUIs.ContainsKey(name))
            OnUIs[name].Destory();
        OnUIs.Remove(name);
        HotMgr.Instance.DeUnLoadUnUse(name);
        //Resources.UnloadUnusedAssets();
        if (name == "UIReward" || name == "UIShop" || !isTip)
        {
            if (callback != null)
            {
                callback();
            }
            GameObject.Destroy(go);
        }
        else {
            go.transform.TipBigAndSmallShowClose(() =>
            {
                if (callback != null)
                {
                    callback();
                }
                if (go)
                {
                    GameObject.Destroy(go);
                }
            });
        }
    }
    public static void CloseAllUI()
    {
        DllUIUpdateList.Clear();
        OnUIs.Clear();
    }
    public static BaseMonoBehaviour GetComponent(Transform tran)
    {
        return OnUIs[tran.name];
    }
    private void RegistDelegate()
    {
        
    }
}