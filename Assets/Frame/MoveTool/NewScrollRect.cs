using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewScrollRect : ScrollRect
{
    private float oldPos;
    public System.Action<GameObject> ClickItem;
    public System.Action<PointerEventData> OnDragEnd;
    public System.Action<PointerEventData> OnDragBegin;
    public System.Action<PointerEventData> DragEnd;
    public System.Action<PointerEventData> OnDraging;
    float time = 0;
    public static bool isDrag = false;

    public void MoveTo(int index, float diff)
    {
        movementType = MovementType.Clamped;
        content.anchoredPosition = new Vector2(-index * diff, content.anchoredPosition.y);
        MonoTool.Instance.WaitEndFrame(() =>
        {
            movementType = MovementType.Elastic;
        });
    }
    public void MoveToItemOne(Transform item, System.Action callback = null)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            var old = movementType;
            movementType = MovementType.Clamped;
            var rt = item.GetComponent<RectTransform>();
            var maxSize = content.rect.size - transform.GetComponent<RectTransform>().rect.size;
            var toPos = Vector3.zero;
            if (horizontal)
            {
                content.anchoredPosition = new Vector2(-rt.anchoredPosition.x - rt.rect.size.x, content.anchoredPosition.y);
                if (content.anchoredPosition.x > maxSize.x)
                {
                    toPos = new Vector2(maxSize.x, content.anchoredPosition.y);
                }
            }
            else
            {
                float y = -rt.anchoredPosition.y + 400;
                if (y > 0)
                {
                    y = 0;
                }
                if (y < -2377)
                {
                    y = -2377;
                }
                toPos = new Vector2(content.anchoredPosition.x, y);
            }
            content.MoveToAwardRt(toPos, 40, () =>
            {
                //Debug.LogError("moveOver");
                content.anchoredPosition = toPos;
                if (callback != null)
                    callback();
                MonoTool.Instance.WaitEndFrame(() =>
                {
                    movementType = old;
                });
            });
        });
    }
    public void MoveToItem(Transform item, System.Action callback = null)
    {
        SetSizeOver(content, () => {
            MonoTool.Instance.WaitTwoFrame(() =>
            {
                MoveToItemBase(item, callback);
            });
        });
    }
    public void MoveToItemBase(Transform item, System.Action callback = null) {
        var old = movementType;
        movementType = MovementType.Clamped;
        var rt = item.GetComponent<RectTransform>();
        var scrllrect = transform.GetComponent<RectTransform>().rect;
        var maxSize = content.GetComponent<RectTransform>().rect.size - scrllrect.size;
        ////Debug.LogError(content.GetComponent<RectTransform>().rect.size);
        if (horizontal)
        {
            content.anchoredPosition = new Vector2(scrllrect.size.x * 0.5f - rt.anchoredPosition.x, content.anchoredPosition.y);
            if (content.anchoredPosition.x > maxSize.x)
            {
                content.anchoredPosition = new Vector2(maxSize.x, content.anchoredPosition.y);
            }
            if (content.anchoredPosition.x > 0)
            {
                content.anchoredPosition = new Vector2(0, content.anchoredPosition.y);
            }
        }
        else
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, -rt.anchoredPosition.y - rt.rect.size.y * 0.5f);
            if (content.anchoredPosition.y > maxSize.y)
            {
                content.anchoredPosition = new Vector2(content.anchoredPosition.x, maxSize.y);
            }
        }
        MonoTool.Instance.WaitEndFrame(() =>
        {
            movementType = old;
            if (callback != null)
                callback();
        });
    }
    private void SetSizeOver(Transform grid, System.Action callback)
    {
        var sizeTool = grid.GetComponent<NewContentSizeFitter>();
        //Debug.LogError(grid.name);
        if (sizeTool)
        {
            sizeTool.ChangeEvent = (isH) =>
            {
                if (horizontal == isH)
                {
                    callback();
                }
                else {
                    callback();
                }
            };
        }
        else {
            callback();
        }
    }
    public void MoveToItemLimit(Transform item, Vector2 modify, float speed = 1, System.Action callback = null, bool Move = true)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            var old = movementType;
            movementType = MovementType.Clamped;
            var rect = -item.GetComponent<RectTransform>().anchoredPosition;
            Vector2 targe = new Vector2(rect.x, rect.y);
            var size = content.localScale;
            var contWidth = content.GetComponent<RectTransform>().sizeDelta.x * size.x;
            var contHeight = content.GetComponent<RectTransform>().sizeDelta.y * size.y;
            var thisHeight = this.GetComponent<RectTransform>().rect.y;
            var thisWidth = this.GetComponent<RectTransform>().rect.x;
            if (System.Math.Abs(contWidth / 2 - thisWidth / 2) < System.Math.Abs(rect.x))
            {
                var newx = System.Math.Abs(contWidth / 2 - thisWidth / 2);
                targe.x = targe.x > 0 ? newx : -newx;
            }
            if (System.Math.Abs(contHeight / 2 - thisHeight / 2) < System.Math.Abs(rect.y))
            {
                var newy = contHeight < thisHeight ? 0 : System.Math.Abs(contHeight / 2 - thisHeight / 2);
                targe.y = targe.y > 0 ? newy : -newy;
            }
            targe += modify;
            if (Move)
            {
                content.GetComponent<RectTransform>().MoveToAward(targe, speed, (tran) =>
                {
                    if (callback != null)
                        callback();
                    MonoTool.Instance.WaitEndFrame(() =>
                    {
                        movementType = old;
                    });
                });
            }
            else
            {
                content.anchoredPosition = targe;
                if (callback != null)
                    callback();
                MonoTool.Instance.WaitEndFrame(() =>
                {
                    movementType = old;
                });
            }

        });
    }
    //扫荡面板用到的移动效果
    public void MoveToItemLimitSweep(Transform item, Vector2 modify, float speed = 1, System.Action callback = null)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            var old = movementType;
            var recttrans = item.GetComponent<RectTransform>();
            var rect = -recttrans.anchoredPosition;
            Vector2 targe = new Vector2(rect.x, rect.y);
            modify -= recttrans.sizeDelta;
            targe = new Vector2(targe.x, targe.y - modify.y < 0 ? 0 : targe.y - modify.y);
            content.GetComponent<RectTransform>().MoveToAward(targe, speed, (tran) =>
            {
                if (callback != null)
                    callback();
                MonoTool.Instance.WaitEndFrame(() =>
                {
                    movementType = MovementType.Elastic;
                });
            });
        });
    }

    public void MoveShipToItemLimit(Transform scrollBar, float endFloat, Transform item, Vector2 modify, Vector2 size, float speed = 1, System.Action callback = null)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            var old = movementType;
            movementType = MovementType.Clamped;
            var targe = SetShipTarget(item, modify, size);


            var localPos = new Vector3(targe.x, targe.y, 0f);
            var worldPos = transform.TransformPoint(localPos);
            shipTarge = worldPos;

            var oldSize = scrollBar.GetComponent<Scrollbar>().value;
            var oldPos = content.position;
            var End = Mathf.Sqrt(Mathf.Pow(worldPos.x - content.position.x, 2.0f) + Mathf.Pow(worldPos.y - content.position.y, 2.0f));

            var nowtIME = TimeTool.SerUtcTime.Second;
            StartCoroutine(MoveTowardIE(content, speed, () =>
            {
                scrollBar.GetComponent<Scrollbar>().value = endFloat;
                if (callback != null)
                    callback();
                MonoTool.Instance.WaitEndFrame(() =>
                {
                    movementType = old;
                });
            }, () =>
            {
                var newPos = Vector2.MoveTowards(content.position, shipTarge, speed);

                var newEnd = Mathf.Sqrt(Mathf.Pow(newPos.x - oldPos.x, 2.0f) + Mathf.Pow(newPos.y - oldPos.y, 2.0f));

                var ratio = (newEnd / End);
                var newSize = ratio * (endFloat - oldSize);
                scrollBar.GetComponent<Scrollbar>().value += newSize;
                oldPos = newPos;
            }));
        });
    }
    private Vector2 SetShipTarget(Transform item, Vector2 modify, Vector2 size)
    {
        var rect = -item.GetComponent<RectTransform>().anchoredPosition;
        Vector2 targe = new Vector2(rect.x, rect.y) * size + modify;
        var contWidth = content.GetComponent<RectTransform>().sizeDelta.x * size.x;
        var contHeight = content.GetComponent<RectTransform>().sizeDelta.y * size.y;

        Vector3[] bgconners = new Vector3[4];//ScrollRect四角的世界坐标
        GetComponent<RectTransform>().GetLocalCorners(bgconners);

        var thisHeight = bgconners[1].y - bgconners[0].y;
        var thisWidth = bgconners[2].x - bgconners[0].x;
        if (System.Math.Abs(contWidth / 2 - thisWidth / 2) < System.Math.Abs(targe.x))
        {
            var newx = System.Math.Abs(contWidth / 2 - thisWidth / 2);
            targe.x = targe.x > 0 ? newx : -newx;
        }
        if (System.Math.Abs(contHeight / 2 - thisHeight / 2) < System.Math.Abs(targe.y))
        {
            var newy = contHeight < thisHeight ? 0 : System.Math.Abs(contHeight / 2 - thisHeight / 2);
            targe.y = targe.y > 0 ? newy : -newy;
        }
        return targe;
    }
    Vector2 shipTarge;
    private IEnumerator MoveTowardIE(RectTransform myTran, float speed, System.Action callback = null, System.Action running = null)
    {
        while (true)
        {
            if (myTran == null)
            {
                yield break;
            }
            if (running != null)
            {
                running();
            }
            myTran.position = Vector2.MoveTowards(myTran.position, shipTarge, speed);

            if (Vector2.Distance(myTran.position, shipTarge) <= speed + 0.5f)
            {
                myTran.position = shipTarge;
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }

    public void MoveToItemX(Transform item, int modify = 0)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            movementType = MovementType.Clamped;
            //Debug.LogError(-item.GetComponent<RectTransform>().anchoredPosition.x);
            content.anchoredPosition = new Vector2(-item.GetComponent<RectTransform>().anchoredPosition.x + modify, 0);
            MonoTool.Instance.WaitEndFrame(() =>
            {
                movementType = MovementType.Elastic;
            });
        });
    }
    public void MoveToItemXAnysc(Transform item, int modify = 0, float speed = 1, System.Action callback = null)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            movementType = MovementType.Clamped;
            //Debug.LogError(-item.GetComponent<RectTransform>().anchoredPosition.x);
            content.GetComponent<RectTransform>().MoveToAward(new Vector2(-item.GetComponent<RectTransform>().anchoredPosition.x + modify, content.GetComponent<RectTransform>().anchoredPosition.y), speed, (tran) =>
            {
                if (callback != null)
                    callback();
                MonoTool.Instance.WaitEndFrame(() =>
                {
                    movementType = MovementType.Elastic;
                });
            });
        });
    }
    public void MoveToItemY(Transform item, int modify = 0, System.Action callback = null)
    {
        MonoTool.Instance.WaitEndFrame(() =>
        {
            movementType = MovementType.Clamped;
            content.anchoredPosition = new Vector2(0, -item.GetComponent<RectTransform>().anchoredPosition.y - modify);
            MonoTool.Instance.WaitEndFrame(() =>
            {
                if (callback != null)
                {
                    callback();
                }
                movementType = MovementType.Elastic;

            });
        });
    }
    public void MoveToY(int index, float diff, float time)
    {
        movementType = MovementType.Clamped;
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, index * diff);
        MonoTool.Instance.Wait(time, () =>
        {
            movementType = MovementType.Clamped;
        });
    }

    public void MoveToY(int index, float diff, float offset, float time)
    {
        movementType = MovementType.Clamped;
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, index * diff + offset);
        MonoTool.Instance.Wait(time, () =>
        {
            movementType = MovementType.Clamped;
        });
    }

    public void ScrollToY(int index, float diff, float offsetY = 0f, System.Action callback = null)
    {
        movementType = MovementType.Clamped;
        float y = index * diff - offsetY;
        Vector2 vec2 = new Vector2(content.anchoredPosition.x, y);
        //uint time = 3;
        //speed = index * diff / time * 60;

        MoveToward(content, vec2, () =>
        {
            MonoTool.Instance.WaitEndFrame(() =>
            {
                movementType = MovementType.Clamped;
            });
            if (callback != null)
            {
                callback();
            }
        });
    }

    public void MoveToward(RectTransform myTran, Vector2 target, System.Action callback = null)
    {
        //myTran.anchoredPosition = Vector2.MoveTowards(myTran.anchoredPosition, target, speed);
        StartCoroutine(MoveTowardIE(myTran, target, callback));
    }
    private IEnumerator MoveTowardIE(RectTransform myTran, Vector2 targetPos, System.Action callback = null)
    {
        while (true)
        {
            Vector2 vec = Vector2.zero;
            //myTran.anchoredPosition = Vector2.MoveTowards(myTran.anchoredPosition, targetPos, speed);
            myTran.anchoredPosition = Vector2.SmoothDamp(myTran.anchoredPosition, targetPos, ref vec, 0.1f, 7200, Time.deltaTime);
            //if (Vector2.Distance(myTran.anchoredPosition, targetPos) <= 1)
            //Debug.LogError(Mathf.Abs(myTran.anchoredPosition.y - targetPos.y));

            if (Vector2.Distance(myTran.anchoredPosition, targetPos) <= 1)
            {
                myTran.anchoredPosition = targetPos;
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }

    public void ScrollToY2(int index, float diff, float offsetY = 0f, float time = 0f, System.Action callback = null)
    {
        movementType = MovementType.Elastic;
        float y = index * diff - offsetY;
        Vector2 vec2 = new Vector2(content.anchoredPosition.x, y);

        MoveToward2(content, vec2, time, () =>
        {
            MonoTool.Instance.Wait(time, () =>
            {
                movementType = MovementType.Clamped;
            });
            if (callback != null)
            {
                callback();
            }
        });
    }
    public void MoveToward2(RectTransform myTran, Vector2 target, float time = 0f, System.Action callback = null)
    {
        //myTran.anchoredPosition = Vector2.MoveTowards(myTran.anchoredPosition, target, speed);
        StartCoroutine(MoveTowardIE2(myTran, target, time, callback));
    }
    private IEnumerator MoveTowardIE2(RectTransform myTran, Vector2 targetPos, float time = 0f, System.Action callback = null)
    {
        while (true)
        {
            Vector2 vec = Vector2.zero;
            myTran.anchoredPosition = Vector2.SmoothDamp(myTran.anchoredPosition, targetPos, ref vec, 0.08f, 7200, Time.deltaTime);
            if (Vector2.Distance(myTran.anchoredPosition, targetPos) <= 5)
            {
                myTran.anchoredPosition = targetPos;
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        isDrag = true;
        oldPos = eventData.position.y;
        time = Time.time;
        if (OnDragBegin != null)
        {
            OnDragBegin(eventData);
        }
        if (isInited)
        {
            _oldPos = grid.anchoredPosition.y;
            isPress = true;
            move = false;
        }
        //print("OnBegin");
    }
    private bool isUseDragPage = true;
    private float distance = 0.5f;
    public override void OnEndDrag(PointerEventData eventData)
    {
        //print("scrollEnd");
        base.OnEndDrag(eventData);
        isDrag = false;
        //if (isUseDragPage)
        //{
        //    if (Input.GetMouseButtonUp(0))//抬起的时候输出目标位置
        //    {
        //        PointUp();
        //    }
        //}
        //print(eventData.position.y - oldPos);
        if (DragEnd != null)
        {
            DragEnd(eventData);
        }
        if (isInited)
        {
            isPress = false;
        }
        if (Mathf.Abs(eventData.position.y - oldPos) < 100)
        {

            if (Time.time - time < 0.6f)
            {
                if (!eventData.lastPress)
                    return;
                if (ClickItem != null)
                {
                    ClickItem(eventData.lastPress);
                }
            }
            else
            {
                if (OnDragEnd != null)
                {
                    OnDragEnd(eventData);
                }
            }
        }
        else
        {
            if (OnDragEnd != null)
            {
                OnDragEnd(eventData);
            }
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount >= 2)
        {
            return;
        }
        base.OnDrag(eventData);
        if (OnDraging != null)
        {
            OnDraging(eventData);
        }
    }
    //public void PointUp()
    //{
    //    float value = (int)(horizontalNormalizedPosition / distance + 0.5f);
    //    horizontalNormalizedPosition = distance * (value);
    //    //ShowChoose(itemList[value + 1]);
    //}
    //滑动箭头显示隐藏---滑动扩展
    [HideInInspector]
    public Transform Left;
    [HideInInspector]
    public Transform Right;
    private bool isHaveDirect = false;
    private float scrollpos
    {
        get
        {
            if (horizontal)
            {
                return horizontalNormalizedPosition;
            }
            else
            {
                return verticalNormalizedPosition;
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        if (content == null)
        {
            return;
        }
        MonoTool.Instance.WaitEndFrame(() =>
        {
            var newCon = content.GetComponent<NewContentSizeFitter>();
            if (!newCon)
            {
                var con = content.GetComponent<ContentSizeFitter>();
                if (con)
                {
                    var hfit = con.horizontalFit;
                    var vfit = con.verticalFit;
                    DestroyImmediate(con);
                    newCon = content.gameObject.AddComponent<NewContentSizeFitter>();
                    newCon.horizontalFit = hfit;
                    newCon.verticalFit = vfit;
                }
                else
                {
                    SetDirect(true);
                    return;
                }
            }
            else
            {
                SetDirect(true);
            }
            newCon.ChangeEvent = SetDirect;
        });
    }
    private void SetDirect(bool ish)
    {
        SetIsHaveDirect();

        if (!isHaveDirect)
        {
            if (Left)
                Left.gameObject.SetActive(false);
            if (Right)
                Right.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (Left)
                Left.gameObject.SetActive(false);
            if (Right)
                Right.gameObject.SetActive(true);
        }
        onValueChanged.AddListener((pos) =>
        {
            if (!isHaveDirect)
            {
                return;
            }
            if (scrollpos <= 0)
            {
                if (horizontal)
                {
                    if (Left)
                        Left.gameObject.SetActive(false);
                    if (Right)
                        Right.gameObject.SetActive(true);
                }
                else
                {
                    if (Left)
                        Left.gameObject.SetActive(true);
                    if (Right)
                        Right.gameObject.SetActive(false);
                }
            }
            else if (scrollpos >= 1)
            {
                if (horizontal)
                {
                    if (Left)
                        Left.gameObject.SetActive(true);
                    if (Right)
                        Right.gameObject.SetActive(false);
                }
                else
                {
                    if (Left)
                        Left.gameObject.SetActive(false);
                    if (Right)
                        Right.gameObject.SetActive(true);
                }
            }
            else
            {
                if (Left)
                    Left.gameObject.SetActive(true);
                if (Right)
                    Right.gameObject.SetActive(true);
            }

        });
    }
    private void SetIsHaveDirect()
    {
        if (transform == null)
        {
            return;
        }
        if (horizontal)
        {
            isHaveDirect = transform.GetComponent<RectTransform>().sizeDelta.x < content.GetComponent<RectTransform>().sizeDelta.x;
        }
        else
        {
            isHaveDirect = transform.GetComponent<RectTransform>().sizeDelta.y < content.GetComponent<RectTransform>().sizeDelta.y;
        }
    }
    protected override void OnDestroy()
    {
        var fitter = content.GetComponent<NewContentSizeFitter>();
        if (fitter)
            fitter.ChangeEvent = null;
    }
    private bool isPress = true;
    private bool move = false;
    private bool isInited = false;
    private int one;
    private RectTransform grid;
    private Vector3 speed;
    private float _oldPos;
    private float _newPos;
    private int max;
    private float Target = 0f;
    public System.Action<int> changeTarget = null;
    public void InitChooseTarget(Transform _grid, int _one)
    {
        max = _grid.childCount;
        grid = _grid.GetComponent<RectTransform>();
        one = _one;
        isInited = true;
    }
    void Update()
    {
        if (!isInited)
        {
            return;
        }
        if (_oldPos != 0 && _newPos != 0 && _oldPos == _newPos)
        {
            return;
        }
        if (!isPress)
        {
            if (Mathf.Abs(this.velocity.y) < 30f)
            {
                isPress = true;
                move = true;
                TargetDragEnd();
            }
        }
        if (move)
        {
            targetControl(one * Target);
        }
    }
    void TargetDragEnd()
    {
        var a = grid.anchoredPosition.y / one;
        _newPos = grid.anchoredPosition.y;
        Target = _newPos > _oldPos ? (int)a + 1.0f : (int)a;
        speed = new Vector3(0f, this.velocity.y, 0f);
        this.velocity = new Vector2(0f, 0f);
        changeTarget((int)Target > max - 2 ? max : (int)Target);
    }
    void targetControl(float _target)
    {
        Debug.LogError("dddd");
        grid.anchoredPosition = Vector3.SmoothDamp(grid.anchoredPosition, new Vector3(0, _target, 0), ref speed, 0.5f);
        if (Mathf.Abs(grid.anchoredPosition.y - _target) < 1f)
        {
            move = false;
        }
    }

}





















































