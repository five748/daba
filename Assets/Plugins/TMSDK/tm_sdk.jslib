mergeInto(LibraryManager.library, {

    TM_SDK_Init:function() {
       GameGlobal.TMSDK.init();
    },

    TM_SDK_syncPlayerInfo:function(trackingId,g_uid,g_nickname,g_zone,g_level,g_vip_level)  {
         GameGlobal.TMSDK.syncPlayerInfo(trackingId,Pointer_stringify(g_uid),Pointer_stringify(g_nickname),Pointer_stringify(g_zone),Pointer_stringify(g_level),Pointer_stringify(g_vip_level));
    },
    TM_SDK_sendGuideEvent:function(isBeginOrOver, guideName, guideId)  {
         GameGlobal.TMSDK.sendGuideEvent(Pointer_stringify(isBeginOrOver), Pointer_stringify(guideName), Pointer_stringify(guideId));
    },


    TMPointer_stringify_adaptor:function(str){
        console.log('TM_SDK_INIT jslib TMPointer_stringify_adaptor', str);
        if (typeof Pointer_stringify !== "undefined") {
            return Pointer_stringify(str)
        }
        return Pointer_stringify(str)
    },

    TM_SDK_GetJudgeConfig: function(id) {
        console.log("TM_SDK_GetJudgeConfig", id);
        return GameGlobal.TMSDK.GetJudgeConfig(id);
    },

    TM_SDK_SENDLOADINGLOG: function(str) {
        wx.tmSDK.sendLoadingLog(Pointer_stringify(str));
    },

    TM_SDK_CreateCustomAd: function(id, left, top, right, bottom) {
        GameGlobal.TMSDK.CreateCustomAd(id, left, top, right, bottom);
    },

    TM_SDK_HideCustomAd: function() {
        GameGlobal.TMSDK.HideCustomAd();
    },

    TM_SDK_CreateRewardedVideoAd: function(id) {
        GameGlobal.TMSDK.CreateRewardedVideoAd(Pointer_stringify(id));
    },

    TM_SDK_ShowRewardedVideoAd: function(id) {
        GameGlobal.TMSDK.ShowRewardedVideoAd(Pointer_stringify(id));
    },

    TM_SDK_ShowBanner: function(id) {
        console.log("TM_SDK_ShowBanner", id);
        GameGlobal.TMSDK.ShowBanner(id);
    },

    TM_SDK_HideBanner: function(id) {
        console.log("TM_SDK_HideBanner", id);
        GameGlobal.TMSDK.HideBanner(id);
    }
});
