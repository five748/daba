using System;
using System.Collections.Generic;

[Serializable]
public class DataAD:ReadAndSaveTool<DataAD>
{
    private static string save_path = "ad";
    public int dayend;//每天结束时间
    public int zanzhuTime;//赞助出现时间
    
    //领奖
    public int day_adnum;//每天领奖看广告次数
    public int day_index = 1;//每天领奖下标
    public int day_time;//每天领奖下次可领取时间
    public List<int> day_reward1 = new List<int>();//领奖广告已领取奖励
    public List<int> day_reward2 = new List<int>();//每天领奖累计次数已领取奖励
    
    //福利
    public int[] attrEndtime = new []{0,0};//buff时间结束1
    public int fuli_adnum;//福利广告次数
    public List<int> fuli_reward = new List<int>();//福利已领取奖励


    
    //新的一天
    public void CheckNewDay()
    {
        var now = TimeTool.SerNowUtcTimeInt;
        if (now >= dayend)
        {
            day_adnum = 0;
            day_index = 1;
            day_time = 0;
            day_reward1.Clear();
            day_reward2.Clear();

            fuli_adnum = 0;
            fuli_reward.Clear();
            dayend = TimeTool.GetTodayZeroTimeInt();
            Save();
        }
    }
    
    
    public static DataAD Read()
    {
        return ReadByPhone(save_path, () =>{return new DataAD();});
    }
    
    public void Save()
    {
        WriteInPhone(this, save_path);
    }
}