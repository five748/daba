using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginDian : MonoBehaviour
{
    private Coroutine co_dian = null;
    private Vector2 size = new Vector2(20, 20);
    public Image img_dian;
    private long wait = 1;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        co_dian = StartCoroutine(show_dian());
    }

    private IEnumerator show_dian()
    {
        while (true)
        {
            size.x = (wait % 4) * 20;
            img_dian.GetComponent<RectTransform>().sizeDelta = size; 
            wait++;
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void OnDisable()
    {
        if (co_dian != null)
        {
            StopCoroutine(co_dian);
            co_dian = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
