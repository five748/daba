using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class CurveData:ReadAndSaveTool<CurveData>
{
    public static string SavePath
    {
        get
        {
            return "Res/Skill/Curve/";
        }
    }
    public CurveData(string _name) {
        moveName = _name;
    }
    public bool xIsHaveTarget = true;
    public bool yIsHaveTarget = true;
    [NonSerialized]
    public float xTargetModify;
    [NonSerialized]
    public float yTargetModify;
    public float time = 1;
    public float time1 = 1;
    public string moveName;
    public float scale = 1;
    public Vector3Asset begin = new Vector3Asset();
    public Vector3Asset end = new Vector3Asset();
    public CurveAsset sumCurveX = new CurveAsset();
    public CurveAsset sumCurveY = new CurveAsset();
    public CurveAsset sumCurveYS = new CurveAsset();
    public CurveAsset sumCurveZ = new CurveAsset();
    public Dictionary<string, CurveAsset> otherCurves;
    public static CurveData ReadToFile(string _name) {
        var data = ReadSkillBase(SavePath + _name, () =>
        {
            return new CurveData(_name);
        });
        return data;
    }
    public CurveData Copy() {
        var copy = new CurveData(moveName);
        copy.xIsHaveTarget = xIsHaveTarget;
        copy.yIsHaveTarget = yIsHaveTarget;
        copy.xTargetModify = xTargetModify;
        copy.yTargetModify = yTargetModify;
        copy.time = time;
        if(begin != null)
            copy.begin = new Vector3Asset(begin.vec);
        if(end != null)
            copy.end = new Vector3Asset(end.vec);
        if(sumCurveX != null)
            copy.sumCurveX = sumCurveX.Copy();
        if(sumCurveY != null)
            copy.sumCurveY = sumCurveY.Copy();
        if(sumCurveYS != null)
            copy.sumCurveYS = sumCurveYS.Copy();
        if(sumCurveZ != null)
            copy.sumCurveZ = sumCurveZ.Copy();
        if(otherCurves != null) {
            copy.otherCurves = new Dictionary<string, CurveAsset>();
            foreach(var item in otherCurves)
            {
                copy.otherCurves.Add(item.Key, item.Value.Copy());
            }
        }
        return copy;
    }
    public Coroutine ExcuteCurveMove(float delTime, System.Action<float, float, float, float> moveAction, System.Action<bool> callback, bool addcallback = false, float _time = -1) {
        float useTime = 0;
        if (_time == -1)
        {
            _time = time;
        }
        if(_time == 0) {
            _time = 0.25f;
        }
        delTime *= 1 / _time;
        delTime *= Time.timeScale;
        if(sumCurveYS == null) {
            sumCurveYS = new CurveAsset();
            //ScrollRect
        }
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if(useTime >= 1) {
                moveAction(sumCurveX.Curve.Evaluate(1), sumCurveY.Curve.Evaluate(1), sumCurveZ.Curve.Evaluate(1), sumCurveYS.Curve.Evaluate(1));
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime), sumCurveY.Curve.Evaluate(useTime), sumCurveZ.Curve.Evaluate(useTime), sumCurveYS.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveX(float delTime, System.Action<float> moveAction, System.Action<bool> callback, bool addcallback = false, float _time = -1)
    {
        float useTime = 0;
        if (_time == -1)
        {
            _time = time;
        }
        if (_time == 0)
        {
            _time = 0.25f;
        }
        delTime *= 1 / _time;
        delTime *= Time.timeScale;
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if (useTime >= 1)
            {
                moveAction(sumCurveX.Curve.Evaluate(1));
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveY(float delTime, System.Action<float> moveAction, System.Action<bool> callback, bool addcallback = false, float _time = -1)
    {
        float useTime = 0;
        if (_time == -1)
        {
            _time = time;
        }
        if (_time == 0)
        {
            _time = 0.25f;
        }
        delTime *= 1 / _time;
        delTime *= Time.timeScale;
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if (useTime >= 1)
            {
                moveAction(sumCurveY.Curve.Evaluate(1));
                callback(false);
                return false;
            }
            moveAction(sumCurveY.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveMoveSpeed(float delTime, System.Action<float, float, float, float> moveAction, System.Action<bool> callback, bool addcallback = false)
    {
        float useTime = 0;
        if (time == 0)
        {
            time = 0.25f;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        if (sumCurveYS == null)
        {
            sumCurveYS = new CurveAsset();
            //ScrollRect
        }
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if (useTime >= 1)
            {
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime), sumCurveY.Curve.Evaluate(useTime), sumCurveZ.Curve.Evaluate(useTime), sumCurveYS.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveWhite(float delTime, System.Action<float> moveAction, System.Action<bool> callback, bool addcallback = false)
    {
        float useTime = 0;
        if (time == 0)
        {
            time = 0.25f;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        if (sumCurveYS == null)
        {
            sumCurveYS = new CurveAsset();
            //ScrollRect
        }
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if (useTime >= 1)
            {
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveBlack(float delTime, System.Action<float> moveAction, System.Action<bool> callback, bool addcallback = false)
    {
        float useTime = 0;
        if (time == 0)
        {
            time = 0.25f;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        if (sumCurveYS == null)
        {
            sumCurveYS = new CurveAsset();
            //ScrollRect
        }
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if (useTime >= 1)
            {
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime));
            return true;
        });
    }
    public void ExcuteHudMove(float delTime, System.Action<float, float, float, float> moveAction, System.Action callback)
    {
        float useTime = 0;
        if (time == 0)
        {
            time = 1;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        if (sumCurveYS == null)
        {
            sumCurveYS = new CurveAsset();
            //ScrollRect
        }
        MonoTool.Instance.StartCor(() =>
        {
            useTime += delTime;
            if (useTime >= 1)
            {
                callback();
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime), sumCurveY.Curve.Evaluate(useTime), sumCurveZ.Curve.Evaluate(useTime), sumCurveYS.Curve.Evaluate(useTime));
            return true;
        });
    }
    public Coroutine ExcuteCurveMoveBack(float delTime, System.Action<float, float, float, float> moveAction, System.Action<bool> callback, bool addcallback = false)
    {
        float useTime = 1;
        if (time == 0)
        {
            time = 1;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        if (sumCurveYS == null)
        {
            sumCurveYS = new CurveAsset();
        }
        return MonoTool.Instance.StartCor(() => {
            if (useTime <= 0)
            {
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime), sumCurveY.Curve.Evaluate(useTime), sumCurveZ.Curve.Evaluate(useTime), sumCurveYS.Curve.Evaluate(useTime));
            useTime -= delTime;
            return true;
        });
    }
    public Coroutine ExcuteCurveTimeMove(float delTime, System.Action<float, float, float> moveAction, System.Action<bool> callback, bool addcallback = false) {
        float useTime = 0;
        if(time == 0)
        {
            time = 1;
        }
        delTime *= 1 / time;
        return MonoTool.Instance.StartCor(() => {
            useTime += delTime;
            if(useTime >= 1)
            {
                callback(false);
                return false;
            }
            moveAction(sumCurveX.Curve.Evaluate(useTime), sumCurveY.Curve.Evaluate(useTime), sumCurveZ.Curve.Evaluate(useTime));
            return true;
        });
    }
    public void ExcuteRotaCurveMove(float delTime, System.Action<Vector3> moveAction) {
        float useTime = 0;
        if(time == 0)
        {
            time = 1;
        }
        delTime *= 1 / time;
        delTime *= Time.timeScale;
        float rangeX = begin.vec.x - end.vec.x;
        float rangeY = begin.vec.y - end.vec.y;
        float rangeZ = begin.vec.z - end.vec.z;
        MonoTool.Instance.StartCor(() =>
        {
            useTime += delTime;
            if(useTime >= 1)
            {
                return false;
            }
            moveAction(begin.vec + new Vector3(sumCurveX.Curve.Evaluate(useTime) * rangeX, sumCurveY.Curve.Evaluate(useTime) * rangeY, sumCurveZ.Curve.Evaluate(useTime) * rangeZ));
            return true;
        });
        }
    public void SaveToFile() {
        Save(Application.dataPath + "/" + SavePath + moveName + ".bytes");
    }
    public void SaveToFile(string path) {
        Save(Application.dataPath + "/" + SavePath + path + ".bytes");
    }
    public bool IsSame(CurveData other) {
        if(this.sumCurveX.IsSame(other.sumCurveX)) {
            return false;
        }
        if(this.sumCurveY.IsSame(other.sumCurveY))
        {
            return false;
        }
        if(this.xIsHaveTarget != other.xIsHaveTarget)
        {
            return false;
        }
        if(this.yIsHaveTarget != other.yIsHaveTarget)
        {
            return false;
        }
        if(this.time != other.time)
        {
            return false;
        }
        return true;
    }
}
