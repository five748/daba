GameGlobal.TMSDK = {
    moduleName: "TMSDK",

    isInitShow: false,
    systemInfo: null,

    init() {
        return Promise.resolve()
            .then(() => {
                if (this.systemInfo) return this.systemInfo;

                this.systemInfo = wx.getWindowInfo();
                return this.systemInfo;

                // return wx.getSystemInfoAsync()
                //     .then(res => {
                //         console.log('getSystemInfoAsync', res);
                //         this.systemInfo = res;
                //         return res;
                //     });
            });
    },

    // 原生模板
    adUnitIds: [
        "adunit-be1a9dcc62cda88c", // 首页_左单个
        "adunit-41dd439471754541", // 升级页_右单个
        "adunit-eaf4bd9b847ef103", // 升级页_左多个
        "adunit-906e6457b2fbbb8c", // 救我页_左多个
        "adunit-a2bbfee7bb31cdc3", // 救我页_右多个
        "adunit-fc0202e088b6c929", // 失败页_左多个
        "adunit-af9ddb92a08467f7", // 失败页_右多个
        "adunit-e45b145d71bee254", // 暂停页_左多个
        "adunit-40450016da182682", // 暂停页_右多个
        "adunit-40d8b56393eb25b2", // 结算页_左多个
        "adunit-a7c79a3e241da920" // 结算页_右多个
    ],
    adUnitIdsConfig: [
        { id: 0, left: 30, top: 50 },
        { id: 1, left: 0, top: 0, right: 40, bottom: 50 },
        { id: 2, left: 30, top: 40 },
        { id: 3, left: 30, top: 40 },
        { id: 4, left: 0, top: 40, right: 40, bottom: 0 },
        { id: 5, left: 30, top: 40 },
        { id: 6, left: 0, top: 40, right: 40, bottom: 0 },
        { id: 7, left: 30, top: 40 },
        { id: 8, left: 0, top: 40, right: 40, bottom: 0 },
        { id: 9, left: 30, top: 40 },
        { id: 10, left: 0, top: 40, right: 40, bottom: 0 }
    ],

    // 激励视频
    videoAdIds: ['adunit-b7f307d0f73e65ce'],

    // banner
    bannerIds: [
        'adunit-bae7073f5017ac9b', // 失败页
        'adunit-c7ee5f9e7356e394', // 暂停页
        'adunit-576e8d0eb51400d5', //结算页
        'adunit-c7ee5f9e7356e394', // 游戏内常驻
    ],

    adUnitObj: {},
    videoAdObj: {},
    bannerIdObj: {},
    bannerIdObjShow: {},
    sendGuideEvent(_isBeginOrOver, _guideName, _guideId) {
        wx.tmSDK.sendEvent('mainTask', {
            taskdes: _guideId + ":" + _guideName + ":" + _isBeginOrOver,
            taskId: _guideId
        });
    },
    syncPlayerInfo(_trackingId, _g_uid, _g_nickname, _g_zone, _g_level, _g_vip_level) {
        wx.tmSDK.syncPlayerInfo({
            trackingId: _trackingId,
            g_uid: _g_uid,
            g_nickname: _g_nickname,
            g_zone: _g_zone,
            g_level: _g_level,
            g_vip_level: _g_vip_level
        })
            .then(res => {
                console.log(res);
            })
            .catch(err => {
                console.error(err);
            });
    },
    CreateCustomAd(id, left, top, right, bottom) {
        this.systemInfo = wx.getWindowInfo();
        if (!id && id !== 0) return console.log('adUnitId 必填');
        if (!this.systemInfo) {
            return this.init().then(() => this.CreateCustomAd(id, left, top, right, bottom));
        }
        var nLeft = left, nTop = top + 10;
        if (nLeft < this.systemInfo.safeArea.left) {
            nLeft = this.systemInfo.safeArea.left + 10;
        }
        if (right) {
            nLeft = this.systemInfo.safeArea.width - right - 30;
        }
        if (bottom) {
            nTop = this.systemInfo.safeArea.height - bottom - 70;
        }

        if (this.adUnitObj[id]) {
            this.adUnitObj[id].show()
                .then(() => {
                    console.log("显示成功！");
                }).catch((res) => {
                    console.log("显示异常" + JSON.stringify(res));
                });
            return;
        };
        console.log('CreateCustomAd', id, left, top, right, bottom)
        this.adUnitObj[id] = wx.createCustomAd({
            adUnitId: this.adUnitIds[id],
            adIntervals: 30,
            style: {
                left: nLeft,
                top: nTop
            }
        });

        this.adUnitObj[id].onLoad(() => {
            console.log("wx积木广告加载成功！");
            // this.adUnitObj[id].show()
            //     .then(() => {
            //         console.log("显示成功！");
            //     }).catch((res) => {
            //         console.log("显示异常" + JSON.stringify(res));
            //     });
        });

        this.adUnitObj[id].onError((res) => {
            this.adUnitObj[id] = null;
            // this.CreateCustomAd(id, left, top, right, bottom);
            console.log("积木广告加载失败！" + res.errMsg + ">>" + res.errCode);
        });

        return id;
    },
    HideCustomAd() {
        if (JSON.stringify(this.adUnitObj) !== "{}") {
            for (const key in this.adUnitObj) {
                if (this.adUnitObj[key]) {
                    this.adUnitObj[key].hide();
                }
            }
        }
    },

    _videoCallback: null,
    _isVideoShow: false,
    CreateRewardedVideoAd(id) {
        //if (!id && id !== 0) return console.log('adUnitId 必填');

        if (!this.videoAdObj[id]) {
            this.videoAdObj[id] = wx.tmSDK.createRewardedVideoAd({
                adUnitId: id
            });
        }

        if (this.videoAdObj[id]) {
            this.videoAdObj[id].onLoad(res => {
                console.error('onLoad', id, res);
                if (this._isVideoShow) {
                    this.videoAdObj[id].show().then(() => {
                    }).catch(() => {
                        if (this._videoCallback) {
                            this._videoCallback(this.moduleName, "onRewardedVideoAd", JSON.stringify({ status: false, msg: "暂无视频广告！" }));
                            this._videoCallback = null;
                        }
                    });
                }
            });

            this.videoAdObj[id].onError((err) => {
                console.error("激励视频加载失败!", err.code, err.msg);
                if (this._videoCallback) {
                    this._videoCallback(this.moduleName, "onRewardedVideoAd", JSON.stringify({ status: false, msg: "暂无视频广告！" }));
                    this._videoCallback = null;
                }
            });

            this.videoAdObj[id].onClose((res) => {
                console.error('onClose');
                this._isVideoShow = false;
                if (res && res.isEnded || res === undefined) {
                    console.error('succ');
                    // 正常播放结束，可以下发游戏奖励
                    if (this._videoCallback) {
                        this._videoCallback(this.moduleName, "onRewardedVideoAd", JSON.stringify({ status: true, msg: "", id: id }));
                        this._videoCallback = null;
                    }
                } else {
                    // 播放中途退出，不下发游戏奖励
                    console.error('fail');
                    if (this._videoCallback) {
                        this._videoCallback(this.moduleName, "onRewardedVideoAd", JSON.stringify({ status: false, msg: "观看完视频才能获得奖励！" }));
                        this._videoCallback = null;
                    }
                }
            });

        } else {
            if (this._videoCallback) {
                this._videoCallback(this.moduleName, "onRewardedVideoAd", JSON.stringify({ status: false, msg: "暂无视频广告！" }));
                this._videoCallback = null;
            }
        }
    },
    ShowRewardedVideoAd(id) {
        this._videoCallback = GameGlobal.Module.SendMessage;
        this._isVideoShow = true;
        if (this.videoAdObj[id]) {
            console.error('onShow:' + id);
            this.videoAdObj[id].show()
                .then(() => {
                })
                .catch((err) => {
                    this.videoAdObj[id].load();
                    this._videoCallback = GameGlobal.Module.SendMessage;
                });
        } else {
            console.error('onCreate');
            this.CreateRewardedVideoAd(id);
        }
    },

    bannerShow: false,
    _loadBanner(AdId) {
        console.error('_loadBanner', this.systemInfo);

        this.systemInfo = wx.getWindowInfo();
        let width = 300;
        let height = 70;
        let left = (this.systemInfo.screenWidth - width) / 2;
        let top = this.systemInfo.screenHeight - height - 35;

        console.log(left, top, '位置信息')

        // if (this.bannerIdObj[AdId]) return;

        this.bannerIdObj[AdId] = wx.tmSDK.createBannerAd({
            adUnitId: this.bannerIds[AdId],
            adIntervals: 30,
            style: {
                top: top,
                left: left,
                width: width
            },
        });
        console.log('加载banner', this.bannerShow)

        //监听
        this.bannerIdObj[AdId].onLoad(() => {
            if (this.bannerShow) {
                this.bannerIdObj[AdId].show();
            }
        })

        this.bannerIdObj[AdId].onError((err) => {
            console.log(err, "banner组件加载失败");
        });
    },
    ShowBanner(AdId = null) {
        this.systemInfo = wx.getWindowInfo();
        if (AdId == 3 && this.systemInfo.screenWidth < 800) {
            this.HideBanner();
            return;
        };
        if (AdId == 3 && !this.JudgeConfig[101812].status) {
            this.HideBanner();
            return;
        };

        this.showBannerID = AdId !== null ? AdId : 0;
        console.log('this.showBannerID', this.showBannerID);
        this.bannerShow = true;
        if (this.bannerIdObj[this.showBannerID]) {
            if (this.bannerIdObjShow[this.showBannerID]) return;
            this.bannerIdObj[this.showBannerID].show();
            this.bannerIdObjShow[this.showBannerID] = true;
        } else {
            this._loadBanner(this.showBannerID);
        }
    },
    HideBanner(AdId = null) {
        console.log('隐藏banner', AdId)
        this.bannerShow = false;
        if (AdId !== null) {
            if (this.bannerIdObj[AdId]) {
                this.bannerIdObj[AdId].hide();
                this.bannerIdObjShow[AdId] = false;
            }
        } else {
            for (const key in this.bannerIdObj) {
                if (this.bannerIdObj[key]) {
                    this.bannerIdObj[key].hide();
                    this.bannerIdObjShow[key] = false;
                }
            }
        }
    },

    JudgeConfig: {},
    GetJudgeConfig(id) {
        return this.JudgeConfig[id].status || 0;
    }
};


wx.onShow(() => {
    console.log('onshow TMSDK');

    wx.tmSDK.init({
        hideRequestLog: false,
        appVersion: '1.0.9'
    });

    GameGlobal.TMSDK.init()
        .then(res => {
            // 提前加载原生模板
            if (!GameGlobal.TMSDK.isInitShow) {
                GameGlobal.TMSDK.isInitShow = true;
                //GameGlobal.TMSDK.adUnitIdsConfig.forEach((item, index) => {
                //    GameGlobal.TMSDK.CreateCustomAd(item.id, item.left, item.top, item.right, item.bottom);
                //});

                // 提前加载banner
                //GameGlobal.TMSDK.bannerIds.forEach((item, index) => {
                //    GameGlobal.TMSDK._loadBanner(index);
                //});

                // 提前加载激励视频
                //GameGlobal.TMSDK.videoAdIds.forEach((item, index) => {
                //    GameGlobal.TMSDK.CreateRewardedVideoAd(index);
                //});

                wx.tmSDK.getJudgeConfig().then(res => {
                    console.log('getJudgeConfig', res);
                    GameGlobal.TMSDK.JudgeConfig = res;
                });
            }
        });

    wx.tmSDK.onShareAppMessage(function () {
        return {
            scene: 'regular', // 必填，分享位ID
            success: function () { },
            cancel: function () { }
        }
    });
});