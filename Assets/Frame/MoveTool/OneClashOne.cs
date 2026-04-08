﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OneClashOne: MonoBehaviour {
    private Transform One;
    private Transform Two;
    public AnimationCurve animationCurve;  // 动画曲线
                                           // Use this for initialization
    void Start () {
        One = transform.GetChild(0);
        Two = transform.GetChild(1);
        BeginClash();
	}
    public void BeginClash() {
        StartCoroutine("MoveToOther");
    }
    private IEnumerator MoveToOther() {
        int len = animationCurve.length;
        int sum = 0;
        while(true) {
            One.transform.localPosition += new Vector3(animationCurve.keys[sum].value, 0) * 175;
            Two.transform.localPosition -= new Vector3(animationCurve.keys[sum].value, 0) * 175;
            sum++;
            if(sum >= len) {
                yield break;
            }
           yield return new WaitForSeconds(0.2f);
        }
    }
	
}





















































