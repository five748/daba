using System;

namespace YuZiSdk.Base
{
    public interface ISdk_RewardAd
    {
        public virtual void ShowAd(int adId,Action<bool> callback){}


        public virtual void LoadAd(){}


    }
}