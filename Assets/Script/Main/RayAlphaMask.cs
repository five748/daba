using UnityEngine;
using UnityEngine.UI;
public class RayAlphaMask : MonoBehaviour, ICanvasRaycastFilter
{
    [Header("透明度过滤阈值")]
    public float alpahThreshold = 0.1f;

    protected RawImage _image;
    private Texture2D _texture;
    void Start()
    {
        _image = GetComponent<RawImage>();
        _texture = _image.mainTexture as Texture2D;
    }

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        //将选中的点转换为Image区域内的本地点
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_image.rectTransform, sp, eventCamera, out localPoint);

        Vector2 pivot = _image.rectTransform.pivot;
        Vector2 normalizedLocal = new Vector2(pivot.x + localPoint.x / _image.rectTransform.rect.width, pivot.y + localPoint.y / _image.rectTransform.rect.height);
        Vector2 uv = new Vector2(
            _image.uvRect.x + normalizedLocal.x * _image.uvRect.width,
            _image.uvRect.y + normalizedLocal.y * _image.uvRect.height);

        uv.x /= _image.texture.width;
        uv.y /= _image.texture.height;


        //获取指定纹理坐标（u, v）处的像素颜色。它返回一个Color结构，其中包含红、绿、蓝和alpha通道的值。
        //Color c = _image.sprite.texture.GetPixel((int)uv.x, (int)uv.y);
        //用于在纹理上执行双线性插值以获取像素颜色值,这个方法使用双线性插值算法来估算纹理中某个位置的颜色,而不是直接从纹理的像素中读取颜色。
        Color c = ((Texture2D)(_image.mainTexture)).GetPixel((int)uv.x, (int)uv.y);
        Debug.LogError(c.a);
        return c.a > alpahThreshold;
    }
}
