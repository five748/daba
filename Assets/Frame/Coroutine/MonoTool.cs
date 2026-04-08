using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Net;
using UnityEngine.UI;

public class MonoTool : SingleMono2<MonoTool>
{
    public Coroutine StartCor(IEnumerator routine, List<Coroutine> Cos)
    {
        var co = StartCoroutine(routine);
        Cos.Add(co);
        return co;
    }
    public Coroutine StartCor(IEnumerator routine)
    {
        var co = StartCoroutine(routine);
        ProssData.Instance.Cos.Add(co);
        return co;
    }

    public Coroutine MoveFieldSlow(Camera cam, float target, float time)
    {
        return StartCor(MoveFieldSlowIE(cam, target, time));
    }
    public IEnumerator MoveFieldSlowIE(Camera cam, float target, float time)
    {
        float v = 0;
        while (true)
        {
            //Debug.LogError(cam.fieldOfView);
            cam.fieldOfView = Mathf.SmoothDamp(cam.fieldOfView, target, ref v, time);
            if (Mathf.Abs(cam.fieldOfView - target) < 0.01f)
            {
                yield break;
            }
            yield return null;
        }
    }
    public void AddComponentDragUI(GameObject go)
    {
        //go.AddComponent<DragSpine>();
    }
    public Coroutine StartCor(float time, System.Func<bool> call, System.Action callback = null)
    {
        return StartCor(IEFun(time, call, callback));
    }
    public IEnumerator IEFun(float time, System.Func<bool> call, System.Action callback = null)
    {
        while (true)
        {
            if (!call())
            {
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return new WaitForSeconds(time);
        }
    }
    public Coroutine StartCor(System.Func<bool> call, System.Action over = null)
    {
        return StartCor(IEFun(call, over));
    }
    public IEnumerator IEFun(System.Func<bool> call, System.Action over = null)
    {
        while (true)
        {
            if (!call())
            {
                if (over != null)
                {
                    over();
                }
                yield break;
            }
            yield return null;
        }
    }
    public Coroutine StartCor(float time, System.Action call, System.Action over)
    {
        return StartCor(StartCoroutineHaveCloseTime(time, call, over));
    }
    private IEnumerator StartCoroutineHaveCloseTime(float time, System.Action call, System.Action over)
    {
        float beginTime = Time.time;
        while (true)
        {
            if (Time.time - beginTime >= time)
            {
                over();
                yield break;
            }
            call();
            yield return null;
        }
    }
    public Coroutine UpdateCall(System.Action callback)
    {
        return StartCor(UpdateCallIE(callback));
    }
    public IEnumerator UpdateCallIE(System.Action callback)
    {
        while (true)
        {
            callback();
            yield return null;
        }
    }
    public Coroutine UpdateCall(float time, System.Action callback)
    {
        return StartCor(UpdateCallIETime(time, callback));
    }
    public IEnumerator UpdateCallIETime(float time, System.Action callback)
    {
        var waitTime = new WaitForSeconds(time);
        while (true)
        {
            if (!GameProcess.Instance.isInBg) {
                callback();
            }
            yield return waitTime;
        }
    }
    public Coroutine UpdateCallHaveTime(float time, System.Action callback)
    {
        return StartCor(UpdateCallIEHaveTime(time, callback));
    }
    public IEnumerator UpdateCallIEHaveTime(float time, System.Action callback)
    {
        var se = new WaitForSeconds(time);
        while (true)
        {
            callback();
            yield return se;
        }
    }
    public Coroutine Wait(float time, System.Action call)
    {
        return StartCor(WaitIE(time, call));
    }

    public Coroutine Wait(float time, int index, System.Action<int> call)
    {
        return StartCor(WaitIE(time, index, call));
    }
    private IEnumerator WaitIE(float time, System.Action call)
    {
        //Debug.LogError("waitTime:" + 1.0f * time / Time.timeScale);
        yield return new WaitForSeconds(1.0f * time / Time.timeScale);
        call();
    }
    private IEnumerator WaitIE(float time, int index, System.Action<int> call)
    {
        yield return new WaitForSeconds(1.0f * time / Time.timeScale);
        call(index);
    }
    public Coroutine WaitEndFrame(System.Action call)
    {
        return StartCor(WaitEndFrameIE(call));
    }
    private IEnumerator WaitEndFrameIE(System.Action call)
    {
        yield return new WaitForEndOfFrame();
        call();
    }
    public Coroutine WaitTwoFrame(System.Action call)
    {
        return StartCor(WaitTwoFrameIE(call));
    }
    private IEnumerator WaitTwoFrameIE(System.Action call)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        call();
    }
    private string _sdkcontent = "";
    public string sdkcontent
    {
        get { return this._sdkcontent; }
        set
        {
            this._sdkcontent = value;
            WhenSDKContentChange();
        }
    }
    public delegate void delSDKContentChange(object sender, string content, EventArgs e);
    public event delSDKContentChange OnSDKContentChanged;
    public void WhenSDKContentChange()
    {
        if (OnSDKContentChanged != null)
        {
            OnSDKContentChanged(this, this._sdkcontent, null);
        }
    }
    public void SilangCallback(string cont)
    {
        sdkcontent = cont;
    }
    public void MoveDistance(Transform myTran, Vector2 distance, float speed, System.Action<Transform> callback = null)
    {
        distance = (Vector2)(myTran.localPosition) + distance;
        StartCor(MoveTowardIE(myTran, distance, speed, callback));
    }
    public void MoveToward(Transform myTran, Vector2 target, float speed, System.Action<Transform> callback = null)
    {
        StartCor(MoveTowardIE(myTran, target, speed, callback));
    }
    private IEnumerator MoveTowardIE(Transform myTran, Vector2 targetPos, float speed, System.Action<Transform> callback = null)
    {
        MoveHelp moveHelp = new MoveHelp(myTran.position, targetPos);
        while (true)
        {
            //print(speed + ":");
            myTran.position = Vector3.MoveTowards(myTran.position, targetPos, speed);
            if (moveHelp.CheckIsGoToTarget(myTran.position))
            {
                myTran.position = targetPos;
                if (callback != null)
                {
                    callback(myTran);
                }
                yield break;
            }
            yield return null;
        }
    }
    public IEnumerator MoveTowardTranIE(Transform myTran, Transform target, float speed, System.Action<Transform> callback = null)
    {
        while (true)
        {
            myTran.position = Vector3.MoveTowards(myTran.position, target.position, speed);
            //Debug.LogError(Vector3.Distance(myTran.position, target.position));
            if (Vector3.Distance(myTran.position, target.position) < 20)
            {
                if (callback != null)
                {
                    callback(myTran);
                }
                yield break;
            }
            yield return null;
        }
    }
    public void MoveTowardRt(RectTransform myTran, Vector2 target, float speed, System.Action callback = null)
    {
        StartCor(MoveTowardRtIE(myTran, target, speed, callback));
    }
    private IEnumerator MoveTowardRtIE(RectTransform myTran, Vector2 targetPos, float speed, System.Action callback = null)
    {
        MoveHelp moveHelp = new MoveHelp(myTran.anchoredPosition, targetPos);
        while (true)
        {
            //print(speed + ":");
            myTran.anchoredPosition = Vector2.MoveTowards(myTran.anchoredPosition, targetPos, speed);
            if (moveHelp.CheckIsGoToTarget(myTran.anchoredPosition))
            {
                myTran.anchoredPosition = targetPos;
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }
    public Coroutine MoveTowardLocal(Transform myTran, Vector3 target, float speed, System.Action callback = null)
    {
        return StartCor(MoveTowardLocallIE(myTran, target, speed, callback));
    }
    private IEnumerator MoveTowardLocallIE(Transform myTran, Vector3 targetPos, float speed, System.Action callback = null)
    {
        MoveHelp moveHelp = new MoveHelp(myTran.localPosition, targetPos);
        while (true)
        {
            if (myTran == null) {
                yield break;
            }
            //print(speed + ":");
            myTran.localPosition = Vector3.MoveTowards(myTran.localPosition, targetPos, speed);
            if (moveHelp.CheckIsGoToTarget(myTran.localPosition))
            {
                myTran.localPosition = targetPos;
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return null;
        }
    }
    public void LookTran(Transform myTran, Transform target, float speed, System.Action callback = null)
    {
        StartCor(LookTranIE(myTran, target, speed, callback));
    }
    private void LookAt(Vector2 oriPos, Vector2 targetPos)
    {
        Vector2 v = targetPos - oriPos;
        transform.right = v;
    }
    private IEnumerator LookTranIE(Transform myTran, Transform target, float speed, System.Action callback = null)
    {
        Vector2 currPos;
        RectTransform rt = myTran.parent.GetComponent<RectTransform>();
        while (true)
        {
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(target.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPoint, UIManager.UICamera, out currPos);
            Vector2 v = currPos - myTran.GetComponent<RectTransform>().anchoredPosition;
            myTran.up = v;
            callback();
            yield break;
            //myTran.LookAt2D(currPos);
            //if (myTran.localPosition.y >= currPos.y)
            //{
            //    if (callback != null)
            //    {
            //        callback();
            //    }
            //    yield break;
            //}
            yield return null;
        }
    }
    public Coroutine CallServer(string url, string data, string token, System.Action<UnityWebRequest> callback)
    {
        return StartCor(DownLoadFromUrlIE(url, data, token, callback));
    }
    private IEnumerator DownLoadFromUrlIE(string url, string data, string token, System.Action<UnityWebRequest> callback)
    {
        var www = new UnityWebRequest(url, "POST");
        if (!string.IsNullOrEmpty(data))
        {
            byte[] databyte = System.Text.Encoding.UTF8.GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(databyte);
        }
        www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
        if (token == null)
        {
            token = "";
        }

        var t = TimeTool.SerNowUtcTimeInt;
        if (token != "")
        {
            var newToken = CryptionTool.GetMonoToken(token, url, t);
            www.SetRequestHeader("token", newToken);
        }
        var apitime = t.ToString();
        //www.SetRequestHeader("apitime", apitime);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        callback(www);
    }
    public void CallSerHttp(string url, string jsonData, string token, System.Action<string> callback) {
        string result = "";
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
        req.Method = "POST";
        req.Timeout = 1800; //设置过期时间:毫秒
        req.ContentType = "application/json";
        byte[] data = Encoding.UTF8.GetBytes(jsonData);
        req.ContentLength = data.Length;
        using (Stream reqStream = req.GetRequestStream())
        {
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
        }
        HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        Stream stream = resp.GetResponseStream();
        using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
        {
            result = reader.ReadToEnd();
        }
        callback(result);
    }
    public Coroutine PostRequest(string url, string data, string token, System.Action<string> callback)
    {
        return StartCor(PostRequestIE(url, data, token, callback));
    }
    IEnumerator PostRequestIE(string url, string jsonData, string token, System.Action<string> callback)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //设置header
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError("请求网络失败:" + url + webRequest.error);
                ProssData.Instance.OpenTimeOutTip();
            }
        }
    }
    
    public Coroutine GetRequest(string url, string token, System.Action<string> callback)
    {
        Debug.Log($"Get请求{url},token:{token}");
        return StartCor(GetRequestIE(url, token, callback));
    }
    IEnumerator GetRequestIE(string url, string token, System.Action<string> callback)
    {
        using (UnityWebRequest webRequest = new UnityWebRequest(url, "GET"))
        {
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //设置header
            webRequest.SetRequestHeader("Content-Type", "application/json");
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                callback(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.LogError(webRequest.error);
            }
        }
    }
    public Coroutine UpdateCallHaveTimeSkill(float time, System.Action callback)
    {
        return StartCor(UpdateCallIEHaveTimeSkill(time, callback));
    }
    public IEnumerator UpdateCallIEHaveTimeSkill(float time, System.Action callback)
    {
        var se = new WaitForSeconds(time);
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.0f, time));
        while (true)
        {
            callback();
            yield return se;
        }
    }
    public Coroutine UpdateCallHaveTimeNOFirst(float time, System.Action callback)
    {
        return StartCor(UpdateCallIEHaveTimeNOFirst(time, callback));
    }
    public IEnumerator UpdateCallIEHaveTimeNOFirst(float time, System.Action callback)
    {
        var se = new WaitForSeconds(time);
        while (true)
        {
            yield return se;
            callback();
        }
    }
}
