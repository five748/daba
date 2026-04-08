using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SEffect : MonoBehaviour
{
    public static SEffect Instance;
    private MsgOne Effect;
    private List<WarnOne> allNeedShows;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        Effect = new MsgOne(transform.GetChild(2));
        allNeedShows = new List<WarnOne>();
    }
    public void Show(string desc)
    {
        desc = desc.ChangeTextStr();
        if (!allNeedShows.Contains(new WarnOne(desc)))
        {
            allNeedShows.Add(new WarnOne(desc));
        }
    }
    int sum = 30;
    // Update is called once per frame
    void Update()
    {
        if (allNeedShows == null)
        {
            return;
        }
        if (allNeedShows.Count == 0)
        {
            sum = 30;
            return;
        }
        sum++;
        if (sum < 30)
        {
            return;
        }
        sum = 0;
        var item = allNeedShows[0];
        allNeedShows.RemoveAt(0);
        MsgOne choose = Effect;
        choose.ShowEffect(item, 0.02f, false, () =>
        {

        });
    }
}
