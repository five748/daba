using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideHollowRect : GuideHollowBase
{
    private float r;//镂空半径
    private float scaleR;//变化后的半径大小 如果需要缩放的话

    public override void Guide(Canvas canvas, RectTransform target, float r = 0)
    {
        base.Guide(canvas, target);
        
        //计算宽高
        float width = (targetCorners[3].x - targetCorners[0].x) / 2;
        float height = (targetCorners[1].y - targetCorners[0].y) / 2;

        material.SetFloat("_SliderX", width);
        material.SetFloat("_SliderY", height);
    }

    public override void Guide(Canvas canvas, RectTransform target, float scale, float time)
    {
        Guide(canvas, target);
        scaleR = r * scale;
        material.SetFloat("_Slider", scaleR);
        timer = time;
        isScaling = true;
        timer = time;
    }

    protected override void Update()
    {
        base.Update();
        if (isScaling)
        {
            material.SetFloat("_Slider", Mathf.Lerp(scaleR, r, timer));
        }
    }
}