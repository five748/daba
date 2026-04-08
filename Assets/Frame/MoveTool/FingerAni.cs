﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FingerAni:MonoBehaviour {
    public float scanSpeed = 0.01f;
    public float scanRange = 1.2f;
    public float Speed = 0.4f;
    public float Range = 8f;
    private float MoveDis;
    private bool IsUp = true;
    private float Modify = 1;
    private float CurrtSpeed = 0;
    private float CurrtScan = 0;
    private RectTransform ThisRectTran;
    private float oldY;
    private Vector3 changeScan;
    private void Awake()
    {
        ThisRectTran = GetComponent<RectTransform>();
        oldY = ThisRectTran.anchoredPosition.y;
        changeScan = Vector3.one * scanSpeed;
    }
    // Update is called once per frame
    void Update() {
        if(MoveDis > 0)
        {
            CurrtSpeed = Speed * (Range / (Mathf.Abs(MoveDis) * 2 + Range));
            CurrtScan = scanSpeed * (scanRange / (Mathf.Abs(MoveDis) * 2 + scanRange));
        }
        else {
            CurrtSpeed = Speed * (Range / (Mathf.Abs(MoveDis) * 2  + Range));
            CurrtScan = scanSpeed * (scanRange / (Mathf.Abs(MoveDis) * 2 + scanRange));
        }
        if(IsUp)
        {
            MoveDis += CurrtSpeed;
            ThisRectTran.anchoredPosition += new Vector2(-CurrtSpeed, CurrtSpeed);
            transform.localScale -= Vector3.one * CurrtScan;
            if (MoveDis >= Range)
            {
                IsUp = false;
            }
        }
        else
        {
            ThisRectTran.anchoredPosition -= new Vector2(-CurrtSpeed, CurrtSpeed);
            MoveDis -= CurrtSpeed;
            transform.localScale += Vector3.one * CurrtScan;
            if (MoveDis <= -Range)
            {
                IsUp = true;
            }
        }
    }
}





















































