using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Table;
using UnityEngine;
using UnityEngine.UI;

public class UIDamItem : MonoBehaviour
{
    public Image imgBg;
    
    public Image imgIcon;
    public Image imgIconMask;
    public Image imgLock;
    public Image imgLockDecorate;
    
    public Text txtTitle;
    public Text txtScore;
    
    public EventTriggerListener btnGoto;
    public EventTriggerListener btnCurrent;
    public EventTriggerListener btnUnlock;

    private DamItem _dataDam;
    private dam _tDam;

    private Vector3 _leftPos = new Vector3(0, 0, 30);
    private Vector3 _rightPos = new Vector3(0, 0, -30);
    private Sequence _mySequence;
    private Sequence _mySequenceDecorate;

    public void Init(DamItem dataDam, Action callUpdateUI)
    {
        _dataDam = dataDam;
        _tDam = TableCache.Instance.damTable[dataDam.damId];

        btnGoto.onClick = (_) =>
        {
            //跳转到对应的大坝
            ChannelMgr.Instance.ChangeDam(_tDam.id);
            UIManager.CloseTip();
        };
        btnCurrent.onClick = (_) =>
        {
            Msg.Instance.Show("已处于该大坝中");
        };
        btnUnlock.onClick = (_) =>
        {
            if (PlayerMgr.Instance.data.score >= _tDam.needScore)
            {
                UIManager.OpenTip("UIDamUnlock", _dataDam.damId.ToString());
                ChannelMgr.Instance.UnlockDam(_tDam.id);
                callUpdateUI?.Invoke();
            }
            else
            {
                Msg.Instance.Show($"评分不足");
            }
        };
        
        update_ui();

        if (dataDam.damId == 2)
        {
            //1号默认解锁了 所以引导2号
            GuideMgr.Instance.BindBtn(btnUnlock.transform, tableMenu.GuideWindownBtn.dam_unlock);
        }
    }

    private void update_ui()
    {
        imgBg.SetImage($"damListBg/{(_tDam.id - 1) % 5}");
        imgIcon.SetImage($"dam/{ChannelMgr.Instance.getCurDamIconId(_tDam.id)}");
        txtTitle.text = _tDam.id + "." + _tDam.name;
        hide_all();
        if (_dataDam.unlock)
        {
            show_unlock();
        }
        else
        {
            show_lock();
        }
    }

    private void hide_all()
    {
        imgIconMask.gameObject.SetActive(false);
        btnGoto.gameObject.SetActive(false);
        btnCurrent.gameObject.SetActive(false);
        imgLock.gameObject.SetActive(false);
        imgLockDecorate.gameObject.SetActive(false);
        txtScore.gameObject.SetActive(false);
    }

    private void show_unlock()
    {
        btnGoto.gameObject.SetActive(ChannelMgr.Instance.DamId != _dataDam.damId);
        btnCurrent.gameObject.SetActive(ChannelMgr.Instance.DamId == _dataDam.damId);
    }

    private void show_lock()
    {
        txtScore.gameObject.SetActive(true);
        if (PlayerMgr.Instance.data.score >= _tDam.needScore)
        {
            txtScore.text = $"需要 <color=#24d39f>{_tDam.needScore}</color> 评分";
        }
        else
        {
            txtScore.text = $"需要 <color=#f13f3f>{_tDam.needScore}</color> 评分";
        }
        
        imgIconMask.gameObject.SetActive(true);
        imgLock.gameObject.SetActive(true);

        if (_dataDam.damId != 1)
        {
            var dataPreDam = ChannelMgr.Instance.Data.Dams[_dataDam.damId - 1];
            if (dataPreDam.unlock)
            {
                imgLockDecorate.gameObject.SetActive(true);
                if (_mySequence == null)
                {
                    imgLock.transform.eulerAngles = Vector3.zero;
                    _mySequence = DOTween.Sequence();
                    _mySequence.Append(imgLock.transform.DORotate(_rightPos, 0.1f).SetEase(Ease.OutQuart));
                    _mySequence.Append(imgLock.transform.DORotate(_leftPos, 0.2f).SetEase(Ease.OutQuart));
                    _mySequence.Append(imgLock.transform.DORotate(_rightPos, 0.2f).SetEase(Ease.OutQuart));
                    _mySequence.Append(imgLock.transform.DORotate(_leftPos, 0.2f).SetEase(Ease.OutQuart));
                    _mySequence.Append(imgLock.transform.DORotate(Vector3.zero, 0.1f).SetEase(Ease.OutQuart));
                    _mySequence.SetDelay(0.5f);
                    _mySequence.SetLoops(-1, LoopType.Restart);
                    _mySequence.Play();
                }
                
                if (_mySequenceDecorate == null)
                {
                    imgLockDecorate.color = new Color(1, 1, 1, 0);
                    _mySequenceDecorate = DOTween.Sequence();
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(1, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(0, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(1, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(0, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(1, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.Append(imgLockDecorate.DOFade(0, 0.15f).SetEase(Ease.Linear));
                    _mySequenceDecorate.SetDelay(0.4f);
                    _mySequenceDecorate.SetLoops(-1, LoopType.Restart);
                    _mySequenceDecorate.Play();
                }
            }
            else
            {
                clear_sequence();
                imgLock.transform.eulerAngles = Vector3.zero;
            }
        }
    }

    private void OnDestroy()
    {
        clear_sequence();
    }

    private void clear_sequence()
    {
        if (_mySequence != null)
        {
            _mySequence.Kill();
            _mySequence = null;
        }
        
        if (_mySequenceDecorate != null)
        {
            _mySequenceDecorate.Kill();
            _mySequenceDecorate = null;
        }
    }
}