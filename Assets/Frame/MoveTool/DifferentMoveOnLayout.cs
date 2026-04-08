﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DifferentMoveOnLayout: DragScroll {
    private Transform[] Bgs;
    public float[] Speeds;
    public Transform Grid;
    // Use this for initialization
    void Awake() {
        Bgs = new Transform[Grid.childCount - 1];
        //print(Bgs.Length);
        for(int i = 0; i < Bgs.Length; i++)
        {
            Bgs[i] = Grid.GetChild(i);
            //print(Bgs[i]);
        }
    }
    private void Update() {
        MoveToward();
    }
   
    public void OtherDragStart() {
        //TargetPos = Bgs[Grid.childCount - 1].localPosition;
        OnDragStart();
    }
    public void OtherDragEnd() {
        OnDragEnd();
    }
    public void OtherDrag(GameObject go, Vector2 delta) {
        OnDrag(delta);
    }
    private Vector3 OldPos;
    private Vector3 DifPos;
    public void MoveToward() {
        DifPos = Target.localPosition - OldPos;
        OldPos = Target.localPosition;
        for(int i = 0; i < Bgs.Length; i++)
        {
            Bgs[i].localPosition += DifPos * Speeds[i];
        }
    }
}





















































