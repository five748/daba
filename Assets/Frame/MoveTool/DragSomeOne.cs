
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum MoveDir
{
    idle = 0,
    leftToRight = 11,
    rightToLeft = 12,
    downToUp = 21,
    upToDown = 22,
}
public enum Movement
{
    Horizontal = 10,
    Vertical = 20,
}

public class DragSomeOne : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler
{
    public Movement movement = Movement.Horizontal;
    private MoveDir _moveState = MoveDir.idle;
    public MoveDir moveState
    {
        get
        {
            return _moveState;
        }
        set
        {
            _moveState = value;
        }
    }
    private RectTransform _target;
    public RectTransform Target
    {
        get
        {
            if (this == null)
            {
                return null;
            }
            if (!_target)
            {
                _target = transform.GetChild(0).GetComponent<RectTransform>();
            }
            return _target;
        }
    }
    public Vector2 _TargetPos;
    public Vector2 TargetPos
    {
        get
        {
            return _TargetPos;
        }
        set
        {
            //Debug.LogError("setTargetPos:" + value);
            _TargetPos = value;
        }
    }
    public bool IsElastic = false;
    public Vector2 LastMoveOutPos;
    public Vector4 _rectRange = Vector4.zero;
    public Vector4 RectRange {
        get {
            return _rectRange;
        }
        set {
            //Debug.LogError("setRect:" + value);
            _rectRange = value;
        }
    }

    protected bool isDraging = false;
    protected bool isDragPage = false;

    private float oldSmoothTime = 0.96f;
    private float SmoothSlowSpeed = 0;
    private bool isSmooth = false;
    private float begin;
    private float end;
    private float beginTime;
    private float SmoothNeedTime = 0.3f;
    protected float SmoothSpeed = 0;
    private float DragSpeed = 0;
    protected bool isMoveOut = false;
    protected Vector3 DragVelocity = Vector3.zero;
    private bool _isLockDrag = false;
    protected System.Action DragEndEvent;
    public bool IsLockDrag
    {
        get
        {
            return _isLockDrag;
        }
        set
        {
            //Debug.LogError("_isLockDrag:" + value);
            _isLockDrag = value;
            if (!_isLockDrag)
            {
                StopMove();
            }
        }
    }
    private bool IsNeedMoveToBase
    {
        get
        {
            return UpDateIsMoveOut;
        }
    }
    //是否强制移动中
    private bool isForceMoving = false;
    //============================= 系统函数 ===============
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isForceMoving)
            return;
        StopMove();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
      
    }
    public void StopMove()
    {
        //Debug.LogError("StopMove");
        moveState = MoveDir.idle;
        TargetPos = Target.anchoredPosition;
        DragVelocity = Vector2.zero;
        SmoothSpeed = 0;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_isLockDrag)
            return;
        if (isForceMoving)
            return;
        StopMove();
        isDraging = true;
        RememberBeginData();
    }
    private void RememberBeginData()
    {
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
    private float maxDelta = 40;
    private Vector2 GetDeltaByMovement(Vector2 delta)
    {
        if (movement == Movement.Horizontal)
        {
            if (delta.x > maxDelta) {
                delta.x = maxDelta;
            }
            if (delta.x < -maxDelta) {
                delta.x = -maxDelta;
            }
            return new Vector2(delta.x, 0);
        }
        else
        {
            if (delta.y > maxDelta)
            {
                delta.y = maxDelta;
            }
            if (delta.y < -maxDelta)
            {
                delta.y = -maxDelta;
            }
            return new Vector2(0, delta.y);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_isLockDrag)
            return;
        if (isForceMoving)
            return;
        Vector2 delta = GetDeltaByMovement(eventData.delta);
        SetMoveState(delta);
        if (UpDateIsMoveOut && !isDragPage)
        {
            MoveOutDrag(delta);
        }
        else
        {
            TargetPos += delta;
        }
    }
    public void SetMoveState(Vector2 delta)
    {
        if (delta == Vector2.zero)
        {
            moveState = MoveDir.idle;
            return;
        }
        int state = 1;
        if (movement == Movement.Horizontal)
        {
            if (delta.x < 0)
            {
                state = 2;
            }
            else
            {
                state = 1;
            }
        }
        else
        {
            if (delta.y > 0)
            {
                state = 1;
            }
            else
            {
                state = 2;
            }
        }
        moveState = (MoveDir)((int)movement + state);
    }
    private void MoveOutDrag(Vector2 delta)
    {
        if (IsElastic)
        {
            float diff = 0;
            if (delta.x > 0)
            {
                diff = TargetPos.x - LastMoveOutPos.x;
            }
            else
            {
                diff = TargetPos.y - LastMoveOutPos.y;
            }
            if (diff < 20)
            {
                diff = 20;
            }
            TargetPos += delta * (10.0f / diff);
        }
        else
        {
            DragVelocity = Vector3.zero;
            TargetPos = LastMoveOutPos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_isLockDrag)
            return;
        if (isForceMoving)
            return;
        isDraging = false;
        CalculateVelocity();
        if (isDragPage)
        {
            if (DragEndEvent != null)
            {
                DragEndEvent();
            }
            return;
        }
        if (IsNeedMoveToBase)
        {
            MoveToBase();
            if (DragEndEvent != null)
            {
                DragEndEvent();
            }
        }
    }
    protected float moveDistanceDragBeginAndEnd = 0;
    private void CalculateVelocity()
    {
        if (movement == Movement.Horizontal)
        {
            end = Target.anchoredPosition.x;
        }
        else
        {
            end = Target.anchoredPosition.y;
        }
        float useTime = Time.time - beginTime;
        moveDistanceDragBeginAndEnd = end - begin;
        DragSpeed = (end - begin) / useTime;
        if (useTime < SmoothNeedTime)
        {
            isSmooth = true;
            if (movement == Movement.Horizontal)
            {
                SmoothSpeed = DragVelocity.x + DragSpeed;
            }
            else
            {
                SmoothSpeed = DragVelocity.y + DragSpeed;
            }
            SmoothSlowSpeed = oldSmoothTime + (1 - oldSmoothTime) * ((SmoothNeedTime - useTime) / SmoothNeedTime);
        }
        else
        {
            isSmooth = false;
        }
    }
    public void SetTargetPos(Vector2 targetPos)
    {
        TargetPos = targetPos;
        Target.anchoredPosition = targetPos;
    }
    public void LateUpdate()
    {
        if (isDraging)
        {
            Target.anchoredPosition = Vector3.SmoothDamp(Target.anchoredPosition, TargetPos, ref DragVelocity, 0.01f, 7200, Time.deltaTime);
            return;
        }
        if (isForceMoving)
        {
            return;
        }
        if (UpDateIsMoveOut)
        {
            MoveToBase();
            return;
        }
        if (movement == Movement.Horizontal)
        {
            if (Mathf.Abs(SmoothSpeed) > 24)
            {
                SmoothSpeed *= SmoothSlowSpeed;
                Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
            }
        }
        else
        {
            if (Mathf.Abs(SmoothSpeed) > 24)
            {
                SmoothSpeed *= SmoothSlowSpeed;
                Target.anchoredPosition += new Vector2(0, SmoothSpeed * Time.deltaTime);
            }
        }
    }
    private bool UpDateIsMoveOut
    {
        get
        {
            return CheckMoveOut(Target.anchoredPosition);
        }
    }
    private bool CheckMoveOut(Vector2 pos)
    {
        //return false;
        if (movement == Movement.Horizontal)
        {
            if (pos.x > -RectRange.x && moveState == MoveDir.rightToLeft)
            {
                LastMoveOutPos = new Vector2(-RectRange.x, TargetPos.y);
                return true;
            }
            if (pos.x <= -RectRange.y && moveState == MoveDir.leftToRight)
            {
                LastMoveOutPos = new Vector2(-RectRange.y, TargetPos.y);
                return true;
            }
        }
        else
        {
            if (pos.y <= -RectRange.w && moveState == MoveDir.upToDown)
            {
                LastMoveOutPos = new Vector2(TargetPos.x, -RectRange.w);
                return true;
            }
            if (pos.y >= -RectRange.z && moveState == MoveDir.downToUp)
            {
                LastMoveOutPos = new Vector2(TargetPos.x, -RectRange.z);
                return true;
            }
        }
        return false;
    }
    protected void MoveTo(Vector2 pos, System.Action moveOver)
    {
        pos = -GetInRangPos(pos);
        MoveCo.Stop();
        MoveCo = MonoTool.Instance.StartCor(MoveToIE(pos, 1f, moveOver));
    }
    protected void MoveToSmoothIndex(Vector2 pos, System.Action moveOver)
    {
        pos = -GetInRangPos(pos);
        if (movement == Movement.Horizontal)
        {
            pos = new Vector2(pos.x, 0);
        }
        else
        {
            pos = new Vector2(0, pos.y);
        }
        SetMoveState(pos);
        MoveCo.Stop();
        MoveCo = MonoTool.Instance.StartCor(MoveToIE(pos, 1000f, moveOver));
    }
    private Coroutine MoveCo;
    private void MoveToBase()
    {
        DragVelocity = Vector2.zero;
        MoveCo.Stop();
        MoveCo = MonoTool.Instance.StartCor(MoveToIE(LastMoveOutPos, 1f));
    }
    private IEnumerator MoveToIE(Vector2 pos, float moveToBaseSpeed, System.Action callback = null)
    {
        isForceMoving = true;
        if (movement == Movement.Horizontal)
        {
            if (Target.anchoredPosition.x < pos.x)
            {
                moveState = MoveDir.leftToRight;
            }
            else
            {
                moveState = MoveDir.rightToLeft;
            }
        }
        else {
            if (Target.anchoredPosition.y < pos.y)
            {
                moveState = MoveDir.upToDown;
            }
            else
            {
                moveState = MoveDir.downToUp;
            }
        }
        
        while (true)
        {
            Target.anchoredPosition = Vector3.SmoothDamp(Target.anchoredPosition, pos, ref DragVelocity, 0.01f, 7200, Time.deltaTime * moveToBaseSpeed);
            if (Vector2.Distance(Target.anchoredPosition, pos) < 4)
            {
                isForceMoving = false;
                Target.anchoredPosition = pos;
                StopMove();
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }
    public Vector2 GetInRangPos(Vector2 pos)
    {
        if (pos.x < RectRange.x)
        {
            pos.x = RectRange.x;
        }
        if (pos.x > RectRange.y)
        {
            pos.x = RectRange.y;
        }
        if (pos.y < RectRange.z)
        {
            pos.y = RectRange.z;
        }
        if (pos.y > RectRange.w)
        {
            pos.y = RectRange.w;
        }

        return pos;
    }
    public void MoveToSmooth(Vector2 pos, System.Action moveOver)
    {
        TargetPos = pos;
        if (MoveCo != null)
            StopCoroutine(MoveCo);
        MoveCo = StartCoroutine(MoveToSmoothIE(moveOver));
    }
    private IEnumerator MoveToSmoothIE(System.Action callback)
    {
        isForceMoving = true;
        if (Mathf.Abs(SmoothSpeed) < 500)
        {
            if (moveState == MoveDir.rightToLeft || moveState == MoveDir.downToUp)
            {
                SmoothSpeed = -500;
            }
            else
            {
                SmoothSpeed = 500;
            }

        }
        while (true)
        {
            if (movement == Movement.Horizontal)
            {
                Target.anchoredPosition += new Vector2(SmoothSpeed * Time.deltaTime, 0);
            }
            else
            {
                Target.anchoredPosition += new Vector2(0, SmoothSpeed * Time.deltaTime);
            }
            if (UpDateIsMoveOut)
            {
                isForceMoving = false;
                Target.anchoredPosition = LastMoveOutPos;
                StopMove();
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }
}