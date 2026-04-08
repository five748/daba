using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Table;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIMainChannelItem:MonoBehaviour
{
    public Transform tfPingtai;
    public Transform tfDengta;
    public Transform tfDiaoji;
    public Transform tfShoufeizhan;
    public Transform tfHonglvdeng;
    public Transform tfJiushengting;
    public Transform tfLuntai;
    public Transform tfJiqi;
    public Transform tfXiansu;
    public Transform tfQianshui;
    public Transform tfQianmen;
    public Transform tfHoushui;
    public Transform tfHoushuiMan;
    public Transform tfHoumen;
    public Transform tfPosShipIn;
    public Transform tfPosShipInUp;
    public Transform tfPosShipOut0;
    public Transform tfPosShipOut;
    public Transform tfPosShip0;
    public Transform tfPosShip1;
    public Transform tfPosQianshuiDown;
    public Transform tfPosQianshuiUp;
    public Transform tfShoufeizhanBuff;
    
    public Transform content;
    
    //生成物的节点
    public Transform tfShips;
    public Transform tfUnlockAnis;
    //航道消息提示
    public EventTriggerListener btnChannelTip;
    public Text txtChannelTip;
    //倒计时
    public Transform tfDaojishi;
    public Image imgDaojishi;
    //收费站职员
    public Image imgStaff;
    
    [NonSerialized]
    private int _channelId = int.MinValue;
    [NonSerialized]
    private int _damId = int.MinValue;
    
    //动画事件
    private FrameImage _frameQianshui = null;
    private FrameImage _frameQianmen = null;
    private FrameImage _frameHoumen = null;
    private FrameImage _frameStaffWave = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doShipMoveToCenter = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doPushQianShui = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doPushQianShuiShip = null;
    private TweenerCore<float, float, FloatOptions> _doDaoJiShi = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doShipLeave0 = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doShipLeave1 = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doPullQianShui = null;

    private Vector3 _waitDirection = Vector3.one;
    
    private GameObjectPool _poolShips;
    private GameObjectPool _poolUnlockAnis;

    private long _shipNum = 0;

    private ChannelItem _dataChannel => ChannelMgr.Instance.GetChannelData(_damId, _channelId); //获取channel数据

    private ModelDamChannel _modelChannel;

    private void OnDestroy()
    {
        ChannelMgr.Instance.FunBuildUnlockAnimation = null;
        ChannelMgr.Instance.FunUpdateChannelUI = null;
        ChannelMgr.Instance.FunStaffUnlock = null;
    }

    public void Init(int damId, int channelId)
    {
        _channelId = channelId;
        _damId = damId;
        _waitDirection = tfPosShip1.position - tfPosShip0.position;
        ChannelMgr.Instance.SetChannelRelativePos(channelId, tfPosShip0.localPosition, _waitDirection, tfPosShipIn.localPosition);
        
        _poolShips = new GameObjectPool("Other/Ship");
        _poolUnlockAnis = new GameObjectPool("Other/UnlockAni");
        
        btnChannelTip.onClick = (go) =>
        {
            if (!_dataChannel.waitWash)
            {
                return;
            }
            
            UIManager.OpenTip("UICleanTip", channelId.ToString(), (str) =>
            {
                ChannelMgr.Instance.WashEnd(damId, channelId);
            });
        };
        
        show();

        ChannelMgr.Instance.FunBuildUnlockAnimation += OnBuildUnlockAnimation;
        ChannelMgr.Instance.FunUpdateChannelUI += OnUpdateChannelUI;
        ChannelMgr.Instance.FunStaffUnlock += OnStaffUnlock;
        ChannelMgr.Instance.FunShipUnlock += OnShipUnlock;

        btnChannelTip.transform.DOScale(1.2f, 1.5f).SetLoops(-1, LoopType.Yoyo);

        MonoTool.Instance.UpdateCall(1f, () =>
        {
            tfShoufeizhanBuff.SetActive(ADMgr.Instance.CheckHaveBuff());
        });

        bind_guide_btn();
    }
    

    public void BindModel(ModelDamChannel model, int damId)
    {
        _damId = damId;
        _modelChannel = model;
    }

    private void OnUpdateChannelUI(int channelId)
    {
        if (channelId != _channelId)
        {
            return;
        }

        show();
    }

    private void show()
    {
        if (!_dataChannel.closingQianmen || !_dataChannel.openingQianmen)
        {
            tfQianmen.SetImage(_dataChannel.IconQianmen);
        }
        
        // tfQianmen.SetImage(_dataChannel.IconQianmen);
        // tfHoumen.SetImage(_dataChannel.IconHoumen);
        //Debug.Log($"大坝 {_damId} 航道 {_channelId} {_dataChannel.IconQianmen}");
        //Debug.Log($"大坝 {_damId} 航道 {_channelId} {_dataChannel.IconHoumen}");

        if (!_dataChannel.closingHoumen && !_dataChannel.openingHoumen)
        {
            tfHoumen.SetImage(_dataChannel.IconHoumen);
        }

        tfPingtai.SetImage(_dataChannel.IconPingTai);
        tfQianshui.SetImage(_dataChannel.IconQianshui);
        tfHoushui.SetImage(_dataChannel.IconHouShuiKong);
        tfHoushuiMan.SetImage(_dataChannel.IconHouShuiMan);
        tfHoushuiMan.SetActive(_dataChannel.openHoumen && !_dataChannel.closingHoumen);
        tfDengta.SetActive(_dataChannel.dengta);
        tfDiaoji.SetActive(_dataChannel.diaoji);
        tfShoufeizhan.SetActive(_dataChannel.shoufeizhan);
        tfHonglvdeng.SetActive(_dataChannel.honglvdeng);
        tfJiushengting.SetActive(_dataChannel.jiushengting);
        tfLuntai.SetActive(_dataChannel.luntai);
        tfJiqi.SetActive(_dataChannel.jiqi);
        tfXiansu.gameObject.SetActive(_dataChannel.xiansu);
        tfDaojishi.SetActive(_dataChannel.showQianshui && !_dataChannel.openingHoumen && !_dataChannel.openHoumen && !_dataChannel.pullQianshui);
        imgStaff.gameObject.SetActive(_dataChannel.staffId != 0);
        if (_dataChannel.staffId != 0)
        {
            if (_frameStaffWave == null)
            {
                _frameStaffWave = imgStaff.PlayLoop("staffWave"+_dataChannel.staffAniId+"_0");
            }
        }
        
        btnChannelTip.gameObject.SetActive(_dataChannel.waitWash);
    }

    public void MakeNewShip(ChannelShip dataShip)
    {
        var go = get_one_ship();
        dataShip.tf = go.transform;
        go.transform.localPosition = dataShip.Pos; //等待的位置
        
        var com = go.GetComponent<UIMainShipItem>();
        com.Init(dataShip);
        com.StartMove();
    }

    private GameObject get_one_ship()
    {
        var go = _poolShips.GetOne();
        go.transform.SetParent(tfShips);
        go.transform.localScale = Vector3.one;
        go.SetActive(true);
        go.name = "Ship" + _shipNum;
        _shipNum++;

        return go;
    }

    public void ShipMoveToCenter()
    {
        if (_dataChannel.moveShip != null)
        {
            var ship = _dataChannel.moveShip.tf;
            var com = ship.GetComponent<UIMainShipItem>();
            com.transform.DOKill();
            com.SetMoveAni();
            //这里的时间重新计算 因为会从这里开始恢复状态
            float time = (_dataChannel.moveShip.Pos - tfPosShipIn.localPosition).magnitude / ChannelMgr.ShipSpeed;
            _doShipMoveToCenter = ship.DOLocalMove(tfPosShipIn.localPosition, time).OnComplete(() =>
            {
                com.SetIdleAni();
                _modelChannel.CloseQianMen();
            });
        }

        foreach (var ship in _dataChannel.ships)
        {
            ship.tf.GetComponent<UIMainShipItem>().StartMove();
        }
    }
    
    public void CloseQianMen()
    {
        if (_frameQianmen != null)
        {
            Debug.LogError("前门还有动画没清空");
        }
        // Debug.Log("关闭前门");
        ChannelMgr.Instance.DoorQianCloseStart(_damId, _channelId);
        _frameQianmen = tfQianmen.GetComponent<Image>().PlayOnceEndActiveTrue($"closeDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_1", (_) =>
        {
            ChannelMgr.Instance.DoorQianCloseEnd(_damId, _channelId);
            _frameQianmen = null;
            show();
            _modelChannel.PushQianShui();
        });
    }
    
    //注水
    public void PushQianShui()
    {
        if (_frameQianshui != null)
        {
            Debug.LogError("前水还有动画没清空");
        }

        ChannelMgr.Instance.WaterPushStart(_damId, _channelId);
        show();

        clear_do_event(ref _doPushQianShui);
        clear_do_event(ref _doPushQianShuiShip);
        _doPushQianShui = tfQianshui.DOLocalMove(tfPosQianshuiUp.localPosition, ChannelMgr.PushWaterTime);
        _doPushQianShuiShip = _dataChannel.moveShip.tf.DOLocalMove(tfPosShipInUp.localPosition, ChannelMgr.PushWaterTime).OnComplete(() =>
        {
            ChannelMgr.Instance.WaterPushEnd(_damId, _channelId);
            show();
            _modelChannel.DaoJiShi();
        });
    }
    
    public void DaoJiShi()
    {
        show();
        tfQianshui.localPosition = tfPosQianshuiUp.localPosition;
        imgDaojishi.fillAmount = 0;
        clear_do_event(ref _doDaoJiShi);
        _doDaoJiShi = imgDaojishi.DOFillAmount(1, ChannelMgr.Instance.GetOverChannelTime(_damId, _channelId, _dataChannel.moveShip.id)).OnComplete(() =>
        {
            if (_dataChannel.moveShip != null)
            {
                var shipId = _dataChannel.moveShip.id;
                if (shipId != 0)
                {
                    var fee = ChannelMgr.Instance.GetOverChannelFee(_damId, _channelId, _dataChannel.moveShip.id);
                    PlayerMgr.Instance.AddItemNum(1, fee,0);
                    UIFly.FlyItem_Camera(tfDaojishi,1,fee);
                }
            }
                
            //打开后门
            _modelChannel.OpenHouMen();
        });
    }
    
    public void OpenHouMen()
    {
        if (_frameHoumen != null)
        {
            Debug.LogError("后门还有动画没清空");
        }
        ChannelMgr.Instance.DoorHouOpenStart(_damId, _channelId);
        show();
        
        staff_animation(() =>
        {
            _frameStaffWave = imgStaff.PlayLoop("staffWave"+_dataChannel.staffAniId+"_0");
        }, ()=>{});
        
        _frameHoumen = tfHoumen.GetComponent<Image>().PlayOnceEndActiveTrue($"openDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_2", (tf) =>
        {
            ChannelMgr.Instance.DoorHouOpenEnd(_damId, _channelId);
            _frameHoumen = null;
            show();
            _modelChannel.ShipLeave();
        });
    }
    
    public void ShipLeave()
    {
        var com = _dataChannel.moveShip.tf.GetComponent<UIMainShipItem>();
        com.transform.DOKill();
        com.SetMoveAni();
        clear_do_event(ref _doShipLeave0);
        _doShipLeave0 = com.transform.DOLocalMove(tfPosShipOut0.localPosition, ChannelMgr.ShipLeaveFirstTime).SetEase(Ease.Linear).OnComplete(() =>
        {
            // Debug.LogError($"大坝 {_damId} 航道 {_channelId} UI表现 船舶离开了 _doShipLeave0");
            _modelChannel.CloseHouMen();
            // ChannelMgr.Instance.ReclaimChannelShip(_dataChannel.moveShip);
            _dataChannel.moveShip = null;
            clear_do_event(ref _doShipLeave1);
            _doShipLeave1 = com.transform.DOLocalMove(tfPosShipOut.localPosition, ChannelMgr.ShipLeaveSecondTime).OnComplete(() =>
            {
                // Debug.LogError($"大坝 {_damId} 航道 {_channelId} UI表现 船舶离开了 _doShipLeave1");
                com.UnbindDataShip();
                _poolShips.RecOne(com.gameObject);
            });
        });
    }
    
    public void CloseHouMen()
    {
        if (_frameHoumen != null)
        {
            Debug.LogError("后门还有动画没清空");
        }
        ChannelMgr.Instance.DoorHouCloseStart(_damId, _channelId);
        show();
        _frameHoumen = tfHoumen.GetComponent<Image>().PlayOnceEndActiveTrue( $"closeDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_2", (tf) =>
        {
            ChannelMgr.Instance.DoorHouCloseEnd(_damId, _channelId);
            _frameHoumen = null;
            show();
            _modelChannel.PullQianShui();
        });
    }

    //排水
    public void PullQianShui()
    {
        if (_frameQianshui != null)
        {
            Debug.LogError("前水还有动画没清空");
        }
        ChannelMgr.Instance.WaterPullStart(_damId, _channelId);
        show();
        _doPullQianShui = tfQianshui.DOLocalMove(tfPosQianshuiDown.localPosition, ChannelMgr.PushWaterTime).OnComplete(() =>
        {
            ChannelMgr.Instance.WaterPullEnd(_damId, _channelId);
            show();
            //打开前门
            _modelChannel.OpenQianMen();
        });
    }
    
    public void OpenQianMen()
    {
        if (_frameQianmen != null)
        {
            Debug.LogError("前门还有动画没清空");
        }
        ChannelMgr.Instance.DoorQianOpenStart(_damId, _channelId);
        _frameQianmen = tfQianmen.GetComponent<Image>().PlayOnceEndActiveTrue( $"openDoor{ChannelMgr.Instance.getCurDamIconId(_damId)}_1", (_) =>
        {
            ChannelMgr.Instance.DoorQianOpenEnd(_damId, _channelId);
            _frameQianmen = null;
            show();
            ChannelMgr.Instance.TurnEnd(_damId, _channelId);
        });
    }

    private void unlock_ani(Transform tfTarget, bool isMin, int tBuildId, Action call)
    {
        show();
        var tBuild = TableCache.Instance.buildingItemTable[tBuildId];
        // var tip = $"评分+{tBuild.score}";
        
        var go = _poolUnlockAnis.GetOne();
        go.transform.SetParent(tfUnlockAnis);
        go.transform.localPosition = new Vector2(tfTarget.localPosition.x + tBuild.effectPos[0], tfTarget.localPosition.y + tBuild.effectPos[1]);
        go.transform.localScale = new Vector3(tBuild.effectSize, tBuild.effectSize);
        go.SetActive(false);

        MainMgr.Instance.MoveScaleAnimation(tfTarget, tfShoufeizhan, isMin ? 0.7f:0.5f, isMin, (callRecovery) =>
        {
            go.SetActive(true);
            go.GetComponent<Image>().PlayOnceEndActiveTrue( tableMenu.frame.starFlash.ToString(), (tf) =>
            {
                // Msg.Instance.Show(tip);
                _poolUnlockAnis.RecOne(go);
                call();
                MusicMgr.Instance.PlaySound(2);
                
                //复原
                callRecovery?.Invoke();
            });
        });
    }

    private void OnBuildUnlockAnimation(int damId, int channelId, int buildId, int tBuildId)
    {
        if (channelId != _channelId)
        {
            return;
        }

        bool isMin = !(damId == 1 && channelId == 0 && buildId is 0 or 1 or 2);

        //建筑id转换为对应的建筑
        switch (buildId)
        {
            case 0:
                unlock_ani(tfShoufeizhan, isMin, tBuildId, ()=>{});
                break;
            case 1:
                unlock_ani(tfJiushengting, isMin, tBuildId, ()=>{});
                break;
            case 2:
                unlock_ani(tfLuntai, isMin, tBuildId, ()=>{});
                break;
            case 3:
                unlock_ani(tfDiaoji, isMin, tBuildId, ()=>{});
                break;
            case 4:
                unlock_ani(tfDengta, isMin,  tBuildId,()=> {});
                break;
            case 5:
                unlock_ani(tfJiqi, isMin,  tBuildId,()=>{});
                break;
            case 6:
                unlock_ani(tfHonglvdeng, isMin, tBuildId, ()=> {});
                break;
            case 7:
                unlock_ani(tfXiansu, isMin, tBuildId, ()=> {});
                break;
        }
    }

    private void OnStaffUnlock(int staffId)
    {
        var tStaff = TableCache.Instance.tollCollectorTable[staffId];
        if (tStaff.forChannelId - 1 != _channelId)
        {
            return;
        }
        
        show();
        imgStaff.SetImage("staff/"+_dataChannel.staffAniId);
        bool isMin = staffId > 1;
        MainMgr.Instance.MoveScaleAnimation(imgStaff.transform, tfShoufeizhan, isMin ? 0.7f:0.5f, isMin, (callRecovery) =>
        {
            staff_animation(() =>
            {
                _frameStaffWave = imgStaff.PlayLoop("staffWave"+_dataChannel.staffAniId+"_0");
            }, callRecovery);
        });
    }
    
    private void OnShipUnlock(int shipId)
    {
        var correctId = shipId == 2 ? 0 : 3;
        if (_channelId != correctId)
        {
            return;
        }
        
        bool isMin = shipId > 2;
        MainMgr.Instance.MoveScaleAnimation(tfShoufeizhan, tfShoufeizhan, isMin ? 0.7f:0.5f, isMin, (callRecovery) =>
        {
            callRecovery?.Invoke();
        });
    }

    private void staff_animation(Action call, Action callRecovery)
    {
        if (_dataChannel.staffId == 0)
        {
            callRecovery?.Invoke();
            return;
        }

        _frameStaffWave = imgStaff.PlayOnceEndActiveTrue("staffWave"+_dataChannel.staffAniId, (_) =>
        {
            _frameStaffWave = null;
            show();
            call?.Invoke();
            callRecovery?.Invoke();
        });
    }

    public void ClearALlEvent()
    {
        clear_frame_image(ref _frameQianshui);
        clear_frame_image(ref _frameQianmen);
        clear_frame_image(ref _frameHoumen);
        clear_frame_image(ref _frameStaffWave);
        clear_do_event(ref _doShipMoveToCenter);
        clear_do_event(ref _doPushQianShui);
        clear_do_event(ref _doPushQianShuiShip);
        clear_do_event(ref _doDaoJiShi);
        clear_do_event(ref _doShipLeave0);
        clear_do_event(ref _doShipLeave1);
        clear_do_event(ref _doPullQianShui);

        tfQianshui.localPosition = tfPosQianshuiDown.localPosition;

        // if (_dataChannel.moveShip != null)
        // {
        //     _dataChannel.moveShip.tf.DOKill();
        // }
    }

    private void clear_frame_image(ref FrameImage fi)
    {
        if (fi != null)
        {
            fi.OverEvent = null;
            fi.Stop();
            fi = null;
        }
    }

    private void clear_do_event(ref TweenerCore<Vector3, Vector3, VectorOptions> doEvent)
    {
        if (doEvent != null)
        {
            // doEvent.OnComplete(() =>
            // {
            //     Debug.Log($"虽然被Kill了 但是还在执行");
            // });
            doEvent.Kill();
            doEvent = null;
        }
    }
    
    private void clear_do_event(ref TweenerCore<float, float, FloatOptions> doEvent)
    {
        if (doEvent != null)
        {
            doEvent.Kill();
            doEvent = null;
        }
    }

    public void RecoveryShips()
    {
        //先清空当前船舶
        var countShip = tfShips.childCount;
        for (int i = 0; i < countShip; i++)
        {
            var tf = tfShips.GetChild(i);
            if (tf.gameObject.activeSelf)
            {
                var com = tf.GetComponent<UIMainShipItem>();
                com.UnbindDataShip();
                com.transform.DOKill();
                _poolShips.RecOne(com.gameObject);
            }
        }
        
        if (_dataChannel.moveShip != null)
        {
            var go = get_one_ship();
            var com = go.GetComponent<UIMainShipItem>();
            _dataChannel.moveShip.tf = go.transform;
            com.Init(_dataChannel.moveShip);
            com.StopMove();
            // com.SetIdleAni();
            //恢复的位置需要根据实际情况来判定 三种情况
            if (_dataChannel.openQianmen)
            {
                //在进入闸门的过程中
                go.transform.localPosition = _dataChannel.moveShip.Pos;
            }
            else if (!_dataChannel.openQianmen && !_dataChannel.showQianshui)
            {
                //进入闸门后 注水前
                go.transform.localPosition = tfPosShipIn.localPosition;
            }
            else if (!_dataChannel.openQianmen && _dataChannel.showQianshui)
            {
                //进入闸门后 注水后
                go.transform.localPosition = tfPosShipInUp.localPosition;
            }
        }

        var count = _dataChannel.ships.Count;
        for (int i = 0; i < count; i++)
        {
            var ship = _dataChannel.ships[i];
            var go = get_one_ship();
            var com = go.GetComponent<UIMainShipItem>();
            ship.tf = go.transform;
            go.transform.localPosition = ship.Pos;
            com.Init(ship);
            if (!ChannelMgr.Instance.CheckShipInAimPos(ship))
            {
                com.StartMove();
                com.SetMoveAni();
            }
        }
    }

    private void bind_guide_btn()
    {
        if (_damId == 1 && _channelId == 0)
        {
            GuideMgr.Instance.BindBtn(tfShoufeizhan, tableMenu.GuideWindownBtn.main_shoufeizhan);
        }
    }
}