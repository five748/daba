using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScreenSide : Single<ScreenSide>
{
    private Dictionary<float, float> angleDic;
    private Vector2 BeginPos;
    private float pi;
    private bool isInited;
    private List<float> angles;
    private float topModify = 800;
    private float downModify = 200;
    private Vector2 screenPos;

    public void Init(Vector2 _beginPos)
    {
        isInited = true;
        pi = 3.14f / 180;
        BeginPos = _beginPos;
        angleDic = new Dictionary<float, float>();
        angles = new List<float>();
        var midify = 1080.0f / Screen.width;
        screenPos = new Vector2(Screen.width, Screen.height) * midify;
        Vector2 leftTopPos = new Vector2(-screenPos.x, screenPos.y) * 0.5f - new Vector2(0, topModify);
        Vector2 leftDownPos = new Vector2(-screenPos.x, -screenPos.y) * 0.5f - new Vector2(0, -downModify);
        Vector2 rightDownPos = new Vector2(screenPos.x, -screenPos.y) * 0.5f - new Vector2(0, -downModify);
        Vector2 rightTopPos = new Vector2(screenPos.x, screenPos.y) * 0.5f - new Vector2(0, topModify);

        angles.Add(_beginPos.GetPointAngle(leftTopPos));
        angles.Add(_beginPos.GetPointAngle(leftDownPos));
        angles.Add(360 + _beginPos.GetPointAngle(rightDownPos));
        angles.Add(360 + _beginPos.GetPointAngle(rightTopPos));
        angles.Add(360);

        angleDic.Add(angles[0], screenPos.y * 0.5f - _beginPos.y - topModify);
        angleDic.Add(angles[1], screenPos.x * 0.5f + _beginPos.x);
        angleDic.Add(angles[2], screenPos.y * 0.5f + _beginPos.y - downModify);
        angleDic.Add(angles[3], screenPos.x * 0.5f - _beginPos.x);
        angleDic.Add(angles[4], screenPos.y * 0.5f - _beginPos.y - topModify);
    }
    public void SetDir(Transform dir, RectTransform child, Vector2 endPos, Transform guideLeft, Transform guideRight, Transform playerMove)
    {
        if (dir == null) {
            return;
        }
        if (!isInited)
        {
            return;
        }
        var angle = playerMove.position.GetPointAngle(endPos);
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        //Debug.LogError("angle:" + angle);
        dir.localRotation = Quaternion.Euler(0, 0, angle);
        guideLeft.parent.localRotation = Quaternion.Euler(0, 0, -angle);
        //guideRight.localRotation = Quaternion.Euler(0, 0, -angle);
        float l = 0;
        foreach (var angleOne in angleDic)
        {
            if (angle < angleOne.Key)
            {
                l = angleOne.Value;
                break;
            }
        }
        var r = l / (float)Math.Cos(moidfyAngle(angle) * pi);
        child.anchoredPosition = new Vector3(0, r - 50, 0);
    }
    public void SetDir(Transform dir, RectTransform child, Vector2 endPos)
    {
        if (!isInited)
        {
            return;
        }
        var angle = BeginPos.GetPointAngle(endPos);
        if (angle < 0)
        {
            angle = 360 + angle;
        }
        //Debug.LogError("angle:" + angle);
        dir.localRotation = Quaternion.Euler(0, 0, angle);
        float l = 0;
        foreach (var angleOne in angleDic)
        {
            if (angle < angleOne.Key)
            {
                l = angleOne.Value;
                break;
            }
        }
        var r = l / (float)Math.Cos(moidfyAngle(angle) * pi);
        child.anchoredPosition = new Vector3(0, r - 50, 0);
    }
    private float moidfyAngle(float angle)
    {
        if (angle < angles[0])
        {
            return angle;
        }
        if (angle < 90f)
        {
            return 90f - angle;
        }
        if (angle < angles[1])
        {
            return angle - 90;
        }
        if (angle < 180f)
        {
            return 180f - angle;
        }
        if (angle < angles[2])
        {
            return angle - 180;
        }
        if (angle < 270)
        {
            return 270 - angle;
        }
        if (angle < angles[3])
        {
            return angle - 270;
        }
        return 360 - angle;
    }

}
