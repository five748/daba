using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragCopyUI: MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 pos = Vector3.zero; //控件初始位置
    private Vector2 mousePos; //鼠标初始位置
    private RectTransform canvasRec; //控件所在画布
    private RectTransform spineRt; //复制出来的物体
    public System.Action<PointerEventData, GameObject> onDragEnd; //拖拽结束时候的回调
    public System.Action<PointerEventData> onDragOn;//拖拽过程中
    public System.Action<PointerEventData> onDragBegin;//开始拖拽

    private bool start = false;

    private Transform list;
    

    public void Init(RectTransform parent, Transform list)
    {
        canvasRec = parent;
        this.list = list;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        list.GetComponent<ScrollRect>().OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (start)
        {
            Vector2 newVec;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRec, eventData.position, eventData.pressEventCamera, out newVec);
            //鼠标移动在画布空间的位置增量
            Vector3 offset = new Vector3(newVec.x - mousePos.x, newVec.y - mousePos.y, 0);
            //原始位置增加位置增量即为现在位置
            spineRt.anchoredPosition = pos + offset;
            
            onDragOn?.Invoke(eventData);
        }
        else
        {
            // pressPosition
            // Debug.Log($"press位置 {eventData.pressPosition.x}  {eventData.pressPosition.y}");
            // Debug.Log($"cur位置 {eventData.position.x}  {eventData.position.y}");

            float offset_x = Math.Abs(Math.Abs(eventData.pressPosition.x) - Math.Abs(eventData.position.x));
            float offset_y = Math.Abs(Math.Abs(eventData.pressPosition.y) - Math.Abs(eventData.position.y));
            if (offset_x <= 50 && offset_y >= 50)
            {
                spineRt = Instantiate(gameObject).GetComponent<RectTransform>();
                spineRt.SetActive(true);
                //控件所在画布空间的初始位置
                // canvasRec = TranTool.GetRootCanvas(transform.GetComponent<RectTransform>()).GetComponent<RectTransform>();
                spineRt.transform.SetParent(canvasRec.transform);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRec, eventData.position, eventData.pressEventCamera, out mousePos);
                spineRt.anchoredPosition = mousePos;
                pos = mousePos;
        
                onDragBegin?.Invoke(eventData);
                start = true;
            }
            else
            {
                if (offset_x > 50)
                {
                    list.GetComponent<ScrollRect>().OnDrag(eventData);
                }
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (start && spineRt.gameObject != null)
        {
            onDragEnd?.Invoke(eventData, spineRt.gameObject);
        }
        start = false;
        list.GetComponent<ScrollRect>().OnEndDrag(eventData);
    }
}