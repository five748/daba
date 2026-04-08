using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewVerticalLayoutGroup : VerticalLayoutGroup
{
    public float modifySize = 10;
    protected new void SetChildAlongAxis(RectTransform rect, int axis, float pos, float size)
    {
        base.SetChildAlongAxis(rect, axis, pos, size + modifySize);
    }
}
