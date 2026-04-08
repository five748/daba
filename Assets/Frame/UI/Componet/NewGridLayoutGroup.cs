using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class NewGridLayoutGroup:GridLayoutGroup
{
    protected override void Start() {
        base.Start();
        if(key == -1)
            return;
        if(!Application.isPlaying)
            return;
    }
    public int key = -1;
    public bool isInArray = false;
    public int keyIndex = -1;
    public string style = "样式";

    protected override void OnDestroy() {
        base.OnDestroy();
        if(key == -1)
            return;
    }
}
