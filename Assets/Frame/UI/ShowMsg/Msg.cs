using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public struct WarnOne
{
    public bool isText;
    public string msc;
    public string iconPath;
    public int id;
    public int num;
    public Transform father;
    public System.Action callback;
    public WarnOne(string _msc)
    {
        isText = true;
        msc = _msc;
        id = 0;
        num = 0;
        iconPath = "";
        father = null;
        callback = null;
    }
    public WarnOne(string _msc, Transform _father, System.Action _callback)
    {
        isText = false;
        msc = _msc;
        id = 0;
        num = 0;
        iconPath = "";
        father = _father;
        callback = _callback;
    }
    public WarnOne(int _id, int _num, string _iconPath, Transform _father = null, System.Action _callback = null)
    {
        isText = false;
        msc = "+" +_num.ToString();
        id = _id;
        num = _num;
        iconPath = _iconPath;
        father = _father;
        callback = _callback;
    }
}
public class Msg : MonoBehaviour
{
    public static Msg Instance;
    private MsgOne WarnText;
    private MsgOne WarnItem;
    private MsgOne WarnPower;
    private bool isShowing;

    private List<WarnOne> allNeedShows;
    public void Start()
    {
        Instance = this;
        DontDestroyOnLoad(transform.gameObject);
        WarnText = new MsgOne(transform.GetChild(0));
        if(transform.childCount > 1)
            WarnItem = new MsgOne(transform.GetChild(1));
        //WarnPower = new MsgOne(transform.GetChild(5));
        allNeedShows = new List<WarnOne>();
    }
    public void ShowPower(string msc)
    {
#if UNITY_EDITOR
        Debug.Log(msc);
#endif
        msc = msc.ChangeTextStr();
        if (!allNeedShows.Contains(new WarnOne(msc)))
        {
            allNeedShows.Add(new WarnOne(msc));
        }
    }
    public void Show(string msc)
    {
#if UNITY_EDITOR
        Debug.Log(msc);
#endif
        msc = msc.ChangeTextStr();
        allNeedShows.Add(new WarnOne(msc));
    }
    public void Show1(string msc, object a)
    {
#if UNITY_EDITOR
        Debug.Log(msc);
#endif
        msc = msc.ChangeTextStr1(a);
        allNeedShows.Add(new WarnOne(msc));
    }
    public void Show2(string msc, object a, object b)
    {
#if UNITY_EDITOR
        Debug.Log(msc);
#endif
        msc = msc.ChangeTextStr2(a, b);
        allNeedShows.Add(new WarnOne(msc));
    }
    public void ShowItem(int id, int num, string iconPath, Transform father, System.Action callback = null)
    {
        allNeedShows.Add(new WarnOne(id, num, iconPath, father, callback));
    }
    public void Show(string msc, Transform father, System.Action callback)
    {
#if UNITY_EDITOR
        Debug.Log(msc);
#endif
        allNeedShows.Add(new WarnOne(msc, father, callback));
    }
    int sum = 30;
    void Update()
    {
        if (allNeedShows == null)
        {
            return;
        }
        if (allNeedShows.Count == 0)
        {
            sum = 30;
            return;
        }
        sum++;
        if (sum < 30)
        {
            return;
        }
        sum = 0;
        var item = allNeedShows[0];
        allNeedShows.RemoveAt(0);
        MsgOne choose = null;
        if (item.isText)
        {
            choose = WarnText;
        }
        else
        {
            choose = WarnItem;
        }
        if(item.msc.Contains("**********"))
        {
            choose = WarnPower;
        }
        isShowing = true;
        choose.Show(item, 60, true, () =>
        {
            isShowing = false;
        });
    }
    public void ShowHp(Transform tran, float hp) { 
    
    }
}
