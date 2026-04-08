using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using SelfComponent;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class UIMain:BaseMonoBehaviour{
    private UIMainAuto Auto = new UIMainAuto();
    
    private Coroutine _coPlaySound = null;//随机声音

    private Coroutine _coTopCheck = null;//最上层检测
    
    private Canvas _mainCanvas;

    private CameraMove _comCameraMove;
    
    private int _damId = 1;
    
    private GameObjectPool _poolContainers;

    private bool isInit = false;
    
    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------

    public void ClickNavigate_back(GameObject button){
        Debug.Log("click" + button.name);
        MainMgr.Instance.openNavigate?.Invoke();
    }
    public void ClickNavigate_send(GameObject button){
        Debug.Log("click" + button.name);
        MainMgr.Instance.openNavigate?.Invoke();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    
    public override void Destory()
    {
        GmMgr.Instance.OnControlAdShow -= control_ad_show;
        if (_coPlaySound != null)
        {
            StopCoroutine(_coPlaySound);
            _coPlaySound = null;
        }

        if (_coTopCheck != null)
        {
            StopCoroutine(_coTopCheck);
            _coTopCheck = null;
        }
        base.Destory();
    }
    
    private void Init(string param)
    {
        ShowSystemLock();
        _damId = ChannelMgr.Instance.Data.finalDamId;
        ChannelMgr.Instance.SetDamId(_damId);
        
        var canvas = GameObject.Find("UIMainCanvas").GetComponent<Canvas>();
        _mainCanvas = TranTool.GetRootCanvas(gameObject.GetComponent<RectTransform>());
        _mainCanvas.GetComponent<RectTransform>().sizeDelta = canvas.GetComponent<RectTransform>().sizeDelta;
        _mainCanvas.transform.localScale = canvas.transform.localScale;
        
        _comCameraMove = Auto.Touch.GetComponent<CameraMove>(); 
        _comCameraMove.targetCanvas = _mainCanvas;
        _comCameraMove.targetCamera = _mainCanvas.worldCamera;
        _comCameraMove.Init();
        
        init_ui();
        update_ui(_damId);
        ChannelMgr.Instance.RunData();
        bind_model_view();

        _comCameraMove.MoveToTarget(Auto.Channels.GetChild(0).GetComponent<UIMainChannelItem>().tfShoufeizhan);
        
        _coPlaySound = StartCoroutine(play_random_sound());
        _coTopCheck = StartCoroutine(top_check());
        
        //左上角 航运
        _poolContainers = new GameObjectPool("Other/Container");
        create_init_container();
        update_cargo();
        NavigateMgr.Instance.FunNavigateSend = navigate_ship_send;
        NavigateMgr.Instance.FunNavigateBack = navigate_ship_back;
        NavigateMgr.Instance.FunUpdateUICargo += update_cargo;
        NavigateMgr.Instance.ComNavigateShip = Auto.Navigate_ship.GetComponent<UIMainNavigateShip>();
        NavigateMgr.Instance.ComNavigateShip.ChangeToFreeShip();
        NavigateMgr.Instance.StartCheckShipBack();

        //广告牌子
        GmMgr.Instance.OnControlAdShow += control_ad_show;
        MainMgr.Instance.updateZhaoShang = updateZhaoShang;
        
        //航道变更
        ChannelMgr.Instance.ChangeDamUpdate = change_dam_update;
        
        //界面动画
        MainMgr.Instance.ComCameraMove = _comCameraMove;
        MainMgr.Instance.MoveScaleAnimation = move_scale_animation;
        MainMgr.Instance.FunMoveToNavigate = move_to_navigate;
        
        GuideMgr.Instance.BindBtn(Auto.Container_bg, tableMenu.GuideWindownBtn.main_navigate_animation);
        UIManager.FadeOut();
    }
    private void SetSendAndBackBySystemLock() {
        bool isOpen = LockSystem.Instance.GetSystemIsOpen(5);
        int scale = 0;
        if (isOpen)
        {
            scale = 1;
        }
        else {
            scale = 0;
        }
        Auto.Navigate.transform.localScale = Vector3.one * scale;
    }
    private void ShowSystemLock() {
        SetSendAndBackBySystemLock();
        MTaskData.Instance.TaskOverEvent += SetSendAndBackBySystemLock;
    }

    private void init_ui()
    {
        //初始化航道
        Auto.Channels.AddChilds(DataDam.ChannelNum);
        for (int i = 0; i < Auto.Channels.childCount; i++)
        {
            var tfPoint = Auto.Channel_points.GetChild(i);
            var tfChannel = Auto.Channels.GetChild(i);
            tfChannel.position = tfPoint.position;
        }
    }

    private void update_ui(int damId)
    {
        if (!isInit)
        {
            for (int i = 0; i < Auto.Channels.childCount; i++)
            {
                var tfChannel = Auto.Channels.GetChild(i);
                tfChannel.GetComponent<UIMainChannelItem>().Init(damId, i);
            }

            isInit = true;
        }
        else
        {
            for (int i = 0; i < Auto.Channels.childCount; i++)
            {
                ChannelMgr.Instance.FunUpdateChannelUI(i);
            }
        }
        
        Auto.Bg.SetImage("map/" + ChannelMgr.Instance.getCurDamIconId(damId));
        Auto.Zhuangshi_youxia.SetImage("damyx/" + ChannelMgr.Instance.getCurDamIconId(damId));
        Auto.Zhuangshi_zuoshang.SetImage("damzsx/" + ChannelMgr.Instance.getCurDamIconId(damId));
        
        updateZhaoShang(damId);
    }

    private void updateZhaoShang(int damId)
    {
        var zhaoshang_info = PlayerMgr.Instance.GetZhaoshangInfo(damId);
        if (zhaoshang_info == null || zhaoshang_info[1] == 0)
        {
            control_ad_show(false);
        }
        else
        {
            control_ad_show(true);
            Auto.Ad_0.SetImage("zhaoshang/" + zhaoshang_info[1]);
            Auto.Ad_1.SetImage("zhaoshang/" + zhaoshang_info[1]);
        }
    }

    private void change_dam_update(int damId)
    {
        _damId = damId;
        bind_model_view();
        update_ui(_damId);
    }

    private void bind_model_view()
    {
        for (int channelId = 0; channelId < Auto.Channels.childCount; channelId++)
        {
            var com = Auto.Channels.GetChild(channelId).GetComponent<UIMainChannelItem>();
            ChannelMgr.Instance.BindModelView(com, _damId, channelId);
        }
    }

    private IEnumerator play_random_sound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10, 21));
            MusicMgr.Instance.PlaySound(Random.Range(5, 10));
        }
    }

    private IEnumerator top_check()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (UIManager.OpenTipNames.Count > 0)
            {
                if (Auto.Touch.gameObject.activeSelf)
                {
                    Auto.Touch.gameObject.SetActive(false);
                }
            }
            else
            {
                if (!Auto.Touch.gameObject.activeSelf)
                {
                    Auto.Touch.gameObject.SetActive(true);
                }
            }
        }
    }

    private void control_ad_show(bool show)
    {
        Auto.Zhuangshi_ad.SetActive(show);
    }

    private void move_scale_animation(Transform tfTarget, Transform tfRecoveryTarget, float ratio, bool isMin, Action<Action> callback)
    {
        //tfTarget 转一下世界坐标 目前看起来直接用 tfTarget.position 并不准确
        Vector3 pos;
        if (isMin)
        {
            pos = _comCameraMove.MoveScaleCalculate(tfRecoveryTarget, ratio);    
        }
        else
        {
            pos = _comCameraMove.MoveScaleCalculate(tfTarget, ratio);
        }
        
        _comCameraMove.EnterAnimation();
        _mainCanvas.worldCamera.transform.DOMove(pos, 0.5f).OnComplete(() =>
        {
            callback(() =>
            {
                _comCameraMove.OutAnimation();
                if (!isMin)
                {
                    _comCameraMove.ScaleAppointRatio(0.5f);
                }
            });
        });
    }
    
    private IEnumerator upload_ship_animation()
    {
        NavigateMgr.Instance.ComNavigateShip.Upload(true);

        var downTime = 0.3f;
        var nextTime = 0.3f;
        var slideTime = 1f;

        Transform left = Auto.Containers_left.GetChild(0);
        Transform right = Auto.Containers_right.GetChild(0);
        
        upload_ship_container_animation(Auto.Container_left_in.localPosition, Auto.Container_left_out.localPosition, Auto.Containers_left, left, () => { });
        yield return new WaitForSeconds(nextTime);
        upload_ship_container_animation(Auto.Container_right_in.localPosition, Auto.Container_right_out.localPosition, Auto.Containers_right, right, () =>
            {
                var table = TableCache.Instance.cargoShipTable[NavigateMgr.Instance.NavigateShipId];
                Debug.Log($"发送的船舶 用于表现 id为{table.id}");
                Auto.Navigate_ship.GetComponent<Image>().PlayLoop(table.move2Ani);
            }
        );
        yield return new WaitForSeconds(nextTime);
        int index = 8;
        while (index > 0)
        {
            if (index % 2 == 0)
            {
                //偶数
                upload_ship_container_animation(Auto.Container_left_in.localPosition, Auto.Container_left_out.localPosition, Auto.Containers_left);
                yield return new WaitForSeconds(nextTime);
            }
            else
            {
                //奇数
                if (index == 1)
                {
                    upload_ship_container_animation(Auto.Container_right_in.localPosition, Auto.Container_right_out.localPosition, Auto.Containers_right, null, create_init_container);
                }
                else
                {
                    upload_ship_container_animation(Auto.Container_right_in.localPosition, Auto.Container_right_out.localPosition, Auto.Containers_right);
                }
                yield return new WaitForSeconds(nextTime);
            }
            index--;
        }

        yield return new WaitForSeconds(slideTime + downTime);
        
        //这里装船完成了
        Auto.Navigate_ship.localPosition = Auto.Navigate_ship_in.localPosition;
        Auto.Navigate_ship.DOLocalMove(Auto.Navigate_ship_out.localPosition, 3f).OnComplete(() =>
        {
            NavigateMgr.Instance.ComNavigateShip.Upload(false);
            //行驶结束
            NavigateMgr.Instance.NavigateEnd(false);
            
        }).SetEase(Ease.Linear);
    }

    private void upload_ship_container_animation(Vector3 startPos, Vector3 endPos, Transform parent, Transform target = null, Action call = null)
    {
        var downTime = 0.3f;
        var downDis = 150f;
        var slideTime = 1f;
        
        if (target == null)
        {
            target = create_container(startPos, parent).transform;
        }

        target.localPosition = startPos;
        target.DOLocalMove(endPos, slideTime).OnComplete(() =>
        {
            target.DOLocalMoveY(target.localPosition.y - downDis, downTime).OnComplete(() =>
            {
                target.DOKill();
                _poolContainers.RecOne(target.gameObject);
                call?.Invoke();
            }).SetEase(Ease.Linear);
        }).SetEase(Ease.Linear);
    }

    private void navigate_ship_send()
    {
        if (Auto.Navigate_ship.localPosition == Auto.Navigate_ship_in.localPosition)
        {
            MonoTool.Instance.StartCor(upload_ship_animation());
        }
        else
        {
            Auto.Navigate_ship.DOLocalMove(Auto.Navigate_ship_in.localPosition, 3f).OnComplete(() =>
            {
                MonoTool.Instance.StartCor(upload_ship_animation());
            }).SetEase(Ease.Linear);
        }
    }
    
    private void navigate_ship_back()
    {
        if (Auto.Navigate_ship.localPosition != Auto.Navigate_ship_out.localPosition)
        {
            Auto.Navigate_ship.localPosition = Auto.Navigate_ship_out.localPosition;
        }

        var table = TableCache.Instance.cargoShipTable[NavigateMgr.Instance.NavigateShipId];
        Auto.Navigate_ship.GetComponent<Image>().PlayLoop(table.moveAni);
        Auto.Navigate_ship.DOLocalMove(Auto.Navigate_ship_in.localPosition, 3f).OnComplete(() =>
        {
            //行驶结束
            NavigateMgr.Instance.NavigateEnd(true);
            
        }).SetEase(Ease.Linear);
    }
    
    private void create_init_container()
    {
        var go0 = create_container(Auto.Container_right_in.localPosition, Auto.Containers_right);
        go0.transform.SetAsFirstSibling();
        var go1 = create_container(Auto.Container_left_in.localPosition, Auto.Containers_left);
        go1.transform.SetAsFirstSibling();
    }

    private GameObject create_container(Vector3 pos, Transform parent)
    {
        var go = _poolContainers.GetOne();
        go.transform.DOKill();
        go.transform.SetParent(parent);
        go.transform.localPosition = pos;
        go.SetActive(true);
        
        return go;
    }
    
    private void update_cargo()
    {
        if (Auto.Container_progress.gameObject.activeSelf)
        {
            Auto.Container_progress.text = $"{Mathf.CeilToInt(NavigateMgr.Instance.Data.cargoCount)}/{NavigateMgr.Instance.Data.capacity}";
            Auto.Container_full.SetActive(NavigateMgr.Instance.Data.cargoCount >= NavigateMgr.Instance.Data.capacity);
        }
    }

    private void move_to_navigate()
    {
        _comCameraMove.MoveToTarget(Auto.Navigate_ship_in);
    }
}