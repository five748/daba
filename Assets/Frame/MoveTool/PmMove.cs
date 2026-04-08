using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PmMove : MonoBehaviour
{

    [Header("开始位置的Transform")]
    public Transform startTransform;
    [Header("结束位置的Transform")]
    public Transform endTransform;

    [Header("抛物线的高度")]
    public float parabolaHeight = 5f;
    [Header("移动速度")]
    public float speed = 5f;

    private Vector3 startPoint;
    private Vector3 endPoint;


    private float distanceToTarget;
    private float startTime;

    private bool isBool = true;
    public Transform MoveTo;
    public bool isUp;
    public System.Action callback;
    public System.Func<Transform> Create;
    public Vector2 ImageSize = Vector2.zero;


    private void Start()
    {
        if (Create != null)
        {
            startTransform = Create();
        }
        if (ImageSize.x != 0)
        {
            startTransform.GetComponent<RectTransform>().sizeDelta = ImageSize;
        }
        startTransform.SetDepth(100);
        startPoint = startTransform.position;
        endPoint = endTransform.position;


        // 设置物体的初始位置
        startTransform.position = startPoint;

        // 计算起始点到落点的距离
        distanceToTarget = Vector3.Distance(startPoint, endPoint);

        // 计算移动的起始时间
        startTime = Time.time;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("重置");
            transform.position = startPoint;
            startTime = Time.time;
            isBool = true;
        }
    }

    private void FixedUpdate()
    {
        if (isBool)
        {
            // 计算已经经过的时间
            float elapsedTime = Time.time - startTime;

            // 计算当前位置
            float journeyFraction = elapsedTime * speed / distanceToTarget;
            Vector3 currentPos = Vector3.Lerp(startPoint, endPoint, journeyFraction);

            // 计算抛物线高度
            if (isUp)
            {
                currentPos.x -= Mathf.Sin(journeyFraction * Mathf.PI) * parabolaHeight;
            }
            else
            {
                currentPos.y += Mathf.Sin(journeyFraction * Mathf.PI) * parabolaHeight;
            }


            // 移动物体
            startTransform.position = currentPos;

            // 如果已经到达目标点，结束移动
            if (isOver())
            {
                // 确保物体在落点位置
                startTransform.position = endPoint;
                isBool = false;
                GameObject.Destroy(startTransform.gameObject);
                if (callback != null)
                {
                    callback();
                }
            }
        }
    }
    private bool isOver()
    {
        if (isUp)
        {
            return startTransform.position.y >= endPoint.y;
        }
        else
        {
            return startTransform.position.y <= endPoint.y;
        }
    }
}
