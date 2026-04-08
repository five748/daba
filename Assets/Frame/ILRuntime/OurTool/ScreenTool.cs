using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.UI;
using System;

public class ScreenTool
{
    public static float changeY;
    public static float DefaultWidth;
    /// <summary>
    /// 开发屏幕的宽
    /// </summary>
    public static float DevelopMaxWidth = 1624f;
    /// <summary>
    /// 开发屏幕的宽
    /// </summary>
    public static float DevelopMinWidth = 1224f;

    /// <summary>
    /// 开发屏幕的长
    /// </summary>
    public static float DevelopHeigh = 750.0f;

    /// <summary>
    /// 开发高宽比
    /// </summary>
    public static float DevelopRate;

    /// <summary>
    /// 设备自身的高
    /// </summary>
    public static int curScreenHeight;

    /// <summary>
    /// 设备自身的高
    /// </summary>
    public static int curScreenWidth;

    /// <summary>
    /// 当前屏幕高宽比
    /// </summary>
    public static float ScreenRate;
    /// <summary>
    /// 世界摄像机rect高的比例
    /// </summary>
    public static float cameraRectHeightRate;

    /// <summary>
    /// 世界摄像机rect宽的比例
    /// </summary>
    public static float cameraRectWidthRate;
    public static bool isOutMaxScreen;
    public static bool isOutMinScreen;
    public static Rect camRect;
    public static bool isInited;
    private static Dictionary<string, float> deviceAdapter = new Dictionary<string, float>();
    public static void Init()
    {
        return;
//        if (isInited)
//        {
//            FitCamera();
//            return;
//        }
//#if !UNITY_IOS
//        if (deviceAdapter.ContainsKey(SystemInfo.deviceModel))
//        {
//            DevelopMaxWidth = deviceAdapter[SystemInfo.deviceModel];
//        }
//#endif
//        isInited = true;
//        DevelopRate = DevelopHeigh / DevelopMaxWidth;
//        curScreenHeight = Screen.height;
//        curScreenWidth = Screen.width;
//        ScreenRate = (float)Screen.height / (float)Screen.width;
//        cameraRectHeightRate = DevelopHeigh / ((DevelopMinWidth / Screen.width) * Screen.height);
//        cameraRectWidthRate = DevelopMaxWidth / ((DevelopHeigh / Screen.height) * Screen.width);
//        isOutMaxScreen = DevelopRate > ScreenRate;
//        isOutMinScreen = (DevelopHeigh / DevelopMinWidth) < ScreenRate;
//        //Debug.LogError("isOutSmallScreen:" + isOutMinScreen);
//        camRect = new Rect((1 - cameraRectWidthRate) / 2, 0, cameraRectWidthRate, 1);
//        if (isOutMinScreen)
//        {
//            camRect = new Rect(0, (1 - cameraRectHeightRate) / 2, 1, cameraRectHeightRate);
//        }
//        FitCamera();
    }
    public static void ReSetScreen()
    {
        return;
        //if (isOutMinScreen)
        //{
        //    return;
        //}
        //DevelopRate = DevelopHeigh / DevelopMaxWidth;
        //curScreenHeight = Screen.height;
        //curScreenWidth = Screen.width;
        //ScreenRate = (float)Screen.height / (float)Screen.width;
        //cameraRectHeightRate = DevelopHeigh / ((DevelopMaxWidth / Screen.width) * Screen.height);
        //cameraRectWidthRate = DevelopMaxWidth / ((DevelopHeigh / Screen.height) * Screen.width);
        //isOutMaxScreen = true;
        //camRect = new Rect((1 - cameraRectWidthRate) / 2, 0, cameraRectWidthRate, 1);
        //FitCamera();
        //ProssData.Instance.CanvasSize = new Vector2(DevelopMaxWidth, ProssData.Instance.CanvasSize.y);
    }
    public static void FitCamera()
    {
        return;
        //if (isOutMinScreen)
        //{
        //    var uicanvas = GameObject.Find("UICanvas");
        //    if (uicanvas)
        //    {
        //        if (!uicanvas.GetComponent<CanvasScaler>())
        //        {
        //            Debug.LogError(uicanvas + "找不到CanvasScaler");
        //            return;
        //        }
        //        uicanvas.GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        //    }
        //    foreach (var item in Camera.allCameras)
        //    {
        //        if (item.name != "blackcam")
        //            item.rect = camRect;
        //    }
        //    return;
        //}
        //if (isOutMaxScreen)
        //{
        //    foreach (var item in Camera.allCameras)
        //    {
        //        if (item.name != "blackcam")
        //            item.rect = camRect;
        //    }
        //    //GL.Clear(true, true, Color.black);
        //}
    }
    public void FixCamera()
    {
        //int ManualWidth = 1080;   //首先记录下你想要的屏幕分辨率的宽
        //int ManualHeight = 1920;   //记录下你想要的屏幕分辨率的高        //普通安卓的都是 1280*720的分辨率
        //int manualHeight;

        ////然后得到当前屏幕的高宽比 和 你自定义需求的高宽比。通过判断他们的大小，来不同赋值
        ////*其中Convert.ToSingle（）和 Convert.ToFloat() 来取得一个int类型的单精度浮点数（C#中没有 Convert.ToFloat() ）；
        //if (Convert.ToSingle(Screen.height) / Screen.width > Convert.ToSingle(ManualHeight) / ManualWidth)
        //{
        //    //如果屏幕的高宽比大于自定义的高宽比 。则通过公式  ManualWidth * manualHeight = Screen.width * Screen.height；
        //    //来求得适应的  manualHeight ，用它待求出 实际高度与理想高度的比率 scale
        //    manualHeight = Mathf.RoundToInt(Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
        //}
        //else
        //{   //否则 直接给manualHeight 自定义的 ManualHeight的值，那么相机的fieldOfView就会原封不动
        //    manualHeight = ManualHeight;
        //}

        //Camera camera = Camera.main;
        //float scale = Convert.ToSingle(manualHeight * 1.0f / ManualHeight);
        //camera.fieldOfView *= scale;                      //Camera.fieldOfView 视野:  这是垂直视野：水平FOV取决于视口的宽高比，当相机是正交时fieldofView被忽略
        ////把实际高度与理想高度的比率 scale乘加给Camera.fieldOfView。
        ////这样就能达到，屏幕自动调节分辨率的效果
    }
}
