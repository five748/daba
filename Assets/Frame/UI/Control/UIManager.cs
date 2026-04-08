using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LitJson;
public class UIManager
{
    public static Transform Root;
    public static RectTransform SceneTalkParent;
    public static GameObject LexMain;
    public static GameObject LexOther;
    public static bool WaitForceOpenUI = false;
    public static GameObject CurrentGo;  //当前显示的UI
    public static Transform TipPanel;
    public static Transform WarnShow;
    public static Transform TipShow;
    public static GameObject TipMask;
    public static GameObject TipMaskText;
    public static EventTriggerListener TipMaskTriger;
    public static CanvasGroup TipMaskCanvasGroup;
    public static Canvas TipMaskDepth;
    public static Canvas TipDepth;
    public static GameObject InstanceParent;
    public static GameObject WarnMsc;
    public static GameObject WarnAddMsc;
    public static GameObject SuccSomeOne;
    public static GameObject FailSomeOne;
    //public static GameObject UIMoney;
    public static string OpenUIName
    {
        get
        {
            return ProssData.Instance.OnUI;
        }
        set
        {
            //Debug.LogError(value);
            ProssData.Instance.OnUI = value;
        }
    }
    private static bool IsOpenMoney;
    private static string Openparam;
    private static string Backparam;
    public static GameObject Power;
    public static GameObject wait;
    public static bool isLastOpenTip;
    private static bool isLoaded = false;
    public static Transform ShowItem;
    public static Transform ShowCityItem;
    public static CanvasGroup showItemCan;
    public static CanvasGroup showCityItemCan;
    public static Transform MainTaskShow;
    public static Text MainText;
    public static GameObject GotoWarn;
    public static Text GotoMsc;
    public static Text GotoMsc1;
    public static GameObject GotoBtn;
    public static Text GotoBtnName;

    public static Transform MirShow;
    public static Image MirImage;
    public static Text MirText;
    public static Transform MirOne;
    public static Transform MirTwo;
    public static Transform MirThree;
    public static Transform OpenSomeTran;

    public static Transform CanvasTopTs;

    public static Dictionary<string, GameObject> CacheUIs = new Dictionary<string, GameObject>();
    public static Dictionary<string, GameObject> GameObjectDict = new Dictionary<string, GameObject>();

    //界面切换信息
    public class ViewData
    {
        public string prefabName;
        public string backparam;
        public bool isOpenMoney;
        public GameObject go;
    }
    // 操作缓存
    private static Stack<ViewData> Cache;
    private static Vector2 _canvasSize = Vector2.zero;
    public static Vector2 CanvasSize
    {
        get
        {
            return _canvasSize;
        }
        set
        {
            //Debug.LogError("setCanvasSize:" + value);
            _canvasSize = value;
        }
    }
    public static bool isSetCanvasSize = false;
    public static Camera UICamera;
    public static Camera MainCamera;
    public static void SetUICamera()
    {
        Root = GameObject.Find("Canvas").transform;
        ProssData.Instance.Root = Root;
        UICamera = Camera.main;
    }
    public static List<string> OpenTipNames
    {
        get
        {
            return ProssData.Instance.Tips;
        }
        set
        {
            ProssData.Instance.Tips = value;
        }
    }
    //设置开始缓存的界面
    public static void InitNavigations(GameObject go, bool isAddMusic = true, int mainTabId = -1)
    {
        //Debug.LogError("InitNavigations");
        Cache = new Stack<ViewData>();
        OpenTipNames = new List<string>();
        CurrentGo = go;
        SetUICamera();
        MainCamera = Camera.main;
        if (!isSetCanvasSize)
        {
            CanvasSize = Root.GetComponent<RectTransform>().sizeDelta;
            ProssData.Instance.CanvasSize = CanvasSize;
            isSetCanvasSize = true;
        }
        if (CanvasSize.x < 100)
        {
            CanvasSize = new Vector2(1334, 750);
        }
        ViewData info = new ViewData();
        OpenUIName = go.name;
        TipShow = Root.Find("TipShow").transform;
        TipMask = TipShow.GetChild(0).gameObject;
        TipMaskText = TipMask.transform.GetChild(0).gameObject;
        TipMaskCanvasGroup = TipMask.GetComponent<CanvasGroup>();
        //Debug.LogError(Root + ":" +TipMask);
        TipMaskDepth = TipMask.GetComponent<Canvas>();
        ProssData.Instance.grey = TipShow.GetComponent<Image>().material;
        //grey = null;
        EventTriggerListener.Get(TipMask).onClick = (btn) =>
        {
            if (!TipMaskText.activeSelf) {
                return;
            }
            CloseTip();
        };
        TipMaskTriger = TipMask.GetComponent<EventTriggerListener>();
        GameObject.DontDestroyOnLoad(TipShow.gameObject);
        info.prefabName = go.name.Replace("(Clone)", "");
        Cache = new Stack<ViewData>();
        //GameObject.Find("CanvasWarn").transform.Find("grid").GetComponent<Msg>().Init();
        Cache.Push(info);
    }
    public static List<string> descs = new List<string>();
    public static bool IsScrollPlay = false;
    public static void ShowAllPeople()
    {
        var notice = Root.Find("CanvasWarn/allnotice");
        if (descs.Count == 0)
        {
            notice.SetActive(false);
            return;
        }
        if (IsScrollPlay)
            return;
        IsScrollPlay = true;
        notice.SetActive(true);
        var desc = descs[0];
        var end = notice.GetComponent<HorseLampCity>();
        end.Init(descs[0]);
        end.callback = () =>
        {
            IsScrollPlay = false;
            descs.RemoveAt(0);
            ShowAllPeople();
        };
    }
    public delegate void OpenUIDelegate();
    private static GameObject oldGo;
    private static bool isOpenUI = true;
    public static bool FirstLogin = true;
    public static void AddToPerent(Transform go, Transform target)
    {
        go.name = go.name.Replace("(Clone)", "");
        go.SetParent(target);
        go.localScale = Vector3.one;
        go.localPosition = Vector3.zero;
        go.localRotation = Quaternion.Euler(Vector3.zero);
        go.gameObject.SetActive(false);
    }
    public static Dictionary<string, string> FliterTipUseDatas = new Dictionary<string, string>();
    public static List<int> flitLst = new List<int>();
    public static void OpenFilter(Dictionary<string, string> _inDatas, System.Action<List<int>> fliteLst)
    {
        FliterTipUseDatas = _inDatas;
        UIManager.OpenTip("UISelectTip", "", (str) =>
        {
            if (str == "1")
                fliteLst(flitLst);
            else
                fliteLst(new List<int>());
        });
    }


    public static void FilterInit()
    {
        flitLst = new List<int>();
    }

    public static void OpenServerOutTimeTip(string param, System.Action<string> callback = null)
    {
        try
        {
            //UIManager.NoticeOld(param, callback);
        }
        catch
        {
            Debug.LogError("ShowCallServeTimeOutErr");
        }
    }
    private static void SetTipDepth(bool isResetMask = true)
    {
        if (ChooseTip == null)
        {
            return;
        }
        if (TipMaskDepth == null)
        {
            return;
        }

        if (isResetMask)
        {
            TipMaskDepth.sortingLayerName = "Tip" + (OpenTipNames.Count + 1);
        }
        var chooseDepth = ChooseTip.GetComponent<UIDepth>();
        if (!chooseDepth)
        {
            chooseDepth = ChooseTip.gameObject.AddComponent<UIDepth>();
        }
        chooseDepth.SetDepthLayout(OpenTipNames.Count + 1);
    }
    public static int TipCount
    {
        get
        {
            return OpenTipNames == null ? 0 : OpenTipNames.Count;
        }
    }
    private static void SetOpenTipDepth(bool isResetMask)
    {
        if (ChooseTip == null)
        {
            return;
        }
        SetTipDepth(isResetMask);
        var depths = ChooseTip.GetComponentsInChildren<UIDepth>(true);
        foreach (var depth in depths)
        {
            depth.SetDepthLayout(OpenTipNames.Count + 1);
        }
    }

    private static bool isLastOpenBigTip = false;
    public static Transform ChooseTip;
    public static void OpenTipNoHaveBig(string name, string param = "", System.Action<string> callback = null,
       System.Action<GameObject> over = null, System.Action fadeoutcallback = null)
    {
        OpenTip(name, param, callback, over, fadeoutcallback, false);
    }
    static List<int> ShowFashion = new List<int>();
    public static void OpenTipNoText(string name, string param = "", System.Action<string> callback = null) {
    
        OpenTip(name, param, callback, null, null, true, false);
    }
    private static Dictionary<string, bool> TipNameHaveText = new Dictionary<string, bool>();
    private static void SetTipNameHaveText(string tipName, bool isHaveText) {
        if (!TipNameHaveText.ContainsKey(tipName))
        {
            TipNameHaveText.Add(tipName, isHaveText);
        }
        else {
            TipNameHaveText[tipName] = isHaveText;
        }
    }
    private static void  CloseSetTipNameHaveText(string tipName) {
        if (TipNameHaveText.ContainsKey(tipName))
        {
            UIManager.TipMaskText.SetActive(TipNameHaveText[tipName]);
        }
    }
    public static void OpenTip(string name, string param = "", System.Action<string> callback = null,
        System.Action<GameObject> over = null, System.Action fadeoutcallback = null, bool isHaveBig = true, bool isHaveText= true)
    {
        SetTipNameHaveText(name, isHaveText);
        if (GoingToName == name)
        {
            return;
        }
        if (EventTriggerListener.IsUIChanging)
        {
            Debug.Log("正在打开");
            return;
        }
        if (OpenTipNames.Contains(name))
        {
            return;
        }
        PlayMusByOpen(name);
        Debug.Log("EventTriggerListener.IsUIChanging :" + EventTriggerListener.IsUIChanging);
        EventTriggerListener.IsUIChanging = true;
        isOpenUI = true;
        isLastOpenBigTip = true;
        isLastOpenTip = true;
        FadeOutCallBack = fadeoutcallback;
        UIManager.TipMaskText.SetActive(isHaveText);
        AssetLoadOld.Instance.LoadUIPrefab(name, (go) =>
        {
            //MusicMgr.Instance.PlaySound(4);
            if (go == null)
            {
                EventTriggerListener.IsUIChanging = false;
                isLastOpenBigTip = false;
                Msg.Instance.Show("功能开发中");
                return;
            }
            OpenUIName = name;
            OpenTipNames.Add(name);
            FadeIn(() =>
            {
                if (EventTriggerListener.IsSceneChanging)
                {
                    return;
                }
                ChooseTip = (GameObject.Instantiate(go, Vector3.zero, Quaternion.Euler(Vector3.zero), TipShow)).transform;
                ChooseTip.name = ChooseTip.name.Replace("(Clone)", "");
                ChooseTip.localPosition = Vector3.zero;
                ChooseTip.gameObject.SetActive(false);
                InitUI(ChooseTip.gameObject, param, callback);
                isLastOpenBigTip = false;
                if (over != null)
                {
                    over(ChooseTip.gameObject);
                }
            });
        });
    }
    public static void CloseAllTip()
    {
        //Debug.LogError("CloseAllTip");
        if (TipMask == null)
        {
            return;
        }
        StopMusByClose(GoingToName);
        GoingToName = "";
        foreach (var item in OpenTipNames)
        {
            var go = TipShow.Find(item);
            if (go)
            {
                Launch.CloseUI(false, go.gameObject, item, "");
            }
        }
        OpenTipNames.Clear();
        TipMask.SetActive(false);
        isLastOpenTip = false;
        OpenUIName = CurrentGo.name;
    }
    public static void CloseAllOldTip()
    {
        GoingToName = "";
        foreach (var item in oldTips)
        {
            var go = TipShow.Find(item);
            if (go)
            {
                Launch.CloseUI(false, go.gameObject, item, "");
                //GameObject.Destroy(go.gameObject);
            }
            OpenTipNames.Remove(item);
        }
        oldTips.Clear();
        TipMask.SetActive(false);
        OpenUIName = CurrentGo.name;
    }
    private static void PlayMusByOpen(string name)
    {

    }
    private static void StopMusByClose(string name)
    {


    }
    public static void CloseTip(System.Action callback)
    {
        CloseTip("", "", callback);
    }
    public static void CloseTip(string str = "0", string tipname = "", System.Action callback = null, bool isUseCloseBack = true, bool closeTipMask = true)
    {
        if (EventTriggerListener.IsUIChanging)
        {
            Debug.Log("EventTriggerListener.IsUIChanging:" + EventTriggerListener.IsUIChanging);
            return;
        }
        GoingToName = "";
        if (OpenTipNames.Count == 0)
        {
            Debug.Log("没有可关闭的Tip");
            //EventTriggerListener.IsLock = false;
            if (callback != null)
            {
                callback();
            }
            return;
        }
        isOpenUI = false;
        string choosename = "";
        if (tipname != "")
        {
            choosename = tipname;
        }
        else
        {
            choosename = OpenTipNames[OpenTipNames.Count - 1];
        }
        if (choosename != "UIPropFromTip")
        {
            if (OpenTipNames.Contains("UIPropFromTip"))
            {
                CloseTip("", "UIPropFromTip");
            }
        }

        StopMusByClose(tipname);
        OpenTipNames.Remove(choosename);
        Transform target = TipShow.Find(choosename);
        //MusicMgr.Instance.PlaySound(5);
        if (target != null)
        {
            Launch.CloseUI(true, target.gameObject, choosename, str, isUseCloseBack);
        }
        if (OpenTipNames.Count == 0)
        {
            isLastOpenTip = false;
            ChooseTip = null;
            if (closeTipMask)
                TipMask.SetActive(false);
            if (JoystickController.Instance != null)
            {
                JoystickController.Instance.isCickDown = false;
            }
            OpenUIName = CurrentGo.name;
        }
        else
        {
            choosename = OpenTipNames[OpenTipNames.Count - 1];
            ChooseTip = TipShow.Find(choosename);
            SetTipDepth();
            if (ChooseTip == null)
            {
                return;
            }
            ChooseTip.GetComponent<RectTransform>().SetAsLastSibling();
            var depths = ChooseTip.GetComponentsInChildren<Canvas>();
            foreach (var depth in depths)
            {
                depth.overrideSorting = true;
            }
            OpenUIName = choosename;
            CloseSetTipNameHaveText(choosename);
        }
        CloseUICallBack?.Invoke();
        if (callback != null)
        {
            callback();
        }
    }
    public static string OpenTipName = "";
    private static bool isCloseTip = false;
    private static string OpenName = "";
    public static string OpenIndex = "";
    private static List<string> oldTips = new List<string>();
    private static bool isFadeOutDestory = true;
    public static void OpenUI(string name, string openparam = "", System.Action callback = null)
    {
        if (GoingToName == name)
        {
            EventTriggerListener.IsUIChanging = false;
            callback?.Invoke();
            return;
        }
        GoingToName = name;
        if (EventTriggerListener.IsUIChanging)
        {
            Debug.Log("正在打开");
            return;
        }
        EventTriggerListener.IsUIChanging = true;
        FadeOutCallBack = callback;
        OpenIndex = "";
        OpenTipName = "";
        if (name.IndexOf("#") != -1)
        {
            string[] strs = name.Split('#');
            name = strs[0];
            OpenIndex = strs[1];
        }
        else if (name.IndexOf("//") != -1)
        {
            name = name.Replace("//", "#");
            string[] strs = name.Split('#');
            ViewData info = new ViewData();
            info.prefabName = strs[0];
            Cache.Push(info);
            name = strs[1];
        }
        else
        {
            string[] strs = name.Split('/');
            if (strs.Length == 1)
            {
                OpenTipName = "";
            }
            else
            {
                name = strs[0];
                OpenTipName = strs[1];
            }
        }
        isOpenUI = true;
        PlayMusByOpen(name);
        OpenUIBase(name, openparam);
    }
    public static string GoingToName = "";
    public static GameObject UICity;
    private static void OpenUIBase(string name, string openparam = "")
    {
        //MusicMgr.Instance.PlaySound(4);
        if (OpenUIName == name)
        {
            if(FadeOutCallBack != null)
            {
                FadeOutCallBack();
            }
            EventTriggerListener.IsUIChanging = false;
            return;
        }
        EventTriggerListener.IsUIChanging = true;
        Openparam = openparam;
        isOpenUI = true;
        isTipToUI = false;
        isLastOpenTip = false;
        //Debug.LogError("openUI:" + name);
        FadeIn(() =>
        {
            LoadPrefab(name, (go) =>
            {
                OpenUIName = name;
                go.SetActive(false);
                Cache.Peek().go = CurrentGo;
                ViewData info = new ViewData();
                oldGo = CurrentGo;
                CurrentGo = go;
                InitUI(go, openparam);
                FadeOutByOpen(() =>
                {
                    info.prefabName = go.name;
                    go.GetComponent<RectTransform>().SetSiblingIndex(2);

                    if (Cache != null)
                        Cache.Push(info);
                });
            });
        });
    }

    public static void LoadPrefab(string name, System.Action<GameObject> callback)
    {
        AssetLoadOld.Instance.LoadUIPrefab(name, (target) =>
        {
            if (target == null)
            {
                EventTriggerListener.IsUIChanging = false;
                Msg.Instance.Show("功能开发中");
                return;
            }
            if (EventTriggerListener.IsSceneChanging)
            {
                return;
            }
            var go = GameObject.Instantiate(target) as GameObject;
            AddToPerentBase(go.transform, Root);
            callback(go);
        });
    }
    public static void LoadOutPrefab(string name, System.Action<GameObject> callback)
    {
        AssetLoadOld.Instance.LoadUIPrefab(name, (target) =>
        {
            //Debug.LogError(target);
            var go = GameObject.Instantiate(target) as GameObject;
            AddToPerentBase(go.transform, Root);
            callback(go);
        });
    }

    private static void AddToPerentBase(Transform go, Transform target)
    {
        if (isTipToUI)
        {
            AddToPerent(go.transform, TipPanel);
            go.transform.SetAsLastSibling();
        }
        else
        {
            AddToPerent(go.transform, Root);
        }
        MusicMgr.Instance.PlaySound(16);
    }
    private static bool isTipToUI;

    private static bool IsCallServeringOld = false;
    public static bool isBackTimeReward = false;
    public static bool isHaveReturnName = true;
    public static void UIBack(string backname = "", System.Action callback = null)
    {
        if (!UIManager.TipShow.gameObject.activeSelf)
        {
            UIManager.TipShow.SetActive(true);
        }
        UIManager.OpenIndex = "";
        var lastName = GoingToName;
        GoingToName = backname;
        if (Cache == null || Cache.Count <= 1)
        {
            if (callback != null)
                callback();
            return;
        }
        if (EventTriggerListener.IsUIChanging)
        {
            if (callback != null)
                callback();
            return;
        }
        EventTriggerListener.IsUIChanging = true;
        //Debug.LogError(EndUIName);
        OpenUIName = "";
        isOpenUI = false;
        Debug.Log(backname);
        //MusicMgr.Instance.PlaySound(5);
        FadeIn(() =>
        {
            ViewData info = null;
            if (Cache.Count > 0)
            {
                if (backname != "" && isHaveReturnName)
                {
                    while (true)
                    {
                        if (Cache.Count == 0)
                        {
                            break;
                        }
                        Cache.Pop();
                        info = Cache.Peek();
                        OpenUIName = Cache.Peek().prefabName;
                        //Debug.Log(OpenUIName);
                        if (OpenUIName == backname)
                            break;
                    }
                }
                else
                {
                    Cache.Pop();
                    info = Cache.Peek();
                    OpenUIName = Cache.Peek().prefabName;
                }

                StopMusByClose(lastName);

                if (info.go != null)
                {
                    oldGo = CurrentGo;
                    Debug.Log(oldGo.name);
                    info.go.SetActive(true);
                    CurrentGo = info.go;
                    //GameObject.Destroy(oldGo);
                    Debug.Log(CurrentGo);
                    isLastOpenTip = false;
                    FadeOut();
                    //MusicMgr.Instance.PlaySound(OpenUIName, false);
                    if (callback != null)
                        callback();
                    //EventTriggerListener.IsUIChanging = false;
                    return;
                }
                LoadPrefab(OpenUIName, (go) =>
                {
                    Debug.Log(OpenUIName);
                    oldGo = CurrentGo;
                    Debug.Log(CurrentGo);
                    CurrentGo = go;
                    InitUI(go, "");
                    if (callback != null)
                        callback();
                });
            }
        });
    }
    public static void CloseUIOrTip(System.Action callback = null)
    {
        if (isLastOpenTip)
        {
            UIManager.CloseTip("", "", callback);
        }
        else
        {
            UIManager.UIBack("", callback);
        }
    }
    public static void UIBack(System.Action callback)
    {
        UIBack("", callback);
    }
    public static CanvasGroup fadeMask;

    //界面挑战淡入淡出
    public static void FadeIn(System.Action callback = null)
    {
        callback();
    }
    public static void FadeOutByOpen(System.Action calback = null)
    {
        if (calback != null)
            calback();
    }
    public static System.Action FadeOutCallBack;
    public static System.Action OnUIOpenCallBack;
    public static System.Action CloseUICallBack;
    public static void FadeOut(System.Action callback = null, bool isbackConnent = false, bool isShowGuide = true, bool isResetMask = true)
    {
        Debug.Log("FadeOut");

        if (WaitForceOpenUI)
        {
            WaitForceOpenUI = false;
        }
     
        if (OpenTipNames.Count == 0)
        {
            TipMask.SetActive(false);
        }
        if (isLastOpenTip)
        {
            if (FadeOutCallBack != null)
            {
                FadeOutCallBack();
                FadeOutCallBack = null;
            }
            if (OnUIOpenCallBack != null)
            {
                OnUIOpenCallBack();
            }

            
            if (TipMask == null)
            {
                EventTriggerListener.IsUIChanging = false;
                return;
            }
            if (ChooseTip == null)
            {
                EventTriggerListener.IsUIChanging = false;
                return;
            }
            SetOpenTipDepth(isResetMask);
            if (isResetMask)
            {
                TipMask.GetComponent<RectTransform>().SetAsLastSibling();
            }
            ChooseTip.GetComponent<RectTransform>().SetAsLastSibling();
            MonoTool.Instance.WaitTwoFrame(() =>
            {
                TipMask.SetActive(true);
                ChooseTip.SetActive(true);
                if (ChooseTip.GetComponent<UINoHaveAni>() == null)
                {
                    ChooseTip.TipBigAndSmallShow(() => {
                        EventTriggerListener.IsUIChanging = false;
                    });
                }
                else {
                    EventTriggerListener.IsUIChanging = false;
                }
                if (callback != null)
                {
                    callback();
                }
            });
            return;
        }
        //Debug.LogError("CurrentGo:" + CurrentGo);
        if (CurrentGo)
        {
            if (isTipToUI)
            {
                CurrentGo.transform.SetParentOverride(Root);
                CurrentGo.transform.SetAsFirstSibling();
            }
            CurrentGo.SetActive(true);
            //var canvas = CurrentGo.GetOrAddComponent<CanvasGroup>();
            //canvas.alpha = 1;
        }
        if (isShowGuide)
        {
            //Debug.Log("fadeoutShowGuide");
            //GuideMgr.Instance.BeginNext();
        }
        if (oldGo)
        {
            Launch.CloseUI(false, oldGo, oldGo.name, "");
        }
        if (isCloseTip)
        {
            isCloseTip = false;
            CloseAllOldTip();
        }
        if (callback != null)
            callback();
        if (!isLastOpenBigTip)
        {
            MonoTool.Instance.WaitEndFrame(() =>
            {
                EventTriggerListener.IsUIChanging = false;
            });
        }
        isFadeOutDestory = true;
        oldGo = null;
        if (FadeOutCallBack != null)
        {
            FadeOutCallBack();
            FadeOutCallBack = null;
        }
        if (OnUIOpenCallBack != null)
        {
            OnUIOpenCallBack();
        }
        //EventTriggerListener.IsUIChanging = false;
    }
    static string lastName;
    public static bool JudgeHideMask()
    {
        if (ChooseTip.name == "UIActivePropFromTip" || ChooseTip.name == "UIPropFromTip")
        {
            if (lastName != "UIBagMain")
                return false;
        }
        lastName = ChooseTip.name;
        return true;
    }
    public static void InitUI(GameObject go, string pranm, System.Action<string> callback = null, System.Action openCallBack = null)
    {
        var baseMono = Launch.InitUI(go.transform, "", pranm, openCallBack);
        baseMono._CallBack = callback;
    }
    public static void Wait(bool bWait)
    {
        EventTriggerListener.IsCallingServer = false;
        if (!wait) return;
        wait.SetActive(bWait);
    }
    public static void ShowSureWarn(string msc, System.Action<string> callback)
    {
        Debug.LogError(msc);
        if (OpenUIName == "UIActiveEyeQuickGame")
        {
            return;
        }
        OpenTip("UIWarnTip", "false_" + msc, callback);
    }
    public static void ShowWarnBool(string msc, System.Action<bool> callback)
    {
        if (OpenUIName == "UIActiveEyeQuickGame")
        {
            return;
        }
        OpenTip("UIWarnTip", "true_" + msc, (str) =>
        {
            callback(str == "1");
        });
    }
    public static void ShowWarn(string msc, System.Action<string> callback)
    {
        if (OpenUIName == "UIActiveEyeQuickGame")
        {
            return;
        }
        OpenTip("UIWarnTip", "true_" + msc, callback);
    }
    public static void ShowWarn(string msc, string leftBtnName, string rightBtnName, System.Action<string> callback, System.Action<GameObject> over = null, System.Action fadeoutcallback = null)
    {
        if (OpenUIName == "UIActiveEyeQuickGame")
        {
            return;
        }
        OpenTip("UIWarnTip", "true_" + msc + "_" + leftBtnName + "_" + rightBtnName, callback, over, fadeoutcallback);
    }
    //开始界面中界面

    public static void OpenTipIn(string name, Transform father, string param = "", System.Action callback = null)
    {
        if (father.childCount != 0)
        {
            if (callback != null)
                callback();
            return;
        }
        AssetLoadOld.Instance.LoadUIPrefab(name, (go) =>
        {
            if (go == null)
            {
                EventTriggerListener.IsUIChanging = false;
                Msg.Instance.Show("功能开发中");
                return;
            }
            MonoTool.Instance.WaitEndFrame(() =>
            {
                var prefab = (GameObject.Instantiate(go) as GameObject).transform;
                prefab.SetParentOverride(father);
                prefab.SetActive(false);
                InitUI(prefab.gameObject, param, null, callback);
            });
        });
    }
    public static void OpenTipAndCloseTip(string openName, string closeName, string param = "", System.Action<string> callback = null,
       System.Action<GameObject> over = null, System.Action fadeoutcallback = null, bool isHaveBig = true)
    {
        OpenTip(openName, param, callback, over, () => {
            RemoveTip(closeName);
            if (fadeoutcallback != null)
            {
                fadeoutcallback();
            }
        }, isHaveBig);
    }
    public static void RemoveTip(string tipname)
    {
        if (OpenTipNames.Count == 0)
        {
            Debug.Log("没有可关闭的Tip");
            return;
        }
        isOpenUI = false;
        StopMusByClose(tipname);
        OpenTipNames.Remove(tipname);
        Transform target = TipShow.Find(tipname);
        if (target != null)
            Launch.CloseUI(false, target.gameObject, tipname, "", false);
    }
    public static void FirstLoadPrefab()
    {
        UIManager.LoadPrefabOne("UIPrefab/UILogin", (go) => {
            go.transform.SetParentOverride(UIManager.Root);
        });
        UIManager.LoadPrefabOne("Other/TipShow", (go) => {
            go.transform.SetParentOverride(UIManager.Root);
        });
        UIManager.LoadPrefabOne("Other/MsgWarn", (go) => {

            go.SetParentOverride(UIManager.Root);
        });
    }
    public static void LoadPrefabOne(string path, System.Action<Transform> callback)
    {
        AssetLoadOld.Instance.LoadPrefab(path, (go) =>
        {
            var one = GameObject.Instantiate(go).transform;
            one.name = one.name.Replace("(Clone)", "");
            callback(one);
        });
    }
}
