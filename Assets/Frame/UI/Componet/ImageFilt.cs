using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/ImageFilt", 16)]
public class ImageFilt : MaskableGraphic
{
    [SerializeField] Texture m_Texture;
    [SerializeField] Vector2 m_UVPivot;
    [HideInInspector] public Vector2 offsetPos;
    Vector2 oldPos;

    protected ImageFilt()
    {
        useLegacyMeshGeneration = false;
    }

    /// <summary>
    /// Returns the texture used to draw this Graphic.
    /// </summary>
    public override Texture mainTexture
    {
        get
        {
            if (m_Texture == null)
            {
                if (material != null && material.mainTexture != null)
                {
                    return material.mainTexture;
                }
                return s_WhiteTexture;
            }

            return m_Texture;
        }
    }

    /// <summary>
    /// Texture to be used.
    /// </summary>
    public Texture texture
    {
        get
        {
            return m_Texture;
        }
        set
        {
            if (m_Texture == value)
                return;

            m_Texture = value;
            SetVerticesDirty();
            SetMaterialDirty();
        }
    }

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        Texture tex = mainTexture;
        vh.Clear();
        if (tex != null)
        {
            var r = GetPixelAdjustedRect();
            var v = new Vector4(r.x, r.y, r.x + r.width, r.y + r.height);
            var uvOffset = new Vector2(offsetPos.x + v.x * m_UVPivot.x, offsetPos.y + v.y * m_UVPivot.y);
            uvOffset /= transform.localScale;
            var uv = new Vector4(
                (v.x + uvOffset.x) * tex.texelSize.x,
                (v.y + uvOffset.y) * tex.texelSize.y,
                (v.z + uvOffset.x) * tex.texelSize.x,
                (v.w + uvOffset.y) * tex.texelSize.y
                );
            {
                var color32 = color;
                vh.AddVert(new Vector3(v.x, v.y), color32, new Vector2(uv.x, uv.y));
                vh.AddVert(new Vector3(v.x, v.w), color32, new Vector2(uv.x, uv.w));
                vh.AddVert(new Vector3(v.z, v.w), color32, new Vector2(uv.z, uv.w));
                vh.AddVert(new Vector3(v.z, v.y), color32, new Vector2(uv.z, uv.y));

                vh.AddTriangle(0, 1, 2);
                vh.AddTriangle(2, 3, 0);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!raycastTarget) return;
        oldPos = eventData.position + offsetPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!raycastTarget) return;
        offsetPos = oldPos - eventData.position;
        SetVerticesDirty();
    }
}