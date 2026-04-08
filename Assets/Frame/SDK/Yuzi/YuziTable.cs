using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace YuZiSdk
{
    //上报表格数据
    public class YuziTable : Single<YuziTable>
    {
        private Dictionary<int,Report> t_Report = new Dictionary<int, Report>(); //上报表
        private Dictionary<int,ReportInfo> t_ReportInfo = new Dictionary<int, ReportInfo>(); //上报信息表
        private Dictionary<int,Ad> t_Ad = new Dictionary<int, Ad>(); //广告表

        private bool isInit = false;

        public void InitTable(IDictionary report,IDictionary reportinfo,IDictionary ad)
        {

            if(isInit) return;
            ChangeType(report,t_Report);
            ChangeType(reportinfo,t_ReportInfo);
            ChangeType(ad,t_Ad);
            // t_ReportInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int,ReportInfo>>(Newtonsoft.Json.JsonConvert.SerializeObject(reportinfo));
            // t_Ad = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<int,Ad>>(Newtonsoft.Json.JsonConvert.SerializeObject(ad));
            isInit = true;
            // t_Report = JsonUtility.FromJson<Dictionary<int,Report>>(JsonUtility.ToJson(report));
            // t_ReportInfo = JsonUtility.FromJson<Dictionary<int,ReportInfo>>(JsonUtility.ToJson(reportinfo));
            // t_Ad = JsonUtility.FromJson<Dictionary<int,Ad>>(JsonUtility.ToJson(ad));
            
            // t_Report = report as Dictionary<int, Report>;
            // t_ReportInfo = reportinfo as Dictionary<int, ReportInfo>;
            // t_Ad = ad as Dictionary<int, Ad>;
        }
        //上报表
        public Report T_Report(int id)
        {
            if (t_Report == null || !t_Report.ContainsKey(id))
            {
                return null;
            }
            return (Report)t_Report[id];
        }
        
        //上报信息表
        public ReportInfo T_ReportInfo(int id)
        {
            if (t_ReportInfo == null || !t_ReportInfo.ContainsKey(id))
            {
                return null;
            }
            return (ReportInfo)t_ReportInfo[id];
        }
        
        //广告表
        public Ad T_Ad(int id)
        {
            if (t_Ad == null || !t_Ad.ContainsKey(id))
            {
                return null;
            }
            return (Ad)t_Ad[id];
        }

        public Dictionary<int, ReportInfo> GetReportConfig()
        {
            if (t_ReportInfo == null)
            {
                Debug.LogError("上报表格数据未初始化,请在SdkMgr.Login()之前调用InitTable()");
                return null;
            }
            return (Dictionary<int, ReportInfo>) t_ReportInfo;
        }

        private void ChangeType<T>(IDictionary dest,Dictionary<int,T> t) where T : new()
        {
            if (dest == null)
            {
                return;
            }
            t.Clear();
           
            foreach (var k in dest.Keys)
            {
                object obj = dest[k];
                obj.GetType();
                T o = new T();
                FieldInfo[] ps = obj.GetType().GetFields();
                foreach (FieldInfo pi in ps)
                {
                    o.GetType().GetField(pi.Name).SetValue(o,pi.GetValue(obj));
                }
                t.Add((int)k,o);
            }
        }
        
        
        public class Report
        {
            public int id;
            public string eKey;
            public string des;
            public int type;
            public int mainId;
            public int childId;
            public System.Collections.Generic.Dictionary<string,int> param;
        }
        
        
        public class ReportInfo
        {
            public int id;
            public string name;
        }
        
        public class Ad
        {
            public int id;
            public string eKey;
            public string des;
            public int reportStart;
            public int reportEnd;
            public int reportError;
        }
        
    }
}