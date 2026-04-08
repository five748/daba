using System;
using System.Collections.Generic;
using System.Reflection;
using Table;
using UnityEditor;
using UnityEngine;
using YuZiSdk;

public class GuideMgr:Single<GuideMgr>
{
    public int Index { get; private set; } = -1; //当前引导的序号
    public int Next { get; private set; } = -1;//下一步引导的序号
    
    public HardGuide HardGuide { get; private set; }//强制引导配置表
    public SoftGuide SoftGuide { get; private set; }//软引导配置表
    
    //特殊按钮

    public Transform SoftBtn;//软引导的按钮

    public bool InSoftGuide { get; private set; } = false;//当前是否处于软引导
    public bool InGuide { get; private set; } = false;//当前是否处于强制引导

    public Action FunSoftCall = null;//软引导的回调

    public EnumGuideBottomType GuideBottomType
    {
        get
        {
            return Data != null ? (EnumGuideBottomType)Data.guideBottomType  : EnumGuideBottomType.eNone ;
        }
    }

    public DataGuide Data = null;//数据
    private Dictionary<int, Transform> _dicBtnMapping = new Dictionary<int, Transform>();//实际的页面按钮和按钮枚举值的映射 执行和显示统一时只需要使用这个
    private Dictionary<int, Transform> _dicBtnExecute = new Dictionary<int, Transform>();//实际引导中执行点击动作的节点,dicBtnMapping用于显示
    private Dictionary<int, string> _dicWinMapping = new Dictionary<int, string>()
    {
        {1, "UIEmpty"},
        {2, "UIFacility"},
        {3, "UIShip"},
        {4, "UIShipUnlock"},
        {5, "UIStaff"},
        {6, "UINurture"},
        {7, "UIZhaoshang"},
        {8, "UIZhaoshanginfo"},
        {9, "UIAd_Fuli"},
        {10, "UICleanTip"},
        {11, "UIJiaoTong"},
        {12, "UIJiaoTongInfo"},
        {13, "UIPintu"},
        {14, "UIPintuInfo"},
        {15, "UIFixShipMenu"},
        {16, "UINavigate"},
        {17, "UIStaffLvUp"},
        {18, "UIStaffOne"},
        {19, "UIDam"},
    };
    
    //打开界面时的参数 主要用于中断的引导的复原
    private Dictionary<int, string> _dicWinParamName = new Dictionary<int, string>()
    {
        
    };

    private GuideLimit _guideLimit = new GuideLimit();
    private GuideSpecial _guideSpecialEvent = new GuideSpecial();
    private bool _finishFlyGuide = false;

    public Action StopShowGuideSlip;
    // public Action BindGuideBtn;

    #region 滑动相关
    public List<Transform> HideBtnList = new List<Transform>();
    public Transform SlipStart { get; private set; }//开始滑动的物体
    public Transform SlipEnd { get; private set; }//结束滑动的物体
    #endregion 滑动相关

    public void Init(Action call) {
        Data = DataGuide.ReadFromFile(() => { });
        
        Index = Data.startIndex;
        if (Data.interrupt)
        {
            init_index(Index, true);
        }
        else
        {
            Next = -1;
        }
            
        call?.Invoke();
    }
    //初始化谁
    private void init_index(int id, bool interrupt = false)
    {
        Index = id;
        Next = id;
        HardGuide = TableCache.Instance.HardGuideTable[id];
        Data.startIndex = id;//保存id
        if (!interrupt)
        {
            Data.index = id;
        }
        Data.interrupt = true;//开启一组引导的时候,标记为true 结束一组的时候标记为false 按组恢复
        Data.SaveToFile();
    }

    /// <summary>
    /// 检查是否应该开启引导
    /// </summary>
    /// <returns></returns>
    public bool CheckOpenGuide()
    {
        if (CheckFinishAllGuide())
        {
            return false;
        }

        if (CheckInGuide())
        {
            return false;
        }

        if (!check_guide_condition())
        {
            //检查当前界面 是否满足打开引导的条件
            return false;
        }

        _dicWinMapping.TryGetValue(HardGuide.win_id, out string winName);
        if (!check_ui_ahead_overall(winName))
        {
            return false;
        }

        if (HardGuide.win_btn != 0 && GetBindBtn() == null)
        {
            return false;
        }

        if (HardGuide.win_btn != 0 && !GetBindBtn().gameObject.activeSelf)
        {
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// 是否完成了所有的强制引导
    /// </summary>
    /// <returns></returns>
    public bool CheckFinishAllGuide()
    {
        return Data.listNotEnd.Count == 0;
    }
    
    /// <summary>
    /// 检查是否有对话
    /// </summary>
    /// <returns></returns>
    public bool CheckHaveDialogue()
    {
        if (HardGuide.dialogue_id != 0)
        {
            var tDialogue = TableCache.Instance.GuideDialogueTable[HardGuide.dialogue_id];
            if (tDialogue.content != null && tDialogue.content.Trim() != "")
            {
                return true;
            }
        }
        
        return false;
    }

    /// <summary>
    /// 检查当前是否在引导中
    /// </summary>
    /// <returns></returns>
    public bool CheckInGuide()
    {
        return InGuide || InSoftGuide;
    }

    /// <summary>
    /// 判定当前引导是否准备好了
    /// </summary>
    /// <returns></returns>
    public bool CheckPrepare()
    {
        //还没有找到下一个引导目标
        if (Next < 0)
        {
            return false;
        }

        InGuide = true;
        return true;
    }

    /// <summary>
    /// 引导是否中断了
    /// </summary>
    /// <returns></returns>
    public bool CheckInterrupt()
    {
        Debug.Log($"引导是否中断了{Data.interrupt}");
        return Data.interrupt;
    }

    //检查引导是否满足开启条件
    private bool check_guide_condition()
    {
        if (Next == -1)
        {
            //表示上一组引导已经结束了 需要检索新的引导
            int id = get_guide_start_id();

            if (id != -1)
            {
                init_index(id);
            }

            return id != -1;
        }
        
        _dicWinMapping.TryGetValue(HardGuide.win_id, out string winName);
        if (winName != "" && get_ui_name() == winName && _guideLimit.CheckPass(HardGuide.open_limit_type, HardGuide.open_limit_param, HardGuide.id))
        {
            return true;
        }
        
        return false;
    }

    private int get_guide_start_id()
    {
        //获取所有窗口的顺序
        string win = get_ui_name();

        int startId = -1;

        if (Data.listNotEnd.Count > 0)
        {
            int id = Data.listNotEnd[0];
            var config = TableCache.Instance.HardGuideTable[id];
            _dicWinMapping.TryGetValue(config.win_id, out string winName);
            if (winName != "" && win == winName && _guideLimit.CheckPass(config.open_limit_type, config.open_limit_param, config.id))
            {
                startId = config.id;
            }
        }

        if (startId != -1)
        {
            Debug.Log($"新手引导==将要开始的新手引导组的起始id是{startId}");
#if TMSDK || DYTMSDK
            //TMSDK.Instance.SDKSendGuideEvent("begin", TableCache.Instance. );
#endif
        }
        return startId;
    }

    /// <summary>
    /// 中断后 恢复界面 恢复引导
    /// </summary>
    /// <returns></returns>
    public bool RecoveryGuideWindow()
    {
        if (!CheckInterrupt())
        {
            Debug.LogError($"之前的引导没有中断, 不需要恢复窗口");
            return false;
        }

        //不需要恢复窗口 则跳过当前引导
        if (HardGuide != null && HardGuide.recovery_window == 0)
        {
            Debug.LogError("恢复窗口为0, 不需要恢复");
            //配表标识为不需要恢复
            CompleteGroup(Data.startIndex);
            return false;
        }
        
        //标识为恢复的 但是可能还有一个完成到某一任务,就结束这一组引导的标志
        if (Data.index != -1)
        {
            var table = TableCache.Instance.HardGuideTable[Data.index];
            if (table.can_pass == 1)
            {
                CompleteGroup(Data.startIndex);
                return false;
            }
        }

        //转换参数 打开窗口
        _dicWinMapping.TryGetValue(HardGuide.win_id, out string winName);
        if (winName == "UIEmpty")
        {
            UIManager.OpenUI(winName, HardGuide.open_param);
        }
        else
        {
            UIManager.OpenTip(winName, HardGuide.open_param);
        }
        return true;
    }

    /// <summary>
    /// 绑定按钮
    /// </summary>
    /// <param name="tsf">按钮节点</param>
    /// <param name="type">对应的枚举值</param>
    /// <param name="tfExecute">执行的节点</param>
    public void BindBtn(Transform tsf, tableMenu.GuideWindownBtn type, Transform tfExecute = null)
    {
        if (_dicBtnMapping.ContainsKey((int)type))
        {
            _dicBtnMapping[(int)type] = tsf;
        }
        else
        {
            _dicBtnMapping.Add((int)type, tsf);
        }

        if (tfExecute != null)
        {
            _dicBtnExecute[(int)type] = tfExecute;
        }
    }

    /// <summary>
    /// 移除绑定的按钮
    /// </summary>
    /// <param name="type"></param>
    public void RemoveBindBtn(tableMenu.GuideWindownBtn type)
    {
        if (_dicBtnMapping.ContainsKey((int)type))
        {
            _dicBtnMapping.Remove((int)type);
        }
    }

    /// <summary>
    /// 获取引导绑定的按钮 用于显示或执行动作
    /// </summary>
    /// <returns></returns>
    public Transform GetBindBtn()
    {
        if (HardGuide == null)
        {
            return null;
        }

        _dicBtnMapping.TryGetValue(HardGuide.win_btn, out Transform tsf);
        if (tsf == null)
        {
            return null;
        }

        return tsf;
    }
    
    public Transform GetBindBtn(tableMenu.GuideWindownBtn type)
    {
        _dicBtnMapping.TryGetValue((int)type, out var result);
        return result;
    }

    /// <summary>
    /// 获取引导绑定的按钮 用于执行动作
    /// </summary>
    /// <returns></returns>
    public Transform GetExecuteBtn()
    {
        if (HardGuide == null)
        {
            return null;
        }
        
        _dicBtnExecute.TryGetValue(HardGuide.win_btn, out Transform tsf);
        if (tsf == null)
        {
            return GetBindBtn();
        }

        return tsf;
    }

    public void CompleteCurGuide()
    {
        if (HardGuide != null)
        {
            YuziMgr.Instance.ReportGuide(HardGuide.id, "");
            Debug.Log($"新手引导==完成的强制引导的id为{HardGuide.id}");
        }

        InGuide = false;
        //修改下一步引导的id
        update_guide_index();
        check_end_group();
        
        //大坝滑动 特殊代码 按理说应该添加到每个引导的特殊事件中
        MainMgr.Instance.ComCameraMove.OutAnimation();
    }
    
    private void update_guide_index()
    {
        if (HardGuide != null)
        {
            Next = HardGuide.next;
            Data.index = Next;
            Data.SaveToFile();
            if (Next != -1)
            {
                HardGuide = TableCache.Instance.HardGuideTable[Next];
            }
        }
    }
    
    //检查是否完成了一组引导
    private void check_end_group()
    {
        if (Next == -1)
        {
            if (Index != -1)
            {
                Data.listEnd.Add(Index);
                Data.listNotEnd.Remove(Index);
                Debug.Log($"新手引导==完成的引导的组别为{Index}");
            }
            Index = -1;
            Data.startIndex = -1;
            Data.interrupt = false;
            Data.SaveToFile();
        }
    }
    
    /// <summary>
    /// 完成某一组引导
    /// </summary>
    /// <param name="id"></param>
    public void CompleteGroup(int id)
    {
        Index = -1;
        Next = -1;
        
        Data.listEnd.Add(id);
        Data.listNotEnd.Remove(id);
        Data.startIndex = -1;
        Data.interrupt = false;
        HardGuide = null;
        Data.SaveToFile();
    }

    /// <summary>
    /// 检查是否完成了某一引导
    /// </summary>
    /// <returns></returns>
    public bool CheckHaveCompleteGuide()
    {
        return Data.listEnd.Count > 0;
    }

    #region soft guide

    public void TriggerSoftGuide(Transform transform, tableMenu.SoftGuideEvent type, string winName)
    {
        if (FunSoftCall == null)
        {
            return;
        }
        
        Data.DicSoftGuideCount.TryGetValue((int)type, out int count);
        if (count <= 0)
        {
            return;
        }
    
        if (InGuide || InSoftGuide)
        {
            return;
        }
    
        if (!check_ui_ahead_overall(winName))
        {
            return;
        }
    
        SoftGuide = TableCache.Instance.SoftGuideTable[(int)type];
        for (int i = 0; i < SoftGuide.hard_complete.Length; i++)
        {
            int groupId = SoftGuide.hard_complete[i];
            if (!CheckGuideGroupComplete(groupId))
            {
                return;
            }
        }
    
        SoftBtn = transform;
        FunSoftCall?.Invoke();
    }
    
    /// <summary>
    /// 检查这个软引导是否结束了
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool CheckSoftGuideEnd(tableMenu.SoftGuideEvent type)
    {
        Data.DicSoftGuideCount.TryGetValue((int)type, out int count);
        return count <= 0;
    }
    
    /// <summary>
    /// 检查这个软引导是否是第一次触发 如果是的话 那么就需要显示对话
    /// </summary>
    /// <param name="configID"></param>
    /// <returns></returns>
    public bool CheckFirstSoftGuide(int configID)
    {
        Data.DicSoftGuideCount.TryGetValue(configID, out int count);
        SoftGuide = TableCache.Instance.SoftGuideTable[configID];
        return SoftGuide.times == count;
    }
    
    /// <summary>
    /// 软引导的时候触发了点击事件 不管是否点中了引导的按钮 都算是点击了
    /// </summary>
    public void SoftGuideClick()
    {
        Data.DicSoftGuideCount.TryGetValue(SoftGuide.id, out int count);
        Data.DicSoftGuideCount[SoftGuide.id] = count - 1;
        Data.SaveToFile();
    }

    #endregion soft guide
    
    public bool CheckGuideGroupComplete(int group)
    {
        if (Data == null)
        {
            return false;
        }
        int len = Data.listEnd.Count;
        for (int i = 0; i < len; i++)
        {
            var config = TableCache.Instance.HardGuideTable[Data.listEnd[i]];
            if (config.group == group)
            {
                return true;
            }
        }

        return false;
    }
    
    //判断对应的ui是不是在最上层
    private bool check_ui_ahead_overall(string winName)
    {
        return get_ui_name() == winName;
    }

    /// <summary>
    /// 引导中需要执行的特殊事件
    /// </summary>
    public void ExecuteSpecialEvent()
    {
        if (HardGuide == null)
        {
            return;
        }
        
        _guideSpecialEvent.Executes(HardGuide.special_event_type, HardGuide.special_event_param);
    }

    /// <summary>
    /// 引导中的特殊事件的还原
    /// </summary>
    public void RecoverySpecialEvent()
    {
        if (HardGuide == null)
        {
            return;
        }
        
        _guideSpecialEvent.Recovery(HardGuide.special_event_type, HardGuide.special_event_param);
    }

    public void SetInSoftGuide(bool value)
    {
        InSoftGuide = value;
    }

    private string get_ui_name()
    {
        return UIManager.OpenUIName;
    }

    public void ResetGuideSlip()
    {
        StopShowGuideSlip?.Invoke();
    }

    public void SetHideBtnList(List<Transform> hideBtnList)
    {
        HideBtnList = hideBtnList;
    }
    
    public void SetSlipBtn(Transform start, Transform end)
    {
        SlipStart = start;
        SlipEnd = end;
    }

    public void SetBottomGuideType(EnumGuideBottomType type)
    {
        Data.guideBottomType = (int)type;
        Data.SaveToFile();
    }

    public void FinishFlyGuide()
    {
        if (_finishFlyGuide)
        {
            return;
        }
        _finishFlyGuide = true;
        RecoverySpecialEvent();
        CompleteCurGuide();
    }

    public void StarFlyGuide()
    {
        Debug.Log("启动 飞行引导 StarFlyGuide");
        _finishFlyGuide = false;
    }

    public void SetGuideNo(int no)
    {
        UIGuide.SetStart(false);
        
        for (int i = Data.listNotEnd.Count - 1; i >= 0; i--)
        {
            var item = Data.listNotEnd[i];
            if (item < no)
            {
                Data.listEnd.Add(item);
                Data.listNotEnd.Remove(item);
            }
            
        }
        init_index(no);
        
        UIGuide.SetStart(true);
    }

    public void ClearData()
    {
        InGuide = false;
        InSoftGuide = false;
    }
}