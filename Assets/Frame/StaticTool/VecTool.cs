using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public static class VecTool
{
    public static void ChangeSceneTranPos(this Transform item, Vector3 pos)
    {
        item.localPosition = new Vector3(pos.x, 0, pos.z);
        item.localRotation = Quaternion.Euler(pos.y, 0, 0);
    }
    public static Vector2 GetAnchoredPositionByLocalPosition(this Transform item, Transform parent)
    {

        var rect = item.GetComponent<RectTransform>();
        var parentRect = parent.GetComponent<RectTransform>();

        Vector2 localPosition2D = new Vector2(rect.localPosition.x, rect.localPosition.y);

        Vector2 anchorMinPos = parentRect.rect.min + Vector2.Scale(rect.anchorMin, parentRect.rect.size);
        Vector2 rectMinPos = rect.rect.min + localPosition2D;
        Vector2 offsetMin = rectMinPos - anchorMinPos;

        Vector2 anchorMaxPos = parentRect.rect.max - Vector2.Scale(Vector2.one - rect.anchorMax, parentRect.rect.size);
        Vector2 rectMaxPos = rect.rect.max + localPosition2D;
        Vector2 offsetMax = rectMaxPos - anchorMaxPos;

        Vector2 sizeDelta = offsetMax - offsetMin;

        Vector2 anchoredPosition = offsetMin + Vector2.Scale(sizeDelta, rect.pivot);

        return anchoredPosition;
    }
    public static float GetAngle(this Transform from, Transform to)
    {
        float angle;
        Vector3 cross = Vector3.Cross(from.up, to.up);
        angle = Vector2.Angle(from.up, to.up);
        //Debug.LogError("偏移:" + angle);
        return cross.z > 0 ? angle : -angle;
    }
    public static float GetPointAngle(this Transform from, Transform to, Transform father) {
        Vector3 targetDir = to.position - from.position; // 目标坐标与当前坐标差的向量
        var angle = Vector3.Angle(father.up, targetDir);
        Vector3 cross = Vector3.Cross(father.up, targetDir);
        if (cross.z > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }
    public static float GetPointAngle(this Transform from, Vector3 to, Vector3 up)
    {
        Vector3 targetDir = to - from.position; // 目标坐标与当前坐标差的向量
        var angle = Vector3.Angle(up, targetDir);
        Vector3 cross = Vector3.Cross(up, targetDir);
        if (cross.z > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }
    public static float GetPointAngle(this Transform from, Vector3 to, Transform father)
    {
        Vector3 targetDir = to - from.position; // 目标坐标与当前坐标差的向量
        var angle = Vector3.Angle(father.up, targetDir);
        Vector3 cross = Vector3.Cross(father.up, targetDir);
        if (cross.z > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }
    public static float GetPointAngle(this Vector2 beginpos, Vector2 topos)
    {
        Vector2 targetDir = topos - beginpos; // 目标坐标与当前坐标差的向量
        var angle = Vector2.Angle(Vector2.up, targetDir);
        Vector3 cross = Vector3.Cross(Vector2.up, targetDir);
        if (cross.z > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }
    public static float GetPointAngle(this Vector3 beginpos, Vector3 topos)
    {
        Vector2 targetDir = topos - beginpos; // 目标坐标与当前坐标差的向量
        var angle = Vector2.Angle(Vector2.up, targetDir);
        Vector3 cross = Vector3.Cross(Vector2.up, targetDir);
        if (cross.z > 0)
        {
            return angle;
        }
        else
        {
            return -angle;
        }
    }


    public static Transform GetNearTran(this Transform me, List<Transform> trans) {
        Transform near = null;
        float minDis = float.MaxValue;
        Vector2 mePos = me.position;
        float dis;
        for (int i = 0; i < trans.Count; i++)
        {
            dis = Vector2.Distance(mePos, trans[i].position);
            if (dis < minDis) {
                minDis = dis;
                near = trans[i];
            }
        }
        return near;
    }
    public static Vector2 GetCircle(this float r, float angle) {
       var v = angle * Mathf.PI / 180;
       return new Vector2(Mathf.Cos(v) * r, Mathf.Sin(v) * r);
    }
    public static bool isNear(this Transform a, bool isHood, Transform b, float checkRange, float beginCheck, float scale)
    {
        if (a == null || b == null) {
            return false;
        }
        if (isHood)
        {
            var nearModify = 300f * scale;
            if (Mathf.Abs(a.position.x - b.position.x) > nearModify && Mathf.Abs(a.position.y - b.position.y) > nearModify)
            {
                return false;
            }
            return Vector2.Distance(a.position, b.position) < nearModify;
        }
        else
        {
            if (Mathf.Abs(a.position.x - b.position.x) > beginCheck && Mathf.Abs(a.position.y - b.position.y) > beginCheck)
            {
                return false;
            }
            return Vector2.Distance(a.position, b.position) < checkRange;
        }

    }
    public static Vector2 GetCirclePoint(this Vector2 point, float r, float angle)
    {
        float x = point.x + r * Mathf.Cos(angle * 3.14f/180);
        float y = point.y + r * Mathf.Sin(angle * 3.14f/180);
        return new Vector2(x, y);
    }
}
