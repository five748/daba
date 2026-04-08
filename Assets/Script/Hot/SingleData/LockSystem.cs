using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSystem : Single<LockSystem>
{
    public Dictionary<int, Table.system> Tab {
        get {
            return TableCache.Instance.systemTable;
        }
    }
    private int TaskId {
        get {
            return MTaskData.Instance.data.OnTaskTabId;
        }
    }
    public bool GetSystemIsOpen(int systemId) {
        if (MTaskData.Instance.data.OpenAllSystem) {
            return true;
        }
        return TaskId >= Tab[systemId].guideTaskId;
    }
    public void ShowSysIconActive(UIEmptyAuto Auto) {
        if (Auto == null || Auto.Btn_facility == null)
        {
            return;
        }
        //设施
        Auto.Btn_facility.SetActive(GetSystemIsOpen(1));
        //员工
        Auto.Btn_staff.SetActive(GetSystemIsOpen(2));
        //养护
        Auto.Btn_nurture.SetActive(GetSystemIsOpen(3));
        //培训
        Auto.Btn_staffup.SetActive(GetSystemIsOpen(4));
        //航运
        Auto.Btn_navigate.SetActive(GetSystemIsOpen(5));
        //指挥交通
        Auto.Btn_jiaotong.SetActive(GetSystemIsOpen(6));
        //装卸货物
        Auto.Btn_pintu.SetActive(GetSystemIsOpen(7));
        //清洗航道
        Auto.Btn_clean.SetActive(GetSystemIsOpen(8));
        //船舶检修
        Auto.Btn_fix.SetActive(GetSystemIsOpen(9));
        //地图
        Auto.Btn_map.SetActive(GetSystemIsOpen(10));
        //每日福利
        Auto.Btn_adfuli.SetActive(GetSystemIsOpen(11));
        //赞助
        Auto.Btn_adzanzhu.SetActive(GetSystemIsOpen(12));
        //领奖
        Auto.Btn_adday.SetActive(GetSystemIsOpen(13));
        //藏品
        Auto.Btn_collect.SetActive(GetSystemIsOpen(14));
        //成就
        Auto.Btn_ach.SetActive(GetSystemIsOpen(15));
        //招商
        Auto.Btn_callbusiness.SetActive(GetSystemIsOpen(16));
        //赛龙舟
        Auto.Btn_longship.SetActive(GetSystemIsOpen(17));
        //主线任务
        Auto.Task.SetActive(GetSystemIsOpen(18));
    }
}
