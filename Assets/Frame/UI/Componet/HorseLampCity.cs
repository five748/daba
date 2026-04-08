using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseLampCity : MonoBehaviour
{
    [Header("遮罩")]
    public RectTransform mask;
    [Header("文本框")]
    public RectTransform text;
    public float speed;
    public bool loop;
    public System.Action callback;
    bool inited = false;
    public bool inMiddle = false;
    float moveLength;
    Vector2 change;
    Vector3[] poses;
    Vector3[] fatherposes;
    public void Init(string str)
    {
        inited = false;

        text.GetComponent<NewText>().text = str;

        MonoTool.Instance.WaitEndFrame(() =>
        {

            poses = new Vector3[4];
            text.GetLocalCorners(poses);

            fatherposes = new Vector3[4];
            mask.GetLocalCorners(fatherposes);

            if (poses[2].x - poses[1].x < fatherposes[2].x - fatherposes[1].x)
            {
                if (inMiddle)
                {
                    text.anchorMin = new Vector2(0.5f, 0.5f);
                    text.anchorMax = new Vector2(0.5f, 0.5f);
                    text.pivot = new Vector2(0.5f, 0.5f);
                }
                text.anchoredPosition = Vector2.zero;
                return;
            }
            else
            {
                text.anchorMin = new Vector2(0f, 0.5f);
                text.anchorMax = new Vector2(0f, 0.5f);
                text.pivot = new Vector2(0f, 0.5f);
                text.anchoredPosition = Vector2.zero;
            }

            text.GetLocalCorners(poses);
            moveLength = poses[2].x;

            change = new Vector2(speed, 0);
            inited = true;
        });
    }
    public bool NoOutMask = false;
    void Update()
    {
        if (!gameObject.activeInHierarchy)
        {
            Next();
            return;
        }

        if (!inited)
        {
            Next();
            return;
        }
        if (NoOutMask)
        {
            if (text.anchoredPosition.x <= fatherposes[2].x - fatherposes[1].x - poses[2].x)
            {
                Next();
                return;
            }
        }

        if (text.anchoredPosition.x < -moveLength)
        {
            if (!loop)
            {
                Next();
                return;
            }
            text.anchoredPosition = new Vector2(fatherposes[2].x - fatherposes[1].x, text.anchoredPosition.y);
        }
        text.anchoredPosition -= change * Time.deltaTime;
    }
    void Next()
    {
        if (callback != null)
        {
            callback();
        }
    }
}
