using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class CurveAsset
{
    private float[] times;
    private float[] values;
    private float[] inTangents;
    private float[] outTangents;
    private float[] inWeights;
    private float[] outWeights;
    [NonSerialized]
    private Keyframe[] _keys;
    private Keyframe[] keys{
        get {
            if(_keys == null) {
                if(times != null) {
                    _keys = new Keyframe[times.Length];
                    for(int i = 0; i < _keys.Length; i++)
                    {
                        _keys[i] = new Keyframe(times[i], values[i], inTangents[i], outTangents[i], inWeights[i], outWeights[i]);
                        _keys[i].weightedMode = WeightedMode.None;
                        //Debug.Log("time:" + times[i] + "value:" + values[i] + "inTangent:" + inTangents[i] + "outTangent:" + outTangents[i] + "inWeight:" + inWeights[i] + "outWeigth:" + outWeights[i]);
                    }
                }
            }
            return _keys;
        }
        set {
            _keys = value;
            if(_keys == null) {
                return;
            }
            times = new float[_keys.Length];
            values = new float[_keys.Length];
            inTangents = new float[_keys.Length];
            outTangents = new float[_keys.Length];
            inWeights = new float[_keys.Length];
            outWeights = new float[_keys.Length];

            for(int i = 0; i < _keys.Length; i++)
            {
                times[i] = _keys[i].time;
                values[i] = _keys[i].value;
                inTangents[i] = _keys[i].inTangent;
                outTangents[i] = _keys[i].outTangent;
                inWeights[i] = _keys[i].inWeight;
                outWeights[i] = _keys[i].outWeight;
            }
        }
    }
    [NonSerialized]
    private AnimationCurve _curve;
    public AnimationCurve Curve
    {
        get
        {
            if(_curve == null)
            {
                if(keys != null)
                    _curve = new AnimationCurve(keys);
                else
                    _curve = new AnimationCurve();
            }
            return _curve;
        }
        set {
            _curve = value;
            keys = _curve.keys;
        }
    }
    public float range;
    public float Range {
        get {
            return range;
        }
    }
    public float time;
    public bool IsSame(CurveAsset other) {
        if(this.range != other.range) {
            return false;
        }
        if(this.Curve.keys != other.Curve.keys)
            return false;
        return true;
    }
    public CurveAsset Copy() {
        var curveAsset = new CurveAsset();
        curveAsset.keys = this.keys;
        curveAsset._curve = this._curve;
        curveAsset.range = this.range;
        return curveAsset;
    }
}
