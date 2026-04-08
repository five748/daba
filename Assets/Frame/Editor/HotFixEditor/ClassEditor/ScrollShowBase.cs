using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScrollShowBase:MonoBehaviour
{
    private int key = -1;
    private List<object> datas;
    private object OnOneData;
    private int OnIndex;
    private int OnRealIndex;
    private List<int> showDatas = new List<int>();
    private Dictionary<int, List<int>> groupDatas;
    private List<int> spcDatas;
    private Vector2 defaultSize;
    private Vector2 keyDefaultSize;
    private LoopScroll loop;
    private int grouId = 1;
    public Transform SpcBtn;
    public Transform[] filtGrids;
    private List<int> GroupKeys;
    public LoopScroll GroupLoop;
    public MenuGrid GronpMenu;
    public Transform[] ChildShows;
    void Start() {
       
    }
    public void Init(List<object> _datas) {
        datas = _datas;
        showDatas.Clear();
        loop = transform.GetComponent<LoopScroll>();
        defaultSize = loop.ItemDefaultSize;
        key = loop.key;
        SetGroupTranAuto();
        SortDataAuto();
        SetGroupDatas();
        SetSpcData();
        if(GroupLoop == null && (filtGrids == null || filtGrids.Length == 0))
            ReShow();
        if(loop.key != -1)
        {
            //if(!AllValueData.Instance.ObjectEvents.ContainsKey(key))
            //{
            //    AllValueData.Instance.ObjectEvents.Add(key, ReShow);
            //}
            //else
            //{
            //    AllValueData.Instance.ObjectEvents[key] += ReShow;
            //}
        }
    }
    private void SetGroupDatas() {
        groupDatas = new Dictionary<int, List<int>>();
        GroupKeys = new List<int>();
        SetGroupDatasAuto();
        ShowGroupMenu();
    }
    private void ShowFiltMenu() {

    }
    public void ShowGroupMenu() {
        if(GroupKeys.Count == 0)
        {
            return;
        }
        SetShowData(GroupKeys[0]);
        if(GronpMenu != null) {
            GronpMenu.ClickEvent = ClickOneKeyNor;
            return;
        }
        if(GroupLoop == null)
            return;
        keyDefaultSize = GroupLoop.ItemDefaultSize;
        GroupLoop.InitItems(GroupKeys.Count, ShowKeyItem, ClickOneKey);
        ReShow();
    }
    private void ClickOneKeyNor(GameObject item, System.Action callback) {
        ClickOneKey(item);
    }
    public Vector2 ShowKeyItem(Transform item, int index, bool isOnlyGetSize) {
        if(!isOnlyGetSize)
        {
            ShowKeyItemOneAuto(item, index);
            item.name = index.ToString();
            return item.GetComponent<RectTransform>().sizeDelta;
        }
        return GetKeyItemSize(index);
    }
    private Vector2 GetKeyItemSize(int index) {
        return defaultSize;
    }
    public void ClickOneKey(GameObject btn) {
        Debug.Log("click:" + btn.name);
        int index = int.Parse(btn.name);
        if(GroupKeys.Contains(index))
        {
            SetShowData(GroupKeys[index]);
        }
        else
        {
            showDatas.Clear();
        }
        ReShow();
    }
    private void SetShowData(int groudId) {
        showDatas = groupDatas[groudId];
    }
    private void SetSpcData() {
        if(SpcBtn == null)
        {
            return;
        }
        spcDatas = new List<int>();
        SetSpcDataAuto();
        EventTriggerListener.Get(SpcBtn).onClick = ShowSpcData;
    }
    private void ShowSpcData(GameObject btn) {
        showDatas = spcDatas;
        ReShow();
    }
    private void ReShow(object ob = null) {
        if(loop.isHaveLoop)
        {
            loop.InitItems(showDatas.Count, ShowItem, ClickItem);
        }
        else
        {
            loop.InitNoScroll(showDatas.Count, ShowItem, ClickItem);
        }
        loop.ClickItemByIndex(OnIndex);
    }
    public void ClickItem(GameObject btn) {
        OnIndex = showDatas[int.Parse(btn.name)];
        OnOneData = datas[OnIndex];
        ShowItemChildAuto();
    }
    public void ShowIndex(int index) {

    }
    public void LoadOther() {

    }
    public Vector2 ShowItem(Transform item, int index, bool isOnlyGetSize) {
        if(!isOnlyGetSize)
        {
            ShowItemOneAuto(item, index);
            return item.GetComponent<RectTransform>().sizeDelta;
        }
        return GetItemSize(index);
    }
    private Vector2 GetItemSize(int index) {
        return defaultSize;
    }
    void OnDestroy() {
        if(key != -1)
        {
            //if(AllValueData.Instance.ObjectEvents.ContainsKey(key))
            //    AllValueData.Instance.ObjectEvents[key] -= ReShow;
        }
    }
    //===================自动生成请勿修改============
    private void SetGroupTranAuto() {
        
    }
    private void SortDataAuto() {

    }
    private void ShowByFilterAuto(List<int> lst) {
        
    }
    private void SetGroupDatasAuto() {
       
    }
    private void SetSpcDataAuto() {
      
    }
    private void ShowItemOneAuto(Transform item, int index) {
      
    }
    private void ShowItemChildAuto() {
        var data = OnOneData;
        foreach(var item in ChildShows)
        {

        }
    }
    private void ShowKeyItemOneAuto(Transform item, int index) {
      
    }
}
