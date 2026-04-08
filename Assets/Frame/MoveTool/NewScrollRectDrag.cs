using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewScrollRectDrag : ScrollRect
{
    public Vector3 beginPos=Vector3.zero;
    public Vector2 space = new Vector2(5, 5);
    public float delta = 25f;//边界范围
    private float speed = 0.1f;//拖动滑动速度

    public delegate void MoveItemDelegate(Transform tran, int index, Action<Transform> callback);

    private List<ScrollRectDragItem> items;
    private Transform[] trans;
    private Transform BaseItem;
    private RectTransform rectTran;

    private bool isScroll;
    private int maxLen;
    private List<int> indexList;
    private Action<List<int>, List<Transform>> ChangeCallback;

    #region 动态填充
    public void InitItems(int maxlen, MoveItemDelegate showItems)
    {
        maxLen = maxlen;
        isScroll = false;
        
        if (items == null)
        {
            items = new List<ScrollRectDragItem>();
            BaseItem = content.GetChild(0);
            BaseItem.SetActive(false);
            rectTran = transform.GetComponent<RectTransform>();
            trans = content.AddChilds(maxlen);
        }

        Vector2 sizeDelta = BaseItem.GetComponent<RectTransform>().sizeDelta;
        content.sizeDelta = new Vector2(sizeDelta.x, (sizeDelta.y + space.y) * maxlen - space.y);

        int index = 0;
        foreach (Transform _tran in trans)
        {
            showItems(_tran, index, (tran) =>
            {
                if (tran != null && tran.GetComponent<ScrollRectDragItem>()==null)
                {
                    ScrollRectDragItem srdi = tran.gameObject.AddComponent<ScrollRectDragItem>();
                    srdi.Init(OnItemBeginDrag, OnItemDrag, OnItemEndDrag);
                    srdi.ParentTran = _tran;
                    srdi.ScrollRectTran = rectTran;
                    items.Add(srdi);
                }
            });
            _tran.localPosition = new Vector3(0, -index * (sizeDelta.y + space.y));
            index++;
        }
    }

    private void InitIndexList()
    {
        indexList = new List<int>();
        for(int i = 0; i < maxLen; i++)
        {
            indexList.Add(i);
        }
    }

    public void SetDragOverCallback(Action<List<int>, List<Transform>> callback)
    {
        ChangeCallback = callback;
    }
    #endregion

    #region 拖拽滑动
    private Vector3 step;//暂存拖拽滑动距离
    Coroutine dragCoroutine;
    private bool isMove;
    private ScrollRectDragItem dragItem;
    private void OnItemBeginDrag(ScrollRectDragItem item)
    {
        InitIndexList();
        isMove = false;
        dragItem = item;
        dragCoroutine = StartCoroutine(MoveIE());
        item.ParentTran.SetAsLastSibling();
    }
    private IEnumerator MoveIE() {
        while (true) {
            if(isMove)
                Move(step);
            yield return null;
        }
    }

    private void OnItemDrag(ScrollRectDragItem item, Vector2 scrollPos, Vector3 worldPos)
    {
        if (isOverBorder(scrollPos))
        {
            if ((worldPos - item.OldPos).y < 0)
                step = new Vector3(0, -speed, 0);
            else
                step = new Vector3(0, speed, 0);
            isMove = true;
        }
        else
        {
            item.OldPos = worldPos;
            isMove = false;
            item.ParentTran.position = new Vector3(item.ParentTran.position.x, item.OffsetPos.y + worldPos.y, item.ParentTran.position.z);
        }

        CheckIndex(item);
    }

    private void OnItemEndDrag(ScrollRectDragItem item)
    {
        StopCoroutine(dragCoroutine);
        Change(Index(item));
        List<Transform> trans = new List<Transform>();
        for (int i = 0; i < maxLen; i++)
        {
            trans.Add(items[i].ParentTran);
        }
        if(ChangeCallback!=null)
            ChangeCallback(indexList, trans);
        indexList = null;
        isScroll = false;
    }

    private bool isOverBorder(Vector2 pos)
    {
        if ((pos.y < (-rectTran.rect.height / 2 + delta)) || (pos.y > (rectTran.rect.height / 2 - delta)))
            return true;
        return false;
    }

    private int Index(ScrollRectDragItem item)
    {
        return items.IndexOf(item);
    }

    private int TransformToIndex(Transform tran)
    {
        Vector2 sizeDelta = BaseItem.GetComponent<RectTransform>().sizeDelta;
        float y;
        for(int i = 0; i < items.Count; i++)
        {
            y = -i * (sizeDelta.y + space.y) - (sizeDelta.y / 2);
            if (tran.localPosition.y > y - sizeDelta.y / 2) return i; 
        }
        return -1;
    }

    private void CheckIndex(ScrollRectDragItem item)
    {
        int index = TransformToIndex(item.ParentTran);
        if (index != Index(item) && index >= 0)
        {
            Change(Index(item), index);
        }
    }

    public void Change(int index, int index2=-1)
    {
        Vector2 sizeDelta = BaseItem.GetComponent<RectTransform>().sizeDelta;
        ScrollRectDragItem cache;
        Transform cacheTran;
        if (index2==-1)
            items[index].ParentTran.localPosition = new Vector3(0, -index * (sizeDelta.y + space.y));
        else
        {
            if (indexList != null)
            {
                indexList[index] ^= indexList[index2];
                indexList[index2] ^= indexList[index];
                indexList[index] ^= indexList[index2];
            }
            else
            {
                items[index].ParentTran.localPosition = new Vector3(0, -index2 * (sizeDelta.y + space.y));
            }

            items[index2].ParentTran.localPosition = new Vector3(0, -index * (sizeDelta.y + space.y));
            cache = items[index];
            items[index] = items[index2];
            items[index2] = cache;
            cacheTran = trans[index];
            trans[index] = trans[index2];
            trans[index2] = cacheTran;

            items[index].ParentTran.name = index.ToString();
            items[index2].ParentTran.name = index2.ToString();
        }
    }
    #endregion

    #region 联动滑动
    public bool IsScroll
    {
        get
        {
            return isScroll;
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        isScroll = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        isScroll = false;
    }

    private void Move(Vector3 v3)
    {
        v3.x = 0;
        Vector3 vv = content.position - v3;
        vv = content.parent.InverseTransformPoint(vv);
        if (vv.y < 0 || vv.y > content.GetComponent<RectTransform>().rect.height - rectTran.rect.height)
            return;
        content.position -= v3;
        dragItem.ParentTran.position += v3;
        isScroll = true;

        CheckIndex(dragItem);
    }
    #endregion
}