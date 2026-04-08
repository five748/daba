using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
[Serializable]
public class TriggerTen
{
    public TriggerTen()
    {

    }
    public TriggerTen(string _chinese, string _typeName, bool _boolValue = false, float _floatValue = 0, string _stringValue = "")
    {
        chinese = _chinese;
        typeName = _typeName;
        boolValue = _boolValue;
        floatValue = _floatValue;
        stringValue = _stringValue;
        if (_typeName == "vector3")
        {
            vecValue3 = new Vector3Asset();
        }
        if (_typeName == "vector2")
        {
            vecValue2 = new Vector2Asset();
        }
    }
    public TriggerTen(string _chinese, string _typeName, List<string> _strs, string defalut = "")
    {
        chinese = _chinese;
        typeName = _typeName;
        if (defalut == "")
            stringValue = _strs[0];
        else
            stringValue = defalut;
        strs = _strs;
    }
    public TriggerTen(string _chinese, string _typeName, int _defaultInt)
    {
        chinese = _chinese;
        typeName = _typeName;
        intValue = _defaultInt;
    }
    public string chinese;
    public string stringValue;
    public bool boolValue;
    public float floatValue;
    public Vector3Asset vecValue3;
    public Vector2Asset vecValue2;
    public string typeName;
    public int intValue;
    public List<string> strs;
    public byte[] bytes;
    public TriggerTen Copy()
    {
        var copy = new TriggerTen(chinese, typeName, boolValue, floatValue, stringValue);
        copy.intValue = intValue;
        copy.strs = strs;

        if (bytes != null)
        {
            copy.bytes = new byte[bytes.Length];
            Array.Copy(bytes, copy.bytes, bytes.Length);
        }
        return copy;
    }
    public long Len
    {
        get
        {
            long len = 0;
            if (stringValue != null)
            {
                len += stringValue.Length;
            }
            if (strs != null)
            {
                foreach (var item in strs)
                {
                    len += item.Length;
                }
            }
            return len;
        }
    }
}

[Serializable]
public class SkillTriggerNode
{
    [NonSerialized]
    public static Dictionary<string, int> TriggerHeight = new Dictionary<string, int>{
        {"相机震动", 180},
        {"颜色改变", 200},
        {"播放声音", 240},
        {"飘血", 220},
        {"显示技能名", 150},
        {"事件", 100},
        {"时间变化", 120},
        {"相机移动", 300},
        {"屏幕颜色变化", 300},
        {"寻敌逻辑", 200},
        {"发射动画", 205},
        {"发射特效", 255},
        {"碰撞飞行物", 280},
        {"激光飞行物", 370},
        {"跟随飞行物", 245},
        {"受击动画", 220},
        {"受击特效", 205},
        {"抛物飞行物", 405},
        {"环形飞行物", 205},
        {"喷火飞行物", 205},
        {"回力镖飞行物", 205},
        {"地雷飞行物", 205},
        {"飞行Buff", 205},
    };
    public SkillTriggerNode()
    {

    }
    public SkillTriggerNode(string _nodeName, int _id, bool _isStory)
    {
        nodeName = _nodeName;
        parents = new List<SkillTriggerNode>();
        childs = new List<SkillTriggerNode>();
        values = new Dictionary<string, TriggerTen>();
        Id = _id;
        if (_nodeName == "相机移动")
        {
            curveMoveData = CurveData.ReadToFile("cameraDefault");
        }
        else
        {
            curveMoveData = CurveData.ReadToFile("default");
        }

        curveRotaData = new CurveData("rota");
        isStory = _isStory;
    }
    //是否是平行任务
    public bool isPar;
    //平行ID
    public List<int> parIds;
    public bool isRoot;
    public bool unActive;
    public bool isStory;
    public int Id;
    public int rootIndex;
    public SkillTriggerNode parent;
    public List<SkillTriggerNode> parents;
    public List<SkillTriggerNode> childs;
    public Dictionary<string, TriggerTen> values;
    public bool isUseCommon = false;
    public string nodeName;
    public string newCurveName;
    public CurveData curveMoveData;
    public CurveData curveRotaData;
    public int fatherClickNpcId;
    public bool isMoveCurve;
    [NonSerialized]
    public GameObject go;
    [NonSerialized]
    public AudioClip clip;
    public int Height
    {
        get
        {
            return TriggerHeight[nodeName];
        }
    }
    [NonSerialized]
    private Rect _rect = Rect.zero;
    public Rect rect
    {
        get
        {
            if (_rect == Rect.zero)
            {
                if (rectX != 0 || rectY != 0 || rectW != 0 || rectH != 0)
                    _rect = new Rect(rectX, rectY, rectW, rectH); ;
            }
            _rect.height = Height;
            return _rect;
        }
        set
        {
            _rect = value;
            rectX = rect.x;
            rectY = rect.y;
            rectW = rect.width;
            rectH = rect.height;
        }
    }
    private float rectX;
    private float rectY;
    private float rectW;
    private float rectH;

    public SkillTriggerNode AddChild(string _nodeName, int _id, bool _isStory)
    {
        //Debug.Log("addChild");
        var child = new SkillTriggerNode(_nodeName, _id, _isStory);
        if (!_isStory)
        {
            child.parent = this;
        }
        else
        {
            child.parents.Add(this);
        }
        child.rect = new Rect(rect.x + 150, rect.y, rect.width, child.Height);
        childs.Add(child);
        return child;
    }
    public void SetRootIndex(SkillTriggerNode copy, SkillTriggerNode one)
    {
        //foreach(var parent in one.parents)
        //{
        //    if(parent.IsRootTrigger())
        //    {
        //        copy.rootIndex = parent.Id;
        //        return;
        //    }
        //    SetRootIndex(copy, parent);
        //}
    }
}
[Serializable]
public class HUDNode : ReadAndSaveTool<HUDNode>
{
    public HUDNode()
    {

    }
    public TriggerTen tem = new TriggerTen("开始位置", "enmu", new List<string> { "上", "中", "下" }, "中");
    public static HUDNode ReadSkill(int hudId, string _nodeName)
    {
        return ReadSkillBase(SavePath + hudId, () => {
            return new HUDNode(hudId, _nodeName);
        });
    }
    public void SaveToFile()
    {
        Save(Application.dataPath + "/" + SavePath + Id + ".bytes");
    }
    public static string SavePath
    {
        get
        {
            return "Res/Skill/hud/".ModifySkillPath();
        }
    }
    public HUDNode(int _id, string _nodeName)
    {
        nodeName = _nodeName;
        Id = _id;
        curveData = CurveData.ReadToFile("cameraDefault");
    }
    //平行ID
    public int Id;
    public string nodeName;
    public CurveData curveData;
    public int Height
    {
        get
        {
            return 380;
        }
    }
    [NonSerialized]
    private Rect _rect = Rect.zero;
    public Rect rect
    {
        get
        {
            if (_rect == Rect.zero)
            {
                if (rectX != 0 || rectY != 0 || rectW != 0 || rectH != 0)
                    _rect = new Rect(rectX, rectY, rectW, rectH); ;
            }
            _rect.height = Height;
            return _rect;
        }
        set
        {
            _rect = value;
            rectX = rect.x;
            rectY = rect.y;
            rectW = rect.width;
            rectH = rect.height;
        }
    }
    private float rectX = 20;
    private float rectY = 20;
    private float rectW = 150;
    private float rectH = 350;
}
[Serializable]
public class SkillAsset : ReadAndSaveTool<SkillAsset>
{
    public SkillAsset()
    {
        scrollPos = new Vector2Asset(5000, 5000);
        LeftHeroId = 1001;
        RightHeroId = 1002;
    }
    public static string SavePath
    {
        get
        {
            return "Skill/";
        }
    }
    public int rootId;
    public Dictionary<int, SkillTriggerNode> AllTriggers = new Dictionary<int, SkillTriggerNode>();
    public Vector2Asset scrollPos;
    public int MaxSkillIndex;
    public string[] TargetIds;
    public int TargetsLen;
    public int LeftHeroId;
    public int RightHeroId;
    public static SkillAsset ReadSkill(int skillId)
    {
        //Debug.Log(skillId);
        return ReadSkillBase(SavePath + skillId, () => {
            return new SkillAsset();
        });
    }
    public void SaveToFile(int skillId)
    {
        //foreach (var item in AllTriggers)
        //{
        //    if (item.Value.nodeName == "相机移动") {
        //        item.Value.curveMoveData.SaveToFile("cameraDefault");
        //        return;
        //    }
        //}
        Save(Application.dataPath + "/" + SavePath + skillId + ".bytes");
    }
}
