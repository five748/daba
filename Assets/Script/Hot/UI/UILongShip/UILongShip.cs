using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEditor;
using YuZiSdk;

public class UILongShip:BaseMonoBehaviour{
    private UILongShipAuto Auto = new UILongShipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private Dictionary<int, ShipItem> ShipDatas
    {
        get
        {
            return ShipMgr.Instance.data.ships;
        }
    }
    private int SumOverDamNum
    {
        get
        {
            //return 10000;
            return PlayerMgr.Instance.data.SumOverDamNum;
        }
    }
    private Dictionary<int, ShipItem> newShipDatas;
    private Dictionary<int, Table.boatRace> tab;
    private void Init(string param){
        UIManager.FadeOut();
        Auto.Numtext.text = $"当前累计过闸{SumOverDamNum.ToString().ChangeColor("24d39f")}条船";
        Show();
    }
    public static bool IsHaveRedOut() {
        foreach (var item in TableCache.Instance.boatRaceTable)
        {
            if (ShipMgr.Instance.data.ships[int.Parse(item.Value.unlockBoatId)].unlock) {
                continue;
            }
            if (item.Value.unlockType == 2) {
                continue;
            }
            if(PlayerMgr.Instance.data.SumOverDamNum >= item.Value.param)
            {
                return true;
            }
        }
        return false;
    }
    private void Show() {
        newShipDatas = new Dictionary<int, ShipItem>();
        tab = new Dictionary<int, Table.boatRace>();
        foreach (var item in TableCache.Instance.boatRaceTable)
        {
            var tabOne = TableCache.Instance.shipTable[int.Parse(item.Value.unlockBoatId)];
            newShipDatas.Add(tabOne.id, ShipDatas[tabOne.id]);
            tab.Add(tabOne.id, item.Value);
        }
        //newShipDatas.SortFirstMiddeDown((data) =>
        //{
        //    if (!data.Value.unlock)
        //    {
        //        return -1;
        //    }
        //    if (data.Value.lv >= 5)
        //    {
        //        return 2;
        //    }
        //    return 0;
        //});
        newShipDatas.ForeachForAddItems(Auto.Grid, (k, v, index, tran) =>{
            var tabOne = TableCache.Instance.shipTable[k];
            tran.SetImage("shipLong/" + tabOne.icon, "icon", true);
            tran.SetText(tabOne.toll, "money");
            bool isenough = SumOverDamNum >= tab[k].param;
            bool isAd = tab[k].unlockType == 2;
            if (isAd)
            {
                isenough = true;
            }
            tran.SetText(tab[k].param.ToString().ChangeColor(GetNumColorStr(isenough)) + "艘", "text/num");
            tran.SetActive(!v.unlock, "lock");
            tran.SetActive(v.unlock, "open");
            var openRt = tran.Find("open").GetComponent<RectTransform>();
            //if (v.unlock) {
            //    if (isAd) {
            //        openRt.anchoredPosition = new Vector2(openRt.anchoredPosition.x, 0);
            //    }
            //    else {
            //        openRt.anchoredPosition = new Vector2(openRt.anchoredPosition.x, -210);
            //    }
             
            //}
          
            tran.SetActive(!isAd, "text");
            tran.SetActive(isAd, "lock/adbtn");
            tran.SetActive(!isAd, "lock/btn");
            if (v.unlock)
            {
                tran.Find("icon").GetComponent<Image>().color = Color.white;
            }
            else {
                tran.Find("icon").GetComponent<Image>().color = Color.gray;
            }
           
            if (!v.unlock)
            {
                tran.SetActive(!isenough, "lock/noenough");
                if (!isenough)
                {
                    tran.SetActive(false, "lock/adbtn");
                    tran.SetActive(false, "lock/btn");
                }
                EventTriggerListener.Get(tran.Find("lock/adbtn")).onClick = (btn) =>
                {
                    Debug.Log("Adbtn");
                    SdkMgr.Instance.ShowAd(5, (iSucc) => {
                        if (iSucc)
                        {
                            OpenShip(v);
                        }
                    });
                };
                EventTriggerListener.Get(tran.Find("lock/btn")).onClick = (btn) =>
                {
                    Debug.Log("btn");
                    OpenShip(v);
                };
            }
        });
    }
    private void OpenShip(ShipItem shipData) {
        Msg.Instance.Show("解锁成功!");
        shipData.unlock = true;
       
        UIManager.OpenTipNoText("UIShipUnlock", shipData.ship_id.ToString() + "_0", (str) =>
        {
            UIManager.CloseAllTip();
            ShipMgr.Instance.data.SaveToFile();
            ShipMgr.Instance.FilterUnlockShip();
            _CallBack?.Invoke(str);
        });
    }
    private string GetNumColorStr(bool isEnough)
    {
        if (isEnough)
        {
            return "419ea6";
        }
        else
        {
            return "a64141";
        }
    }
}

