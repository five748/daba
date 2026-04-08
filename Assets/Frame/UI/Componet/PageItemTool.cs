using System;
using System.Threading.Tasks;
using System.Transactions;
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageItemTool : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    private ScrollRect scrollRect;
    private Transform content;
    public int direction = 1;
    public float smoothing = 4;
    private float[] pageArray = new float[50];
    public Image pre;
    public Image next;
    private float targetVerticalPosition = 1;
    private float targetHorizontalPosition = 0;
    public int nowpage = 0;
    private int pageNum = 10;
    private bool isDraging = false;
    // Use this for initialization
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
        content = gameObject.transform.GetChild(0);
        EventTriggerListener.Get(pre.gameObject).onClick = (btn) =>
        {
            MoveToPage(nowpage - 1);
        };
        EventTriggerListener.Get(next.gameObject).onClick = (btn) =>
        {
            MoveToPage(nowpage + 1);
        };
        if (direction == 1)
        {
            targetVerticalPosition = 1;
        }
        else
        {
            targetHorizontalPosition = 0;
        }
    }
    public void ReStart(int num)
    {
        scrollRect = GetComponent<ScrollRect>();
        //scrollRect.verticalNormalizedPosition = 1;
        scrollRect.horizontalNormalizedPosition = 0;
        scrollRect.horizontal = false;
        pre.gameObject.transform.SetAllGrey(true);
        next.gameObject.transform.SetAllGrey(false);
        //targetVerticalPosition = 1;
        targetHorizontalPosition = 0;
        nowpage = 0;
        pageNum = num;
        for (int i = 0; i < pageNum; i++)
        {
            pageArray[i] = (float)i / (num - 1);
        }
    }


    // Update is called once per frame
    // void Update()
    // {
    //     if (isDraging == false)
    //     {
    //         scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition,
    //             targetHorizontalPosition, Time.deltaTime * smoothing);
    //         float offset = Mathf.Abs(pageArray[nowpage] - targetHorizontalPosition);
    //         if (offset <= 0.00005f)
    //         {
    //             scrollRect.horizontal = false;
    //         }
    //     }
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        float pos = scrollRect.horizontalNormalizedPosition;
        int index = 0;
        float offset = Mathf.Abs(pageArray[index] - pos);
        for (int i = 1; i < pageArray.Length; i++)
        {
            float offsetTemp = Mathf.Abs(pageArray[i] - pos);
            if (offsetTemp < offset)
            {
                index = i;
                offset = offsetTemp;
            }
        }
        targetHorizontalPosition = pageArray[index];
    }

    public void MoveToPage(int index)
    {
        if (index < 0 || index >= pageNum) return;
        scrollRect.horizontal = true;
        Debug.Log("跳转到：第" + index.ToString());
        targetHorizontalPosition = pageArray[index];
        scrollRect.horizontalNormalizedPosition = pageArray[index];
        scrollRect.horizontal = false;
        nowpage = index;
        pre.gameObject.transform.SetAllGrey(false);
        next.gameObject.transform.SetAllGrey(false);
        if (nowpage == 0)
        {
            pre.gameObject.transform.SetAllGrey(true);
        }
        else if (nowpage == pageNum - 1)
        {
            next.gameObject.transform.SetAllGrey(true);
        }
    }
}
