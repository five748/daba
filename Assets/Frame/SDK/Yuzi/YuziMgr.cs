using System;
using System.Collections.Generic;
using GameVersionSpace;
using YuZiSdk.Base;
using UnityEngine;
using Object = System.Object;

namespace YuZiSdk
{
    //鱼子公司管理器,处理服务端上报打点

    //鱼子账号
    [Serializable]
    public class YuziAccount : ReadAndSaveTool<YuziAccount>
    {
        public string userId;            //登录游戏生成的id                对应服务端的 user_id
        public string deviceId;              //设备码                           对应服务端的 deviceUID
        public int appId;                 //y游戏id,服务端下发

        public string ConfigMd5;                  //版本号

        public string Token;                  //服务端下发,版本变更和注册时修改
        public long registerTime;   //注册时间秒
        

        public bool isRegister = false;

        public void SetDeviceId(string deviceUID, string registerUID)
        {
            //设备码 = 手机设备码 + 随机数
            deviceId = string.IsNullOrEmpty(deviceUID) ? RandomCode() : deviceUID;
            userId = string.IsNullOrEmpty(registerUID) ? RandomCode() : registerUID;
        }

        private string RandomCode() { return DateTimeOffset.Now.Millisecond.ToString() + UnityEngine.Random.Range(1000, 10000).ToString(); }
        public void Save() { WriteInPhone(this, "yuzi_new"); }
        public YuziAccount Read() { return ReadByPhone("yuzi_new", () => { return new YuziAccount(); }); }
    }

    //鱼子公司管理器,处理服务端上报
    public partial class YuziMgr : Single<YuziMgr>
    {
        private ISdk _sdk; //对应sdk
        public YuziAccount _account;//本地账号信息
        public static bool isOpenReport = false;
        public static bool isOpenYuziReport = false;
        public bool isNew = false;

        public YuziMgr()
        {
            _sdk = SdkMgr.Instance.Sdk;
            _account = new YuziAccount();
            _account = _account.Read();
        }


        //激活账号
        public void UserActive(string devId, string uid,Action<bool> callback)
        {
            isNew = false;
            if (!isOpenReport )
            {
                callback.Invoke(true);
                return;
            }

            if (_account.appId > 0 && _account.appId != HotMgr.Instance.buildCache.SDKChannlID)
            {
                _account = new YuziAccount();
            }
            if (_account.isRegister)
            {
                Debug.Log("鱼子账号已注册userId:" + _account.userId);
                callback(true);
                return;
            }
            isNew = true;
            _account.SetDeviceId(devId, uid);
            RQ_ActiveAccount req = new RQ_ActiveAccount();
            req.app_id = HotMgr.Instance.buildCache.SDKChannlID.ToString();
            // req.app_name = SdkMgr.Instance.sdkConfig.app_name;
            req.device_code = _account.deviceId;
            // req.app_platform = SdkMgr.Instance.sdkConfig.platform;
            // req.app_channel = SdkMgr.Instance.sdkConfig.os + "-" + SdkMgr.Instance.sdkConfig.platform;
            // req.version_no = Application.version;
            //Debug.LogError("RS_ActiveAccount");
            YuziHttp.HttpPost<RS_ActiveAccount>(RQ_ActiveAccount.url, req, (RS_ActiveAccount res) =>
            {
                if (res == default || res == null || res.code != 200)
                {
                    Debug.Log("激活失败");
                    clearAccount();
                    callback(true);
                    return;
                }
                Debug.Log("鱼子激活成功,appId = " + res.result.app_id);
                _account.appId = res.result.app_id;
                // _account.userId = res.result.user_id;
                if (res.result.open_id == null || res.result.open_id == "")
                    callback(true);
                else
                {
                    _account.isRegister = true;
                    _account.userId = res.result.open_id;
                    _account.Save();
                    callback(true);
                }
            });
        }

        private void clearAccount()
        {
            _account = new YuziAccount();
            _account.Save();
            isOpenReport = false;
        }

        //注册账号
        public void UserRegister(Action<bool> callback)
        {
            if (_account.isRegister)
            {
                UserLogin(callback);
                return;
            }

            if (_account.registerTime == 0)
            {
                _account.registerTime = DateTimeOffset.Now.Millisecond;
                _account.Save();
            }

            RQ_Register req = new RQ_Register();
            req.user_id = _account.userId;
            req.app_id = _account.appId;
            req.device_code = _account.deviceId;
            // req.version_no = Application.version;
            YuziHttp.HttpPost<RS_Register>(RQ_Register.url, req, (RS_Register res) =>
            {
                if (res == default || res == null || res.code != 200)
                {
                    Debug.LogError($"注册账号失败，进行重新 激活 -> 注册");
                    clearAccount();
                    callback(true);
                    return;
                }
                _account.registerTime = res.result.create_time;
                _account.userId = res.result.user_id;
                _account.appId = res.result.app_id;
                _account.isRegister = true;
                _account.Save();
                Debug.Log("注册成功,userId = " + res.result.user_id);
                UserLogin(callback);
                
            });
        }

        //用户登录
        public void UserLogin(Action<bool> callback)
        {
            _sdk.Report.ReportGameProcess(EnumGameProcess.eLogin);
            if (!isOpenReport)
            {
                callback.Invoke(true);
                return;
            }
            if (!_account.isRegister)
            {
                Debug.Log("未注册鱼子账号!");
                callback.Invoke(false);
                return;
            }

            RQ_Login req = new RQ_Login();
            req.user_id = _account.userId;
            req.app_id = _account.appId;
            req.version_no = Application.version;

            // req.device_code = _account.deviceId;
            // req.server_id = "0";
            YuziHttp.HttpPost<RS_Login>(RQ_Login.url, req, (RS_Login res) =>
            {
                if (res == default || res == null || res.code != 200)
                {
                    Debug.LogError($"登入失败，重新登录");
                    clearAccount();
                    callback(true);
                    return;
                }
                _account.Token = res.result.token;
                YuziHttp.token = _account.Token;
                _account.Save();
                Debug.Log($"----鱼子登录成功----userId = " + _account.userId);
                Debug.Log($"----token = " + _account.Token);
                // UpLoadConfig();
                callback.Invoke(true);
            });

        }



        /**
         * *******上报***********
         */

        //code:在广告结束时会返回一个code,code=2000成功,其他失败
        public void ReportAd(int adId, string key, int code = 0)
        {
            if (!isOpenReport) return;

            var table = YuziTable.Instance.T_Ad(adId);
            if (table == null) { return; }
            
            var tReport = YuziTable.Instance.T_Report(table.reportStart);
            if(tReport == null){return;}
            
            var tReportInfo = YuziTable.Instance.T_ReportInfo(tReport.childId);
            if(tReportInfo == null){return;}

            string[] adPositionList = tReportInfo.name.Split("_");
            var param = new ReportParam.Ad();
            param.AdId = adId;
            param.Position = adId + "";
            param.PositionType = adId + "";
            switch (key)
            {
                case ReportKey.ad_load:
                case ReportKey.ad_skip:
                case ReportKey.ad_btn_show:
                case ReportKey.ad_btn_click:
                case ReportKey.ad_end:
                    _sdk.Report.ReportCustom(key, param);
                    break;
                case ReportKey.ad_load_result:
                    param.AdId = code;
                    _sdk.Report.ReportCustom(key, param);
                    break;
                case ReportKey.ad_show:
                    _sdk.Report.ReportCustom(key, param);
                    ReportYuziAd(table.reportStart);
                    break;
                case ReportKey.ad_success:
                    _sdk.Report.ReportCustom(key, param);
                    ReportYuziAd(table.reportEnd);
                    break;
                case ReportKey.ad_fail:
                    param.err_code = code;
                    _sdk.Report.ReportCustom(key, param);
                    ReportYuziAd(table.reportError);
                    break;
            }
        }

        /** 上报广告相关 */
        private void ReportYuziAd(int reportId)
        {
            if (!isOpenReport || !isOpenYuziReport) return;
            var table = YuziTable.Instance.T_Report(reportId);
            if (table == null) { Debug.LogError("不存在广告上报id:" + reportId); return; }
            Dictionary<int, object> req = new Dictionary<int, object>()
            {{
                    table.type, new Dictionary<int, int>() {{table.mainId, table.childId}}
            }};
            Debug.Log("上报鱼子广告打点:" + table.des);
            YuziHttp.HttpPostWithToken(RQ_AD_URL, req, null);
        }


        /** 上报引导 guidId:引导id*/
        public void ReportGuide(int guidId, string name)
        {
            if (!isOpenReport) return;
            // int mainGuideId = guidId / 100 * 100;
            //
            // var tMainGuideReport = YuziTable.Instance.T_ReportInfo(mainGuideId);
            // var tSubGuideReport = YuziTable.Instance.T_ReportInfo(guidId);
            // var tReport = YuziTable.Instance.T_Report(5000);
            // if (null == tMainGuideReport || null == tSubGuideReport || null == tReport)
            // {
            //     Debug.LogWarning("没有设置引导上报<MainGuideId>：" + mainGuideId + ", <SubGuideId>：" + guidId);
            //     return;
            // }
            // // int type = 500000;//YuziTable.Instance.reportTable[5000].type;
            //
            // // req = {500000:{5xx000:主id信息, 5000xx:子id信息}}
            // Dictionary<int, object> req = new Dictionary<int, object>()
            // {{
            //     tReport.type, new Dictionary<int, string>() 
            //     {
            //         {mainGuideId, tMainGuideReport.name} ,
            //         {guidId, tSubGuideReport.name} 
            //     }
            // }};
            // YuziHttp.HttpPostWithToken(RQ_GAME_URL,req,null);
            //sdk上报
            ReportParam.Guide sdkInfo = new ReportParam.Guide();
            sdkInfo.guideId = guidId;
            sdkInfo.des = name;
            _sdk.Report.ReportCustom(ReportKey.GUIDE, sdkInfo);
            Debug.Log($"引导上报:{guidId},{name}");

        }

        // {300000:{30x000:参数, 3000xx:参数}}
        public void ReportNormal(int mainId,Object param1,Object param2)
        {
            var tReport = YuziTable.Instance.T_Report(mainId);
            Dictionary<int, object> req = new Dictionary<int, object>()
            {{
                tReport.type, new Dictionary<int, string>() 
                {
                    {tReport.mainId, param1.ToString()} ,
                    {tReport.childId, param2.ToString()} 
                }
            }};
            YuziHttp.HttpPostWithToken(RQ_GAME_URL,req,null);

        }

        /** 上报自定义打点
         * reportId:配置表TD_report的id
         * reportParam:与配置表中param相对应,数字id换成值
         */
        public void ReportCustom(int reportId, Dictionary<string, object> reportParam)
        {
            if (!isOpenYuziReport || !isOpenReport) return;

            var tReport = YuziTable.Instance.T_Report(reportId);
            if (tReport == null)
            {
                Debug.LogError("没有设置自定义上报：" + reportId);
                return;
            }

            // req = {70x000:{7000xx:参数, 7000xx:参数}} 和 {70x000:{}}

            Dictionary<int, object> param = new Dictionary<int, object>();
            if (reportParam != null)
            {
                if (tReport.param == null)
                {
                    Debug.LogError("自定义上报参数param不存在：" + reportId);
                    return;
                }
                foreach (var kv in reportParam)
                {
                    if (!tReport.param.ContainsKey(kv.Key)) continue;
                    int key = tReport.param[kv.Key];
                    object val = kv.Value;
                    param.Add(key, val);
                }
            }
            Dictionary<int, object> req = new Dictionary<int, object>() { { tReport.mainId, param } };
            YuziHttp.HttpPostWithToken(RQ_CUSTOM_URL, req, null);
            Debug.Log($"自定义上报:{reportId}");
        }

    }

}