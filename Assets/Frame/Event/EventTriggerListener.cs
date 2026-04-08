using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;

public class EventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler, IPointerEnterHandler
{
    public bool is3D;
    public string GotoPrefabPath;
    public int key = -1;
    public bool isInArray = false;
    public List<int> keyIndexs;
    private static bool _islock = false;
    public static bool IsLock
    {
        get
        {
            return _islock;
        }
        set
        {
            //Debug.LogError(value);
            _islock = value;
        }
    }
    private static bool _isFollowing;
    public static bool IsFollowing
    {
        get
        {
            return _isFollowing;
        }
        set
        {
            //Debug.LogError(value);
            _isFollowing = value;
        }
    }
    public static bool isGuideLock = false;
    public static bool isBuyLock = false;
    private static bool isClicking = false;
    private static bool _isCallingServer;
    public static bool IsCallingServer
    {
        get
        {
            return _isCallingServer;
        }
        set
        {
            _isCallingServer = value;
        }
    }
    private static bool _isUIChanging = false;
    public static bool IsUIChanging
    {
        get
        {
            return _isUIChanging;
        }
        set
        {
            //Debug.LogError("IsUIChanging" + value);
            _isUIChanging = value;
        }
    }
    private static bool _isSceneChanging = false;
    public static bool IsSceneChanging
    {
        get
        {
            return _isSceneChanging;
        }
        set
        {
            _isSceneChanging = value;
        }
    }

    private bool IsCanClick = true;
    public System.Action<GameObject> _onClick;
    public List<System.Action<GameObject>> oldClick = new List<System.Action<GameObject>>();
    public System.Action<GameObject, PointerEventData> onClickEvent;//需要同时注册onClick
    public System.Action<GameObject> onClick
    {
        get
        {
            return _onClick;
        }
        set
        {
            //Debug.LogError(value);
            _onClick = value;
        }
    }

    public System.Action<GameObject> onDown;
    public System.Action<GameObject> onPress;
    public System.Action<GameObject> onEnter;
    public System.Action<GameObject> onExit;
    public System.Action<GameObject> onUp;
    public System.Action<GameObject> onSelect;
    public System.Action<GameObject> onUpdateSelect;
    public UnityEngine.Events.UnityAction onClickButton;
    public static bool isFree
    {
        get
        {
            bool isfree = !IsFollowing && !IsLock && !IsCallingServer && !IsUIChanging && !isGuideLock && !IsSceneChanging;
            if (!isfree)
            {
                if (IsFollowing)
                {
                    Debug.Log("IsFollowing:true");
                }
                if (IsLock)
                {
                    Debug.Log("IsLock:true");
                }
                if (IsCallingServer)
                {
                    Debug.Log("IsCallingServer:true");
                }
                if (IsUIChanging)
                {
                    Debug.Log("IsUIChanging:true");
                }
                if (isGuideLock)
                {
                    Debug.Log("isGuideLock:true");
                }
                if (IsSceneChanging)
                {
                    Debug.Log("IsSceneChanging:true");
                }
            }
            return isfree;
        }
    }
    static public EventTriggerListener Get(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        listener.enabled = true;
        return listener;
    }
    static public EventTriggerListener GetNoSpine3D(Transform tran)
    {
        return GetNoSpine3D(tran.gameObject);
    }
    static public EventTriggerListener GetNoSpine3D(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        listener.is3D = true;
        var collider = go.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = go.AddComponent<BoxCollider>();
        }
        collider.center = new Vector3(19, 84, -1);
        collider.size = new Vector3(210, 171, 1);
        return listener;
    }
    static public EventTriggerListener GetLevelSpine3D(Transform tran)
    {
        return GetLevelSpine3D(tran.gameObject);
    }
    static public EventTriggerListener GetLevelSpine3D(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        listener.is3D = true;
        var collider = go.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = go.AddComponent<BoxCollider>();
        }
        collider.center = new Vector3(19, -50, -0);
        collider.size = new Vector3(300, 600, 1);
        return listener;
    }
    static public EventTriggerListener Get3D(GameObject go)
    {
        EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
        if (listener == null)
        {
            listener = go.AddComponent<EventTriggerListener>();
        }
        listener.is3D = true;
        var collider = go.GetComponent<BoxCollider>();
        if (collider == null)
        {
            collider = go.AddComponent<BoxCollider>();
        }
        return listener;
    }
    static public EventTriggerListener Get(Transform tran)
    {
        return Get(tran.gameObject);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.LogError("OnPointerClick");
        if (Input.touchCount > 1)
        {
            return;
        }
        if (IsFollowing)
        {
            Msg.Instance.Show("护送中,无法行动...");
            return;
        }
        UIClickEffect click = transform.GetComponent<UIClickEffect>();
        if (click)
        {
            if (click.isLockByEventListen)
            {
                if (!isFree)
                    return;
            }
        }
        else
        {
            if (!isFree)
                return;
        }
        if (isExit)
        {
            if (Time.time - oldTime > UIClickEffect.validTime)
            {
                if (click)
                {
                    click.ClickSmall(() =>
                    {

                    });
                }
                return;
            }
        }
        if (!IsCanClick)
            return;
        if (onUp != null)
        {
            onUp(gameObject);
        }
        //Debug.LogError(onClick);
        if (onClick != null && !longPressTriggered)
        {
            if (sum != 0)
            {
                return;
            }
            start = true;
            sum = 0;
            onClick(gameObject);
            onClickEvent?.Invoke(gameObject, eventData);
            for (int i = 0; i < oldClick.Count; i++)
            {
                oldClick[i](gameObject);
            }
        }
        if (click)
        {
            click.ClickSmall(() =>
            {

            });
        }
    }
    public float speed = 0.02f;
    private Vector3 GetInpectorEulers(Transform myGo)
    {
        Vector3[] corners = new Vector3[4];
        myGo.GetComponent<RectTransform>().GetWorldCorners(corners);
        var X = corners[0].x + (corners[2].x - corners[0].x) / 2;
        var Y = corners[0].y + (corners[2].y - corners[0].y) / 2;
        return new Vector3(X, Y, 0);
    }
    private Vector3 oldSize;
    private Vector3 oldPos;
    private Vector2 oldanchorMin;
    private Vector2 oldanchorMax;
    private Vector2 oldpivot;
    public IEnumerator ChangeImageBlack()
    {
        float sum = 1;
        float a = 1;
        oldSize = GetComponent<RectTransform>().localScale;
        oldPos = transform.position;
        oldanchorMin = GetComponent<RectTransform>().anchorMin;
        oldanchorMax = GetComponent<RectTransform>().anchorMax;
        oldpivot = GetComponent<RectTransform>().pivot;


        var itemRT = this.GetComponent<RectTransform>();
        var nowPos = GetInpectorEulers(itemRT);
        itemRT.anchorMin = new Vector2(0.5f, 0.5f);
        itemRT.anchorMax = new Vector2(0.5f, 0.5f);
        itemRT.pivot = new Vector2(0.5f, 0.5f);
        itemRT.position = nowPos;
        while (true)
        {
            sum -= speed;
            a -= speed / 2;

            GetComponent<RectTransform>().localScale = new Vector3(sum, sum, sum);
            if (sum <= 0.85f)
            {
                GetComponent<RectTransform>().localScale = oldSize;
                GetComponent<RectTransform>().anchorMin = oldanchorMin;
                GetComponent<RectTransform>().anchorMax = oldanchorMax;
                GetComponent<RectTransform>().pivot = oldpivot;
                transform.position = oldPos;
                yield break;
            }
            yield return null;
        }
    }


    int sum = 0;
    bool start = false;
    public void Update()
    {
        if (!start)
        {
            return;
        }
        sum++;
        if (sum == 10)
        {
            sum = 0;
            start = false;
        }
    }

    List<string> ActiveBtnName = new List<string>()
    {
        "_recharge",
        "_active",
        "_bonu",
    };
    private float oldTime = 0;
    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.LogError("OnPointerDown");
        if (Input.touchCount > 1)
        {
            return;
        }
        isExit = false;
        longPressTriggered = false;
        UIClickEffect click = transform.GetOrAddComponent<UIClickEffect>();
        if (click)
        {
            if (click.isLockByEventListen)
            {
                if (!isFree)
                    return;
            }
        }
        else
        {
            if (!isFree)
                return;
        }
        if (!IsCanClick)
            return;
        if (click)
        {
            oldTime = Time.time;

            click.ClickBig();
        }
        if (onDown != null)
        {
            onDown(gameObject);
        }
        if (onPress != null)
        {
            oldTime = Time.time;
            isPointerDown = true;
            StartCoroutine(inPress());
            for (int i = 0; i < oldClick.Count; i++)
            {
                oldClick[i](gameObject);
            }
        }
    }
    public static bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float _pressTime = 0.4f;

    public void SetPressTime(float val)
    {
        _pressTime = Mathf.Max(0.1f,val);
    }
    private IEnumerator inPress()
    {
        while (true)
        {
            //Debug.LogError("长安");
            if (!isPointerDown)
            {
                break;
            }
            if (Time.time - oldTime > _pressTime)
            {
                longPressTriggered = true;
                onPress(gameObject);
                yield return new WaitForSeconds(_pressTime);
            }
            yield return null;
        }
    }
    private bool isExit = false;
    public void OnPointerExit(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            return;
        }
        UIClickEffect click = transform.GetComponent<UIClickEffect>();
        if (click)
        {
            click.ClickSmall(() =>
            {

            });
        }
        isExit = true;
        if (!isFree)
            return;
        if (!IsCanClick)
            return;
        if (onExit != null) onExit(gameObject);
        if (onPress != null)
        {
            isPointerDown = false;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.touchCount > 1)
        {
            return;
        }
        UIClickEffect click = transform.GetComponent<UIClickEffect>();
        if (click)
        {
            click.ClickSmall(() =>
            {

            });
        }
        if (!isFree)
            return;
        if (!IsCanClick)
            return;
        if (onEnter != null) onEnter(gameObject);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onPress != null)
        {
            isPointerDown = false;
        }
    }
}