using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class UIPinTuItem : MonoBehaviour
{

    public int type = 1;//1 在背包  2在地图
    public Image icon;
    public RectTransform rect = null;
    public EventTriggerListener btn_info;
    private List<int[]> grids = new List<int[]>();


    public CargoPintuType config;
    
    public void Init(int id)
    {
        config = TableCache.Instance.CargoPintuTypeTable[id];
        icon.SetImage("pintu/" + id,true);
        btn_info.onDown = o =>
        {
            UIPintuInfo.ins.OnMoveItem(this,config.id);
        };
    }


    public void SetBag()
    {
        type = 1;
        // btn_info.onDown = o =>
        // {
        //     UIPintuInfo.ins.OnMoveItem(this,config.id);
        // };
        // btn_info.onClick = null;
    }
    
    
    public float Width()
    {
        return rect.sizeDelta.x;
    }

    public void SetGrid(List<Vector3> pos)
    {
        type = 2;
        // btn_info.onDown = null;
        // btn_info.onClick = o =>
        // {
        //     UIPintuInfo.ins.GridToBag(this,true);
        // };
        var size = UIPintuInfo.size;
        grids.Clear();
        float minx = 9999;
        float miny = 9999;
        float maxx = -1;
        float maxy = -1;
        for (int i = 0; i < pos.Count; i++)
        {
            var p = pos[i];
            int x = (int)(p.x / size);
            int y =  (int)(p.y / size);
            grids.Add(new []{x,y});
            minx = Mathf.Min(minx, x * size + size / 2);
            miny = Mathf.Min(miny, y * size + size / 2);
            maxx = Mathf.Max(maxx, x * size + size / 2);
            maxy = Mathf.Max(maxy, y * size + size / 2);
        }

        var px = (minx + maxx) / 2;
        var py = (miny + maxy) / 2;
        transform.localPosition = new Vector3(px,py,0);
    }
    
    
    public List<Vector3> Round()
    {
        var size = UIPintuInfo.size;
        var pos = transform.localPosition;
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < config.pos.Length; i++)
        {
            var p = config.pos[i];
            posList.Add(new Vector3(p[0] * size + pos.x,p[1] * size + pos.y));
        }
        return posList;
    }

    public bool Contanins(int x, int y)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            if (grids[i][0] == x && grids[i][1] == y)
            {
                return true;
            }            
        }
        return false;
    }
}