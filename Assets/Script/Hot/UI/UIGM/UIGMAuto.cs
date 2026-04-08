using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class UIGMAuto{
    public Transform List_ships;
    public Text Txt_score;
    public Text Txt_item_id;
    public Text Txt_item_num;
    public Text Txt_game_speed;
    public Text Txt_unlock_channel;
    public Text Txt_ship_channel_id;
    public Text Txt_create_ship_id;
    public Text Txt_jiaotong;
    public GameObject Btn_close;
    public GameObject Btn_openallsystem;
    public GameObject Btn_over_task;
    public GameObject Btn_add_score;
    public GameObject Btn_add_item;
    public GameObject Btn_ship_repair;
    public GameObject Btn_wash_event;
    public GameObject Btn_close_event;
    public GameObject Btn_start_event;
    public GameObject Btn_game_speed;
    public GameObject Btn_unlock_channel;
    public GameObject Btn_reset_call;
    public GameObject Btn_show_ad;
    public GameObject Btn_close_ad;
    public GameObject Btn_add_next_ship;
    public GameObject Btn_jioatong;
    private string _stringTxt_score;
    private string _stringTxt_item_id;
    private string _stringTxt_item_num;
    private string _stringTxt_game_speed;
    private string _stringTxt_unlock_channel;
    private string _stringTxt_ship_channel_id;
    private string _stringTxt_create_ship_id;
    private string _stringTxt_jiaotong;
    public void Init(Transform myTran, UIGM model){
        UITool.Instance.SetAnchor(myTran);
        InitGameObject(myTran);
        AddListen(model);
        SetLanguage(myTran);
    }
    private void InitGameObject(Transform myTran){
        List_ships = myTran.Find("middle/出船/list_ships");
        Txt_score = myTran.Find("middle/积分/InputField (Legacy)/txt_score").GetComponent<Text>();
        Txt_item_id = myTran.Find("middle/道具/InputField (Legacy)/txt_item_id").GetComponent<Text>();
        Txt_item_num = myTran.Find("middle/道具/InputField (Legacy) (1)/txt_item_num").GetComponent<Text>();
        Txt_game_speed = myTran.Find("middle/速度调整/InputField (Legacy)/txt_game_speed").GetComponent<Text>();
        Txt_unlock_channel = myTran.Find("middle/解锁航道/InputField (Legacy)/txt_unlock_channel").GetComponent<Text>();
        Txt_ship_channel_id = myTran.Find("middle/增加船舶/InputField (Legacy)/txt_ship_channel_id").GetComponent<Text>();
        Txt_create_ship_id = myTran.Find("middle/增加船舶/InputField (Legacy) (1)/txt_create_ship_id").GetComponent<Text>();
        Txt_jiaotong = myTran.Find("middle/指挥交通/InputField (Legacy)/txt_jiaotong").GetComponent<Text>();
        Btn_close = myTran.Find("middle/btn_close").gameObject;
        Btn_openallsystem = myTran.Find("middle/任务/btn_openallsystem").gameObject;
        Btn_over_task = myTran.Find("middle/任务/btn_over_task").gameObject;
        Btn_add_score = myTran.Find("middle/积分/btn_add_score").gameObject;
        Btn_add_item = myTran.Find("middle/道具/btn_add_item").gameObject;
        Btn_ship_repair = myTran.Find("middle/船舶维修/btn_ship_repair").gameObject;
        Btn_wash_event = myTran.Find("middle/航道冲洗/btn_wash_event").gameObject;
        Btn_close_event = myTran.Find("middle/关闭副玩法/btn_close_event").gameObject;
        Btn_start_event = myTran.Find("middle/开启副玩法/btn_start_event").gameObject;
        Btn_game_speed = myTran.Find("middle/速度调整/btn_game_speed").gameObject;
        Btn_unlock_channel = myTran.Find("middle/解锁航道/btn_unlock_channel").gameObject;
        Btn_reset_call = myTran.Find("middle/出船/btn_reset_call").gameObject;
        Btn_show_ad = myTran.Find("middle/广告/btn_show_ad").gameObject;
        Btn_close_ad = myTran.Find("middle/广告/btn_close_ad").gameObject;
        Btn_add_next_ship = myTran.Find("middle/增加船舶/btn_add_next_ship").gameObject;
        Btn_jioatong = myTran.Find("middle/指挥交通/btn_jioatong").gameObject;
    }
    void AddListen(UIGM model){
        EventTriggerListener.Get(Btn_close).onClick = model.ClickBtn_close;
        EventTriggerListener.Get(Btn_openallsystem).onClick = model.ClickBtn_openallsystem;
        EventTriggerListener.Get(Btn_over_task).onClick = model.ClickBtn_over_task;
        EventTriggerListener.Get(Btn_add_score).onClick = model.ClickBtn_add_score;
        EventTriggerListener.Get(Btn_add_item).onClick = model.ClickBtn_add_item;
        EventTriggerListener.Get(Btn_ship_repair).onClick = model.ClickBtn_ship_repair;
        EventTriggerListener.Get(Btn_wash_event).onClick = model.ClickBtn_wash_event;
        EventTriggerListener.Get(Btn_close_event).onClick = model.ClickBtn_close_event;
        EventTriggerListener.Get(Btn_start_event).onClick = model.ClickBtn_start_event;
        EventTriggerListener.Get(Btn_game_speed).onClick = model.ClickBtn_game_speed;
        EventTriggerListener.Get(Btn_unlock_channel).onClick = model.ClickBtn_unlock_channel;
        EventTriggerListener.Get(Btn_reset_call).onClick = model.ClickBtn_reset_call;
        EventTriggerListener.Get(Btn_show_ad).onClick = model.ClickBtn_show_ad;
        EventTriggerListener.Get(Btn_close_ad).onClick = model.ClickBtn_close_ad;
        EventTriggerListener.Get(Btn_add_next_ship).onClick = model.ClickBtn_add_next_ship;
        EventTriggerListener.Get(Btn_jioatong).onClick = model.ClickBtn_jioatong;
    }
    public void SetLanguage(Transform myTran){
				//组件:Text 部分
		Text get;
		Transform trf;
		trf= myTran.Find("middle/任务/btn_openallsystem/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/任务/btn_over_task/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/积分/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/积分/btn_add_score/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/道具/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/道具/InputField (Legacy) (1)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/道具/btn_add_item/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/道具/txt_desc_0");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/船舶维修/btn_ship_repair/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/航道冲洗/btn_wash_event/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/关闭副玩法/btn_close_event/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/开启副玩法/btn_start_event/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/速度调整/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/速度调整/btn_game_speed/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/解锁航道/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/解锁航道/btn_unlock_channel/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/解锁航道/txt_desc1");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/出船/list_ships/Viewport/Content/item/btn_call_one/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/出船/list_ships/Viewport/Content/item/btn_call_always/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/出船/btn_reset_call/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/广告/btn_show_ad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/广告/btn_close_ad/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/增加船舶/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/增加船舶/InputField (Legacy) (1)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/增加船舶/btn_add_next_ship/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/增加船舶/txt_desc_0");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/指挥交通/InputField (Legacy)/Placeholder");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}
		trf= myTran.Find("middle/指挥交通/btn_jioatong/text");
		if(trf!=null){
			get=trf.GetComponent<Text>();
			get.text = get.text.ChangePrefabStr();
		}

    }
    public string StringTxt_score{
        get { return _stringTxt_score; }
        set {
            if(_stringTxt_score == value)
                return;
            _stringTxt_score = value;
            SetUILabelTxt_score(_stringTxt_score);
        }
    }
    public string StringTxt_item_id{
        get { return _stringTxt_item_id; }
        set {
            if(_stringTxt_item_id == value)
                return;
            _stringTxt_item_id = value;
            SetUILabelTxt_item_id(_stringTxt_item_id);
        }
    }
    public string StringTxt_item_num{
        get { return _stringTxt_item_num; }
        set {
            if(_stringTxt_item_num == value)
                return;
            _stringTxt_item_num = value;
            SetUILabelTxt_item_num(_stringTxt_item_num);
        }
    }
    public string StringTxt_game_speed{
        get { return _stringTxt_game_speed; }
        set {
            if(_stringTxt_game_speed == value)
                return;
            _stringTxt_game_speed = value;
            SetUILabelTxt_game_speed(_stringTxt_game_speed);
        }
    }
    public string StringTxt_unlock_channel{
        get { return _stringTxt_unlock_channel; }
        set {
            if(_stringTxt_unlock_channel == value)
                return;
            _stringTxt_unlock_channel = value;
            SetUILabelTxt_unlock_channel(_stringTxt_unlock_channel);
        }
    }
    public string StringTxt_ship_channel_id{
        get { return _stringTxt_ship_channel_id; }
        set {
            if(_stringTxt_ship_channel_id == value)
                return;
            _stringTxt_ship_channel_id = value;
            SetUILabelTxt_ship_channel_id(_stringTxt_ship_channel_id);
        }
    }
    public string StringTxt_create_ship_id{
        get { return _stringTxt_create_ship_id; }
        set {
            if(_stringTxt_create_ship_id == value)
                return;
            _stringTxt_create_ship_id = value;
            SetUILabelTxt_create_ship_id(_stringTxt_create_ship_id);
        }
    }
    public string StringTxt_jiaotong{
        get { return _stringTxt_jiaotong; }
        set {
            if(_stringTxt_jiaotong == value)
                return;
            _stringTxt_jiaotong = value;
            SetUILabelTxt_jiaotong(_stringTxt_jiaotong);
        }
    }
    private void SetUILabelTxt_score(string text){
        Txt_score.text = text;
    }
    private void SetUILabelTxt_item_id(string text){
        Txt_item_id.text = text;
    }
    private void SetUILabelTxt_item_num(string text){
        Txt_item_num.text = text;
    }
    private void SetUILabelTxt_game_speed(string text){
        Txt_game_speed.text = text;
    }
    private void SetUILabelTxt_unlock_channel(string text){
        Txt_unlock_channel.text = text;
    }
    private void SetUILabelTxt_ship_channel_id(string text){
        Txt_ship_channel_id.text = text;
    }
    private void SetUILabelTxt_create_ship_id(string text){
        Txt_create_ship_id.text = text;
    }
    private void SetUILabelTxt_jiaotong(string text){
        Txt_jiaotong.text = text;
    }
}
