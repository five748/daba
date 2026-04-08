using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using UnityEngine.Windows;
#if DY
using StarkSDKSpace;
#endif

public class Inputs : MonoBehaviour, IPointerClickHandler, IPointerExitHandler
{
    public InputField input;
    private bool isShowKeyboard = false;
    public void Awake()
    {
        input = GetComponent<InputField>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick");
        ShowKeyboard();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnPointerExit");
        if (!input.isFocused)
        {
            HideKeyboard();
        }
    }
#if WX
    public void OnInput(WeChatWASM.OnKeyboardInputListenerResult v)
    {
        Debug.Log("onInput");
        Debug.Log(v.value);
      
        if (input.isFocused)
        {
            input.text = v.value;
        }
    }

    public void OnConfirm(WeChatWASM.OnKeyboardInputListenerResult v)
    {
        // 输入法confirm回调
        Debug.Log("onConfirm");
        Debug.Log(v.value);
        HideKeyboard();
    }

    public void OnComplete(WeChatWASM.OnKeyboardInputListenerResult v)
    {
        // 输入法complete回调
        Debug.Log("OnComplete");
        Debug.Log(v.value);
        HideKeyboard();
    }
#endif
#if DY
    public void OnComplete(string str)
    {
        // 输入法complete回调
        Debug.Log("OnComplete");
        HideKeyboard();
    }
#endif
    public string confirmType = "done"; // 可选值有: "done", "next", "search", "go", "send"
    public int maxInputLength = 100; // 最大输入长度
    public bool multiple = false; // 是否多行输入
    private void ShowKeyboard()
    {
        if (!isShowKeyboard)
        {
            isShowKeyboard = true;
#if WX
            WeChatWASM.WX.ShowKeyboard(new WeChatWASM.ShowKeyboardOption()
            {
                defaultValue = "",
                maxLength = 20,
                confirmType = "go"
            });
            //绑定回调
            WeChatWASM.WX.OnKeyboardConfirm(OnConfirm);
            WeChatWASM.WX.OnKeyboardComplete(OnComplete);
            WeChatWASM.WX.OnKeyboardInput(OnInput);
#endif
#if DY
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardCompleteEvent = OnComplete;
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().ShowKeyboard(new StarkSDKSpace.StarkKeyboard.ShowKeyboardOptions()
            {
                maxLength = maxInputLength,
                multiple = multiple,
                defaultValue = "",
                confirmType = confirmType
            });
            StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardInputEvent += OnKeyboardInput;
#endif
        }

    }
#if DY
    private void OnKeyboardInput(string value)
    {
        Debug.Log($"OnKeyboardInput: {value}");
        if (input.isFocused)
        {
            input.text = value;
        }

    }
#endif
    private void HideKeyboard()
    {
        if (isShowKeyboard)
        {
#if WX
            WeChatWASM.WX.HideKeyboard(new WeChatWASM.HideKeyboardOption());
            //删除掉相关事件监听
            WeChatWASM.WX.OffKeyboardInput(OnInput);
            WeChatWASM.WX.OffKeyboardConfirm(OnConfirm);
            WeChatWASM.WX.OffKeyboardComplete(OnComplete);
#endif
            isShowKeyboard = false;
            UnregisterKeyboardEvents();

        }
    }
    private void UnregisterKeyboardEvents()
    {
#if DY
        StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardInputEvent -= OnKeyboardInput;
        StarkSDKSpace.StarkSDK.API.GetStarkKeyboard().onKeyboardCompleteEvent -= OnComplete;
#endif
    }
}
