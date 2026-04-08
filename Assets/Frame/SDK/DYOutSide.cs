using System;
using System.Collections;
using System.Collections.Generic;
using TreeData;
using UnityEngine;

public class DYOutSide : MonoBehaviour
{
    public static DYOutSide Instance;
    private void Awake()
    {
        Init();
    }
    // Start is called before the first frame update
    public void Init()
    {
#if DY
        if (UIDYOutSideTip.isGetRewardSer) {
            transform.SetActive(false);
            return;
        }
        var tt = StarkSDKSpace.StarkSDK.API.GetStarkSideBarManager();
        tt.CheckScene(StarkSDKSpace.StarkSideBar.SceneEnum.SideBar, (isSucc) => {
            if (!isSucc)
            {
                transform.SetActive(false);
                Debug.Log("不能接入侧边栏");
                return;
            }
            transform.SetActive(true);
            transform.localScale = Vector3.one;
            EventTriggerListener.Get(transform).onClick = (btn) =>
            {
                Debug.Log("clickOutSide");
                UIManager.OpenTip("UIDYOutSideTip", "", (str) => {
                    if (str == "1") {
                        Init();
                    }
                });
            };
        },() => { 
        
        }, (index, str) => { 
            
        });
#else
        transform.SetActive(false);
#endif
    }
}
