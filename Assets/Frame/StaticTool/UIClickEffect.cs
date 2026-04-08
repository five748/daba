using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class UIClickEffect : MonoBehaviour
{
    public int musicIndex = 1;
    private Vector3 OldScan;
    public static bool isClicking = false;
    public static float changeDiff = 0.08f;
    public static float speed = 0.02f;
    public static float validTime = 0.2f;
    public bool isLockByEventListen = true;
    private bool isChangeByColor = true;
    public NewImage[] NeedChangeColorImages = new NewImage[0];
    public NewText[] NeedChangeColorTexts = new NewText[0];
    public string needChangeChildPath = "";
    public bool ClickZoom = true;
    private float zoom_scale = 1.1f;
    private Vector3 oldSize = Vector3.zero;
    private Coroutine ChangeBigCo;
    private void Start()
    {
        if(oldSize == Vector3.zero) oldSize = GetComponent<RectTransform>().localScale;
    }
    private void Awake()
    {
        if (isChangeByColor)
        {
            if ((NeedChangeColorImages == null || NeedChangeColorImages.Length == 0) && (NeedChangeColorTexts == null || NeedChangeColorTexts.Length == 0))
            {
                var tran = transform;
                if (needChangeChildPath != "")
                {
                    tran = transform.Find(needChangeChildPath);
                }
                var image = tran.GetComponent<NewImage>();
                if (image != null)
                {
                    NeedChangeColorImages = new NewImage[1];
                    NeedChangeColorImages[0] = image;
                }
                else
                {
                    var text = tran.GetComponent<NewText>();
                    NeedChangeColorTexts = new NewText[1];
                    NeedChangeColorTexts[0] = text;
                }
            }
            int index = -1;

            if (NeedChangeColorImages.Length != 0)
            {
                foreach (var item in NeedChangeColorImages)
                {
                    index++;
                    if (item != null)
                    {
                        item.oldColor = item.color;
                    }
                }
            }
            //if (NeedChangeColorTexts.Length != 0)
            //{
            //    foreach (var item in NeedChangeColorTexts)
            //    {
            //        index++;
            //        if (item != null)
            //        {
            //            item.oldColor = item.color;
            //        }
            //    }
            //}
        }
    }

    public void ClickBig()
    {
        MusicMgr.Instance.PlaySound(musicIndex);
        if (isChangeByColor)
        {
            if (isTextGrey())
            {
                return;
            }
            if (NeedChangeColorImages.Length != 0 || ClickZoom)
            {
                if (ChangeBigCo != null)
                    StopCoroutine(ChangeBigCo);
                ChangeBigCo = StartCoroutine(ChangeImageBlack());
            }
        }
    }
    private bool isTextGrey()
    {
        if (ClickZoom)
        {
            return false;
        }
        if (NeedChangeColorTexts == null)
        {
            return false;
        }
        if (NeedChangeColorTexts.Length == 0)
        {
            return false;
        }
        if (NeedChangeColorTexts[0] == null)
        {
            return false;
        }
        return NeedChangeColorTexts[0].isGrey;
    }
    private Vector3 GetInpectorEulers(Transform myGo)
    {
        Vector3[] corners = new Vector3[4];
        myGo.GetComponent<RectTransform>().GetWorldCorners(corners);
        var X = corners[0].x + (corners[2].x - corners[0].x) / 2;
        var Y = corners[0].y + (corners[2].y - corners[0].y) / 2;
        return new Vector3(X, Y, 0);
    }
    private bool ischanged = false;

    public IEnumerator ChangeImageBlack()
    {
        float sum = 1;
        var needSum = 0.85f;
        //MusicMgr.Instance.PlaySound(1);
        if (ClickZoom)
        {
            var itemRT = this.GetComponent<RectTransform>();
            //var nowPos = GetInpectorEulers(itemRT);
            //itemRT.pivot = new Vector2(0.5f, 0.5f);
            //itemRT.position = new Vector3(nowPos.x, nowPos.y, itemRT.position.z);
            if (oldSize == Vector3.zero)
            {
                oldSize = itemRT.localScale;
            }
            sum = oldSize.x;
            needSum = oldSize.x * zoom_scale;
        }
        ischanged = true;
        while (true)
        {
            sum = zoom_scale > 1 ? sum + speed : sum - speed;
            if (ClickZoom)
            {
                GetComponent<RectTransform>().localScale = new Vector3(sum, sum, sum);
            }
            else
            {
                for (int i = 0; i < NeedChangeColorImages.Length; i++)
                {
                    if (NeedChangeColorImages[i] == null)
                    {
                        yield break;
                    }
                    NeedChangeColorImages[i].color = new Color(sum, sum, sum, 1);
                }
                for (int i = 0; i < NeedChangeColorTexts.Length; i++)
                {
                    if (NeedChangeColorTexts[i] == null)
                    {
                        yield break;
                    }
                    NeedChangeColorTexts[i].Color = new Color(sum, sum, sum, 1);
                }
            }

            if (zoom_scale < 1 && sum <= needSum || zoom_scale >= 1 && sum >= needSum)
            {
                yield break;
            }
            yield return null;
        }
    }
    public IEnumerator ClickZoomOut(Transform item)
    {
        float sum = item.GetComponent<RectTransform>().localScale.x;
        ischanged = false;
        while (true)
        {
            if (item == null || transform == null)
            {
                yield break;
            }
            sum = zoom_scale <= 1 ? sum + speed : sum - speed;

            item.GetComponent<RectTransform>().localScale = new Vector3(sum, sum, sum);
            if (zoom_scale >= 1 && sum <= oldSize.x || zoom_scale < 1 && sum >= oldSize.x)
            {
                item.GetComponent<RectTransform>().localScale = oldSize;
                yield break;
            }
            yield return null;
        }
    }
    Coroutine zoomOut;

    public void ClickSmall(System.Action callback)
    {
        if (isTextGrey())
        {
            return;
        }
        if (isChangeByColor)
        {
            if (ChangeBigCo != null)
                StopCoroutine(ChangeBigCo);
            if (ClickZoom)
            {
                if (ischanged)
                {
                    if (zoomOut != null)
                    {
                        zoomOut.Stop();
                    }
                    zoomOut = MonoTool.Instance.StartCor(ClickZoomOut(transform));
                }
            }
            else
            {
                int index = -1;
                if (NeedChangeColorImages.Length != 0)
                {
                    foreach (var item in NeedChangeColorImages)
                    {
                        index++;
                        if (item != null && item.transform != null)
                        {
                            if (item.oldColor.a != 0)
                            {
                                item.color = item.oldColor;

                            }
                        }
                    }
                }
                if (NeedChangeColorTexts.Length != 0)
                {
                    foreach (var item in NeedChangeColorTexts)
                    {
                        index++;
                        if (item != null && item.transform != null)
                        {
                            if (item.oldColor.a != 0)
                                item.Color = item.oldColor;
                        }
                    }
                }
            }

        }
    }
}