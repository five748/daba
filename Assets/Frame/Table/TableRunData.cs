using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;

public class TableRunData : Single<TableRunData>
{

    public void Init() {
        InitFrame();
    }
    public Dictionary<string, Table.frame> frameNumAndSpeed;
    private void InitFrame() {
        frameNumAndSpeed = new Dictionary<string, frame>();
        foreach (var one in FrameTableCache.Instance.frameTable)
        {
            frameNumAndSpeed.Add(one.Value.enumKey, one.Value);
        } 
    }
}
