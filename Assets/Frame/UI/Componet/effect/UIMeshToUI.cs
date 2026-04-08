using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
 
[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
public class UIMeshToUI : MaskableGraphic
{
    private Texture texture;
    public override Texture mainTexture
    {
        get
        {
            if (particleTexture)
            {
                return particleTexture;
             
            }
            return null;
        }
    }
    private Mesh newMesh;
    public Texture particleTexture;
    private Vector4 _uv = Vector4.zero;
    protected override void Awake()
    {
        base.Awake();
        if (Application.isPlaying)
        {
            newMesh = GetComponent<MeshFilter>().mesh;
        }
        else {
            newMesh = GetComponent<MeshFilter>().mesh;
        }
        if (Application.isPlaying)
        {
            //renderer.material = material;
            material = GetComponent<MeshRenderer>().material;
        }
#if UNITY_EDITOR
        else
        {
            material = GetComponent<MeshRenderer>().sharedMaterial;
        }
#endif
        _uv = new Vector4(0, 0, 1, 1);
    }

    protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs, Color32 color)
    {
        UIVertex[] vbo = new UIVertex[vertices.Length];
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            var vert = UIVertex.simpleVert;
            vert.color = color;
            vert.position = transform.InverseTransformPoint(v);
            vert.uv0 = uvs[i];
            vbo[i] = vert;
        }
        return vbo;
    }
    

    private UIVertex[] _quad = new UIVertex[4];
    protected override void OnPopulateMesh(VertexHelper vh)
    {

        //vh.Clear();
        //var vv = System.Array.ConvertAll<Vector3, Vector2>(newMesh.vertices, v3 =>
        //{
        //    return new Vector2((v3.x), (v3.y));
        //});
        ////根据顶点和三角面索引来画
        //vh.AddUIVertexStream(new List<UIVertex>(SetVbo(vv, newMesh.uv, Color.white)), new List<int>(newMesh.triangles));


        vh.Clear();
        Vector2 position = transform.InverseTransformPoint(transform.position);

        _quad[0] = UIVertex.simpleVert;
        _quad[0].color = color;
        _quad[0].uv0 = new Vector2(_uv.x, _uv.y);

        _quad[1] = UIVertex.simpleVert;
        _quad[1].color = color;
        _quad[1].uv0 = new Vector2(_uv.x, _uv.w);

        _quad[2] = UIVertex.simpleVert;
        _quad[2].color = color;
        _quad[2].uv0 = new Vector2(_uv.z, _uv.w);

        _quad[3] = UIVertex.simpleVert;
        _quad[3].color = color;
        _quad[3].uv0 = new Vector2(_uv.z, _uv.y);
        float size = 0.5f;
        Vector2 corner1 = new Vector2(position.x - size, position.y - size);
        Vector2 corner2 = new Vector2(position.x + size, position.y + size);

        _quad[0].position = new Vector2(corner1.x, corner1.y);
        _quad[1].position = new Vector2(corner1.x, corner2.y);
        _quad[2].position = new Vector2(corner2.x, corner2.y);
        _quad[3].position = new Vector2(corner2.x, corner1.y);

        vh.AddUIVertexQuad(_quad);

    }
  

    void Update()
    {
        if (Application.isPlaying)
        {
            //unscaled animation within UI
            SetAllDirty();
        }
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
