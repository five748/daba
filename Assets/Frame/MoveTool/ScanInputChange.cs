using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScanInputChange : MonoBehaviour
{

    private bool isTwoTouch;
    private Vector2 firstTouch;
    private Vector2 secondTouch;
    private float lastDistance;
    private float twoTouchDistance;
    public ScrollRect scroll;
    private Vector2 oldpos;
    private float oldScan;
    private RectTransform mRT;
    private float smallScan = 0.5f;
    private float bigScsn = 1f;
    private float oneAdd = 0.8722424f;
    private float oneAddY = 0.0425f;
    public float changeX;
    public float changeY;
    private float h;
    private float w;
    private float oldx = 0;
    private float oldy = -92;
    private float px;
    private float py;
    Vector3[] bgconners = new Vector3[4];//ScrollRect四角的世界坐标
    private void ChangePivot(Vector2 pos)
    {

        px = 0.5f - (pos.x - transform.position.x) * w;
        py = 0.5f - (pos.y - transform.position.y) * h;
        mRT.pivot = new Vector2(px, py);
    }

    void LateUpdate()
    {


        if (!mRT)
        {

            mRT = transform.GetComponent<RectTransform>();
            scroll = transform.parent.GetComponent<ScrollRect>();
            //Vector3[] bgconners = new Vector3[4];//ScrollRect四角的世界坐标
            //mRT.GetLocalCorners(bgconners);
            changeX = (Screen.height - 1334f) * oneAdd + 787.345f;
            changeY = (Screen.height - 1334f) * oneAddY + 92f;
            UIManager.Root.GetComponent<RectTransform>().GetWorldCorners(bgconners);
            w = 0.5f / bgconners[3].x - transform.position.x;
            h = 0.5f / bgconners[2].y - transform.position.y;

        }
        twoTouchDistance = Vector2.Distance(firstTouch, secondTouch);
        //if (Input.touchCount > 0)
        //{
        //    Ray ray = new Ray();
        //    ray = UIManager.mainCamera.ScreenPointToRay(Input.GetTouch(0).position);

        //    if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
        //    {
        //        Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
        //        transform.Translate(touchDeltaPosition.x * Time.deltaTime * 0.2f, touchDeltaPosition.y * Time.deltaTime * 0.2f, 0);
        //    }
        //}

        //if (Input.touchCount == 1) {
        //    if (transform.localScale.x == 1.5f) {

        //    }
        //}


        if (Input.touchCount > 1)
        {
            //当第二根手指按下的时候
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                scroll.enabled = false;
                isTwoTouch = true;
                //获取第一根手指的位置
                firstTouch = Input.touches[0].position;
                //获取第二根手指的位置
                secondTouch = Input.touches[1].position;

                lastDistance = Vector2.Distance(firstTouch, secondTouch);
                oldpos = mRT.anchoredPosition / transform.localScale.x;
                oldScan = transform.localScale.x;

            }

            //如果有两根手指按下
            if (isTwoTouch)
            {
                //每一帧都得到两个手指的坐标以及距离
                firstTouch = Input.touches[0].position;
                secondTouch = Input.touches[1].position;

                ChangePivot(new Vector2((firstTouch.x + secondTouch.x) * 0.5f, (firstTouch.y + secondTouch.y) * 0.5f));
                twoTouchDistance = Vector2.Distance(firstTouch, secondTouch);

                //当前图片的缩放
                Vector3 curImageScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
                //两根手指上一帧和这帧之间的距离差
                //因为100个像素代表单位1，把距离差除以100看缩放几倍
                float changeScaleDistance = (twoTouchDistance - lastDistance) / 100;
                //因为缩放 Scale 是一个Vector3，所以这个代表缩放的Vector3的值就是缩放的倍数
                Vector3 changeScale = new Vector3(changeScaleDistance, changeScaleDistance, 0);
                //图片的缩放等于当前的缩放加上 修改的缩放
                transform.localScale = curImageScale + changeScale;
                //控制缩放级别
                transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, smallScan, bigScsn), Mathf.Clamp(transform.localScale.y, smallScan, bigScsn), 1);

                //mRT.anchoredPosition = oldpos * transform.localScale.x;

                //mRT.anchoredPosition = new Vector2(Mathf.Clamp(mRT.anchoredPosition.x, -changeX *transform.localScale.x, changeX * transform.localScale.x), Mathf.Clamp(mRT.anchoredPosition.y, -changeY * transform.localScale.x, changeY * transform.localScale.x));
                //这一帧结束后，当前的距离就会变成上一帧的距离了
                lastDistance = twoTouchDistance;

            }
        }
        if (isTwoTouch)
        {
            if (Input.touchCount == 0)
            {
                isTwoTouch = false;
                scroll.enabled = true;
            }
        }


#if UNITY_EDITOR
        ChangePivot(Input.mousePosition);
        transform.localScale -= Input.GetAxis("Mouse ScrollWheel") * Vector3.one;
        transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x, 1f, 1.5f), Mathf.Clamp(transform.localScale.y, 1f, 1.5f), 1);

#endif
    }
}

























