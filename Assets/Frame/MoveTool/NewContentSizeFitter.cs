﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewContentSizeFitter : ContentSizeFitter
{
    public System.Action<bool> ChangeEvent;
    public override void SetLayoutHorizontal()
    {
        base.SetLayoutHorizontal();
        if(ChangeEvent != null){
            ChangeEvent(true);
        }
    }
    public override void SetLayoutVertical()
    {
        base.SetLayoutVertical();
        if (ChangeEvent != null)
        {
            ChangeEvent(false);
        }
    }
}





















































