
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class DragRota : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerDownHandler
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
            if (!transform)
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
    public float _TargetRota;
    public float TargetRota
    {
        get
        {
            return _TargetRota;
        }
        set
        {
            _TargetRota = value;
        }
    }
    public bool IsElastic = true;
    public Vector2 LastMoveOutPos;
    public Vector4 _rectRange = Vector4.zero;
    public Vector4 RectRange
    {
        get
        {
            return _rectRange;
        }
        set
        {
            //Debug.LogError("setRect:" + value);
            _rectRange = value;
        }
    }
    public float angleOne = 6f;
    public float radius = 0f;
    public Vector2 itemSize = new Vector2();
    public Vector2 itemOffset = new Vector2();

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
    protected float DragVelocity = 0;
    private bool _isLockDrag = false;
    protected System.Action DragEndEvent;
    protected System.Action ChangeItemEvent;
    private float _onTargetRota;
    public float OnTargetRota
    {
        get
        {
            return _onTargetRota;
        }
        set
        {
            _onTargetRota = value;
        }
    }
    public float RotaSpeed = 0.1f;
    public float MiddleY = 378;

    public bool IsLockDrag
    {
        get
        {
            return _isLockDrag;
        }
        set
        {
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
    public void StopMove()
    {
        moveState = MoveDir.idle;
        DragVelocity = 0;
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
        begin = TargetRota;
        beginTime = Time.time;
    }
    private float maxDelta = 20;
    private Vector2 GetDeltaByMovement(Vector2 delta)
    {
        if (delta.x > maxDelta)
        {
            delta.x = maxDelta;
        }
        if (delta.x < -maxDelta)
        {
            delta.x = -maxDelta;
        }
        return new Vector2(delta.x, 0);
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_isLockDrag)
            return;
        if (isForceMoving)
            return;
        Vector2 delta = GetDeltaByMovement(eventData.delta);
        if (delta.x < 0)
        {
            moveState = MoveDir.rightToLeft;
        }
        else
        {
            moveState = MoveDir.leftToRight;
        }
        TargetRota -= delta.x * RotaSpeed;
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
            if (DragEndEvent != null)
            {
                DragEndEvent();
            }
        }
    }
    protected float moveDistanceDragBeginAndEnd = 0;
    public float maxDragSpeed = 0.1f;
    private void CalculateVelocity()
    {
        end = TargetRota;
        float useTime = Time.time - beginTime;
        moveDistanceDragBeginAndEnd = end - begin;
        //Debug.LogError("begin:" + begin + "end:" + end + "useTime:" + useTime);
        DragSpeed = moveDistanceDragBeginAndEnd / useTime;
        if (DragSpeed > maxDragSpeed)
        {
            DragSpeed = maxDragSpeed;
        }
        if (DragSpeed < -maxDragSpeed)
        {
            DragSpeed = -maxDragSpeed;
        }
        //Debug.LogError(DragSpeed);
        if (useTime < SmoothNeedTime)
        {
            isSmooth = true;
            SmoothSpeed = DragSpeed;
            SmoothSlowSpeed = 0.96f;
        }
        else
        {
            isSmooth = false;
        }
    }
    public void SetTargetPos(float rota)
    {
        TargetRota = rota;
        Target.rotation = Quaternion.Euler(0, 0, rota);
    }
    private void SmoothAngle()
    {
        //OnTargetRota = TargetRota;
        OnTargetRota = Mathf.SmoothDampAngle(OnTargetRota, TargetRota, ref DragVelocity, 0.01f, 1000, Time.deltaTime);
        Target.rotation = Quaternion.Euler(0, 0, OnTargetRota);
    }
    public void LateUpdate()
    {

        if (isDraging)
        {
            SmoothAngle();
            ChangeItemEvent?.Invoke();
            return;
        }
        if (isForceMoving)
        {
            return;
        }
        if (!isSmooth)
        {
            return;
        }
        if (Mathf.Abs(SmoothSpeed) > 24)
        {
            OnTargetRota += SmoothSpeed * Time.deltaTime;
            Target.rotation = Quaternion.Euler(0, 0, OnTargetRota);
            SmoothSpeed *= SmoothSlowSpeed;
            ChangeItemEvent?.Invoke();
        }
    }
    private bool UpDateIsMoveOut
    {
        get
        {
            return false;
        }
    }
    private Coroutine MoveCo;
    protected void MoveTo(float pos, System.Action moveOver)
    {
        MoveCo.Stop();
        MoveCo = MonoTool.Instance.StartCor(MoveToIE(pos, 1f, moveOver));
    }
    protected void MoveToSmoothIndex(float pos, System.Action moveOver)
    {
        MoveCo.Stop();
        MoveCo = MonoTool.Instance.StartCor(MoveToIE(pos, 1000f, moveOver));
    }
    private IEnumerator MoveToIE(float pos, float moveToBaseSpeed, System.Action callback = null)
    {
        isForceMoving = true;
        if (OnTargetRota < pos)
        {
            moveState = MoveDir.leftToRight;
        }
        else
        {
            moveState = MoveDir.rightToLeft;
        }
        while (true)
        {
            SmoothAngle();
            if (Mathf.Abs(OnTargetRota - pos) < 4)
            {
                isForceMoving = false;
                Target.rotation = Quaternion.Euler(0, 0, OnTargetRota);
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
    public void MoveToSmooth(float pos, System.Action moveOver)
    {
        TargetRota = pos;
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