using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;
using YuZiSdk;
public enum MainTaskMenu {
    /// <summary>
    /// 解锁某条船
    /// </summary>
    OpenSomeOneShip = 1,
    /// <summary>
    /// 获得经营收入
    /// </summary>
    GetMoneyNum = 2,
    /// <summary>
    /// 解锁某个设备
    /// </summary>
    OpenSomeOneEquip = 3,
    /// <summary>
    /// 招商签约满xx次
    /// </summary>
    SumSignShipNum = 4,
    /// <summary>
    /// 培养一名4s员工
    /// </summary>
    TrainOneStaffMaxLv = 5,
    /// <summary>
    /// 船总等级达到
    /// </summary>
    SumShipLvNum = 6,
    /// <summary>
    /// 某个职员升到5级
    /// </summary>
    SomeOneStaffMaxLv = 7,
    /// <summary>
    /// 航道冲洗xx次
    /// </summary>
    SumCleanNum = 8,
    /// <summary>
    /// 指挥交通
    /// </summary>
    SumDirTrafNum = 9,
    /// <summary>
    /// 装载货物
    /// </summary>
    SumLoadUpNum = 10,
    /// <summary>
    /// 船舶检修次数
    /// </summary>
    SumFixShipNum = 11,
    /// <summary>
    /// 航运发货次数
    /// </summary>
    SumShipSendNum = 12,
    /// <summary>
    ///某个养护设备升到五级
    /// </summary>
    FixEquipLvNum = 13,
}
public class MTaskData : Single<MTaskData>
{
    private DataMainTask _data;
    public DataMainTask data {
        get
        {
            if (_data == null) {
                _data = DataMainTask.ReadFromFile();
            }
            return _data;
        }
    }
    private UIEmptyAuto Auto;
    private bool isOver;
    public bool IsOver{ get => isOver; }
    public int currentId { get=>data.OnTaskTabId; }

    public Table.task tabOne {
        get { 
            return TableCache.Instance.taskTable[data.OnTaskTabId];
        }
    }
    public System.Action TaskOverEvent;
    public void Init(UIEmptyAuto _auto) {
        //data.OnTaskTabId = 101;
        AddUpKey();
        Auto = _auto;
        ShowTask();
        EventTriggerListener.Get(Auto.Task).onClick = ClickTask;
    }
    public void ShowTask() {
        SetIsMax();
        CheckIsOver();
        ShowTaskTran();
    }
    public void CrrentTaskOver() {
        data.OnTaskNum = tabOne.targetNum;
        SetIsMax();
        CheckIsOver();
        ShowTaskTran();
    }
    //添加数量
    public void AddTaskNum(MainTaskMenu taskType, int num = 1, int other = 0) {
        if (num < 0)
        {
            return;
        }
        AddUpKey();
        int taskTypeId = (int)taskType;
        string sumKey = taskTypeId + "_" + other;
        if (data.taskAddUpNums.ContainsKey(sumKey))
        {
            data.taskAddUpNums[sumKey] += num;
        }
        if (!(other != -1 && other != tabOne.param))
        {
            if (tabOne.typeId == taskTypeId)
            {
                data.OnTaskNum += num;
                if (Auto != null)
                {
                    SetIsMax();
                    CheckIsOver();
                    ShowTaskTran();
                }
            }
        }
        data.SaveToFile();
    }
    //等于数量
    public void SetTaskNum(MainTaskMenu taskType, int num, int other = 0)
    {
        if (num < 0) {
            return;
        }
        int taskTypeId = (int)taskType;
        string sumKey = taskTypeId + "_" + other;
        if (data.taskAddUpNums.ContainsKey(sumKey))
        {
            data.taskAddUpNums[sumKey] = num;
        }
        if (other != -1 && other != tabOne.param)
        {
            //特殊参数
            return;
        }
        if (tabOne.typeId == taskTypeId)
        {
            data.OnTaskNum = num;
            if (Auto != null)
            {
                SetIsMax();
                CheckIsOver();
                ShowTaskTran();
            }
        }
    }
    
    private void AddUpKey() {
        if (data.taskAddUpNums.Count != 0) {
            return;
        }
        foreach (var item in TableCache.Instance.taskTable)
        { 
            if (TableCache.Instance.taskTypeTable[item.Value.typeId].type == 1) {
               
                var key = item.Value.typeId + "_" + item.Value.param;
                if (!data.taskAddUpNums.ContainsKey(key))
                {
                    data.taskAddUpNums.Add(key, 0);
                }
            }
        }
    }
    private void CheckIsOver() {
        isOver = data.OnTaskNum >= tabOne.targetNum;
    }
    public void ClickTask(GameObject btn) {
        Debug.Log("clickTask");
        if (isOver)
        {
            YuziMgr.Instance.ReportNormal(5001, 0,data.OnTaskTabId);
            GetAward();
            if (TaskOverEvent != null)
            {
                TaskOverEvent();
            }
            data.SaveToFile();
#if DYTMSDK
            P8SDK.Instance.SDKSendGuideEvent("begin", tabOne.desc0 + tabOne.desc1 + tabOne.desc2, data.OnTaskTabId.ToString());
#endif
        }
        else
        {
            GoToUI();
        }
    }
    public bool isGoToMessage;
    public void GoToUI() {
        UIManager.CloseAllTip();
        var taskType = tabOne.typeId;
        if (taskType == 1) {
            //解锁某条船
            UIEmpty.Instance.ClickScore(Auto.Task.gameObject);
        }
        if (taskType == 2)
        {
            //获得经营收入
            Msg.Instance.Show("当前界面过闸收入!");
        }
        if (taskType == 3)
        {
            //解锁某个设备
            UIEmpty.Instance.ClickBtn_facility(Auto.Task.gameObject);
        }
        if (taskType == 4)
        {
            //招商签约满xx次
            UIEmpty.Instance.ClickBtn_callbusiness(Auto.Task.gameObject);
        }
        if (taskType == 5)
        {
            //培养一名4s员工
            UIEmpty.Instance.ClickBtn_staffup(Auto.Task.gameObject);
        }
        if (taskType == 6)
        {
            //船总等级达到
            UIEmpty.Instance.ClickScore(Auto.Task.gameObject);
        }
        if (taskType == 7)
        {
            //某个职员升到5级
            //if (!StaffMgr.Instance.data.staffs[tabOne.param].unlock) {
            //    Msg.Instance.Show($"职员{TableCache.Instance.tollCollectorTable[tabOne.param].name}未解锁!");
            //    return;
            //}
            UIStaff.MainTaskGoToId = tabOne.param;
            UIEmpty.Instance.ClickBtn_staff(Auto.Task.gameObject);
        }
        if (taskType == 8)
        {
            //航道冲洗xx次
            UIEmpty.Instance.ClickBtn_clean(Auto.Task.gameObject);

        }
        if (taskType == 9)
        {
            //指挥交通
            UIEmpty.Instance.ClickBtn_jiaotong(Auto.Task.gameObject);
        }
        if (taskType == 10)
        {
            //装载货物
            UIEmpty.Instance.ClickBtn_pintu(Auto.Task.gameObject);
        }
        if (taskType == 11)
        {
            //船舶检修次数
            UIEmpty.Instance.ClickBtn_fix(Auto.Task.gameObject);
        }
        if (taskType == 12)
        {
            //航运发货次数
            UINavigate.IsGotoOrder = true;
            UIEmpty.Instance.ClickBtn_navigate(Auto.Task.gameObject);
        }
        if (taskType == 13)
        {
            ///养护设备
            var damId = TableCache.Instance.equipmentLvupTable[tabOne.param].forDamId;
            bool isLock = !ChannelMgr.Instance.Data.Dams[damId].unlock;
            if (isLock)
            {
                Msg.Instance.Show($"大坝{TableCache.Instance.damTable[damId].name}未解锁!");
                return;
            }
            UINurture.MainTaskGoToId = tabOne.param;
            UIEmpty.Instance.ClickBtn_nurture(Auto.Task.gameObject);
        }

    }
    public void GetAward() {
        var award = tabOne.reward;
        //Msg.Instance.ShowItem(award[0], award[1], "图标/" + TableCache.Instance.TItemTable[award[0]].icon, Auto.MscPoint);
        //M_Item.Instance.Fly(award[0], 79, Auto.Task.position, 0);
        UIFly.FlyItem(award[0].id, award[0].num);
        PlayerMgr.Instance.AddItemNum(award[0].id, award[0].num);
        isOver = false;
        //string desc = tabOne.desc0 + tabOne.desc1 + tabOne.desc2;
        data.OnTaskTabId++;
        string key = tabOne.typeId + "_" + tabOne.param;
        if (data.taskAddUpNums.ContainsKey(key))
        {
            data.OnTaskNum = data.taskAddUpNums[key];
        }
        else
        {
            data.OnTaskNum = 0;
        }
        if (SetIsMax())
        {
            return;
        }
        CheckIsOver();
        ShowTaskTran();
    }
    private bool SetIsMax() {
        if (!TableCache.Instance.taskTable.ContainsKey(data.OnTaskTabId))
        {
            Auto.Task.SetActive(false);
            return true;
        }
        Auto.Task.SetActive(true);
        return false;
    }
    public void ShowTaskTran() {
        if (Auto == null || Auto.Over == null)
        {
            return;
        }
        string desc2 = tabOne.desc2;
        if (desc2 == "\"\"")
        {
            desc2 = "";
        }
        Auto.StringTaskmsc = tabOne.desc0 + tabOne.desc1 + desc2;
        Auto.StringTasknum = (data.OnTaskNum + "/" + tabOne.targetNum).ChangeColor("a64141");
        Auto.Noover.SetActive(!isOver);
        Auto.Over.SetActive(isOver);
    }
}
