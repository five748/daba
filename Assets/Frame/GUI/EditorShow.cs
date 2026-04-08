using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
public class EditorShow {
    public static void DrawOtherCurve(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.curveMoveData == null)
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        var otherCurves = trigger.curveMoveData.otherCurves;
        List<string> keys = new List<string>();
        foreach (var item in otherCurves)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < otherCurves.Count; i++)
        {
            GUILayout.BeginHorizontal();
            otherCurves[keys[i]].Curve = EditorGUILayout.CurveField(keys[i] + "曲线", otherCurves[keys[i]].Curve);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Box("曲线变化范围", mySkyle.box);
            otherCurves[keys[i]].range = StyleCenter.ShowFloatTextField(otherCurves[keys[i]].range, mySkyle);
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Box("持续时间", mySkyle.box);
            otherCurves[keys[i]].time = StyleCenter.ShowFloatTextField(otherCurves[keys[i]].time, mySkyle);
            GUILayout.EndHorizontal();
        }
    }
    public static void DrawMoveCurveTime(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("时间长度", mySkyle.box);
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("时间曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawBlackScreenCurveTime(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawHudMove(HUDNode trigger, GUISkin mySkyle, bool isHaveOtherTime = false)
    {
        if (trigger.curveData == null)
        {
            trigger.curveData = new CurveData("xx");
        }
        if (trigger.curveData == null)
        {
            return;
        }
        if (trigger.curveData.sumCurveYS == null)
        {
            trigger.curveData.sumCurveYS = new CurveAsset();
            trigger.curveData.sumCurveYS.range = 1;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("起点X偏移", mySkyle.box);
        trigger.curveData.sumCurveX.range = StyleCenter.ShowFloatTextField(trigger.curveData.sumCurveX.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("起点Y偏移", mySkyle.box);
        trigger.curveData.sumCurveY.range = StyleCenter.ShowFloatTextField(trigger.curveData.sumCurveY.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("终点X", mySkyle.box);
        trigger.curveData.sumCurveYS.range = StyleCenter.ShowFloatTextField(trigger.curveData.sumCurveYS.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Y", mySkyle.box);
        trigger.curveData.sumCurveZ.range = StyleCenter.ShowFloatTextField(trigger.curveData.sumCurveZ.range, mySkyle);
        GUILayout.EndHorizontal();

       
        GUILayout.BeginHorizontal();
        GUILayout.Box("大小", mySkyle.box);
        trigger.curveData.scale = StyleCenter.ShowFloatTextField(trigger.curveData.scale, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("移动时长", mySkyle.box);
        trigger.curveData.time = StyleCenter.ShowFloatTextField(trigger.curveData.time, mySkyle);
        
        GUILayout.EndHorizontal();
        if (isHaveOtherTime) {
            GUILayout.BeginHorizontal();
            GUILayout.Box("进入技能时长", mySkyle.box);
            trigger.curveData.time1 = StyleCenter.ShowFloatTextField(trigger.curveData.time1, mySkyle);
            GUILayout.EndHorizontal();
        }
       
        GUILayout.BeginHorizontal();
        GUILayout.Box("x位移曲线", mySkyle.box);
        trigger.curveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("y位移曲线", mySkyle.box);
        trigger.curveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("透明度", mySkyle.box);
        trigger.curveData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("大小", mySkyle.box);
        trigger.curveData.sumCurveYS.Curve = EditorGUILayout.CurveField(trigger.curveData.sumCurveYS.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawCameraMove(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点X偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveX.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveX.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Y偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveY.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveY.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("移动时长", mySkyle.box);
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("x位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("y位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("大小", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawStoryCamMoveCurve(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点X偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveX.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveX.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Y偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveY.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveY.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点z偏移", mySkyle.box);
        if (trigger.curveMoveData.sumCurveYS == null)
        {
            trigger.curveMoveData.sumCurveYS = new CurveAsset();
        }
        trigger.curveMoveData.sumCurveYS.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveYS.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("视野偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveZ.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("移动时长", mySkyle.box);
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("x位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("y位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.Box("z位移曲线", mySkyle.box);
        if (trigger.curveMoveData.sumCurveYS == null)
        {
            trigger.curveMoveData.sumCurveYS = new CurveAsset();
        }
        trigger.curveMoveData.sumCurveYS.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveYS.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("视野", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawMoveCurve(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点X偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveX.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveX.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Y偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveY.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveY.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("跳跃Y偏移", mySkyle.box);
        if (trigger.curveMoveData.sumCurveYS == null)
        {
            trigger.curveMoveData.sumCurveYS = new CurveAsset();
        }
        trigger.curveMoveData.sumCurveYS.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveYS.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("移动时长", mySkyle.box);
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("x位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("y位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("跳跃", mySkyle.box);
        if (trigger.curveMoveData.sumCurveYS == null)
        {
            trigger.curveMoveData.sumCurveYS = new CurveAsset();
        }
        trigger.curveMoveData.sumCurveYS.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveYS.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("透明度", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawLaserCurve(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("炮口变化时长", mySkyle.box);
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("炮口变化曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("光束等待时长", mySkyle.box);
        trigger.values["linebegin"].floatValue = StyleCenter.ShowFloatTextField(trigger.values["linebegin"].floatValue, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("光束变化时长", mySkyle.box);
        trigger.curveMoveData.time1 = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time1, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("光束变化曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();
        
    }
    public static void DrawStoryMoveCurve(SkillTriggerNode trigger, GUISkin mySkyle, bool isBySpeed = false)
    {
        if (!trigger.isUseCommon && trigger.curveMoveData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveMoveData = new CurveData("xx");
        }
        if (trigger.curveMoveData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点X偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveX.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveX.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Y偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveY.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveY.range, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("终点Z偏移", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.range = StyleCenter.ShowFloatTextField(trigger.curveMoveData.sumCurveZ.range, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (isBySpeed)
        {
            GUILayout.Box("移动速度", mySkyle.box);
        }
        else {
            GUILayout.Box("移动时长", mySkyle.box);
        }
        trigger.curveMoveData.time = StyleCenter.ShowFloatTextField(trigger.curveMoveData.time, mySkyle);
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Box("x位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("y位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveY.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("Z位移曲线", mySkyle.box);
        trigger.curveMoveData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveMoveData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();
    }
    public static void DrawRotaCurve(SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (!trigger.isUseCommon && trigger.curveRotaData == null && !string.IsNullOrEmpty(trigger.values["curveData"].stringValue))
        {
            trigger.curveRotaData = new CurveData("xx");
        }

        if (trigger.curveRotaData == null)
        {
            return;
        }
        GUILayout.BeginHorizontal();
        GUILayout.Box("开始旋转", mySkyle.box);
        trigger.curveRotaData.begin.vec = StyleCenter.ShowVector3Field("", trigger.curveRotaData.begin.vec, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("结束旋转", mySkyle.box);
        trigger.curveRotaData.end.vec = StyleCenter.ShowVector3Field("", trigger.curveRotaData.end.vec, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("旋转时长", mySkyle.box);
        trigger.curveRotaData.time = StyleCenter.ShowFloatTextField(trigger.curveRotaData.time, mySkyle);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("x旋转曲线", mySkyle.box);
        trigger.curveRotaData.sumCurveX.Curve = EditorGUILayout.CurveField(trigger.curveRotaData.sumCurveX.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("y旋转曲线", mySkyle.box);
        trigger.curveRotaData.sumCurveY.Curve = EditorGUILayout.CurveField(trigger.curveRotaData.sumCurveY.Curve);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Box("z旋转曲线", mySkyle.box);
        trigger.curveRotaData.sumCurveZ.Curve = EditorGUILayout.CurveField(trigger.curveRotaData.sumCurveZ.Curve);
        GUILayout.EndHorizontal();
    }
    public static int beinRunStoryTabId;
    public static int beginStoryId;   
    public static void ShowSkillFly(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle) {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            var valuesDefaultkeys = new List<string>(valuesDefault.Keys);
            for (var i = 0; i < valuesDefaultkeys.Count; i++)
            {
                var item = valuesDefault[valuesDefaultkeys[i]];
                if (!trigger.values.ContainsKey(valuesDefaultkeys[i]))
                {
                    trigger.values.Add(valuesDefaultkeys[i], item);
                }
                else
                {
                    trigger.values[valuesDefaultkeys[i]].strs = item.strs;
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        var valueskeys = new List<string>(trigger.values.Keys);
        for (var i = 0; i < valueskeys.Count; i++)
        {
            keys.Add(valueskeys[i]);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "enmu")
                {
                    StyleCenter.ShowMenuButtonLayout(trigger.values[keys[i]].stringValue, trigger.values[keys[i]].strs, ProssData.Instance.guiEvent, mySkyle.box, keys[i], (str, index) =>
                    {
                        trigger.values[index].stringValue = str;
                    });
                }
                else if (keys[i] == "prefabName")
                {
                    if (trigger.go == null && trigger.values["prefabName"].stringValue != "")
                    {
                        AssetLoadOld.Instance.LoadTriggerPrefab(trigger.values["prefabName"].stringValue, (go) =>
                        {
                            trigger.go = go;
                        });
                    }
                    trigger.go = (GameObject)EditorGUILayout.ObjectField(trigger.go, typeof(GameObject), true);
                    trigger.values["prefabName"].stringValue = AssetDatabase.GetAssetPath(trigger.go);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowHeroTigger(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle) {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            var valuesDefaultkey = new List<string>(valuesDefault.Keys);
            for (var i = 0; i < valuesDefaultkey.Count; i++)
            {
                var item = valuesDefault[valuesDefaultkey[i]];
                if (!trigger.values.ContainsKey(valuesDefaultkey[i]))
                {
                    trigger.values.Add(valuesDefaultkey[i], item);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>(trigger.values.Keys);
        //foreach(var item in trigger.values)
        //{
        //    keys.Add(item.Key);
        //}
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowSkillModifyColor(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle) {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            var keys2 = new List<string>(valuesDefault.Keys);
            for (var i = 0; i < keys2.Count; i++)
            {
                var item = valuesDefault[keys2[i]];
                if (!trigger.values.ContainsKey(keys2[i]))
                {
                    trigger.values.Add(keys2[i], item);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>(trigger.values.Keys);
        //foreach(var item in trigger.values)
        //{
        //    keys.Add(item.Key);
        //}
        for (int i = 0; i < keys.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
            if (keys[i].Contains("curveData"))
            {
                if (trigger.curveMoveData == null)
                {
                    trigger.curveMoveData = new CurveData("xx");
                }
                if (trigger.curveMoveData.otherCurves == null)
                {
                    trigger.curveMoveData.otherCurves = new Dictionary<string, CurveAsset>();
                    trigger.curveMoveData.otherCurves.Add("r", new CurveAsset());
                    trigger.curveMoveData.otherCurves.Add("g", new CurveAsset());
                    trigger.curveMoveData.otherCurves.Add("b", new CurveAsset());
                    trigger.curveMoveData.otherCurves.Add("a", new CurveAsset());
                }
                GUILayout.EndHorizontal();
                DrawOtherCurve(trigger, mySkyle);
            }
            else if (trigger.values[keys[i]].typeName == "float")
            {
                trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                GUILayout.EndHorizontal();
            }
            else if (trigger.values[keys[i]].typeName == "bool")
            {
                trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                GUILayout.EndHorizontal();
            }
            else
            {
                trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                GUILayout.EndHorizontal();
            }

        }
    }
    public static void ShowBlackScTrigger(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            var valuesDefaultkeys = new List<string>(valuesDefault.Keys);
            for (var i = 0; i < valuesDefaultkeys.Count; i++)
            {
                var item = valuesDefault[valuesDefaultkeys[i]];
                if (!trigger.values.ContainsKey(valuesDefaultkeys[i]))
                {
                    trigger.values.Add(valuesDefaultkeys[i], item);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        var valueskeys = new List<string>(trigger.values.Keys);
        for (var i = 0; i < valueskeys.Count; i++)
        {
            var item = trigger.values[valueskeys[i]];
            keys.Add(valueskeys[i]);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                EditorShow.DrawBlackScreenCurveTime(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowSkillMoveTo(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
                else
                {
                    trigger.values[item.Key].strs = item.Value.strs;
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "enmu")
                {
                    StyleCenter.ShowMenuButtonLayout(trigger.values[keys[i]].stringValue, trigger.values[keys[i]].strs, ProssData.Instance.guiEvent, mySkyle.box, keys[i], (str, index) =>
                    {
                        trigger.values[index].stringValue = str;
                    });
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowSkillCameraMoveTo(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
                else
                {
                    trigger.values[item.Key].strs = item.Value.strs;
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawCameraMove(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "enmu")
                {
                    StyleCenter.ShowMenuButtonLayout(trigger.values[keys[i]].stringValue, trigger.values[keys[i]].strs, ProssData.Instance.guiEvent, mySkyle.box, keys[i], (str, index) =>
                    {
                        trigger.values[index].stringValue = str;
                    });
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowTimeChange(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurveTime(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowTiming(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowSkillUseTarget(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "int")
                {
                    trigger.values[keys[i]].intValue = StyleCenter.ShowIntTextField(trigger.values[keys[i]].intValue, mySkyle);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    public static void ShowCameraMove(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }
        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawStoryCamMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "enmu")
                {
                    StyleCenter.ShowMenuButtonLayout(trigger.values[keys[i]].stringValue, trigger.values[keys[i]].strs, ProssData.Instance.guiEvent, mySkyle.box, keys[i], (str, index) =>
                    {
                        trigger.values[index].stringValue = str;
                    });
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
    
    public static void ShowOptions(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle)
    {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault.CopyValues();
        }
        else
        {
            foreach (var item in valuesDefault)
            {
                if (!trigger.values.ContainsKey(item.Key))
                {
                    trigger.values.Add(item.Key, item.Value.Copy()); ;
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        foreach (var item in trigger.values)
        {
            keys.Add(item.Key);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else
                {
                    trigger.values[keys[i]].stringValue = EditorGUILayout.TextField(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
     public static void ShowStoryBackTo(Dictionary<string, TriggerTen> valuesDefault, SkillTriggerNode trigger, GUISkin mySkyle, System.Action click) {
        if (trigger.values == null || trigger.values.Count == 0)
        {
            trigger.values = valuesDefault;
        }
        else
        {
            var valuesDefaultkeys = new List<string>(valuesDefault.Keys);
            for (var i = 0; i < valuesDefaultkeys.Count; i++)
            {
                var item = valuesDefault[valuesDefaultkeys[i]];
                if (!trigger.values.ContainsKey(valuesDefaultkeys[i]))
                {
                    trigger.values.Add(valuesDefaultkeys[i], item);
                }
            }
        }
        GUI.DragWindow(new Rect(0, 0, 150, 20));
        List<string> keys = new List<string>();
        var valueskeys = new List<string>(trigger.values.Keys);
        for (var i = 0; i < valueskeys.Count; i++)
        {
            var item = trigger.values[valueskeys[i]];
            keys.Add(valueskeys[i]);
        }

        for (int i = 0; i < keys.Count; i++)
        {
            if (keys[i].Contains("curveData"))
            {
                EditorShow.DrawMoveCurve(trigger, mySkyle);
            }
            else
            {
                GUILayout.BeginHorizontal();
                GUILayout.Box(trigger.values[keys[i]].chinese, mySkyle.box);
                if (trigger.values[keys[i]].typeName == "float")
                {
                    trigger.values[keys[i]].floatValue = StyleCenter.ShowFloatTextField(trigger.values[keys[i]].floatValue, mySkyle);
                }
                else if (trigger.values[keys[i]].typeName == "bool")
                {
                    trigger.values[keys[i]].boolValue = GUILayout.Toggle(trigger.values[keys[i]].boolValue, keys[i]);
                }
                else if (trigger.values[keys[i]].typeName == "enmu")
                {
                    StyleCenter.ShowMenuButtonLayout(trigger.values[keys[i]].stringValue, trigger.values[keys[i]].strs, ProssData.Instance.guiEvent, mySkyle.box, keys[i], (str, index) =>
                    {
                        trigger.values[index].stringValue = str;
                    });
                }
                else
                {
                    trigger.values[keys[i]].stringValue = GUILayout.TextArea(trigger.values[keys[i]].stringValue, mySkyle.textArea);
                }
                GUILayout.EndHorizontal();
            }
        }
    }
}
#endif