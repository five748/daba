﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DonotDestroy : MonoBehaviour {
	void Awake () {
		GameProcess.Instance.DonotDestoryGos.Add(gameObject);
		GameObject.DontDestroyOnLoad(this);
	}
}