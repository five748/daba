using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TreeData;

public class UIDYOutSideTip:BaseMonoBehaviour{
    private UIDYOutSideTipAuto Auto = new UIDYOutSideTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
#if DY
        var tt = StarkSDKSpace.StarkSDK.API.GetStarkSideBarManager();
        tt.NavigateToScene(StarkSDKSpace.StarkSideBar.SceneEnum.SideBar,
                () =>
                {
                    //succ
                    Debug.LogError("succ");
                },
                () =>
                {
                    //complete
                    Debug.LogError("complete");
                },
                (index, str) =>
                {
                    Debug.LogError(index + " :" + str);
                });
#endif
    }
    public void ClickBtnaward(GameObject button){
        Debug.Log("click" + button.name);
        foreach (var item in awards)
        {
            PlayerMgr.Instance.AddItemNum(item.id, item.num);
        }
        isGetRewardSer = true;
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public static UIDYOutSideTip Instance;
    public static bool isGetRewardSer
    {
        get
        {
            return PlayerMgr.Instance.data.isGetRewardDY;
        }
        set {
            PlayerMgr.Instance.data.isGetRewardDY = value;
        }
    }
    public static bool isOpenByOutSide {
        get { 
            return PlayerMgr.Instance.data.isOpenByOutSide;
        }
        set {
            PlayerMgr.Instance.data.isOpenByOutSide = value;
        }
    }
    private void Init(string param){
        Instance = this;
        UIManager.FadeOut();
        SetAwardBtn();
        ShowAward();
    }
    public void SetAwardBtn() {
        Auto.Btn.SetActive(!isOpenByOutSide);
        Auto.Btnaward.SetActive(isOpenByOutSide);
    }
    private List<item> awards;
    private void ShowAward() {
        var awardStr = TableCache.Instance.configTable[902].param;
        awards = awardStr.ToItems();
        var newAwards = new List<item>();
        int modify = 1;
        if (!TableCache.Instance.progressCoeTable.ContainsKey(PlayerMgr.Instance.data.score))
        {
            Debug.LogError("分数必须为5的倍数|分数:" + PlayerMgr.Instance.data.score);
        }
        else
        {
            modify = TableCache.Instance.progressCoeTable[PlayerMgr.Instance.data.score].coe;
        }
      
        foreach (var item in awards)
        {
            if (item.id == 1)
            {
                newAwards.Add(new item()
                {
                    id = item.id,
                    num = item.num * modify,
                });
            }
            else {
                newAwards.Add(new item()
                {
                    id = item.id,
                    num = item.num,
                });
            }   
        }
        awards = newAwards;
        ShowAward(Auto.Grid, awards);
    }
    public void ShowAward(Transform tran, List<TreeData.item> awards)
    {
        var aItems = tran.AddChilds(awards.Count);
        int index = -1;
        awards.ForEach((item) => {
            index++;
            var aitem = aItems[index];
            aitem.SetImage("icon/" + item.id, "icon");
            aitem.SetText(TableCache.Instance.itemTable[item.id].initCost, "name");
            aitem.SetText("x" + item.num, "num");
            //aitem.AddIconInfoClick(item.id);
        });
    }
   
    public static void OnShowOneParam(Dictionary<string, object> param)
    {
        Debug.Log(param);
        string from = "";
        string location = "";
        if (param.ContainsKey("launch_from"))
        {
            from = param["launch_from"].ToString();
        }
        if (param.ContainsKey("location"))
        {
            location = param["location"].ToString();
        }
        if (from == "homepage" && location == "sidebar_card")
        {
            isOpenByOutSide = true;
        }
        if (isOpenByOutSide)
        {
            if (Instance != null)
            {
                DYOutSide.Instance.Init();
            }
            if (UIDYOutSideTip.Instance != null)
            {
                if (!isGetRewardSer)
                {
                    UIDYOutSideTip.Instance.SetAwardBtn();
                }
            }
        }
    }
}


