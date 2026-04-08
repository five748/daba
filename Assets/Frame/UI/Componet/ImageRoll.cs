using UnityEngine;
using UnityEngine.UI;
public class ImageRoll : MonoBehaviour
{
    private Image image;
    private Material mtl;
    private Vector2 dis;
    private float screenModify;
    private float x;
    private float y;
    public bool IsChangeX;
    public float xSpeed;
    private static bool _isStopPlay = false;
    public static bool IsStopPlay {
        get {
            return _isStopPlay;
        }
        set {
            //Debug.LogError("IsStopPlay:" + value);
            _isStopPlay = value;
        }
    }
    // Use this for initialization
    void Awake()
    {
        screenModify = 2340 / 1080.0f;
        SetZero();
    }
    public void SetZero() {
        image = transform.GetComponent<Image>();
        mtl = image.materialForRendering;
        mtl.SetFloat("_DistanceX", 0);
        mtl.SetFloat("_DistanceY", 0);
    }
    public void SetUV(Vector2 add) {
        x += add.x;
        y += add.y / screenModify;
        mtl.SetFloat("_DistanceX", x);
        mtl.SetFloat("_DistanceY", y);
    }
    public void Update()
    {
        if (IsStopPlay) {
            return;
        }
        if (!IsChangeX) {
            return;
        }
        if (ProssData.Instance.isOpenGMTip)
        {
            return;
        }
        x += xSpeed;
        mtl.SetFloat("_DistanceX", x);
    }
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        //Debug.LogError("5455");
        mtl.SetFloat("_DistanceX", 0);
        mtl.SetFloat("_DistanceY", 0);
#endif
    }
}