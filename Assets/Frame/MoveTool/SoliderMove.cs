﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool SetOneEvent(Transform a, bool b);
//动画部分====================================


public class SoliderMove
{
    private int Speed = 50;
    private Transform[] Items;
    private Vector2[] Poses;
    public SetOneEvent SetOne;
    public System.Action<Transform> IdleEvent;
    public System.Action<Transform> MoveEvent;
    public System.Action<Transform, bool, System.Action<bool, bool, bool>> AttEvent;
    private int ofirst = 0;
    private int efirst = 0;
    private Vector2 DiffPos;
    private System.Action OverEvent;

    private Queue<Transform> Owns = new Queue<Transform>();
    private Queue<Transform> Enemys = new Queue<Transform>();

    public SoliderMove(Transform[] items, Vector2[] poses, SetOneEvent setOne, System.Action<Transform> idleEvent, System.Action<Transform> moveEvent, System.Action<Transform, bool, System.Action<bool, bool, bool>> attEvent, System.Action Over)
    {
        DiffPos = poses[1] - poses[0];
        Items = items;
        Poses = poses;
        SetOne = setOne;
        IdleEvent = idleEvent;
        MoveEvent = moveEvent;
        AttEvent = attEvent;
        OverEvent = Over;
        GetNoUseItemAndSet(Owns, Poses[0], true);
        GetNoUseItemAndSet(Enemys, Poses[9], false);
        isBattingOver = false;
        isBatting = false;
        MonoTool.Instance.WaitTwoFrame(() => {
            if (Owns.Count == 0) {
                MoveToEnd(true);
                return;
            }
            if (Enemys.Count == 0) {
                MoveToEnd(false);
                return;
            }
            MoveToNext();
            MoveToNext1();
        });
    }
    private void OMoveToNext(System.Action callback = null)
    {
        ofirst++;
        //Debug.Log(ofirst + ":" + efirst);
        int index = -1;
        int index1 = -1;
        int len = Owns.Count;
     
        foreach (var item in Owns)
        {
            index++;
            MoveEvent(item);
            MonoTool.Instance.MoveTowardRt(item.GetComponent<RectTransform>(), Poses[ofirst - index], Speed * 0.02f, () => {
                index1++;
                if (index1 == len - 1)
                {
                    GetNoUseItemAndSet(Owns, Poses[0], true);
                    
                    if (callback != null)
                        callback();
                }
            });
        }
        if (Owns.Count == 0)
        {
            GetNoUseItemAndSet(Owns, Poses[0], true);

            if (callback != null)
                callback();
        }
    }
    private void EMoveToNext(System.Action callback = null)
    {
        efirst++;
        //Debug.Log(ofirst + ":" + efirst);
        int index1 = -1;
        int len = Enemys.Count;
        int index = 9 - efirst - 1;
        foreach (var item in Enemys)
        {
            index++;
            MoveEvent(item);
            MonoTool.Instance.MoveTowardRt(item.GetComponent<RectTransform>(), Poses[index],  Speed * 0.02f, () => {
                index1++;
                if (index1 == len - 1)
                {
                    GetNoUseItemAndSet(Enemys, Poses[9], false);
                    if (callback != null)
                        callback();
                }
            });
        }
        if (Enemys.Count == 0) {
            GetNoUseItemAndSet(Enemys, Poses[9], false);
            if (callback != null)
                callback();
        }
    }
    private void OMoveToEnd(System.Action callback = null)
    {
        ofirst++;
        //Debug.Log(ofirst + ":" + efirst);
        int index = -1;
        int index1 = -1;
        int len = Owns.Count;
        LastRemove = null;
        foreach (var item in Owns)
        {
            index++;
            MoveEvent(item);
            MonoTool.Instance.MoveTowardRt(item.GetComponent<RectTransform>(), Poses[ofirst - index], Speed * 0.02f, () => {
                index1++;
                if (index1 == len - 1)
                {
                    GetNoUseItemAndSet(Owns, Poses[0], true);
                    if (callback != null)
                        callback();
                }
            });
        }
       
    }
    private void EMoveToEnd(System.Action callback = null)
    {
        efirst++;
        int index1 = -1;
        int len = Enemys.Count;
        int index = 9 - efirst - 1;
        LastRemove = null;
        foreach (var item in Enemys)
        {
            index++;
            MoveEvent(item);
            MonoTool.Instance.MoveTowardRt(item.GetComponent<RectTransform>(), Poses[index], Speed * 0.02f, () => {
                index1++;
                if (index1 == len - 1)
                {
                    GetNoUseItemAndSet(Enemys, Poses[9], false);
                    if (callback != null)
                        callback();
                }
            });
        }
    }
    private void OverOwn() {
       
    }
    private bool isBattingOver = false;
    private bool GetNoUseItemAndSet(Queue<Transform> queue, Vector2 pos, bool isOwn)
    {
        Transform lastitem = null;
        if (isBatting && LastRemove != null)
        {
            lastitem = LastRemove;
        }
        else {
            foreach (var item in Items)
            {
                if (!item.gameObject.activeInHierarchy)
                {
                    lastitem = item;
                    break;
                }
            }
        }

        if (SetOne(lastitem, isOwn))
        {
            lastitem.SetActive(true);
            lastitem.GetComponent<RectTransform>().anchoredPosition = pos;
            queue.Add(lastitem);
            return true;
        }
        else
        {
            Debug.Log("false");
            lastitem.SetActive(false);
            return false;
        }
    }
    private bool isBatting = false;
    private Transform LastRemove;
    private void AttOver(bool isWin, bool isOver, bool isOwnOver)
    {
        if (isBattingOver)
            return;

        if (isOver) {
            if (isWin)
            {
                LastRemove = Enemys.Remove();
            }
            else
            {
                LastRemove = Owns.Remove();
            }

            Debug.Log("BattleOver");
            isBattingOver = true;
            LastRemove.SetActive(false);
            MoveToEnd(isOwnOver);
            return;
        }
        if (isWin)
        {
            LastRemove = Enemys.Remove();
        }
        else
        {
            LastRemove = Owns.Remove();
        }
        LastRemove.SetActive(false);
        if (isWin)
        {
          
            efirst--;
            Debug.Log(ofirst + ":" + efirst);
            if (ofirst != 8)
            {
                OMoveToNext(Atting);
            }
            else
            {
                EMoveToNext(Atting);
            }
        }
        else
        {
           
            ofirst--;
            Debug.Log(ofirst + ":" + efirst);
            if (efirst != 8)
            {
                EMoveToNext(Atting);
            }
            else
            {
                OMoveToNext(Atting);
            }
        }
        Debug.Log("false");
      
    }

    private void MoveToEnd(bool isOwnOver)
    {
        Debug.Log("MoveToEnd");
        if (!isOwnOver)
        {
            if (ofirst >= 8)
            {
                foreach (var item in Owns)
                {
                    IdleEvent(item);
                }
                OverEvent();
                return;
            }
            OMoveToEnd(() => {
                MoveToEnd(isOwnOver);
            });
        }
        else
        {
            if (efirst >= 8)
            {
                foreach (var item in Enemys)
                {
                    IdleEvent(item);
                }
                OverEvent();
                return;
            }
            EMoveToEnd(() => {
                MoveToEnd(isOwnOver);
            });
        }
    }
    private bool MoveIndexxxOver = false;
    public void MoveToNext(System.Action callback = null)
    {
        if (ofirst == 4)
        {
            isBatting = true;
            if (!MoveIndexxxOver)
            {
                Atting();
                MoveIndexxxOver = true;
                if (callback != null)
                {
                    callback();
                }
            }
            return;
        }
        OMoveToNext(() =>
        {
            MoveToNext(callback);
        });

    }
    private void MoveToNext1(System.Action callback = null) {
        if (efirst == 4)
        {
            isBatting = true;
            if (!MoveIndexxxOver) {
                Atting();
                MoveIndexxxOver = true;
                if (callback != null)
                {
                    callback();
                }
            }
            return;
        }
        EMoveToNext(() =>
        {
            MoveToNext1(callback);
        });
    }
    private void Atting()
    {
        int index = -1;
        foreach (var item in Owns)
        {
            index++;
            if (index == 0)
            {
                if (isBattingOver) {
                    return;
                }
                AttEvent(item, true, AttOver);
            }
            else
            {
                IdleEvent(item);
            }
        }
        int index1 = -1;
        foreach (var item in Enemys)
        {
            index1++;
            if (index1 == 0)
            {
                if (isBattingOver)
                {
                    return;
                }
                AttEvent(item, false, AttOver);
            }
            else
            {
                IdleEvent(item);
            }
        }
       
    }
}

























