using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollRectDragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 oldPos;
    private Vector3 offsetPos;
    private Action<ScrollRectDragItem> actionForBeginDrag;
    private Action<ScrollRectDragItem, Vector2, Vector3> actionForDrag;
    private Action<ScrollRectDragItem> actionForEndDrag;

    private Transform parentTran;
    private RectTransform scrollRectTran;

    private RectTransform rectTran;

    public Vector3 OldPos
    {
        set
        {
            oldPos = value;
        }
        get
        {
            return oldPos;
        }
    }

    public Transform ParentTran
    {
        set
        {
            parentTran = value;
        }
        get
        {
            return parentTran;
        }
    }

    public Vector3 OffsetPos
    {
        get
        {
            return offsetPos;
        }
    }

    public RectTransform ScrollRectTran
    {
        set
        {
            scrollRectTran = value;
        }
    }

    public void Init(Action<ScrollRectDragItem> actionForBeginDrag, Action<ScrollRectDragItem, Vector2, Vector3> actionForDrag, Action<ScrollRectDragItem> actionForEndDrag)
    {
        this.actionForBeginDrag = actionForBeginDrag;
        this.actionForDrag = actionForDrag;
        this.actionForEndDrag = actionForEndDrag;
        rectTran = transform.GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 newPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTran, Input.mousePosition, eventData.enterEventCamera, out newPos);
        oldPos = newPos;
        offsetPos = parentTran.position - newPos;

        actionForBeginDrag(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 vector2;
        Vector3 vector3;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scrollRectTran, Input.mousePosition, eventData.enterEventCamera, out vector2);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTran, Input.mousePosition, eventData.enterEventCamera, out vector3);

        actionForDrag(this, vector2, vector3);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        actionForEndDrag(this);
    }
}