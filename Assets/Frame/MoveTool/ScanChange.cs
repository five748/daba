﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScanChange: MonoBehaviour {
    private float Speed = 0.4f;
    private float Range = 8f;
    private float Scan = 1;
    private bool IsUp = false;
    private float Modify = 1;
    private float CurrtSpeed = 0;
    public float ChangeSpeed = 0.05f;
    float speed = 0;
    // Update is called once per frame
    void Update()
    {
       
        if (IsUp)
        {
           speed = (1.1f - Scan) * ChangeSpeed;
           Scan += speed;
            if (Scan >= 1f)
            {
                IsUp = false;
            }
        }
        else {
            speed = (Scan - 0.8f) * ChangeSpeed;
            Scan -= speed;
            if (Scan <= 0.9f)
            {
                IsUp = true;
            }
        }
        transform.localScale = Vector3.one * Scan;
    }
}





















































