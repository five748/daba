using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Table;
using System.Linq;

public class UIFixShipTip:BaseMonoBehaviour{
    private UIFixShipTipAuto Auto = new UIFixShipTipAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickMask(GameObject button){
        Debug.Log("click" + button.name);
    }
    public void ClickBigger(GameObject button){
        Debug.Log("click" + button.name);
        var tran = GetNearItem();
        if (tran == null || tran.gameObject.activeSelf)
        {
            return;
        }
        Debug.Log("near:" + tran.name);
        var key = int.Parse(tran.name);
        var pmMove = tran.GetComponent<PmMove>();
        pmMove.callback = () => {
            ShowOne(key);
        };
        pmMove.Create = () =>
        {
            var child = tran.GetChild(0);
            child.SetImage("fixicon/" + key, "");
            return child;
        };
        pmMove.endTransform = items[0];
        var scr = Auto.Scrolldown.GetComponent<NewScrollRect>();
        scr.MoveToItemBase(items[0], () => {
            tran.SetActive(true);
            pmMove.enabled = true;
        });
    }
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public List<int> FixIds
    {
        get
        {
            return PlayerMgr.Instance.data.fixItems[shipId];
        }
    }
    public int shipId;
    private int channel_id = 0;
    private void Init(string param){
        UIManager.FadeOut();
        shipId = int.Parse(param);
        channel_id = param.ToInt();
        SetItems();
        InitChildItems();
        ShowItems();
        ShowNum();
    }
    private List<Transform> childItems;
    private List<Transform> items;
    private List<Vector2> shipPos = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(0, 424),
    };
    private void SetItems() {
        List<Transform> posParents = new List<Transform>
        {
            Auto.Pos1,
            Auto.Pos2,
            Auto.Pos3
        };
        Auto.Shipdown.SetImage("fixship/" + shipId, "", true);
        Auto.Shipdtop.SetImage("fixship/" + shipId + shipId, "", true);
        Auto.Shipdown.GetComponent<RectTransform>().anchoredPosition = shipPos[shipId - 1];
        var posParent = posParents[shipId - 1];
        var poses = new List<Vector2>();
        int index = -1;
        posParent.GetAllChild((child) => {
            index++;
            var tran = Auto.Shipdtop.GetChild(index);
            tran.name = child.name;
            tran.GetComponent<RectTransform>().anchoredPosition = child.GetComponent<RectTransform>().anchoredPosition;
        });
    }
    private void InitChildItems() {
        childItems = new List<Transform>();
        for (int i = 0; i < Auto.Shipdtop.childCount; i++)
        {
            var item = Auto.Shipdtop.GetChild(i);
            childItems.Add(item);
            if (FixIds.Contains(int.Parse(item.name))) {
                item.SetActive(true);
                item.ClearChild();
            }
        }
    }
    private void ShowItems() {
        items = Auto.Grid.AddChilds(7).ToList();
        ShowHaveItems();
    }
    private void ShowHaveItems() {
        foreach (var id in FixIds)
        {
            ShowOneBase(id);
        }
    }
    private Transform GetNearItem() {
        Transform minTran = null;
        float minDis = 0;
        foreach (var item in childItems)
        {
            var dis = Vector2.Distance(Auto.Mask.transform.position, item.position);
            if (dis < 100) {
                if (minTran == null) {
                    minDis = dis;
                    minTran = item;
                }
                else {
                    if (dis < minDis) {
                        minDis = dis;
                        minTran = item;
                    }
                }
            }
        }
        return minTran;
    }
    private void ShowOne(int key)
    {
        FixIds.Add(key);
        ShowOneBase(key);
        ShowNum();
        PlayerMgr.Instance.data.SaveToFile();
    }
    private void ShowOneBase(int id) {
        var tran = items[0];
        items.RemoveAt(0);
        tran.SetImage("fixicon/" + id, "icon");
        tran.SetActive(true, "icon");
        tran.SetText(TableCache.Instance.repairTable[id].name, "text");
    }
    private int overNum {
        get {
            return FixIds.Count;
        }
    }
    private void ShowNum() {
        Auto.StringOvernum = "检修安全隐患项(已检修x/7):".Replace("x", overNum.ToString());
        if (overNum == 7) {
            Debug.Log("完成");
            MTaskData.Instance.AddTaskNum(MainTaskMenu.SumFixShipNum);
            var _shipId = TableCache.Instance.repairDungeonTable[shipId].shipId;
            ShipMgr.Instance.data.ships[_shipId].unlock = true;
            ShipMgr.Instance.data.SaveToFile();
            UIManager.CloseAllTip();
            UIManager.OpenTipNoText("UIShipUnlock", _shipId + "_0", (str) =>
            {
                UIFixShipMenu.Instance._CallBack?.Invoke(str);
            });
        }
    }
}






