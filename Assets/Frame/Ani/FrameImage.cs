using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using static UnityEngine.Application;

/// <summary>
/// 2D序列帧特效
/// </summary>
public class FrameImage : MonoBehaviour
{
    private Image img;//图片组件

    [Header("资源目录")]
    public string resDir;

    [Header("总帧数")]
    [Space(25)]
    public int totalFarme;
    [Header("帧率")]
    public int frameRate;

    [Header("是否自动播放")]
    [Space(25)]
    public bool autoPlay = false;
    [Header("是否循环播放")]
    public bool isLoop = false;
    [Header("是否来回播放")]
    public bool isPingPong = false;

    public bool isSetActiveFalse = true;
    public bool isSetSize = true;
    public bool isShowLast = true;

    private Sprite curSprite;//当前Sprite
    private int curFrame;//当前帧
    private float timer;//计时器
    private bool isPlaying;//是否在播放

    private Dictionary<int, Action> eventDict = new Dictionary<int, Action>();//存储事件的字典
    public System.Action<Transform> OverEvent;
    public Sprite[] sprites;
    public int playNum;
    public int overNum;
    private bool isAutoPlayed = false;
#if SHIP_AIR
    public ShipPart shipPart;
    public System.Action<ShipPart, Transform> OverShipEvent;
#endif
    #region Init

    private void Awake()
    {
        img = GetComponent<Image>();
        //img.enabled = false;
    }
    private void OnEnable()
    {
        if (autoPlay)
        {
            if (isAutoPlayed) {
                return;
            }
            isAutoPlayed = true;
            resDir = resDir.Split('_')[0];
            if (sprites != null && sprites.Length != 0)
            {
                img.sprite = sprites[0];
            }
            Replay();
        }
    }

    #endregion

    #region Main

    /// <summary>
    /// 注册事件
    /// </summary>
    public void RegisterEvent(int eventFrame, Action onFinish)
    {
        if (!eventDict.ContainsKey(eventFrame))
        {
            eventDict.Add(eventFrame, onFinish);
        }
    }
    public void Replay()
    {
        playNum = 0;
        Play();
    }
    /// <summary>
    /// 播放
    /// </summary>
    public void Play(bool isLoopPlay = false)
    {
        if (isLoopPlay) {
            curFrame = 0;
            return;
        }
        isPlaying = false;
        ClearSprites();
        OnPreLoad(() => {
            if (this == null) {
                return;
            }
            if (img == null)
            {
                img = GetComponent<Image>();
            }
            timer = frameRate;
            curFrame = 0;
            isPlaying = true;
        });
    }
    private void OnPreLoad(System.Action callback)
    {
        if (ProssData.Instance.FrameImageUseRes)
        {
            AssetLoadOld.Instance.LoadDirSprite(transform,resDir, (spritesWWW) =>
            {
                //Debug.LogError(abName);
                sprites = spritesWWW.SortImage();
                callback();
            });
        }
        else {
            callback();
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public void Pause()
    {
        isPlaying = false;
    }


    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        playNum = 0;
        isPlaying = false;
        //清除结束的事件
        ClearFinishEvent();
#if SHIP_AIR
        if (OverShipEvent != null)
        {
            OverShipEvent(shipPart, transform);
            OverShipEvent = null;
        }
#endif
        //卸载资源
        if (isSetActiveFalse)
        {
            gameObject.SetActive(false);
        }
        if (OverEvent != null)
        {
            OverEvent(transform);
        }
        if (!isShowLast) {
            if(sprites.Length > 0)
            img.sprite = sprites[0];
        }
    }


    #endregion

    #region Misc

    /// <summary>
    /// 检测事件
    /// </summary>
    private void CheckEvent()
    {
        int tempKey = -1;

        foreach (var temp in eventDict)
        {
            if (curFrame == temp.Key)
            {
                tempKey = temp.Key;
                eventDict[temp.Key]?.Invoke();
            }
        }

        eventDict.Remove(tempKey);
    }
    /// <summary>
    /// 检测播放
    /// </summary>
    private void CheckPlay()
    {
        if (isPlaying == false)
        {
            return;
        }

        timer++;
        if (timer >= frameRate)
        {
            //播放下一帧
            if (isPingPong)
            {
                PlayPinePoneFrame();
            }
            else {
                PlayNextFrame();
            }
            timer = 0;
        }
    }
    private bool isAdd = true;
    private void PlayPinePoneFrame()
    {
        if (curFrame >= totalFarme - 1)
        {
            isAdd = false;
        }
        if (curFrame <= 0) {
            isAdd = true;
        }
        if (ProssData.Instance.FrameImageUseRes)
        {
            try
            {
                img.sprite = sprites[curFrame];
            }
            catch (Exception e)
            {
                Debug.LogError($"出错帧动画的资源路径为 {resDir}");
                throw;
            }

            if (isSetSize)
            {
                isSetSize = false;
                img.SetNativeSize();
            }
        }
        else
        {
            string path = resDir + "/" + curFrame;
            //Debug.LogError(Time.time + ":" +curFrame);
            curSprite = Resources.Load<Sprite>(path);
            //Debug.LogError(path);
            if (curSprite == null)
            {
                Debug.LogError("没有此图片：" + path);
            }
            else
            {
                img.sprite = curSprite;
                if (isSetSize)
                {
                    isSetSize = false;
                    img.SetNativeSize();
                }
             
            }
        }
        if (isAdd)
        {
            curFrame++;
        }
        else {
            curFrame--;
        }
    }

    /// <summary>
    /// 播放下一帧
    /// </summary>
    private void PlayNextFrame()
    {
        if (overNum != 0 && playNum >= overNum) {
            Stop();
            return;
        }
        if (curFrame >= totalFarme)
        {
            if (isLoop)
            {
                Play(true);
            }
            else {
                Stop();
                return;
            }
        }
        if (ProssData.Instance.FrameImageUseRes)
        {
            if (sprites != null && sprites.Length != 0)
            {
                try
                {
                    img.sprite = sprites[curFrame];
                }
                catch (Exception e)
                {
                    Debug.Log($"出错帧动画的资源路径为 {resDir}");
                    //throw;
                }
            }
           
            if (isSetSize)
            {
                isSetSize = false;
                img.SetNativeSize();
            }
            curFrame++;

        }
        else {
            string path = resDir + "/" + curFrame;
            //Debug.LogError(Time.time + ":" +curFrame);
            curSprite = Resources.Load<Sprite>(path);
            //Debug.LogError(path);
            if (curSprite == null)
            {
                Debug.LogError("没有此图片：" + path);
            }
            else
            {
                img.sprite = curSprite;
                if (isSetSize)
                {
                    isSetSize = false;
                    img.SetNativeSize();
                }
                curFrame++;
            }
        }
        if(playNum < 1000)
            playNum++;
    }
    public void PlayPingPone() { 
        
    }
    /// <summary>
    /// 清除结束的事件
    /// </summary>
    private void ClearFinishEvent()
    {
        eventDict.Clear();
    }

    private void FixedUpdate()
    {
        //检测事件
        CheckEvent();
        //检测播放
        CheckPlay();
    }
    public void ClearSprites() {
        sprites = null;
    }
    public void SetFirst()
    {
        img.sprite = sprites[0];
    }

    #endregion
}