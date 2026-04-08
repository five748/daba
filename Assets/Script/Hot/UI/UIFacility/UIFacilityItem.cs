using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SelfComponent;
using UnityEngine;
using UnityEngine.UI;

public class UIFacilityItem : MonoBehaviour
{
    public Text txt_title;
    public ListView list;
    public Transform content;

    private Action _funUpdateAll;
    private int _channelId;
    private int _damId;

    private Func<bool> _funMoveCheck;
    
    public void Init(int damId, int channelId, Action funUpdateAll, Func<bool> funMoveCheck)
    {
        _funUpdateAll = funUpdateAll;
        _funMoveCheck = funMoveCheck;
        _damId = damId;
        _channelId = channelId;
        txt_title.text = $"{channelId+1}号通道";//显示使用
        list.Init(update_item_detail);
        update_list();
    }

    private void update_list()
    {
        // Debug.LogError($"执行了{index} 的更新update_list");
        var ids = BuildMgr.Instance.Data.Builds[_damId].channels[_channelId].builds;
        var count = ids.Count;
        list.ShowList(count);
        int nextUnlock = int.MinValue;
        for (int i = 0; i < count; i++)
        {
            if (!BuildMgr.Instance.Data.Builds[_damId].channels[_channelId].builds[i].unlock)
            {
                nextUnlock = i;
                break;
            }
        }

        MonoTool.Instance.StartCoroutine(move_to(nextUnlock));
    }

    private void update_item_detail(int buildId, GameObject go)
    {
        // Debug.LogError($"执行了{index} 的更新update_list 子项{index_detail}");
        var id = BuildMgr.Instance.Data.Builds[_damId].channels[_channelId].builds[buildId].tBuildId;
        go.GetComponent<UIFacilityItemDetail>().Init(_damId, _channelId, buildId, id, update_list, _funUpdateAll);
    }
    
    private IEnumerator move_to(int index)
    {
        while (!transform.parent.gameObject.activeSelf || !gameObject.activeSelf || !_funMoveCheck())
        {
            yield return new WaitForFixedUpdate();
        }
        
        list.MoveTo(index);
    }
}