using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace SelfComponent
{
    public class CameraMove : MonoBehaviour
    {
        public GameObject aimObject;
        public RectTransform rectObject;
        [FormerlySerializedAs("_camera")] public Camera targetCamera;
        [FormerlySerializedAs("canvas")] public Canvas targetCanvas;

        private float _minScaleRatio = 0f;
        public float MinScaleRatio => _minScaleRatio;
        private float _maxScaleRatio = 0f;
        private float _minX = 0f;
        private float _maxX = 0f;
        private float _minY = 0f;
        private float _maxY = 0f;

        public float dragSpeed = 2f; // 鼠标拖动速度
        private float dragSpeedWb = 300f; // 手指拖动速度
        private Vector3 _dragOrigin; // 拖动起始位置
        private Vector2 _dragOriginWb; // 拖动起始位置
        private Vector2 _frustumSize;

        private float _cameraInitZ = 0f;

        private float _scrollSpeed = 180.0f;
        private float _scrollSpeedWb = 0.5f;
        public float moveSpeed = 2f;

        private bool _isScrolling = false;
        private Vector3 _lastMousePosition;

        private bool _isZooming = false;
        private Vector2 _initialTouch1;
        private Vector2 _initialTouch2;

        private bool _isInAnimation = false;

        public void Init()
        {
            //调整相机位置
            CalculateCameraPosition();
            _cameraInitZ = targetCamera.transform.position.z;
            _frustumSize = CalculateFrustumSize();
            CalcPositionRangeLimit();
            CalcScaleSizeLimit();
        }

        private void CalcPositionRangeLimit()
        {
            Rect rectObjectRect = rectObject.rect;
            _minX = MathF.Min(-rectObjectRect.width / 2 + _frustumSize.x / 2, rectObjectRect.width / 2 - _frustumSize.x / 2);
            _minY = MathF.Min(rectObjectRect.height / 2 - _frustumSize.y / 2, -rectObjectRect.height / 2 + _frustumSize.y / 2);
            _maxX = MathF.Max(-rectObjectRect.width / 2 + _frustumSize.x / 2, rectObjectRect.width / 2 - _frustumSize.x / 2);
            _maxY = MathF.Max(rectObjectRect.height / 2 - _frustumSize.y / 2, -rectObjectRect.height / 2 + _frustumSize.y / 2);

            // Debug.Log("min_x " + min_x);
            // Debug.Log("max_x " + max_x);
            // Debug.Log("min_y " + min_y);
            // Debug.Log("max_y " + max_y);
        }

        private void CalcScaleSizeLimit()
        {
            //计算可以缩放的大小 和canvas的尺寸有关
            Rect rectObjectRect = rectObject.rect;
            RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
            var rect = canvasRect.rect;
            var scaleMinX = rect.width / rectObjectRect.width;
            var scaleMinY = rect.height / rectObjectRect.height;
            _minScaleRatio = MathF.Max(scaleMinX, scaleMinY);
            _maxScaleRatio = 1.3f;
        }

        // 计算摄像机的z轴位置接平面尺寸
        private Vector2 CalculateFrustumSize(float z = int.MinValue)
        {
            // 获取相机的视场(Field of View)
            float fieldOfView = targetCamera.fieldOfView;

            // 获取相机的远近裁剪平面
            float nearPlane = targetCamera.nearClipPlane;
            float farPlane = targetCamera.farClipPlane;

            // 获取当前相机的位置
            Vector3 cameraPosition = targetCamera.transform.position;

            //判定是否传参
            if (z == int.MinValue)
            {
                z = cameraPosition.z;
            }

            // 计算相机位置处的截平面高度
            float frustumHeight = 2.0f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad) * MathF.Abs(z);

            // 计算相机位置处的截平面宽度
            float frustumWidth = frustumHeight * targetCamera.aspect;

            // Debug.Log("Frustum Height: " + frustumHeight);
            // Debug.Log("Frustum Width: " + frustumWidth);
            return new Vector2(frustumWidth, frustumHeight);
        }

        //计算摄像机应该在的位置 用于根据屏幕初始化摄像机
        void CalculateCameraPosition()
        {
            // 获取 Canvas 的尺寸
            RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
            var rect = canvasRect.rect;
            float canvasWidth = rect.width;
            float canvasHeight = rect.height;

            // 获取相机的视场(Field of View)
            float fieldOfView = targetCamera.fieldOfView;

            // 计算 Canvas 在相机视野中的高度所需的距离
            // 这里使用相机视场的高度（tan(fieldOfView/2)），也可以使用宽度（tan(fieldOfView/2) * aspect）
            float canvasDistance = canvasHeight / (2.0f * Mathf.Tan(fieldOfView * 0.5f * Mathf.Deg2Rad));

            // 设置相机在 z 轴上的位置
            targetCamera.transform.position =
                new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, -canvasDistance);
        }

        void Update()
        {
            if (_isInAnimation)
            {
                return;
            }
            
            MoveCamera();

#if UNITY_EDITOR
            ScaleCamera();
#endif

            ScaleCameraFinger();
        }

        void MoveCamera()
        {
            // Debug.Log("MoveCamera 版本----1");

            if (_isZooming)
            {
                return;
            }
            
#if !UNITY_EDITOR
            if (Input.touchCount == 1)
            {
                // 获取第一个触摸点
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Debug.Log($"开始触摸屏幕");
                    _dragOriginWb = touch.position;
                    return;
                }

                if (touch.phase != TouchPhase.Moved)
                {
                    Debug.Log($"不是移动");
                    return;
                }

                if (MathF.Abs(touch.position.x - _dragOriginWb.x) < 1 || MathF.Abs(touch.position.y - _dragOriginWb.y) < 1)
                {
                    Debug.Log($"移动距离小于1 不处理");
                    return;
                }
                Debug.Log($"触摸屏幕位置 {touch.position.x} {touch.position.y}");

                //一个比较线性的值
                Vector2 pos = Camera.main.ScreenToViewportPoint(_dragOriginWb - touch.position);
                var position = targetCamera.transform.position;
                var distance = getDistance();
                var factory = (position.z + _cameraInitZ / _minScaleRatio) / (distance / 2);
                Vector3 moveDirection = position + new Vector3(pos.x * factory * dragSpeedWb, pos.y * factory * dragSpeedWb, 0);
                #if !(DY || WX)
                    moveDirection.x = -moveDirection.x;
                    moveDirection.y = -moveDirection.y;
                #endif
                
                position = new Vector3(
                    Math.Clamp(moveDirection.x, _minX, _maxX),
                    Math.Clamp(moveDirection.y, _minY, _maxY),
                    position.z
                );
                targetCamera.transform.position = position;
                _dragOriginWb = touch.position;
            }
#endif

#if UNITY_EDITOR
            // 鼠标按下
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = Input.mousePosition;
                return;
            }

            // 鼠标拖动
            if (!Input.GetMouseButton(0))
            {
                return;
            }

            //一个比较线性的值
            Vector3 pos = Camera.main.ScreenToViewportPoint(_dragOrigin - Input.mousePosition).normalized;
            var position = targetCamera.transform.position;
            Vector3 move = position + new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);
            position = new Vector3(
                Math.Clamp(move.x, _minX, _maxX),
                Math.Clamp(move.y, _minY, _maxY),
                position.z
            );
            targetCamera.transform.position = position;
            _dragOrigin = Input.mousePosition;
#endif
        }

        void ScaleCamera()
        {
            // 检测键盘输入
            if (Input.GetKeyDown(KeyCode.C))
            {
                _isScrolling = true;
                _lastMousePosition = Input.mousePosition;
            }
            else if (Input.GetKeyUp(KeyCode.C))
            {
                _isScrolling = false;
            }

            // 检测鼠标滚轮事件
            if (_isScrolling)
            {
                float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
                if (scrollAmount == 0)
                {
                    return;
                }

                // 缩放中心点
                Vector3 zoomCenter = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                var cameraPos = targetCamera.transform.position;
                // 调整摄像机的 z 轴位置
                Vector3 move = new Vector3(0, 0, Math.Clamp(
                        scrollAmount * _scrollSpeed + cameraPos.z,
                        _cameraInitZ / _minScaleRatio,
                        _cameraInitZ / _maxScaleRatio
                    )
                );
                _frustumSize = CalculateFrustumSize(move.z);
                CalcPositionRangeLimit();
                // 计算缩放后相机位置与缩放中心的偏移
                Vector3 offset = zoomCenter - cameraPos;
                move.x += cameraPos.x + offset.x;
                move.y += cameraPos.y + offset.y;
                targetCamera.transform.position = new Vector3(
                    Math.Clamp(cameraPos.x, _minX, _maxX),
                    Math.Clamp(cameraPos.y, _minY, _maxY),
                    move.z
                );
                // Debug.Log($"摄像机的缩放极限为  {camera_init_z / min_scale_ratio} {camera_init_z / max_scale_ratio}");
            }
        }

        void ScaleCameraFinger()
        {
            // 检测触摸屏输入
            if (Input.touchCount == 2)
            {
                if (!_isZooming)
                {
                    _isZooming = true;
                    _initialTouch1 = Input.GetTouch(0).position;
                    _initialTouch2 = Input.GetTouch(1).position;
                }
                else
                {
                    Vector2 currentTouch1 = Input.GetTouch(0).position;
                    Vector2 currentTouch2 = Input.GetTouch(1).position;
                    float initialDistance = Vector2.Distance(_initialTouch1, _initialTouch2);
                    float currentDistance = Vector2.Distance(currentTouch1, currentTouch2);
                    float deltaDistance = currentDistance - initialDistance;
                    float zoomFactor = deltaDistance * 1f;//缩放速度调节

                    // 计算两指中点作为缩放中心
                    Vector3 zoomCenter = Camera.main.ScreenToWorldPoint((currentTouch1 + currentTouch2) / 2f);

                    var cameraPos = targetCamera.transform.position;
                    // 调整摄像机的 z 轴位置
                    Vector3 move = new Vector3(0, 0, Math.Clamp(
                            zoomFactor * _scrollSpeedWb + cameraPos.z,
                            _cameraInitZ / _minScaleRatio,
                            _cameraInitZ / _maxScaleRatio
                        )
                    );
                    _frustumSize = CalculateFrustumSize(move.z);
                    CalcPositionRangeLimit();
                    // 计算缩放后相机位置与缩放中心的偏移
                    Vector3 offset = zoomCenter - cameraPos;
                    move.x += cameraPos.x + offset.x;
                    move.y += cameraPos.y + offset.y;
                    targetCamera.transform.position = new Vector3(
                        Math.Clamp(cameraPos.x, _minX, _maxX),
                        Math.Clamp(cameraPos.y, _minY, _maxY),
                        move.z
                    );
                }
            }
            else
            {
                _isZooming = false;
            }
        }

        /// <summary>
        /// 缩放一定的倍率 受控件内部_scrollSpeed影响
        /// </summary>
        /// <param name="ratio"></param>
        public void Scale(float ratio)
        {
            var cameraPos = targetCamera.transform.position;
            // 调整摄像机的 z 轴位置
            Vector3 move = new Vector3(0, 0, Math.Clamp(
                    ratio * _scrollSpeed + cameraPos.z,
                    _cameraInitZ / _minScaleRatio,
                    _cameraInitZ / _maxScaleRatio
                )
            );
            _frustumSize = CalculateFrustumSize(move.z);
            CalcPositionRangeLimit();
            // 计算缩放后相机位置与缩放中心的偏移
            targetCamera.transform.position = new Vector3(
                Math.Clamp(cameraPos.x, _minX, _maxX),
                Math.Clamp(cameraPos.y, _minY, _maxY),
                move.z
            );
        }
        
        /// <summary>
        /// 缩放到指定的大小
        /// </summary>
        /// <param name="ratio">缩放倍率</param>
        public void ScaleAppointRatio(float ratio)
        {
            float minZ = _cameraInitZ / _minScaleRatio;
            float maxZ = _cameraInitZ / _maxScaleRatio;

            var distance = minZ - maxZ;
            var pos = distance * ratio;
            
            var cameraPos = targetCamera.transform.position;
            // 调整摄像机的 z 轴位置
            Vector3 move = new Vector3(0, 0, Math.Clamp(
                    pos,
                    _cameraInitZ / _minScaleRatio,
                    _cameraInitZ / _maxScaleRatio
                )
            );
            _frustumSize = CalculateFrustumSize(move.z);
            CalcPositionRangeLimit();
            // 计算缩放后相机位置与缩放中心的偏移
            targetCamera.transform.position = new Vector3(
                Math.Clamp(cameraPos.x, _minX, _maxX),
                Math.Clamp(cameraPos.y, _minY, _maxY),
                move.z
            );
        }

        public Vector3 MoveScaleCalculate(Transform tfTarget, float ratio)
        {
            float minZ = _cameraInitZ / _minScaleRatio;
            float maxZ = _cameraInitZ / _maxScaleRatio;
            
            var distance = minZ - maxZ;
            var pos = distance * ratio;
            
            // 调整摄像机的 z 轴位置
            Vector3 move = new Vector3(tfTarget.position.x, tfTarget.position.y, Math.Clamp(
                    pos,
                    _cameraInitZ / _minScaleRatio,
                    _cameraInitZ / _maxScaleRatio
                )
            );
            _frustumSize = CalculateFrustumSize(move.z);
            CalcPositionRangeLimit();
            
            //直接移到对应的世界坐标上
            move.x = Math.Clamp(move.x, _minX, _maxX);
            move.y = Math.Clamp(move.y, _minY, _maxY);

            return move;
        }
        
        public void MoveToTarget(Transform tfTarget)
        {
            // 调整摄像机的 z 轴位置
            Vector3 move = new Vector3(tfTarget.position.x, tfTarget.position.y, targetCamera.transform.position.z);
            _frustumSize = CalculateFrustumSize(move.z);
            CalcPositionRangeLimit();
            
            //直接移到对应的世界坐标上
            move.x = Math.Clamp(move.x, _minX, _maxX);
            move.y = Math.Clamp(move.y, _minY, _maxY);

            targetCamera.transform.position = move;
        }

        public void SurroundDam(Action call, Transform tfShouFeiZhan)
        {
            EnterAnimation();
            var initPos = new Vector3(
                Math.Clamp(tfShouFeiZhan.position.x, _minX, _maxX), 
                Math.Clamp(tfShouFeiZhan.position.y, _minY, _maxY), 
                targetCamera.transform.position.z
            );
            targetCamera.transform.position = initPos;
            //新手引导视角向右下移动 3f为移动时间
            targetCamera.transform.DOMove(new Vector3(_maxX * 0.3f, _minY * 0.3f, targetCamera.transform.position.z), 3f).OnComplete(() =>
            {
                //新手引导视角向左上移动 3f为移动时间
                targetCamera.transform.DOMove(initPos, 3f).OnComplete(() =>
                {
                    OutAnimation();
                    call?.Invoke();
                }).SetEase(Ease.Linear);    
            }).SetEase(Ease.Linear);
        }

        public void EnterAnimation()
        {
            _isInAnimation = true;
        }

        public void OutAnimation()
        {
            _isInAnimation = false;
        }

        private float getDistance()
        {
            float minZ = _cameraInitZ / _minScaleRatio;
            float maxZ = _cameraInitZ / _maxScaleRatio;

            var distance = minZ - maxZ;

            return distance;
        }
    }
}