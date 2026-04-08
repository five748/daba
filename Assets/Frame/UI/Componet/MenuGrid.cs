using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MenuGrid:MonoBehaviour {
    public int ChooseChildIndex = -1;
    public Transform Choose;
    public System.Action<GameObject, System.Action> ClickEvent;
    public Transform ViewGrid;
    private Vector3 Modify;
    private GameObject ChooseView;
    private Dictionary<string, GameObject> Views;
    public int LinkShowChildIndex = -1;
    private bool IsFirst = true;
    private GameObject ChooseImage;
    public bool isChangeByBig = false;
    public float oldScan = 0.9f;
    public float bigScan = 1f;
    public Transform ChooseItem;
    public bool isNeedAddChilds = false;
    public bool isUseAlpha = false;
    public bool isCanClickOur = false;
    private void Awake() {
    
    }
    public void Start() {
        if(!IsFirst)
            return;
        if (isNeedAddChilds) {
            return;
        }
        IsFirst = false;
        Init();
    }
    public void Init()
    {
        IsFirst = false;
        AddListen();
        ChooseItem = transform.GetChild(0);
        if (Choose)
        {
            if (LinkShowChildIndex == -1)
                Modify = transform.GetChild(0).position - Choose.position;
            else
                Modify = transform.GetChild(0).GetChild(LinkShowChildIndex).position - Choose.position;
        }
        if (ChooseChildIndex != -1)
        {
            if(!ChooseImage)
                ChooseImage = transform.GetChild(0).GetChild(ChooseChildIndex).gameObject;
        }
        if (ViewGrid)
        {
            Views = new Dictionary<string, GameObject>();
            for(int i = 0; i < ViewGrid.transform.childCount; i++)
            {
                ViewGrid.transform.GetChild(i).SetActive(i == 0);
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                //Debug.LogError(transform.name);
                if (i >= ViewGrid.childCount)
                    break;
                if (LinkShowChildIndex == -1)
                    Views.Add(transform.GetChild(i).name, ViewGrid.GetChild(i).gameObject);
                else
                    Views.Add(transform.GetChild(i).GetChild(LinkShowChildIndex).name, ViewGrid.GetChild(i).gameObject);
            }
            if (!ChooseView)
            {
                ChooseView = Views[transform.GetChild(0).name];
            }
        }
    }
    public void AddListen() {
        GameObject go;
        for(int i = 0; i < transform.childCount; i++)
        {
            if (ViewGrid != null)
            {
                if (i >= ViewGrid.childCount)
                    break;
            }
            if (LinkShowChildIndex == -1)
                go = transform.GetChild(i).gameObject;
            else
                go = transform.GetChild(i).GetChild(LinkShowChildIndex).gameObject;
            EventTriggerListener.Get(go).onClick =  ClickMenu;
        }
    }
    public void ClickMenu(int index) {
        Start();
        Debug.Log("ClickMenu"+index + transform.GetChild(index).gameObject.name);
        ClickMenu(transform.GetChild(index).gameObject);
    }
    public void ClickMenu(GameObject button) {
        //Debug.LogError("ClickMenu");
        if (ClickEvent != null)
        {
            ClickEvent(button, () =>
            {
                ChangeState(button);
            });
        }
        else {
            ChangeState(button);
        }
    }
    private GameObject chooeButton;
    public void ChangeState(GameObject button) {
        if(Choose)
        {
            if(isCanClickOur)
            {
                if(!chooeButton) {
                    chooeButton = transform.GetChild(0).gameObject;
                }
                if(chooeButton == button)
                {
                    Choose.SetActive(!Choose.gameObject.activeSelf);
                }
                else {
                    Choose.SetActive(true);
                    Choose.parent = button.transform;
                    Choose.position = button.transform.position - Modify;
                }
                chooeButton = button;
            }
            else {
                Choose.parent = button.transform;
                Choose.position = button.transform.position - Modify;
            }
          
        }
        if(ChooseChildIndex != -1)
        {
            if(isCanClickOur)
            {
                if(!ChooseImage)
                {
                    ChooseImage = transform.GetChild(0).GetChild(ChooseChildIndex).gameObject;
                }
                var newChooseImage = button.transform.GetChild(ChooseChildIndex).gameObject;
                if(ChooseImage == newChooseImage)
                {
                    ChooseImage.SetActive(!ChooseImage.activeSelf);
                }
                else {
                    ChooseImage.SetActive(false);
                    newChooseImage.SetActive(true);
                }
                ChooseImage = newChooseImage;
            }
            else {
                if(ChooseImage)
                {
                    ChooseImage.SetActive(false);
                }
                ChooseImage = button.transform.GetChild(ChooseChildIndex).gameObject;
                ChooseImage.SetActive(true);
            }
        }
        if(ViewGrid)
        {
            if(ChooseView)
            {
                if(isUseAlpha)
                {
                    ChooseView.GetComponent<CanvasGroup>().alpha = 0;
                }
                else
                {
                    ChooseView.SetActive(false);
                }
            }
            ChooseView = Views[button.name];
            if(isUseAlpha)
            {
                ChooseView.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                ChooseView.SetActive(true);
            }
        }
        if(isChangeByBig)
        {
            ChooseItem.localScale = Vector3.one * oldScan;
            ChooseItem = button.transform;
            ChooseItem.localScale = Vector3.one * bigScan;
        }
    }
}





















































