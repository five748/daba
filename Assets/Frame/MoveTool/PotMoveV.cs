﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PotMoveV : MonoBehaviour {
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
    public float speed = 1;
    private float y;
    private void ChangePos()
    {
        if (isAdd)
        {
            if (target.anchoredPosition.y <= 256)
            {
                y = target.anchoredPosition.y + speed;
                SetTargetPos(y);
            }
            else
            {
                isAdd = false;
               
            }
        }
        else
        {
            if (target.anchoredPosition.y >= -16)
            {
                y = target.anchoredPosition.y - speed;
                SetTargetPos(y);
            }
            else
            {
                isAdd = true;
            }
        }
    }

    private void SetTargetPos(float y)
    {
        target.anchoredPosition = new Vector2(target.anchoredPosition.x, y);
    }
}




















