﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotMoveO : MonoBehaviour {

    public Transform sun;

    public float r; //半径
    public float w; //角度
    public float speed;

    public float x;
    public float y;
    private RectTransform mrt;
    void Awake()
    {
        //transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(10 * Random.value, 10 * Random.value, 0); //重置做圆周的开始位置
        //r = Vector3.Distance(transform.GetComponent<RectTransform>().anchoredPosition, sun.transform.GetComponent<RectTransform>().anchoredPosition); //两个物品间的距离
        w = 0f; // ---角速度
        //speed = 1 * Random.value; // 这个应该所角速度了
        mrt = transform.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {

        //下面的概念有点模糊了
        w += speed * Time.deltaTime; // 

        x = Mathf.Cos(w) * r;
        y = Mathf.Sin(w) * r;

        mrt.anchoredPosition = new Vector3(x, y);


    }



    //private RectTransform target;
    //private void Awake()
    //{
    //    target = transform.GetComponent<RectTransform>();
    //}
    //// Update is called once per frame
    //void Update()
    //{
    //    transform.Rotate(Vector3.zero, 1);
    //    //ChangePos();
    //}
    //private bool isAdd = true;
    //private float speed = 1;
    //private float y;
    //private void ChangePos()
    //{
    //    if (isAdd)
    //    {
    //        if (target.anchoredPosition.y <= 80)
    //        {
    //            y = target.anchoredPosition.y + speed;
    //            SetTargetPos(y);
    //        }
    //        else
    //        {
    //            isAdd = false;
    //            y = target.anchoredPosition.y - speed;
    //            SetTargetPos(y);
    //        }
    //    }
    //    else
    //    {
    //        if (target.anchoredPosition.y >= -80)
    //        {
    //            y = target.anchoredPosition.y - speed;
    //            SetTargetPos(y);
    //        }
    //        else
    //        {
    //            isAdd = true;
    //            y = target.anchoredPosition.y + speed;
    //            SetTargetPos(y);
    //        }
    //    }
    //}

    //private void SetTargetPos(float y)
    //{
    //    float x = Mathf.Sqrt(6400 - y * y);
    //    if (isAdd)
    //    {
    //        x = -x;
    //    }
    //    target.anchoredPosition = new Vector2(x, y);
    //}
}




















