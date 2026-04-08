﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UpEffect:MonoBehaviour {
    public float Speed = 0.4f;
    public float Range = 8f;
    private float MoveDis;
    private bool IsUp = true;
    private float Modify = 1;
    private float CurrtSpeed = 1;
    private RectTransform ThisRectTran;
    private float oldX;
    private float oldY;
    public float UpSpeed = 1;
    public float DownSpeed = 1;
    private void OnEnable()
    {
        Init();
        ThisRectTran.anchoredPosition = new Vector2(oldX, oldY);
        MoveDis = oldY;
    }
    public void Init()
    {
        ThisRectTran = GetComponent<RectTransform>();
        oldY = ThisRectTran.anchoredPosition.y;
        oldX = ThisRectTran.anchoredPosition.x;
    }

    public void Update()
    {
        if (MoveDis > 0)
        {
            CurrtSpeed = Speed * (Range / ((Mathf.Abs(MoveDis) * 2) + Range));
        }
        else
        {
            CurrtSpeed = Speed * (Range / ((Mathf.Abs(MoveDis)) * 2 + Range));
        }
        if (IsUp)
        {
            CurrtSpeed *= UpSpeed;
            MoveDis += CurrtSpeed;
            ThisRectTran.anchoredPosition += new Vector2(0, CurrtSpeed);
            if (MoveDis >= Range)
            {
                IsUp = false;
            }
        }
        else
        {
            CurrtSpeed *= DownSpeed;
            MoveDis -= CurrtSpeed;
            ThisRectTran.anchoredPosition -= new Vector2(0, CurrtSpeed);
            if (MoveDis <= -Range)
            {
                IsUp = true;
            }
        }
    }
}





















































