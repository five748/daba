using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offsetPos = Vector3.zero;  //临时记录点击点与UI的相对位置
    private RectTransform rt;
    public bool isCopy;
    public System.Action BeginDragEvent;
    public System.Action<Transform, Transform> EndDragEvent;
    public Transform EndParentTran;
    public bool IsCheckChild = true;
    public float Dis = 100;
    private Vector3 currPos;
    public Transform dragParent;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 newPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPos);
        rt.position = newPos - offsetPos;
        CheckDrag();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (dragParent == null) {
            dragParent = transform.parent;
        }
        rt = GameObject.Instantiate(transform.gameObject, dragParent).transform.GetComponent<RectTransform>();
        rt.position = transform.position;
        rt.transform.GetOrAddComponent<LayoutElement>();
        rt.transform.SetDepthTop();
        if (!isCopy)
        {
            transform.SetActive(false);
        }
        Vector3 newPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, Input.mousePosition, eventData.enterEventCamera, out newPos);
        offsetPos = newPos - rt.position;
        rt.localScale *= 1.2f;
        if (BeginDragEvent != null)
        {
            BeginDragEvent();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EndCheck();
        if (isCopy)
        {
            GameObject.Destroy(rt.gameObject);
        }
        else {
            transform.SetActive(true);
        }
        
    }
    private void CheckDrag() {
        if (EndDragEvent == null || EndParentTran == null)
        {
            return;
        }
        if (IsCheckChild)
        {
            for (int i = 0; i < EndParentTran.childCount; i++)
            {
                var child = EndParentTran.GetChild(i);
                if (Vector2.Distance(rt.position, child.position) < Dis)
                {
                    child.localScale = Vector3.one * 1.2f;
                }
                else
                {
                    child.localScale = Vector3.one;
                }
            }
        }
    }
    private void EndCheck() {
        if (EndDragEvent == null || EndParentTran == null)
        {
            return;
        }
        if (IsCheckChild)
        {
            for (int i = 0; i < EndParentTran.childCount; i++)
            {
                var child = EndParentTran.GetChild(i);
                child.localScale = Vector3.one;
            }
            for (int i = 0; i < EndParentTran.childCount; i++)
            {
                var child = EndParentTran.GetChild(i);
                if (CheckOne(child))
                {
                    return;
                }
            }
        }
        else {
            CheckOne(EndParentTran);
        }
    }
    private bool CheckOne(Transform tran) {
        //Debug.LogError(Vector2.Distance(rt.position, tran.position));
        if (Vector2.Distance(rt.position, tran.position) < Dis)
        { 
            EndDragEvent(tran, transform);
            return true;
        }
        else {
            return false;
        }
    }
}