using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RangeTool
{
    public static bool isOutScreen(this Transform tran, Camera cam)
    {
        Vector3 value = cam.WorldToScreenPoint(tran.position);
        return value.x > Screen.width || value.x < 0 || value.y < 0 || value.y > Screen.height;
    }
    public static bool isOutBigScreen(this Transform tran, Camera cam)
    {
        Vector3 value = cam.WorldToScreenPoint(tran.position);
        return value.x > ProssData.Instance.ScreenBigWH.x || value.x < 0 || value.y < 0 || value.y > ProssData.Instance.ScreenBigWH.y;
    }
    public static bool isOutSmallScreen(this Transform tran, Camera cam)
    {
        Vector3 value = cam.WorldToScreenPoint(tran.position);
        return value.x > ProssData.Instance.ScreenSmallWH.x || value.x < 0 || value.y < 0 || value.y > ProssData.Instance.ScreenSmallWH.y;
    }
}
