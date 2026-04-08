using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using UnityEngine.UI;
public class UIJiaoTongInfo:BaseMonoBehaviour{
    private UIJiaoTongInfoAuto Auto = new UIJiaoTongInfoAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtnclose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip("UIJiaoTongInfo");

    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private directTraffic config;
    public RectTransform rect;
    public static UIJiaoTongInfo ins;
    private void Init(string param){
        UIManager.FadeOut();
        ins = this;
        config = TableCache.Instance.directTrafficTable[param.ToInt()];
        rect = Auto.Grid.GetComponent<RectTransform>();
        Auto.Layer.text = $"(第{config.id}关)";
        Auto.Reward2.text = $"疏通所有拥堵船只即可获得:<color=#24d39f>  {config.reward[0].num}</color>";
        InitGrid();
        
        //下面是可移动船只引导按钮
        GuideMgr.Instance.BindBtn(GetGuideChip(), tableMenu.GuideWindownBtn.jiaotong_info);
    }


    private List<UIJiaoTongItem> chips = new List<UIJiaoTongItem>();
    public static int size = 140;
    private void InitGrid()
    {
        var items = Auto.Grid.AddChilds(config.chipPos.Length);
        var left = 99999f;
        var right = -99999f;
        var top = -99999f;
        var down = 999999f;
        
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i].GetComponent<UIJiaoTongItem>();
            var res =item.Init(config.chipPos[i]);
            chips.Add(item);
            left = Mathf.Min(left, res[0]);
            right = Mathf.Max(right, res[0]);
            down = Mathf.Min(down, res[1]);
            top = Mathf.Max(top, res[1]);
        }

        var maxw = Mathf.Max(right, -left) + 100;
        var maxh = Mathf.Max(top, -down) + 100;
        rect.sizeDelta = new Vector2(maxw * 2,maxh * 2);
        var scale = Mathf.Min(1,ProssData.Instance.CanvasSize.x / (maxw * 2 + 70));
        Auto.Grid.localScale = new Vector3(scale,scale,scale);
    }
    
    

    public void Success()
    {
        if (chips.Count <= 0)
        {
            UIManager.OpenTipNoText("UIJiaoTongResult",config.id + "-" + "1");
        }
    }

    public void Fail()
    {
        for (int i = 0; i < chips.Count; i++)
        {
            var c = chips[i];
            var next = c.GetNext();
            if (CheckCanMove(c, next))
            {
                return;
            }
        }
        
        UIManager.OpenTipNoText("UIJiaoTongResult",config.id + "-" + "0");

    }

    

    private bool isMove;
    public void ClickMoveChip(UIJiaoTongItem chip)
    {
        // if (isMove)
        // {
        //     return;
        // }
        isMove = true;
        MoveChip(chip);
    }

    private void MoveChip(UIJiaoTongItem chip)
    {
        if (chip.isMove)
        {
            return;
        }
        if (chip.isInScreen())
        {
            isMove = false;
            chips.Remove(chip);
            GameObject.Destroy(chip.gameObject);
            Success();
            return;
        }
        
        var next = chip.GetNext();
        if (CheckCanMove(chip,next))
        {
            var pos = chip.ToNext();
            chip.transform.DOLocalMove(pos, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                chip.isMove = false;
                MoveChip(chip);
            });
        }
        else
        {
            chip.Shake();
            isMove = false;
            Fail();
        }
    }

    public override void Destory()
    {
        base.Destory();
        foreach (var uiJiaoTongItem in chips)
        {
            DOTween.Kill(uiJiaoTongItem.transform,false);
        }
    }

    private bool CheckCanMove(UIJiaoTongItem chip,int[] grid)
    {
        for (int i = 0; i < chips.Count; i++)
        {
            if (chip != chips[i] && chips[i].CheckInGrid(grid))
            {
                // chips[i].Shake();
                return false;
            }
        }

        return true;

    }


    private Transform GetGuideChip()
    {
        for (int i = 0; i < chips.Count; i++)
        {
            var c = chips[i];
            var next = c.GetNext();
            if (CheckCanMove(c, next))
            {
                return c.transform;
            }
        }
        return null;
    }
    
    
}

