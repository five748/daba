﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Flotage:MonoBehaviour {
    public float Speed = 0.01f;
    public float Range = 0.2f;
    public bool IsUp = true;
    private float MoveDis;
    private float Modify = 1;
    private float CurrtSpeed = 0;
    private Transform ThisRectTran;
    private float oldY;
    private void Awake()
    {
        ThisRectTran = transform;
        oldY = ThisRectTran.localPosition.y;
    }
    // Update is called once per frame
    int sumChange = 10;
    int sum = 0;
    bool isChange = false;
    void Update() {
        if(isChange) {
            sum++;
            if(sum > 10) {
                isChange = false;
                sum = 0;
            }
            return;
        }
        if(MoveDis > 0)
        {
            CurrtSpeed = Speed * (Range / (Mathf.Abs(MoveDis) * 2 + Range));
        }
        else {
            CurrtSpeed = Speed * (Range / (Mathf.Abs(MoveDis) * 2  + Range));
        }
        if(IsUp)
        {
            MoveDis += CurrtSpeed;
            ThisRectTran.localPosition += new Vector3(0,0, CurrtSpeed);
            if(MoveDis >= Range)
            {
                isChange = true;
                sum = 0;
                IsUp = false;
            }
        }
        else
        {
            ThisRectTran.localPosition -= new Vector3(0,0, CurrtSpeed);
            MoveDis -= CurrtSpeed;
            if(MoveDis <= -Range)
            {
                isChange = true;
                sum = 0;
                IsUp = true;
            }
        }
    }
}





















































