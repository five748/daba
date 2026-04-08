using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

public class UIGuide:BaseMonoBehaviour
{
    private UIGuideAuto Auto = new UIGuideAuto();
    private GuideMgr _guideMgr;

    private bool _start = false;//是否开始引导
    private Transform _aimBtn = null;//目标按钮
    private Coroutine _coTip = null;//提示
    
    private Coroutine _coSlip = null;//滑动
    private bool _showSlip = false;//是否开始展示滑动
    
    private static UIGuide _guide = null;

    private Vector3 _posDialogue = new Vector3(0, 0);

    private Coroutine _coTryGuide = null;
    private Color _colorMaskShow = new Color(0, 0, 0, 0.5f);
    private Color _colorMaskTransparent = new Color(0, 0, 0, 0f);
    
    public static bool ShowGuide = true;
    
    public override void BaseInit(){
        //Debug.LogError("BaseInit");
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_type_finger(GameObject button){
        Debug.Log("click" + button.name);
        GuideBtnClick();
    }
    public void ClickBtn_mask(GameObject button){
        Debug.Log("click" + button.name);
        //点击遮罩
        HardGuideOuterClick();
    }
    public void ClickIndicator_finger(GameObject button){
        Debug.Log("click" + button.name);
        GuideBtnClick();
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private static Coroutine _updateCheckIE;
    private void Init(string param){
        UIManager.FadeOut();
        _guide = this;
    }

    public static void InitGuide(System.Action callback)
    {
        if (!ShowGuide)
        {
            callback();
            return;
        }
        GuideMgr.Instance.Init(()=>
        {
            _guide._guideMgr = GuideMgr.Instance;
            if (_guide._guideMgr.CheckFinishAllGuide())
            {
                Debug.Log("-------1111");
                _guide.gameObject.SetActive(false);
                callback();
                return;
            }

            if (_guide._guideMgr.CheckInterrupt())
            {
                _guide._guideMgr.RecoveryGuideWindow();
            }

            _guide.gameObject.SetActive(true);
            _guide._start = true;
            Debug.Log("-------GuideInit222");
            GameProcess.Instance.StopEvent += UIGuide._guide.Clear;
            _updateCheckIE.Stop();
            _updateCheckIE = MonoTool.Instance.UpdateCall(() =>
            {
                _guide.CheckHardGuide();
                // Debug.Log("-------CheckHardGuide");
                _guide.UpdateHollow();
            });
            callback();
        });
        
    }
    
    public void Clear()
    {
        _guideMgr.ClearData();
        _updateCheckIE.Stop();
        transform.SetActive(false);
    }

    public static void SetStart(bool value)
    {
        _guide._start = value;
    }

    private void SetMaskActive(bool value)
    {
        Auto.Btn_mask.SetActive(value);
        //只有聊天 那么就隐藏
        if (_guideMgr.HardGuide != null && (_guideMgr.HardGuide.show_type is (int)tableMenu.GuideShowType.just_dialogue or (int)tableMenu.GuideShowType.surround_dam || _guideMgr.HardGuide.delay != 0))
        {
            Auto.Btn_mask.GetComponent<Image>().color = _colorMaskTransparent;
            if (_guideMgr.HardGuide.delay != 0)
            {
                MonoTool.Instance.Wait(_guideMgr.HardGuide.delay, () =>
                {
                    Auto.Btn_mask.GetComponent<Image>().color = _colorMaskShow;
                });
            }
        }
        else
        {
            Auto.Btn_mask.GetComponent<Image>().color = _colorMaskShow;
        }
    }

    private void CloseGuide()
    {
        SetMaskActive(false);
        HideIntroduction();
        _aimBtn = null;
        // Debug.Log("-------CloseGuide");
    }

    private void CloseSoftGuide()
    {
        SetMaskActive(false);
        HideIntroduction();
        _guideMgr.SetInSoftGuide(false);
        _aimBtn = null;
    }

    private IEnumerator TryGuide()
    {
        //用于异步后的判定
        var oldId = _guideMgr.HardGuide.id;
        MainMgr.Instance.ComCameraMove.EnterAnimation();
        
        //引导配表中带有延迟
        if (_guideMgr.HardGuide.delay > 0)
        {
            _start = false;
            HideIntroduction();
            yield return new WaitForSeconds(_guideMgr.HardGuide.delay);
            _start = true;
        }
        
        if (_guideMgr.CheckHaveDialogue())
        {
            _start = false;
            HideIntroduction();
            
            //对话带镂空的情况
            if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialog_hollow || _guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialogue_and_finger)
            {
                Transform btn = _guideMgr.GetBindBtn();
                while (btn == null || !btn.gameObject.activeSelf)
                {
                    yield return new WaitForFixedUpdate();
                    btn = _guideMgr.GetBindBtn();
                }

                _aimBtn = btn;
                Auto.Btn_type_finger.GetComponent<BtnGuide>().BtnAim = _aimBtn;
                
                // if (_guideMgr.HardGuide.group == 1)
                // {
                //     set_hollow_active();
                //     show_hollow(btn, 200);
                // }
                // else
                // {
                //     set_hollow_active();
                //     show_hollow(btn);
                // }
                
                set_hollow_active();
                show_hollow(btn);
            }
            
            if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialogue_and_finger)
            {
                ShowWithFinger();
            }
            
            _guideMgr.ExecuteSpecialEvent();
            _posDialogue.y = 0 + _guideMgr.HardGuide.posY;
            Auto.Dialogue.FindChildTransform("nd_frame").localPosition = _posDialogue;
            var result = Auto.Dialogue.GetComponent<DialogueSystem>().ShowDialogue(_guideMgr.HardGuide.dialogue_id);
            yield return result.Wait(() =>
            {
                if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialogue_and_finger)
                {
                    UpdateWithFingerPosition();
                }
            });

            _start = true;
        }
        else
        {
            HideIntroduction();
            _guideMgr.ExecuteSpecialEvent();
        }

        if (oldId == _guideMgr.HardGuide.id)
        {
            if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.just_dialogue || _guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialog_hollow)
            {
                CompleteGuide();
            }
            else
            {
                StartCoroutine(OpenGuide());
            }
        }
    }

    private IEnumerator OpenGuide()
    {
        var btn = _guideMgr.GetBindBtn();

        if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.finger)
        {
            while (btn == null || !btn.gameObject.activeSelf)
            {
                yield return new WaitForFixedUpdate();
                btn = _guideMgr.GetBindBtn();
            }
            
            _aimBtn = btn;
            Auto.Btn_type_finger.GetComponent<BtnGuide>().BtnAim = _aimBtn;
        }
        
        if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.finger)
        {
            ShowWithFinger();
        }
        else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.none_finger)
        {
            ShowTip();
        }
        else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.just_dialogue)
        {
            
        }
        else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.assembly_slip)
        {
            _coSlip = StartCoroutine(ShowAssemblySlip());
        }
        else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.get_reward)
        {
            
        }
        else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.fly_guide)
        {
            _guideMgr.StarFlyGuide();
        }
    }

    private void ShowTip()
    {
        Auto.Type_indicator.SetActive(false);
        Auto.Type_tip.SetActive(true);

        set_hollow_active();
        show_hollow(Auto.Type_tip.GetChild(0), 310);
        
        _coTip = MonoTool.Instance.Wait(4f, () =>
        {
            CompleteGuide();
            // coro_tip.Stop();
        });
    }

    private void ShowWithFinger()
    {
        if (_aimBtn == null)
        {
            return;
        }
        
        Auto.Type_indicator.SetActive(true);
        Auto.Type_tip.SetActive(false);
        set_hollow_active();
        show_hollow(_aimBtn);
        UpdateWithFingerPosition();
    }

    private void UpdateWithFingerPosition()
    {
        if (_aimBtn == null)
        {
            return;
        }

        if (_guideMgr.HardGuide.id is 10040 or 140050)
        {
            var pos = transform_star_world_to_screen(_aimBtn);
            Auto.Type_indicator.position = new Vector3(
                pos.x + _aimBtn.GetComponent<RectTransform>().rect.center.x,
                pos.y + _aimBtn.GetComponent<RectTransform>().rect.center.y
            );
        }
        else
        {
            var position = _aimBtn.position;
            Auto.Type_indicator.position = new Vector3(
                position.x + _aimBtn.GetComponent<RectTransform>().rect.center.x,
                position.y + _aimBtn.GetComponent<RectTransform>().rect.center.y
            );
        }

        if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.finger || _guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialogue_and_finger)
        {
            set_hollow_active();
            show_hollow(_aimBtn);
        }
    }
    
    private IEnumerator ShowAssemblySlip()
    {
        while (_guideMgr.SlipStart == null || _guideMgr.SlipEnd == null)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        if (_guideMgr.HardGuide.show_type != (int)tableMenu.GuideShowType.assembly_slip)
        {
            yield break;
        }
        SetMaskActive(false);
        Auto.Type_indicator.SetActive(false);
        Auto.Type_assembly_slip.SetActive(true);
        Auto.Type_tip.SetActive(false);
        _guideMgr.StopShowGuideSlip = () =>
        {
            _showSlip = false;
        };
        if (!_showSlip)
        {
            Auto.Slip_finger.position = new Vector3(_guideMgr.SlipStart.position.x, _guideMgr.SlipStart.position.y);
        }
        _showSlip = true;
        _coSlip.Stop();
        _coSlip = null;
    }

    private void HideIntroduction()
    {
        Auto.Type_tip.SetActive(false);
        Auto.Type_indicator.SetActive(false);
        set_hollow_active(false);
        show_hollow(Auto.Btn_mask.transform, 0);
        _guideMgr.ResetGuideSlip();
    }

    private void CompleteGuide()
    {
        set_hollow_active(false);
        
        if (_guideMgr.InSoftGuide)
        {
            _guideMgr.SetInSoftGuide(false);
            _aimBtn = null;
            _guideMgr.SoftGuideClick();
        }
        else
        {
            _guideMgr.ResetGuideSlip();
            //恢复特殊事件
            _guideMgr.RecoverySpecialEvent();
            _guideMgr.CompleteCurGuide();
        }  
    }
    
    private void GuideBtnClick()
    {
        //没有按钮的时候也可能是进入下一步的引导
        if (_aimBtn == null)
        {
            return;
        }

        if (_guideMgr.InSoftGuide)
        {
            ExecuteEvents.Execute(_aimBtn.gameObject, new PointerEventData(EventSystem.current),ExecuteEvents.pointerClickHandler);  
            CompleteGuide();
        }
        else
        {
            if (_guideMgr.HardGuide.exhibition != 1)
            {
                var btnExecute = _guideMgr.GetExecuteBtn();
                ExecuteEvents.Execute(btnExecute.gameObject, new PointerEventData(EventSystem.current),ExecuteEvents.pointerClickHandler);    
            }
            Auto.Dialogue.GetComponent<DialogueSystem>().EndDialogue();
            CompleteGuide();
        }
    }

    /// <summary>
    /// 软引导的时候使用 现在可以不用
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <returns></returns>
    private GameObject IsPointerOverGameObject(Vector2 mousePosition)
    {
        //创建一个点击事件
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        //向点击位置发射一条射线, 检测是否点击UI
        EventSystem.current.RaycastAll(eventData, raycastResults);
        if (raycastResults.Count > 0)
        {
            for (int i = 0; i < raycastResults.Count; i++)
            {
                var ray = raycastResults[i];
                var com = ray.gameObject.GetComponent<Button>();
                var com1 = ray.gameObject.GetComponent<EventTriggerListener>();
                if (com != null || com1 != null)
                {
                    return ray.gameObject;
                }
            }
        }

        return null;
    }

    private void CheckHardGuide()
    {
        //新手引导结束了 返回
        if (_guideMgr.CheckFinishAllGuide())
        {
            if (!_guideMgr.CheckInGuide())
            {
                // Debug.Log("-------1");
                CloseGuide();
            }
            
            return;
        }

        if (!_start)
        {
            if (_guideMgr.CheckInGuide() && Auto.Indicator_finger.gameObject.activeSelf)
            {
                // Debug.Log("-------1");
                if (_guideMgr.HardGuide.delay  == 0)
                {
                    UpdateWithFingerPosition();
                }
            }
            return;
        }
        
        //展示手指动作
        ShowSlipAction();

        if (_guideMgr.CheckOpenGuide())
        {
            _guideMgr.CheckPrepare();
            _guideMgr.SetInSoftGuide(false);
            
            //打开了引导 拉起遮罩
            SetMaskActive(true);
            _coTryGuide = StartCoroutine(TryGuide());
        }
        else
        {
            if (!_guideMgr.CheckInGuide()) 
            {
                CloseGuide();
            }
        }
        if (_guideMgr.CheckInGuide())
        {
            if (_guideMgr.InGuide)
            {
                if (_guideMgr.HardGuide == null)
                {
                    return;
                }

                if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.none_finger)
                {
                    // ShowTip();
                }
                else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.finger || _guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.dialogue_and_finger)
                {
                    // ShowWithFinger();
                    //有延迟的时候不应该更新界面
                    if (_guideMgr.HardGuide.delay  == 0)
                    {
                        UpdateWithFingerPosition();
                    }
                }
                else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.just_dialogue)
                {
                    
                }
                else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.assembly_slip)
                {
                    if (!_showSlip)
                    {
                        _coSlip = StartCoroutine(ShowAssemblySlip());
                    }
                }
                else if (_guideMgr.HardGuide.show_type == (int)tableMenu.GuideShowType.get_reward)
                {
                    
                }
            }
        }
    }

    private void UpdateHollow()
    {
        if (Auto.Img_hollow.gameObject.activeSelf && Auto.Img_hollow_1.gameObject.activeSelf)
        {
            var btn = _guideMgr.GetBindBtn();
            if (btn)
            {
                if (_guideMgr.HardGuide.id is 10040 or 140050)
                {
                    var pos = transform_star_world_to_screen(_aimBtn);
                    show_hollow(btn, -1, pos);
                    return;
                }

                show_hollow(btn);
            }
        }
    }
    
    //点击引导圈外部
    private void HardGuideOuterClick()
    {
        if (_guideMgr.InGuide)
        {
            if (_guideMgr.HardGuide.can_click_outer == 1)
            {
                CompleteGuide();
            }
            else
            {
                Msg.Instance.Show("新手引导中");
            }
        }
    }

    private void set_hollow_active(bool value = true)
    {
        Auto.Img_hollow.SetActive(value);
        Auto.Img_hollow_1.SetActive(false);
    }

    private void show_hollow(Transform btn, float radius = -1, Vector3? pos = null)
    {
        if (radius < 0)
        {
            float size = Math.Max(btn.GetComponent<RectTransform>().sizeDelta.x, btn.GetComponent<RectTransform>().sizeDelta.y) + 100; 
            if (pos == null)
            {
                var position = btn.position;
                if (_guideMgr.HardGuide.id is 10040 or 140050)
                {
                    position = transform_star_world_to_screen(btn);
                }
                var btnPos = new Vector3(
                    position.x + btn.GetComponent<RectTransform>().rect.center.x,
                    position.y + btn.GetComponent<RectTransform>().rect.center.y
                );
                Auto.Img_hollow.position = btnPos;
                Auto.Img_hollow_1.position = btnPos;
            }
            else
            {
                Auto.Img_hollow.position = (Vector3)pos;
                Auto.Img_hollow_1.position = (Vector3)pos;
            }

            Auto.Nd_hollow.position = Auto.Img_hollow.position;
            //根据给的圈大小 手工调整一下 加的数值
            Auto.Img_hollow.GetComponent<RectTransform>().sizeDelta = new Vector2(size + 50, size + 50);
            Auto.Img_hollow_1.GetComponent<RectTransform>().sizeDelta = new Vector2(size + 50, size + 50);
            Auto.Btn_mask.GetComponent<GuideHollowCircle>().Guide(TranTool.GetRootCanvas(Auto.Btn_mask.GetComponent<RectTransform>()), Auto.Nd_hollow.GetComponent<RectTransform>(), size / 2);
        }
        else
        {
            if (pos == null)
            {
                var position = btn.position;
                if (_guideMgr != null && _guideMgr.HardGuide != null && _guideMgr.HardGuide.id is 10040 or 140050)
                {
                    position = transform_star_world_to_screen(btn);
                }
                var btnPos = new Vector3(
                    position.x + btn.GetComponent<RectTransform>().rect.center.x,
                    position.y + btn.GetComponent<RectTransform>().rect.center.y
                );
                Auto.Img_hollow.position = btnPos;
                Auto.Img_hollow_1.position = btnPos;
            }
            else
            {
                Auto.Img_hollow.position = (Vector3)pos;
                Auto.Img_hollow_1.position = (Vector3)pos;
            }
            if (radius == 0)
            {
                Auto.Img_hollow.GetComponent<RectTransform>().sizeDelta = new Vector2(0,0);
                Auto.Img_hollow_1.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            }
            else
            {
                Auto.Img_hollow.GetComponent<RectTransform>().sizeDelta = new Vector2((radius+90) * 2, (radius+90) * 2);
                Auto.Img_hollow_1.GetComponent<RectTransform>().sizeDelta = new Vector2((radius+90) * 2, (radius+90) * 2);
            }
            Auto.Nd_hollow.position = Auto.Img_hollow.position;
            Auto.Btn_mask.GetComponent<GuideHollowCircle>().Guide(TranTool.GetRootCanvas(Auto.Btn_mask.GetComponent<RectTransform>()), Auto.Nd_hollow.GetComponent<RectTransform>(), radius);
        }
        Auto.Btn_type_finger.GetComponent<RectTransform>().sizeDelta = Auto.Img_hollow.GetComponent<RectTransform>().sizeDelta;
    }

    private void ShowSlipAction()
    {
        if (!_showSlip)
        {
            Auto.Slip_finger.SetActive(false);
            return;
        }

        if (_guideMgr.SlipStart == null || _guideMgr.SlipEnd == null)
        {
            Auto.Slip_finger.SetActive(false);
            return;
        }

        Auto.Slip_finger.SetActive(true);
        if (Math.Abs(Auto.Slip_finger.position.x - _guideMgr.SlipEnd.position.x) <= 5 && Math.Abs(Auto.Slip_finger.position.y - _guideMgr.SlipEnd.position.y) <= 5)
        {
            Auto.Slip_finger.position = new Vector3(_guideMgr.SlipStart.position.x, _guideMgr.SlipStart.position.y);
        }
        
        Auto.Slip_finger.position = Vector3.MoveTowards(
            new Vector3(Auto.Slip_finger.position.x, Auto.Slip_finger.position.y), 
            new Vector3(_guideMgr.SlipEnd.position.x, _guideMgr.SlipEnd.position.y),
            800 * Time.deltaTime
        );
    }
    
    private Vector3 transform_star_world_to_screen(Transform btn)
    {
        Camera battleCamera = GameObject.Find("UIMainCanvas").GetComponent<Canvas>().worldCamera;
        // Vector3 screen_pos = battle_camera.WorldToScreenPoint(btn.position);
        
        Vector3 pos = battleCamera.WorldToViewportPoint(btn.position);
        if (pos.z >= 0)
        {
            pos = Camera.main.ViewportToWorldPoint(pos);
            pos.z = 0;
        }
        else {
            pos = Camera.main.ViewportToWorldPoint(pos);
            pos.z = Camera.main.farClipPlane + 10f;
        }

        return pos;
    }
 
    //todo 目前只做了强制引导的那部分内容
}


