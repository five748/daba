using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using System.IO;
using GameVersionSpace;
using System;
using Cysharp.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine.XR;

public class ResVersion
{
    public string path;
    public string md5;
    public long size;
    public int sort;
}
public class ABOne {
    //========================other
    //根据平台获取streamingAssetsPath
    public string GetStreamingAssetsPath()
    {
        return Application.streamingAssetsPath;
    }
    public string AssetBundlePath
    { //assetbundle资源包保存路径
        get
        {
            
            //AssetPath
            return Application.persistentDataPath + "/asset/";
        }
    }
    public string GetDownLoadURL(string path)
    {
        if (!string.IsNullOrEmpty(HotMgr.Instance.HotResURL))
        {
            return HotMgr.Instance.HotResURL + path.Replace(@"\", @"/");
        }
        else {
            //return "https://t-dig.huanlexiuxian.cn/buider/asset/" + path.Replace(@"\", @"/");
            //return HotConfig.DownURL + "/StreamingAssets/test/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#if TMSDK
            //return "https://t-dig.huanlexiuxian.cn/buider/asset/" + path.Replace(@"\", @"/");
            return HotConfig.DownURL + "/StreamingAssets/tm/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#elif DYTMSDK
            return HotConfig.DownURL + "/StreamingAssets/tmdy/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#elif P8SDK
            return HotConfig.DownURL + "/StreamingAssets/p8wx/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#elif WX
            return HotConfig.DownURL + "/StreamingAssets/wx/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#elif DY
            return HotConfig.DownURL + "/StreamingAssets/dy/" + HotMgr.Instance.buildCache.resName + "/asset/" + path.Replace(@"\", @"/");
#endif
            return "";
        }
    }
    public string notNumPath;
    private string _basePath;
    public string basePath {
        get {
            return _basePath;
        }
        set {
            value = value.Replace(".c", "");
            _basePath = value;
            if (!string.IsNullOrEmpty(notNumPath))
            {
                var cacheName = notNumPath.Split('/');
                abName = cacheName[cacheName.Length - 1];
            }
            isHaveVariant = _basePath.Contains(HotConfig.ArtABFlag);
            _inPath = GetStreamingAssetsPath() + "/asset/" + value;
            _outPath = AssetBundlePath + value;
            _urlPath = GetDownLoadURL(value);
            SetPathVariant();
        }
    }
    private void SetPathVariant() {
        if (isHaveVariant)
        {
            if (ProssData.Instance.language == Language.FChinese)
            {
                inPath = _inPath + ".fchinese";
                outPath = _outPath + ".fchinese";
                urlPath = _urlPath + ".fchinese";
            }
            else if (ProssData.Instance.language == Language.English) {
                inPath = _inPath + ".english";
                outPath = _outPath + ".english";
                urlPath = _urlPath + ".english";
            }
            else
            {
                inPath = _inPath + ".c";
                outPath = _outPath + ".c";
                urlPath = _urlPath + ".c";
            }
        }
        else {
            inPath = _inPath;
            outPath = _outPath;
            urlPath = _urlPath;
        }
    }
    private string abName;
    private bool isHaveVariant;
    private string _inPath;
    private string _outPath;
    private string _urlPath;
    private string inPath;
    private string outPath;
    private string urlPath;
    public bool isNeedUrlDownLoad;
    public bool isNeedUrlDownLoadLater;
    public long size;
    public bool isLoaded;
    public bool isIn;
    public bool isNeedDel;
    public byte[] thisBytes;
    public string thisStr;
    private int _versionId;
    public int versionId;
    public Dictionary<string, ResVersion> versionDic;
    public string loadPath {
        get {
            if (isIn)
            {
                return inPath;
            }
            else {
                return outPath;
            }
        }
    }
    public ResVersion GetResVersion(string path) {
        if (versionDic.ContainsKey(path)) {
            return versionDic[path];
        }
        return new ResVersion();
    }
    public Dictionary<string, ResVersion> GetVersionDic() {
        versionDic = new Dictionary<string, ResVersion>();
        var str = GetABStr();
        //Debug.LogError(str);
        if (string.IsNullOrEmpty(str))
        {
            return versionDic;
        }
        str.Replace("\r\n", "\n");
        //Config
        string[] items = str.Split('\n');
        string[] versionInfo = items[0].Split(',');
        for (int i = 0; i < items.Length; i++)
        {
            string[] info = items[i].Split(',');
            if (info != null && info.Length >= 2)
            {
                ResVersion fv = new ResVersion();
                fv.path = info[0];
                fv.md5 = info[1];
                fv.size = long.Parse(info[2]);
                if (info.Length >= 3) {
                    fv.sort = int.Parse(info[3]);
                }
                versionDic.Add(info[0], fv);
            }
        }
        return versionDic;
    }
    public string GetABStr() {
        if (thisBytes != null && string.IsNullOrEmpty(thisStr)) {
            thisStr = System.Text.Encoding.UTF8.GetString(thisBytes);
        }
        return thisStr;
    }
    public string[] DepPaths {
        get {
            return null;
            //return AssetPath.AssetDep.GetAllDependencies(basePath);
        }
    }
    public void DownLoadVersion(System.Action<byte[]> callback)
    {
        if (isNeedUrlDownLoad)
        {
            //Debug.Log("Load:" + urlPath);
            DownLoadFromUrl(urlPath, (bytes, abOne) => {
                ab = abOne;
                if (ab != null)
                    ab.Unload(true);
                LoadEnd(bytes, true);
                callback(bytes);
            }, true);
        }
        else
        {
            if (ProssData.Instance.platform == Platform.WebGL)
            {
                callback(null);
                return;
            }
            if (!isIn)
            {
                byte[] bytes = new byte[0];
                if (File.Exists(outPath))
                {
                    bytes = File.ReadAllBytes(outPath);
                }
                LoadEnd(bytes, true);
                callback(bytes);
            }
            else
            {
                DownLoadFromLocal(loadPath, (bytes) =>
                {
                    LoadEnd(bytes, true);
                    callback(bytes);
                });
            }
        }
    }

    public async UniTask<AssetBundle> DownLoad(bool isVersion = false) {
        if (ab != null) {
            return null;
        }
        if (isNeedUrlDownLoad || isNeedUrlDownLoadLater)
        {
            var www = await DownLoadFromUrlWWW(urlPath);
            if (isVersion)
            {
                var bytes = www.downloadHandler.data;
                LoadEnd(bytes, isVersion);
            }
            else {
                ab = (www.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                if (!isOtherLoaded) {
                    if (ab != null)
                    {
                        ab.Unload(true);
                    }
                }
            }
        }
        else {
            if (ProssData.Instance.platform == Platform.WebGL) {
                return null;
            }
            if (isVersion && !isIn)
            {
                byte[] bytes = new byte[0];
                if (File.Exists(outPath))
                {
                    bytes = File.ReadAllBytes(outPath);
                }
                LoadEnd(bytes, isVersion);
            }
            else
            {
                DownLoadFromLocal(loadPath, (bytes) =>
                {
                    LoadEnd(bytes, isVersion);
                });
            }
        }
        return ab;
    }
    private void LoadEnd(byte[] bytes, bool isVersion = false) {
        isLoaded = true;
        thisBytes = bytes;
        if (isVersion) {
            SetVersionDic();
        }
        if (isNeedUrlDownLoad && !isVersion) {
            SaveTo();
        }
        isNeedUrlDownLoad = false;
    }
    public void SetVersionDic() {
        GetVersionDic();
        if (versionDic.Count != 0)
        {
            var data = versionDic.First().Value;
            versionId = int.Parse(data.md5);
        }
    }
    public void RemoveUnUseAb() {
        abDepNum--;
        if (abDepNum <= 0) {
            abDepNum = 0;
            ClearAb();
            go.Target = null;
        }
    }
    public void RemoveUnWRAb()
    {
        if (go == null) {
            return;
        }
        if (go.IsAlive) {
            return;
        }
        ClearAb();
    }
    private WeakReference go;
    private AssetBundle ab;
    private int abDepNum = 0;
    public async UniTask<object> LoadAb(System.Func<AssetBundle, string, object> getAssetFun) {

        //WeakReference
        //abDepNum++;
        if (go != null && go.IsAlive && go.Target == null)
        {
            return go.Target;
        }
        if (ab == null)
        {
            if (ProssData.Instance.platform == Platform.WebGL)
            {
                //Debug.LogError(urlPath);
                var www = await DownLoadFromUrlWWW(urlPath);
                ab = (www.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                ABToGo(getAssetFun);
                //www.Dispose();
            }
            else
            {
                //Debug.LogError(loadPath);
                ab = AssetBundle.LoadFromFile(loadPath);
                ABToGo(getAssetFun);
            }
        }
        else {
            ABToGo(getAssetFun);
        }
        //ab.Unload(false);
        //if (go.Target == null)
        //{
        //    Debug.Log("找不到:" + basePath);
        //}
        return go.Target;
    }
    private void ABToGo(System.Func<AssetBundle, string, object> getAssetFun) {
        //if ("https://www.resmagicgame.com/Xjfc/Webgl/StreamingAssets/dy/live/asset/res/image/changeimage/图标/1_ff83e19bc3bcaae0931b88d2cfe63db7" == urlPath)
        //{
        //    Debug.LogError(urlPath);
        //}
        System.Object obj = getAssetFun(ab, abName);
        if (go == null)
        {
            go = new WeakReference(obj);
        }
        else
        {
            go.Target = obj;
        }
        obj = null;
    }
    public async void LoadDep(System.Action callback) {
        //Debug.LogError(loadPath);
        abName = "AssetBundleManifest";
        var depAb = await LoadAb((ab, _abName) => {
            return ab.LoadAsset(abName);
        });
        AssetPath.AssetDep = depAb as AssetBundleManifest;
        callback();
    }
    public UnityEngine.Object GetAsset(AssetBundle ab)
    {
        return ab.LoadAsset(ab.name);
    }
    private void DownLoadFromLocal(string path, System.Action<byte[]> callback) {
        MonoTool.Instance.StartCoroutine(DownLoadFromLocalIE(path, callback));
    }
    private IEnumerator DownLoadFromLocalIE(string path, System.Action<byte[]> callback) {
        using(UnityWebRequest www = UnityWebRequest.Get(AssetPath.ModifyFullPath(path)))
        {
            yield return www.SendWebRequest();
            if(www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("path:" + path);
            }
            if(callback != null)
            {
                callback(www.downloadHandler.data);
            }
        }
    }
    private int loadNum = 0;
    private Coroutine DownLoadFromUrl(string url, System.Action<byte[], AssetBundle> callback, bool isVersion) {
       loadNum = 0;
       return MonoTool.Instance.StartCor(DownLoadFromUrlIE(url, callback, isVersion));
	}
    private IEnumerator DownLoadFromUrlIE(string url, System.Action<byte[], AssetBundle> callback, bool isVersion) {
        loadNum++;
        if (isVersion)
        {
            //Debug.LogError(url);
            using (UnityWebRequest www = UnityWebRequest.Get(url + "?timestamp=" + DateTime.Now.Ticks))
            {
                www.SetRequestHeader("Cache-Control", "no-cache");
                www.timeout = 100;
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ProtocolError || www.downloadHandler.data == null)
                {
                    //ProssData.Instance.OpenTimeOutTip();
                    if (Application.internetReachability == NetworkReachability.NotReachable)//断线重连
                    {
                        Debug.LogError("找不到:" + url);
                        Msg.Instance.Show("网络连接失败!");
                        callback(www.downloadHandler.data, null);
                        yield break;
                    }
                    if (www.error.IndexOf("404") != -1)
                    {
                        Debug.LogError("找不到:" + url);
                        //Msg.Instance.Show("找不到:" + url);
                        callback(null, null);
                        yield break;
                    }
                    if (loadNum >= 5)
                    {
                        Debug.LogError("下载失败:" + url);
                        Msg.Instance.Show("下载失败:" + url);
                        callback(null, null);   
                        yield break;
                    }
                    Debug.LogError("下载失败重新下载:" + url);
                    MonoTool.Instance.StartCoroutine(DownLoadFromUrlIE(url, callback, isVersion));
                }
                else
                {
                    if (callback != null)
                    {
                        callback(www.downloadHandler.data, null);
                    }
                }
            }
        }
        else {
            using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
            {
                //Debug.LogError(url);
                www.timeout = 100;
                yield return www.SendWebRequest();
                if (www.result == UnityWebRequest.Result.ProtocolError)
                {
                    if (Application.internetReachability == NetworkReachability.NotReachable)//断线重连
                    {
                        callback(null, null);
                        yield break;
                    }
                    if (www.error.IndexOf("404") != -1)
                    {
                        Debug.LogError("找不到:" + url);
                        callback(null, null);
                        yield break;
                    }
                    if (loadNum >= 5)
                    {
                        Debug.LogError("下载失败:" + url);
                        callback(null, null);
                        yield break;
                    }
                    Debug.LogError("下载失败重新下载:" + url);
                    MonoTool.Instance.StartCoroutine(DownLoadFromUrlIE(url, callback, isVersion));
                }
                else
                {
                    if (callback != null)
                    {
                        var _ab = (www.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                        callback(null, _ab);
                    }
                }
            }
        }
    }
    private Dictionary<string, UnityWebRequestAsyncOperation> CacheWWW = new Dictionary<string, UnityWebRequestAsyncOperation>();
    private bool isOtherLoaded = false;
    private async UniTask<UnityWebRequest> DownLoadFromUrlWWW(string url)
    {
        //Debug.LogError(url);
        loadNum++;
        if (!CacheWWW.ContainsKey(url)) {
            CacheWWW.Add(url, null);
        }
        if (CacheWWW[url] == null)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
            www.timeout = 100;
            CacheWWW[url] = www.SendWebRequest();
        }
        else {
            isOtherLoaded = true;
        }
        var wwwOp = CacheWWW[url];
        //Debug.LogError(CacheWWW[url]);
        await wwwOp;
        var result = wwwOp.webRequest.result;

        if (wwwOp.webRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)//断线重连
            {
                return null;
            }
            if (wwwOp.webRequest.error.IndexOf("404") != -1)
            {
                Debug.LogError("找不到:" + url);
                return null;
            }
            if (loadNum >= 5)
            {
                Debug.LogError("下载失败:" + url);

                return null;
            }
            Debug.LogError("下载失败重新下载:" + url);
           return await DownLoadFromUrlWWW(url);
        }
        CacheWWW.Remove(url);
        return wwwOp.webRequest;
    }
    private void DownLoadFromUrlWX(string url, System.Action<byte[], AssetBundle> callback, bool isVersion)
    {
        loadNum = 0;
        MonoTool.Instance.StartCor(DownLoadFromUrlIEWX(url, callback, isVersion));
    }
    private IEnumerator DownLoadFromUrlIEWX(string url, System.Action<byte[], AssetBundle> callback, bool isVersion)
    {
        loadNum++;
#if WX
        using (UnityWebRequest www = WeChatWASM.WXAssetBundle.GetAssetBundle(url))
        {
            Debug.Log(url);
            www.timeout = 100;
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (loadNum >= 5)
                {
                    Debug.LogError("下载失败:" + url);
                    callback(null, null);
                    yield break;
                }
                if (Application.internetReachability == NetworkReachability.NotReachable)//断线重连
                {
                    callback(null, null);
                    yield break;
                }
                if (www.error.IndexOf("404") != -1)
                {
                    Debug.LogError("找不到:" + url);
                    callback(null, null);
                    yield break;
                }
                Debug.LogError("下载失败重新下载:" + url);
                MonoTool.Instance.StartCoroutine(DownLoadFromUrlIE(url, callback, isVersion));
            }
            else
            {
                if (callback != null)
                {
                    var _ab = (www.downloadHandler as WeChatWASM.DownloadHandlerWXAssetBundle).assetBundle;
                    callback(null, _ab);
                }
            }
        }
#else
        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            Debug.Log(url);
            www.timeout = 100;
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                if (loadNum >= 5)
                {
                    Debug.LogError("下载失败:" + url);
                    callback(null, null);
                    yield break;
                }
                if (Application.internetReachability == NetworkReachability.NotReachable)//断线重连
                {
                    callback(null, null);
                    yield break;
                }
                if (www.error.IndexOf("404") != -1)
                {
                    Debug.LogError("找不到:" + url);
                    callback(null, null);
                    yield break;
                }
                Debug.LogError("下载失败重新下载:" + url);
                MonoTool.Instance.StartCoroutine(DownLoadFromUrlIE(url, callback, isVersion));
            }
            else
            {
                if (callback != null)
                {
                    var _ab = (www.downloadHandler as DownloadHandlerAssetBundle).assetBundle;
                    callback(null, _ab);
                }
            }
        }
#endif
    }
    public void SaveTo() {
        if(thisBytes != null)
            SaveFile(outPath, thisBytes);
    }
    public void Del() {
        if(System.IO.File.Exists(outPath))
            System.IO.File.Delete(outPath);
    }
    public void SaveFile(string fileName, byte[] bytes)
    {
        if (ProssData.Instance.platform == Platform.WebGL) {
            return;
        }
        CreateDir(fileName);
        FileStream stream = new FileStream(fileName, FileMode.Create);
        BufferedStream buffStream = new BufferedStream(stream);
        buffStream.Write(bytes, 0, bytes.Length);
        buffStream.Flush();
        stream.Flush();
        buffStream.Close();
        stream.Close();
    }
    //根据文件路径创建文件夹(假设路径是/a/b/c.prefab, 建立文件夹/a/b/)
    public void CreateDir(string path)
    {
        string info = GetDirectoryByFileName(path);
        if (!Directory.Exists(info))
        {
            Debug.Log("createFile:" + path);
            Directory.CreateDirectory(info);
        }
    }
    //获取文件的前一个文件夹路径
    public string GetDirectoryByFileName(string path)
    {
        return Path.GetDirectoryName(path);
    }
    public void ClearAb() {
        if (ab != null) {
            ab.Unload(true);
        }
    }
}
