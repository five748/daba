namespace YuZiSdk.Base
{
    
    
    public class ISdkReport
    {
    
        public virtual void SetParam(){}//公共参数:初始化设置一次就行，后续不需要再上报
        public virtual void ReportGameProcess(EnumGameProcess key){}//SDK初始化之后上报,上报游戏流程
        public virtual void ReportLogin(string userName, long tiemStamp){}//登录上报
        public virtual void ReportLogout(float online_time){}//登出上报
        public virtual void ReportCustom(string key,ReportParam.IParam data){}//自定义上报
        public virtual ReportCustomData GetCustomData(string key,ReportParam.IParam data){return new ReportCustomData();}//获取自定义上报数据结构

        //上报流程说明
        protected virtual string GetReportGameProcessVal(EnumGameProcess procedureKey)
        {
            switch (procedureKey)
            {
                case EnumGameProcess.eLaunch:
                    return "启动";
                case EnumGameProcess.eReadyRealName:
                    return "准备实名认证";
                case EnumGameProcess.eRealName:
                    return "实名认证";
                case EnumGameProcess.eReadyLogin:
                    return "准备登入";
                case EnumGameProcess.eLogin:
                    return "登入";
                case EnumGameProcess.eInitData:
                    return "初始化数据";
                case EnumGameProcess.eEnterGame:
                    return "进入游戏";
            }

            return null;
        }
    }
}