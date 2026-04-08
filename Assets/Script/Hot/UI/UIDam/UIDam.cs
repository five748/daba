using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SelfComponent;
using UnityEngine.UI;
public class UIDam:BaseMonoBehaviour{
    private UIDamAuto Auto = new UIDamAuto();
    
    private ListView listView;
    
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
        listView = Auto.DamViews.GetComponent<ListView>();
        listView.Init(updateListItem);
        update_list();
    }
    
    private void update_list()
    {
        listView.ShowList(ChannelMgr.Instance.Data.Dams.Count);
        StartCoroutine(move_to(ChannelMgr.Instance.GetCurDamData().damId - 1));
    }
    
    private void updateListItem(int index, GameObject go)
    {
        var dams = ChannelMgr.Instance.Data.Dams;
        var com = go.GetComponent<UIDamItem>();
        com.Init(dams[index + 1], update_list);

    }
    
    private IEnumerator move_to(int index)
    {
        while (!gameObject.activeSelf)
        {
            yield return new WaitForFixedUpdate();
        }
        
        listView.MoveTo(index);
    }
}

