using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoopTalk : DragScroll
{
    public Transform BeginTran;
    public Transform LastTran;
    public Scrollbar scrbar;
    private RectTransform GridTran;
    private float NeedMoveDistance;
    private int ItemLen;
    public RectTransform[] Items;
    private int _one;
    public int One
    {
        get
        {
            return _one;
        }
        set
        {
            //Debug.LogError(value);
            _one = value;
        }
    }
    public int Last;
    public float FirstPos;
    public float LastPos;
    public int LoadFirstItem;
    public int _loadLastItem = 0;
    private int LoadedLastMax;
    public int LoadLastItem
    {
        get
        {
            return _loadLastItem;
        }
        set
        {
            _loadLastItem = value;
            if (value > LoadedLastMax)
            {
                //print(_loadLastItem + ":" + LoadedLastMax);
                //MaxRight += itemH;
                LoadedLastMax = value;
                //    //FirstMoveToEnd();
            }
        }
    }
    private bool CanDragDown = true;
    private bool CanDragUp = false;
    private int _maxLen = 0;
    private float ScrollWidth = 703f;
    private float ScrollHeight = 500f;
    private bool isCanMove = false;
    private bool isCanLoad = false;
    public float AddDistanScan = 0f;
    private float oldPos;
    public Transform Choose;
    public string ChooseName;
    public System.Action MoveOutEvent;
    private float maxShowRight;

    public int MaxLen
    {
        get
        {
            return _maxLen;
        }
        set
        {
            _maxLen = value;
            SetMax();
        }
    }
    private Vector2 UpPos;
    private Vector2 DownPos;
    public System.Action<int> InitEvent;
    public MoveItemDelegate MoveItemEvent;
    private bool isInited = false;
    private int _minLen = 0;
    private Vector2[] OldPoss;
    private Vector2 _space;
    public void MoveToIndex(int index)
    {
        float pos = 0;
        if (index < _minLen / 2)
        {
            pos = MaxLeft;
        }
        else if (index > _maxLen - _minLen / 2)
        {
            pos = MaxRight;
        }
        else
        {
            pos = (index - _minLen / 2) * diff + oldPos;
        }
        Vector2 targetpos = Vector2.zero;
        if (movement == Movement.Horizontal)
        {
            targetpos = new Vector2((float)pos, Target.anchoredPosition.y);
        }
        else
        {
            targetpos = new Vector2(Target.anchoredPosition.x, (float)pos);
        }
        MoveToTarget(targetpos);
    }
    private int outMaxLen = 0;
    public void InitLoop(int maxLen, int minLen, MoveItemDelegate ShowItem, System.Action<GameObject> ClickEvent = null, System.Action LoadEvent = null)
    {
        isCanUpdate = false;
        if (!isLoaded)
        {
            _minLen = minLen;
            Init();
            outMaxLen = maxLen;
            isCanMove = outMaxLen > ItemLen;
            MoveItemEvent = ShowItem;
            MoveOutEvent = LoadEvent;
            //isLoaded = true;
        }
        else
        {
            TargetPos = oldTranPos;
            One = 0;
            Last = ItemLen - 1;
            LoadFirstItem = 1;
            for (int i = 0; i < ItemLen; i++)
            {
                Items[i].anchoredPosition = OldPoss[i];
                Items[i].name = i.ToString();
                Items[i].gameObject.SetActive(false);
            }
            outMaxLen = maxLen;
            isCanUpdate = false;
            isCanMove = outMaxLen > ItemLen;
            if (ChooseName != "")
            {
                ChooseItem.Find(ChooseName).gameObject.SetActive(false);
            }
            Target.anchoredPosition = Vector2.zero;
        }
        isInitedLoop = false;
        InitLoopItems(outMaxLen, ClickEvent);
        SetDirect();
    }
    public delegate int MoveItemDelegate(Transform tran, int index, bool isNeedShow = true);
    public void InitLoop(Vector2 space, int maxLen, int minLen, MoveItemDelegate ShowItem, System.Action<GameObject> ClickEvent = null, System.Action LoadEvent = null)
    {
        _space = space;
        newMessageIndex = maxLen;
        newMessageItem = new List<RectTransform>();
        InitLoop(maxLen, minLen, ShowItem, ClickEvent, LoadEvent);
    }
    private int ChooseIndex;
    private Transform ChooseItem;
    private bool isInitedLoop = false;
    private void InitLoopItems(int num, System.Action<GameObject> ClickEvent)
    {
        int len = Items.Length;
        if (isInitedLoop)
        {
            for (int i = 0; i < num; i++)
            {
                MoveItemEvent(Items[i % len], i, true);
            }
            return;
        }
        isInitedLoop = true;
        isCanUpdate = false;

        int sum = 0;
        int add = 0;
        int addlen = 0;
        float sumdiff = 0;
        itemHeights = new Dictionary<int, float>();
        for (int i = 0; i < num; i++)
        {
            add = MoveItemEvent(Items[i % len], i, false);
            itemH = add;
            oldAdd = add;
            //print("add:" + i + "|" + add + "|" + sumdiff);
            sum += add;
            if (i == 0)
            {
                itemHeights.Add(0, 0);
            }
            else
            {
                itemHeights.Add(i, -sumdiff);
            }
            sumdiff += add + diff;
        }
        _maxLen = num;
        if (sumdiff > ScrollHeight)
        {
            MaxRight = diff * (MaxLen) - ScrollHeight + AddDistanScan * diff + sum;
            IsFirstOut = false;
            cacheMaxRight = MaxRight;
            Target.anchoredPosition = new Vector2(0, MaxRight);
        }
        else
        {
            MaxRight = 0;
            cacheMaxRight = sumdiff;
            IsFirstOut = true;
            LoadLastItem = num - 1;
        }
        if (num > len)
        {
            addlen = num - len;
            LoadLastItem = num - 1;
            LoadFirstItem = num - len + 1;
        }
        else
        {
            addlen = 0;
        }
        int index = -1;
        for (int i = addlen; i < num; i++)
        {
            index++;
            Items[index].name = i.ToString();
            MoveItemEvent(Items[index], i);
            Items[index].SetActive(true);
            Items[index].anchoredPosition = new Vector2(0, itemHeights[i]);
        }
        float min = BeginTran.localPosition.y;
        float max = BeginTran.localPosition.y;
        One = 0;
        if (num < ItemLen)
            Last = num - 1;
        else
            Last = ItemLen - 1;
        //float curry = 0;
        //for (int i = 0; i < ItemLen; i++)
        //{
        //    curry = Items[i].anchoredPosition.y;
        //    if (curry < min)
        //    {
        //        min = curry;
        //        Last = i;
        //    }
        //    if (curry > max) {
        //        max = curry;
        //        One = i;
        //    }
        //}
        for (int i = 0; i < ItemLen; i++)
        {
            EventTriggerListener.Get(Items[i].gameObject).onClick = ((btn) =>
            {
                if (ChooseName != "")
                {
                    ChooseItem.Find(ChooseName).gameObject.SetActive(false);
                    ChooseItem = btn.transform;
                    ChooseItem.Find(ChooseName).gameObject.SetActive(true);
                }
                if (Choose)
                {
                    Choose.SetParent(btn.transform);
                    Choose.gameObject.SetActive(true);
                    Choose.SetAsLastSibling();
                }
                ChooseIndex = int.Parse(btn.name);
                if (ClickEvent != null)
                    ClickEvent(btn);
            });
        }
        //LoadLastItem = num;
        ChooseItem = Items[0];
        if (ChooseName != "")
        {
            ChooseItem.transform.Find(ChooseName).gameObject.SetActive(true);
        }
        if (Choose)
        {
            Choose.SetParent(ChooseItem);
            Choose.gameObject.SetActive(true);
        }
    }
    private bool isInit = false;
    private void Start()
    {

    }
    // Use this for initialization
    public bool isLoaded;
    private Vector2 oldTranPos;
    private RectTransform thisRectTran;
    public void Init()
    {
        thisRectTran = GetComponent<RectTransform>();
        ScrollWidth = thisRectTran.sizeDelta.x;
        ScrollHeight = thisRectTran.sizeDelta.y;
        //print(ScrollWidth);
        //print(ScrollHeight);
        isInit = true;
        //MaxLeft = int.MinValue;
        isInited = true;
        GridTran = transform.GetChild(0).GetComponent<RectTransform>();
        TargetPos = GridTran.anchoredPosition;
        oldTranPos = TargetPos;
        SetLoopData();
        AddListen();
    }
    private void SetItems()
    {
        if (!GridTran)
        {
            GridTran = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>();
        }
        ItemLen = GridTran.childCount;
        //print(ItemLen);
        if (ItemLen == 1)
        {
            ItemLen = _minLen + 3;
            GridTran.AddChilds(ItemLen, _space);
        }
        if (OldPoss == null)
        {
            OldPoss = new Vector2[ItemLen];
            Items = new RectTransform[ItemLen];
            for (int i = 0; i < ItemLen; i++)
            {
                Items[i] = GridTran.GetChild(i).GetComponent<RectTransform>();
                OldPoss[i] = Items[i].GetComponent<RectTransform>().anchoredPosition;
                Items[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < ItemLen; i++)
            {
                Items[i].anchoredPosition = OldPoss[i];
                Items[i].SetActive(false);
            }
            GridTran.anchoredPosition = Vector2.zero;
        }
    }
    private bool isDown = false;
    private void AddListen()
    {
        BeginMoveElasticEvent = BeginMoveOut;
        MoveElasticEvent = MoveOut;
        if (scrbar)
        {
            EventTriggerListener.Get(scrbar.gameObject).onDown = (data) =>
            {
                isDown = true;
            };
            EventTriggerListener.Get(scrbar.gameObject).onUp = (data) =>
            {
                isDown = false;
            };
            ;
        }
    }
    public float diff = 0;
    private void SetLoopData()
    {
        SetItems();
        if (InitEvent != null)
        {
            InitEvent(ItemLen);
        }
        One = 0;
        Last = ItemLen - 1;
        LoadFirstItem = 1;
        itemDistances = new float[ItemLen];
        if (movement == Movement.Horizontal)
        {
            diff = Items[0].anchoredPosition.x - Items[1].anchoredPosition.x;
            FirstPos = Items[One].anchoredPosition.x;
            LastPos = Items[Last].anchoredPosition.x;
            NeedMoveDistance = FirstPos - LastPos + diff;
            oldPos = GridTran.anchoredPosition.x;
        }
        else
        {
            diff = Items[1].anchoredPosition.y - Items[2].anchoredPosition.y;
            FirstPos = Items[One].anchoredPosition.y;
            LastPos = Items[Last].anchoredPosition.y;
            NeedMoveDistance = FirstPos - LastPos + diff;
            float diff1 = Items[1].position.y - Items[2].position.y;
            FirstPos = Items[One].position.y + diff1 * 2;
            LastPos = Items[Last].position.y + diff1 * 2;
            oldPos = GridTran.position.y;
        }
        FirstPos = BeginTran.position.y;
        LastPos = LastTran.position.y;
        OldTargetX = 0;
        Vector3[] conners = new Vector3[4];//ScrollRect四角的世界坐标
        thisRectTran.GetLocalCorners(conners);
        ScrollWidth = conners[2].x - conners[0].x;
        ScrollHeight = conners[2].y - conners[0].y;
    }
    Vector3[] conners = new Vector3[4];//ScrollRect四角的世界坐标
    private void SetMax()
    {
        if (Items == null)
        {
            SetItems();
        }
        //print(_maxLen + ":" + _minLen);
        isCanMove = outMaxLen > Items.Length;
        isCanLoad = _maxLen > ItemLen;
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].gameObject.SetActive(i < MaxLen);
        }
        if (!isCanMove)
        {
            MaxLeft = 0;
            MaxRight = 0;
            return;
        }
        if (movement == Movement.Horizontal)
        {
            MaxRight = 0;
            MaxLeft = diff * (MaxLen) + ScrollWidth + AddDistanScan * diff;
            maxShowRight = -(MaxLeft - ScrollWidth - diff);
        }
        else
        {
            MaxLeft = 0;
            MaxRight = diff * (MaxLen) - ScrollHeight + AddDistanScan * diff;
            maxShowRight = -(MaxRight + ScrollHeight - diff);
        }
        //MaxLeft = MaxLeft * 750 / Screen.width;
    }
    private bool _isChange = false;
    private void BeginMoveOut()
    {
        return;
    }
    private void MoveOut()
    {
        if (!isCanMove)
            return;
        //print(IsForward);
        if (IsForward)
        {
            print("change");
            if (MoveOutEvent != null)
            {
                MoveOutEvent();
            }
        }
    }
    private bool isChange
    {
        get
        {
            return _isChange;
        }
        set
        {

            _isChange = value;
        }
    }
    private void Update()
    {
        var flag = true;
        if (!isCanUpdate)
            flag = false;
        if (!isCanMove)
            flag = false;
        if (flag)
        {
            OnVScrollMove();
            UpDateDirect();
        }
        for (int i = 0; i < newMessageItem.Count; i++)
        {
            var item = newMessageItem[i];
            if (item.name == newMessageIndex.ToString())
            {
                if (item.CheckIsHaveIn(transform))
                {
                    if (ShowNewMessageCallback != null)
                        ShowNewMessageCallback();
                    newMessageIndex++;
                    newMessageItem.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    private int newMessageIndex;
    private List<RectTransform> newMessageItem;
    public System.Action ShowNewMessageCallback;

    int index = 0;
    float oldItmH;
    private bool MoveItemLastToOne(int last, RectTransform item)
    {
        index = LoadFirstItem - 2;
        oldItmH = itemDistances[last];
        if (MoveItemEvent != null)
        {
            if (index >= 0 && index < MaxLen)
            {
                item.name = index.ToString();
                itemDistances[last] = MoveItemEvent(item, index);
                item.anchoredPosition = new Vector2(0, itemHeights[index]);
                return true;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    private bool MoveItemOneToLast(int one, RectTransform item)
    {
        index = LoadLastItem + 1;
        oldItmH = itemDistances[one];
        if (MoveItemEvent != null)
        {
            if (index >= 0 && index < MaxLen)
            {
                item.name = (index).ToString();
                itemDistances[one] = MoveItemEvent(item, index);
                item.anchoredPosition = new Vector2(0, itemHeights[index]);
                if (index >= newMessageIndex)
                {
                    if (!newMessageItem.Contains(item))
                        newMessageItem.Add(item);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }
    private float[] itemDistances;

    public float OldTargetX = 0;
    private float MoveDistancX = 0;


    private void OnVScrollMove()
    {
        if (!IsForward)
        {
            if (Items[Last].position.y <= LastPos)
            {
                if (!MoveItemLastToOne(Last, Items[Last]))
                {
                    return;
                }
                Last--;
                if (Last < 0)
                {
                    Last = ItemLen - 1;
                }
                One--;
                if (One < 0)
                {
                    One = ItemLen - 1;
                }
                LoadFirstItem--;
                LoadLastItem--;
                if (LoadFirstItem <= 0)
                {
                    CanDragDown = false;
                }
            }
        }
        else
        {

            if (Items[One].position.y >= FirstPos)
            {
                if (!MoveItemOneToLast(One, Items[One]))
                {
                    return;
                }
                Last++;
                if (Last >= ItemLen)
                {
                    Last = 0;
                }
                One++;
                if (One >= ItemLen)
                {
                    One = 0;
                }
                LoadFirstItem++;
                LoadLastItem++;
            }
        }

        //for (int i = 0; i < Items.Length; i++)
        //{

        //}
    }

    //滑动箭头扩展
    public Transform Left;
    public Transform Right;
    private bool isHaveDirect = true;
    private void SetDirect()
    {
        if (_maxLen > _minLen)
        {
            if (Left)
                Left.gameObject.SetActive(false);
            if (Right)
                Right.gameObject.SetActive(true);
        }
        else
        {
            if (Left)
                Left.gameObject.SetActive(false);
            if (Right)
                Right.gameObject.SetActive(false);
            return;
        }
    }
    private float scrollpos
    {
        get
        {
            if (movement == Movement.Horizontal)
            {
                return Target.anchoredPosition.x;
            }
            else
            {
                return Target.anchoredPosition.y;
            }
        }
    }
    private void UpDateDirect()
    {
        if (scrollpos <= _maxLeft)
        {
            if (Left)
                Left.gameObject.SetActive(true);
            if (Right)
                Right.gameObject.SetActive(false);
        }
        else if (scrollpos >= MaxRight)
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
                Right.gameObject.SetActive(true);
        }
    }
    public void OnMinLeft()
    {
        if (Left)
            Left.gameObject.SetActive(false);
        if (Right)
            Right.gameObject.SetActive(true);
    }
    public void OnMaxRight()
    {
        if (Left)
            Left.gameObject.SetActive(true);
        if (Right)
            Right.gameObject.SetActive(true);
    }

    //addone
    private float itemH;
    private bool IsFirstOut = true;
    private float cacheMaxRight;
    private int oldAdd;
    private Dictionary<int, float> itemHeights = new Dictionary<int, float>();
    private float UpDistance;
    private float DownDistance;
    private float positonH;
    public bool AddOne(int _itemH)
    {
        itemH = _itemH;
        cacheMaxRight += itemH - _space.y;

        if (_maxLen == 0)
        {
            positonH = 0;
            LoadLastItem++;
            itemDistances[_maxLen] = _itemH;
            itemHeights.Add(_maxLen, 0);
        }
        else
        {
            if (_maxLen < Items.Length)
            {
                LoadLastItem++;
                Items[_maxLen].anchoredPosition = Items[_maxLen - 1].anchoredPosition - new Vector2(0, oldAdd + diff);

                itemDistances[_maxLen] = _itemH;
            }
            itemHeights.Add(_maxLen, itemHeights[_maxLen - 1] - oldAdd - diff);
        }
        if (_maxLen < Items.Length)
        {
            MoveItemEvent(Items[_maxLen], _maxLen);
            if (!newMessageItem.Contains(Items[_maxLen]))
                newMessageItem.Add(Items[_maxLen]);
        }
        _maxLen++;

        if (_maxLen <= Items.Length)
        {
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].gameObject.SetActive(i < MaxLen);
            }
        }

        if (_maxLen >= Items.Length)
        {
            Last = Items.Length - 1;
            isCanMove = true;
            isMoveOut = false;
            isCanUpdate = true;
        }
        if (IsFirstOut)
        {
            //print(transform.GetComponent<RectTransform>().rect.height);
            if (cacheMaxRight > ScrollHeight)
            {
                IsFirstOut = false;
                MaxRight = cacheMaxRight - ScrollHeight;
                cacheMaxRight = MaxRight;
            }
            else
            {
                MaxRight = 0;
            }
        }
        else
        {
            MaxRight = cacheMaxRight;
        }
        oldAdd = _itemH;
        IsForward = true;
        if (Target == null)
        {
            return false;
        }
        if (Target.anchoredPosition.y >= MaxRight + _space.y - _itemH)
        {
            MoveToEnd();
            return false;
        }
        return true;
    }

    public void MoveToEnd()
    {
        MoveToEndFast(new Vector2(0, (float)MaxRight));
    }
    private void ModifyPos()
    {

    }
}







