using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMainNavigateShip : MonoBehaviour
{
    public Transform tfBack;
    public Transform tfSend;
    public Image imgShip;

    private int _tShipId = 0;
    private bool _upload = false;
    
    public void UpdateUI(int tShipId)
    {
        if (_upload)
        {
            tfBack.SetActive(false);
            tfSend.SetActive(false);
            return;
        }
        
        _tShipId = tShipId;
        var dataShip = NavigateMgr.Instance.Data.ships[tShipId];
        Debug.Log($"发送的船舶 UIMainNavigateShip 要显示的船舶id为 {_tShipId}");
        var tShip = TableCache.Instance.cargoShipTable[dataShip.tId];
        transform.GetComponent<Image>().PlayLoop(tShip.moveAni);
        tfBack.SetActive(false);
        tfSend.SetActive(false);
        if (dataShip.sendTime != 0 && dataShip.sendTime <= TimeTool.CurTimeSeconds)
        {
            tfBack.SetActive(true);
        }
        else
        {
            if (dataShip.sendTime == 0)
            {
                tfSend.SetActive(true);
            }
        }
    }

    public void Reload()
    {
        if (_tShipId == 0)
        {
            return;
        }
        UpdateUI(_tShipId);
    }

    public void Upload(bool v)
    {
        _upload = v;
    }

    public void ChangeToFreeShip()
    {
        foreach (var (_, ship) in NavigateMgr.Instance.Data.ships)
        {
            if (ship.lv <= 0)
            {
                continue;
            }
            
            //非空船
            if (ship.cargoCount > 0 || ship.sendTime > 0)
            {
                continue;
            }

            if (NavigateMgr.Instance.Data.cargoCount >= ship.Capacity && ship.sendTime <= 0)
            {
                var tShip = TableCache.Instance.cargoShipTable[ship.tId];
                transform.GetComponent<Image>().PlayLoop(tShip.moveAni); //SetImage($"ship/{tShip.icon}");
                return;
            }
        }
    }
}