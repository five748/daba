﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMove : MonoBehaviour
{
    private float waitTime = 0.04f;
    private Transform[] Items;
    private int ChooseIndex = 0;
    private int ItemLen;
    public Transform Grid;
    public int WidthNum;
    public int HighNum;
    public int modifyX;
    public int modifyY;
    private CanvasGroup[] chooses;

    public Transform[] Init()
    {
        Init(WidthNum, HighNum, Grid);
        return Items;
    }
    public void Init(int widthNum, int highNum, Transform grid)
    {
        if (widthNum == 0 || highNum == 0 || grid == null || grid.childCount == 0) {
            return;
        }
        SetItemPos(widthNum, highNum, grid);
    }
    private void SetItemPos(int widthNum, int highNum, Transform grid) {
        RectTransform baseitem = grid.GetChild(0).GetComponent<RectTransform>(); 
        var poses = CalculationsPos(baseitem.sizeDelta.x + modifyX, baseitem.sizeDelta.y + modifyY, widthNum, highNum);
        ItemLen = poses.Count;
        Items = grid.AddChilds(poses.Count);
        chooses = new CanvasGroup[ItemLen];
        int index = -1;
        foreach (var item in Items)
        {
            index++;
            item.GetComponent<RectTransform>().anchoredPosition = poses[index];
            item.SetActive(true);
            chooses[index] = item.Find("choose").GetComponent<CanvasGroup>();
        }
    }
    private List<Vector2> CalculationsPos(float oneWidth, float oneHight, int widthNum, int highNum) {
        List<Vector2> poses = new List<Vector2>();
        Vector2 firstPos = new Vector2(-(oneWidth * (widthNum - 1) * 0.5f), (oneHight * (highNum - 1) * 0.5f));
        poses.Add(firstPos);
        int dir = 0;
        int sum = 0;
        while (true) {
            sum++;
            if (dir == 3)
            {
                firstPos = firstPos + new Vector2(0, oneHight);
                if (sum >= highNum - 1)
                {
                    sum = 0;
                    break;
                }
            }
            if (dir == 2)
            {
                firstPos = firstPos - new Vector2(oneWidth, 0);
                if (sum >= widthNum - 1)
                {
                    sum = 0;
                    dir++;
                }
            }
            if (dir == 1)
            {
                firstPos = firstPos - new Vector2(0, oneHight);
                if (sum >= highNum - 1)
                {
                    sum = 0;
                    dir++;
                }
            }
            if (dir == 0) {
                firstPos = firstPos + new Vector2(oneWidth, 0);
                if (sum >= widthNum - 1) {
                    sum = 0;
                    dir++;
                }
            }
            poses.Add(firstPos);
        }
        return poses;
    }
    public void MoveOnce(int stopIndex, int loopNum, System.Action callback = null) {
        //EventTriggerListener.IsLock = true;
        StartCoroutine(MoveIE(stopIndex, loopNum, () => {
            //EventTriggerListener.IsLock = false;
            if(callback != null)
                callback();
        })); 
    }
    private List<int> StopIndexs;
    private System.Action<int> GetAwardEvent;
    private bool isMoving = false;
    public void MoveNum(List<int> stopIndexs, System.Action<int> getAward, System.Action callback) {
        isMoving = true;
        GetAwardEvent = getAward;
        StopIndexs = stopIndexs;
        MoveLoop(0, 3, () => {
            callback();
        });
    }
    public void MoveLoop(int index, int loopNum, System.Action callback = null) {
        if (index >= StopIndexs.Count)
        {
            callback();
            return;
        }
        int stopIndex = StopIndexs[index];
        if (index > 0)
        {
            loopNum = 1;
        }
        MoveOnce(stopIndex, loopNum, () => {
            GetAward(index);
            MonoTool.Instance.Wait(0.5f, () => {
                index++;
                MoveLoop(index, loopNum, callback);
            });
        });
    }
    private void GetAward(int index) {
        GetAwardEvent(index);
    }
    private int StopIndex;
    private System.Action MoveEndCallback;
    private IEnumerator MoveIE(int stopIndex, int loopnum, System.Action callback) {
        MoveEndCallback = callback;
        isMoving = true;
        int loopSum = 0;
        chooses[StopIndex].alpha = 0;
        StopIndex = stopIndex;
        while (true) {
            ChooseIndex++;
            if (ChooseIndex >= ItemLen) {
                loopSum++;
                ChooseIndex = 0;
            }
            SetHight(ChooseIndex);
            if (loopSum > loopnum)
            {
                isMoving = false;
                if (ChooseIndex == stopIndex) {
                    yield break;
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }
    private Coroutine Co;
    private void SetHight(int index) {
        chooses[index].alpha = 1;
        if (Co == null) {
            Co = StartCoroutine(SetAlpha());
        }
    }
    private IEnumerator SetAlpha() {
        while (true) {
            for (int i = 0; i < ItemLen; i++)
            {
                if (!isMoving && i == StopIndex)
                {
                    //chooses[i].alpha = 1;
                }
                else {
                    chooses[i].alpha -= 0.3f;
                }
            }
            if (!isMoving) {
                bool isOver = true;
                for (int i = 0; i < ItemLen; i++)
                {
                    if (i != StopIndex && chooses[i].alpha > 0) {
                        isOver = false;
                        break;
                    }
                }
                if (isOver) {
                    Co = null;
                    MoveEndCallback();
                    yield break;
                }
            }
            yield return new WaitForSeconds(waitTime);
        }
    }

}







