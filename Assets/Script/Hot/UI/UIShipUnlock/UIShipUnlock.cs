using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;
public class UIShipUnlock:BaseMonoBehaviour{
    private UIShipUnlockAuto Auto = new UIShipUnlockAuto();
    
    private Vector3 _lightRotation = new Vector3(0, 0, 360); // 旋转的量

    private int _shipId;
    private int _type = 0;

    private TweenerCore<Vector3, Vector3, VectorOptions> _doScale = null;
    private TweenerCore<Vector3, Vector3, VectorOptions> _doMove = null;
    private TweenerCore<Color, Color, ColorOptions> _doFade = null;

    public override void BaseInit(){    
        Auto.Init(transform, this);    
        Init(param);    
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    public void ClickBtn_sure(GameObject button){
        Debug.Log("click" + button.name);
        //不要改这里 需要全关闭的界面自己关 这个界面多界面复用的 逻辑不统一
        UIManager.CloseTip();
        _CallBack?.Invoke(_shipId.ToString());
    }
    //------------------------------点击方法标记请勿删除---------------------------------
    private void Init(string param){
        UIManager.FadeOut();
        var split = param.Split("_");
        _shipId = split[0].ToInt();
        _type = split[1].ToInt();
        update_ui();
        
        GuideMgr.Instance.BindBtn(Auto.Btn_sure.transform, tableMenu.GuideWindownBtn.ship_unlock_sure);
    }

    private void update_ui()
    {
        Auto.Light.DORotate(_lightRotation, 3.0f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
        
        Auto.Ribbons.localScale = new Vector3(0.1f, 0.1f, 0);
        Auto.Ribbons.localPosition = Vector3.zero;
        Auto.Ribbons.SetActive(true);
        _doScale = Auto.Ribbons.DOScale(new Vector3(0.3f,0.3f,0.3f), 0.3f)
            .SetEase(Ease.OutCubic).OnComplete(() =>
            {
                if (Auto.Ribbons == null)
                {
                    return;
                }
                _doScale = Auto.Ribbons.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutCubic);
            });

        _doMove = Auto.Ribbons.DOMoveY(Auto.Ribbons.position.y + 600, 0.5f)
            .SetEase(Ease.OutCubic) // 使用弹跳插值效果
            .OnComplete(() => // 当上升动画完成时
            {
                // 下落200
                _doMove = Auto.Ribbons.DOMoveY(Auto.Ribbons.position.y - 1200 ,0.7f)
                    .SetEase(Ease.InCubic); // 使用线性插值
                //渐隐
                _doFade = Auto.Ribbons.GetComponent<Image>().DOFade(0, 0.7f)
                    .SetEase(Ease.InCubic) // 使用线性插值
                    .OnComplete(() => // 当渐隐动画完成时
                    {
                        Auto.Ribbons.SetActive(false);
                    });
            });

        if (_type == 0)
        {
            //Auto.Icon.SetImage("ship/"+_shipId, true);
            var tShip = TableCache.Instance.shipTable[_shipId];
            Auto.Icon.PlayLoop(tShip.moveAni);
            Auto.Name_lv.text = tShip.name;
            Auto.Des.text = "过闸收费";
            Auto.Money.text = tShip.toll.ToString();    
        }
        else if (_type == 1)
        {
            var tShip = TableCache.Instance.cargoShipTable[_shipId];
            //Auto.Icon.SetImage("ship/"+tShip.icon, true);
            Auto.Icon.PlayLoop(tShip.move2Ani);
            Auto.Title.text = $"解锁{tShip.name}";
            Auto.Name_lv.text = tShip.name;
            Auto.Des.text = "运载量";
            Auto.Money.text = tShip.capacity.ToString();
            Auto.Img_item.gameObject.SetActive(false);
        }
    }

    public override void Destory()
    {
        Auto.Light.DOKill();
        Auto.Ribbons.DOKill();

        if (_doScale != null)
        {
            _doScale.Kill();
            _doScale = null;
        }
        if (_doMove != null)
        {
            _doMove.Kill();
            _doMove = null;
        }
        if (_doFade != null)
        {
            _doFade.Kill();
            _doFade = null;
        }
        
        base.Destory();
    }
}




