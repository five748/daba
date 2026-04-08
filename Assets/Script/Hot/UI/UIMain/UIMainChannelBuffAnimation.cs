using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIMainChannelBuffAnimation : MonoBehaviour
{
    public Image imgJianTou;
    public Image imgJianTou1;
    public CanvasGroup tfQun;
    public CanvasGroup tfQun1;
    public CanvasGroup tfQun2;
    
    private Color hide = new Color(1, 1, 1, 0);
    private Vector3 jtStart;
    private Vector3 qunStart;
    
    // Start is called before the first frame update
    void Start()
    {
        jtStart = imgJianTou.transform.localPosition;
        qunStart = tfQun.transform.localPosition;
        MonoTool.Instance.StartCor(animation());
    }

    private IEnumerator animation()
    {
        while (true)
        {
            imgJianTou.color = hide;
            imgJianTou1.color = hide;
            tfQun.alpha = 0;
            tfQun1.alpha = 0;
            tfQun2.alpha = 0;

            imgJianTou.DOFade(1f, 0.3f);
            yield return new WaitForSeconds(0.25f);
            imgJianTou.transform.DOLocalMoveY(160f, 0.7f).OnComplete(() =>
            {
                imgJianTou.color = hide;
                imgJianTou.transform.localPosition = jtStart;
            }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.25f);
            tfQun.DOFade(1f, 0.2f);
            tfQun1.DOFade(1f, 0.2f);
            imgJianTou1.DOFade(1f, 0.5f);
            yield return new WaitForSeconds(0.1f);
            tfQun.transform.DOLocalMoveY(160f, 0.8f);
            tfQun1.transform.DOLocalMoveY(80f, 0.8f);
            tfQun2.DOFade(1f, 0.15f);
            yield return new WaitForSeconds(0.15f);
            tfQun2.transform.DOLocalMoveY(160f, 0.65f).OnComplete(() =>
            {
                tfQun.alpha = 0;
                tfQun1.alpha = 0;
                tfQun2.alpha = 0;
                tfQun.transform.localPosition = qunStart;
                tfQun1.transform.localPosition = qunStart;
                tfQun2.transform.localPosition = qunStart;
            });
            imgJianTou1.transform.DOLocalMoveY(160f, 0.7f).OnComplete(() =>
            {
                imgJianTou1.color = hide;
                imgJianTou1.transform.localPosition = jtStart;
            }).SetEase(Ease.Linear);
            yield return new WaitForSeconds(0.7f);
        }
    }

}
