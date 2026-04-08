

using System;
using TreeData;
using UnityEngine;

public class StaffMgr : Singleton<StaffMgr>
{
    public DataStaff data { get; }
    
    private StaffMgr()
    {
        data = DataStaff.ReadFromFile();
        //GameProcess.Instance.SaveEvent += data.SaveToFile;
    }

    public void Unlock(int staffId)
    {
        data.staffs[staffId].unlock = true;
        ChannelMgr.Instance.UnlockStaff(staffId);
        data.SaveToFile();
    }
}