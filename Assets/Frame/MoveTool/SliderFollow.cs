﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SliderFollow : MonoBehaviour {
    public UnityEngine.UI.Image image;
    public float width;
    public RectTransform mRT;
    private void Awake()
    {
        image = transform.parent.GetComponent<UnityEngine.UI.Image>();
        mRT = transform.GetComponent<RectTransform>();
    }
    

    // Update is called once per frame
    void Update () {
        if (width == 0) {
            width = image.transform.GetComponent<RectTransform>().sizeDelta.x;
        }
        mRT.anchoredPosition = new Vector2(width * image.fillAmount, 0);

    }
}




















