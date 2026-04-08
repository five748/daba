using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStaffItem : MonoBehaviour
{
    public Image icon;
    public Text lb_name_lv;
    public Text lb_position;
    public Text lb_ability;
    public Text lb_require;

    public Transform tf_unenough;
    public EventTriggerListener btn_unenough;
    public Transform tf_lock;
    public EventTriggerListener btn_lock;
    
    private int index = 0;
    private int staff_id = 0;
    
    public void init(int staff_id, int index)
    {
        this.staff_id = staff_id;
        this.index = index;

        var t_staff = TableCache.Instance.tollCollectorTable[staff_id];
        icon.SetImage("staff/" + staff_id);
        icon.GetComponent<Image>().PlayLoop($"staffWait{staff_id}");
        lb_name_lv.text = $"{t_staff.name} Lv.{1}";
        lb_position.text = $"岗位 <color=#419ea6>{index + 1}</color> 号航道";
        lb_ability.text = $"收银速度 <color=#419ea6>+3%</color>";
        var staff = StaffMgr.Instance.data.staffs[staff_id];
        if (staff.unlock)
        {
            tf_unenough.SetActive(false);
            tf_lock.SetActive(false);
            lb_require.gameObject.SetActive(false);
        }
        else
        {
            lb_require.gameObject.SetActive(true);
            if (t_staff.score <= PlayerMgr.Instance.data.score)
            {
                tf_unenough.SetActive(false);
                tf_lock.SetActive(true);
                lb_require.text = $"需 <color=#419ea6>{t_staff.score}</color> 评分";
            }
            else
            {
                tf_unenough.SetActive(true);
                tf_lock.SetActive(false);
                lb_require.text = $"需 <color=#a64141>{t_staff.score}</color> 评分";
            }
        }
        
        btn_unenough.onClick = (go) =>
        {
            Msg.Instance.Show("积分不足");
        };

        btn_lock.onClick = (go) =>
        {
            Debug.Log($"点击解锁职员");
            //todo 需要知道是哪个大坝 哪个航道 先写死 
            if (!ChannelMgr.Instance.CheckUnlockShouFeiZhan(1, staff_id, staff_id))
            {
                Msg.Instance.Show($"请先解锁{staff_id}号航道的收费站");
                return;
            }
            
            // if (PlayerMgr.Instance.AddScore(-t_staff.score))
            // {
                StaffMgr.Instance.Unlock(staff_id);
                init(staff_id, index);
                UIManager.CloseAllTip();
            // }
        };
    }
}
