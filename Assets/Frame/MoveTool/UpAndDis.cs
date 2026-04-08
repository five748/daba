using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpAndDis : MonoBehaviour
{
    private int maxNum = 50;
    public void Start()
    {
        MonoTool.Instance.StartCor(BeginMove());
    }
    //public void OnEnable()
    //{
    //    Debug.LogError("OnEnable");
    //}
    //public void OnDisable()
    //{
    //    Debug.LogError("OnDisable");
    //}

    private IEnumerator BeginMove() {
        if(transform == null)
        {
            yield break;
        }
        var tran = transform.GetComponent<RectTransform>();
        float alpahadd = 0.01f;
        CanvasGroup can = transform.GetOrAddComponent<CanvasGroup>();
        can.alpha = 1;
        Vector2 addPos = new Vector2(0, 8.5f);
        int num = 0;
        while (true)
        {
            if (tran == null)
            {
                yield break;
            }
            num++;
            tran.anchoredPosition += addPos;
            can.alpha -= alpahadd;
            if (num >= maxNum)
            {
                if(gameObject)
                    GameObject.Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
