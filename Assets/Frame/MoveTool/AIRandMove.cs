﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using UnityEngine;
using System.Collections;

public class AIRandMove : MonoBehaviour
{
    public float vel_x, vel_y, vel_z;//速度
    /// <summary>
    /// 最大、最小飞行界限
    /// </summary>
    public float maxPos_x = 500;
    public float maxPos_y = 300;
    public float minPos_x = -500;
    public float minPos_y = -300;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(vel_x, vel_y, 0, Space.Self);
        Check();
    }
    //判断是否达到边界，达到边界则将速度改为负的
    void Check()
    {
       
        if (transform.localPosition.x < minPos_x)
        {
            transform.localPosition = new Vector3(maxPos_x, transform.localPosition.y, 0);
        }
    }
}














































