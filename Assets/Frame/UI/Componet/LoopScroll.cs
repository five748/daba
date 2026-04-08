
using System;
using System.Collections.Generic;
using UnityEngine;
public class LoopScroll : DragSomeOne
{
    public int key = -1;
    public enum AddDirEnum
    {
        left = 0,
        right = 1,
        top = 2,
        down = 3,
    }
    public enum BeginDir
    {
        TopLeft = 0,
        TopMiddle = 1,
        TopRight = 2,
        MiddleLeft = 3,
        MiddleRight = 4,
        DownLeft = 5,
        DownMiddle = 6,
        DownRight = 7,
    }
    private Vector2 _defauleSize = Vector2.zero;
    public Vector2 ItemDefaultSize
    {
        get
        {
            if (_defauleSize == Vector2.zero)
            {
                var oneitem = Target.GetChild(0).GetComponent<RectTransform>();
                _defauleSize = oneitem.sizeDelta * oneitem.localScale.x;
            }
            return _defauleSize;
        }
    }
    public BeginDir beginDir;
    private Vector4 scrollRect;
    private Vector4 scrollRectWorld;
    private Vector4 OldScrollRect;
    public List<RectTransform> items;
    private List<Vector2> allItemPos = new List<Vector2>();
    private Vector2 FirstItemPos;
    private Vector2 LastItemPos;
    private Vector2 LoadInRectPos;
    private Vector2 OldFirstItemPos;
    private Vector2 OldLastItemPos;
    private Vector2 OldLoadInRectPos;
    private Transform BaseItem;
    private int itemCount;
    private System.Func<Transform, int, bool, Vector2> ShowItemEvent;
    public int FirstIndex;
    public int LastIndex = -1;
    private AddDirEnum AddDir;
    public bool IsInited = false;
    private System.Action LoadEvent;
    public int MaxLen;
    public Vector2 BeginPos;
    private Vector2 oldBeginPos;
    public Vector2 space;
    private int FirstItemIndex;
    private int LastItemIndex;
    public string[] Sorts;
    public string[] Filts;
    public string GroupStr;
    public LoopScroll GroupShow;
    public string[] SpcKeys;
    public string[] SpcValues;
    public Transform[] ChildShows;
    public bool isHaveStart;
    public bool isHaveLoop = true;
    private bool isForceAddToFirst;
    private System.Action<GameObject> ClickItem;
    private System.Action<GameObject, GameObject> ChangeClickItem;
    private System.Action OnSlideEvent;
    private RectTransform LastClickItem;
    public bool isSlow = false;
    public void InitItems(int maxlen, System.Func<Transform, int, bool, Vector2> showItems,
        System.Action<GameObject> clickItem = null, string baseUIName = "", System.Action loadEvent = null,
        System.Action<GameObject, GameObject> changeClickItem = null, System.Action callback = null)
    {
        if (LoopCoroutine != null)
            LoopCoroutine.Stop();
        MaxLen = maxlen;
        ClickItem = clickItem;
        ChangeClickItem = changeClickItem;
        ShowItemEvent = showItems;
        DragEndEvent = loadEvent;
        LastIndex = -1;
        if (IsInited)
        {
            ResetData();
        }
        else
        {
            InitData(baseUIName);
            IsInited = true;
        }
        oldBeginPos = BeginPos;
        SetItemFillScroll(() =>
        {
            SetTargetSize();
            oldBeginPos = BeginPos;
            if (callback != null)
                callback();
        });
        //if (isSlow) {
        //    TargetEndItemPos = new Vector2(oneSize.x, -oneSize.y * maxlen);
        //    SetTargetSize();
        //}
    }
    private bool isFirstMoveTo = false;
    public void InitItemByMoveTo(int moveToIndex, int maxlen, System.Func<Transform, int, bool, Vector2> showItems,
        System.Action<GameObject> clickItem = null, string baseUIName = "", System.Action loadEvent = null,
        System.Action<GameObject, GameObject> changeClickItem = null)
    {
        isFirstMoveTo = true;
        InitItems(maxlen, showItems, clickItem, baseUIName, loadEvent, changeClickItem, () =>
        {
            MoveAndClickItemByIndex(moveToIndex);
            isFirstMoveTo = false;
        });
    }

    public void AddOtherData(int maxlen)
    {
        SetItemFillScroll(() =>
        {
            SetTargetSize();
        });
    }
    public void InitItems<T>(List<T> data, System.Action<Transform, int, bool, T> action)
    {
        this.InitItems(data.Count, (item, index, isOnlyGeSize) =>
        {
            if (!isOnlyGeSize)
            {
                item.name = index.ToString();
                if (index >= 0 && index < data.Count)
                {
                    var dataItem = data[index];
                    action(item, index, isOnlyGeSize, dataItem);
                }
            }

            return this.ItemDefaultSize;
        });
    }

    public void ClickItemByIndex(int index)
    {
        if (index == -1)
        {
            index = 0;
        }
        foreach (var item in items)
        {
            if (item.name == index.ToString())
            {
                Debug.Log("click");
                if (ClickItem != null)
                {
                    ClickItem(item.gameObject);
                }
                if (ChangeClickItem != null)
                {
                    ChangeClickItem(item.gameObject, LastClickItem == null ? null : LastClickItem.gameObject);
                    LastClickItem = item;
                }
                return;
            }
        }
    }
    public void MoveAndClickItemByIndexOld(int index, System.Action<Transform> callback = null)
    {
        if (index == -1)
        {
            index = 0;
        }
        MoveToIndex(index, Vector2.zero, callback);
    }
    public void ClickLeftOrRight(int index, bool isLeft, System.Action<Transform> callback = null)
    {
        if (index == -1)
        {
            index = 0;
        }
        RectTransform target = null;
        foreach (var item in items)
        {
            if (item.name == index.ToString())
            {
                target = item;
                break; ;
            }
        }
        if (target != null)
        {
            if (!target.CheckIsOut(OldScrollRect))
            {
                callback(target);
                return;
            }
        }
        ClickMoveTo(index, isLeft, callback);
    }
    public void ClickMoveTo(int index, bool isLeft, System.Action<Transform> callback)
    {
        //Debug.LogError(index);
        Vector2 pos = allItemPos[index];
        if (!isLeft)
        {
            var dif = Vector2.zero;
            if (allItemPos.Count > 1)
            {
                dif = allItemPos[1] - allItemPos[0];
            }
            pos += dif;
            if (movement == Movement.Horizontal)
            {
                pos -= new Vector2(GetComponent<RectTransform>().sizeDelta.x, 0);
            }
            else
            {
                pos += new Vector2(0, GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        if (movement == Movement.Horizontal)
        {
            pos = new Vector2(pos.x - BeginPos.x, 0);
        }
        else
        {
            pos = new Vector2(0, pos.y - BeginPos.y);
        }
        MoveTo(pos, () =>
        {
            foreach (var item in items)
            {
                if (item.name == index.ToString())
                {
                    callback(item);
                    return;
                }
            }
        });
    }

    public void MoveToIndex(int index, Vector2 modify, System.Action<Transform> callback)
    {
        //Debug.LogError(index);
        Vector2 pos = allItemPos[index];
        if (movement == Movement.Horizontal)
        {
            pos = new Vector2(pos.x, 0);
        }
        else
        {
            pos = new Vector2(0, pos.y);
        }
        pos += modify;
        MoveToSmoothIndex(pos, () =>
            {
                foreach (var item in items)
                {
                    if (item.name == index.ToString())
                    {
                        callback(item);
                        return;
                    }
                }
            });
    }

    public void InitNoScroll(int num, System.Func<Transform, int, bool, Vector2> showItems, System.Action<GameObject> clickItem = null)
    {
        IsElastic = false;
        var tranitems = transform.AddChilds(num);
        for (int i = 0; i < tranitems.Length; i++)
        {
            tranitems[i].SetAllGrey(false);
            items.Add(tranitems[i].GetComponent<RectTransform>());
            EventTriggerListener.Get(items[i].gameObject).onClick = clickItem;
        }
    }
    public void ReSetItems()
    {

        foreach (var item in items)
        {
            item.SetAllGrey(false);
            ShowItemEvent(item, int.Parse(item.name), false);
        }
    }
    private void ResetData()
    {
        foreach (var item in items)
        {
            item.SetActive(false);
        }
        BeginPos = oldBeginPos;
        Sum = -1;
        FirstIndex = 0;
        LastItemIndex = 0;
        FirstIndex = 0;
        LastIndex = -1;
        FirstItemPos = OldFirstItemPos;
        LastItemPos = OldLastItemPos;
        LoadInRectPos = Vector2.zero;
        LastMoveOutPos = Vector2.zero;
        Target.anchoredPosition = Vector2.zero;
        TargetPos = Vector2.zero;
        OutIndex = -1;
        IsOut = false;
        allItemPos.Clear();
        RectRange = Vector4.zero;
        //Debug.LogError(RectRange);
    }
    void OnDestroy()
    {
        OnSlideEvent = null;
        DragEndEvent = null;
    }
    public bool isModify = false;
    public bool isModifyBag = false;
    private void InitData(string baseUIName = "")
    {
        if (IsInited)
        {
            return;
        }
        Canvas.ForceUpdateCanvases();
        if (baseUIName != "")
        {
            if (Target.childCount == 1)
            {
                GameObject.DestroyImmediate(Target.GetChild(0).gameObject);
            }
            AssetLoadOld.Instance.LoadPrefab("BaseUIPrefab/" + baseUIName, (go) =>
            {
                GameObject go1 = GameObject.Instantiate(go, Target) as GameObject;
            });
        }
        BaseItem = Target.GetChild(0);
        BaseItem.SetActive(false);
        scrollRect = GetComponent<RectTransform>().GetRange();
        scrollRectWorld = GetComponent<RectTransform>().GetRangeWorld();
        //scrollRect += new Vector4(0,0, -100, 100); 
        OldScrollRect = scrollRectWorld + new Vector4(-0.1f, 0.1f, -0.1f, 0.1f);
        items = new List<RectTransform>();
        SetPos();
        if (isModify)
        {
            ModifyBeginPos();
        }
        if (isModifyBag)
        {
            ModifyBagBeginPos();
        }
        LoadInRectPos = Vector2.zero;
        BeginPos += modifyBeginPos;
        FirstItemPos = BeginPos;
        LastItemPos += BeginPos;
        OldFirstItemPos = FirstItemPos;
        OldLastItemPos = LastItemPos;
        OldLoadInRectPos = LoadInRectPos;
        scrollRect += new Vector4(BeginPos.x, BeginPos.x, BeginPos.y, BeginPos.y);
        //scrollRectWorld += new Vector4(BeginPos.x, BeginPos.x, BeginPos.y, BeginPos.y);
    }
    public void RefreshScrollRect()
    {
        scrollRect = GetComponent<RectTransform>().GetRange();
        scrollRectWorld = GetComponent<RectTransform>().GetRangeWorld();
        //scrollRect += new Vector4(0,0, -100, 100); 
        OldScrollRect = scrollRectWorld + new Vector4(-0.1f, 0.1f, -0.1f, 0.1f);
        scrollRect += new Vector4(BeginPos.x, BeginPos.x, BeginPos.y, BeginPos.y);
    }
    void ModifyBagBeginPos()
    {
        var oneWidth = BaseItem.GetComponent<RectTransform>().sizeDelta.x + space.x;
        Vector2 newBeginPos = new Vector2();
        if ((LastItemPos.x % oneWidth) > (oneWidth / 2))
        {
            var newSpace = LastItemPos.x % oneWidth - space.x * 2;
            var oneNum = (int)(LastItemPos.x / oneWidth);
            var needSpace = new Vector2(space.x + (int)(newSpace / (oneNum - 1)), space.y);
            if (needSpace.x >= space.x)
            {
                space = needSpace;
            }
            var NSpace = LastItemPos.x % oneWidth - space.x * 2;
            newBeginPos = new Vector2(NSpace / 2, BeginPos.y);
        }
        else if ((LastItemPos.x % oneWidth) > space.x)
        {
            var newSpace = LastItemPos.x % oneWidth;
            newBeginPos = new Vector2(newSpace / 2, BeginPos.y);
        }
        if (newBeginPos.x >= BeginPos.x)
        {
            BeginPos = newBeginPos;
        }
    }
    void ModifyBeginPos()
    {
        var oneWidth = BaseItem.GetComponent<RectTransform>().sizeDelta.x + space.x;
        if ((LastItemPos.x % oneWidth) > (oneWidth / 2))
        {
            var newSpace = LastItemPos.x % oneWidth - space.x * 2;
            var oneNum = (int)(LastItemPos.x / oneWidth);
            space = new Vector2(space.x + (int)(newSpace / (oneNum - 1)), space.y);
            BeginPos = new Vector2((int)(newSpace % (oneNum - 1)) / 2, BeginPos.y);
        }
        else if ((LastItemPos.x % oneWidth) > space.x)
        {
            var newSpace = LastItemPos.x % oneWidth;
            BeginPos = new Vector2(newSpace / 2, BeginPos.y);
        }
    }
    private Vector2 modifyBeginPos;
    private void SetPos()
    {
        RectTransform itemRT = BaseItem.GetComponent<RectTransform>();
        modifyBeginPos = itemRT.sizeDelta * 0.5f;
        Vector2 pos = Vector2.zero;
        if (beginDir == BeginDir.TopMiddle)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2(0, scrollRect.w - scrollRect.z);
            modifyBeginPos = new Vector2(0, -modifyBeginPos.y);
            pos = new Vector2(0.5f, 1);
        }
        if (beginDir == BeginDir.TopLeft)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2(scrollRect.y - scrollRect.x, scrollRect.w - scrollRect.z);
            modifyBeginPos = new Vector2(modifyBeginPos.x, -modifyBeginPos.y);
            pos = new Vector2(0, 1);
        }
        if (beginDir == BeginDir.TopRight)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2(scrollRect.x, scrollRect.z);
            modifyBeginPos = new Vector2(-modifyBeginPos.x, 0);
            pos = new Vector2(1, 1);
        }
        if (beginDir == BeginDir.MiddleLeft)
        {
            movement = Movement.Horizontal;
            LastItemPos = new Vector2(scrollRect.y, 0);
            modifyBeginPos = new Vector2(modifyBeginPos.x, 0);
            pos = new Vector2(0, 0.5f);
        }
        if (beginDir == BeginDir.MiddleRight)
        {
            movement = Movement.Horizontal;
            LastItemPos = new Vector2(scrollRect.x, 0);
            modifyBeginPos = new Vector2(-modifyBeginPos.x, 0);
            pos = new Vector2(1, 0.5f);
        }
        if (beginDir == BeginDir.DownMiddle)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2((scrollRect.x + scrollRect.y) * 0.5f, scrollRect.w);
            modifyBeginPos = new Vector2(0, -modifyBeginPos.y);
            pos = new Vector2(0.5f, 0);
        }
        if (beginDir == BeginDir.DownLeft)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2(scrollRect.y, scrollRect.w);
            modifyBeginPos = new Vector2(modifyBeginPos.x, -modifyBeginPos.y);
            pos = new Vector2(0, 0);
        }
        if (beginDir == BeginDir.DownRight)
        {
            movement = Movement.Vertical;
            LastItemPos = new Vector2(scrollRect.x, scrollRect.w);
            modifyBeginPos = new Vector2(-modifyBeginPos.x, -modifyBeginPos.y);
            pos = new Vector2(1, 0);
        }
        Vector2 middlePos = new Vector2(0.5f, 0.5f);
        Target.pivot = pos;
        Target.anchorMin = pos;
        Target.anchorMax = pos;
        itemRT.pivot = middlePos;
        itemRT.anchorMin = pos;
        itemRT.anchorMax = pos;
    }
    public void InitByItemFirstPos(int index, Vector2 pos)
    {
        FirstIndex = index;
        LastIndex = index - 1;
        SetItemFillScroll(() =>
        {
            SetTargetSize();
        });
    }
    //item铺满整个可视区域
    private int Sum = -1;
    private bool IsOut = false;
    public int slowNum = 2;
    private int slowBeginNum = -1;
    public Coroutine LoopCoroutine;
    private void SetItemFillScroll(System.Action callback)
    {
        Sum++;
        if (Sum >= MaxLen)
        {
            callback.Invoke();
            return;
        }
        if (LastIndex >= MaxLen)
        {
            callback.Invoke();
            return;
        }
        if (FirstIndex == 0 && LastIndex >= MaxLen)
        {
            callback.Invoke();
            return;
        }
        if (FirstIndex > 0 && !LastItemPos.CheckPosIsOutRangeTopAndLeft(scrollRect))
        {
            isForceAddToFirst = true;
        }
        AddOneItem();
        isForceAddToFirst = false;
        if (!IsOut && isSlow)
        {
            slowBeginNum = 0;
            LoopCoroutine = MonoTool.Instance.WaitEndFrame(() =>
            {
                if (this == null)
                {
                    return;
                }
                SetItemFillScroll(callback);
            });
        }
        // else if (isSlowMove)
        // {
        //     //处理完动态效果，回调出来SetItemFillScroll(callback);
        // }
        else
        {
            slowBeginNum++;
            SetItemFillScroll(callback);
        }
    }
    // public bool isSlowMove = false
    public void SetTargetSize()
    {
        if (gameObject == null)
        {
            return;
        }
        if (movement == Movement.Horizontal)
        {
            Target.sizeDelta = new Vector2(Mathf.Abs(TargetEndItemPos.x), scrollRect.w - scrollRect.z);
        }
        else
        {
            Target.sizeDelta = new Vector2(scrollRect.y - scrollRect.x, Mathf.Abs(TargetEndItemPos.y));
        }
        //Target.sizeDelta += new Vector2(Math.Abs(BeginPos.x), Math.Abs(BeginPos.y)) * 2 - space;
        if (IsOut)
        {
            RectRange = Target.GetRange() - scrollRect;
            RectRange = new Vector4(0, RectRange.y - RectRange.x, RectRange.z - RectRange.w, 0);
            IsLockDrag = false;
        }
        else
        {
            RectRange = new Vector4(0, BeginPos.x, -BeginPos.y, 0);
            IsLockDrag = true;
        }
        FirstItemIndex = 0;
        LastItemIndex = items.Count - 1;
        Target.anchoredPosition = Vector2.zero;
        TargetPos = Vector2.zero;
    }
    //修正grid的位置
    private void ReviseGridPos()
    {

    }
    private int OutIndex = -1;
    private int OutLen = 2;
    private int OneLen = 1;
    private bool isSetOneLen = false;
    private int oneBegin = -1;
    private Vector2 oldPos;
    private Vector2 oneSize;
    private void AddOneItem()
    {
        Vector2 size = ShowItemEvent(null, Sum, true);
        oneSize = size;
        size += space;
        Vector2 newPos = GetNewLoadPos(size);
        if (beginDir == BeginDir.MiddleLeft || beginDir == BeginDir.MiddleRight)
        {

        }
        else
        {
            if (!isSetOneLen)
            {
                oneBegin++;
                if (Sum != 0)
                {
                    if (oldPos.y != newPos.y)
                    {
                        isSetOneLen = true;
                        OneLen = oneBegin;
                        OutLen = 2 * OneLen;
                        //Debug.LogError(OutLen);
                    }
                }
            }
        }
        oldPos = newPos;
        allItemPos.Add(newPos);
        if (allItemPos.Count < allItemPos.Count)
        {
            allItemPos.Add(newPos);
        }

        if (IsOut)
        {
            OutIndex++;
        }
        if (OutIndex <= OutLen)
        {
            LoadOneItem(newPos);
        }
    }
    private void LoadOneItem(Vector2 newPos)
    {
        if (this == null)
        {
            return;
        }
        LastIndex++;
        RectTransform changeItem = null;
        if (LastIndex < items.Count)
        {
            changeItem = items[LastIndex];
        }
        else
        {
            changeItem = Target.AddOneItem(BaseItem).GetComponent<RectTransform>();
            //changeItem.gameObject.AddComponent<DebugGameObjectAcitve>();
            items.Add(changeItem);
            if (ClickItem != null)
            {
                EventTriggerListener.Get(changeItem).onClick = (button) =>
                {
                    ClickItem(button);
                };
            }
            if (ChangeClickItem != null)
            {
                EventTriggerListener.Get(changeItem).onClick = (button) =>
                {
                    ChangeClickItem(button, LastClickItem == null ? null : LastClickItem.gameObject);
                    LastClickItem = changeItem;
                };
            }
        }
        changeItem.name = Sum.ToString();
        changeItem.anchoredPosition3D = newPos;
        changeItem.SetActive(true);
        changeItem.SetAllGrey(false);
        if (!isFirstMoveTo)
            ShowItemEvent(changeItem, LastIndex, false);
        if (!IsOut)
            IsOut = changeItem.CheckIsOut(OldScrollRect);
    }
    private Vector2 GetNextLoadInRectPos(Vector2 size)
    {
        if (beginDir == BeginDir.MiddleLeft)
        {
            return LoadInRectPos + new Vector2(size.x, 0);
        }
        if (beginDir == BeginDir.MiddleRight)
        {
            return LoadInRectPos + new Vector2(size.x, 0);
        }
        if (beginDir == BeginDir.TopMiddle)
        {
            return LoadInRectPos - new Vector2(0, size.y);
        }
        if (beginDir == BeginDir.DownMiddle)
        {
            return LoadInRectPos - new Vector2(0, size.y);
        }
        if (beginDir == BeginDir.TopLeft)
        {
            var pos = LoadInRectPos + new Vector2(size.x, 0);
            if (Sum == 0)
            {
                pos = new Vector2(BeginPos.x, LoadInRectPos.y - size.y);
            }
            if (pos.x + size.x * 2 > LastItemPos.x)
            {
                pos = new Vector2(BeginPos.x, LoadInRectPos.y - size.y * 2);
            }
            return pos;
        }
        if (beginDir == BeginDir.TopRight)
        {
            var pos = LoadInRectPos + new Vector2(size.x, 0);
            if (pos.x < LastItemPos.x)
            {
                pos = new Vector2(FirstItemPos.x + size.x, LoadInRectPos.y + size.y);
            }
            return pos;
        }
        if (beginDir == BeginDir.DownLeft)
        {
            var pos = LoadInRectPos - new Vector2(size.x, 0);
            if (pos.x > LastItemPos.x)
            {
                pos = new Vector2(FirstItemPos.x - size.x, LoadInRectPos.y - size.y);
            }
            return pos;
        }
        if (beginDir == BeginDir.DownRight)
        {
            var pos = LoadInRectPos + new Vector2(size.x, 0);
            if (pos.x < LastItemPos.x)
            {
                pos = new Vector2(FirstItemPos.x + size.x, LoadInRectPos.y - size.y);
            }
            return pos;
        }
        return Vector2.zero;
    }
    private Vector2 TargetEndItemPos;
    private Vector2 GetNewLoadPos(Vector2 size)
    {
        Vector2 pos = Vector2.zero;
        size = size * 0.5f;
        LoadInRectPos = GetNextLoadInRectPos(size);
        if (beginDir == BeginDir.MiddleLeft)
        {
            pos = LoadInRectPos;
            LoadInRectPos += new Vector2(size.x, 0);
            TargetEndItemPos = LoadInRectPos;
        }
        if (beginDir == BeginDir.MiddleRight)
        {
            pos = LoadInRectPos + new Vector2(size.x, 0);
            TargetEndItemPos = LoadInRectPos;
            LoadInRectPos -= new Vector2(size.x, 0);
        }
        if (beginDir == BeginDir.TopMiddle)
        {
            pos = LoadInRectPos + space;
            LoadInRectPos -= new Vector2(0, size.y);
            TargetEndItemPos = LoadInRectPos;
        }
        if (beginDir == BeginDir.DownMiddle)
        {
            pos = LoadInRectPos;
            TargetEndItemPos = LoadInRectPos;
            LoadInRectPos += new Vector2(0, size.y);
        }
        if (beginDir == BeginDir.TopLeft)
        {
            pos = LoadInRectPos;
            LoadInRectPos += new Vector2(size.x, 0);
            TargetEndItemPos = LoadInRectPos - new Vector2(0, size.y);
        }
        if (beginDir == BeginDir.TopRight)
        {
            pos = LoadInRectPos;
            TargetEndItemPos = LoadInRectPos - new Vector2(0, size.y);
            LoadInRectPos -= new Vector2(size.x, 0);
        }
        if (beginDir == BeginDir.DownLeft)
        {
            pos = LoadInRectPos;
            TargetEndItemPos = LoadInRectPos + new Vector2(0, size.y);
            LoadInRectPos += new Vector2(size.x, 0);
        }
        if (beginDir == BeginDir.DownRight)
        {
            pos = LoadInRectPos;
            TargetEndItemPos = LoadInRectPos + new Vector2(0, size.y);
            LoadInRectPos -= new Vector2(size.x, 0);
        }
        return pos;
    }
    public new void LateUpdate()
    {
        if (!IsInited)
        {
            return;
        }
        base.LateUpdate();
        if (OnSlideEvent != null)
        {
            OnSlideEvent();
        }
        if (moveState == MoveDir.idle)
        {
            return;
        }
        for (int i = 0; i < OneLen; i++)
        {
            if (moveState == MoveDir.rightToLeft || moveState == MoveDir.downToUp)
            {
                if (LastIndex != allItemPos.Count - 1)
                {
                    if (items[FirstItemIndex].CheckAllIsOut(scrollRectWorld))
                    {
                        ChangeFirstToLast();
                    }
                }
            }
            else
            {
                if (FirstIndex != 0)
                {
                    if (items[LastItemIndex].CheckAllIsOut(scrollRectWorld))
                    {
                        ChangeLastToFirst();
                    }
                }
            }
        }
    }
    private void ChangeFirstToLast(bool isSetItemEvent = true)
    {
        if (LastIndex + 1 < allItemPos.Count)
        {
            items[FirstItemIndex].anchoredPosition = allItemPos[LastIndex + 1];
            items[FirstItemIndex].SetActive(true);
            if (isSetItemEvent)
            {
                items[FirstItemIndex].SetAllGrey(false);
                ShowItemEvent(items[FirstItemIndex], LastIndex + 1, false);
            }
            else
            {
                items[FirstItemIndex].name = (LastIndex + 1) + "needSet";
            }
            FirstIndex++;
            LastIndex++;
            FirstItemIndex++;
            LastItemIndex++;
            if (FirstItemIndex > items.Count - 1)
            {
                FirstItemIndex = 0;
            }
            if (LastItemIndex > items.Count - 1)
            {
                LastItemIndex = 0;
            }
        }
    }
    private void ChangeLastToFirst(bool isSetItemEvent = true)
    {
        if (FirstIndex - 1 >= 0)
        {
            items[LastItemIndex].anchoredPosition = allItemPos[FirstIndex - 1];
            items[LastItemIndex].SetActive(true);
            if (isSetItemEvent)
            {
                items[LastItemIndex].SetAllGrey(false);
                ShowItemEvent(items[LastItemIndex], FirstIndex - 1, false);
            }
            else
            {
                items[LastItemIndex].name = (FirstIndex - 1) + "needSet";
            }
            FirstIndex--;
            LastIndex--;
            FirstItemIndex--;
            LastItemIndex--;
            if (FirstItemIndex < 0)
            {
                FirstItemIndex = items.Count - 1;
            }
            if (LastItemIndex < 0)
            {
                LastItemIndex = items.Count - 1;
            }
        }
    }
    public void MoveAndClickItemByIndex(int index, System.Action<Transform> callback = null)
    {
        var oldIndex = index;
        if (movement == Movement.Horizontal)
        {
            index -= Mathf.CeilToInt((scrollRect.y - scrollRect.x) / 2.0f / ItemDefaultSize.x);
        }
        else
        {
            index -= Mathf.CeilToInt((scrollRect.w - scrollRect.z) / 2.0f / ItemDefaultSize.y);
        }
        if (index < 0) {
            index = 0;
        }
        if (index > allItemPos.Count - 1) {
            index = allItemPos.Count - 1;
        }
        Vector2 pos = -GetInRangPos(allItemPos[index] + ItemDefaultSize * 0.5f);
        if (movement == Movement.Horizontal)
        {
            pos = new Vector2(pos.x, TargetPos.y);
        }
        else
        {
            pos = new Vector2(TargetPos.x, pos.y);
        }
        TargetPos = pos;
        Target.anchoredPosition = pos;
        int begin = FirstIndex;
        if (begin < index)
        {
            int len = index - begin - OneLen;
            if (len <= 0)
            {
                if (callback != null)
                {
                    callback(items[index]);
                }
                return;
            }
            for (int i = 0; i < len; i++)
            {
                ChangeFirstToLast(false);
            }
        }
        else
        {
            int len = begin - index - OneLen;
            if (len <= 0)
            {
                if (callback != null)
                {
                    callback(items[oldIndex]);
                }
                return;
            }
            for (int i = 0; i < begin - index; i++)
            {
                ChangeLastToFirst(false);
            }
        }
        int index1 = -1;
        foreach (var item in items)
        {
            index1++;
            if (!item.CheckIsOut(OldScrollRect) && index1 < MaxLen)
            {
                item.SetActive(true);
            }
            if (item.name.Contains("needSet"))
            {
                var itemName = item.name.Replace("needSet", "");
                item.name = itemName;
                item.SetAllGrey(false);
                ShowItemEvent(item, int.Parse(itemName), false);
            }
        }
        if (callback != null)
        {
            foreach (var item in items)
            {
                if (item.name == oldIndex.ToString())
                {
                    callback(item);
                    return;
                }
            }
        }
    }
    public void MoveAndClickItemByIndexSmooth(int index, Vector2 modify, System.Action<Transform> callback = null)
    {
        if (index == -1)
        {
            index = 0;
        }
        MoveToIndex(index, modify, callback);
    }
    public Vector2 GetItemInScrollPos(RectTransform rectTran)
    {
        return rectTran.anchoredPosition + Target.anchoredPosition;
    }
    public void Create()
    {

    }
    public void SetOnSlideEvent(System.Action action)
    {
        OnSlideEvent += action;
    }
}