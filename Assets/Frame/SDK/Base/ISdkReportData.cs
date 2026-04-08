namespace YuZiSdk.Base
{
    public enum EnumGameProcess
    {
        eLaunch,//启动
        eReadyRealName,//准备实名认证
        eRealName,//实名认证
        eReadyLogin,//准备登入
        eLogin,//登入
        eInitData,//初始化数据
        eEnterGame,//进入游戏主界面
    }
    
    //上报类型
    public class ReportKey
    {
        public const string GUIDE = "GUIDE";
        
        public const string ad_load = "REWARD_AD_REQUEST";// 激励广告加载
        public const string ad_load_result = "REWARD_AD_SEND";// 激励广告加载结果
        public const string ad_btn_show = "REWARD_AD_BUTTON_SHOW";// 激励广告按钮显示
        public const string ad_btn_click = "REWARD_AD_BUTTON_CLICK";// 激励广告按钮点击
        public const string ad_show = "REWARD_AD_SHOW";// 激励广告显示
        public const string ad_end = "REWARD_AD_SHOW_FINISH";// 激励广告结束
        public const string ad_success = "REWARD_AD_SHOW_CLOSE";// 激励广告领取奖励
        public const string ad_skip = "REWARD_AD_SHOW_SKIP";// 激励广告跳过
        public const string ad_fail = "REWARD_AD_SHOW_FAIL";// 激励广告失败
        
        public const string prop_change = "MONEY_FLOW";// 道具变更
        
        public const string btn_click = "BUTTON_CLICK";// 按钮点击
        public const string level_up = "PLAYER_LEVEL_UP";//玩家升级
        public const string task = "TASK";//任务完成
    }

    public class ReportCustomData
    {
        public string key;
        public string json;
    }

    
    //上报参数类
    public class ReportParam
    {
        public interface IParam
        {
            
        }
        
        public class Ad : IParam
        {
            public int AdId;
            public string Position = "";//广告位置
            public string PositionType = "";//类型
            public int err_code;
        }
        
        public class Guide : IParam
        {
            public int guideId;
            public string des;
        }
        
        public class PlayerLevel : IParam
        {
            public int level;
            public int before;
        }
        
        public class Task : IParam
        {
            public int tasktype;
            public int taskid;
            public string taskname;
            public string taskdesc;
        }
        
        public class Prop : IParam
        {
            
        }
        
        public class Button : IParam
        {
            
        }

        
    }


}