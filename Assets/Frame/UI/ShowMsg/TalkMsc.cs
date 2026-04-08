using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkMsc
{
    private Coroutine CO;
    private Text mscText;
    private string Msc;
    public GameObject go;
    private Coroutine NpcTalkCo;
    public void Follow(Transform spine, Vector2 pos)
    {
        NpcTalkCo = MonoTool.Instance.UpdateCall(() =>
        {
            if (go != null)
            {
                go.transform.SetBattleHUD(spine, pos);
            }
            else
            {
                MonoTool.Instance.StopCoroutine(NpcTalkCo);
            }
        });
    }
    public void StopTalk()
    {
        if (CO != null)
        {
            MonoTool.Instance.StopCoroutine(CO);
            mscText.text = Msc;
        }
    }
    public void ShowTextResHaveBg(Text text, string str, float disTime = 0.04f, int onelinenum = 11, System.Action callback = null, float time = 1f, bool ShowAllWithClick = false)
    {
        //SceneMainData.AddRuning(ShowTextResNoHaveBg(text, str, disTime, onelinenum, () =>
        //{
        //    SceneMainData.AddRuning(MonoTool.Instance.Wait(time, () =>
        //    {

        //        CO.Stop();
        //        NpcTalkCo.Stop();
        //        if (go != null)
        //            GameObject.DestroyImmediate(go);
        //        if (callback != null)
        //            callback();
        //    }));
        //}, ShowAllWithClick), (isSucc) =>
        //{
        //    CO.Stop();
        //    NpcTalkCo.Stop();
        //    if (go != null)
        //        GameObject.Destroy(go);
        //});
    }
    public Coroutine ShowTextResNoHaveBg(Text text, string str, float disTime = 0.04f, int onelinenum = 11, System.Action callback = null, bool ShowAllWithClick = false)
    {
        if (CO != null)
        {
            MonoTool.Instance.StopCoroutine(CO);
        }
        str = str.InsertLine(onelinenum);
        //str = SceneMainData.SetTalkByIsland(str);
        Msc = str;
        mscText = text;
        CO = MonoTool.Instance.StartCor(ShowTextResNoHaveBgIE(text, str, disTime, onelinenum, callback, ShowAllWithClick));
        return CO;
    }
    private IEnumerator ShowTextResNoHaveBgIE(Text msc, string str, float disTime, int onelinenum = 11, System.Action callback = null, bool ShowAllWithClick = false)
    {
        int len = str.Length;
        int sum = -1;
        string onstr = "";
        int colorIndex;
        int sumLen = onelinenum;
        len = str.Length;
        bool isHaveEndColor = true;
        while (true)
        {
            if (msc == null)
            {
                yield break;
            }
            if (ShowAllWithClick)
                if (Input.GetMouseButton(0))
                {
                    msc.text = str;
                    if (callback != null)
                    {
                        callback();
                    }
                    yield break;
                }
            sum++;
            if (len - sum > 6)
            {
                onstr = str.Substring(sum);

                if (onstr.Substring(0, 6) == "<color")
                {
                    //print(onstr);
                    colorIndex = onstr.IndexOf(">") + 1;
                    sum += colorIndex;
                    isHaveEndColor = false;
                }
                if (onstr.Substring(0, 7) == "</color")
                {
                    colorIndex = onstr.IndexOf(">") + 1;
                    sum += colorIndex;
                    isHaveEndColor = true;
                }
            }
            if (len - sum > 2)
            {
                var onChar = str[sum];
                if (onChar == '【' && str[sum + 2] == '】') sum += 2;
            }
            msc.text = isHaveEndColor ? str.Substring(0, sum) : $"{str.Substring(0, sum)}</color>";
            if (sum >= len)
            {
                if (callback != null)
                {
                    callback();
                }
                yield break;
            }
            yield return new WaitForSeconds(disTime);
        }
    }
}
