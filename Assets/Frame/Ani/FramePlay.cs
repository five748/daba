using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.VisualScripting;

public static class FrameTool
{
    public static FrameImage PlayOnce(this Image image, string effectName, System.Action<Transform> callback = null, int frameRate = 0)
    {
        //Debug.LogError(effectName);
        var scr = image.GetOrAddComponent<FrameImage>();
        string oldRes = scr.resDir;
        scr.resDir = "Frame/" + effectName;
        if (oldRes != scr.resDir)
        {
            scr.ClearSprites();
        }
        var tabOne = TableRunData.Instance.frameNumAndSpeed[effectName];
        scr.frameRate = frameRate == 0 ? tabOne.frameRate : frameRate;
        scr.totalFarme = tabOne.sfNum + 1;
        scr.autoPlay = false;
        scr.isLoop = false;
        scr.Replay();
        if (callback != null)
        {
            scr.OverEvent = callback;
        }
        return scr;
    }
    public static FrameImage PlayOnceEndActiveTrue(this Image image, string effectName, System.Action<Transform> callback = null, int frameRate = 0)
    {
        //Debug.LogError(effectName);
        var scr = PlayOnce(image, effectName, callback, frameRate);
        scr.isSetActiveFalse = false;
        return scr;
    }
    public static FrameImage PlayLoop(this Image image, string effectName,int frameRate =0) {
        //Debug.LogError(effectName);
        var scr = image.GetOrAddComponent<FrameImage>();
        string oldRes = scr.resDir;
        scr.resDir = "Frame/" + effectName;
        if (oldRes != scr.resDir)
        {
            scr.ClearSprites();
        }
        if (!TableRunData.Instance.frameNumAndSpeed.ContainsKey(effectName))
        {
            Debug.Log("找不到 帧动画表数据:" + effectName);
            return null;
        }
        var tabOne = TableRunData.Instance.frameNumAndSpeed[effectName];
        scr.frameRate = frameRate == 0 ? tabOne.frameRate : frameRate;
        scr.totalFarme = tabOne.sfNum + 1;
        scr.autoPlay = false;
        scr.isLoop = true;
        scr.Pause();
        scr.Play();
        return scr;
    }
    
    public static FrameImage PlayLoop(this Image image, string effectName,int totalFrame,int frameRate,System.Action<Transform> callback = null) {
        //Debug.LogError(effectName);
        var scr = image.GetOrAddComponent<FrameImage>();
        string oldRes = scr.resDir;
        scr.resDir = "Frame/" + effectName;
        if (oldRes != scr.resDir)
        {
            scr.ClearSprites();
        }
        scr.frameRate = frameRate;
        scr.totalFarme = totalFrame;
        scr.autoPlay = false;
        scr.isLoop = true;
        if (callback != null)
        {
            scr.OverEvent = callback;
        }
        scr.Pause();
        scr.Play();
        return scr;
    }
    
    public static FrameImage PlayOnce(this Image image, string effectName,int totalFrame,int frameRate,System.Action<Transform> callback = null) {
        //Debug.LogError(effectName);
        var scr = image.GetOrAddComponent<FrameImage>();
        string oldRes = scr.resDir;
        scr.resDir = "Frame/" + effectName;
        if (oldRes != scr.resDir)
        {
            scr.ClearSprites();
        }
        scr.frameRate = frameRate;
        scr.totalFarme = totalFrame;
        scr.autoPlay = false;
        scr.isLoop = false;
        if (callback != null)
        {
            scr.OverEvent = callback;
        }

        scr.isSetSize = true;
        scr.Pause();
        scr.Play();
        return scr;
    }
    
    
    
}
