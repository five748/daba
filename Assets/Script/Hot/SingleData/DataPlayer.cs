

using System;
using System.Collections.Generic;
using System.Diagnostics;
using TreeData;
using LitJson;

[Serializable]
public class DataPlayer : ReadAndSaveTool<DataPlayer>
{
    //public bool IsLoadResOver;
    public bool isGetRewardDY;
    public bool isOpenByOutSide;
    private static string _savePath = "player";

    public Dictionary<int, long> items = new();
    public Dictionary<int, List<int>> fixItems = new();
    public int score = 5;
    public int ChooseMemberMenuId = 1;
    public int ChooseStaffMenuId = 1;

    public bool IsHaveCleanShip;
    public int CleanIndex = 1;
    public Dictionary<int, int> cleanItems = new Dictionary<int, int>();
    public List<int> cleanOverItems = new List<int>();
    //藏品
    public List<int> collectLst = new List<int>();
    public int dayCollectIndex;
    public bool isCollectCanGet;
    public int collectAwardEndTime;
    public int collectTodayTime;
    public int collectId;
    public int collectCount;

    //招商
    public Dictionary<int, int[]> zhaoshang = new Dictionary<int, int[]>();//{大坝Id:[解锁数量,招商Id,招商品质,招商倒计时]}
    public Dictionary<int, int[]> zhaoshangtype = new Dictionary<int, int[]>();//{大坝Id:[招商类型1,招商品质1,招商类型2,招商品质2,招商类型3,招商品质3]}
    public List<int> pintu = new List<int>();//装卸货物数据
    public List<int> jiaotong = new List<int>();//指挥交通数据
    //成就
    public Dictionary<int, int> OnAchId = new Dictionary<int, int>();
    public int OutLineTime;
    public int MaxOutLineTime;
    [LitJson.JsonIgnore]
    public int SumOpenShipNum
    { //累计解锁船只数量
        get
        {
           
            int index = 0;
            foreach (var item in ShipMgr.Instance.data.ships)
            {
                if (item.Value.unlock) {
                    index++;
                }
            }
            return index;
        }
    }
    public int SumCleanShipNum; //清洗河道次数达到
    public int SumCollectNum;   //获得藏品数量
    public int SumOverDamNum;   //累计过闸数量
    public int SumStaffLvNum;   //职员总等级达到
    public static DataPlayer ReadFromFile()
    {
        return ReadByPhone(_savePath, () =>
            { 
                var data = new DataPlayer();
                foreach (var tItem in TableCache.Instance.itemTable)
                {
                    var item = new item();
                    item.id = tItem.Value.id;
                    item.num = tItem.Value.initNum;
                    data.items.TryAdd(tItem.Value.id, item.num);
                }
                foreach (var item in TableCache.Instance.achievementTable)
                {
                    if (!data.OnAchId.ContainsKey(item.Value.typeId)) {
                        data.OnAchId.Add(item.Value.typeId, item.Key);
                    }
                }
                data.MaxOutLineTime = int.Parse(TableCache.Instance.configTable[401].param);
                return data;
            }
        );
    }
    public void SaveToFile()
    {
        WriteInPhone(this, _savePath);
    }
}
