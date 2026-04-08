using GameVersionSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABData : SingleMono2<ABData> {
	public void PreLoad(System.Action callback) {
        callback();
    }
}
