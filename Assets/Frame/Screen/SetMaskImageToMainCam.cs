using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetMaskImageToMainCam : MonoBehaviour
{
    public Transform NewFather;
    // Start is called before the first frame update
    public void Init(GameObject uilogin)
    {
        if(NewFather == null)
        {
            return;
        }
        var topImage = transform.Find("topImage");
        var dowmImage = transform.Find("downImage");

        ImageOne(topImage, uilogin.transform);
        ImageOne(dowmImage, uilogin.transform);
    }
    private void ImageOne(Transform maskImage, Transform uilogin)
    {
        var imagePos = maskImage.GetComponent<RectTransform>().anchoredPosition;
        var newImage = GameObject.Instantiate(maskImage.gameObject).transform;
        newImage.SetParentOverride(NewFather);
        newImage.GetComponent<RectTransform>().anchoredPosition = imagePos;
        newImage.SetTranLayer(NewFather.gameObject.layer);

        maskImage.SetParentOverride(uilogin);
        maskImage.GetComponent<RectTransform>().anchoredPosition = imagePos;
       
    }

}
