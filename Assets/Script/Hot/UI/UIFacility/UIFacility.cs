using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.UIElements;
using ListView = SelfComponent.ListView;

public class UIFacility:BaseMonoBehaviour{
    private UIFacilityAuto Auto = new UIFacilityAuto();

    private ListView _list;

    private int _choseDamId = int.MinValue;
    
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
    private void Init(string param){
        UIManager.FadeOut();
        _list = Auto.List.GetComponent<ListView>();
        _list.Init(update_item);
        init_list();
        init_buttons();
    }

    //初始化顶部按钮
    private void init_buttons()
    {
        Auto.Dam_btns_content.AddChilds(TableCache.Instance.damTable.Count);

        int index = 0;
        foreach (var (_, dam) in TableCache.Instance.damTable)
        {
            var tf = Auto.Dam_btns_content.GetChild(index);
            var bgUnlock = tf.FindChildTransform("bg_unlock");
            var bgChose = tf.FindChildTransform("bg_chose");
            var bgLock = tf.FindChildTransform("bg_lock");
            var txtName = tf.FindChildTransform("txt_name").GetComponent<Text>();
            var txtLockName = tf.FindChildTransform("txt_lock_name").GetComponent<Text>();
            bgUnlock.gameObject.SetActive(false);
            bgChose.gameObject.SetActive(false);
            bgLock.gameObject.SetActive(false);
            var dataDam = ChannelMgr.Instance.GetDamData(dam.id);

            bgUnlock.GetComponent<EventTriggerListener>().onClick = (_) =>
            {
                _choseDamId = dam.id;
                update_list();
                init_buttons();
            };
            
            bgChose.GetComponent<EventTriggerListener>().onClick = (_) =>
            {
                Msg.Instance.Show($"已在当前列表");
            };
            
            bgLock.GetComponent<EventTriggerListener>().onClick = (_) =>
            {
                Msg.Instance.Show($"尚未解锁");
            };

            if (_choseDamId == dam.id)
            {
                bgChose.gameObject.SetActive(true);
                txtName.gameObject.SetActive(true);
                txtName.text = $"<color=#ffffff>{dam.name}</color>";
            }
            else if (dataDam.unlock)
            {
                bgUnlock.SetActive(true);
                txtName.gameObject.SetActive(true);
                txtName.text = $"<color=#cbc0b4>{dam.name}</color>";
            }
            else
            {
                bgLock.gameObject.SetActive(true);
                txtName.gameObject.SetActive(false);
                txtLockName.text = $"{dam.name}";
            }
            ++index;
        }
    }

    private void init_list()
    {
        //默认切换到下一个要解锁的设备上
        var nextChannelId = int.MinValue;
        foreach (var (_, dam) in BuildMgr.Instance.Data.Builds)
        {
            var dataDam = ChannelMgr.Instance.GetDamData(dam.damId);
            if (!dataDam.unlock)
            {
                //大坝没解锁
                _choseDamId = dataDam.damId - 1;
                nextChannelId = dataDam.channels.Count - 1;
                break;
            }

            nextChannelId = find_aim_channel(dam.damId);
            if (nextChannelId != int.MinValue)
            {
                _choseDamId = dataDam.damId;
                break;
            }
        }

        if (nextChannelId == int.MinValue || _choseDamId == int.MinValue)
        {
            Debug.Log($"没有可解锁的设备了");
            nextChannelId = DataDam.ChannelNum - 1;
            _choseDamId = BuildMgr.Instance.Data.Builds.Count - 1;
        }
        
        _list.ShowList(DataDam.ChannelNum);
        StartCoroutine(move_to(nextChannelId));
    }

    private void update_list()
    {
        var nextChannelId = find_aim_channel(_choseDamId);
        _list.ShowList(DataDam.ChannelNum);
        StartCoroutine(move_to(nextChannelId));
    }

    private int find_aim_channel(int damId)
    {
        int result = int.MinValue;

        for (int i = 0; i < DataDam.ChannelNum; i++)
        {
            var channel = BuildMgr.Instance.Data.Builds[damId].channels[i];
            foreach (var t in channel.builds)
            {
                if (!t.unlock)
                {
                    result = channel.channelId;
                    return result;
                }
            }
        }
        
        return result;
    }

    private IEnumerator move_to(int index)
    {
        while (!gameObject.activeSelf)
        {
            yield return new WaitForFixedUpdate();
        }
        
        _list.MoveTo(index);
        Auto.Dam_btns_content.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = _choseDamId / (float)TableCache.Instance.damTable.Count;
    }

    private void update_item(int channelId, GameObject go)
    {
        go.GetComponent<UIFacilityItem>().Init(_choseDamId, channelId, update_list, () =>
        {
            return gameObject.activeSelf;
        });
    }
}

