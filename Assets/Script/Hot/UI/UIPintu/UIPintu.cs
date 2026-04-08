using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SelfComponent;
using Table;
using UnityEngine.UI;

public class UIPintu:BaseMonoBehaviour{
    private UIPintuAuto Auto = new UIPintuAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);
        
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIPintu");
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        list = Auto.List.GetComponent<ListView>();
        list.Init(UpdateListItem);
        UpdateList();
    }

    private ListView list;


    private List<loadingOfCargo> data;
    private void UpdateList()
    {
        var list1 = new List<loadingOfCargo>();
        var list2 = new List<loadingOfCargo>();
        var over = PlayerMgr.Instance.data.pintu;
        foreach (var kv in TableCache.Instance.loadingOfCargoTable)
        {
            if (over.Contains(kv.Key))
            {
                list1.Add(kv.Value);
            }
            else
            {
                list2.Add(kv.Value);
            }
        }

        data = new List<loadingOfCargo>(list2.Concat(list1));
        list.ShowList(data.Count);
    }

    private void UpdateListItem(int index, GameObject o)
    {
        var config = data[index];
        var item = o.transform;
        var score = PlayerMgr.Instance.data.score;
        var isEnd = PlayerMgr.Instance.data.pintu.Contains(config.id);

        item.GetChild(0).SetText("关卡" + config.id);
        item.GetChild(1).SetText($"通关奖励:  <color=#419ea6>{config.reward + config.itemId.Length}</color>");
        item.GetChild(2).gameObject.SetActive(score >= config.score && !isEnd);
        item.GetChild(3).gameObject.SetActive(score < config.score || isEnd);
        if (isEnd)
        {
            item.GetChild(3).GetChild(1).SetText("");
            item.GetChild(3).GetChild(0).SetText("已通关");
            
        }
        else
        {
            item.GetChild(3).GetChild(1).SetText($"需<color=#a64141>{config.score}</color>评分");
            item.GetChild(3).GetChild(0).SetText("挑战");
        }
        item.Find("icon").GetComponent<Image>().SetImage("zhuagnxiehuowu/" + config.icon);

        item.GetChild(2).GetComponent<EventTriggerListener>().onClick = o =>
        {
            UIManager.OpenTip("UIPintuInfo",config.id.ToString(), (s) =>
            {
                UpdateList();
            });
        };
        if (index == 0)
        {
            //下面是挑战引导按钮
            GuideMgr.Instance.BindBtn(item.GetChild(2), tableMenu.GuideWindownBtn.pintu_challenge);
        }
    }
}


