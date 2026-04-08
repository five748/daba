using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragScroll : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private bool isForward = true;
    public enum Movement
    {
        Horizontal,
        Vertical,
    }
    public RectTransform Target;
    protected Vector2 TargetPos;
    public Movement movement;
    public bool isDraging = false;
    private bool isSmooth = false;
    private float begin;
    private float end;
    private float beginTime;
    public float SmoothNeedTime = 0.3f;
    public float SmoothSpeed = 0;
    private float DragSpeed = 0;
    private Vector3 DragVelocity = Vector3.zero;
    private  bool IsElastic = false;
    protected System.Action BeginMoveElasticEvent;
    protected System.Action MoveElasticEvent;
    private bool isBeginMove = false;
    public System.Action MoveEnd;
    public System.Action MoveNextEnd;
    public System.Action MoveBegin;
    //private Scroll
    public float _oldSmoothTime = 0.96f;
    public bool IsForward
    {
        get
        {
            return isForward;
        }

        set
        {
            isForward = value;
        }
    }

    private float SmoothModify = 0;
    public float _maxLeft = -10000;
    public float MaxLeft
    {
        get
        {
            return _maxLeft;
        }
        set
        {
            _maxLeft = value;
            //print(_maxLeft);
        }
    }
    public float MaxRight = 0;
    public float scrollleft;
    public float scrollright;
    public float oldMaxRight;
    public void OnPress()
    {
        TargetPos = Target.anchoredPosition;
        DragVelocity = Vector2.zero;
        SmoothSpeed = 0;
    }
    public void SetOldTargetPos(Vector2 targetPos) {
        OnPress();
        IsForward = Target.anchoredPosition.y < targetPos.y;
        Target.anchoredPosition = targetPos;
    }
    protected void OnDragStart()
    {
        isCanUpdate = true;
        if (isAuto)
            return;
        //print("OnDragStart");
        isBeginMove = false;
        isDraging = true;
        DragVelocity = Vector3.zero;
        TargetPos = Target.anchoredPosition;
        SmoothSpeed = 0;
        if (movement == Movement.Horizontal)
        {
            begin = Target.anchoredPosition.x;
        }
        else
        {
            begin = Target.anchoredPosition.y;
        }
        beginTime = Time.time;
    }
    public Vector2 BaseTargetPos;
    protected bool isMoveOut = false;
    protected bool isMoveChange = false;
    protected void OnDrag(Vector2 delta)
    {
        if (isAuto)
            return;
        if (movement == Movement.Horizontal)
        {
            if (delta.x != 0)
                IsForward = delta.x < 0;
            delta = new Vector2(delta.x, 0);
            if (TargetPos.x >= MaxRight && delta.x > 0)
            {
                if (IsElastic)
                {
                    Double diff = TargetPos.x - MaxRight;
                    if (diff < 20)
                    {
                        diff = 20;
                    }
                    TargetPos += delta * (10.0f / (float)diff);
                    BaseTargetPos = new Vector2((float)MaxRight, TargetPos.y);
                    isMoveOut = true;
                    isMoveChange = true;
                    if (isMoveOut)
                    {
                        if (!isBeginMove)
                        {
                            isBeginMove = true;
                            if (BeginMoveElasticEvent != null)
                            {
                                BeginMoveElasticEvent();
                            }
                        }
                    }
                }
                else
                {
                    DragVelocity = Vector3.zero;
                    TargetPos.x = (float)MaxRight;
                    isMoveChange = true;
                }
                return;
            }
            if (TargetPos.x <= MaxLeft && delta.x < 0)
            {
                if (IsElastic)
                {
                    float diff = (float)MaxLeft - TargetPos.x;
                    if (diff < 10)
                    {
                        diff = 10;
                    }
                    TargetPos += delta * (10.0f / diff);
                    BaseTargetPos = new Vector2((float)MaxLeft, TargetPos.y);
                    isMoveOut = true;
                    isMoveChange = true;
                    if (isMoveOut)
                    {
                        if (!isBeginMove)
                        {
                            isBeginMove = true;
                            if (BeginMoveElasticEvent != null)
                            {
                                BeginMoveElasticEvent();
                            }
                        }
                    }
                }
                else
                {
                    DragVelocity = Vector3.zero;
                    TargetPos.x = (float)MaxLeft;
                }
                return;
            }
        }
        else
        {
            if (delta.y != 0)
                IsForward = delta.y > 0;
            delta = new Vector2(0, delta.y);
            if (TargetPos.y >= MaxRight && delta.y > 0)
            {
                if (IsElastic)
                {
                    float diff = TargetPos.y - (float)MaxRight;
                    if (diff < 20)
                    {
                        diff = 20;
                    }
                    TargetPos += delta * (10.0f / diff);
                    BaseTargetPos = new Vector2(TargetPos.x, (float)MaxRight);
                    isMoveOut = true;
                    isMoveChange = true;
                    if (isMoveOut)
                    {
                        if (!isBeginMove)
                        {
                            isBeginMove = true;
                            if (BeginMoveElasticEvent != null)
                            {
                                BeginMoveElasticEvent();
                            }
                        }
                    }
                }
                else
                {
                    DragVelocity = Vector3.zero;
                    TargetPos.y = (float)MaxRight;
                }
                return;
            }
            if (TargetPos.y <= MaxLeft && delta.y < 0)
            {
                if (IsElastic)
                {
                    float diff = (float)MaxLeft - TargetPos.y;
                    if (diff < 10)
                    {
                        diff = 10;
                    }
                    TargetPos += delta * (10.0f / diff);
                    BaseTargetPos = new Vector2(TargetPos.x, (float)MaxLeft);
                    isMoveOut = true;
                    isMoveChange = true;
                    if (isMoveOut)
                    {
                        if (!isBeginMove)
                        {
                            isBeginMove = true;
                            if (BeginMoveElasticEvent != null)
                            {
                                BeginMoveElasticEvent();
                            }
                        }
                    }
                }
                else
                {
                    DragVelocity = Vector3.zero;
                    TargetPos.y = (float)MaxLeft;
                }
                return;
            }
        }
        TargetPos += delta;
    }
    protected void OnDragEnd()
    {
        if (isMoveOut)
        {
            isMoveChange = false;
            MoveToBase();
        }
        //print("dragEnd");
        isDraging = false;
        if (movement == Movement.Horizontal)
        {
            end = Target.anchoredPosition.x;
        }
        else
        {
            end = Target.anchoredPosition.y;
        }
        float useTime = Time.time - beginTime;
        DragSpeed = (end - begin) / useTime;
        //Debug.Log(useTime);
        if (useTime < SmoothNeedTime)
        {
            isSmooth = true;
            SmoothSpeed = DragVelocity.x + DragSpeed;
            SmoothModify = _oldSmoothTime + (1 - _oldSmoothTime) * ((SmoothNeedTime - useTime) / SmoothNeedTime);
            //SetTargetPos();
        }
        else
        {
            isSmooth = false;
        }
        if (MoveEnd != null) {
            MoveEnd();
        }
    }
    public float SmoothTime = 0;
    private void SetTargetPos()
    {
        float sum = 0;
        int index = 0;
        while (true)
        {
            index++;
            if (Mathf.Abs(SmoothSpeed) > 50 && index < 1000)
            {
                SmoothSpeed *= SmoothModify;
                sum += SmoothSpeed * 0.02f;
            }
            else
            {
                if (movement == Movement.Horizontal)
                    TargetPos += new Vector2(sum, 0);
                else
                    TargetPos += new Vector2(0, sum);
                SmoothTime = sum / DragVelocity.x;
                break;
            }
        }
    }
    public bool isCanUpdate = false;
    public bool isMoveToNext = false;
    public void LateUpdate()
    {
        if (!isCanUpdate)
        {
            return;
        }

        if (isDraging)
        {
            //print(TargetPos);
            Target.anchoredPosition = Vector3.SmoothDamp(Target.anchoredPosition, TargetPos, ref DragVelocity, 0.01f, 7200, Time.deltaTime);
        }
        else
        {
            if (isMoveToNext)
            {
                if (movement == Movement.Horizontal)
                {
                    //Target.localanchoredPosition = Vector3.SmoothDamp(Target.localanchoredPosition, TargetPos, ref DragVelocity, SmoothTime, 7200, Time.deltaTime);
                    if (Mathf.Abs(SmoothSpeed) > 50)
                    {
                        SmoothSpeed *= SmoothModify;
                        Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
                    }
                    else {
                        if(isForward)
                            SmoothSpeed = -50;
                        else
                            SmoothSpeed = 50;
                        Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(SmoothSpeed) > 50)
                    {
                        SmoothSpeed *= SmoothModify;
                        Target.anchoredPosition += new Vector2(0, SmoothSpeed * 0.02f);
                    }
                    else {
                        if (isForward)
                            SmoothSpeed = -50;
                        else
                            SmoothSpeed = 50;
                        Target.anchoredPosition += new Vector2(0, SmoothSpeed * 0.02f);
                    }
                }
            }
            else {
                if (isMoveOut)
                {
                    return;
                }
                if (movement == Movement.Horizontal)
                {
                    //Target.localanchoredPosition = Vector3.SmoothDamp(Target.localanchoredPosition, TargetPos, ref DragVelocity, SmoothTime, 7200, Time.deltaTime);
                    if (Mathf.Abs(SmoothSpeed) > 24)
                    {
                        SmoothSpeed *= SmoothModify;
                        Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
                    }
                }
                else
                {
                    if (Mathf.Abs(SmoothSpeed) > 24)
                    {
                        SmoothSpeed *= SmoothModify;
                        Target.anchoredPosition += new Vector2(0, SmoothSpeed * 0.02f);
                    }
                }
            }
        }
        if (isMoveOut)
            return;
        if (movement == Movement.Horizontal)
        {
            if (Target.anchoredPosition.x >= MaxRight)
            {
                //Debug.LogError(MaxRight);
                DragVelocity = Vector2.zero;
                Target.anchoredPosition = new Vector3((float)MaxRight, Target.anchoredPosition.y);
                MoveNextOver();
            }
            if (Target.anchoredPosition.x <= MaxLeft)
            {
                //Debug.LogError(MaxLeft);
                DragVelocity = Vector2.zero;
                Target.anchoredPosition = new Vector3((float)MaxLeft, Target.anchoredPosition.y);
                MoveNextOver();
            }
        }
        else
        {
            if (Target.anchoredPosition.y >= MaxRight)
            {
                //isMoveOut = true;
                DragVelocity = Vector2.zero;
                Target.anchoredPosition = new Vector3(Target.anchoredPosition.x, (float)MaxRight);
                MoveNextOver();
            }
            if (Target.anchoredPosition.y <= MaxLeft)
            {
                //isMoveOut = true;
                DragVelocity = Vector2.zero;
                Target.anchoredPosition = new Vector3(Target.anchoredPosition.x, (float)MaxLeft);
                MoveNextOver();
            }
        }
    }
    private void MoveNextOver() {
        if (!isMoveToNext) {
            return;
        }
        isMoveToNext = false;
        SmoothSpeed = 0;
        if (MoveNextEnd != null)
        {
            MoveNextEnd();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //print("onDrag");
        OnDrag(eventData.delta);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (MoveBegin != null) {
            MoveBegin();
        }
        OnDragStart();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnDragEnd();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnPress();
    }
    public void OnPointerUp(PointerEventData eventData)
    {

    }
    public void MoveToNext() {
        if (Mathf.Abs(SmoothSpeed) > 24)
        {
            SmoothSpeed *= SmoothModify;
            Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
        }
        if (Target.anchoredPosition.x >= MaxRight)
        {

            DragVelocity = Vector2.zero;
            Target.anchoredPosition = new Vector3((float)MaxRight, Target.anchoredPosition.y);
        }
        if (Target.anchoredPosition.x <= MaxLeft)
        {
            DragVelocity = Vector2.zero;
            Target.anchoredPosition = new Vector3((float)MaxLeft, Target.anchoredPosition.y);
        }
    }

    private bool isAuto = false;
    private Vector2 DragPos = Vector2.zero;
    public void MoveToEndV(int speed, Vector2 pos, Action callback = null)
    {
        DragPos = pos;
        TargetPos = pos;
        if (!isAuto)
            StartCoroutine(MoveToEndVIE(speed, callback));
    }
    public void MoveToEndFast(Vector2 pos) {
        DragVelocity = Vector2.zero;
        SmoothSpeed = 0;
        DragPos = pos;
        TargetPos = pos;
        Target.anchoredPosition = DragPos;
    }
    private IEnumerator MoveToEndVIE(int speed, Action callback = null)
    {
        isAuto = true;
        while (true)
        {
            Target.anchoredPosition = Vector3.MoveTowards(Target.anchoredPosition, DragPos, speed);
            if (Mathf.Abs(Target.anchoredPosition.y - DragPos.y) < 2)
            {
                Target.anchoredPosition = DragPos;
                TargetPos = DragPos;
                if (callback != null)
                    callback();
                isAuto = false;
                yield break;
            }
            yield return null;
        }
    }
    public void MoveAdd(Vector2 pos, Action<Transform> callback = null) {
        isForward = true;
        float posx = pos.x;
        if (posx >= MaxRight)
        {
            posx = MaxRight;
        }
        if (posx <= MaxLeft)
        {
            posx = MaxLeft;
        }
        pos = new Vector2(posx, pos.y);
        DragPos = pos;
        TargetPos = pos;
        if (!isAuto)
        {
            MonoTool.Instance.MoveToward(Target, pos, 10, callback);
        }
    }
    public void MoveToTarget(Vector2 pos, Action<Transform> callback = null)
    {
        float posx = pos.x;
        if (posx >= MaxRight)
        {
            posx = MaxRight;
        }
        if (posx <= MaxLeft)
        {
            posx = MaxLeft;
        }
        pos = new Vector2(posx, pos.y);
        DragPos = pos;
        TargetPos = pos;
        if (!isAuto)
        {
            MonoTool.Instance.MoveToward(Target, pos, 60, callback);
        }
    }
    public void MoveToTargetX(Vector2 pos, Action<Transform> callback = null)
    {
        DragVelocity = Vector2.zero;
        SmoothSpeed = 0;
        float posx = pos.x;
        if (posx >= MaxRight)
        {
            posx = MaxRight;
        }
        if (posx <= MaxLeft)
        {
            posx = MaxLeft;
        }
        pos = new Vector2(posx, Target.GetComponent<RectTransform>().anchoredPosition.y);
        DragPos = pos;
        TargetPos = pos;
        if (!isAuto)
        {
            MonoTool.Instance.MoveToward(Target, pos, 60, callback);
        }
    }
    public void MoveToSomeOneX(Transform item, Action callback = null) {
        Vector2 pos = new Vector2(-transform.GetComponent<RectTransform>().anchoredPosition.x - item.GetComponent<RectTransform>().anchoredPosition.x, 0);
        //pos = pos * transform.localScale.x;
        if (pos.x < _maxLeft)
        {
            pos = new Vector2(_maxLeft, 0);
        }
        if (pos.x > MaxRight) {
            pos = new Vector2(MaxRight, 0);
        }
        MoveToTarget(pos);
    }
    private IEnumerator MoveTo(Action callback = null)
    {
        isAuto = true;
        while (true)
        {
            Target.anchoredPosition = Vector3.MoveTowards(Target.anchoredPosition, DragPos, 20);
            if (Mathf.Abs(Target.anchoredPosition.x - DragPos.x) < 2)
            {
                Target.anchoredPosition = DragPos;
                TargetPos = DragPos;
                if (callback != null)
                    callback();
                isAuto = false;
                yield break;
            }
            yield return null;
        }
    }
    public void MoveToBase()
    {
        //print("moveToBase");
        StartCoroutine("MoveToBaseIE");
    }
    public float moveToBaseSpeed = 0.01f;
    private IEnumerator MoveToBaseIE()
    {
        DragVelocity = Vector2.zero;
        while (true)
        {
            Target.anchoredPosition = Vector3.SmoothDamp(Target.anchoredPosition, BaseTargetPos, ref DragVelocity, moveToBaseSpeed, 7200, Time.deltaTime);
            if (Vector2.Distance(Target.anchoredPosition, BaseTargetPos) < 4)
            {
                Target.anchoredPosition = BaseTargetPos;
                isMoveOut = false;
                SmoothSpeed = 0;
                //print("isMoveOutFalse");
                if (MoveElasticEvent != null)
                {
                    MoveElasticEvent();
                }
                yield break;
            }
            yield return null;
        }
    }
}




