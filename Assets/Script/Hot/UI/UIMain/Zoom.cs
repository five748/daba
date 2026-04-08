using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Zoom : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public RectTransform ZoomTarget;
    public ScrollRect ScrollRect;
    public float ZoomSensitivity = 0.001f;
    //最小缩放 最大缩放 缩放的改变 不同分辨率适配
    private float _zoomMin, _zoomMax, _scaleChange, _sceenWidthProportion;
    //触摸计算后的中间点
    private Vector2 _midPoint;
    private Vector2 _centerPivot = new Vector2(0.5f, 0.5f);
    private bool _isZoom, _resete = false;
    public Button Next;
    [SerializeField]
    List<Texture> _textures = new List<Texture>();
    //设置vtexture在view内的最大最小缩放

    private Canvas _canvas;
    void setZoom()
    {
        _canvas = TranTool.GetRootCanvas(gameObject.GetComponent<RectTransform>());
        RectTransform canvasRectTransform = _canvas.GetComponent<RectTransform>();
        RectTransform rect_content = ZoomTarget.GetComponent<RectTransform>();
        var rect_canvas = canvasRectTransform.rect;
        var minValue1 = 1 / rect_content.sizeDelta.x * rect_canvas.width;//(ScrollRect.transform as RectTransform).rect.width / ZoomTarget.rect.width;
        var minValue2 = 1 / rect_content.sizeDelta.y * rect_canvas.height;//(ScrollRect.transform as RectTransform).rect.height / ZoomTarget.rect.height;
        _zoomMin = minValue1 < minValue2 ? minValue2 : minValue1;
        _zoomMax = 1;//_zoomMin + 1;
        //default min view
        ZoomTarget.localScale = new Vector3(1, 1);
    }
    void Start()
    {
        setZoom();
        //切换texture logic
        // var rawimg = ZoomTarget.GetComponent<RawImage>();
        // Assert.IsTrue(_textures.Count > 0, "texture list is null");
        // rawimg.texture = _textures[0];
        // rawimg.SetNativeSize();
        // setZoom();
        // Next.onClick.AddListener(() =>
        // {
        //     var nextid = 0;
        //     if (_textures.Count > 1)
        //     {
        //         for (int i = 0; i < _textures.Count; i++)
        //         {
        //             if (_textures[i].Equals(rawimg.texture))
        //             {
        //                 nextid = (i + 1) == _textures.Count ? 0 : i + 1;
        //                 break;
        //             }
        //         }
        //     }
        //     rawimg.texture = _textures[nextid];
        //     rawimg.SetNativeSize();
        //     setZoom();
        // });
    }
    void Update()
    {
        _sceenWidthProportion = Screen.width / _canvas.GetComponent<RectTransform>().rect.width; //Screen.width / 1920f;
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.LeftControl) && Input.mouseScrollDelta.y != 0)
        {
            _midPoint = Input.mousePosition;
            bool isPosInViewportRect = RectTransformUtility.RectangleContainsScreenPoint(ScrollRect.viewport, new Vector2(_midPoint.x, _midPoint.y));
            if (isPosInViewportRect)
            {
                ScrollRect.horizontal = false;
                ScrollRect.vertical = false;
                ComputePivot(_midPoint);
                SetMouseScrollWheelScale();
                _resete = false;
            }
        }
        else if (!_resete)
        {
            ScrollRect.horizontal = true;
            ScrollRect.vertical = true;
            _resete = true;
            SetPivot(_centerPivot);
        }
        //中心点缩放
        if (Input.GetKey(KeyCode.C) && Input.mouseScrollDelta.y != 0)
        {
            //前提ScrollRect.viewport pivot = 0.5, 0.5
            ComputePivot(ScrollRect.viewport.position);
            SetMouseScrollWheelScale();
        }
        //设置缩放
        void SetMouseScrollWheelScale()
        {
            _scaleChange = Input.mouseScrollDelta.y * ZoomSensitivity;
            if (_scaleChange != 0)
            {
                var scaleX = ZoomTarget.localScale.x;
                scaleX += _scaleChange;
                scaleX = Mathf.Clamp(scaleX, _zoomMin, _zoomMax);
                ZoomTarget.localScale = new Vector3(scaleX, scaleX);
            }
        }
#endif
    }
    //计算pivot pos && Set
    public void ComputePivot(Vector3 inputPos)
    {
        Vector2 currentPivot = ZoomTarget.pivot;
        Vector3 vector3 = ZoomTarget.InverseTransformPoint(inputPos); //input pos to ui local pos  相对于pivot
        // new pivot = (input local pos + 当前pivot 所在的 local pos)   / ui总大小
        float x = ((vector3.x) + (currentPivot.x * ZoomTarget.rect.width)) / ZoomTarget.rect.width;
        float y = ((vector3.y) + (currentPivot.y * ZoomTarget.rect.height)) / ZoomTarget.rect.height;
        SetPivot(new Vector2(x, y));
    }
    private Vector3[] _zoomTargetFourCornersArray = new Vector3[4];
    private Vector3[] _viewportFourCornersArray = new Vector3[4];
    //根据输入的坐标设置map中心点位置
    public void SetPivot(Vector2 targetPivot)
    {
        var currentPivot = ZoomTarget.pivot;
        ZoomTarget.pivot = targetPivot;
        //计算位置偏移
        var offset_pivot = targetPivot - currentPivot;
        var offset_pos = new Vector3((offset_pivot.x * ZoomTarget.rect.width * ZoomTarget.localScale.x),
                                     (offset_pivot.y * ZoomTarget.rect.height * ZoomTarget.localScale.y), 0);
        ZoomTarget.position = ZoomTarget.position + offset_pos * _sceenWidthProportion;
        FixZoomTargetPosition();
        void FixZoomTargetPosition()
        {
            ZoomTarget.GetWorldCorners(_zoomTargetFourCornersArray);
            ScrollRect.viewport.GetWorldCorners(_viewportFourCornersArray);
            // 左下角
            Vector3 offset = _viewportFourCornersArray[0] - _zoomTargetFourCornersArray[0];
            ZoomTarget.position = ZoomTarget.position + new Vector3(offset.x > 0 ? 0 : offset.x,
                                                offset.y > 0 ? 0 : offset.y,
                                                0) * _sceenWidthProportion;
            // 右上角
            offset = _viewportFourCornersArray[2] - _zoomTargetFourCornersArray[2];
            ZoomTarget.position = ZoomTarget.position + new Vector3(offset.x < 0 ? 0 : offset.x,
                                                offset.y < 0 ? 0 : offset.y,
                                                0) * _sceenWidthProportion;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);
            //touch点只能在viewport内有效
            var isZeroViewportRect = RectTransformUtility.RectangleContainsScreenPoint(ScrollRect.viewport, touchZero.position, Camera.main);
            var isOneViewportRect = RectTransformUtility.RectangleContainsScreenPoint(ScrollRect.viewport, touchOne.position, Camera.main);
            if (!isZeroViewportRect || !isOneViewportRect)
            {
                Debug.Log($"两点触摸 但是被返回了");
                // Msg.Instance.Show("两点触摸 但是被返回了");
                return;
            }
            ScrollRect.horizontal = false;
            ScrollRect.vertical = false;
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = touchDeltaMag - prevTouchDeltaMag;
            _scaleChange = deltaMagnitudeDiff * ZoomSensitivity;

            _midPoint = (touchOne.position + touchZero.position) / 2;
            ComputePivot(_midPoint);
            if (_scaleChange != 0)
            {
                var scaleX = ZoomTarget.localScale.x;
                scaleX += _scaleChange;
                scaleX = Mathf.Clamp(scaleX, _zoomMin, _zoomMax);
                ZoomTarget.localScale = new Vector3(scaleX, scaleX);
            }
            _resete = false;
        }

        if (Input.touchCount <= 1)
        {
            ScrollRect.horizontal = true;
            ScrollRect.vertical = true;
            ScrollRect.OnDrag(eventData);
        }
    }
    int endCount = 0;
    public void OnEndDrag(PointerEventData eventData)
    {
        if (Input.touchCount <= 1)
            _isZoom = false;
        endCount++;
        if (Input.touchCount == 2 && endCount == 2)
        {
            _isZoom = false;
        }
        ScrollRect.OnEndDrag(eventData);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        endCount = 0;
        if (Input.touchCount <= 1) ScrollRect.OnBeginDrag(eventData);
        else _isZoom = true;
    }
}