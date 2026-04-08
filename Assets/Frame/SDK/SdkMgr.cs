
using System;
using System.Collections;
using YuZiSdk.Base;
using UnityEngine;
#if WX
using WeChatWASM;
#endif
using System.Collections.Generic;
using GameVersionSpace;
using Unity.VisualScripting;
#if DY
using StarkSDKSpace;
#endif
using UnityEngine.Windows;

namespace YuZiSdk
{
    public class SdkMgr : Single<SdkMgr>
    {
        private ISdk _sdk;
        public ISdk Sdk { get => _sdk; }


        public SdkConfig sdkConfig;
        private SDKType _sdkType;

        private bool isInit = false;


        public SdkMgr()
        {

            ReadSDKConfig();
#if UNITY_ANDROID
            sdkConfig.os = "Android";  
#endif
#if UNITY_IOS
            sdkConfig.os = "IOS";  
#endif
#if UNITY_EDITOR
            _sdk = new EmptySdk.EmptySdk();
            sdkConfig.os = "window";
            sdkConfig.platform = "unity";
#elif WX
            _sdk = new EmptySdk.EmptySdk();
            sdkConfig.platform = "微信小游戏";
#elif DY
            _sdk = new EmptySdk.EmptySdk();
            sdkConfig.platform = "抖音小游戏";
#else
           _sdk = new EmptySdk.EmptySdk();
#endif
        }
        private void ReadSDKConfig()
        {
            var sdkCache = SdkCache.ReadData();
            _sdkType = sdkCache.ChooseSDK;
            sdkConfig = new SdkConfig();
        }
        //游戏初始化,完成后回调显示登录按钮
        public void Init(Action<bool> callback = null)
        {
            if (isInit)
            {
                callback.Invoke(true);
                return;
            }
            _sdk.Init(isSuccess =>
            {
                if (!isSuccess)
                {
                    isInit = false;
                    callback?.Invoke(false);
                    return;
                }
                isInit = true;
                _sdk.InitSdk();
                _sdk.Report.ReportGameProcess(EnumGameProcess.eLaunch);
                _sdk.Ad.LoadAd();
#if  WX && !UNITY_EDITOR
                GetWXOpenId(s =>
                {
                    sdkConfig.deviceId = s;
                    callback.Invoke(true);

                });
                return;
#endif
                Debug.Log("sdk初始化成功");
                callback.Invoke(true);
            });
        }

        public void ActiveYuzi(Action<bool> callback)
        {
            YuziMgr.Instance.UserActive(sdkConfig.deviceId, sdkConfig.Uid,callback);
        }
        //点击登录按钮调用,完成后回调显示加载进度条,初始化游戏数据
        public void Login(Action<bool> callback)
        {
            if (!isInit)
            {
                Init(b =>
                {
                    if (b)
                    {
                        Login(callback);
                    }
                    else
                    {
                        Msg.Instance.Show("Sdk初始化失败");
                        callback.Invoke(false);
                    }
                });
                return;
            }
            //实名
            _sdk.Report.ReportGameProcess(EnumGameProcess.eRealName);
            _sdk.RealName(isSuccess =>
            {
                if (!isSuccess)
                {
                    Msg.Instance.Show("实名检测失败");
                    callback.Invoke(false);
                    return;
                }
                YuziMgr.Instance.UserRegister(callback);

            });
        }
        private System.Action<bool> _AdCallback;
        private void AdCallBack(bool isSuscc) {
            MusicMgr.Instance.BeginAll();
            _AdCallback(isSuscc);
        }
        //播放广告
        public void ShowAd(int adId, Action<bool> call, bool skip = false)
        {
// #if !(WX || DY)
//             call.Invoke(true);
//             return;
// #endif

            if (skip)
            {
                call.Invoke(true);
                return;
            }
            YuziMgr.Instance.ReportAd(adId, ReportKey.ad_btn_click);
            if (!GameProcess.Instance.IsHaveConnet)
            {
                Msg.Instance.Show("暂无网络，请重新连接");
                call.Invoke(false);
                return;
            }
            MusicMgr.Instance.PauseAll();
            _AdCallback = call;
            call = AdCallBack;
#if TMSDK || DYTMSDK
            TMSDK.Instance.ShowRewardedVideoAd(adId, HotMgr.Instance.buildCache.WXAdId, (data) => {
                if (data == null) {
                    call.Invoke(false);
                    return;
                }
                if (data.status)
                {
                    Debug.LogError("AdSucc");
                    call.Invoke(true);
                }
                else {
                    Debug.LogError("AdFail");
                    call.Invoke(false);
                }
            });
            return;
#endif

#if DY && !UNITY_EDITOR
            ShowDYAD(adId, call);    
            return;
#endif
#if WX && !UNITY_EDITOR
            if (string.IsNullOrEmpty(HotMgr.Instance.buildCache.WXAdId)) {
                Msg.Instance.Show("广告未开放!");
                call.Invoke(false);
                return;
            }

            WeChatWASM.WXRewardedVideoAd backToLifeVideoAd = WeChatWASM.WX.CreateRewardedVideoAd(
                new WXCreateRewardedVideoAdParam()
                {
                    adUnitId = HotMgr.Instance.buildCache.WXAdId,
                    multiton = true
                });
            if (backToLifeVideoAd != null)
            {
                YuziMgr.Instance.ReportAd(adId,ReportKey.ad_show);
                backToLifeVideoAd.Show();
            }
            else
            {
                call(false);
                return;
            }
            backToLifeVideoAd.OnClose((WeChatWASM.WXRewardedVideoAdOnCloseResponse res) =>
            {
                if ((res != null && res.isEnded) || res == null)
                {
                    // 正常播放结束，可以下发游戏奖励
                    YuziMgr.Instance.ReportAd(adId,ReportKey.ad_success);
                    call(true);
                }
                else
                {
                    // 播放中途退出，不下发游戏奖励
                    YuziMgr.Instance.ReportAd(adId,ReportKey.ad_fail);
                    call(false);
                }
            });
            Msg.Instance.Show("广告未开放!");
            call.Invoke(false);
            return;
#endif
            _sdk.Ad.ShowAd(adId, (bool s) =>
            {
                call.Invoke(s);
            });
        }
#if DY
        private StarkAdManager DYAdMgr;
        private void ShowDYAD(int adId, System.Action<bool> callback) {
            if (DYAdMgr == null) {
                DYAdMgr = StarkSDK.API.GetStarkAdManager();
                //StarkAdManager.IsShowLoadAdToast = true;
            }
            DYAdMgr.ShowVideoAdWithId(HotMgr.Instance.buildCache.DYAdId, (iSucc) =>
            {
                callback.Invoke(iSucc);
                YuziMgr.Instance.ReportAd(adId, ReportKey.ad_success);
            }, (index, str) => {
                Debug.LogError("视频播放错误:" + index + ":" + str);
                callback.Invoke(false);
                YuziMgr.Instance.ReportAd(adId, ReportKey.ad_fail);
                }
             );
        }
#endif
        public void IsHaveSensitiveWords(string str, System.Action callback)
        {
#if DY
            StarkSDKSpace.StarkSDK.API.ReplaceSensitiveWords(str, (index, str, data) =>
            {
                //Debug.LogError(index);
                //Debug.LogError(data);
                if (index != 0)
                {
                    callback();
                    return;
                }
                if (data.ContainsKey("audit_result"))
                {
                    var haveStr = data["audit_result"].ToString();
                    if (haveStr == "1") {
                        Msg.Instance.Show("该输入含有敏感字, 请重新输入!");
                        return;
                    }
                }
                callback();
            });
#else
            callback();
#endif
        }
        public bool IsClick = false;
        public void OpenWXMes()
        {
#if WX
            WX.GetSetting(new GetSettingOption()
            {
                withSubscriptions = true,
                success = (res) =>
                {
                    Dictionary<string, string> itemSettings = res.subscriptionsSetting.itemSettings;
                    if (itemSettings.ContainsKey("SYS_MSG_TYPE_WHATS_NEW"))
                    {
                        //已经弹出
                        Debug.LogError("已经弹出");
                    }
                    else
                    {
                        Debug.LogError("弹出通知");
                        WX.ShowModal(new ShowModalOption() { 
                            title = "提示",
                            content = "是否开启通知？",
                            success= (res) =>
                            {
                                if (res.confirm)
                                {
                                    OpenWXWindow(null);

                                }
                                else { 
                                    
                                }
                            },
                            fail = (res) =>
                            {

                            }
                        });
                    }
                },
                fail = (res) =>
                {
                    Debug.LogError("GetSetting fail" + JsonUtility.ToJson(res));

                }
            });
#endif
        }
#if WX
        private System.Action WXCallBack;
        public void OpenWXWindow(OnTouchStartListenerResult touchEvent)
        {
            Debug.LogError("弹出");
            var sendEvent = new WeChatWASM.RequestSubscribeSystemMessageOption();
            sendEvent.msgTypeList = new string[] {
                            "SYS_MSG_TYPE_WHATS_NEW"
                         };
            sendEvent.complete = (backData) => {
                Debug.LogError(backData.errMsg);
                WXCallBack();
            };
            sendEvent.success = (backData) => {
                Debug.LogError(backData.errMsg);
                WXCallBack();
            };
            sendEvent.fail = (backData) => {
                Debug.LogError(backData.errMsg);
                WXCallBack();
            };
            WeChatWASM.WX.RequestSubscribeSystemMessage(sendEvent);
            //WX.OffTouchEnd(OpenWXWindow);
        }
        public void GetWXOpenId(System.Action<string> callback)
        {
            LoginOption loginOption = new LoginOption();
            loginOption.success = ((e) =>
            {
                Debug.LogError("openId:" + e.code);
                callback(e.code);
            });
            loginOption.fail = ((e) =>
            {
                Debug.LogError("获取唯一OpenId失败");
            });
            WX.Login(loginOption);
        }
#endif

    }
}