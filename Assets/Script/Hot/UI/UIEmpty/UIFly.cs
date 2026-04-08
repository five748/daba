




using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIFly
{

    public static Transform _flyParent;
    public static Dictionary<int, Transform> _endTran = new Dictionary<int, Transform>();//飞行终点
    public static Transform _noticTran;//跑马灯

    private static GameObjectPool _poolfly;


    public static void Init(Transform parent,Transform end1,Transform end2,Transform paomadeng)
    {
        _flyParent = parent;
        _endTran.Add(1,end1);
        _endTran.Add(2,end2);
        _noticTran = paomadeng;
        _poolfly = new GameObjectPool("Other/FlyItem");
    }

    //isAddTip:是否飘右上角奖励数字
    public static void FlyItem(int itemId,int num, bool isAddTip = true)
    {
        fly(itemId,num,Vector3.zero,Msg.Instance.transform, isAddTip);
    }

    public static void FlyItem_Camera(Transform tfStart,int itemId,int num)
    {
        if (_noticTran == null)
        {
            return;
        }
        
        //坐标转换
        var screenPoint = GameObject.Find("UIMainCanvas").GetComponent<Canvas>().worldCamera.WorldToScreenPoint(tfStart.position);
        var worldPoint = UIManager.MainCamera.ScreenToWorldPoint(screenPoint);
        fly(itemId,num,new Vector3(worldPoint.x,worldPoint.y,0), _noticTran.parent,true);
    }





    private static List<string> noticeList = new List<string>();
    private static bool isRunNotice;

    //有奖励的跑马灯
    public static void PlayNotice(string str,int itemId,int itemNum)
    {
        PlayNotice(str.Replace("%s",$"<color=#5eee79>{itemNum}</color>"),itemId);
    }
    //道具id大于零则为有道具奖励的跑马灯
    public static void PlayNotice(string str,int itemId = 0)
    {
        if (itemId > 0)
        {
            noticeList.Add(itemId + "$" + str);
        }
        else
        {
            noticeList.Add(str.Replace("$",""));            
        }
        if (_noticTran == null)
        {
            return;
        }
        if (isRunNotice)
        {
            return;
        }
        show_notice();
    }


    private static void show_notice()
    {
        if (noticeList.Count <= 0)
        {
            _noticTran.gameObject.SetActive(false);
            isRunNotice = false;
            return;
        }

        isRunNotice = true;
        var str = noticeList[0];
        noticeList.RemoveAt(0);
        var move = _noticTran.GetChild(1);
        var lb_str = move.GetChild(0);
        var icon = lb_str.GetChild(0).GetComponent<Image>();
        if (str.Contains("$"))
        {
            var ss = str.Split("$");
            lb_str.SetText(ss[1]);
            icon.SetImage("icon/" + ss[0]);
        }
        else
        {
            lb_str.SetText(str);
        }
        move.localPosition = new Vector3(1200,0,0);
        _noticTran.gameObject.SetActive(true);
        move.DOLocalMove(new Vector3(-700f, 0, 0), 7f).SetEase(Ease.Linear).OnComplete(() =>
        {
            show_notice();
        });
    }
    
    
    
   


    public static void ShowRewardTip(int id,int num)
    {
        if (!_endTran.ContainsKey(id) || _endTran[id] == null)
        {
            return;
        }

        var item = _endTran[id].GetChild(3);
        item.SetText("+" + num);
        DOTween.Kill(item);
        item.localScale = new Vector3(0,0,0);
        item.localPosition = new Vector3(140,0,0);
        item.gameObject.SetActive(true);
        item.DOScale(new Vector3(1, 1, 1), 0.15f).OnComplete(() =>
        {
            item.DOLocalMove(new Vector3(140, 40,0), 1f).OnComplete(() =>
            {
                item.gameObject.SetActive(false);
            });
        });
    }
    

    private static void fly(int id,int num,Vector3 worldPos,Transform parent, bool addTip)
    {
        if (!_endTran.ContainsKey(id) || _endTran[id] == null)
        {
            return;
        }
        int count = Random.Range(4, 6);
        Vector3 randomPos = new Vector2(0, 0);
        var endPos = _endTran[id].GetChild(0).position;

        var foward = new Vector3(0,120,0);
        for (int i = 0; i < count; i++)
        {
            int index = i;
            var go = _poolfly.GetOne().transform;
            go.SetParent(parent);
            go.position = worldPos;
            go.GetChild(0).SetImage("icon/" + id);
            go.SetActive(true);
            var dir = endPos - go.position;
            var time = dir.magnitude / 2500f;
            var angle = (i / (float) count) * 360;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            randomPos = rotation * foward;
            go.localScale = new Vector3(0,0,0);
            DOTween.Sequence()
                .Append(go.DOMove(worldPos + randomPos, 0.25f))
                .Join(go.DOScale(new Vector3(1, 1, 1), 0.15f))
                .AppendInterval(i * 0.1f)
                .Append(go.DOMove(endPos, time))
                .AppendCallback(() =>
                {
                    if (index == 0)
                    {
                        MusicMgr.Instance.PlaySound(3);
                        if (addTip)
                        {
                            ShowRewardTip(id,num);
                        }
                    }
                    _poolfly.RecOne(go.gameObject);
                });
        }
    }

}