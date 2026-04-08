using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemValComponent : MonoBehaviour
{
    public Image icon;
    public Text lb_count;
    public EventTriggerListener btn_add;
    public string key;
    [Tooltip("是否是显示消耗 (10/999) ")]
    public bool isCost = false;//是否是消耗

    private int _cost = 0;

    private Action _funAdd = null;//右侧添加按钮的回调

    private void Awake()
    {
        if (icon == null)
        {
            icon = transform.GetChild(0).GetComponent<Image>();
        }
    }

    private void OnEnable()
    {
        PlayerMgr.Instance.FunItemChange += ItemChange;
        UpdateItem();
    }

    private void ItemChange(int itemId, long itemNum)
    {
        if (itemId != key.ToInt())
        {
            return;
        }
        UpdateItem();
    }

    private void OnDisable()
    {
        PlayerMgr.Instance.FunItemChange -= ItemChange;
    }


    public void UpdateItem()
    {
        btn_add.gameObject.SetActive(_funAdd != null);
        var itemId = int.Parse(key);
        var data = PlayerMgr.Instance.data.items;
        var num = data.ContainsKey(itemId) ? data[itemId] : 0;
        lb_count.text = isCost ? $"{_cost}/{num}" : num.ChangeNum();
        icon.SetImage("icon/" + key);

        btn_add.onClick = (go) =>
        {
            _funAdd?.Invoke();
        };
    }

    public void UpdateCost(int cost,int id = 0)
    {
        if (id > 0)
        {
            key = $"{id}";
        }
        isCost = true;
        this._cost = cost;
        UpdateItem();
    }

    public void SetAddCall(Action call)
    {
        _funAdd = call;
        btn_add.gameObject.SetActive(true);
    }
}