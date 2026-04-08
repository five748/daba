using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UINavigate:BaseMonoBehaviour{
    private UINavigateAuto Auto = new UINavigateAuto();
    
    private int Score {
        get {
            //return 10000;
            return PlayerMgr.Instance.data.score;
        }
    }
    
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickCan_up_capacity(GameObject button){
        Debug.Log("click" + button.name);
        if (NavigateMgr.Instance.CapacityUplevel())
        {
            Msg.Instance.Show($"升级成功");
            update_company();
        }
    }
    public void ClickCan_up_output(GameObject button){
        Debug.Log("click" + button.name);
        if (NavigateMgr.Instance.OutputUplevel())
        {
            var tOutput = TableCache.Instance.warehouseTable[NavigateMgr.Instance.Data.outputLv];
            var tOutputPre = TableCache.Instance.warehouseTable[NavigateMgr.Instance.Data.outputLv - 1];
            var disOutput = (tOutput.output - tOutputPre.output).ToString("0.0");
            Msg.Instance.Show($"产出速度：+{disOutput}集装箱/秒");
            update_company();
        }
    }
    public void ClickBtn_full_ad(GameObject button){
        Debug.Log("click" + button.name);
        NavigateMgr.Instance.FullCargoAd(b =>
        {
            if (b)
            {
                Msg.Instance.Show($"仓库填充满了");
            }
            update_company();
        });
    }
    public void ClickBtn_company(GameObject button){
        Debug.Log("click" + button.name);
        on_company_click();
    }
    public void ClickBtn_order(GameObject button){
        Debug.Log("click" + button.name);
        on_order_click();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public static bool IsGotoOrder;
    private void Init(string param){
        UIManager.FadeOut();
        if (IsGotoOrder)
        {
            on_order_click();
        }
        else {
            on_company_click();
        }
        IsGotoOrder = false;
        update_red();
        NavigateMgr.Instance.FunUpdateUICargo += update_cargo;
        
        GuideMgr.Instance.BindBtn(Auto.Btn_order.transform, tableMenu.GuideWindownBtn.navigate_order);
    }

    public override void Destory()
    {
        NavigateMgr.Instance.FunUpdateUICargo -= update_cargo;
        base.Destory();
    }

    private void update_cargo()
    {
        if (Auto.Company.gameObject.activeSelf)
        {
            Auto.Progress_num.text = $"{Mathf.CeilToInt(NavigateMgr.Instance.Data.cargoCount)}/{NavigateMgr.Instance.Data.capacity}";
        }

        if (Auto.Order.gameObject.activeSelf)
        {
            Auto.Txt_cargo_count.text = $"集装箱：<color=#419ea6>{Mathf.CeilToInt(NavigateMgr.Instance.Data.cargoCount)}</color>";
        }
        
        update_red();
    }

    private void on_company_click()
    {
        Auto.Order.gameObject.SetActive(false);
        Auto.Company.gameObject.SetActive(true);
        Auto.Btn_company.transform.FindChildTransform("nor").SetActive(false);
        Auto.Btn_company.transform.FindChildTransform("choose").SetActive(true);
        
        Auto.Btn_order.transform.FindChildTransform("nor").SetActive(true);
        Auto.Btn_order.transform.FindChildTransform("choose").SetActive(false);
        update_company();
    }

    private void update_company()
    {
        Auto.Img_full.transform.SetActive(MathF.Abs(NavigateMgr.Instance.Data.capacity - NavigateMgr.Instance.Data.cargoCount) <= 0.1f);
        Auto.Progress_num.text = $"{Mathf.CeilToInt(NavigateMgr.Instance.Data.cargoCount)}/{NavigateMgr.Instance.Data.capacity}";
        Auto.Progress.fillAmount = NavigateMgr.Instance.Data.cargoCount / (float)NavigateMgr.Instance.Data.capacity;
        Auto.Txt_capacity.text = $"仓库容量：<color=#419ea6>{NavigateMgr.Instance.Data.capacity}集装箱</color>";
        Auto.Txt_output.text = $"生产速度：<color=#419ea6>{NavigateMgr.Instance.Data.output}集装箱/秒</color>";
        var tCapacity = TableCache.Instance.warehouseTable[NavigateMgr.Instance.Data.capacityLv];
        var tOutput = TableCache.Instance.warehouseTable[NavigateMgr.Instance.Data.outputLv];
        var haveNum = PlayerMgr.Instance.GetItemNum(2);
        // tran.SetText("需要x评分".Replace("x", needScore.ToString().ChangeColor(GetNumColorStr(Score >= needScore))), "lock/msc");
        var enoughCapacity = tCapacity.capacityCost[0].num <= haveNum;
        var enoughOutput = tOutput.outputCost[0].num <= haveNum;
        Auto.Capacity_money.text = $"{tCapacity.capacityCost[0].num.ToString().ChangeColor(GetNumColorStr(enoughCapacity))}";
        Auto.Output_money.text = $"{tOutput.outputCost[0].num.ToString().ChangeColor(GetNumColorStr(enoughOutput))}";
        Auto.Un_up_capacity.SetActive(!enoughCapacity);
        Auto.Can_up_capacity.SetActive(enoughCapacity);
        Auto.Un_up_output.SetActive(!enoughOutput);
        Auto.Can_up_output.SetActive(enoughOutput);

        update_company_list();
    }

    private void update_company_list()
    {
        var data = NavigateMgr.Instance.Data.ships.SortFirstMiddeDown(pair =>
        {
            if (pair.Value.lv > 0)
            {
                return -1;
            }
            
            if (pair.Value.lv == 0)
            {
                return 1;
            }
            
            return 2;
        });
        
        data.ForeachForAddItems(Auto.Grid_company, (k, v, index, tran) => {
            show_company_one(tran, v, k, index);
        });
    }

    private void show_company_one(Transform tran, CargoShipItem data, int k, int index)
    {
        var tabOne = TableCache.Instance.cargoShipTable[data.tId];
        tran.SetActive(data.lv > 0, "open");
        tran.SetActive(data.lv == 0, "lock");
        tran.SetActive(data.lv != 0, "lvbg");
        tran.SetText("Lv." + data.lv, "lvbg/lv");
        tran.SetImage("ship/" + tabOne.icon, "icon");
        tran.SetText($"{tabOne.name}", "name");
        tran.SetText($"运载量：<color=#419ea6>+{tabOne.capacity + tabOne.capacityLvup * data.lv}集装箱</color>", "prop0");
        tran.SetText($"航速：<color=#419ea6>+{tabOne.speed}节</color>", "prop1");
        if (data.lv == 0)
        {
            tran.SetActive(false, "max");
            tran.SetActive(false, "open");
            tran.SetActive(true, "lock");
            if (tabOne.unlockType == 1)
            {
                tran.SetActive(true, "lock/lock");
                tran.SetActive(true, "lock/lock/btn");
                tran.SetActive(false, "lock/lock/btn_unlock_ad");
                tran.SetActive(false, "lock/unenough");
                tran.SetActive(true, "lock/msc");
                
                int needScore = tabOne.score;
                tran.SetText("需要x评分".Replace("x", needScore.ToString().ChangeColor(GetNumColorStr(Score >= needScore))), "lock/msc");
                if (Score < needScore)
                {
                    tran.SetActive(true, "lock/unenough");
                }
                else {
                    tran.SetActive(true, "lock/lock");
                    EventTriggerListener.Get(tran.Find("lock/lock/btn")).onClick = (btn) =>
                    {
                        Debug.Log($"船舶解锁 {tabOne.id}");
                        NavigateMgr.Instance.UplevelShip(tabOne.id, (b) =>
                        {
                            if (b)
                            {
                                Msg.Instance.Show("解锁成功!");
                                update_company_list();
                                UIManager.OpenTip("UIShipUnlock", $"{tabOne.id}_1");
                            }
                        });
                    };

                    if (index == 0)
                    {
                        GuideMgr.Instance.BindBtn(tran.Find("lock/lock/btn"), tableMenu.GuideWindownBtn.navigate_first_unlock);
                    }
                }
            }
            else 
            {
                tran.SetActive(true, "lock/lock");
                tran.SetActive(false, "lock/lock/btn");
                tran.SetActive(true, "lock/lock/btn_unlock_ad");
                tran.SetActive(false, "lock/unenough");
                tran.SetActive(false, "lock/msc");
                EventTriggerListener.Get(tran.Find("lock/lock/btn_unlock_ad")).onClick = (btn) =>
                {
                    Debug.Log($"船舶解锁 {tabOne.id}");
                    NavigateMgr.Instance.UplevelShip(tabOne.id, (b) =>
                    {
                        if (b)
                        {
                            Msg.Instance.Show("解锁成功!");
                            update_company_list();
                            UIManager.OpenTip("UIShipUnlock", $"{tabOne.id}_1");
                        }
                    });
                };
            }
        }
        else {
            tran.SetActive(false, "max");
            tran.SetActive(true, "open");
            tran.SetActive(false, "lock");

            var haveNum = PlayerMgr.Instance.GetItemNum(2);
            var needNum = tabOne.cost[0].num;
            tran.SetText(needNum.ToString().ChangeColor(GetNumColorStr(haveNum >= needNum)), "open/money");
            EventTriggerListener.Get(tran.Find("open/btn")).onClick = (btn) =>
            {
                Debug.Log("船舶 升级");
                NavigateMgr.Instance.UplevelShip(tabOne.id, (b) =>
                {
                    if (b)
                    {
                        Msg.Instance.Show("升级成功!");
                        update_company_list();
                    }
                    else {
                        UIManager.OpenTip("UIAd_day", "", (str) => {
                            show_company_one(tran, data, k, index);
                        });
                    }
                });
            };
        }
    }

    private void on_order_click()
    {
        Auto.Order.gameObject.SetActive(true);
        Auto.Company.gameObject.SetActive(false);
        Auto.Btn_order.transform.FindChildTransform("nor").SetActive(false);
        Auto.Btn_order.transform.FindChildTransform("choose").SetActive(true);
        
        Auto.Btn_company.transform.FindChildTransform("nor").SetActive(true);
        Auto.Btn_company.transform.FindChildTransform("choose").SetActive(false);
        update_order();
    }

    private void update_order()
    {
        Dictionary<int, CargoShipItem> data = new Dictionary<int, CargoShipItem>();
        int leftShip = 0;
        foreach (var (_, ship) in NavigateMgr.Instance.Data.ships)
        {
            if (ship.lv > 0)
            {
                data.TryAdd(ship.tId, ship);
                
                if (ship.sendTime == 0 && ship.cargoCount == 0)
                {
                    leftShip++;
                }
            }
        }

        Auto.Txt_cargo_count.text = $"集装箱：<color=#419ea6>{Mathf.CeilToInt(NavigateMgr.Instance.Data.cargoCount)}</color>";
        Auto.Txt_left_ship.text = $"空闲船只：<color=#419ea6>{leftShip}/{data.Count}</color>";
        data.SortFirstMiddeDown(pair =>
        {
            var tab = TableCache.Instance.cargoShipTable[pair.Value.tId];
            if (pair.Value.sendTime == 0 && pair.Value.cargoCount == 0)
            {
                if (NavigateMgr.Instance.Data.cargoCount >= pair.Value.cargoCount)
                {
                    return 0;
                }

                return 1;
            }
            
            if (TimeTool.CurTimeSeconds >= pair.Value.sendTime)
            {
                return -1;
            }

            return 2;
        });
        data.ForeachForAddItems(Auto.Grid_order, (k, v, index, tran) => {
            show_order_one(tran, v, k, index);
        });
    }

    //更新订单
    private void show_order_one(Transform tran, CargoShipItem data, int k, int index)
    {
        var tabOne = TableCache.Instance.cargoShipTable[data.tId];
        var tOrder = TableCache.Instance.orderTable[data.tOrderId];
        tran.SetActive(data.lv != 0, "lvbg");
        tran.SetText("Lv." + data.lv, "lvbg/lv");
        tran.SetImage("ship/" + tabOne.icon, "icon");
        tran.SetText($"{tabOne.name}", "name");
        string moveTime = TimeTool.FormatTime(tOrder.dis / data.Speed) ;
        tran.SetText($"时间：<color=#419ea6>{moveTime}</color>", "prop0");
        tran.SetText($"奖励：<color=#419ea6>+{NavigateMgr.Instance.GetRewardCount(tOrder, data)}</color>", "prop1");
        
        tran.SetActive(false, "send");
        tran.SetActive(false, "get");
        tran.SetActive(false, "shipping");
        
        var needNum = data.Capacity;
        if (data.sendTime == 0 && data.cargoCount == 0)
        {
            //还没派送订单
            tran.SetActive(true, "send");
            bool enough = NavigateMgr.Instance.Data.cargoCount >= needNum;
            tran.SetText(needNum.ToString().ChangeColor(GetNumColorStr(enough)), "send/msc");
            tran.SetActive(enough, "send/enough");
            tran.SetActive(!enough, "send/unenough");
            if (index == 0)
            {
                GuideMgr.Instance.BindBtn(tran.Find("send/enough/btn"), tableMenu.GuideWindownBtn.navigate_order_send);
            }
            EventTriggerListener.Get(tran.Find("send/enough/btn")).onClick = (btn) =>
            {
                Debug.Log($"运货 发送订单 {tabOne.id}");
                NavigateMgr.Instance.SendOrder(tabOne.id);
                UIManager.CloseAllTip();
                MainMgr.Instance.FunMoveToNavigate?.Invoke();
            };
        }
        else
        {
            if (TimeTool.CurTimeSeconds >= data.sendTime)
            {
                //送到
                tran.SetActive(true, "get");
                EventTriggerListener.Get(tran.Find("get/btn")).onClick = (btn) =>
                {
                    UIManager.OpenTip("UISendEnd", tabOne.id.ToString(), (_) =>
                    {
                        update_order();
                    });
                };
            }
            else
            {
                //还没送到
                tran.SetActive(true, "shipping");
                var go = tran.FindChildTransform("shipping/time");
                TimeTool.CutTime(go.gameObject, (int)data.sendTime, 0.5f, (str) =>
                {
                    go.GetComponent<Text>().text = str;
                }, () =>
                {
                    update_order();
                }, null);
            }
        }
    }

    private void update_red()
    {
        Auto.Btn_company.transform.FindChildTransform("red").SetActive(NavigateMgr.Instance.CheckRedCompany());
        Auto.Btn_order.transform.FindChildTransform("red").SetActive(NavigateMgr.Instance.CheckRedOrder());
    }
    
    private string GetNumColorStr(bool isEnough) {
        if (isEnough)
        {
            return "419ea6";
        }
        return "a64141";
    }
}

