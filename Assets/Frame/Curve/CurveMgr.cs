using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CurveMgr : Single<CurveMgr> {
    //公用运动曲线
    public Dictionary<string, CurveData> Curves = new Dictionary<string, CurveData>();
    public CurveData GetCurveData(string curveMoveName) {
        if(Curves.ContainsKey(curveMoveName))
        {
            return Curves[curveMoveName];
        }
        CurveData cure = CurveData.ReadToFile(curveMoveName);
        Curves.Add(curveMoveName, cure);
        return cure;
    }
    public List<string> GetCommonCurveMoveNames() {
        List<string> names = new List<string>();
        //names.Add("新建");
        CurveData.SavePath.GetAllFileName(null, fileInfo => {
            names.Add(fileInfo.Name.Replace(".bin", ""));
        });
        return names;
    }
    public List<string> GetCommonCurveMoveNamesHaveNew() {
        List<string> names = new List<string>();
        names.Add("新建");
        CurveData.SavePath.GetAllFileName(null, fileInfo => {
            names.Add(fileInfo.Name.Replace(".bin", ""));
        });
        return names;
    }
}
