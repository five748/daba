using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Collections;
using Table;
using Unity.VisualScripting;

public class JoystickController : MonoBehaviour
{
    //摇杆半径
    private float joyRadius;
    private RectTransform joyCenter;
    //摇杆外围
    private RectTransform joyRange;
    //private RectTransform joyLight;
    [Range(0, 1)]
    public float gravitySensitivity = 0.3f;
    //摇杆外围起始位置
    private Vector2 joyRangeBeginPos;
    //摇杆中心起始位置
    private Vector2 joyCenterBeginPos;
    public static JoystickController Instance;
    public static System.Action dragBegin;
    public static System.Action<Vector2, float> moveEvent;
    public static System.Action dragEnd;
    public static System.Func<bool> LockEvent;
    public void Awake()
    {
        Instance = this;
        //获取圆的半径
        joyRange = this.transform.GetChild(0).GetComponent<RectTransform>();
        joyRadius = joyRange.rect.size.x * 0.27f;
        joyCenter = joyRange.GetChild(1).GetComponent<RectTransform>();
        //joyLight = joyRange.GetChild(0).GetComponent<RectTransform>();
    }
    public bool isBeginDrag = false;
    private Vector2 oldPos;
    private bool _isClickDown = false;
    public bool isPause = false;
    public bool isCickDown
    {
        get
        {
            return _isClickDown;
        }
        set
        {
            _isClickDown = value;
            if (_isClickDown == false)
            {
                isBeginDrag = false;
            }
        }
    }
    public void Update()
    {
        if (isPause) {
            return;
        }
        if (EventTriggerListener.IsLock)
        {
            return;
        }
        if (EventTriggerListener.IsFollowing)
        {
            return;
        }
        if (EventSystem.current == null)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (LockEvent != null)
            {
                if (LockEvent())
                {
                    return;
                }
            }
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            oldPos = eventData.position;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);
            isCickDown = true;
        }
        if (isCickDown && !isBeginDrag && !ProssData.Instance.isDragScroll)
        {
            //Debug.LogError("ClickEmpty");
            if (Vector2.Distance(oldPos, Input.mousePosition) > 1)
            {
                isBeginDrag = true;
                OnBeginDrag();
            }
        }

        if (isBeginDrag && !ProssData.Instance.isDragScroll)
        {
            OnDrag();
        }
        if (Input.GetMouseButtonUp(0))
        {
            ProssData.Instance.isDragScroll = false;
            isCickDown = false;
            OnEndDrag();
        }
    }
    public void OnPointerDown(PointerEventData eventData)//实现接口调用该方法
    {
        Debug.Log(eventData.button);//这里如果用鼠标左键触发就打印life，右键触发就打印right
        Debug.Log(eventData);//你可以通过eventData获取更多信息，这里是打印鼠标的位置(跟Input.mousePosition输出结果是一样的)以及delta(0.0,0.0),不知道是啥...
        Debug.Log(Input.mousePosition);
        Debug.Log(eventData.pointerPressRaycast.gameObject.name);
        Debug.Log("press");
    }
    public void OnBeginDrag()
    {
        Vector2 _posMouse;
        //屏幕坐标和UI坐标的转换
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joyRange, Input.mousePosition, Camera.main, out _posMouse);
        joyRangeBeginPos = _posMouse;
        //将摇杆的中心位置设置为鼠标点击的位置，即动态变化摇杆位置
        joyRange.anchoredPosition = joyRangeBeginPos;
        joyCenter.anchoredPosition = Vector2.zero;
        //记录下当前摇杆拖动的位置
        joyCenterBeginPos = joyCenter.anchoredPosition;
        joyRange.SetActive(true);
        if (dragBegin != null)
            dragBegin();
    }

    public void OnDrag()
    {
        //Debug.LogError("OnDrag");
        Vector2 _posMouse;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joyRange, Input.mousePosition, Camera.main, out _posMouse);
        //拖拽方向
        Vector2 dragDir = _posMouse - joyCenterBeginPos;
        //拖拽距离
        float dis = dragDir.magnitude;

        if (dis <= joyRadius)
        {
            joyCenter.anchoredPosition = _posMouse;
        }
        else
        {
            joyCenter.anchoredPosition = dragDir.normalized * joyRadius;
        }

        var rote = joyRange.GetPointAngle(joyCenter, joyRange);
        if (moveEvent != null)
        {
            moveEvent(joyRange.anchoredPosition, rote);
        }
    }
    private float beginAngle;
    public float GetPointAngleOur(Transform from, Transform to, Transform old)
    {
        Vector3 targetDir = to.position - from.position; // 目标坐标与当前坐标差的向量
        var angle = Vector3.Angle(from.up, targetDir);
        Vector3 cross = Vector3.Cross(from.up, targetDir);
        if (cross.z > 0)
        {
            angle = -angle;
        }
        Debug.LogError("angle:" + angle);
        beginAngle = angle;
        return angle;
    }
    private Coroutine WaitIE;
    private Coroutine WaitFollowIE;
    public void OnEndDrag()
    {
        joyRange.anchoredPosition = joyCenter.anchoredPosition = Vector2.zero;
        joyRange.SetActive(false);
        if (dragEnd != null) {
            dragEnd();
        }
    }
    public void OpenDrag()
    {
        isPause = false;
    }
    public void CloseDrag() {
        if (joyRange == null) {
            return;
        }
        isPause = true;
        ProssData.Instance.isDragScroll = false;
        isCickDown = false;
        isBeginDrag = false;
        if (moveEvent != null)
        {
            moveEvent(Vector2.zero, 0);
        }
        OnEndDrag();
    }
}