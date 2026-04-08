using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YuZiSdk;

public class UIStaffOne:BaseMonoBehaviour{
    private UIStaffOneAuto Auto = new UIStaffOneAuto();
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickClose(GameObject button){
        Debug.Log("click" + button.name);
        UIManager.CloseTip();
    }
    public void ClickBtn(GameObject button){
        Debug.Log("click" + button.name);
        bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
        if (isMax) {
            Msg.Instance.Show("已满级!");
            return;
        }
        var needNum = int.Parse(TableCache.Instance.configTable[101].param);
        if (!PlayerMgr.Instance.AddItemNum(2, -needNum))
        {
            UIManager.OpenTip("UIAd_day", "", (str) => {
                Show();
            });
            return;
        }
        SetLvUp();
        SetSumSpeed();
        Msg.Instance.Show("培训成功!");
        ReportOver();
        StaffMgr.Instance.data.SaveToFile();
        Show();
    }
    private void SetLvUp() {
        int lvNum = Random.Range(0, 3) == 2 ? 2 : 1;
        Dictionary<int, int> lvData = new Dictionary<int, int>();
        if (data.oneLv != 5) {
            lvData.Add(1, data.oneLv);
        }
        if (data.twoLv != 5)
        {
            lvData.Add(2, data.twoLv);
        }
        if (data.threeLv != 5)
        {
            lvData.Add(3, data.threeLv);
        }
        if (data.fourLv != 5)
        {
            lvData.Add(4, data.fourLv);
        }
        var keys = new List<int>(lvData.Keys);
        for (int i = 0; i < lvNum; i++)
        {
            if (lvData.Count == 0) {
                break;
            }
            int random = Random.Range(0, lvData.Count);
            int lv = keys[random];
            if (lv == 1)
            {
                data.oneLv++;
            }
            if (lv == 2)
            {
                data.twoLv++;
            }
            if (lv == 3)
            {
                data.threeLv++;
            }
            if (lv == 4)
            {
                data.fourLv++;
            }
            lvData.Remove(lv);
            keys.RemoveAt(random);
        }
    }
    public void ClickBtnad(GameObject button){
        Debug.Log("click" + button.name);
        bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
        if (isMax)
        {
            Msg.Instance.Show("已满级!");
            return;
        }
        SdkMgr.Instance.ShowAd(9,(isSucc) => {
            if (isSucc) {
                data.oneLv = 5;
                data.twoLv = 5;
                data.threeLv = 5;
                data.fourLv = 5;
                SetSumSpeed();
                StaffMgr.Instance.data.SaveToFile();
                Msg.Instance.Show("培训成功!");
                ReportOver();
                Show();
            }
        }, GuideMgr.Instance.InGuide);

    }
    private void ReportOver() {
        //bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
        //if (isMax)
        //{
        //    YuziMgr.Instance.ReportCustom(2009, new Dictionary<string, object>()
        //    {
        //        {"staffLvUpMax", data.staff_id}
        //    });
        //}
    }
    public void ClickOne(GameObject button){
        Debug.Log("click" + button.name);
        ShowChoose(button.transform.position, data.oneLv);
    }
    public void ClickTwo(GameObject button){
        Debug.Log("click" + button.name);
        ShowChoose(button.transform.position, data.twoLv);
    }
    public void ClickThree(GameObject button){
        Debug.Log("click" + button.name);
        ShowChoose(button.transform.position, data.threeLv);
    }
    public void ClickFour(GameObject button){
        Debug.Log("click" + button.name);
        ShowChoose(button.transform.position, data.fourLv);
    }
    private Coroutine Co;
    private void ShowChoose(Vector3 position, int lv) {
        Auto.Choose.SetActive(true);
        Auto.Choose.position = position;
        Auto.StringNum = "过闸速度" + $"+{TableCache.Instance.trainPropTable[lv].prop}%".ChangeColor("1ba37b");
        Co.Stop();
        Co = MonoTool.Instance.Wait(2, () => {
            if (this == null || Auto.Choose == null) {
                return;
            }
            Auto.Choose.SetActive(false);
        });
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private int staffId;
    private StaffItem data {
        get {
            return StaffMgr.Instance.data.staffs[staffId];
        }
    }
    private void Init(string param){
        UIManager.FadeOut();
        staffId = int.Parse(param);
        Show();
        
        GuideMgr.Instance.BindBtn(Auto.Btnad.transform, tableMenu.GuideWindownBtn.staff_lv_up_one_max);
    }
    private void SetSumSpeed() {
        var tab = TableCache.Instance.trainPropTable;
        data.SumAddSpeed = tab[data.oneLv].prop + tab[data.twoLv].prop + tab[data.threeLv].prop + tab[data.fourLv].prop;
        bool isMax = (data.oneLv + data.twoLv + data.threeLv + data.fourLv) == 20;
        if (isMax)
        {
            MTaskData.Instance.AddTaskNum(MainTaskMenu.TrainOneStaffMaxLv, 1);
        }
    }
    private void Show() {
        var tab = TableCache.Instance.trainPropTable;
        var tabOne = TableCache.Instance.tollCollectorTable[staffId];
        Auto.SpriteIcon = "staff/" + tabOne.aniId;
        Auto.Icon.PlayLoop($"staffWait{tabOne.aniId}");
        Auto.StringName = TableCache.Instance.tollCollectorTable[staffId].name;
        Auto.StringMsc = "总属性:过闸速度" + $"+{data.SumAddSpeed}%".ChangeColor("1ba37b");
        Auto.One.transform.SetText($"微笑[{tab[data.oneLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.oneLv].color));
        Auto.Two.transform.SetText($"礼貌[{tab[data.twoLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.twoLv].color));
        Auto.Three.transform.SetText($"着装[{tab[data.threeLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.threeLv].color));
        Auto.Four.transform.SetText($"热情[{tab[data.fourLv].propLv}级]>>".ChangeColor(TableCache.Instance.trainPropTable[data.fourLv].color));
        SetFatherTextColor(Auto.One.transform, TableCache.Instance.trainPropTable[data.oneLv].color);
        SetFatherTextColor(Auto.Two.transform, TableCache.Instance.trainPropTable[data.twoLv].color);
        SetFatherTextColor(Auto.Three.transform, TableCache.Instance.trainPropTable[data.threeLv].color);
        SetFatherTextColor(Auto.Four.transform, TableCache.Instance.trainPropTable[data.fourLv].color);
        var needNum = int.Parse(TableCache.Instance.configTable[101].param);
        var isEnough = PlayerMgr.Instance.IsEnough(2, needNum);
        Auto.Neednum.text = needNum.ToString().ChangeColor(GetNumColorStr(isEnough));
    }
    private void SetFatherTextColor(Transform tran, string color)
    {
        var child = tran.GetChild(0).GetComponent<Text>();
        child.text = "__________".ChangeColor(color);
    }
    private string GetNumColorStr(bool isEnough)
    {
        if (isEnough)
        {
            return "FFFFFF";
        }
        else
        {
            return "a64141";
        }
    }
}





