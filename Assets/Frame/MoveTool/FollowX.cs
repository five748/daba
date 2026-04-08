﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FollowX : MonoBehaviour {
	public float FollowScale = 1;
    public Transform FollowTarget;
	
	// Update is called once per frame
	void Update () {
		if (FollowTarget == null) {
			return;
		}
		transform.position =  new Vector3(FollowTarget.position.x * FollowScale, transform.position.y, transform.position.z);
	}
}





















































