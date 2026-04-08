using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFacilityItemDetail : MonoBehaviour
{
    public Text txt_name;
    public Text txt_num;
    
    public Image img_icon;

    public EventTriggerListener btn_unlock;
    
    public Transform tf_lock;
    public Transform tf_unlock;
    public Transform tf_unenough;
    
    private Table.buildingItem tBuild;

    private void Start()
    {
        PlayerMgr.Instance.FunItemChange += onItemChange;
    }

    private void OnDestroy()
    {
        PlayerMgr.Instance.FunItemChange -= onItemChange;
    }

    public void Init(int damId, int channelId, int buildId, int tBuildId, Action funUpdate, Action funUpdateAll)
    {
        tBuild = TableCache.Instance.buildingItemTable[tBuildId];
        
        txt_name.text = tBuild.name;
        txt_num.text = tBuild.unlock_cost.ToString();
        img_icon.SetImage("channelIcon/" + (buildId + 1), true);
        
        btn_unlock.onClick = (_) =>
        {
            if (PlayerMgr.Instance.AddItemNum(1, -tBuild.unlock_cost))
            {
                BuildMgr.Instance.Unlock(damId, channelId, buildId, tBuildId);
                if (tBuildId <= TableCache.Instance.configTable[901].param.ToInt())
                {
                    //小于配置的 才做主界面上的动画
                    UIManager.CloseAllTip();
                }
                funUpdate?.Invoke();
                if (buildId == BuildMgr.Instance.Data.Builds[damId].channels[channelId].builds.Count - 1)
                {
                    //最后一个 需要显示下一行 更新界面
                    funUpdateAll();
                }
            }
            else {
                UIManager.OpenTip("UIAd_Zanzhu", "", s =>
                {
                    Init(damId, channelId, buildId, tBuildId, funUpdate, funUpdateAll);
                    UIEmpty.Instance.UpdateAdRed();
                });
            }
        };
        
        if (damId == 1 && channelId == 0)
        {
            if (buildId == 1)
            {
                GuideMgr.Instance.BindBtn(btn_unlock.transform, tableMenu.GuideWindownBtn.facility_jiushengting);
            }
            else if (buildId == 2)
            {
                GuideMgr.Instance.BindBtn(btn_unlock.transform, tableMenu.GuideWindownBtn.facility_jiushengquan);
            }
            else if (buildId == 3)
            {
                GuideMgr.Instance.BindBtn(btn_unlock.transform, tableMenu.GuideWindownBtn.facility_diaotai);
            }
        }
        
        //显示判定 存在return的情况
        var dataBuild = BuildMgr.Instance.Data.Builds[damId].channels[channelId].builds[buildId];
        var buildCount = BuildMgr.Instance.Data.Builds[damId].channels[channelId].builds.Count;
        tf_lock.SetActive(false);
        tf_unenough.SetActive(false);
        tf_unlock.SetActive(false);
        if (dataBuild.unlock)
        {
            //解锁了
            tf_lock.SetActive(false);
            tf_unenough.SetActive(false);
            tf_unlock.SetActive(true);
        }
        else
        {
            //未解锁
            if (channelId != 0)
            {
                //上一行没全部解锁 这一行都是not enough的状态
                if (!BuildMgr.Instance.Data.Builds[damId].channels[channelId - 1].builds[buildCount - 1].unlock)
                {
                    tf_unenough.SetActive(true);
                    return;
                }
            }
            
            if (buildId == 0)
            {
                tf_lock.SetActive(true);
            }
            else
            {
                var dataPreBuild = BuildMgr.Instance.Data.Builds[damId].channels[channelId].builds[buildId - 1];
                if (dataPreBuild.unlock)
                {
                    tf_lock.SetActive(true);
                }
                else
                {
                    tf_unenough.SetActive(true);
                }
            }
        }

        //如果显示的是带按钮的
        if (tf_lock.gameObject.activeSelf)
        {
            if (!PlayerMgr.Instance.IsEnough(1, tBuild.unlock_cost))
            {
                txt_num.text = $"<color=#a64141>{tBuild.unlock_cost}</color>";
            }
            else
            {
                txt_num.text = $"<color=#ffffff>{tBuild.unlock_cost}</color>";
            }
        }
        
        // LayoutRebuilder.ForceRebuildLayoutImmediate(txt_num.transform.parent.GetComponent<RectTransform>());
    }

    private void onItemChange(int id, long num)
    {
        if (tf_lock.gameObject.activeSelf)
        {
            if (!PlayerMgr.Instance.IsEnough(1, tBuild.unlock_cost))
            {
                txt_num.text = $"<color=#a64141>{tBuild.unlock_cost}</color>";
            }
            else
            {
                txt_num.text = $"<color=#ffffff>{tBuild.unlock_cost}</color>";
            }
        }
    }
}
