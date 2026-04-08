using System;

namespace YuZiSdk.Base
{
    public abstract class ISdk
    {
        public ISdk_RewardAd Ad;//激励广告类
        public ISdkReport Report;//sdk上报类
        
        public abstract void Init(Action<bool> callback);//初始化
        public abstract void InitSdk();//初始化后调用,设置sdk的广告及上报类

        public abstract void Login(Action callback);

        public abstract void RealName(Action<bool> callback);//实名检测
        
        public abstract void OpenPrivacyPolicySetting();//打开个人隐私设置弹窗
        
        public abstract void OpenAgeTip();//打开适龄弹窗

    }
}