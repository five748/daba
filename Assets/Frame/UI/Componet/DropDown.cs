using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDown : MonoBehaviour
{
    private List<string> Lst;
    private int ChooseIndex;
    private System.Action<int> ClickMenu;
    private Text mainText;
    private GameObject upBtn;
    private GameObject downBtn;
    private GameObject Template;
    private Transform grid;
    private Transform Mask;
    public void Init(List<string> _lst, int _chooseIndex, System.Action<int> _clickMenu, Transform mask = null)
    {
        Lst = _lst;
        ChooseIndex = _chooseIndex;
        ClickMenu = _clickMenu;
        mainText = transform.Find("Label").GetComponent<Text>();
        upBtn = transform.Find("Arrowup").gameObject;
        downBtn = transform.Find("Arrowdown").gameObject;
        Template = transform.Find("Template").gameObject;
        grid = Template.transform.Find("Viewport/Content");
        Mask = mask;
        EventTriggerListener.Get(transform).onClick = ClickDown;
        ShowScroll();
        Show();
    }
    private void Show()
    {
        mainText.text = Lst[ChooseIndex];
        downBtn.SetActive(true);
        upBtn.SetActive(false);
        Template.SetActive(false);
        if (Mask != null)
            Mask.SetActive(false);
    }
    private void ClickDown(GameObject btn)
    {
        bool isActive = Template.activeSelf;
        downBtn.SetActive(isActive);
        upBtn.SetActive(!isActive);

        Template.SetActive(!isActive);
        if (!isActive)
        {
            for (int i = 0; i < items.Length; i++)
            {
                var item = items[i];
                item.SetActive(true, "nameexit");
                item.SetActive(false, "nameenter");
                item.SetActive(false, "bg");
            }
        }

        if (Mask)
            Mask.SetActive(!isActive);
    }
    Transform[] items;
    private void ShowScroll()
    {
        items = grid.AddChilds(Lst.Count);
        int index = -1;
        foreach (var item in items)
        {
            index++;
            string str = Lst[index];
            item.SetText(str, "nameenter");
            item.SetText(str, "nameexit");
            item.SetActive(true, "nameexit");
            item.SetActive(false, "nameenter");
            item.SetActive(false, "bg");
            EventTriggerListener.Get(item).onClick = (btn) =>
            {
                ChooseIndex = int.Parse(btn.name);
                Show();
                ClickMenu(ChooseIndex);
            };
            EventTriggerListener.Get(item).onEnter = (btn) =>
            {
                item.SetActive(true, "nameenter");
                item.SetActive(false, "nameexit");
                item.SetActive(true, "bg");
            };
            EventTriggerListener.Get(item).onExit = (btn) =>
            {
                item.SetActive(false, "nameenter");
                item.SetActive(true, "nameexit");
                item.SetActive(false, "bg");
            };
        }

        if (Mask != null)
        {
            EventTriggerListener.Get(Mask).onClick = (btn) =>
            {
                Template.SetActive(false);
                Mask.SetActive(false);
            };
        }
    }
}
