using System;
using System.Collections.Generic;

namespace YuZiSdk.Base
{
    //公司上报管理基类,实现在  YuziMgr
    public interface ICompany
    {
        /** 激活用户 */
        public virtual void UserActive(string devId, Action<bool> callback){ callback.Invoke(true);}
        
        /** 玩家登录 */
        public virtual void UserLogin(Action<bool> callback){callback.Invoke(true);}
        
        /** 上报广告,code:在广告结束时会返回一个code,code=2000成功,其他失败 */
        public virtual void ReportAd(int adId, string key,int code = 0){}
        
        /** 上报玩家升级升级 */
        public virtual void ReportPlayerLevel(int lv){}

        /** 上报引导 guidId:引导id*/
        public virtual void ReportGuide(int guidId){}
        
        /** 上报自定义打点
         * reportId:配置表TD_report的id
         * reportParam:与配置表中param相对应,数字id换成值
         */
        public virtual void ReportCustom(int reportId, Dictionary<string,object> reportParam){}

    }
}