using System.Net.Mime;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MsgOne
{
    private BuffPool<GameObject> MscPool;
    private GameObject WarnOne;
    public Transform WarnParent;
    public MsgOne(Transform target)
    {
        if (target == null) {
            return;
        }
        WarnParent = target;
        WarnOne = WarnParent.GetChild(0).gameObject;
        if (MscPool == null)
        {
            MscPool = new BuffPool<GameObject>(Create, Used, Free);
        }
    }
    public void Show(WarnOne data, int maxNum, bool needwait, System.Action over)
    {
        try
        {
            MonoTool.Instance.StartCor(WarnIE(data, maxNum, needwait, over));
        }
        catch { 
            
        }
    }
    public void ShowEffect(WarnOne data, float speed, bool needwait, System.Action over)
    {
        MonoTool.Instance.StartCor(EffectIE(data, speed, needwait, over));
    }
    private GameObject Create()
    {
        GameObject go = GameObject.Instantiate(WarnOne);
        go.transform.SetParent(WarnParent);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;
        return go;
    }
    private void Used(GameObject go)
    {
        if (!go)
        {
            return;
        }
        go.GetComponent<RectTransform>().SetAsFirstSibling();
        go.SetActive(true);
        go.transform.GetOrAddComponent<CanvasGroup>().alpha = 0;
    }
    private void Free(GameObject go)
    {
        go.transform.localPosition = Vector3.zero;
        go.SetActive(false);
    }
    private IEnumerator WarnIE(WarnOne data, int maxNum, bool needwait = true, System.Action callback = null)
    {
        if (MscPool == null)
            yield break;
        RectTransform tran = MscPool.GetOne().GetComponent<RectTransform>();
        if (data.father != null)
        {
            tran.parent = data.father;
            tran.position = data.father.position;
        }
        Vector2 addPos = new Vector2(0, 1.5f);
        var text = tran.Find("text");
        text.SetText(data.msc);
        if (data.id != 0)
            text.Find("icon").GetComponent<Image>().SetImageAndActive(data.iconPath);
        int num = 0;
        float alpahadd = 0.01f;
        CanvasGroup can = tran.GetComponent<CanvasGroup>();
        can.alpha = 1;
        if (data.id == 0)
        {
            if (needwait)
                yield return new WaitForSeconds(1f);
        }
        while (true)
        {
            num++;
            if (tran == null)
            {
                yield break;
            }
            tran.anchoredPosition += addPos;
            if (data.id == 0)
                can.alpha -= alpahadd;
            if (num >= maxNum)
            {
                if (data.id != 0)
                {
                    yield return new WaitForSeconds(1f);
                }
                if (tran != null)
                {
                    MscPool.Free(tran.gameObject);
                    can.alpha = 1.0f;
                }

                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }
    static bool ISFri = true;
    private IEnumerator EffectIE(WarnOne data, float speed = 0.02f, bool needwait = true, System.Action callback = null)
    {
        if (MscPool == null)
            yield break;
        RectTransform tran = MscPool.GetOne().GetComponent<RectTransform>();
        if (ISFri)
        {
            ISFri = false;
            yield break;
        }
        var text = tran.Find("name");
        var shadow = tran.Find("shadow");
        text.SetText(data.msc);
        shadow.SetText(data.msc);
        bool isShow = true;
        bool full = false;
        CanvasGroup can = tran.GetComponent<CanvasGroup>();
        can.alpha = 0;

        if (needwait)
            yield return new WaitForSeconds(1f);
        while (true)
        {
            if (tran == null)
            {
                yield break;
            }
            if (isShow)
            {
                can.alpha += speed;
                if (can.alpha >= 1)
                {
                    can.alpha = 1.0f;
                    isShow = false;
                }
            }
            else
            {
                if (!full)
                {
                    yield return new WaitForSeconds(1f);
                    full = true;
                }
                if (can == null)
                {
                    yield break;
                }
                can.alpha -= speed;
                if (can.alpha <= 0)
                {
                    MscPool.Free(tran.gameObject);
                    if (callback != null)
                    {
                        callback();
                    }
                    yield break;
                }
            }
            yield return null;
        }
    }
}
