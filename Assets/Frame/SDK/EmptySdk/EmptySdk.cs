using System;
using YuZiSdk.Base;

namespace YuZiSdk.EmptySdk
{
    public class EmptySdk : ISdk
    {
        public override void InitSdk()
        {
            Ad = new EmptyRewardAd();
            Report = new EmptyReport();
        }

        public override void Init(Action<bool> callback)
        {
            callback.Invoke(true);
        }

        public override void Login(Action callback)
        {
            callback.Invoke();
        }

        public override void RealName(Action<bool> callback)
        {
            callback.Invoke(true);
        }

        
        /// <summary>
        //打开个人隐私设置弹窗
        public override void OpenPrivacyPolicySetting() { }

        //打开适龄弹窗
        public override void OpenAgeTip() { }
    }
}