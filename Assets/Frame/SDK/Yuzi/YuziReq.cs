using System.Collections.Generic;

namespace YuZiSdk
{
    public partial class YuziMgr 
    {
        //上报广告
        private const string RQ_AD_URL = "Advert/report_ad";
        //上报游戏
        private const string RQ_GAME_URL = "Game/games_report";
        //上报自定义
        private const string RQ_CUSTOM_URL = "Game/custom_report";
        
        
        public abstract class RS<T>
        {
            public int code;
            public string msg;
            public T result;
        }
        
        
        #region 激活账号

        private class RQ_ActiveAccount
        {
            public const string url = "User/activate";
            public string app_id;
            public string app_name;//app名称
            public string app_platform;//平台名
            public string app_channel;//渠道名
            public string device_code;//设备码
            // public string version_no;//版本号
        }
        
        private class RS_ActiveAccount : RS<RS_ActiveAccount.ActiveAccount>
        {
            internal class ActiveAccount
            {
                public int app_id;
                public string user_id;
                public string open_id;
            }
        }
        
        #endregion

        #region 注册账号

        private class RQ_Register
        {
            public const string url = "User/register";

            public string user_id;//有sdk的用户ID则用，没有则自己生成一个
            public string device_code;//设备码
            public int app_id;//
            // public string version_no;//版本号
        }

        private class RS_Register : RS<RS_Register.Register>
        {
            internal class Register
            {
                public long create_time;
                public string user_id;
                public int app_id;
            }
        }


        #endregion
        
        
        #region 上传配置

        private class RQ_UploadConfig
        {
            public const string url = "User/category";

            public string app_id;
            public List<Dictionary<string, string>> category;
        }

        #endregion
        
        
        #region 登入

        public class RQ_Login
        {
            public const string url = "User/login";

            public int app_id;
            public string user_id;//有sdk的用户ID则用，没有则自己生成一个
            public string device_code;//设备码
            public string version_no;//版本号
            public string server_id;//区服ID，旧版本无此参数，缺省值 0
        }

        private class RS_Login : RS<RS_Login.Login>
        {
            internal class Login
            {
                public string token;
            }
        }
        
        public class RS_Common : RS<object> {}
            
        #endregion

    }

}