using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using LitJson;
using Object = UnityEngine.Object;
using static UnityEngine.GraphicsBuffer;

public static class TranTool
{
    public static Coroutine BigToSmall(this Transform target, float scan, float speed = 0.02f, System.Action callback = null, bool isContinue = false, int Times = 1)
    {
        float sum = target.localScale.x;
        float old = sum;
        bool isAdd = true;
        bool end = false;
        var times = 0;
        if (isContinue || Times == 0)
        {
            if (callback != null)
            {
                callback();
            }
            if (Times == 0)
                return null;
        }
        return MonoTool.Instance.StartCor(
            () =>
            {
                if (!target)
                    return false;
                if (isAdd)
                {

                    sum += speed;
                    target.localScale = Vector3.one * sum;
                    if (sum >= scan)
                    {
                        isAdd = false;
                    }
                    return true;
                }
                else
                {
                    sum -= speed;
                    target.localScale = Vector3.one * sum;
                    if (isContinue)
                    {
                        if (sum > old)
                        {
                            isAdd = false;
                        }
                        else
                        {
                            isAdd = true;
                            if (callback != null && !end)
                            {
                                callback();
                                end = true;
                            }
                        }
                        return true;
                    }
                    else
                    {
                        if (sum > old)
                        {
                            isAdd = false;
                        }
                        else
                        {
                            isAdd = true;
                            times++;
                        }
                        return times != Times;
                    }
                }
            },
            () =>
            {
                if (target != null)
                    target.localScale = Vector3.one * old;
                if (callback != null)
                {
                    callback();
                }
            }
        );
    }
    public static void SmallToBig(this Transform target, float scan, float speed = 0.02f, System.Action callback = null)
    {

        float sum = target.localScale.x;
        float old = sum;
        bool isAdd = true;
        MonoTool.Instance.StartCor(
            () =>
            {
                if (!target)
                    return false;
                if (isAdd)
                {
                    sum -= speed;
                    target.localScale = Vector3.one * sum;
                    if (sum <= scan)
                    {
                        isAdd = false;
                    }
                    return true;
                }
                else
                {
                    sum += speed;
                    target.localScale = Vector3.one * sum;
                    return sum < old;
                }
            },
            () =>
            {
                target.localScale = Vector3.one * old;
                if (callback != null)
                {
                    callback();
                }
            }
        );
    }
    public static Vector3 GetInpectorEulers(this Transform myGo)
    {
        Vector3[] corners = new Vector3[4];
        myGo.GetComponent<RectTransform>().GetWorldCorners(corners);
        var X = corners[0].x + (corners[2].x - corners[0].x) / 2;
        var Y = corners[0].y + (corners[2].y - corners[0].y) / 2;
        return new Vector3(X, Y, 0);
    }
    public static void ToBig(this Transform target, float scan, float speed = 0.02f, System.Action callback = null)
    {

        float sum = target.localScale.x;
        float old = sum;
        MonoTool.Instance.StartCor(
            () =>
            {
                if (!target)
                    return false;
                sum += speed;
                target.localScale = Vector3.one * sum;
                if (sum <= scan)
                {
                    return true;
                }
                return false;
            },
            () =>
            {
                target.localScale = Vector3.one * scan;
                if (callback != null)
                {
                    callback();
                }
            }
        );
    }
    public static void ToSmall(this Transform target, float scan, float speed = 0.02f, System.Action callback = null)
    {

        float sum = target.localScale.x;
        float old = sum;
        MonoTool.Instance.StartCor(
            () =>
            {
                if (!target)
                    return false;
                sum -= speed;
                target.localScale = Vector3.one * sum;
                if (sum >= scan)
                {
                    return true;
                }
                return false;
            },
            () =>
            {
                target.localScale = Vector3.one * scan;
                if (callback != null)
                {
                    callback();
                }
            }
        );
    }

    public static void BigToSmallOne(this Transform target, float scan, float speed = 0.02f, System.Action callback = null)
    {
        float sum = target.localScale.x;
        float old = sum;
        bool isAdd = true;
        MonoTool.Instance.StartCor(
            () =>
            {
                if (!target)
                    return false;
                if (isAdd)
                {
                    sum += speed;
                    target.localScale = Vector3.one * sum;
                    if (sum >= scan)
                    {
                        isAdd = false;
                    }
                    return true;
                }
                else
                {
                    sum -= speed;
                    target.localScale = Vector3.one * sum;
                    return sum > old;
                }
            },
            () =>
            {
                if (target == null)
                    return;
                target.localScale = Vector3.one * old;
                if (callback != null)
                {
                    callback();
                }
            }
        );
    }
    public static void CopyAndMoveTo(this Transform tran, Transform target, float speed = 0.1f, System.Action callback = null)
    {
        var item = tran.CopyToTargetOne(target).GetComponent<RectTransform>();
        item.MoveToAwardRt(Vector3.zero, speed, () =>
        {
            GameObject.Destroy(item.gameObject);
            if (callback != null)
            {
                callback();
            }
        });
    }
    public static void MoveToRT(this Transform tran, Transform target, float speed = 0.1f, System.Action callback = null)
    {
        tran.SetParent(target);
        tran.SetDepthTop();
        tran.GetComponent<RectTransform>().MoveToAwardRt(Vector3.zero, speed, () =>
        {
            GameObject.Destroy(tran.gameObject);
            if (callback != null)
            {
                callback();
            }
        });
    }

    public static IEnumerator AddChildrenIE(int num, Transform grid, int per_count, Action<Transform, int> update_call)
    {
        Transform[] items = new Transform[num];
        int left = per_count;
        for (int i = 0; i < num - 1; i++)
        {
            Transform item = AddChildOne(grid);
            update_call(item, i + 1);
            items[i] = item;
            left--;
            if (left <= 0)
            {
                left = per_count;
                yield return new WaitForFixedUpdate();
            }
        }

        update_call(grid.GetChild(0), 0);
        yield return new WaitForFixedUpdate();
    }
    
    public static Transform[] AddChilds(int num, Transform grid, Vector2 space)
    {
        Transform[] items = AddChilds(num, grid);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(i * space.x, i * space.y);
        }
        return items;
    }
    public static Transform[] AddChilds(int num, Transform grid)
    {
        Transform[] items = new Transform[num];
        int len = grid.childCount;
        if (num == 0)
        {
            for (int i = 0; i < len; i++)
            {
                grid.GetChild(i).gameObject.SetActive(false);
            }
            return items;
        }
        if (grid.childCount >= num)
        {
            for (int i = 0; i < len; i++)
            {
                if (i < num)
                {
                    items[i] = grid.GetChild(i);
                }
                grid.GetChild(i).gameObject.SetActive(i < num);
            }
            return items;
        }
        else
        {
            for (int i = 0; i < len; i++)
            {
                items[i] = grid.GetChild(i);
            }
        }
        items[0] = grid.GetChild(0);
        items[0].name = "0";
        items[0].gameObject.SetActive(true);
        for (int i = len; i < num; i++)
        {
            items[i] = GameObject.Instantiate(items[0]);
            items[i].SetParent(grid);
            items[i].localScale = Vector3.one;
            items[i].gameObject.SetActive(true);
            items[i].name = i.ToString();
        }
        return items;
    }
    public static void SetActive(this Transform item, bool isActive)
    {
        //if (item && item.parent != null)
        //{
        //    if (item.parent.name == "hudgrid")
        //    {
        //        Debug.LogError(item.name + ":" + isActive);
        //    }
        //}
        if (item)
            item.gameObject.SetActive(isActive);
    }
    public static void SetActive(this Transform item, bool isActive, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        item.gameObject.SetActive(isActive);
    }
    public static void SetFatherAllActiveTrue(this Transform item, string path)
    {
        var paths = path.Split('/');
        string str = "";
        foreach (var one in paths)
        {
            str += one;
            //Debug.LogError(str);
            var tran = item.Find(str);
            if (tran)
            {
                tran.SetActive(true);
            }
            else {
                Debug.LogError("找不到路径:" + str);
            }
            str += "/";
        }
    }
    public static void SetTextOneNoShow(this Transform item, string str, string path = "")
    {
        if (str == "1")
        {
            str = "";
        }
        item.SetText(str, path);
    }
    public static void SetMeshText(this Transform item, string str, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        //Debug.LogError("text:" + str);
        item.GetComponent<TextMesh>().text = str;
    }
    public static void SetText(this Transform item, string str, string path = "")
    {
        if (item == null)
        {
            Debug.LogError("参数 item 不能为空");
        }
        string fatherName = item.name;
        if (path != "")
        {
            item = item.Find(path);
        }
        if (item == null)
        {
            Debug.LogError($"找不到{fatherName}下的{path}");
            return;
        }
        Text t = item.GetComponent<Text>();
        if (t == null)
        {
            Debug.LogError($"在{item.name}上没有找到Text组件");
            return;
        }
        item.GetComponent<Text>().text = str;
    }
    public static void SetText(this Transform item, long str, int index, string path = "")
    {
        item.SetText(str.ToString(), index, path);
    }
    public static void SetText(this Transform item, string str, int index, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        var newText = item.GetComponent<NewText>();
        newText.keyIndex = index;
        newText.text = str;
    }
    public static void SetText(this Transform item, int str, int key, string path = "")
    {
        item.SetText(str.ToString(), key, path);
    }
    public static void SetText(this Transform item, object o, string path = "", int fontSize = -1)
    {
        if (item == null)
        {
            Debug.LogError("参数 item 不能为空");
        }
        string fatherName = item.name;
        if (path != "")
        {
            item = item.Find(path);
        }
        if (item == null)
        {
            Debug.LogError($"找不到{fatherName}下的{path}");
            return;
        }
        Text text = item.GetComponent<Text>();
        if (text == null)
        {
            Debug.LogError($"在{item.name}上没有找到Text组件");
            return;
        }
        text.text = o.ToString();
        if (fontSize > 0)
        {
            text.fontSize = fontSize;
        }
    }
    public static void SetText(this Transform item, int str, string path = "")
    {
        item.SetText(str.ToString(), path);
    }
    public static void SetText(this Transform item, uint str, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        item.GetComponent<Text>().text = ((ulong)(str)).ChangeNum();
    }
    public static void SetText(this Transform item, ulong str, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        item.GetComponent<Text>().text = str.ChangeNum();
    }
    public static void SetText(this Transform item, ulong str, bool active)
    {
        item.GetComponent<Text>().text = str.ChangeNum();
        item.gameObject.SetActive(active);
    }
    public static void SetSlider(this Transform item, float fill, int index, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        var newSlder = item.GetComponent<NewSlider>();
        //newSlder.keyIndex = index;
        newSlder.SetFill(fill);
    }
    public static void SetEvent(this Transform item, List<int> keyIndexs, int index, string path = "")
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        var eventTrigger = item.GetComponent<EventTriggerListener>();
        eventTrigger.keyIndexs = keyIndexs;
    }
    public static void SetImage(this Transform item, int imageName, string path = "", bool isAutoSize = false)
    {
        item.SetImage(imageName.ToString(), path, isAutoSize);
    }
    public static void SetImage(this Transform item, int imageName, int key, string path = "", bool isAutoSize = false)
    {
        item.SetImage(imageName.ToString(), key, path, isAutoSize);
    }

    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="item"></param>
    /// <param name="imageName"> 图片的名称 </param>
    /// <param name="path"> 与 item 的父子关系路径 </param>
    /// <param name="isAutoSize">  </param>
    /// <param name="transparency"> 透明度 </param>
    public static void SetImage(this Transform item, string imageName, string path = "", bool isAutoSize = false, float transparency = 1f)
    {
        if (item == null)
        {
            //Debug.LogError("参数 item 不能为空");
            return;
        }
        if (path != "")
        {
            item = item.Find(path);
        }
        //Debug.LogError(imageName);
        item.GetComponent<Image>().SetImage(imageName, isAutoSize, transparency);
    }
    public static void SetMeshImage(this MeshRenderer meshRender, string imageName)
    {
        AssetLoadOld.Instance.LoadTextureImage(meshRender.transform, FrameConfig.ImagePath + "ChangeImage/" + imageName, (texture) =>
        {
            if (texture == null)
            {
                return;
            }
            meshRender.material.SetTexture("_MainTex", texture);
        });
    }

    public static void SetImage(this Transform item, string imageName, int index, string path = "", bool isAutoSize = false)
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        //Debug.LogError(path);
        var newImage = item.GetComponent<NewImage>();
        newImage.keyIndex = index;
        newImage.SetImage(imageName, isAutoSize);
    }
    //public static void SetImage(this Image image, string imageName, bool isAutoSize = false, float transparency = 1f)
    //{
    //    AssetLoadOld.Instance.LoadImage(imageName, (go) =>
    //    {
    //        if (go != null)
    //        {
    //            image.sprite = go as Sprite;
    //            if (isAutoSize)
    //            {
    //                image.SetNativeSize();
    //            }
    //            var c = image.color;
    //            c.a = transparency;
    //            image.color = c;
    //        }
    //        else
    //        {
    //            Debug.Log("找不到图片:" + imageName);
    //        }
    //    });
    //}
    public static void SetImage(this Image image, string imageName, bool isAutoSize = false, float transparency = 1f, Action<Sprite> call = null)
    {
        AssetLoadOld.Instance.LoadImage(image.transform, imageName, (go) =>
        {
            if (go != null)
            {
                if (image == null) {
                    call?.Invoke(go);
                    return;
                }
                image.sprite = go;
                if (isAutoSize)
                {
                    image.SetNativeSize();
                }
                var c = image.color;
                c.a = transparency;
                image.color = c;
                
                call?.Invoke(go);
            }
            else
            {
                Debug.Log("找不到图片:" + imageName);
            }
        });
    }

    public static void SetImageNor(this Image image, string imageName, bool isAutoSize = false, float transparency = 1f)
    {
        AssetLoadOld.Instance.LoadImageAsset(FrameConfig.ImagePath + imageName, ".png", (go) =>
        {
            if (go != null)
            {
                image.sprite = go as Sprite;
                if (isAutoSize)
                {
                    image.SetNativeSize();
                }
                var c = image.color;
                c.a = transparency;
                image.color = c;
            }
            else
            {
                Debug.LogError("找不到图片:" + FrameConfig.ImagePath  + "/" + imageName);
            }
        });
    }
    public static void SetImageAndEnable(this Image image, string imageName)
    {
        AssetLoadOld.Instance.LoadImage(image.transform, imageName, (go) =>
        {
            if (go != null)
            {
                image.sprite = go as Sprite;
                image.enabled = true;
            }
            else
            {
                //Debug.LogError("找不到:"+ imageName);
                image.enabled = false;
            }
        });
    }
    public static void SetImageAndActive(this Image image, string imageName)
    {
        AssetLoadOld.Instance.LoadImage(image.transform, imageName, (go) =>
        {
            if (go != null)
            {
                image.sprite = go as Sprite;
                image.enabled = true;
                image.transform.SetActive(true);
            }
            else
            {
                //Debug.LogError("找不到:"+ imageName);
                image.enabled = false;
                image.transform.SetActive(false);
            }
        });
    }

    public static void SetImageFromOrigin(this Image image, string imageName, bool isAutoSize = false, float transparency = 1f)
    {
        AssetLoadOld.Instance.LoadImageFromOrigin(imageName, (go) =>
        {
            if (go != null)
            {
                image.sprite = go as Sprite;
                if (isAutoSize)
                {
                    image.SetNativeSize();
                }
                var c = image.color;
                c.a = transparency;
                image.color = c;
            }
            else
            {
                Debug.LogError("找不到图片:" + imageName);
            }
        });
    }

    public static void SetImageGrey(this Image image, bool state = true)
    {

        image.material = state ? ProssData.Instance.grey : null;
    }
    public static void SetImageGrey(this Transform tran, bool state = true, string path = "")
    {
        if (path != "")
        {
            var image = tran.Find(path).GetComponent<Image>();
            image.material = state ? ProssData.Instance.grey : null;
        }
        else
        {
            var image = tran.GetComponent<Image>();
            image.material = state ? ProssData.Instance.grey : null;
        }
    }
    public static void SetRawImage(this Transform item, string imageName, string path = "", bool isAutoSize = false)
    {
        if (path != "")
        {
            item = item.Find(path);
        }
        item.GetComponent<RawImage>().SetRawImage(imageName);
    }
    public static void SetRawImage(this RawImage rawimage, string imageName)
    {
        AssetLoadOld.Instance.LoadRawImage(rawimage.transform, imageName, (go) =>
        {
            if (go != null)
            {
                rawimage.texture = go;
            }
        });
    }
    public static void SetAlpha(this Transform tran, float alpha, string path = "")
    {
        if (path != "")
        {
            tran = tran.Find(path);
        }
        //Debug.LogError(path);
        if (tran == null)
        {
            return;
        }
        var canvas = tran.GetComponent<CanvasGroup>();
        if (canvas == null)
        {
            canvas = tran.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = alpha;
    }

    public static void SetBoxState(this Transform box, int state, string close = "close", string open = "open", System.Action<GameObject> action = null)
    {
        box.SetImageGrey(state == 0, close);
        box.SetActive(state < 2, close);
        box.SetActive(state == 2, open);
        if (action != null)
        {
            EventTriggerListener.Get(box).onClick = (button) =>
            {
                action(button);
            };
        }
    }
    public static void SetAwardState(this Transform btn, int state, string path = "", string get = "领取", string geted = "已领取")
    {
        btn.SetAllGrey(state == 2);
        btn.SetText(state == 2 ? geted : get, path);
    }
    public static void SetGrey(this Transform tran, bool isGrey)
    {
        tran.GetComponent<Image>().material = isGrey ? ProssData.Instance.grey : null;
    }
    public static void SetAllGrey(this Transform tran, bool state = true, string path = "", bool outLineGrey = true)
    {
        var tranPath = path == "" ? tran : tran.Find(path);

        foreach (var image in tranPath.GetComponentsInChildren<Image>(true))
        {
            if (image.name == "guidehand" || image.name == "guidedesc" || image.name == "guidearrow")
            {
                continue;
            }
            if (state && image.sprite != null)
            {
                if (image.sprite.name == "Icon_JiNengWeiJieSuo")
                {
                    image.material = null;
                    continue;
                }
            }
            image.material = state ? ProssData.Instance.grey : null;
        }
        foreach (var test in tranPath.GetComponentsInChildren<NewText>())
        {
            if (test.name == "guidehand" || test.name == "guidedesc" || test.name == "guidearrow")
            {
                continue;
            }
            if (state)
            {
                test.isGrey = true;
                //if (test.color != new Color(120 / 255f, 120 / 255f, 120 / 255f, 1f))
                //    test.oldColor = test.color;
                test.Color = new Color(60 / 255f, 60 / 255f, 60 / 255f, 1f);
            }
            else
            {
                if (test.isGrey)
                    test.Color = test.oldColor;
                test.isGrey = false;
            }
        }
        foreach (var GradientAuto in tranPath.GetComponentsInChildren<GradientAuto>())
        {
            if (GradientAuto.name == "guidehand" || GradientAuto.name == "guidedesc" || GradientAuto.name == "guidearrow")
            {
                continue;
            }
            GradientAuto.enabled = !state;
        }
        if (outLineGrey)
        {

            foreach (var outLine in tranPath.GetComponentsInChildren<Outline>())
            {
                if (outLine.name == "guidehand" || outLine.name == "guidedesc" || outLine.name == "guidearrow")
                {
                    continue;
                }
                outLine.enabled = !state;
            }
        }
    }
    public static void SetImageColorGrey(this Transform tran, bool state = true, string path = "", bool outLineGrey = true)
    {
        var tranPath = path == "" ? tran : tran.Find(path);

        foreach (var image in tranPath.GetComponentsInChildren<Image>(true))
        {
            if (image.name == "guidehand" || image.name == "guidedesc" || image.name == "guidearrow")
            {
                continue;
            }
            if (state && image.sprite != null)
            {
                if (image.sprite.name == "Icon_JiNengWeiJieSuo")
                {
                    image.material = null;
                    continue;
                }
            }
            image.material = state ? ProssData.Instance.grey : null;
        }

    }
    public static void SetAllGreyNoBg(this Transform tran, bool state = true, string path = "", bool outLineGrey = true)
    {
        var tranPath = path == "" ? tran : tran.Find(path);

        foreach (var image in tranPath.GetComponentsInChildren<Image>())
        {
            var tranName = image.name;
            if (tranName == "guidehand" || tranName == "guidedesc" || tranName == "guidearrow" || tranName == "bg")
            {
                continue;
            }
            image.material = state ? ProssData.Instance.grey : null;
        }
        foreach (var test in tranPath.GetComponentsInChildren<NewText>())
        {
            var tranName = test.name;
            if (tranName == "guidehand" || tranName == "guidedesc" || tranName == "guidearrow")
            {
                continue;
            }
            if (state)
            {
                test.isGrey = true;
                //test.oldColor = test.color;
                test.color = new Color(56 / 255f, 56 / 255f, 56 / 255f, 1f);
            }
            else
            {
                if (test.isGrey)
                    test.color = test.oldColor;
                test.isGrey = false;
            }
        }
        foreach (var GradientAuto in tranPath.GetComponentsInChildren<GradientAuto>())
        {
            var tranName = GradientAuto.name;
            if (tranName == "guidehand" || tranName == "guidedesc" || tranName == "guidearrow")
            {
                continue;
            }
            GradientAuto.enabled = !state;
        }
        if (outLineGrey)
        {

            foreach (var outLine in tranPath.GetComponentsInChildren<Outline>())
            {
                var tranName = outLine.name;
                if (tranName == "guidehand" || tranName == "guidedesc" || tranName == "guidearrow")
                {
                    continue;
                }
                outLine.enabled = !state;
            }
        }
    }

    public static void SetImageTransparent(this Transform tran, bool state = true, string path = "")
    {
        Color imageColorT = new Color();
        Color imageColorDefault = new Color(1, 1, 1, 1);
        imageColorT.a = 0;
        var tranPath = path == "" ? tran : tran.Find(path);

        foreach (var image in tranPath.GetComponentsInChildren<Image>())
        {
            image.color = state ? imageColorT : imageColorDefault;
        }
    }
    public static void SetImageOneTransparent(this Transform tran, bool state = true, string path = "", float a = 0)
    {
        Color imageColorT = Color.white;
        Color imageColorDefault = new Color(1, 1, 1, 1);
        imageColorT.a = a;
        var tranPath = path == "" ? tran : tran.Find(path);

        tranPath.GetComponent<Image>().color = state ? imageColorT : imageColorDefault;
    }
    public static void SetOneChildActive(this Transform item, int num)
    {
        for (int i = 0; i < item.childCount; i++)
        {
            item.GetChild(i).gameObject.SetActive(i == num);
        }
    }
    //盖章效果
    public static void ChopEffect(this Transform item, float maxScan = 10, float targetScan = 1f, float speed = 1f, System.Action callback = null)
    {
        if (item == null)
        {
            return;
        }
        item.SetActive(true);
        Image image = item.GetComponent<Image>();
        //float colorspeed = 1.0f * speed / (maxScan - targetScan);
        float sumcolor = 0;
        item.localScale = Vector3.one * maxScan;
        MonoTool.Instance.StartCor(
            () =>
            {
                if (!item)
                    return false;
                // sumcolor += colorspeed;
                //image.color = new Color(1, 1, 1, sumcolor);
                //speed *= 0.9f;
                maxScan -= speed;
                item.localScale = Vector3.one * maxScan;
                return maxScan > targetScan;
            },
            () =>
            {
                if (!item)
                    return;
                item.localScale = Vector3.one * targetScan;
                if (callback != null)
                    callback();
            });
    }
    public static void ChopEffect1(this GameObject go)
    {
        Debug.Log(go);
    }
    public static void MoveToAward(this Transform item, Vector3 tarPos, float speed = 0.01f, System.Action<Transform> callback = null)
    {
        MonoTool.Instance.MoveToward(item, tarPos, speed, callback);
    }
    public static void MoveToAwardEndTran(this Transform item, Transform target, float speed = 0.01f, System.Action<Transform> callback = null)
    {
        MonoTool.Instance.StartCor(MonoTool.Instance.MoveTowardTranIE(item, target, speed, callback));
    }
    public static void MoveToAwardRt(this RectTransform item, Vector3 tarPos, float speed = 0.01f, System.Action callback = null)
    {
        MonoTool.Instance.MoveTowardRt(item, tarPos, speed, callback);
    }
    public static Coroutine MoveToAwardLocal(this Transform item, Vector3 tarPos, float speed = 0.01f, System.Action callback = null)
    {
        return MonoTool.Instance.MoveTowardLocal(item, tarPos, speed, callback);
    }
    public static void LookAtTran(this Transform item, Transform target, float speed = 0.01f, System.Action callback = null)
    {
        MonoTool.Instance.LookTran(item, target, speed, callback);
    }
    //消失
    public static void MoveAndDis(this Transform item, Vector3 tarPos, float speed = 0.01f, System.Action callback = null)
    {
        float scan = item.localScale.x;
        Vector3 speedPos = (tarPos - item.position) / (scan / speed);
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               scan -= speed;
               item.localScale = Vector3.one * scan;
               item.position += speedPos;
               return scan > 0;
           },
           () =>
           {
               item.localScale = Vector3.zero;
               if (callback != null)
                   callback();
           });
    }
    public static void MoveAndDisToTargetScan(this Transform item, Vector3 tarPos, float speed = 0.01f, float targetScan = 0, System.Action callback = null)
    {
        float scan = item.localScale.x;
        Vector3 speedPos = (tarPos - item.position) / ((scan - targetScan) / speed);
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               scan -= speed;
               item.localScale = Vector3.one * scan;
               item.position += speedPos;
               return scan > targetScan;
           },
           () =>
           {
               item.localScale = Vector3.zero;
               if (callback != null)
                   callback();
           });
    }

    //消失
    public static void DisSomeOne(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        float scan = item.localScale.x;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               scan -= speed;
               item.localScale = Vector3.one * scan;
               return scan > 0;
           },
           () =>
           {
               item.localScale = Vector3.zero;
               if (callback != null)
                   callback();
           });
    }
    private static Dictionary<int, bool> YiedFlags = new Dictionary<int, bool>();
    public static void StopCoroutine(this Transform item)
    {
        YiedFlags[item.GetHashCode()] = true;
    }
    public static void DisSomeOneByAlpha(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 1;
        float sum = 1;
        int hascode = item.GetHashCode();
        if (!YiedFlags.ContainsKey(hascode))
            YiedFlags.Add(hascode, false);
        YiedFlags[hascode] = false;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (YiedFlags[hascode])
               {
                   return false;
               }
               if (canvas == null)
               {
                   return false;
               }
               sum -= speed;
               canvas.alpha = sum;
               return sum > 0;
           },
           () =>
           {
               if (canvas)
               {
                   if (YiedFlags[hascode])
                   {
                       YiedFlags.Remove(hascode);
                       return;
                   }
                   canvas.alpha = 0;
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }
    //渐变放大出现
    public static void BigAndShowNoAlpha(this Transform item, float targeScan = 1, float speed = 0.01f, System.Action callback = null)
    {
        float scan = 0;
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        float aSpeed = speed / targeScan;
        canvas.alpha = 0;
        item.localScale = Vector3.zero;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
               {
                   return false;
               }
               scan += speed;
               item.localScale = Vector3.one * scan;
               canvas.alpha += aSpeed;
               ///Debug.Log(scan + ":" + targeScan);
               return scan < targeScan;
           },
           () =>
           {
               if (!item)
                   return;
               item.localScale = Vector3.one * targeScan;
               canvas.alpha = 1;
               if (callback != null)
                   callback();
           });
    }
    public static void BigAndShow(this Transform item, float beginScan, float targeScan = 1, float speed = 0.01f, System.Action callback = null)
    {
        float scan = beginScan;
        float aSpeed = speed / targeScan;
        item.localScale = new Vector3(beginScan, beginScan, beginScan);
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
               {
                   return false;
               }
               scan += speed;
               item.localScale = Vector3.one * scan;
               //Debug.Log(scan + ":" + targeScan);
               return scan > targeScan;
           },
           () =>
           {
               if (!item)
                   return;
               item.localScale = Vector3.one * targeScan;
               if (callback != null)
                   callback();
           });
    }
    public static float tipBein = 0.8f;
    public static float tipEnd = 1.05f;
    public static float tipspeed = 0.03f;
    public static void TipBigAndSmallShow(this Transform item, System.Action callback = null)
    {
        float scan = tipBein;
        float speed = tipspeed;
        item.localScale = new Vector3(tipBein, tipBein, tipBein);
        bool isAdd = true;
        CanvasGroup tipMask = null;
        if (UIManager.TipCount == 0) {
            tipMask = UIManager.TipMask.transform.GetOrAddComponent<CanvasGroup>();
            tipMask.alpha = 0;
        }
        var achange = speed / (1 - tipBein);
        var itemMask = item.GetOrAddComponent<CanvasGroup>();
        itemMask.alpha = 0;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
               {
                   return false;
               }
               if (isAdd)
               {
                   if (tipMask)
                   {
                       tipMask.alpha += achange;
                   }
                   itemMask.alpha += achange;
                   speed *= 0.99f;
                   scan += speed;
                   item.localScale = Vector3.one * scan;
                   if (scan > tipEnd) {
                       isAdd = false;
                   }
                   return true;
               }
               else {
                   //speed *= 1.01f;
                   scan -= tipspeed;
                   item.localScale = Vector3.one * scan;
                   return scan > 1;
               }
           },
           () =>
           {
               if (!item)
                   return;
               item.localScale = Vector3.one;
               if (callback != null)
                   callback();
           });
    }
    public static void TipBigAndSmallShowClose(this Transform item, System.Action callback = null)
    {
        float scan = 1;
        float speed = tipspeed;
        item.localScale = new Vector3(1, 1, 1);
        bool isAdd = true;
        var achange = speed/ (1 - tipBein);
        var can = item.GetOrAddComponent<CanvasGroup>();
        CanvasGroup tipMask = null;
        if (UIManager.TipCount == 0) {
            UIManager.TipMask.SetActive(false);
            var go = GameObject.Instantiate(UIManager.TipMask).transform;
            go.SetParentOverride(UIManager.TipMask.transform.parent);
            go.GetComponent<RectTransform>().sizeDelta = UIManager.TipMask.GetComponent<RectTransform>().sizeDelta;
            tipMask = go.GetOrAddComponent<CanvasGroup>();
            go.localScale = Vector3.one;
            go.SetActive(true);
        }
       
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
               {
                   return false;
               }
               if (isAdd)
               {
                   speed *= 0.99f;
                   scan += speed;
                   item.localScale = Vector3.one * scan;
                   if (scan > tipEnd ){
                       isAdd = false;
                   }
                   return true;
               }
               else
               {
                   speed *= 1.02f;
                   scan -= speed;
                   can.alpha -= achange;
                   if (tipMask) {
                       tipMask.alpha = can.alpha;
                   }
                  // Debug.LogError(can.alpha);
                   item.localScale = Vector3.one * scan;
                   return scan > tipBein;
               }
           },
           () =>
           {
               if (!item)
                   return;
               item.localScale = Vector3.one;
               if (tipMask != null) {
                   GameObject.Destroy(tipMask.gameObject);
               }
               if (callback != null)
                   callback();
           });
    }
    public static void AlphaShow(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        float aSpeed = speed;
        canvas.alpha = 0;
        float sumAlpha = 0;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
               {
                   return false;
               }
               sumAlpha += aSpeed;
               canvas.alpha = sumAlpha;
               return sumAlpha > 1;
           },
           () =>
           {
               if (!item)
                   return;
               canvas.alpha = 1;
               if (callback != null)
                   callback();
           });
    }
    public static void ShowByAlphBig(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 0;
        item.gameObject.SetActive(true);
        float sum = 0;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               sum += speed;
               canvas.alpha = sum;
               Debug.Log(canvas.alpha);
               return sum > 1;
           },
           () =>
           {
               if (canvas)
               {
                   canvas.alpha = 1;
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }
    public static Coroutine ChangeAlpha(this Transform item, bool isAdd, float speed = 0.01f, System.Action<float> runingCall = null, System.Action callback = null)
    {
        if (!item) return null;
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        float begin = 0;
        if (!isAdd)
        {
            begin = 1;
        }
        canvas.alpha = begin;
        //if (isAdd)
        //{
        //    Debug.LogError("AddFadeInt");
        //}
        //else {
        //    Debug.LogError("AddFadeOut");
        //}
        return MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               if (isAdd)
               {
                   begin += speed;
               }
               else
               {
                   begin -= speed;
               }
               canvas.alpha = begin;
               //Debug.LogError(begin);
               if (runingCall != null)
               {
                   runingCall(begin);
               }
               if (isAdd)
               {
                   return begin < 1;
               }
               else
               {
                   return begin > 0;
               }
           },
           () =>
           {
               if (canvas)
               {
                   if (isAdd)
                   {
                       canvas.alpha = 1;
                   }
                   else
                   {
                       canvas.alpha = 0;
                   }
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }

    public static Coroutine DisAlph(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 1;
        item.gameObject.SetActive(true);
        float sum = 1;
        //Debug.Log(item.name);
        return MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               sum -= speed;
               canvas.alpha = sum;
               //Debug.Log(canvas.alpha);
               return sum > 0;
           },
           () =>
           {
               if (canvas)
               {
                   //Debug.Log(item.name);
                   canvas.alpha = 0;
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }
    /// <summary>
    /// 渐变隐藏
    /// </summary>
    /// <param name="item"></param>
    /// <param name="speed"></param>
    /// <param name="callback"></param>
    public static void HideByAlpha11(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 1;
        item.gameObject.SetActive(true);
        float sum = 1;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               sum -= speed;
               canvas.alpha = sum;

               //Debug.Log(canvas.alpha);
               return sum > 0
               ;
           },
           () =>
           {
               if (canvas)
               {
                   canvas.alpha = 1;
                   item.SetActive(false);
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }

    public static void ShowByAlphToTarget(this RectTransform item, Vector2 targetPos, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 0;
        item.gameObject.SetActive(true);
        float sum = 0;
        Vector2 speedpos = new Vector2(targetPos.x - item.anchoredPosition.x, targetPos.y - item.anchoredPosition.y) * speed;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               sum += speed;
               canvas.alpha = sum;
               item.anchoredPosition += speedpos;


               return sum < 1;
           },
           () =>
           {
               if (canvas)
               {
                   item.anchoredPosition = targetPos;
                   canvas.alpha = 1;
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }

    public static void LoopAlph(this Image image, float rangsmall = 0.5f, float speed = 0.01f)
    {
        float sum = 1;
        bool cut = true;

        MonoTool.Instance.StartCor(
           () =>
           {
               if (!image)
                   return false;
               if (cut)
               {
                   //speed *= 1.04f;
                   sum -= speed;
                   image.color -= new Color(0, 0, 0, speed);
               }
               else
               {
                   //speed *= 0.96f;
                   sum += speed;
                   image.color += new Color(0, 0, 0, speed);
               }
               if (sum >= 1)
               {
                   cut = true;
               }
               if (sum < rangsmall)
               {
                   cut = false;
               }
               return true;
           },
           () =>
           {

           });
    }
    public static void ShowByAlphaSmall(this Transform item, float speed = 0.01f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = 1;
        float sum = 1;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (canvas == null)
               {
                   return false;
               }
               sum -= speed;
               canvas.alpha = sum;

               //Debug.Log(canvas.alpha);
               return sum < 0;
           },
           () =>
           {
               if (canvas)
               {
                   canvas.alpha = 0;
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }
    
    public static void ShowByAlphaRange(this Transform item, float start, float end, float total_second, float update_time = 0.1f, System.Action callback = null)
    {
        var canvas = item.GetComponent<CanvasGroup>();
        if (!canvas)
        {
            canvas = item.gameObject.AddComponent<CanvasGroup>();
        }
        canvas.alpha = start;
        float speed = (start - end) * -1 / total_second;
        float accumulate_time = 0;
        MonoTool.Instance.StartCor(update_time, () =>
        {
            if (!item || !canvas || MathF.Abs(canvas.alpha) > MathF.Abs(end) || accumulate_time >= total_second)
            {
                callback?.Invoke();
                return false;
            }

            accumulate_time += update_time;
            return true;
        }, () =>
        {
            if (!canvas)
            {
                return;
            }
            canvas.alpha += speed;
        });
    }
    
    //图片变暗变亮
    public static void ChangeImageBlackToWhite(this Image image, float speed = 0.01f, System.Action callback = null)
    {
        image.ChangeImageBlack(speed, () =>
        {
            MonoTool.Instance.Wait(0.3f, () =>
            {
                image.ChangeImageWhite(speed, callback);
            });
        });
    }
    //图片变暗
    public static void ChangeImageBlack(this Image image, float speed = 0.01f, System.Action callback = null)
    {
        float sum = 1;
        float a = 1;
        //Debug.Log(image.transform.parent);
        MonoTool.Instance.StartCor(
          () =>
          {
              if (!image)
                  return false;

              sum -= speed;
              a -= speed / 2;
              image.color = new Color(sum, sum, sum, 1);
              //Debug.Log(sum);
              return sum > 0.6f;
          },
          () =>
          {
              if (callback != null)
              {
                  callback();
              }
          });
    }
    //图片变暗
    public static void ChangeImageBlack(this Graphic image, float speed = 0.01f, System.Action callback = null)
    {
        float sum = 1;
        float a = 1;
        //Debug.Log(image.transform.parent);
        MonoTool.Instance.StartCor(
          () =>
          {
              if (!image)
                  return false;

              sum -= speed;
              a -= speed / 2;
              image.color = new Color(sum, sum, sum, 1);
              //Debug.Log(sum);
              return sum > 0.6f;
          },
          () =>
          {
              if (callback != null)
              {
                  callback();
              }
          });
    }
    //图片变暗
    public static void ChangeImageWhite(this Image image, float speed = 0.01f, System.Action callback = null)
    {
        float sum = image.color.r;
        float a = image.color.a;
        float aspeed = speed / (sum / a);
        //Debug.Log(image.transform.parent);
        MonoTool.Instance.StartCor(
          () =>
          {
              if (!image)
                  return false;

              sum += speed;
              a += aspeed;
              image.color = new Color(sum, sum, sum, 1);
              //Debug.Log(sum);
              return sum < 1f;
          },
          () =>
          {
              if (!image)
                  return;
              image.color = Color.white;
              if (callback != null)
              {
                  callback();
              }
          });
    }
    //渐隐
    public static void HideByAlpha(this Transform item, float smallspeed = 0.01f, float bigSpeed = 0.01f, float wait = 0.02f, System.Action callback = null)
    {
        item.ShowByAlphBig(smallspeed, () =>
        {

        });
    }

    //渐隐渐现
    public static void ShowByAlphaOnce(this Transform item, float smallspeed = 0.01f, float bigSpeed = 0.01f, float wait = 0.02f, System.Action callback = null)
    {
        item.ShowByAlphBig(smallspeed, () =>
        {
            MonoTool.Instance.Wait(wait, () =>
            {
                item.ShowByAlphaSmall(bigSpeed, callback);
            });
        });
    }
    //渐隐渐现
    public static void ShowByAlphaOnceBig(this Transform item, float smallspeed = 0.04f, float bigSpeed = 0.04f, float wait = 0.02f, System.Action callback = null)
    {
        item.ShowByAlphBig(smallspeed, () =>
        {
            MonoTool.Instance.Wait(wait, () =>
            {
                item.ShowByAlphaSmall(bigSpeed, () =>
                {
                    item.ShowByAlphBig(smallspeed, callback);
                });
            });
        });
    }
    public static void ChaneGauss(this Transform item, float target = 8, float speed = 0.1f, System.Action callback = null)
    {
        var m = item.GetComponent<Image>().material;

        float sum = 0;
        MonoTool.Instance.StartCor(
           () =>
           {
               if (!item)
                   return false;
               if (m == null)
               {
                   return false;
               }
               sum += speed;
               m.SetFloat("_BlurSize", sum);
               return sum < target;
           },
           () =>
           {
               if (m)
               {
                   if (callback != null)
                   {
                       callback();
                   }
               }
           });
    }
    public static void SetUninoFlag(this Transform obj, string str)
    {
        if (string.IsNullOrEmpty(str))
        {
            return;
        }
        //"2,4,1,1#0.8433384#0#0,2#0#0.6985157#0,3#0.3618702#0.1956486#0,1"
        var strs = str.Split(',');
        int kind = 1;
        if (strs.Length > 6)
        {
            kind = int.Parse(strs[6]);
        }
        obj.SetImage("unionflagBg/0" + strs[0]);
        var data1 = strs[3].Split('#');
        SetColor(obj, float.Parse(data1[1]), float.Parse(data1[2]), float.Parse(data1[3]), kind);
        //
        obj.Find("faceicon").SetImage("unionflag/" + strs[1]);
        var data2 = strs[4].Split('#');
        SetColor(obj.Find("faceicon"), float.Parse(data2[1]), float.Parse(data2[2]), float.Parse(data2[3]), kind);
        //
        if (int.Parse(strs[2]) < 10)
        {
            obj.Find("flagicon").SetImage("unionflagIcon/00" + strs[2]);
        }
        else
        {
            obj.Find("flagicon").SetImage("unionflagIcon/0" + strs[2]);
        }
        var data3 = strs[5].Split('#');
        SetColor(obj.Find("flagicon"), float.Parse(data3[1]), float.Parse(data3[2]), float.Parse(data3[3]), kind);
    }
    static void SetColor(Transform tra, float n1, float n2, float n3, int kinds)
    {
        if (kinds == 1)
        {
            tra.GetComponent<NewImage>().color = Color.HSVToRGB(n1, n2, n3);
        }
        else
        {
            tra.GetComponent<NewImage>().color = new Color(n1 / 360f, n2 / 360f, n3 / 360f);
        }
    }
    static void SetColor2(Transform tra, float n1, float n2, float n3, int kinds)
    {
        if (kinds == 1)
        {
            tra.GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(n1, n2, n3);
        }
        else
        {
            tra.GetComponent<MeshRenderer>().material.color = new Color(n1 / 360f, n2 / 360f, n3 / 360f);
        }
    }
    //public static GameObject PlayerEffect(this Transform item, string name, float distanceTime = 0.14f, float scan = 1, bool isLoop = false, System.Action callback = null)
    //{
    //    return GameTools.PlayEffect(name, item, distanceTime, scan, isLoop, callback);
    //}
    public static void DisEffect(this Transform item, string name)
    {
        Transform tran = item.Find(item.name + name);
        if (tran)
        {
            GameObject.Destroy(tran.gameObject);
        }
    }
    public static void MoveSlider(this Image image, double value, float moveTimes, System.Action<string> exp, System.Action callback = null)
    {
        double speed = (image.fillAmount - value) / moveTimes;
        double sum = image.fillAmount;
        MonoTool.Instance.StartCor(() =>
        {
            if (!image)
                return false;
            sum -= speed;
            exp(sum.ToPTC());
            image.fillAmount = (float)sum;
            return sum > value;
        },
        () =>
        {
            image.fillAmount = (float)value;
            exp(value.ToPTC());
            if (callback != null)
                callback();
        });
    }
    public static void MoveSliderWorld(this Image image, double value, float moveTimes, System.Action<double> exp, System.Action callback = null)
    {
        double speed = (image.fillAmount - value) / moveTimes;
        double sum = image.fillAmount;
        MonoTool.Instance.StartCor(() =>
        {
            if (!image)
                return false;
            sum -= speed;
            exp(sum);
            image.fillAmount = (float)sum;
            return sum > value;
        },
        () =>
        {
            image.fillAmount = (float)value;
            exp(value);
            if (callback != null)
                callback();
        });
    }
    //推进
    public static void MoveLoop(this Transform item, Vector2 dir, int times)
    {
        int index = -1;
        int sum = 0;
        int max = times + 10;
        Vector2 oldPos = item.position;
        MonoTool.Instance.StartCor(
          () =>
          {
              if (!item)
                  return false;
              index++;
              if (index >= max)
              {
                  item.position = oldPos;
                  index = 0;
              }
              if (index < times)
                  item.Translate(dir);
              return item;
          }, null);
    }

    private static bool isAlwaysUp = false;



    //设置子物体某个显示
    public static void ChildShowOne(this Transform grid, int index)
    {
        int len = grid.childCount;
        if (index >= grid.childCount)
        {

            return;
        }
        for (int i = 0; i < len; i++)
        {
            grid.GetChild(i).SetActive(i == index);
        }
    }
    private static string[] elides = new string[] {
        "",
        ".",
        "..",
        "...",
    };
    //省略号文字
    public static void ElideText(this Text text, string str, float waitTime = 0.25f)
    {
        int index = 0;
        MonoTool.Instance.StartCor(waitTime, () =>
        {
            if (!text)
                return false;
            //Debug.Log(index);
            if (index > 3)
            {
                index = 0;
            }
            text.text = str + elides[index];
            index++;
            return true;
        },
       () =>
       {

       });
    }
    //省略号
    public static void ElideEffect(this Transform father, float speed = 0.3f)
    {
        Transform[] childs = new Transform[father.childCount];
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = father.GetChild(i);
            childs[i].SetActive(false);
        }
        int index = 0;
        MonoTool.Instance.StartCor(speed, () =>
        {
            if (!father)
                return false;
            index++;
            //Debug.Log(index);
            if (index >= childs.Length)
            {
                index = -1;
                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i].SetActive(false);
                }
            }
            else
            {
                childs[index].SetActive(true);
            }
            return true;
        },
       () =>
       {

       });
    }
    private static bool isHorseLamping1;
    private static string cityMessage;
    //跑马灯效果(主界面)
    public static void HorseLampCity(this Text text, string str, float speed = 2, bool isLoop = false, System.Action callback = null)
    {
        text.text = str;
        if (!isHorseLamping1)
        {
            text.ShowOneHorseLampCity(speed, isLoop, callback);
        }

    }
    private static void ShowOneHorseLampCity(this Text text, float speed = 2, bool isLoop = false, System.Action callback = null)
    {
        isHorseLamping1 = true;
        MonoTool.Instance.WaitEndFrame(() =>
        {
            RectTransform rectTran = text.rectTransform;
            Vector3[] poses = new Vector3[4];
            rectTran.GetLocalCorners(poses);
            float width = poses[2].x - poses[0].x;
            float posx = -width;
            Vector2 oldPos = rectTran.anchoredPosition;
            Vector2 change = new Vector2(speed, 0);
            text.transform.parent.SetActive(true);

            MonoTool.Instance.StartCor(
              () =>
              {
                  if (text == null)
                  {
                      isHorseLamping1 = false;

                      return false;
                  }
                  rectTran.anchoredPosition -= change;
                  return rectTran.anchoredPosition.x >= posx;
              },
              () =>
              {
                  if (text == null)
                  {

                      isHorseLamping1 = false;
                      return;
                  }
                  isHorseLamping1 = false;
                  text.transform.parent.SetActive(false);
                  rectTran.anchoredPosition = oldPos;
                  if (isLoop)
                  {
                      text.ShowOneHorseLampCity(speed, isLoop, callback);
                  }
                  else
                  {
                      if (callback != null)
                          callback();
                  }
              }
          );
        });
    }
    private static Vector2 oldHorseUpSize = Vector2.zero;
    private static float oldy;
    public static void SetChildActive(this Transform tran, bool active)
    {
        for (int i = 0; i < tran.childCount; i++)
        {
            tran.GetChild(i).SetActive(active);
        }
    }
    public static void GetAllChild(this Transform tran, System.Action<Transform> callback)
    {
        for (int i = 0; i < tran.childCount; i++)
        {
            callback(tran.GetChild(i));
        }
    }
    public static void CreateLink(this Text text, string str)
    {
        if (text == null)
            return;

        //克隆Text，获得相同的属性
        Text underline = GameObject.Instantiate(text) as Text;
        underline.name = "Underline";

        underline.transform.SetParentOverride(text.transform);
        RectTransform rt = underline.rectTransform;

        //设置下划线坐标和位置
        rt.anchoredPosition3D = Vector3.zero;
        rt.offsetMax = Vector2.zero;
        rt.offsetMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.anchorMin = Vector2.zero;
        string oldstr = underline.text.RemoveColor();
        oldstr = oldstr.Replace(str, "</color>" + str + "<color=#00000000>");
        underline.text = "";

        string strline = "";
        for (int i = 0; i < str.Length * 2; i++)
        {
            strline += "_";
        }

        Debug.Log(strline);
        oldstr = oldstr.Replace(str, strline);
        underline.text = "<color=#00000000>" + oldstr + "</color>";
    }
    /// <summary>
    /// Autos the increase number.
    /// </summary>
    /// <param name="text">Text.</param>
    /// <param name="starString">前缀</param>
    /// <param name="increaseNum">递增的目标数字</param>
    public static void AutoIncreaseNum(this Text text, string prefixString, int increaseNum)
    {
    }
    public static void TestTran(this Transform tran)
    {
        Debug.Log(tran);
    }
    public static void TestTran(this GameObject tran)
    {
        Debug.Log(tran);
    }
    public static void TwoChopEffect(this GameObject tran)
    {
        Debug.Log(tran);
    }
    public static void Clear()
    {
        MonoTool.Instance.Clear();
        oldHorseUpSize = Vector2.zero;
        oldy = 0;
    }
    public static Vector4 GetRange(this RectTransform rectTran)
    {
        Vector3[] bgconners = new Vector3[4];//ScrollRect四角的世界坐标
        rectTran.GetLocalCorners(bgconners);
        return new Vector4(bgconners[0].x, bgconners[2].x, bgconners[0].y, bgconners[2].y);
    }
    public static Vector4 GetRangeWorld(this RectTransform rectTran)
    {
        Vector3[] bgconners = new Vector3[4];//ScrollRect四角的世界坐标
        rectTran.GetWorldCorners(bgconners);
        return new Vector4(bgconners[0].x, bgconners[2].x, bgconners[0].y, bgconners[2].y);
    }
    public static Vector4 GetRange(this Transform transform)
    {
        return transform.GetComponent<RectTransform>().GetRange();
    }
    public static Vector4 GetRangeWorld(this Transform transform)
    {
        return transform.GetComponent<RectTransform>().GetRangeWorld();
    }
    public static Transform AddOneItem(this Transform father, Transform item)
    {
        GameObject go = GameObject.Instantiate(item.gameObject, Vector3.zero, Quaternion.Euler(Vector3.zero), father);
        return go.transform;
    }
    public static bool CheckPosIsOutRange(this Vector2 pos, Vector4 rectRange)
    {
        return pos.x < rectRange.x || pos.x > rectRange.y || pos.y < rectRange.z || pos.y > rectRange.w;
    }
    public static bool CheckPosIsOutRangeTopAndLeft(this Vector2 pos, Vector4 rectRange)
    {
        return pos.x < rectRange.x || pos.y > rectRange.z;
    }
    public static bool CheckIsOut(this RectTransform rt, Vector4 rect)
    {
        var ourRect = rt.GetRangeWorld();
        return ourRect.x < rect.x || ourRect.y > rect.y || ourRect.z < rect.z || ourRect.w > rect.w;
    }
    public static bool CheckIsOut(this RectTransform rt, Transform tran)
    {
        Vector4 rect = tran.GetRangeWorld();
        return CheckIsOut(rt, rect);
    }
    public static bool CheckAllIsOut(this RectTransform rt, Vector4 rect)
    {
        var ourRect = rt.GetRangeWorld();
        return ourRect.x > rect.y || ourRect.y < rect.x || ourRect.z > rect.w || ourRect.w < rect.z;
    }
    public static bool CheckAllIsOut(this Transform rt, Transform target)
    {
        Vector4 rect = target.GetRangeWorld();
        var ourRect = rt.GetRangeWorld();
        return ourRect.x > rect.y || ourRect.y < rect.x || ourRect.z > rect.w || ourRect.w < rect.z;
    }
    public static bool CheckIsHaveIn(this Transform rt, Transform target)
    {
        return !rt.CheckAllIsOut(target);
    }
    public static bool CheckIsOut(this Vector2 pos, Vector4 rect)
    {
        return pos.x < rect.x || pos.x > rect.y || pos.y < rect.z || pos.y > rect.w;
    }
    public static bool CheckIsOutY(this Vector2 pos, Vector4 rect)
    {
        return pos.y < rect.z || pos.y > rect.w;
    }
    public static bool CheckIsInX(this Vector2 pos, Vector4 rect)
    {
        return pos.x > rect.x && pos.x < rect.y;
    }
    public static bool CheckIsInY(this Vector2 pos, Vector4 rect)
    {
        return !pos.CheckIsOutY(rect);
    }
    public static bool CheckIsIn(this Vector2 pos, Vector4 rect)
    {
        return !pos.CheckIsOut(rect);
    }
    public static void StepHud(this Transform hud, Transform father, Vector2 modify)
    {
        hud.StepHud(father.position, modify);
    }
    public static void StepHud(this Transform hud, Vector3 position, Vector2 modify)
    {
        if (hud == null)
            return;
        Vector2 itemPos = Camera.main.WorldToScreenPoint(position);
        hud.position = itemPos - modify;
    }
    public static void ClearChildNoHaveOther(this Transform father)
    {
        if (father == null)
        {
            return;
        }
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < father.childCount; i++)
        {
            childs.Add(father.GetChild(i));
        }
        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i].name != "other")
                GameObject.DestroyImmediate(childs[i].gameObject);
        }
    }
    public static void ClearChild(this Transform father)
    {
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < father.childCount; i++)
        {
            childs.Add(father.GetChild(i));
        }
        for (int i = 0; i < childs.Count; i++)
        {
            GameObject.DestroyImmediate(childs[i].gameObject);
        }
    }
    public static void ClearChildLeftOne(this Transform father, int num = 1)
    {
        if (father == null)
        {
            return;
        }
        List<Transform> childs = new List<Transform>();
        for (int i = 0; i < father.childCount; i++)
        {
            childs.Add(father.GetChild(i));
        }
        for (int i = num; i < childs.Count; i++)
        {
            GameObject.DestroyImmediate(childs[i].gameObject);
        }
        for (int i = 0; i < num; i++)
        {
            father.GetChild(i).SetActive(false);
        }
     
    }
    public static void LoopChild(this Transform target, System.Action<Transform, string> callback, string path = "")
    {
        callback(target, path);
        for (int i = 0; i < target.childCount; i++)
        {
            var child = target.GetChild(i);
            var newPath = path + "/" + child.name;
            if (newPath.Substring(0, 1) == "/")
            {
                newPath = newPath.Substring(1);
            }
            child.LoopChild(callback, newPath);
        }
    }
    public static Transform CopyOneChild(this Transform grid)
    {
        if (grid.childCount == 0)
        {
            return null;
        }
        var one = grid.GetChild(0);
        return GameObject.Instantiate(one.gameObject, grid).transform;
    }
    public static Transform CopyOne(this Transform item)
    {
        if (item.parent == null)
        {
            return null;
        }
        return GameObject.Instantiate(item.gameObject, item.parent).transform;
    }
    public static Transform CopyToTargetOne(this Transform item, Transform target)
    {
        if (item.parent == null)
        {
            return null;
        }
        var one = GameObject.Instantiate(item.gameObject, target).transform;
        one.position = item.position;
        one.RemoveComponent<EventTriggerListener>();
        one.SetDepthTop();
        return one;
    }
    public static void SetLocalPositionX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localPosition;
        v.x = newValue;
        transform.localPosition = v;
    }
    public static void SetLocalScaleX(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.x = newValue;
        transform.localScale = v;
    }
    public static void SetLocalScaleY(this Transform transform, float newValue)
    {
        Vector3 v = transform.localScale;
        v.y = newValue;
        transform.localScale = v;
    }
    public static void SetParentOverride(this Transform child, Transform father, bool setScale = false)
    {
        Vector3 oldScan = child.localScale;
        child.SetParent(father);
        child.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        if (!setScale)
        {
            child.localScale = oldScan;
        }
    }
    public static void SetParentOverrideNoScale(this Transform child, Transform father)
    {
        SetParentOverride(child, father, true);
    }
    public static void SetParentOverride(this Transform child, Transform father, Vector3 pos)
    {
        child.SetParent(father);
        child.localPosition = pos;
    }
    public static void SetParentOverrideWorld(this Transform child, Transform father, Vector3 pos)
    {
        child.SetParent(father);
        child.position = pos;
    }
    public static Transform[] AddChildsUseOneSize(this Transform grid, int num, string baseUIName = "")
    {
        Vector3 size = Vector3.one;
        if (grid.childCount > 0)
            size = grid.GetChild(0).localScale;
        return grid.AddChilds(num, size, baseUIName);
    }
    public static Transform[] AddChilds(this Transform grid, int num, Vector2 size, string baseUIName = "")
    {
        var items = grid.AddChilds(num, baseUIName);
        foreach (var item in items)
        {
            item.localScale = size;
        }
        return items;
    }

    public static Transform[] AddChilds(this Transform grid, int num, string baseUIName = "")
    {
        float scale = 1;
        if (baseUIName != "")
        {
            if (grid.childCount == 1)
            {
                scale = grid.GetChild(0).localScale.x;
                GameObject.DestroyImmediate(grid.GetChild(0).gameObject);
            }
            AssetLoadOld.Instance.LoadPrefab("BaseUIPrefab/" + baseUIName, (go) =>
            {
                GameObject go1 = GameObject.Instantiate(go, grid) as GameObject;
                go1.transform.localScale = Vector3.one * scale;
            });
        }

        Transform[] items = new Transform[num];
        int len = grid.childCount;
        if (num == 0)
        {
            for (int i = 0; i < len; i++)
            {
                grid.GetChild(i).gameObject.SetActive(false);
            }
            return items;
        }
        if (grid.childCount >= num)
        {
            for (int i = 0; i < len; i++)
            {
                if (i < num)
                {
                    items[i] = grid.GetChild(i);
                    items[i].name = i.ToString();
                }
                grid.GetChild(i).gameObject.SetActive(i < num);
            }
            return items;
        }
        else
        {
            for (int i = 0; i < len; i++)
            {
                items[i] = grid.GetChild(i);
                grid.GetChild(i).gameObject.SetActive(true);
            }
        }

        items[0] = grid.GetChild(0);
        items[0].name = "0";
        items[0].gameObject.SetActive(true);
        for (int i = len; i < num; i++)
        {
            items[i] = GameObject.Instantiate(items[0], grid);
            items[i].localScale = Vector3.one * scale;
            //items[i].localScale = Vector3.one;
            items[i].gameObject.SetActive(true);
            items[i].name = i.ToString();
        }
        return items;
    }

    public static Transform[] AddChildsNoUseDefault(this Transform grid, int num, string baseUIName = "")
    {
        if (num == 0)
        {
            for (int i = 0; i < grid.childCount; i++)
            {
                grid.GetChild(i).gameObject.SetActive(false);
            }
            return new Transform[0];
        }
        var defaultItem = GameObject.Instantiate(grid.GetChild(0).gameObject);
        var items = grid.AddChilds(num, baseUIName);
        defaultItem.transform.SetParent(grid);
        defaultItem.transform.localPosition = Vector3.zero;
        defaultItem.transform.localScale = Vector3.one;
        defaultItem.transform.localRotation = Quaternion.Euler(Vector3.zero);
        defaultItem.transform.SetAsFirstSibling();
        defaultItem.SetActive(false);
        defaultItem.name = "default";
        return items;
    }
    public static Transform AddChildOne(this Transform grid)
    {
        var item = GameObject.Instantiate(grid.GetChild(0).gameObject).transform;
        item.SetParent(grid);
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.Euler(Vector3.zero);
        return item;
    }
    public static Transform AddChildOne(this Transform grid, string str)
    {
        var item = GameObject.Instantiate(grid.Find(str).gameObject).transform;
        item.SetParent(grid);
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.Euler(Vector3.zero);
        return item;
    }
    public static Transform AddChildOne(this Transform grid, GameObject one)
    {
        var item = GameObject.Instantiate(one).transform;
        item.SetParent(grid);
        item.localPosition = Vector3.zero;
        item.localScale = Vector3.one;
        item.localRotation = Quaternion.Euler(Vector3.zero);
        return item;
    }
    public static T[] CopyUnityObject<T>(this T t, int num) where T : Object
    {
        T[] ts = new T[num];
        for (int i = 0; i < num; i++)
        {
            if (i == 0)
            {
                ts[i] = t;
            }
            else
            {
                ts[i] = GameObject.Instantiate(t);
            }
        }
        return ts;
    }
    public static T GetOrAddComponent<T>(this Transform tran) where T : Behaviour
    {
        var t = tran.GetComponent<T>();
        if (t == null)
        {
            t = tran.gameObject.AddComponent<T>();
        }
        return t;
    }
    public static void RemoveComponent<T>(this Transform tran) where T : Behaviour
    {
        var t = tran.GetComponent<T>();
        if (t != null)
        {
            GameObject.DestroyImmediate(t);
        }
    }
    public static void ShowNpcTalk(this Transform spine, string str, Vector2 pos, int oneLineNum = 20, System.Action callback = null, bool isLevel = false, int fontSize = 21, float speed = 0.04f, float time = 1f, bool isUI = false, bool ShowAllWithClick = false)
    {
        //string prefabName = "Assets/Res/Prefab/HUD/talkmsc.prefab";
        //if (spine == null)
        //{
        //    Debug.LogError("Spine:Null");
        //}
        //AssetLoadOld.Instance.LoadTriggerPrefab(prefabName, (go) =>
        //{
        //    if (go == null)
        //    {
        //        Debug.LogError("找不到预制:" + prefabName);
        //        return;
        //    }
        //    GameObject StoryAndAINameGo = GameObject.Instantiate(go);
        //    StoryAndAINameGo.transform.parent = isUI ? spine : UIManager.SceneTalkParent;
        //    //Debug.LogError(isUI + "----" + str);
        //    StoryAndAINameGo.transform.GetComponent<Canvas>().sortingOrder = isUI ? 0 : -1;
        //    StoryAndAINameGo.transform.localScale = Vector3.one;
        //    TalkMsc talkMsc = new TalkMsc();
        //    talkMsc.go = StoryAndAINameGo;
        //    if (!isUI)
        //    {
        //        talkMsc.Follow(spine, pos);
        //    }
        //    else
        //    {
        //        StoryAndAINameGo.GetComponent<RectTransform>().anchoredPosition = pos;
        //    }
        //    var textComp = StoryAndAINameGo.transform.Find("text").GetComponent<Text>();
        //    //Debug.LogError(StoryAndAINameGo.name+",,,"+StoryAndAINameGo.transform.parent.name+",,,"+StoryAndAINameGo.transform.localPosition);
        //    textComp.fontSize = fontSize;
            //if (ShowAllWithClick) MainStoyrMgr.Instance.NowTalkMsc = talkMsc;
            //talkMsc.ShowTextResHaveBg(textComp, str, speed, oneLineNum, () =>
            //{
            //    if (callback != null)
            //        callback();
            //}, time, ShowAllWithClick);
        //});
    }
    public static void ShowImageTalk(this Transform spine, string str, Vector2 pos, string bgName, float waittime = 3, string iconName = "", string num = "", System.Action callback = null)
    {
        string prefabName = "Assets/Res/Prefab/HUD/imagetalkmsc.prefab";
        AssetLoadOld.Instance.LoadTriggerPrefab(prefabName, (go) =>
        {
            if (go == null)
            {
                Debug.LogError("找不到预制:" + prefabName);
                return;
            }
            GameObject talk = GameObject.Instantiate(go);
            talk.transform.SetParent(spine);
            talk.transform.localScale = Vector3.one;
            talk.GetComponent<RectTransform>().anchoredPosition = pos;
            talk.transform.SetImage("talkbg/" + bgName);
            if (bgName == "beflower")
            {
                talk.transform.Find("text").SetText(str);
                talk.transform.Find("num").SetText(num);
                talk.transform.GetComponent<HorizontalLayoutGroup>().padding.top = 36;
            }
            else
            {
                talk.transform.GetComponent<HorizontalLayoutGroup>().padding.top = 52;
            }
            talk.transform.Find("image").SetActive(bgName == "beflower");
            talk.transform.Find("num").SetActive(bgName == "beflower");
            if (bgName == "beskin")
            {
                talk.transform.Find("text").SetText(str);
            }
            MonoTool.Instance.Wait(waittime, () =>
             {
                 if (talk != null)
                 {
                     GameObject.Destroy(talk);
                     callback();
                 }
             });
        });
    }
    public static void ShowOurTalk(this Transform spine, string str, Vector2 pos, int oneLineNum = 20, System.Action callback = null)
    {
        string prefabName = "Assets/Res/Prefab/HUD/ourtalkmsc.prefab";
        AssetLoadOld.Instance.LoadTriggerPrefab(prefabName, (go) =>
        {
            if (go == null)
            {
                Debug.LogError("找不到预制:" + prefabName);
                return;
            }
            GameObject StoryAndAINameGo = GameObject.Instantiate(go);
            StoryAndAINameGo.transform.SetParent(UIManager.SceneTalkParent);
            StoryAndAINameGo.transform.localScale = Vector3.one;
            TalkMsc talkMsc = new TalkMsc();
            talkMsc.go = StoryAndAINameGo;
            talkMsc.Follow(spine, pos);
            talkMsc.ShowTextResHaveBg(StoryAndAINameGo.transform.Find("text").GetComponent<Text>(), str, 0.04f, oneLineNum, () =>
            {
                if (callback != null)
                    callback();
            });
        });
    }
    public static Vector2 SetBattleHUD(this Transform hud, Transform follow, Vector3 modify, bool isUI = false)
    {
        if (isUI)
        {
            hud.SetParentOverride(follow);
            if (follow == null)
            {
                return Vector2.zero;
            }
            Vector2 curPos = follow.position + modify;
            hud.GetComponent<RectTransform>().anchoredPosition3D += modify;
            return curPos;
        }
        if (UIManager.MainCamera == null || UIManager.UICamera == null)
        {
            Debug.LogError("相机未初始化!");
            return Vector2.zero;
        }
        if (follow == null)
        {
            return Vector2.zero;
        }
        Vector3 localPos = follow.localPosition + modify;
        Vector3 worldPos = follow.TransformPoint(localPos);
        Vector2 screenPoint = UIManager.MainCamera.WorldToScreenPoint(worldPos);
        Vector2 currPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Root.GetComponent<RectTransform>(), screenPoint, UIManager.UICamera, out currPos);
        hud.localPosition = currPos;
        return currPos;
    }
    public static Vector2 SetBattleHUDWorld(this Transform hud, Transform follow, Vector3 modify, bool isUI = false)
    {
        if (isUI)
        {
            hud.SetParentOverride(follow);
            if (follow == null)
            {
                return Vector2.zero;
            }
            Vector2 curPos = follow.position + modify;
            hud.GetComponent<RectTransform>().anchoredPosition3D += modify;
            return curPos;
        }
        if (UIManager.MainCamera == null || UIManager.UICamera == null)
        {
            Debug.LogError("相机未初始化!");
            return Vector2.zero;
        }
        if (follow == null)
        {
            return Vector2.zero;
        }
        Vector3 worldPos = follow.position + modify;
        Vector2 screenPoint = UIManager.MainCamera.WorldToScreenPoint(worldPos);
        Vector2 currPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(UIManager.Root.GetComponent<RectTransform>(), screenPoint, UIManager.UICamera, out currPos);
        hud.localPosition = currPos;
        return currPos;
    }
    public static Vector3 GetInspectorRotationValueMethod(this Transform transform)
    {
        // 获取原生值
        System.Type transformType = transform.GetType();
        System.Reflection.PropertyInfo m_propertyInfo_rotationOrder = transformType.GetProperty("rotationOrder", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object m_OldRotationOrder = m_propertyInfo_rotationOrder.GetValue(transform, null);
        System.Reflection.MethodInfo m_methodInfo_GetLocalEulerAngles = transformType.GetMethod("GetLocalEulerAngles", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        object value = m_methodInfo_GetLocalEulerAngles.Invoke(transform, new object[] { m_OldRotationOrder });
        //Debug.Log("反射调用GetLocalEulerAngles方法获得的值：" + value.ToString());
        string temp = value.ToString();
        //将字符串第一个和最后一个去掉
        temp = temp.Remove(0, 1);
        temp = temp.Remove(temp.Length - 1, 1);
        //用‘，’号分割
        string[] tempVector3;
        tempVector3 = temp.Split(',');
        //将分割好的数据传给Vector3
        Vector3 vector3 = new Vector3(float.Parse(tempVector3[0]), float.Parse(tempVector3[1]), float.Parse(tempVector3[2]));
        return vector3;
    }
    public static void SetDepth(this Transform item, int depth)
    {
        var uidepath = item.GetComponent<UIDepth>();
        if (!uidepath)
        {
            uidepath = item.gameObject.AddComponent<UIDepth>();
        }
        uidepath.SetDepth(depth);
    }
    public static void SetAllDepth(this Transform item, int depth)
    {
        SetDepth(item, depth);
        var uidepaths = item.GetComponentsInChildren<UIDepth>();
        for (int i = 0; i < uidepaths.Length; i++)
        {
            uidepaths[i].SetDepth(depth);
        }

    }
    public static void SetDepthTop(this Transform item)
    {
        var uidepath = item.GetComponent<UIDepth>();
        if (!uidepath)
        {
            uidepath = item.gameObject.AddComponent<UIDepth>();
            uidepath.SetDepthLayout("msg");
        }
        else
        {
            uidepath.SetDepthLayout("msg");
        }
    }
    public static bool IsNull(this BaseMonoBehaviour instance)
    {
        return instance != null && instance.transform;
    }
    public static bool IsNull(this MonoBehaviour instance)
    {
        return instance != null && instance.transform;
    }
    public static void SetTranLayer(this Transform tran, int layer)
    {
        tran.gameObject.layer = layer;
        Transform[] childs = tran.GetComponentsInChildren<Transform>(true);
        foreach (Transform child in childs)
        {
            child.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// 对比字符串字节长度与指定长度
    /// </summary>
    /// <param name="str">需要对比字符串</param>
    /// <param name="length">指定长度</param>
    /// <returns></returns>
    public static bool StringOutLength(string str, int length)
    {
        byte[] bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
        return bytes.Length > length;
    }

    /// <summary>
    /// 返回字符串字节长度 中文两字节 其他一字节
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int GetStringLength(string str)
    {
        byte[] bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(str);
        return bytes.Length;
    }
    public static Dictionary<string, TriggerTen> CopyValues(this Dictionary<string, TriggerTen> valuesDefault)
    {
        Dictionary<string, TriggerTen> values = new Dictionary<string, TriggerTen>();
        foreach (var item in valuesDefault)
        {
            values.Add(item.Key, item.Value.Copy());
        }
        return values;
    }
    public static void SliderAddListener(this Slider slider, System.Action callback)
    {
        slider.onValueChanged.AddListener((value) =>
        {
            if (callback != null)
                callback();
        });
    }
    public static void SliderRemoveAllListener(this Slider slider)
    {
        slider.onValueChanged.RemoveAllListeners();
    }
    
    public static Canvas GetRootCanvas(RectTransform rectTransform)
    {
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        if (canvas)
        {
            return canvas.isRootCanvas ? canvas : canvas.rootCanvas;
        }
        else
        {
            return null;
        }
    }
    
    public static Transform FindChildTransform(this Transform parent, string name)
    {
        Transform result = parent.Find(name);

        if (result != null)
        {
            return result;
        }

        for(int i = 0; i < parent.childCount; i++)
        {
            result = FindChildTransform(parent.GetChild(i), name);
            if(result != null)
            {
                return result;
            }
        }

        return result;
    }
    
    public static void LoopAllTran(this Transform tran, System.Action<Transform> callback) {
        callback(tran);
        for (int i = 0; i < tran.childCount; i++)
        {
            var child = tran.GetChild(i);
            child.LoopAllTran(callback);
        }
    }
}





