using UnityEngine;
using System;
using System.Xml.Serialization;

public class VectorAsset : MonoBehaviour {

}
public struct Vec2Int {
    public Vec2Int(int _x, int _y) { 
        x = _x;
        y = _y;
    }
    public int x;
    public int y;
}
[Serializable]
public class FloorData
{
    public float beginPos;
    public float endPos = 0;
    public float cambeginPos = 0;
    public float camendPos = 0;
    public string bridgePos;
    public string npcs = "";
    public string portals = "0";
    public string floorAngle = "0";
    public string bridgeAngle = "0";
}
[Serializable]
public class HighPanel
{
    public float one;
    public float two = 0;
    public float three = 0;
    public float four = 0;
    public float hight = 0;
    public float width = 0;
    public float begin = 0;

}
public class NpcData
{
    public int Id;
    public int floor;
    public float posX;
    public float posY;
    public float posH;
    public string dir = "左";
}
[Serializable]
public class Vector4Asset
{
    private float x;
    private float y;
    private float z;
    private float a;
    [NonSerialized]
    private Rect _vec;
    public Rect vec
    {
        get
        {
            if (_vec == Rect.zero)
            {
                if (x != 0 || y != 0 || z != 0 || a != 0)
                {
                    _vec = new Rect(x, y, z, a);
                }
            }
            return _vec;
        }
        set
        {
            _vec = value;
            x = _vec.x;
            y = _vec.y;
            z = _vec.width;
            a = _vec.height;
        }
    }
    public Vector4Asset() {

    }
    public Vector4Asset(float x, float y, float z, float a) {
        vec = new Rect(x, y, z, a);
    }
    public Vector4Asset(Rect pos) {
        vec = pos;
    }
    public string _vecStr;
    public string VecStr
    {
        get
        {
            return _vecStr;
        }
        set
        {
            _vecStr = value;
        }
    }
    public void ChangeByVecStr() {
        if(string.IsNullOrEmpty(_vecStr))
        {
            return;
        }
        if(!_vecStr.Contains(","))
        {
            Debug.LogError("格式错误:" + _vecStr);
            return;
        }
        var strs = _vecStr.Split(',');
        if(strs.Length != 3)
        {
            Debug.LogError("格式错误:" + _vecStr);
            return;
        }
        x = float.Parse(strs[0]);
        y = float.Parse(strs[1]);
        z = float.Parse(strs[2]);
    }
}
[Serializable]
public class Vector3Asset
{
    private float x;
    private float y;
    private float z;
    [NonSerialized]
    [XmlIgnore]
    private Vector3 _vec = Vector3.zero;
    public Vector3 vec
    {
        get
        {
            if(_vec == Vector3.zero)
            {
                if(x != 0 || y != 0 || z != 0)
                {
                    _vec = new Vector3(x, y, z);
                }
            }
            return _vec;
        }
        set
        {
            _vec = value;
            x = _vec.x;
            y = _vec.y;
            z = _vec.z;
        }
    }
    public Vector3Asset() {

    }
    public Vector3Asset(float x, float y, float z) {
        vec = new Vector3(x, y, z);
    }
    public Vector3Asset(Vector3 pos) {
        vec = pos;
    }
    public string _vecStr;
    public string VecStr
    {
        get
        {
            return _vecStr;
        }
        set
        {
            _vecStr = value;
        }
    }
    public void ChangeByVecStr() {
        if(string.IsNullOrEmpty(_vecStr))
        {
            return;
        }
        if(!_vecStr.Contains(","))
        {
            Debug.LogError("格式错误:" + _vecStr);
            return;
        }
        var strs = _vecStr.Split(',');
        if(strs.Length != 3)
        {
            Debug.LogError("格式错误:" + _vecStr);
            return;
        }
        x = float.Parse(strs[0]);
        y = float.Parse(strs[1]);
        z = float.Parse(strs[2]);
    }
}
[Serializable]
public class Vector2Asset
{
    private float x;
    private float y;
    [NonSerialized]
    [XmlIgnore]
    private Vector2 _vec = Vector2.zero;
    public Vector2 vec
    {
        get
        {
            if(_vec == Vector2.zero)
            {
                if(x != 0 || y != 0)
                {
                    _vec = new Vector2(x, y);
                }
            }
            return _vec;
        }
        set
        {
            _vec = value;
            x = _vec.x;
            y = _vec.y;
        }
    }
    public Vector2Asset() {

    }
    public Vector2Asset(float x, float y) {
        vec = new Vector2(x, y);
    }
    public Vector2Asset(Vector2 pos) {
        vec = pos;
    }
}
