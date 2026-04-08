using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameConfig
{
    public static string ClientPath
    {
        get
        {
            return Application.dataPath.CutDir(1);
        }
    }
    public static string ClintFatherPath
    {
        get
        {
            return Application.dataPath.CutDir(2);
        }
    }
    public static string UIPrefabPath
    {
        get
        {
            return "Res/Prefab/UIPrefab/";
        }
    }
    public static string PrefabPath
    {
        get
        {
            return "Res/Prefab/";
        }
    }
    public static string TablePath
    {
        get
        {
            return "Resources/table/".ModifyTablePath();
        }
    }
    public static string TableBytesPath
    {
        get
        {
            return "Res/tableByte/tab";
        }
    }
    public static string ImagePath
    {
        get
        {
            return "Res/Image/";
        }
    }
    public static string MusicPath
    {
        get
        {
            return "Res/music/";
        }
    }
    public static string UIEffectPath
    {
        get
        {
            return "Res/effect/prefab/ui/";
        }
    }
    public static string ArtPath
    {
        get
        {
            return ClintFatherPath + "art/";
        }
    }
    public static string PlanTablePath
    {
        get
        {
            return ClintFatherPath + "plan/3_数值文档";
        }
    }
    public static string DataTablePath
    {
        get
        {
            return Application.dataPath.CutDir(3) + "data/配置表/channel_渠道.xlsx";
        }
    }
    public static string UnityTabPath
    {
        get
        {
            return Application.dataPath + "/Resources/table/";
            // return Application.dataPath + "/Res/table/";
        }
    }
    public static string tableMenuScritPath {
        get {
            return Application.dataPath + "/Script/Main/Auto/tab/";
        }
    }
    public static string tableScritPath
    {
        get
        {
            return Application.dataPath + "/Script/Main/Auto/tab/scr/";
        }
    }
    public static string tableFrameScritPath
    {
        get
        {
            return Application.dataPath + "/baseframewxold/TabScr/";
        }
    }
    public static string TableCache
    {
        get
        {
            return Application.dataPath + "/Script/Main/Auto/tab/";
        }
    }
    public static string TableJsonPath
    {
        get
        {
            return "Res/tabjson/";
        }
    }
    public static string TableByteSavePath
    {
        get
        {
            return Application.dataPath + "/Res/tablebyte/";
        }
    }
    public static string DataCenterJsonPath
    {
        get
        {
            return Application.persistentDataPath + "/DataCenter/CenterJson.txt";
        }
    }
    public static string AutoTreeDataPath
    {
        get
        {
            return Application.dataPath + "/Script/Proto/Auto/";
        }
    }
    public static string DataCenterPath
    {
        get
        {
            return Application.dataPath + "/Res/DataCenter/center.bin";
        }
    }
    public static string AutoGoTreeDataPath
    {
        get
        {
            return ClintFatherPath + "plan/serverdoc/iproto/";
        }
    }
    public static string AutoProtoTreeDataPath
    {
        get
        {
            return ClintFatherPath + "plan/serverdoc/protobuf/";
        }
    }
    public static string AutoProtoEvent
    {
        get
        {
            return ClintFatherPath + "plan/serverdoc/protobuf/ProtoEvent.txt";
        }
    }
    public static string AutoDataStruct
    {
        get
        {
            return ClintFatherPath + "plan/serverdoc/protobuf/DataStruct.txt";
        }
    }
    public static string ProtoPath
    {
        get
        {
            return ClintFatherPath + "plan/serverdoc/iproto_auto/";
        }
    }
}
