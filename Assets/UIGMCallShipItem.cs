using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGMCallShipItem : MonoBehaviour
{
    public Image img_icon;
    public EventTriggerListener btn_call_one;
    public EventTriggerListener btn_call_always;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init(int id)
    {
        img_icon.SetImage("ship/"+id);
        btn_call_one.onClick = (go) =>
        {
            // ChannelMgr.Instance.FunMakeShip?.Invoke(false, id);
        };

        btn_call_always.onClick = (go) =>
        {
            ChannelMgr.Instance.AlwaysOneID = id;
        };
    }
}
