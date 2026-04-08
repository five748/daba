using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UIDepth : MonoBehaviour
{
    public int order;
    public bool isUI = true;
    public bool isUpData = false;
    public bool isEnableSet = true;
    public bool isLockLayout;
    private void Awake()
    {

    }
    public void SetDepth(int _order)
    {
        //if (transform.name == "fire1") {
        //    Debug.LogError("fire1");
        //}
        SetDepth(_order, ProssData.Instance.TipsCount);
    }
    public void SetDepth()
    {
        SetDepth(order, ProssData.Instance.TipsCount);
    }
    public void AddSetDepth(int Num)
    {
        var newOrder = order + Num;
        SetDepth(newOrder, ProssData.Instance.TipsCount);
    }
    public void SetDepthByTipCount(int tipCount)
    {
        SetDepth(order, tipCount);
    }
    public void SetDepth(int depth, int tipCount)
    {
        //Debug.LogError(transform.name + tipCount);
        order = depth;
        if (isUI)
        {
            Canvas canvas = GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = gameObject.AddComponent<Canvas>();
                gameObject.AddComponent<GraphicRaycaster>();
            }
            canvas.overrideSorting = true;
            canvas.sortingOrder = order;
            if (tipCount == 0)
            {
                canvas.sortingLayerName = "Default";
            }
            else
            {
                canvas.sortingLayerName = "Tip" + (tipCount + 1);
            }
        }
        else
        {
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                Destroy(gameObject.GetComponent<GraphicRaycaster>());
                Destroy(canvas);
            }
            Renderer[] renders = GetComponentsInChildren<Renderer>(true);
            //Debug.LogError(transform.name + ":" + UIManager.OpenTipNames.Count);
            foreach (Renderer render in renders)
            {
                render.sortingOrder = order;
                if (tipCount == 0)
                {
                    render.sortingLayerName = "Default";
                }
                else
                {
                    render.sortingLayerName = "Tip" + (tipCount + 1);
                }
            }
        }
    }

    public void SetSortingLayer(string layerString)
    {
        Canvas[] canvas = GetComponentsInChildren<Canvas>(true);
        foreach (var can in canvas)
        {
            can.overrideSorting = true;
            can.sortingLayerName = layerString;
        }
        Renderer[] renders = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer render in renders)
        {
            render.sortingOrder = order;
            render.sortingLayerName = layerString;
        }
    }
    public void SetDepthLayout(int layoutNum)
    {
        if (isLockLayout) {
            return;
        }
        string layoutName = "";
        if (layoutNum == 0)
        {
            layoutName = "Default";
        }
        else
        {
            layoutName = "Tip" + layoutNum;
        }
        SetDepthLayout(layoutName);
    }
    public void SetDepthLayout(string layoutName)
    {
        if (gameObject.GetComponent<Canvas>() == null)
        {
            gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }
        gameObject.GetComponent<Canvas>().additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;
        Canvas[] canvas = GetComponentsInChildren<Canvas>(true);
        foreach (var can in canvas)
        {
            can.overrideSorting = true;
            can.sortingLayerName = layoutName;
            can.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1 | AdditionalCanvasShaderChannels.TexCoord2 | AdditionalCanvasShaderChannels.TexCoord3;
        }

        Renderer[] renders = GetComponentsInChildren<Renderer>(true);
        foreach (Renderer render in renders)
        {
            render.sortingLayerName = layoutName;
        }
    }

    public void SetUIAndEffectDepth(int depth)
    {
        order = depth;
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }
        canvas.overrideSorting = true;
        canvas.sortingOrder = order;
        Renderer[] renders = GetComponentsInChildren<Renderer>(true);

        foreach (Renderer render in renders)
        {
            render.sortingOrder = order;
        }
    }
    public void SetAddDepth(int depth)
    {
        order = order % 1000;
        order += depth;
        SetDepth(order);
    }
    public void OnEnable()
    {
        if (isEnableSet)
            SetDepth(order);
        
    }
    void OnGUI()
    {
        if (!isUpData)
        {
            return;
        }
        isUpData = false;
        SetDepth(order);
    }
}





















































