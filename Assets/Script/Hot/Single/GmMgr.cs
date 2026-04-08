using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GmMgr : Singleton<GmMgr>
{
    public Action<bool> OnControlAdShow;
    
    public void add_score(int num)
    {
        PlayerMgr.Instance.AddScore(num);
    }
    
    public void add_item(int item_id, int num)
    {
        PlayerMgr.Instance.AddItemNum(item_id, num);
    }

    public void add_repair_ship()
    {
        // ChannelMgr.Instance.FunMakeShip?.Invoke(true, 0);
    }

    public void add_wash_event()
    {
        var channelId = Random.Range(0, 2);
        ChannelMgr.Instance.WashStart(1, channelId);
    }
    
    public void change_speed(float speed)
    {
        // ChannelMgr.Instance.SetChannelSpeed(speed);

        Time.timeScale = speed;
    }
}