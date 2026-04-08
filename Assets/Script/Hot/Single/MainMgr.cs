
using System;
using SelfComponent;
using UnityEngine;

public class MainMgr : Singleton<MainMgr>
{
    public Action<Transform, Transform, float, bool, Action<Action>> MoveScaleAnimation;
    public Action<int> updateZhaoShang;
    public Action openNavigate;
    public CameraMove ComCameraMove;
    public Action FunMoveToNavigate;
}