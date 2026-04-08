﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Jump : MonoBehaviour
{
    public float g = -9.8f; 
    private RectTransform target;
    private Vector2 oldPos;
    public float baseV = 10;
    private float speed = 0;
    // Use this for initialization
    void Start()
    {
        target = transform.GetChild(0).GetComponent<RectTransform>();
        oldPos = target.anchoredPosition;
        //Drop();
    }
    //自由落体
    private void Drop()
    {
        StopCoroutine("UpIE");
        StopCoroutine("DropIE");
        target.anchoredPosition = oldPos;
        StartCoroutine("DropIE");
    }
    
    float oldx = 0;
    public float jumpV = 400;
    private bool isJumping = false;
    private IEnumerator BeginJump()
    {
        isJumping = true;
        float oldtime = Time.time;
        float y = 0;
        float time = 0;
        float rotaZ = 0;
        float changez = 360 / (jumpV / g * 2);
        float addx = 3f;
        float jumpg = g;
        while (true)
        {
            addx *= 0.984f;
            oldx += addx;
            if (oldx > 837)
            {
                oldx = -20;
            }
            if (speed > 0)
            {
                jumpg *= 0.986f;
                changez *= 0.986f;
            }
            else
            {
                //jumpg *= 1.001f;
                //changez *= 1.001f;
            }

            speed = jumpV + jumpg * time;
            time = Time.time - oldtime;
            y = jumpg * time * time * 0.5f + jumpV * time;

            rotaZ += changez * Time.deltaTime;
            //print(target.anchoredPosition);
            if (rotaZ <= -360)
            {
                target.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                target.localRotation = Quaternion.Euler(new Vector3(0, 0, rotaZ));
            }
            if (target.anchoredPosition.y <= 10 && speed < 0)
            {
                isJumping = false;
                target.anchoredPosition = new Vector2(target.anchoredPosition.x, 0);
                yield break;
            }
            target.anchoredPosition = new Vector2(oldx, y);
            yield return null;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isJumping)
                return;
            StartCoroutine("BeginJump");
        }
        if (Input.GetKey(KeyCode.Q))
        {
            oldx = 0;
            target.anchoredPosition = Vector2.zero;
        }
    }
}





















































