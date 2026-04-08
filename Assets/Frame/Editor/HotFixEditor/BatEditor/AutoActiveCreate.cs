using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class AutoActiveCreate
{
    private static List<string> fakeActive = new List<string>() { "IntegralGrowthRank" };

    [MenuItem("自定义工具/生成活动")]
    public static void CreateActiveData()
    {
//        string path = Application.dataPath + "/Script/Activity/AutoActive.cs";
//        string dicStr = "";
//        string setStr = "";
//        string forStr = "";
//        string uiStr = "";
//        foreach (var item in TableCache.Instance.activity_typeTable)
//        {
//            if (item.Key < 4 || fakeActive.Contains(item.Value.EnumKey))
//            {
//                continue;
//            }
//            var str = "    private Dictionary<string, ActivityModel<Activity.WWW, TreeData.TTT>> _TTTs = new Dictionary<string, ActivityModel<Activity.WWW, TreeData.TTT>>();";
//            string str1 = @"    public ActivityModel<Activity.WWW, TreeData.TTT> TTT(string activeId) {
//        if (activeId == """") {
//            if (_TTTs.Count == 0)
//                return null;
//            return _TTTs.GetFirst();
//        }
//        return _TTTs[activeId];
//    }";
//            var str2 = @"            case XXX:
//                if(!_TTTs.ContainsKey(activeId)) {
//                    _TTTs.Add(activeId, null);
//                }
//                _TTTs[activeId] = ActivityModel<Activity.WWW, TreeData.TTT>.Create(activity, activityData);
//                flag = AutoActiveShow.Instance.IsShow(activeId);
//                if (flag) {
//                    AutoActiveRedPoint.Instance.CheckTTT(_TTTs[activeId]);
//                }
//                return flag;";
//            var str3 = @"        keys = _TTTs.GetKeys();
//        for (int i = 0; i < (keys == null ? 0 : keys.Count); i++) {
//            callback(_TTTs[keys[i]].Activity);
//        }";
//            var str4 = @"
//        { XXX, ""UITTT"" },";
//            str = str.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
//            str1 = str1.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
//            str2 = str2.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
//            str3 = str3.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
//            str4 = str4.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
//            if (item.Value.Type == 2)
//            {
//                str = str.Replace("WWW", item.Value.EnumKey);
//                str1 = str1.Replace("WWW", item.Value.EnumKey);
//                str2 = str2.Replace("WWW", item.Value.EnumKey);
//                str3 = str3.Replace("WWW", item.Value.EnumKey);
//            }
//            else
//            {
//                str = str.Replace("WWW", "ActiveConfig");
//                str1 = str1.Replace("WWW", "ActiveConfig");
//                str2 = str2.Replace("WWW", "ActiveConfig");
//                str3 = str3.Replace("WWW", "ActiveConfig");
//            }
//            dicStr += str + "\n" + str1 + "\n";
//            setStr += str2 + "\n";
//            forStr += str3 + "\n";
//            uiStr += str4;
//        }
//        string Fun1Str = @"    public bool SetData(TreeData.Activitysone activity, TreeData.ActivityDataListone activityData) {
//        var activeId = activity.Key;
//        bool flag;
//        switch(activity.ActType) {
//XXX  
//        }
//        return false;
//    }";
//        string Fun2Str = @"    public void Foreach(System.Action<TreeData.Activitysone> callback) {
//        List<string> keys;
//XXX
//    }";
//        string Fun3Str = @"
//    private Dictionary<int, string> uiName = new Dictionary<int, string>()
//    {XXX
//    };
//    public string GetUIName(int type)
//    {
//        if (!uiName.ContainsKey(type))
//        {
//            return null;
//        }
//        return uiName[type];
//    }";
//        dicStr += Fun1Str.Replace("XXX", setStr) + "\n";
//        dicStr += Fun2Str.Replace("XXX", forStr) + "\n";
//        dicStr += Fun3Str.Replace("XXX", uiStr) + "\n";
//        string classStr = @"using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class AutoActive : Single2<AutoActive> {
//XXX
//}";
//        classStr = classStr.Replace("XXX", dicStr);
//        path.WriteByUTF8(classStr);

//        CreateActiveShow();
//        CreateActiveRedPoint();

//        AssetDatabase.Refresh();
    }

    public static void CreateActiveShow()
    {
    //    string path = Application.dataPath + "/Script/Activity/AutoActiveShow.cs";
    //    string content = path.ReadByUTF8();
    //    foreach (var item in TableCache.Instance.activity_typeTable)
    //    {
    //        if (item.Key < 4 || fakeActive.Contains(item.Value.EnumKey))
    //        {
    //            continue;
    //        }
    //        if (content.Contains(item.Value.EnumKey+ "IsShow"))
    //        {
    //            continue;
    //        }
    //        string str1 = @"    private bool _TTTIsShow(string activityId) {
    //    if (AutoActive.Instance.TTT(activityId).Activity.ShowActivityTime >= TimeTool.SerNowUtcTimeInt || AutoActive.Instance.TTT(activityId).Activity.HideActivityTime <= TimeTool.SerNowUtcTimeInt)
    //        return false;
    //    //TODO:代码自动生成，判断活动是否显示，在此修改代码
    //    return true;
    //}";
    //        var str2 = @"            case XXX:
    //            return _TTTIsShow(activityId);";
    //        str1 = str1.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
    //        str2 = str2.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
    //        var index = content.IndexOf("//=====================代码自动生成标记，请勿删除=====================") + 57;
    //        content = content.Insert(index, "\n" + str1);
    //        index = content.IndexOf("//=====================代码自动生成标记，请勿删除=====================", index);
    //        content = content.Insert(index, str2 + "\n");
    //    }
    //    path.WriteByUTF8(content);
    }

    public static void CreateActiveRedPoint()
    {
    //    string path = Application.dataPath + "/Script/Activity/AutoActiveRedPoint.cs";
    //    string content = path.ReadByUTF8();
    //    foreach (var item in TableCache.Instance.activity_typeTable)
    //    {
    //        if (item.Key < 4 || fakeActive.Contains(item.Value.EnumKey))
    //        {
    //            continue;
    //        }
    //        if (content.Contains("Check" + item.Value.EnumKey))
    //        {
    //            continue;
    //        }
    //        string str1 = @"    private struct TTTStruct
    //{

    //};
    //private TTTStruct TTTData;
    //public void CheckTTT(ActivityModel<Activity.WWW, TreeData.TTT> activityModel) {
    //    if (!AutoActiveShow.Instance.IsShow(activityModel.Activity.Key))
    //    {

    //        return;
    //    }

    //}";
    //        string str2 = @"            case XXX:
    //            CheckTTT(AutoActive.Instance.TTT(activityId));
    //            break;";
    //        str1 = str1.Replace("TTT", item.Value.EnumKey);
    //        str2 = str2.Replace("TTT", item.Value.EnumKey).Replace("XXX", item.Key.ToString());
    //        if (item.Value.Type == 2)
    //        {
    //            str1 = str1.Replace("WWW", item.Value.EnumKey);
    //        }
    //        else
    //        {
    //            str1 = str1.Replace("WWW", "ActiveConfig");
    //        }
    //        var index = content.IndexOf("//=====================代码自动生成标记，请勿删除=====================") + 57;
    //        index = content.IndexOf("//=====================代码自动生成标记，请勿删除=====================", index);
    //        content = content.Insert(index, str1 + "\n\n");
    //        index += str1.Length + 59;
    //        index = content.IndexOf("//=====================代码自动生成标记，请勿删除=====================", index);
    //        content = content.Insert(index, str2 + "\n");
    //    }
    //    path.WriteByUTF8(content);
    }
}
