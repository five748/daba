using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SelfComponent;
using UnityEngine.UI;
public class UIGM:BaseMonoBehaviour{
    private UIGMAuto Auto = new UIGMAuto();

    private ListView list;
    
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_close(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickBtn_add_score(GameObject button){
        Debug.Log("click" + button.name);
        GmMgr.Instance.add_score(Auto.Txt_score.text.ToInt());
    }
    public void ClickBtn_add_item(GameObject button){
        Debug.Log("click" + button.name);
        GmMgr.Instance.add_item(Auto.Txt_item_id.text.ToInt(), Auto.Txt_item_num.text.ToInt());
    }
    public void ClickBtn_ship_repair(GameObject button){
        Debug.Log("click" + button.name);
        // GmMgr.Instance.add_repair_ship();
        UIManager.CloseTip("UIGM");
        UIManager.OpenTip("UIFixShipTip", "1");
    }
    public void ClickBtn_wash_event(GameObject button){
        Debug.Log("click" + button.name);
        // GmMgr.Instance.add_wash_event();
        UIManager.CloseTip("UIGM");
        UIManager.OpenTip("UICleanTip", "1");
    }
    public void ClickBtn_game_speed(GameObject button){
        Debug.Log("click" + button.name);
        float.TryParse(Auto.Txt_game_speed.text, out var floatValue);
        GmMgr.Instance.change_speed(floatValue);
    }
    public void ClickBtn_close_event(GameObject button){
        Debug.Log("click" + button.name);
        ChannelMgr.Instance.OnCreateEvent(false);
    }
    public void ClickBtn_start_event(GameObject button){
        Debug.Log("click" + button.name);
        ChannelMgr.Instance.OnCreateEvent(true);
    }
    public void ClickBtn_unlock_channel(GameObject button){
        Debug.Log("click" + button.name);
        
        int.TryParse(Auto.Txt_unlock_channel.text, out var value);
        if (value < 0)
        {
            Msg.Instance.Show("不能小于0");
        }
        else if (value > DataDam.ChannelNum - 1)
        {
            Msg.Instance.Show("不能超过上限");
        }
        else {
            for (int i = 0; i <= value; i++)
            {
                var channel = ChannelMgr.Instance.GetCurDamChannelData(i);
                channel.dengta = true;
                channel.diaoji = true;
                channel.shoufeizhan = true;
                channel.honglvdeng = true;
                channel.jiushengting = true;
                channel.luntai = true;
                channel.jiqi = true;
                channel.xiansu = true;

                foreach (var item in BuildMgr.Instance.Data.Builds[ChannelMgr.Instance.DamId].channels[i].builds)
                {
                    item.unlock = true;
                }
                
                ChannelMgr.Instance.FunUpdateChannelUI?.Invoke(i);
            }
            
            BuildMgr.Instance.Data.SaveToFile();
            ChannelMgr.Instance.Data.SaveToFile();
        }
        
    }
    public void ClickBtn_reset_call(GameObject button){
        Debug.Log("click" + button.name);
        ChannelMgr.Instance.AlwaysOneID = int.MinValue;
    }
    public void ClickBtn_show_ad(GameObject button){
        Debug.Log("click" + button.name);
        GmMgr.Instance.OnControlAdShow?.Invoke(true);
    }
    public void ClickBtn_close_ad(GameObject button){
        Debug.Log("click" + button.name);
        GmMgr.Instance.OnControlAdShow?.Invoke(false);
    }
    public void ClickBtn_over_task(GameObject button){
        Debug.Log("click" + button.name);
        MTaskData.Instance.CrrentTaskOver();
    }
    public void ClickBtn_openallsystem(GameObject button){
        Debug.Log("click" + button.name);
        MTaskData.Instance.data.OpenAllSystem = true;
    }
    public void ClickBtn_add_next_ship(GameObject button){
        Debug.Log("click" + button.name);
        ChannelMgr.Instance.GMMakeShip(Auto.Txt_ship_channel_id.text.ToInt(), Auto.Txt_create_ship_id.text.ToInt());
    }
    public void ClickBtn_jioatong(GameObject button){
        Debug.Log("click" + button.name);
        var id = Auto.Txt_jiaotong.text.ToInt();
        if (id > 0 && PlayerMgr.Instance.data.jiaotong.Contains(id))
        {
            PlayerMgr.Instance.data.jiaotong.Remove(id);
            PlayerMgr.Instance.data.SaveToFile();
        }
    }
    //------------------------------点击方法标记请勿删除---------------------------------

    private void Init(string param){
        UIManager.FadeOut();

        list = Auto.List_ships.GetComponent<ListView>();
        list.Init(update_item);
        list.ShowList(TableCache.Instance.shipTable.Count);
    }

    private void update_item(int index, GameObject go)
    {
        go.GetComponent<UIGMCallShipItem>().init(index + 1);
    }
}

