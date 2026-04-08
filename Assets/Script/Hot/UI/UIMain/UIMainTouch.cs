using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMainTouch :MonoBehaviour//, IPointerDownHandler, IPointerUpHandler
{
    public ScrollRect scrollView;
    
    private Canvas _canvas;
    
    private float initialDistance;
    
    public RectTransform content;
    public float zoomSpeed = 0.5f;
    public float minZoomScale = 0.5f;
    public float maxZoomScale = 1.0f;

    private Vector2[] lastTouchPositions = new Vector2[2];
    private Vector2[] currentTouchPositions = new Vector2[2];
    private float lastTouchDistance;

    public void Init()
    {
        _canvas = TranTool.GetRootCanvas(gameObject.GetComponent<RectTransform>());
        RectTransform canvasRectTransform = _canvas.GetComponent<RectTransform>();
        RectTransform rect_content = scrollView.content.GetComponent<RectTransform>();
        var scale_min_x = 1 / rect_content.sizeDelta.x * canvasRectTransform.rect.width;
        var scale_min_y = 1 / rect_content.sizeDelta.y * canvasRectTransform.rect.height;
        minZoomScale = Mathf.Max(scale_min_x, scale_min_y);
    }
    
    void Update()
    {
        if (Input.touchCount == 2)
        {
            currentTouchPositions[0] = Input.GetTouch(0).position;
            currentTouchPositions[1] = Input.GetTouch(1).position;
        
            if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began)
            {
                lastTouchPositions[0] = currentTouchPositions[0];
                lastTouchPositions[1] = currentTouchPositions[1];
                lastTouchDistance = Vector2.Distance(currentTouchPositions[0], currentTouchPositions[1]);
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                float currentTouchDistance = Vector2.Distance(currentTouchPositions[0], currentTouchPositions[1]);
                float deltaDistance = currentTouchDistance - lastTouchDistance;
        
                // Vector2 zoomCenter = (currentTouchPositions[0] + currentTouchPositions[1]) / 2f;
        
                // 根据缩放距离调整缩放比例
                float scaleFactor = 1f + deltaDistance * zoomSpeed * Time.deltaTime;
                Vector2 newScale = content.localScale * scaleFactor;
        
                // 限制缩放比例在指定范围内
                newScale = new Vector2(Mathf.Clamp(newScale.x, minZoomScale, maxZoomScale),
                    Mathf.Clamp(newScale.y, minZoomScale, maxZoomScale));
        
                content.localScale = newScale;
        
                // 根据缩放中心调整位置
                // Vector2 pivotOffset = zoomCenter - new Vector2(content.position.x, content.position.y);
                // Vector2 scaledPivotOffset = pivotOffset * scaleFactor;
                // content.anchoredPosition -= scaledPivotOffset - pivotOffset;

                content.anchoredPosition = new Vector2(-content.sizeDelta.x * newScale.x / 2f, content.sizeDelta.y * newScale.y / 2); 
                lastTouchDistance = currentTouchDistance;
            }
        }
    }
}
