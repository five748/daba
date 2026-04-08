using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewSlider : Slider, IBeginDragHandler, IEndDragHandler
{
    protected override void Start()
    {
        base.Start();
    }
    public int key = -1;
    public bool isAutoSize = false;
    public bool isInArray = false;
    public int keyIndex = -1;
    public void SetFill(float fill)
    {
        value = fill;
    }
    public void SetFill(float fill, int index)
    {
        if (keyIndex != index)
        {
            return;
        }
        value = fill;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
