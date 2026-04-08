using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnGuide : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Transform BtnAim;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (BtnAim == null) {
            return;
        }
        
        ExecuteEvents.Execute(BtnAim.gameObject, new PointerEventData(EventSystem.current),ExecuteEvents.pointerDownHandler);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!gameObject.activeSelf)
        {
            return;
        }
        if (BtnAim == null)
        {
            return;
        }

        ExecuteEvents.Execute(BtnAim.gameObject, new PointerEventData(EventSystem.current),ExecuteEvents.pointerUpHandler);
    }
}