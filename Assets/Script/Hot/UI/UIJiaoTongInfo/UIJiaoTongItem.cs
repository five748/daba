using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIJiaoTongItem : MonoBehaviour
{

    public Image icon;
    public EventTriggerListener btn;
    public AnimationCurve curve;
    public int x;
    public int y;
    public int len;
    public int[] dir;
    public bool isMove;

    private void OnEnable()
    {
        btn.onClick = o =>
        {
            UIJiaoTongInfo.ins.ClickMoveChip(this);
        };
    }

    public float[] Init(int[] data)
    {
        x = data[0];
        y = data[1];
        len = data[2];
        var angle = data[3];
        if (angle == 0)
        {
            dir = new[] {0, 1};
        }else if (angle == -90)
        {
            dir = new[] {1, 0};
        }else if (angle == 90)
        {
            dir = new[] {-1, 0};
        }else if(angle == 180)
        {
            dir = new[] {0, -1};
        }else if(angle == 270)
        {
            dir = new[] {1, 0};
        }
        else
        {
            Debug.LogError("配置表错误,船方向错误:" + angle);
            angle = 0;
            dir = new[] {0, 1};
        }
        transform.eulerAngles = new Vector3(0,0,angle);
        icon.SetImage("jiaotong/" + len,true);
        var size = UIJiaoTongInfo.size;
        var rect = transform.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x,len * size);
        transform.localPosition = GetPosByGrid(x,y);
        float x1 = size * x + dir[0] * len;
        float y1 = size * y + dir[1] * len;
        return new[] {x1, y1};
    }

    
    public Vector3 GetPosByGrid(int x,int y)
    {
        var size = UIJiaoTongInfo.size;
        if (dir[1] != 0)
        {
            return new Vector3(size * x, size * y - (dir[1] * size / 2), 0);
        }
        else
        {
            return new Vector3(size * x - (dir[0] * size / 2), size * y, 0);
        }
        
    }

    public Vector3 ToNext()
    {
        x += dir[0];
        y += dir[1];
        isMove = true;
        return GetPosByGrid(x, y);
    }
    
    public int[] GetNext()
    {
        if (dir[0] != 0)
        {
            return new[] {x + (len) * dir[0], y};
        }
        else
        {
            return new[] {x, y + (len) * dir[1]};
        }        
    }


    public bool CheckInGrid(int[] grid)
    {
        if (dir[0] != 0)
        {
            var x1 = dir[0] > 0 ? x : x - len + 1;
            var x2 = dir[0] > 0 ? x + len - 1 : x;
            return grid[1] == y && grid[0] >= x1 && grid[0] <= x2;
        }
        else
        {
            var y1 = dir[1] > 0 ? y : y - len + 1;
            var y2 = dir[1] < 0 ? y : y + len - 1;
            return grid[0] == x && grid[1] >= y1 && grid[1] <= y2;
        }
        
    }

    public bool isInScreen()
    {
        float w = ProssData.Instance.CanvasSize.x;
        float h = ProssData.Instance.CanvasSize.y;
        if (dir[0] > 0)
        {
            return transform.position.x > w / 2;
        }else if (dir[0] < 0)
        {
            return transform.position.x < -w / 2;
        }else if (dir[1] > 0)
        {
            return transform.position.y > h / 2;
        }
        else
        {
            return transform.position.y < -h / 2;
        }

    }


    //抖动
    public void Shake()
    {
        var p = GetPosByGrid(x, y);
        
        if (dir[0] != 0)
        {
            DOTween.Kill(transform, true);
            transform.localPosition = new Vector3(p.x - dir[0] * 40, p.y, 0);
            transform.DOLocalMove(p, 0.15f).SetEase(curve);
        }
        else
        {
            DOTween.Kill(transform, true);
            transform.localPosition = new Vector3(p.x, p.y - dir[1] * 40, 0);
            transform.DOLocalMove(p, 0.15f).SetEase(curve);
        }
    }






}