using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITool:Single<UITool>
{
    public void SetAnchor(Transform myTran)
    {
        if (myTran == null)
        {
            Debug.LogError("error: myTran is null");
            return;
        }
        if (!ProssData.Instance.isSetCanvasSize)
        {
            return;
        }
        var rt = myTran.GetComponent<RectTransform>();
        rt.sizeDelta = ProssData.Instance.CanvasSize;
        var top = myTran.Find("top");
        if (top)
        {
            top.GetComponent<RectTransform>().anchoredPosition -= ProssData.Instance.CanvasModifyTop;
        }
        //var down = myTran.Find("down");
        //if (down)
        //{
        //    down.GetComponent<RectTransform>().anchoredPosition += ProssData.Instance.CanvasModifyDown;
        //}
    }
}
