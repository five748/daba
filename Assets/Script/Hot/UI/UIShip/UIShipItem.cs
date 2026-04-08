using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIShipItem : MonoBehaviour
{
    public Image icon;
    public Text lb_name_lv;
    public Text lb_money;
    public Text lb_ability;
    public Text lb_require;

    public Transform tf_unenough;
    public EventTriggerListener btn_unenough;
    public Transform tf_lock;
    public EventTriggerListener btn_lock;
    
    private int index = 0;
    private int ship_id = 0;
    
    public void init(int ship_id, int index)
    {
        this.ship_id = ship_id;
        this.index = index;

        var t_ship = TableCache.Instance.shipTable[ship_id];
        //icon.SetImage("ship/" + ship_id, true, 1f, sprite =>
        //{
        //    var size = icon.GetComponent<RectTransform>().sizeDelta;
        //    var scale = 246 / size.x;
        //    scale = Mathf.Min(scale, 246 / size.y);
        //    icon.transform.localScale = new Vector3(scale, scale, 1);
        //});
        lb_name_lv.text = $"{t_ship.name} Lv.{1}";
        lb_money.text = $"{t_ship.toll}";
        lb_ability.text = $"过闸时间 <color=#419ea6>{t_ship.passTime}秒</color>";
        var ship = ShipMgr.Instance.data.ships[ship_id];
        if (ship.unlock)
        {
            tf_unenough.SetActive(false);
            tf_lock.SetActive(false);
            lb_require.gameObject.SetActive(false);
        }
        else
        {
            lb_require.gameObject.SetActive(true);
            if (t_ship.score <= PlayerMgr.Instance.data.score)
            {
                tf_unenough.SetActive(false);
                tf_lock.SetActive(true);
                lb_require.text = $"需 <color=# >{t_ship.score}</color> 评分";
            }
            else
            {
                tf_unenough.SetActive(true);
                tf_lock.SetActive(false);
                lb_require.text = $"需 <color=#a64141>{t_ship.score}</color> 评分";
            }
        }
        
        btn_unenough.onClick = (go) =>
        {
            Msg.Instance.Show("积分不足");
        };

        btn_lock.onClick = (go) =>
        {
            ShipMgr.Instance.UnlockShip(ship_id);
            init(ship_id, index);
            UIManager.OpenTip("UIShipUnlock", $"{ship_id}_0");
            // if (PlayerMgr.Instance.AddScore(-t_ship.score))
            // {
            // }
        };
    }
}