using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorHp : MonoBehaviour
{
    int sum = 0;
    //Dictionary<int, hp_color> tab;
    private Text text;
    private Text existtext;
    private Image hp0;
    private Image hp1;
    private List<float> hpPoints = new List<float>();
    long FullHP;
    private int oldIndex;
    private float oldFill;
    private int newIndex;
    private float newFill;
    private float oldCurrentHp = 0;
    private float newCurrentHp;
    private bool isInited;
    private List<Color> colors = new List<Color>();
    private bool isLockHp;
    public void Init(long fullHp, long currentHp, bool _isLockHp = false)
    {
        //Debug.LogError(_isLockHp);
        //InitClear();
        //isLockHp = _isLockHp;
        //tab = TableCache.Instance.hp_colorTable;
        //sum = tab.Count.SumAdd();
        //text = transform.Find("hptext").GetComponent<Text>();
        //hp0 = transform.Find("hpone").GetComponent<Image>();
        //hp1 = transform.Find("hptwo").GetComponent<Image>();
        //existtext = transform.Find("existtext").GetComponent<Text>();
        //FullHP = fullHp;
      
        //int index = 0;
        //for (int i = 0; i < tab.Count + 1; i++)
        //{
        //    index += i;
        //    hpPoints.Add(1.0f * FullHP * index / sum);
        //}
        //foreach (var tablone in tab)
        //{
        //    Color color;
        //    ColorUtility.TryParseHtmlString("#" + tablone.Value.name, out color);
        //    colors.Add(color);
        //}
        //ShowHp(currentHp);
        //SetLockHp();
    }
    private void InitClear() {
        sum = 0;
        hpPoints.Clear();
        colors.Clear();
    }
    public void ShowChangeHp(long change)
    {
        ShowHp((long)newCurrentHp + change);
    }
    private Coroutine Co;
    public void ShowHp(long currentHp)
    {
        if (isLockHp) {
            return;
        }
        newCurrentHp = currentHp;
        for (int i = 0; i < hpPoints.Count - 1; i++)
        {
            if (currentHp >= hpPoints[i] && currentHp <= hpPoints[i + 1])
            {
                newIndex = i;
                newFill = 1.0f * (currentHp - hpPoints[i]) / (hpPoints[i + 1] - hpPoints[i]);
                break;
            }
        }
        if (isInited)
        {
            if (Co != null)
                MonoTool.Instance.StopCoroutine(Co);
            Co = MonoTool.Instance.StartCor(MoveTo());
        }
        else
        {
            isInited = true;
            ShowOldToNewQuick();
        }
    }
    private void ShowOldToNewQuick()
    {
        oldIndex = newIndex;
        oldFill = newFill;
        oldCurrentHp = newCurrentHp;
        ShowFill(oldIndex, oldFill);

        ShowColor(oldIndex);
        ShowText();
    }
    private void ShowFill(int index, float fill)
    {
        if (index == 0)
        {
            hp0.fillAmount = 0;
        }
        else
        {
            hp0.fillAmount = 1;
        }
        hp1.fillAmount = fill;
    }
    private void ShowColor(int index)
    {
        if (index != 0)
        {
            hp0.color = colors[index - 1];
        }
        hp1.color = colors[index];
    }
    private void ShowText()
    {
        if (oldCurrentHp <= 0)
        {
            existtext.text = "x" + newIndex;
            text.text = 0 + "/" + FullHP.ToString().ChangeSize(16);
            hp1.fillAmount = 0;
            return;
        }
        existtext.text = "x" + (newIndex + 1);
        text.text = (long)oldCurrentHp + "/" + FullHP.ToString().ChangeSize(16);
    }
    public IEnumerator MoveTo()
    {
        if (hp1 == null)
        {
            yield break;
        }
        bool isAdd = false;
        if (newIndex == oldIndex)
        {
            isAdd = newFill > oldFill;
        }
        else
        {
            isAdd = newIndex > oldIndex;
        }
        float fillSpeed = 0.01f;
        float hpSpeed = fillSpeed * (hpPoints[oldIndex + 1] - hpPoints[oldIndex]);
        while (true)
        {
            if (hp1 == null)
            {
                yield break;
            }
            if (isAdd)
            {
                oldFill += fillSpeed;
                oldCurrentHp += hpSpeed;
                hp1.fillAmount = oldFill;
                ShowText();
                if (oldIndex == newIndex)
                {
                    if (oldFill >= newFill)
                    {
                        ShowOldToNewQuick();
                        yield break;
                    }
                }
                if (oldFill >= 1)
                {
                    oldIndex++;
                    oldFill = 0;
                    ShowColor(oldIndex);
                    ShowText();
                    MonoTool.Instance.StartCor(MoveTo());
                    yield break;
                }
            }
            else
            {
                oldFill -= fillSpeed;
                oldCurrentHp -= hpSpeed;
                hp1.fillAmount = oldFill;
                ShowText();
                if (oldIndex == newIndex)
                {
                    if (oldFill <= newFill)
                    {
                        ShowOldToNewQuick();
                        yield break;
                    }
                }
                if (oldFill <= 0)
                {
                    oldIndex--;
                    oldFill = 1;
                    ShowColor(oldIndex);
                    ShowText();
                    MonoTool.Instance.StartCor(MoveTo());
                    yield break;
                }
            }
            yield return null;
        }
    }
}
