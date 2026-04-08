
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[AddComponentMenu("UI/MeshRawImage")]
public partial class MeshRawImage : RawImage
{
    public Mesh mesh;
    public Vector2 tiling = Vector2.one;
    public Vector2 offset;
    
    protected override void Awake()
    {
        base.Awake();
        _quad = new List<UIVertex>(SetVbo(mesh.vertices, mesh.uv, Color.white));
        triangles = new List<int>(mesh.triangles);
    }
   
    private List<UIVertex> _quad;
    private List<int> triangles;
    private Vector4 _uv = Vector4.zero;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        if (mesh == null) {
            return;
        }
        _quad = new List<UIVertex>(SetVbo(mesh.vertices, mesh.uv, color));
        vh.Clear();
        vh.AddUIVertexStream(_quad, triangles);
    }
    protected UIVertex[] SetVbo(Vector3[] vertices, Vector2[] uvs, Color32 color)
    {
        UIVertex[] vbo = new UIVertex[vertices.Length];
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            var vert = UIVertex.simpleVert;
            vert.color = color;
            //vert.position = transform.InverseTransformPoint(v);
            vert.position = v;
            vert.uv0 = uvs[i] * tiling + offset;
            vbo[i] = vert;
        }
        return vbo;
    }
    void Update()
    {
        if (Application.isPlaying)
        {
            //unscaled animation within UI
            SetAllDirty();
        }
    }
    private List<UIVertex> SetDefault()
    {
        UIVertex[] quads = new UIVertex[4];
        Vector2 position = transform.InverseTransformPoint(transform.position);

        quads[0] = UIVertex.simpleVert;
        quads[0].color = color;
        quads[0].uv0 = new Vector2(_uv.x, _uv.y);

        quads[1] = UIVertex.simpleVert;
        quads[1].color = color;
        quads[1].uv0 = new Vector2(_uv.x, _uv.w);

        quads[2] = UIVertex.simpleVert;
        quads[2].color = color;
        quads[2].uv0 = new Vector2(_uv.z, _uv.w);

        quads[3] = UIVertex.simpleVert;
        quads[3].color = color;
        quads[3].uv0 = new Vector2(_uv.z, _uv.y);
        float size = 0.5f;
        Vector2 corner1 = new Vector2(position.x - size, position.y - size);
        Vector2 corner2 = new Vector2(position.x + size, position.y + size);

        quads[0].position = new Vector2(corner1.x, corner1.y);
        quads[1].position = new Vector2(corner1.x, corner2.y);
        quads[2].position = new Vector2(corner2.x, corner2.y);
        quads[3].position = new Vector2(corner2.x, corner1.y);
        return new List<UIVertex>(quads);
    }
#if UNITY_EDITOR
    void LateUpdate()
    {
        if (!Application.isPlaying)
        {
            SetAllDirty();
        }
    }
#endif
}