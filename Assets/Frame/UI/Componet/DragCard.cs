using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCard : DragRota
{
    private int firstAngleIndex = 0;
    private int lastAngelIndex = 0;
    private int firstDataIndex = 0;
    private int lastDataIndex = 0;
    private int firstItemIndex = 0;
    private int lastItemIndex = 0;
    private Vector4 OldScrollRect;
    private int curAngleIndex = 0;
    private int curItemIndex = 0;
    private bool canDrag = false;

    private int maxItemCount = 0;
    private int maxDataCount = 0;
    private int maxShowCount = 0;
    private int showItemCount = 0;
    private int canDragItemCount = 0;
    private int addItemCount = 0;
    private List<RectTransform> ItemLists = new List<RectTransform>();
    private System.Func<RectTransform, int, bool> ShowItemEvent;
    public System.Action<int> ClickCutAction;

    private RectTransform curItem;
    private int curDataIndex;
    public void InitItems(int itemCount, System.Func<RectTransform, int, bool> showItems)
    {
        var scrollRectWorld = GetComponent<RectTransform>().GetRangeWorld();
        OldScrollRect = scrollRectWorld + new Vector4(-0.1f, 0.1f, -0.1f, 0.1f);
        itemSize = transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta;
        itemOffset = new Vector2(10f, 0f);
        maxItemCount = (int)(360 / angleOne);
        maxDataCount = itemCount;
        showItemCount = 7;
        canDragItemCount = showItemCount - 1;
        addItemCount = 2;
        maxShowCount = showItemCount + addItemCount;
        ShowItemEvent = showItems;
        //拖动中卡牌切换 拖动结束礼包内容刷新效果
        isDragPage = true;
        DragEndEvent = () =>
        {
            isClick = true;
            var center = ItemLists[ReturnItemIndex(curItemIndex)];
            var index = ReturnDataIndex(curDataIndex);
            cutItemEvent?.Invoke(center, index, true);
        };

        canDrag = itemCount >= canDragItemCount;
        var itemCounts = canDrag ? maxShowCount : itemCount;
        if (canDrag)
        {
            ChangeItemEvent = ChangeItem;
        }
        else
        {
            IsLockDrag = true;
        }

        if (radius == 0)
        {
            GetRadius();
        }

        var items = transform.GetChild(0).AddChilds(itemCounts);
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i].GetComponent<RectTransform>();
            ItemLists.Add(item);
        }

        curAngleIndex = canDrag ? maxShowCount / 2 > 4 ? 4 : maxShowCount / 2 : maxDataCount / 2;
        curDataIndex = curAngleIndex;
        curItemIndex = curAngleIndex;
        curItem = ItemLists[curAngleIndex];
        OnTargetRota = curAngleIndex * angleOne;
        TargetRota = curAngleIndex * angleOne;
        Target.rotation = Quaternion.Euler(0, 0, curAngleIndex * angleOne);

        for (int i = 0; i < ItemLists.Count; i++)
        {
            var item = ItemLists[i];
            SetItemByIndex(item, int.Parse(item.name), i);
        }

        firstAngleIndex = 0;
        lastAngelIndex = maxShowCount - 1;
        firstDataIndex = 0;
        lastDataIndex = maxShowCount - 1;
        firstItemIndex = 0;
        lastItemIndex = maxShowCount - 1;
    }

    private Coroutine MoveCo;
    public void SetRotateByIndex(RectTransform item, int index)
    {
        //leftToRight: curItem.name > item.name         1  57
        //rightToLeft: curItem.name < item.name
        var curIndex = int.Parse(curItem.name);
        var curdataindex = curDataIndex;
        var nowIndex = int.Parse(item.name);
        var diff = curAngleIndex - nowIndex;
        var num = Mathf.Abs(diff);
        var isTurn = num >= showItemCount;
        diff = isTurn ? -diff : diff;
        moveState = diff > 0 ? MoveDir.leftToRight : MoveDir.rightToLeft;
        if (isTurn)
        {
            var min = curAngleIndex < nowIndex ? curAngleIndex : nowIndex;
            var max = curAngleIndex > nowIndex ? curAngleIndex : nowIndex;
            num = min + maxItemCount - max;
        }

        curItem = item;
        curDataIndex = index;
        //刷新之前的选中
        for (int i = 0; i < ItemLists.Count; i++)
        {
            var one = ItemLists[i];
            var oneIndex = int.Parse(one.name);

            if (Mathf.Abs(oneIndex - curIndex) % maxDataCount == 0)
            {
                SetItemByIndex(one, oneIndex, curdataindex);
            }
        }
        //刷新当前的选中
        SetItemByIndex(item, nowIndex, index);
        if (isClick)
        {
            isClick = false;
            //设置旋转角度
            OnTargetRota = nowIndex * angleOne;
            TargetRota = nowIndex * angleOne;
            var rotate = nowIndex * angleOne;
            MoveCo.Stop();
            MoveCo = MonoTool.Instance.StartCor(MoveToRotate(rotate, 0.2f, null));
        }

        for (int i = 0; i < num; i++)
        {
            if (moveState == MoveDir.rightToLeft)
            {
                ChangeFirstToLast();
            }
            if (moveState == MoveDir.leftToRight)
            {
                ChangeLastToFirst();
            }
        }
    }

    private IEnumerator MoveToRotate(float rotate, float time, System.Action callback = null)
    {
        //Target.rotation = Quaternion.Euler(0, 0, rotate);
        float t = 0;
        while (true)
        {
            t += Time.deltaTime;
            float a = t / time;
            Target.rotation = Quaternion.Lerp(Target.rotation, Quaternion.Euler(0, 0, rotate), 0.5f);
            if (a >= 1.0f)
            {
                Target.rotation = Quaternion.Euler(0, 0, rotate);
                callback?.Invoke();
                break;
            }
            yield return null;
        }

        yield return null;
    }

    private void SetItemByIndex(RectTransform item, int itemIndex, int dataIndex)
    {
        item.name = itemIndex.ToString();

        var rad = dataIndex == curDataIndex ? radius + 15f : radius;
        //var rad = radius;
        var angle = itemIndex * angleOne;
        var x = Mathf.Cos(Mathf.PI / 180f * (90 - angle)) * rad;
        var y = Mathf.Cos(Mathf.PI / 180f * angle) * rad;
        item.anchoredPosition = new Vector2(x, y);
        item.localEulerAngles = new Vector3(item.eulerAngles.x, item.eulerAngles.y, -angle);

        ShowItemEvent?.Invoke(item, dataIndex % maxDataCount);
        curItem = curDataIndex == dataIndex % maxDataCount ? item : curItem;
    }

    private System.Func<RectTransform, int, bool, bool> cutItemEvent;
    private bool isClick = false;
    //左右按钮点击事件
    public void ClickCut(int dir, RectTransform card = null, int index = 0)
    {
        isClick = true;
        if (dir == 0)
        {
            cutItemEvent?.Invoke(card, index, true);
            return;
        }

        var num = dir > 0 ? 1 : -1;
        var curItemIndex = 0;
        for (int i = 0; i < ItemLists.Count; i++)
        {
            if (curItem.name == ItemLists[i].name)
            {
                curItemIndex = i;
            }
        }

        curItemIndex = canDrag ? ReturnItemIndex(curItemIndex + num) : ReturnDataIndex(curItemIndex + num);
        cutItemEvent?.Invoke(ItemLists[curItemIndex], ReturnDataIndex(curDataIndex + num), true);
    }

    public void SetCutItemEvent(System.Func<RectTransform, int, bool, bool> cutItem)
    {
        if (cutItemEvent == null)
        {
            cutItemEvent = cutItem;
        }
    }

    private void ChangeItem()
    {
        if (moveState == MoveDir.idle)
        {
            return;
        }
        for (int i = 0; i < 1; i++)
        {
            if (moveState == MoveDir.rightToLeft)
            {
                if (ItemLists[firstItemIndex].CheckAllIsOut(OldScrollRect))
                {
                    ChangeFirstToLast();

                    //居中选中功能
                    var center = ItemLists[ReturnItemIndex(curItemIndex)];
                    var index = ReturnDataIndex(curDataIndex + 1);
                    cutItemEvent?.Invoke(center, index, false);
                }
            }
            else
            {
                if (ItemLists[lastItemIndex].CheckAllIsOut(OldScrollRect))
                {
                    ChangeLastToFirst();

                    //居中选中功能
                    var center = ItemLists[ReturnItemIndex(curItemIndex)];
                    var index = ReturnDataIndex(curDataIndex - 1);
                    cutItemEvent?.Invoke(center, index, false);
                }
            }
        }

    }

    //计算半径
    private void GetRadius()
    {
        var offset = itemSize.x + itemOffset.x;
        radius = offset / 2 / Mathf.Sin(Mathf.PI / 180f * (angleOne / 2));
        var th = transform.GetChild(0).GetComponent<RectTransform>();
        th.anchoredPosition = new Vector2(th.anchoredPosition.x, th.anchoredPosition.y - radius);
    }

    //更新索引数据 移动item
    private void ChangeFirstToLast()
    {
        if (IsLockDrag)
        {
            return;
        }

        var item = ItemLists[firstItemIndex];

        curAngleIndex = ReturnAngleIndex(curAngleIndex + 1);
        curItemIndex = ReturnItemIndex(curItemIndex + 1);
        lastAngelIndex = ReturnAngleIndex(lastAngelIndex + 1);
        firstAngleIndex = ReturnAngleIndex(firstAngleIndex + 1);
        lastDataIndex = ReturnDataIndex(lastDataIndex + 1);
        firstDataIndex = ReturnDataIndex(firstDataIndex + 1);
        lastItemIndex = ReturnItemIndex(lastItemIndex + 1);
        firstItemIndex = ReturnItemIndex(firstItemIndex + 1);

        SetItemByIndex(item, lastAngelIndex, lastDataIndex);
    }
    private void ChangeLastToFirst()
    {
        if (IsLockDrag)
        {
            return;
        }

        var item = ItemLists[lastItemIndex];

        curAngleIndex = ReturnAngleIndex(curAngleIndex - 1);
        curItemIndex = ReturnItemIndex(curItemIndex - 1);
        lastAngelIndex = ReturnAngleIndex(lastAngelIndex - 1);
        firstAngleIndex = ReturnAngleIndex(firstAngleIndex - 1);
        lastDataIndex = ReturnDataIndex(lastDataIndex - 1);
        firstDataIndex = ReturnDataIndex(firstDataIndex - 1);
        lastItemIndex = ReturnItemIndex(lastItemIndex - 1);
        firstItemIndex = ReturnItemIndex(firstItemIndex - 1);

        SetItemByIndex(item, firstAngleIndex, firstDataIndex);
    }

    private int ReturnAngleIndex(int i)
    {
        var index = i >= maxItemCount ? i - maxItemCount : i < 0 ? maxItemCount + i : i;

        return index;
    }

    private int ReturnDataIndex(int i)
    {
        var index = i >= maxDataCount ? i - maxDataCount : i < 0 ? maxDataCount + i : i;

        return index;
    }
    private int ReturnItemIndex(int i)
    {
        var index = i >= maxShowCount ? i - maxShowCount : i < 0 ? maxShowCount + i : i;

        return index;
    }

}
