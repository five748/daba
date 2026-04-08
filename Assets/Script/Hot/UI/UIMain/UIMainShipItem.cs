using System;
using System.Collections;
using System.Collections.Generic;
using Table;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIMainShipItem : MonoBehaviour
{
    public EventTriggerListener btn_repair;

    public Transform tf_body;
    public Transform tf_ani_body;

    private bool _isMoving = false;

    private Vector3 _firstPos;
    private Vector3 _vDir;

    private Vector3 _aimPos = Vector3.one;
    private float _speed = 0f;

    private ship _tShip = null;

    private bool _needUpdateShip = false;

    [NonSerialized]
    public int ID = int.MinValue;

    public bool IsIdle { get; private set; } = true;

    private ChannelShip _dataShip;

    private readonly Vector3 _initOffset = new Vector3(-37, 76);
    
    private void OnEnable()
    {
        // 20240515 去除船舶检修与主界面的交互
        // btn_repair.onClick = (go) =>
        // {
        //     UIManager.OpenTip("UIFixShipTip", _dataShip.channelId.ToString());
        // };
        btn_repair.gameObject.SetActive(false);
    }

    public void Init(ChannelShip dataShip)
    {
        //船的id 用于表现
        _dataShip = dataShip;
        ID = dataShip.id;
        _tShip = TableCache.Instance.shipTable[ID];
        // Debug.Log($"生成的船舶名字为{t_ship.name}");
        _needUpdateShip = true;
        update_ship_ui();
        update_btn_show();
        IsIdle = false;
        SetIdleAni();
    }

    public void RepairEnd()
    {
        update_btn_show();
        _needUpdateShip = true;
        ID = _dataShip.id;
        _tShip = TableCache.Instance.shipTable[ID];
        update_ship_ui();
        UpdateShipShow();//只修改表现 不修改状态
    }

    private void update_ship_ui()
    {
        if (!_needUpdateShip)
        {
            return;
        }

        _needUpdateShip = false;
        if (_tShip.isFrameAni == "1")
        {
            tf_body.SetActive(true);
            tf_ani_body.SetActive(false);
            tf_body.localScale = new Vector3(_tShip.scale_size, _tShip.scale_size);
            var offset = new Vector3(_tShip.offset[0], _tShip.offset[1]);
            tf_body.localPosition = _initOffset + offset;
        }
        else
        {
            tf_body.SetActive(false);
            tf_ani_body.SetActive(true);
            bool find = false;
            for (int i = 0; i < tf_ani_body.childCount; i++)
            {
                var tf = tf_ani_body.GetChild(i);
                if (tf.name == "ship"+ID)
                {
                    find = true;
                    tf.SetActive(true);
                }
                else
                {
                    tf.SetActive(false);
                }
            }

            if (!find)
            {
                var prefabName = "Ship/ship" + _tShip.icon;
                AssetLoadOld.Instance.LoadPrefab(prefabName, (prefab) =>
                {
                    if (prefab != null)
                    {
                        GameObject go = Instantiate(prefab, tf_ani_body, false);
                        go.transform.localScale = new Vector2(_tShip.scale_size, _tShip.scale_size);
                        var offset = new Vector2(_tShip.offset[0], _tShip.offset[1]);
                        go.transform.localPosition = offset;
                        go.name = "ship" + ID;
                        UpdateShipShow();
                    }
                    else
                    {
                        Debug.Log("找不到对应的prefab" + prefabName);
                    }
                });
            }
        }
    }
    
    public void SetMoveAni()
    {
        if (IsIdle == false)
        {
            return;
        }
        IsIdle = false;
        UpdateShipShow();
    }

    public void SetIdleAni()
    {
        if (IsIdle)
        {
            return;
        }
        IsIdle = true;
        UpdateShipShow();
    }

    private void UpdateShipShow()
    {
        if (_tShip.isFrameAni == "1")
        {
            var img = tf_body.GetComponent<Image>();
            if (_tShip.idleAni == _tShip.moveAni && img.GetComponent<FrameImage>() != null && img.GetComponent<FrameImage>().resDir == _tShip.moveAni)
            {
                return;
            }
            if (ID == 8)
            {
                if (IsIdle)
                {
                    img.PlayOnceEndActiveTrue("chuan8qiehuan", (tf) =>
                    {
                        if (!this)
                        {
                            return;
                        }

                        img.PlayLoop(_tShip.idleAni).isSetSize = true;
                    }).isSetSize = true; 
                }
                else
                {
                    img.PlayOnceEndActiveTrue("chuan8qiehuan2", (tf) =>
                    {
                        if (!this)
                        {
                            return;
                        }
                        img.PlayLoop(_tShip.moveAni).isSetSize = true;
                    }).isSetSize = true; 
                }
            }
            else
            {
                img.PlayLoop(IsIdle ? _tShip.idleAni : _tShip.moveAni).isSetSize = true;
            }
            
            img.SetNativeSize();
            return;
        }
        
        for (int i = 0; i < tf_ani_body.childCount; i++)
        {
            var tf = tf_ani_body.GetChild(i);
            if (tf.name == "ship"+ID)
            {
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }

                if (!tf.gameObject.activeSelf)
                {
                    tf.gameObject.SetActive(true);
                }
                tf.GetComponent<Animator>().Play(IsIdle ? _tShip.idleAni : _tShip.moveAni);
                break;
            }
        }
    }

    private void update_btn_show()
    {
        btn_repair.gameObject.SetActive(_dataShip.isRepair);
    }

    public void UnbindDataShip()
    {
        _dataShip = null;
    }

    public void StartMove()
    {
        _isMoving = true;
    }

    public void StopMove()
    {
        _isMoving = false;
    }

    private void FixedUpdate()
    {
        if (!this)
        {
            return;
        }

        if (!_isMoving)
        {
            return;
        }

        if (_dataShip == null)
        {
            return;
        }

        if (!ChannelMgr.Instance.CheckShipInAimPos(_dataShip))
        {
            SetMoveAni();
        }
        else
        {
            SetIdleAni();
        }

        transform.localPosition = _dataShip.Pos;
    }
}