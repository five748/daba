using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// 引导中的特殊性事件的处理 例如在引导过程中暂停某些事物
/// </summary>
public class GuideSpecial
{
    //执行特殊事件
    public void Executes(int[] types, float[] datas)
    {
        if (types == null)
        {
            return;
        }
        
        for (int i = 0; i < types.Length; i++)
        {
            int type = types[i];
            float data = datas[i];
            execute(type, data);
        }
    }

    //复原特殊事件
    public void Recovery(int[] types, float[] datas)
    {
        if (types == null)
        {
            return;
        }
        
        for (int i = 0; i < types.Length; i++)
        {
            int type = types[i];
            float data = datas[i];
            recovery(type, data);
        }
    }

    private void execute(int type, float data)
    {
        //Debug.LogError(type);
        switch (type)
        {
            // case (int)tableMenu.GuideSpecialEvent.assembly_btn_hide:
            //     MonoTool.Instance.StartCoroutine(assembly_btn_visible(false));
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_section_filter:
            //     assembly_section_filter(data);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_section_chose_appoint:
            //     assembly_section_chose_appoint((int)data);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_point_chose_appoint:
            //     assembly_point_chose_appoint((int)data);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.get_section:
            //     get_section(data);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.main_create_boss:
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.main_button_control:
            //     main_button_control(data);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.star_guide_set:
            //     star_guide_set();
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.chip_btn_hide:
            //     MonoTool.Instance.StartCoroutine(chip_btn_visible(false));
            //     break;
            case (int)tableMenu.GuideSpecialEvent.surround_dam:
                surround_dam();
                break;
            case (int)tableMenu.GuideSpecialEvent.pintu_btn_hide:
                MonoTool.Instance.StartCoroutine(pintu_btn_visible(false));
                break;
        }
    }

    private void recovery(int type, float data)
    {
        switch (type)
        {
            // case (int)tableMenu.GuideSpecialEvent.assembly_btn_hide:
            //     MonoTool.Instance.StartCoroutine(assembly_btn_visible(true));
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_section_filter:
            //     assembly_section_filter(-1);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_section_chose_appoint:
            //     assembly_section_chose_appoint(int.MaxValue);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.assembly_point_chose_appoint:
            //     assembly_point_chose_appoint(int.MaxValue);
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.main_create_boss:
            //     main_create_boss(data);
            //     break;
            case (int)tableMenu.GuideSpecialEvent.close_ui_tip:
                //the function execute always in the end of guide step.
                close_ui_tip(data);
                break;
            // case (int)tableMenu.GuideSpecialEvent.fly_guide:
            //     fly_guide();
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.fly_guide_end:
            //     fly_guide_end();
            //     break;
            // case (int)tableMenu.GuideSpecialEvent.chip_btn_hide:
            //     MonoTool.Instance.StartCoroutine(chip_btn_visible(true));
            //     break;
            case (int)tableMenu.GuideSpecialEvent.pintu_btn_hide:
                MonoTool.Instance.StartCoroutine(pintu_btn_visible(true));
                break;
        }
    }

    private void surround_dam()
    {
        MainMgr.Instance.ComCameraMove.SurroundDam(() =>
        {
            GuideMgr.Instance.CompleteCurGuide();
        }, GuideMgr.Instance.GetBindBtn(tableMenu.GuideWindownBtn.main_shoufeizhan));
    }
    
    private IEnumerator pintu_btn_visible(bool value)
    {
        while (GuideMgr.Instance.HideBtnList.Count <= 0)
        {
            yield return new WaitForFixedUpdate();
        }

        for (int i = 0; i < GuideMgr.Instance.HideBtnList.Count; i++)
        {
            GuideMgr.Instance.HideBtnList[i].SetActive(value);
        }
    }

    private void close_ui_tip(float data)
    {
        UIManager.CloseTip();
    }


}