using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;
using Object = System.Object;

namespace YuZiSdk
{
    
    internal class ReqData
    {
        public string url; 
        public string data;
        public Action<YuziMgr.RS_Common> cb;
    }
    public class YuziHttp
    {
        private static Queue<ReqData> _reqQueue = new Queue<ReqData>();
        private static bool _isReqQueue = false;
        public static string token = "";
        
        public static void HttpPost<T>(string url, Object req, Action<T> callback)
        {
            string data = JsonMapper.ToJson(req);
            MonoTool.Instance.StartCor(Request<T>(SdkMgr.Instance.sdkConfig.Url_Report + url, data, callback));
        }

        //队列请求,上报请求使用这个方法,不需要返回值
        public static void HttpPostWithToken(string url, Object req, Action<YuziMgr.RS_Common> callback)
        {
            string data = JsonMapper.ToJson(req);
            ReqData req_data = new ReqData();
            req_data.url = SdkMgr.Instance.sdkConfig.Url_Report + url;
            req_data.data = data;
            req_data.cb = callback;
            _reqQueue.Enqueue(req_data);
            if (_isReqQueue)
            {
                return;
            }

            _isReqQueue = true;
            QueueRequest();
        }

        private static void QueueRequest()
        {
            ReqData reqData = _reqQueue.Dequeue();
            if (reqData == null)
            {
                _isReqQueue = false;
                return;
            }
            MonoTool.Instance.StartCor(Request<YuziMgr.RS_Common>(reqData.url, reqData.data, (YuziMgr.RS_Common res) =>
            {
                _isReqQueue = _reqQueue.Count > 0;

                if (res?.code == 900002 || res?.code == 900001){return;}

                if (res?.code == 900001)
                {
                    Debug.LogWarning($"Token过期:${res.msg}，进行重新 登入");
                    YuziMgr.Instance.UserLogin(null);
                    return;
                }

                reqData.cb?.Invoke(res);
                if (_isReqQueue) QueueRequest();
            },token));

        }
        
        private static IEnumerator Request<T>(string url,string data,Action<T> callback,string token = "")
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("网络断开，请检查网络连接!");
                callback?.Invoke(default);
                yield break;
            }
            Debug.Log(url + data);

            using UnityWebRequest request = new UnityWebRequest(url);
            request.method = "Post";
            request.timeout = 2000;
            request.SetRequestHeader("Content-Type","application/json");
            if (token != "") { request.SetRequestHeader("Authorization",$"Bearer {token}"); }

            if (data != "")
            {
                byte[] dataBuffer = Encoding.UTF8.GetBytes(data);
                request.uploadHandler = new UploadHandlerRaw(dataBuffer);
            }
            request.downloadHandler = new DownloadHandlerBuffer();
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)//成功
            {
                string res_txt = request.downloadHandler.text;
                res_txt = res_txt.Replace("\"result\":\"\"", "\"result\":null");
                try
                {
                    Debug.Log("上报返回:" + res_txt);
                    T res = JsonMapper.ToObject<T>(res_txt);
                    callback?.Invoke(res);
                }
                catch (Exception e)
                {
                    Debug.LogError($"上报返回解析出错,url:{url},data:{res_txt}");

                    callback?.Invoke(default);
                }
                
                //try
                //{
                //    T res = JsonMapper.ToObject<T>(res_txt);
                //    callback?.Invoke(res);
                //}
                //catch
                //{
                //    Debug.LogError(url);
                //}
            }
            else
            {
                Debug.LogError($"上报请求出错,url:{url},data:{data}");
                callback?.Invoke(default);
            }

        }
        
        
    }
}