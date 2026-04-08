﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.Globalization;
using UnityEditor;
using Cysharp.Threading.Tasks;

namespace GameVersionSpace
{
    public enum VersionType
    {
        inVersion,
        outVersion,
        serVersion,
    }
    public class CheckVersionData
    {
        public int channel;
        public int localVersionId;
        public string AppVersion;
        public string DeviceId;
        public long LastCheckTime;
    }
    public class ChannelData
    {
        public int ChannelId;
    }
    public class CheckVersionDataBack
    {
        public CheckVersionDataBackBase Data;
        public int Code;
    }
    public class CheckVersionDataBackBase
    {
        public bool isSucc;
        public bool isNeedLoad;
        public int serVersionId;
        public string hotResUrl;//热更地址

        //公告相关
        public string publicity;
        public long publicityStartTime;
        public long publicityEndTime;
        public List<DailyPublicity> dailyPublicity;
    }
    #region 公告相关
    public class DailyPublicity
    {
        public string Id;
        public int ChannelId;
        public string Title;
        public string Content;
        public int PosterId;
        public int Priority;
        public long StartTime;
        public long EndTime;
        public long UpdateTime;
    }
    #endregion
    public class LoadOverData
    {
        public int versionId;
        public List<string> loadOverKeys = new List<string>();
    }
    public class HotMgr : Single<HotMgr>
    {
        public string HotResURL;
        public Dictionary<string, List<string>> PrefabAbs =new Dictionary<string, List<string>>();
        public Dictionary<string, ABOne> AbDatasReal;
        public Dictionary<string, ABOne> AbDatas;
        private LoadOverData loadOver;
        private ABOne InVersion;
        private ABOne OutVersion;
        private ABOne SerVersion;
        public int VersionId;
        private int oneLoadNum = 20;
        public long SumResSize;
        public long LoadSize;
        public Transform tran;
        public Text MscText;
        public Text SizeText;
        public Image slider;
        private System.Action resOver;
        #region 公告相关
        public string Publicity;
        public long PublicityStartTime;
        public long PublicityEndTime;
        //TODO 以下两个变量需要本地缓存
        public List<DailyPublicity> DailyPublicityList = new List<DailyPublicity>();
        public long lastCheckTime = 0;
        #endregion
        public void Init(Transform _tran, System.Action callback)
        {
            buildCache = BuildCache.ReadData();
            if (!ProssData.Instance.IsOpenHot)
            {
                FChineseDic = HotConfig.ArtReadPath.ReadToDicStr();
                callback();
                return;
            }
            //#if UNITY_EDITOR
            //            PlayerSettings.WebGL.dataCaching = false;
            //#endif
            resOver = callback;
            if (_tran.name == "UILoadRes")
            {
                tran = _tran;
                MscText = tran.Find("middle/msc").GetComponent<Text>();
                SizeText = tran.Find("middle/size").GetComponent<Text>();
                slider = tran.Find("middle/fg").GetComponent<Image>();
                tran.Find("middle").SetActive(true);
                SizeText.transform.SetActive(false);
            }
            ShowLoadSize("检查资源中...".ChangeTextStr());
            UpDataFillIE = MonoTool.Instance.UpdateCall(() => {
                if (fillNum < 0.5f)
                {
                    fillNum += addFill;
                }
                else if (fillNum < 0.7f)
                {
                    fillNum += addFill * 0.5f;
                }
                else if (fillNum < 0.8f)
                {
                    fillNum += addFill * 0.2f;
                }
                else {
                    fillNum += addFill * 0.1f;
                }
                if (fillNum > 0.95f) {
                    return;
                }
                slider.fillAmount = fillNum;
            });
            LoadClientVersion(() =>
            {
                CheckVersionBySer((isNeedLoad) =>
                {
                    if (!isNeedLoad)
                    {
                        CompareVersion();
                        LoadOver();
                        return;
                    }
                    LoadSerVersion(async () =>
                    {
                        CompareVersion();
                        DownLoadRes(LoadOver);
                    });
                });
            });
        }
        public void LoadOver()
        {
            if (SerVersion != null)
            {
                SerVersion.SaveTo();
                VersionId = SerVersion.versionId;
                Debug.LogError("versionId:" + VersionId);
            }
            MscTextIE.Stop();
            LoadDep(resOver);
        }
        private int MscTextIndex;
        private string[] MscTextStrs = new string[4];
        private Coroutine MscTextIE;
        public void AddPrefabAbNames(string prefabName, string abName) {
            if (!PrefabAbs.ContainsKey(prefabName)) {
                PrefabAbs.Add(prefabName, new List<string>());
            }
            PrefabAbs[prefabName].Add(abName);
        }
        public void DeUnLoadUnUse(string prefabName) {
            //Resources.UnloadUnusedAssets();
            //if (!PrefabAbs.ContainsKey(prefabName))
            //{
            //    return;
            //}
            //foreach (var abName in PrefabAbs[prefabName])
            //{
            //    if (abName.Contains("common"))
            //    {
            //        continue;
            //    }
            //    if (AbDatas.ContainsKey(abName))
            //    {
            //        AbDatas[abName].RemoveUnUseAb();
            //    }
            //    if (AbDatasReal.ContainsKey(abName))
            //    {
            //        AbDatasReal[abName].RemoveUnUseAb();
            //    }
            //}
            //PrefabAbs[prefabName].Clear();
        }
        public void RemoveUnWRAb() {
            foreach (var item in AbDatas)
            {
                item.Value.RemoveUnWRAb();
            }
            foreach (var item in AbDatasReal)
            {
                item.Value.RemoveUnWRAb();
            }
        }
        private void ShowLoadMscContinue(string str)
        {
            if (MscText == null)
            {
                return;
            }
            MscTextStrs[0] = str;
            MscTextStrs[1] = str + ".";
            MscTextStrs[2] = str + "..";
            MscTextStrs[3] = str + "...";
            MscTextIndex = 0;
            MscTextIE = MonoTool.Instance.UpdateCall(0.1f, () =>
            {
                MscTextIndex++;
                if (MscTextIndex >= MscTextStrs.Length)
                    MscTextIndex = 0;
                MscText.text = MscTextStrs[MscTextIndex];
            });
        }
        public void ShowLoadMsc(string str)
        {
            if (MscText == null)
            {
                return;
            }
            MscText.text = str;
        }
        private void ShowLoadSize(string str)
        {
            if (SizeText)
                SizeText.text = str;
        }
        private void LoadClientVersion(System.Action callback)
        {
            LoadVersion(VersionType.inVersion, (inAbone) =>
            {
                if (inAbone.thisBytes != null)
                {
                    if (inAbone.thisBytes == null)
                    {
                        Debug.LogError("游戏内版本文件不能为空!");
                    }
                    InVersion = inAbone;
                    LoadVersion(VersionType.outVersion, (outAbone) =>
                    {
                        OutVersion = outAbone;
                        //Debug.LogError(OutVersion.versionId);
                        if (OutVersion.versionId != 0)
                        {
                            if (OutVersion.versionId > InVersion.versionId)
                                VersionId = OutVersion.versionId;
                            else
                                VersionId = InVersion.versionId;
                        }
                        else
                        {
                            VersionId = InVersion.versionId;
                        }
                        //Debug.LogError(Application.persistentDataPath);
                        //Debug.LogError("versionId:" + VersionId);
                        callback();
                    });
                }
                else {
                    callback();
                }
            });
        }
        public BuildCache buildCache;
        private void LoadVersion(VersionType versionType, System.Action<ABOne> callback)
        {
            ABOne one = new ABOne();
            one.basePath = $"version{buildCache.resVersion}.txt";
            if (versionType == VersionType.inVersion)
            {
                one.isIn = true;
            }
            if (versionType == VersionType.outVersion)
            {
                one.isIn = false;
            }
            if (versionType == VersionType.serVersion)
            {
                one.isNeedUrlDownLoad = true;
                one.isIn = false;
            }
            one.DownLoadVersion((bytes) => {
                callback(one);
            });
        }
        public void CheckVersionBySer(System.Action<bool> callback)
        {
            if (ProssData.Instance.platform == Platform.WebGL)
            {
                callback(true);
            }
            else {
                callback(false);
            }
            //CheckVersionData data = new CheckVersionData();
            //data.channel = ProssData.Instance.ServerChannel;
            //data.localVersionId = VersionId;
            //data.AppVersion = Application.version;
            //data.LastCheckTime = lastCheckTime;
            //// data.DeviceId = SystemInfo.deviceUniqueIdentifier;
            //Debug.Log(Application.persistentDataPath);
            //Debug.Log("请求热更检查");
            //CallCheckoutVersion(data, (backData) =>
            //{
            //    if (backData == null)
            //    {
            //        callback(false);
            //        return;
            //    }
            //    HotResURL = backData.Data.hotResUrl;
            //    callback(backData.Data.isNeedLoad);
            //});
        }
        //private void CallCheckoutVersion(CheckVersionData data, System.Action<CheckVersionDataBack> callback)
        //{
        //    Debug.Log(JsonMapper.ToJson(data));
        //    ServerMono.Instance.CallServerMessage("Game/CheckVersion", JsonMapper.ToJson(data), (str) =>
        //    {
        //        Debug.Log(str);
        //        var backData = JsonMapper.ToObject<CheckVersionDataBack>(str);
        //        //TODO Test
        //        // backData.Data.isNeedLoad = false;
        //        //TODO Test
        //        Publicity = backData.Data.publicity;
        //        PublicityStartTime = backData.Data.publicityStartTime;
        //        PublicityEndTime = backData.Data.publicityEndTime;
        //        for (var i = 0; i < backData.Data.dailyPublicity.Count; i++)
        //        {
        //            DailyPublicityList.Add(backData.Data.dailyPublicity[i]);
        //        }
        //        DailyPublicityList = backData.Data.dailyPublicity;
        //        callback(backData);
        //    });
        //}
        public void LoadSerVersion(System.Action callback)
        {
            LoadVersion(VersionType.serVersion, (serAbone) =>
            {
                if (serAbone.thisBytes == null)
                {
                    Debug.LogError("服务端版本文件不能为空!");
                    callback();
                    return;
                }
                else
                {
                    SerVersion = serAbone;
                }
                callback();
            });
        }
        private void CompareVersion()
        {
            //var strAsset = Resources.Load<TextAsset>("version");
            //if (strAsset != null)
            //{
            //    InVersion = new ABOne();
            //    InVersion.thisStr = strAsset.text;
            //    InVersion.SetVersionDic();
            //}
            AbDatasReal = new Dictionary<string, ABOne>();
            AbDatas = new Dictionary<string, ABOne>();
            Dictionary<string, ResVersion> keys = new Dictionary<string, ResVersion>();
            AddVerKey(InVersion, keys);
            AddVerKey(OutVersion, keys);
            AddVerKey(SerVersion, keys);
            foreach (var item in keys)
            {
                if (item.Key.Contains("version.txt")) {
                    continue;
                }
                var path = item.Key;
                ABOne one = new ABOne();
                var newPath = CutABPath(path);
                one.notNumPath = newPath;
                one.basePath = path;
                one.size = item.Value.size;
                if (ProssData.Instance.platform == Platform.WebGL)
                {

#if !UNITY_EDITOR
                    one.isNeedUrlDownLoad = true;
#endif
                    if (path.Contains("/changeimage/") || path.Contains("/frame/"))
                    {
                        one.isNeedUrlDownLoad = false;
                        one.isNeedUrlDownLoadLater = true;
                    }
                    else {
                        if (item.Value.sort == 0)
                        {
                            one.isNeedUrlDownLoad = true;
                        }
                        else {
                            one.isNeedUrlDownLoad = false;
                            one.isNeedUrlDownLoadLater = true;
                        }
                    }
                    //Debug.LogError(path);
                    AbDatasReal.Add(path, one);
                    AbDatas.Add(newPath, one);
                    continue;
                }
                var inRes = InVersion.GetResVersion(path);
                if (OutVersion == null) {
                    OutVersion = InVersion;
                }
                var outRes = OutVersion.GetResVersion(path);
                var clientRes = (OutVersion.thisBytes == null || OutVersion.thisBytes.Length == 0) ? inRes : outRes;
                if (SerVersion != null)
                {
                    var serRes = SerVersion.GetResVersion(path);
                    one.isNeedUrlDownLoad = serRes.md5 != clientRes.md5;
                    if (string.IsNullOrEmpty(serRes.md5))
                    {
                        one.isNeedUrlDownLoad = false;
                    }
                    one.isIn = string.IsNullOrEmpty(serRes.md5) || inRes.md5 == serRes.md5;
                    one.isNeedDel = SerVersion.versionId != 0 && string.IsNullOrEmpty(serRes.md5) && !string.IsNullOrEmpty(outRes.md5);
                    if (one.isNeedDel)
                    {
                        Debug.LogError("needDel:" + path);
                        one.Del();
                    }
                }
                else
                {
                    one.isIn = string.IsNullOrEmpty(outRes.md5) || inRes.md5 == outRes.md5;
                }
                AbDatasReal.Add(path, one);
                AbDatas.Add(newPath, one);
            }
        }
        private string CutABPath(string path) {
            if (ProssData.Instance.platform != Platform.WebGL) {
                return path;
            }
            if (!path.Contains("_")) {
                return path;
            }
            var strs = path.Split('_');
            var str = "";
            for (int i = 0; i < strs.Length - 1; i++)
            {
                str += strs[i] + "_";
            }
            return str.CutLast();
        }
        private void AddVerKey(ABOne one, Dictionary<string, ResVersion> keys)
        {
            if (one != null)
            {
                int index = -1;
                foreach (var item in one.versionDic)
                {
                    index++;
                    if (index < 2)
                    {
                        continue;
                    }
                    if (!keys.ContainsKey(item.Key))
                        keys.Add(item.Key, item.Value);
                }
            }
        }
        private Coroutine UpDataFillIE;
        private float fillNum;
        private float targetFill = 85;
        private float addFill = 0.01f;
        public void DownLoadRes(System.Action callback)
        {
            SumResSize = 0;
            if (loadOver == null || loadOver.versionId != VersionId)
            {
                loadOver = new LoadOverData();
            }
            List<string> keys = new List<string>();
            foreach (var item in AbDatas)
            {
                if (item.Value.isNeedUrlDownLoad)
                {
                    keys.Add(item.Key);
                    SumResSize += item.Value.size;
                }
            }
            if (keys.Count == 0)
            {
                callback();
                return;
            }
            ShowLoadMsc("加载资源中(x)".Replace("x", SumResSize.ToMStr()));
            LoadSize = 0;
            ShowLoadSize("加载:x".Replace("x", LoadSize.ToMStr()));
            int overIndex = 0;
           
            DownLoadOne(overIndex, keys, callback);
        }
        private async void DownLoadOne(int overIndex, List<string> keys, System.Action callback)
        {
            if (overIndex >= keys.Count)
            {
                UpDataFillIE.Stop();
                callback();
                return;
            }
            List<UniTask<AssetBundle>> tasks = new List<UniTask<AssetBundle>>();
            for (int i = 0; i < oneLoadNum; i++)
            {
                var path = keys[overIndex];
                overIndex++;
                if (overIndex >= keys.Count)
                {
                    break;
                }
                LoadSize += AbDatas[path].size;
                tasks.Add(AbDatas[path].DownLoad(false));
            }
            //targetFill = 1.0f * LoadSize / SumResSize;
            //addFill = 1.0f * (targetFill - fillNum) / oneLoadNum;
            await UniTask.WhenAll(tasks);
            DownLoadOne(overIndex, keys, callback);
        }
        public void DownLoadLater()
        {
            if (AbDatas == null) {
                return;
            }
            List<string> keys = new List<string>();
            foreach (var item in AbDatas)
            {
                if (item.Value.isNeedUrlDownLoadLater)
                {
                    //Debug.LogError(item.Key);
                    keys.Add(item.Key);
                }
            }
            if (keys.Count == 0)
            {
                return;
            }
            int overIndex = 0;
            DownLoadOneLater(overIndex, keys);
        }
        private async void DownLoadOneLater(int overIndex, List<string> keys)
        {
            if (overIndex >= keys.Count)
            {
                //Msg.Instance.Show("后台加载资源完毕!");
                return;
            }
            List<UniTask<AssetBundle>> tasks = new List<UniTask<AssetBundle>>();
            for (int i = 0; i < 5; i++)
            {
                var path = keys[overIndex];
                overIndex++;
                if (overIndex >= keys.Count)
                {
                    break;
                }
                tasks.Add(AbDatas[path].DownLoad(false));
            }
            await UniTask.WhenAll(tasks);
            DownLoadOneLater(overIndex, keys);
        }
        private void LoadDep(System.Action callback)
        {
            if (!AbDatas.ContainsKey(AssetPath.DepBundlePath) && !AbDatasReal.ContainsKey(AssetPath.DepBundlePath))
            {
                Debug.Log("找不到:" + AssetPath.DepBundlePath);
                callback();
                return;
            }
            AbDatas[AssetPath.DepBundlePath].LoadDep(callback);
        }
        public async UniTask<object> LoadAB(string path, System.Func<AssetBundle, string, object> getAssetFun)
        {
            if (AbDatas == null)
            {
                return null;
            }
            if (!AbDatas.ContainsKey(path) && !AbDatasReal.ContainsKey(path))
            {
                Debug.Log("找不到:" + path);
                return null;
            }
            //Debug.LogError(path);
            if (AbDatas.ContainsKey(path))
            {
               return await AbDatas[path].LoadAb(getAssetFun);
            }
            else {
                 return await AbDatasReal[path].LoadAb(getAssetFun);
            } 
        }
        public bool IsHaveSpine(string spinePath, string spineId)
        {
            return IsHaveRes(string.Format("Res/Spine/{0}/{1}/{1}_SkeletonData", spinePath, spineId), ".asset");
        }
        public bool IsHaveRes(string path, string end)
        {
            if (AbDatas == null)
            {
                //Debug.LogError(Application.dataPath + "/" + path + end);
                return File.Exists(Application.dataPath + "/" + path + end);
            }
            else
            {
                //Debug.LogError(path.ToLower());
                return AbDatas.ContainsKey(path.ToLower());
            }
        }
        public Dictionary<string, string> FChineseDic = new Dictionary<string, string>();
        public string ModifyLanguage(string str) {
            if (ProssData.Instance.language == Language.Chinese) {
                return str;
            }
            if (FChineseDic.ContainsKey(str))
            {
                if (ProssData.Instance.language == Language.English)
                {
                    return FChineseDic[str].Replace("FChinese", "Engish");
                }
                else {
                    return FChineseDic[str];
                }
            }
            return str;
        }
        public void DelAb()
        {
            if (AbDatas == null)
            {
                return;
            }
            foreach (var item in AbDatas)
            {
                item.Value.ClearAb();
            }
        }
        public new void clear()
        {
            DelAb();
            base.clear();
        }
    }
}