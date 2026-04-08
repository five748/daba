using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewColliderBox2D : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private Vector3 offset;
    public System.Action<RectTransform> action;
    public float minX=-535;
    public float maxX=535;
    public float minY=-255;
    public float maxY=255;

    void Start() { }
    void Update() { }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (action != null)
            action(collider.GetComponent<RectTransform>());
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.localPosition - Input.mousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var target = Input.mousePosition + offset;
        if (target.x < minX || target.x > maxX || target.y < minY || target.y > maxY)
        {
            target.x = target.x < minX ? minX : target.x;
            target.x = target.x > maxX ? maxX : target.x;
            target.y = target.y < minY ? minY : target.y;
            target.y = target.y > maxY ? maxY : target.y;
            offset = transform.localPosition - Input.mousePosition;
        }
        transform.localPosition = target;
    }

    public void SetArea(Vector2 area)
    {
        minX = -area.x / 2;
        maxX = area.x / 2;
        minY = -area.y / 2;
        maxY = area.y / 2;
    }
}
