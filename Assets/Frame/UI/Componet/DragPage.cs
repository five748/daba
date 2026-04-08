using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragPage : DragSomeOne
{
    private bool isInited = false;
    private RectTransform[] Items = new RectTransform[3];
    private float leftpos;
    private float rightpos;
    private Vector2 diffpos;
    private Vector2 onediff;
    public float Width;
    public float Height;
    private int _onIndexH;
    private int ChangeIndex;
    public int OnIndexH
    {
        get
        {
            return _onIndexH;
        }
        set
        {
            ChangeIndex = value - _onIndexH;
            _onIndexH = value;
        }
    }
    private int _onIndexV;
    public int OnIndexV
    {
        get
        {
            return _onIndexV;
        }
        set
        {
            ChangeIndex = value - _onIndexV;
            _onIndexV = value;
        }
    }
    private int OnIndex
    {
        get
        {
            if (movement == Movement.Horizontal)
            {
                return _onIndexH;
            }
            else
            {
                return _onIndexV;
            }
        }
    }
    private int MaxLen
    {
        get
        {
            if (movement == Movement.Horizontal)
            {
                return MaxLenH;
            }
            else
            {
                return MaxLenV;
            }
        }
    }
    public int MaxLenH;
    public int MaxLenV;
    public System.Action<int, Transform> ShowChangeItem;
    public System.Action<int, Transform> ShowMiddleItem;
    public GameObject left;
    public GameObject right;
    public GameObject up;
    public GameObject down;
    private Transform middleItem;
    public bool isUseMoveSpc = false;
    private RectTransform First;
    private RectTransform Middle;
    private RectTransform Last;
    public void ClickLeft(GameObject button)
    {
        Debug.Log("clickLeft");
        if (OnIndexH == 0)
        {
            return;
        }
        OnIndexH--;
        MoveToNext();
    }
    public void ClickRight(GameObject button)
    {
        Debug.Log("ClickRight");
        if (OnIndexH >= MaxLenH - 1)
        {
            return;
        }
        OnIndexH++;
        MoveToNext();
    }
    public void ClickUp(GameObject button)
    {
        Debug.Log("clickLeft");
        if (OnIndexV == 0)
        {
            return;
        }
        OnIndexV--;
        MoveToNext();
    }
    public void ClickDown(GameObject button)
    {
        Debug.Log("ClickRight");
        if (OnIndexV >= MaxLenV - 1)
        {
            return;
        }
        OnIndexV++;
        MoveToNext();
    }
    private Vector2 pos
    {
        get
        {
            Vector2 _pos = new Vector2(Width * OnIndexH, -Height * OnIndexV);
            _rectRange.x = _pos.x;
            _rectRange.y = _pos.x;
            _rectRange.z = _pos.y;
            _rectRange.w = _pos.y;
            Debug.LogError(_rectRange.w);
            return _pos;
        }
    }
    private void MoveToNext()
    {
        MoveTo(pos, MoveOver);
    }
    private void MoveToFast()
    {

        Target.anchoredPosition = pos;
    }
    private void Start()
    {
        Init(Width, Height, MaxLenH, MaxLenV, 0, 0, null, null);
    }
    public void Init(float width, float height, int maxlenH, int maxlenV, int onIndexH, int onIndexV, System.Action<int, Transform> showMiddleItem, System.Action<int, Transform> showChangeItem)
    {
        isDragPage = true;
        IsElastic = false;
        isInited = true;
        Width = width;
        Height = height;
        MaxLenH = maxlenH;
        MaxLenV = maxlenV;
        OnIndexH = onIndexH;
        OnIndexV = onIndexV;
        ShowMiddleItem = showMiddleItem;
        ShowChangeItem = showChangeItem;

        var items = Target.AddChilds(3);
        for (int i = 0; i < 3; i++)
        {
            Items[i] = items[i].GetComponent<RectTransform>();
        }
        First = Items[0];
        Middle = Items[1];
        Last = Items[2];
        if (left != null)
        {
            EventTriggerListener.Get(left).onClick = ClickLeft;
        }
        if (right != null)
        {
            EventTriggerListener.Get(right).onClick = ClickRight;
        }
        if (up != null)
        {
            EventTriggerListener.Get(up).onClick = ClickUp;
        }
        if (down != null)
        {
            EventTriggerListener.Get(down).onClick = ClickDown;
        }
        diffpos = new Vector2(3 * width, 0);
        onediff = new Vector2(width, 0);
        SetTargetPos(pos);
        DragEndEvent += DragEndCall;
        MoveOver();
        InitShowItem();
        SetDriActive();
    }
    private void InitShowItem()
    {
        if (ShowChangeItem == null)
            return;
        ShowChangeItem(OnIndex, Middle);
        if (OnIndex != 0)
        {
            ShowChangeItem(OnIndex - 1, First);
        }
        if (OnIndex < MaxLen - 1)
        {
            ShowChangeItem(OnIndex + 1, Last);
        }
    }
    private void MoveOver()
    {
        First = Items[OnIndexH % 3];
        Middle = Items[(OnIndexH + 1) % 3];
        Last = Items[(OnIndexH + 2) % 3];
        First.SetActive(OnIndex != 0);
        Last.SetActive(OnIndex < MaxLen - 1);
        First.anchoredPosition = new Vector2(-Width, Height) - Target.anchoredPosition;
        Middle.anchoredPosition = new Vector2(0, 0) - Target.anchoredPosition;
        Last.anchoredPosition = new Vector2(Width, -Height) - Target.anchoredPosition;
        if (ChangeIndex == 0)
        {
            return;
        }
        Transform changeItem = null;
        int middleIndex = 0;
        int changeIndex = 0;
        if (movement == Movement.Horizontal)
        {
            middleIndex = OnIndexH;
            if (ChangeIndex < 0)
            {
                changeItem = First;
                changeIndex = OnIndexH + 2;
            }
            else
            {
                changeItem = Last;
                changeIndex = OnIndexH - 2;
            }
            if (ChangeIndex < 0 || ChangeIndex > MaxLenH - 1)
            {
                SetDriActive();
                return;
            }

        }
        else
        {
            middleIndex = OnIndexV;
            if (ChangeIndex < 0)
            {
                changeItem = First;
                changeIndex = OnIndexV + 2;
            }
            else
            {
                changeItem = Last;
                changeIndex = OnIndexV - 2;
            }
            if (ChangeIndex < 0 || ChangeIndex > MaxLenV - 1)
            {
                SetDriActive();
                return;
            }
        }
        if (ShowMiddleItem != null)
        {
            ShowMiddleItem(middleIndex, middleItem);
        }
        if (ShowChangeItem != null)
        {
            ShowChangeItem(ChangeIndex, changeItem);
        }
        SetDriActive();
    }
    private bool isHaveChange
    {
        get
        {
            return Mathf.Abs(SmoothSpeed) > 100 || Mathf.Abs(moveDistanceDragBeginAndEnd / Width) > 0.05f;
        }
    }
    void DragEndCall()
    {
        if (moveState == MoveDir.rightToLeft)
        {
            if (isHaveChange)
            {
                if (OnIndexH < MaxLenH - 1)
                {
                    OnIndexH++;
                }
            }
        }
        if (moveState == MoveDir.leftToRight)
        {
            if (isHaveChange)
            {
                if (OnIndexH > 0)
                {
                    OnIndexH--;
                }
            }
        }
        if (moveState == MoveDir.upToDown)
        {
            if (isHaveChange)
            {
                if (OnIndexV > 0)
                {
                    OnIndexV--;
                }
            }
        }
        if (moveState == MoveDir.downToUp)
        {
            if (isHaveChange)
            {
                if (OnIndexV < MaxLenV - 1)
                {
                    OnIndexV++;
                }
            }
        }
        if (isHaveChange && isUseMoveSpc)
        {
            MoveToSmooth(pos, MoveOver);
        }
        else
        {
            MoveTo(pos, MoveOver);
        }
        SetDriActive();
    }
    private void SetDriActive()
    {
        if (left)
            left.SetActive(OnIndexH != 0);
        if (right)
            right.SetActive(OnIndexH != MaxLenH - 1);
        if (up)
            up.SetActive(OnIndexV != 0);
        if (down)
            down.SetActive(OnIndexV != MaxLenV - 1);
    }
}