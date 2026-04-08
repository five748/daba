using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;

public class TranslateTool: SingleMono2<TranslateTool>
{
    private int lastUse;
    private int index=0;

    class BackData1 {
        public string type;
        public int errorCode;
        public int elapsedTime;
        public List<List<_BackData1>> translateResult;
    }
    class _BackData1
    {
        public string src;
        public string tgt;
    }

    public void GetLanguage(string text, int language, System.Action<string> callback)
    {
        if((index++)%2==0)
            GetLanguage1(text, language, callback);
        else
            GetLanguage2(text, language, callback);
    }

    public void GetLanguage1(string text, int language, System.Action<string> callback)
    {
        var arg = "AUTO";
        var http1 = "http://fanyi.youdao.com/translate?&doctype=json&type={0}&i={1}";
        switch (language)
        {
            case 0://英语
                arg = "ZH_CN2EN";
                break;
            case 1://日语
                arg = "ZH_CN2JA";
                break;
            case 2://韩语
                arg = "ZH_CN2KR";
                break;
        };
        StartCoroutine(CallHttpForTestIE(string.Format(http1, arg, text), (str) => {
            Debug.Log(str);
            var backData = JsonMapper.ToObject<BackData1>(str);
            callback(backData.translateResult[0][0].tgt);
        }));
    }
    public void GetLanguage2(string text, int language, System.Action<string> callback)
    {
        var arg = "en";
        var http2 = "https://translate.googleapis.com/translate_a/single?client=gtx&dt=t&sl=zh-CN&tl={0}&q={1}";
        switch (language)
        {
            case 0://英语
                arg = "EN";
                break;
            case 1://日语
                arg = "JA";
                break;
            case 2://韩语
                arg = "KR";
                break;
        };
        StartCoroutine(CallHttpForTestIE(string.Format(http2, arg, text), (str) => {
            Debug.Log(str);
            var backData = JsonMapper.ToObject(str);
            callback(backData[0][0][0].ToString());
        }));
    }

    private IEnumerator CallHttpForTestIE(string http, System.Action<string> callback)
    {
        if(TimeTool.SerNowTimeInt- lastUse < 3)
        {
            yield break;
        }
        System.DateTime dateTime = System.DateTime.UtcNow;
        var www = new UnityWebRequest(http, "GET");
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        callback(www.downloadHandler.text);
    }
}
