﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoopAni : MonoBehaviour {
    private AnimatorStateInfo stateinfo;
    private Animator ator;

    public void Begin()
    {
        return;
        if (!ator)
        {
            ator = transform.GetComponent<Animator>();
            stateinfo = ator.GetCurrentAnimatorStateInfo(0);
        }
        ator.speed = 1;
        ator.Play("pots");
        print("1");
    }
    public void Over()
    {
        return;
        if (!ator)
        {
            ator = transform.GetComponent<Animator>();
            stateinfo = ator.GetCurrentAnimatorStateInfo(0);
        }
        ator.speed = -1;
       
        ator.Play("pots");
        print("-1");
    }
}




















