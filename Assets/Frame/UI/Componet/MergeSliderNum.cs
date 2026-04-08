using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class MergeSliderNum : MonoBehaviour
{
    public void ClickCut(GameObject button)
    {
        GameLog.Log("click" + button.name);
        if (HaveMergeNum == 0)
        {
            return;
        }
        if (UseNum <= 1)
        {
            UseNum = 1;
            return;
        }
        UseNum -= 1;
        Debug.LogError(UseNum);
        ShowText();
        slider.value -= change;
    }
    public void ClickAdd(GameObject button)
    {
        GameLog.Log("click" + button.name);
        if (HaveMergeNum == 0)
        {
            return;
        }
        if (UseNum > HaveMergeNum)
        {
            UseNum = (int)(HaveMergeNum);
            return;
        }
        UseNum += 1;
        ShowText();
        slider.value += change;
    }
    private void ShowText()
    {
        //;
        mergeText.text = MergeStr + UseNum;
    }
    private Slider slider;
    private Text text;
    private Text mergeText;
    public int UseNum = 1;
    private float HaveMergeNum = 4.0f;
    private long HaveNum;
    private float change = 0;
    private int _itemId;
    private long MaxNum = -1;
    private int Step = 1;
    private string ChineseStr = "合成消耗 ";
    private string MergeStr = "合成数量  ";
    private int MergeNum;
    public void InitNum(int itemId, long maxMergeNum, int _step, long haveNum, int firstNum = 1)
    {
        UseNum = 1;
        MaxNum = maxMergeNum;
        Step = _step;
        mergeText = transform.Find("mergeText").GetComponent<Text>();
        mergeText.gameObject.SetActive(true);
        text = transform.Find("text").GetComponent<Text>();
        EventTriggerListener.Get(transform.Find("add").gameObject).onClick = ClickAdd;
        EventTriggerListener.Get(transform.Find("cut").gameObject).onClick = ClickCut;
        _itemId = itemId;
        HaveNum = haveNum;
        HaveMergeNum = Mathf.FloorToInt(1.0f * HaveNum / Step);
        if (MaxNum != -1)
        {
            if (HaveMergeNum > MaxNum)
            {
                HaveMergeNum = MaxNum;
            }
        }
        text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + Step.ChangeNum();
        change = 1.0f * firstNum / HaveMergeNum;
        slider = GetComponent<Slider>();
        slider.enabled = true;
        if (HaveMergeNum == 1)
        {
            slider.value = 1;
            slider.enabled = false;
            text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + Step;
            mergeText.text = MergeStr + "1";
            RemoveListen();
            return;
        }
        if (HaveMergeNum <= 1)
        {
            slider.value = (float)HaveNum / Step;
            slider.enabled = false;
            text.text = ChineseStr + HaveNum.ChangeNum() + "/" + Step;
            mergeText.text = MergeStr + "0";
            RemoveListen();
            return;
        }
        slider.onValueChanged.AddListener(Change);
        Show();
        slider.value = change;

    }
    public void ReSetNum(ulong cutNum, long haveNum)
    {
        if (cutNum == 0)
        {
            HaveMergeNum = haveNum;
        }
        else
        {
            HaveMergeNum -= cutNum;
        }
        if (MaxNum != -1)
        {
            if (HaveMergeNum > MaxNum)
            {
                HaveMergeNum = MaxNum;
            }
        }
        if (HaveMergeNum == 0)
        {
            UseNum = 0;
            slider.value = 0;
            slider.enabled = false;
            text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + 0;
            mergeText.text = MergeStr + "0";
            RemoveListen();
            return;
        }
        UseNum = 1;
        if (HaveMergeNum == 1)
        {
            slider.value = 1;
            slider.enabled = false;
            text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + Step;
            mergeText.text = MergeStr + "1";
            RemoveListen();
            return;
        }
        if (HaveMergeNum <= 1)
        {
            slider.value = 1;
            slider.enabled = false;
            text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + 0;
            mergeText.text = MergeStr + "0";
            RemoveListen();
            return;
        }
        change = 1.0f / HaveMergeNum;
        Show();
    }
    private void Show()
    {
        if (HaveMergeNum == 1)
        {
            slider.value = 1;
            slider.enabled = false;
            text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + Step;
            mergeText.text = MergeStr + "1";
            return;
        }
        else
        {
            ShowText();
        }
        slider.value = UseNum * change;
    }
    private void Change(float value)
    {
        if (HaveMergeNum <= 1)
        {
            return;
        }
        UseNum = Mathf.RoundToInt(HaveMergeNum * value);
        if (UseNum <= 1)
        {
            UseNum = 1;
            slider.value = change;
        }
        else
        {
            slider.value = UseNum * change;
        }
        text.text = ChineseStr + (UseNum * Step).ChangeNum() + "/" + Step;
        ShowText();
    }
    private void RemoveListen()
    {
        Destroy(transform.Find("add").GetComponent<EventTriggerListener>());
        Destroy(transform.Find("cut").GetComponent<EventTriggerListener>());
    }
}





















































