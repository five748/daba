﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotMoveH : MonoBehaviour {
    private RectTransform target;
    private void Awake()
    {
        target = transform.GetComponent<RectTransform>();
    }
   
    void Update()
    {
        ChangePos();
    }
    private bool isAdd = true;
    private float speed = 1;
    private float x;
    private void ChangePos()
    {
        if (isAdd)
        {
            if (target.anchoredPosition.x <= 200)
            {
                x = target.anchoredPosition.x + speed;
                SetTargetPos(x);
            }
            else
            {
                isAdd = false;
               
            }
        }
        else
        {
            if (target.anchoredPosition.x >= -200)
            {
                x = target.anchoredPosition.x - speed;
                SetTargetPos(x);
            }
            else
            {
                isAdd = true;
            }
        }
    }

    private void SetTargetPos(float x)
    {
        target.anchoredPosition = new Vector2(x, target.anchoredPosition.y);
    }
}




















