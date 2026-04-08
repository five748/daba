using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIEmptyAuto{
    public GameObject Redfuli;
    public GameObject Redzs;
    public GameObject Redday;
    public Transform Gold;
    public Transform Diamond;
    public Transform Fly_gold;
    public Transform Redship;
    public Transform Redcollect;
    public Transform Redach;
    public Transform Redcallbusiness;
    public Transform Redlongship;
    public Transform Task;
    public Transform Noover;
    public Transform Over;
    public Transform Redstaff;
    public Transform Redstaffup;
    public Transform Navigate_back;
    public Transform Navigate_send;
    public Transform Navigate_full;
    public Transform Redfix;
    public Transform Redclean;
    public Transform Redjt;
    public Transform Redpt;
    public Transform Notice;
    public Image Img_btn_ship;
    public Text Score_num;
    public Text Ad_time;
    public Text Taskmsc;
    public Text Tasknum;
    public Text Cleantime;
    public GameObject Score;
    public GameObject Btn_mail;
    public GameObject Btn_gm;
    public GameObject Btn_set;
    public GameObject Btn_collect;
    public GameObject Btn_ach;
    public GameObject Btn_callbusiness;
    public GameObject Btn_longship;
    public GameObject Btn_adfuli;
    public GameObject Btn_adzanzhu;
    public GameObject Btn_adday;
    public GameObject Taskbig;
    public GameObject Btn_facility;
    public GameObject Btn_staff;
    public GameObject Btn_staffup;
    public GameObject Btn_nurture;
    public GameObject Btn_navigate;
    public GameObject Btn_map;
    public GameObject Btn_add_scale;
    public GameObject Btn_reduce_scale;
    public GameObject Btn_fix;
    public GameObject Btn_clean;
    public GameObject Btn_jiaotong;
    public GameObject Btn_pintu;
    private string _spriteImg_btn_ship;
    private string _stringScore_num;
    private string _stringAd_time;
    private string _stringTaskmsc;
    private string _stringTasknum;
    private string _stringCleantime;
    public void Init(Transform myTran, UIEmpty model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        Redfuli = myTran.Find("top/gridRT/btn_adfuli/redfuli").gameObject;
        Redzs = myTran.Find("top/gridRT/btn_adzanzhu/redzs").gameObject;
        Redday = myTran.Find("top/gridRT/btn_adday/redday").gameObject;
        Gold = myTran.Find("top/gold");
        Diamond = myTran.Find("top/diamond");
        Fly_gold = myTran.Find("top/fly_gold");
        Redship = myTran.Find("top/score/redship");
        Redcollect = myTran.Find("top/gridLT/btn_collect/redcollect");
        Redach = myTran.Find("top/gridLT/btn_ach/redach");
        Redcallbusiness = myTran.Find("top/gridLT/btn_callbusiness/redcallbusiness");
        Redlongship = myTran.Find("top/gridLT/btn_longship/redlongship");
        Task = myTran.Find("down/task");
        Noover = myTran.Find("down/task/noover");
        Over = myTran.Find("down/task/over");
        Redstaff = myTran.Find("down/ly/btn_staff/redstaff");
        Redstaffup = myTran.Find("down/ly/btn_staffup/redstaffup");
        Navigate_back = myTran.Find("down/ly/btn_navigate/navigate_back");
        Navigate_send = myTran.Find("down/ly/btn_navigate/navigate_send");
        Navigate_full = myTran.Find("down/ly/btn_navigate/navigate_full");
        Redfix = myTran.Find("down/gridRD/btn_fix/redfix");
        Redclean = myTran.Find("down/gridRD/btn_clean/redclean");
        Redjt = myTran.Find("down/gridRD/btn_jiaotong/redjt");
        Redpt = myTran.Find("down/gridRD/btn_pintu/redpt");
        Notice = myTran.Find("notice");
        Img_btn_ship = myTran.Find("top/score/img_btn_ship").GetComponent<Image>();
        Score_num = myTran.Find("top/score/score_num").GetComponent<Text>();
        Ad_time = myTran.Find("top/gridRT/btn_adday/ad_time").GetComponent<Text>();
        Taskmsc = myTran.Find("down/task/taskmsc").GetComponent<Text>();
        Tasknum = myTran.Find("down/task/noover/tasknum").GetComponent<Text>();
        Cleantime = myTran.Find("down/gridRD/btn_clean/cleantime").GetComponent<Text>();
        Score = myTran.Find("top/score").gameObject;
        Btn_mail = myTran.Find("top/btn_mail").gameObject;
        Btn_gm = myTran.Find("top/btn_gm").gameObject;
        Btn_set = myTran.Find("top/gridLT/btn_set").gameObject;
        Btn_collect = myTran.Find("top/gridLT/btn_collect").gameObject;
        Btn_ach = myTran.Find("top/gridLT/btn_ach").gameObject;
        Btn_callbusiness = myTran.Find("top/gridLT/btn_callbusiness").gameObject;
        Btn_longship = myTran.Find("top/gridLT/btn_longship").gameObject;
        Btn_adfuli = myTran.Find("top/gridRT/btn_adfuli").gameObject;
        Btn_adzanzhu = myTran.Find("top/gridRT/btn_adzanzhu").gameObject;
        Btn_adday = myTran.Find("top/gridRT/btn_adday").gameObject;
        Taskbig = myTran.Find("down/taskbig").gameObject;
        Btn_facility = myTran.Find("down/ly/btn_facility").gameObject;
        Btn_staff = myTran.Find("down/ly/btn_staff").gameObject;
        Btn_staffup = myTran.Find("down/ly/btn_staffup").gameObject;
        Btn_nurture = myTran.Find("down/ly/btn_nurture").gameObject;
        Btn_navigate = myTran.Find("down/ly/btn_navigate").gameObject;
        Btn_map = myTran.Find("down/btn_map").gameObject;
        Btn_add_scale = myTran.Find("down/btn_add_scale").gameObject;
        Btn_reduce_scale = myTran.Find("down/btn_reduce_scale").gameObject;
        Btn_fix = myTran.Find("down/gridRD/btn_fix").gameObject;
        Btn_clean = myTran.Find("down/gridRD/btn_clean").gameObject;
        Btn_jiaotong = myTran.Find("down/gridRD/btn_jiaotong").gameObject;
        Btn_pintu = myTran.Find("down/gridRD/btn_pintu").gameObject;
    }
    void AddListen(UIEmpty model){
        EventTriggerListener.Get(Score).onClick = model.ClickScore;
        EventTriggerListener.Get(Btn_mail).onClick = model.ClickBtn_mail;
        EventTriggerListener.Get(Btn_gm).onClick = model.ClickBtn_gm;
        EventTriggerListener.Get(Btn_set).onClick = model.ClickBtn_set;
        EventTriggerListener.Get(Btn_collect).onClick = model.ClickBtn_collect;
        EventTriggerListener.Get(Btn_ach).onClick = model.ClickBtn_ach;
        EventTriggerListener.Get(Btn_callbusiness).onClick = model.ClickBtn_callbusiness;
        EventTriggerListener.Get(Btn_longship).onClick = model.ClickBtn_longship;
        EventTriggerListener.Get(Btn_adfuli).onClick = model.ClickBtn_adfuli;
        EventTriggerListener.Get(Btn_adzanzhu).onClick = model.ClickBtn_adzanzhu;
        EventTriggerListener.Get(Btn_adday).onClick = model.ClickBtn_adday;
        EventTriggerListener.Get(Taskbig).onClick = model.ClickTaskbig;
        EventTriggerListener.Get(Btn_facility).onClick = model.ClickBtn_facility;
        EventTriggerListener.Get(Btn_staff).onClick = model.ClickBtn_staff;
        EventTriggerListener.Get(Btn_staffup).onClick = model.ClickBtn_staffup;
        EventTriggerListener.Get(Btn_nurture).onClick = model.ClickBtn_nurture;
        EventTriggerListener.Get(Btn_navigate).onClick = model.ClickBtn_navigate;
        EventTriggerListener.Get(Btn_map).onClick = model.ClickBtn_map;
        EventTriggerListener.Get(Btn_add_scale).onClick = model.ClickBtn_add_scale;
        EventTriggerListener.Get(Btn_reduce_scale).onClick = model.ClickBtn_reduce_scale;
        EventTriggerListener.Get(Btn_fix).onClick = model.ClickBtn_fix;
        EventTriggerListener.Get(Btn_clean).onClick = model.ClickBtn_clean;
        EventTriggerListener.Get(Btn_jiaotong).onClick = model.ClickBtn_jiaotong;
        EventTriggerListener.Get(Btn_pintu).onClick = model.ClickBtn_pintu;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("top/score/score_des");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/task/over/overstr");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/ly/btn_facility/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/ly/btn_staff/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/ly/btn_staffup/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/ly/btn_nurture/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/ly/btn_navigate/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_map/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_add_scale/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("down/btn_reduce_scale/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("notice/move/value");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string SpriteImg_btn_ship{
        get { return _spriteImg_btn_ship; }
        set {
            if(_spriteImg_btn_ship == value)
                return;
            _spriteImg_btn_ship = value;
            SetUISpriteImg_btn_ship(_spriteImg_btn_ship);
        }
    }
    public string StringScore_num{
        get { return _stringScore_num; }
        set {
            if(_stringScore_num == value)
                return;
            _stringScore_num = value;
            SetUILabelScore_num(_stringScore_num);
        }
    }
    public string StringAd_time{
        get { return _stringAd_time; }
        set {
            if(_stringAd_time == value)
                return;
            _stringAd_time = value;
            SetUILabelAd_time(_stringAd_time);
        }
    }
    public string StringTaskmsc{
        get { return _stringTaskmsc; }
        set {
            if(_stringTaskmsc == value)
                return;
            _stringTaskmsc = value;
            SetUILabelTaskmsc(_stringTaskmsc);
        }
    }
    public string StringTasknum{
        get { return _stringTasknum; }
        set {
            if(_stringTasknum == value)
                return;
            _stringTasknum = value;
            SetUILabelTasknum(_stringTasknum);
        }
    }
    public string StringCleantime{
        get { return _stringCleantime; }
        set {
            if(_stringCleantime == value)
                return;
            _stringCleantime = value;
            SetUILabelCleantime(_stringCleantime);
        }
    }
    private void SetUISpriteImg_btn_ship(string spriteName){
        Img_btn_ship.SetImage(spriteName);
    }
    private void SetUILabelScore_num(string text){
        Score_num.text = text;
    }
    private void SetUILabelAd_time(string text){
        Ad_time.text = text;
    }
    private void SetUILabelTaskmsc(string text){
        Taskmsc.text = text;
    }
    private void SetUILabelTasknum(string text){
        Tasknum.text = text;
    }
    private void SetUILabelCleantime(string text){
        Cleantime.text = text;
    }
}
