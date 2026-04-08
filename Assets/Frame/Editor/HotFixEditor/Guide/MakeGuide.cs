using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[Serializable]
public class GuideUI
{
    public string desc;//描述
    public GameObject _Desc;//描述预制体
    private GameObject __Desc;
    public GameObject Desc
    {
        get
        {
            return __Desc;
        }
        set
        {
            if (__Desc == value)
            {
                return;
            }
            __Desc = value;
            if (value != null)
            {
                descPosition = value.GetComponent<RectTransform>().anchoredPosition;
            }
        }
    }
    public Vector2 descPosition;//描述位置

    public GameObject _Hand;//手指预制体
    private GameObject __Hand;
    public GameObject Hand
    {
        get
        {
            return __Hand;
        }
        set
        {
            if (__Hand == value)
            {
                return;
            }
            __Hand = value;
            if (value != null)
            {
                HandPosition = value.GetComponent<RectTransform>().anchoredPosition;
                HandRotation = value.GetComponent<RectTransform>().rotation.eulerAngles;
            }
        }
    }
    public Vector2 HandPosition;//手指位置
    public Vector3 HandRotation;//手指旋转角度

    public int ArrowNum;
    public GameObject _Arrow = null;
    private GameObject __Arrow;
    public GameObject Arrow
    {
        get
        {
            return __Arrow;
        }
        set
        {
            if (__Arrow == value)
            {
                return;
            }
            __Arrow = value;
            if (value != null)
            {
                ArrowPosition = value.GetComponent<RectTransform>().anchoredPosition;
                ArrowRotation = value.GetComponent<RectTransform>().rotation.eulerAngles;
            }
        }
    }
    public Vector2 ArrowPosition;//箭头位置
    public Vector3 ArrowRotation;//箭头旋转角度
}

public class MakeGuide : MonoBehaviour
{
    public List<GuideUI> guideList = new List<GuideUI>();

    private void Update()
    {
        foreach (var item in guideList)
        {
            item.Desc = item._Desc;
            item.Hand = item._Hand;
            item.Arrow = item._Arrow;
        }
    }
}
