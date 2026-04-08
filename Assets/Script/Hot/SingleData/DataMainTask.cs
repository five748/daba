

using System;
using System.Collections.Generic;
using System.Diagnostics;
using TreeData;

[Serializable]
public class DataMainTask : ReadAndSaveTool<DataMainTask>
{
    private static string save_path = "maintask";
    //主线
    public int OnTaskTabId = 1;     //主线任务Id
    public long _onTaskNum;
    public bool OpenAllSystem = false;
    [LitJson.JsonIgnore]
    public long OnTaskNum {
        get {
            return _onTaskNum;
        }
        set {
            //UnityEngine.Debug.LogError(value);
            _onTaskNum = value;
        }
    }    //当前主线任务完成数量    
    public Dictionary<string, int> taskAddUpNums = new Dictionary<string, int>();//累计数量

    public static DataMainTask ReadFromFile()
    {
        return ReadByPhone(save_path, () =>
            { 
                return new DataMainTask();
            }
        );
    }
    public void SaveToFile()
    {
        WriteInPhone(this, save_path);
    }
}
