﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Follow : MonoBehaviour {
    public float FollowSpeed = 1.0f;
    public Transform FollowTarget;
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
        transform.position = FollowTarget.position;
	}
}





















































