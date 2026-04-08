using System;
using System.Threading.Tasks;
using YuZiSdk.Base;
using UnityEngine;

namespace YuZiSdk.EmptySdk
{
    public class EmptyRewardAd : ISdk_RewardAd
    {
        public int adId;
        private bool isShow;

        public async void ShowAd(int adId, Action<bool> callback)
        {
            if (isShow)
            {
                return;
            }
            isShow = true;
            Debug.Log("----模拟广告----");
            this.adId = adId;
            YuziMgr.Instance.ReportAd(this.adId,ReportKey.ad_btn_click);
            await Task.Delay(100);
            YuziMgr.Instance.ReportAd(this.adId,ReportKey.ad_show);
            await Task.Delay(100);
            YuziMgr.Instance.ReportAd(this.adId,ReportKey.ad_end);
            await Task.Delay(100);
            YuziMgr.Instance.ReportAd(this.adId,ReportKey.ad_success);
            callback.Invoke(true);
            isShow = false;

        }

    }
}