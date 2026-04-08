﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShaderMask : UIBehaviour {

#if UNITY_EDITOR
    protected override void Reset()
    {
        base.Reset();
        Init();
    }
#endif
    Shader newShader;
    float minX;
    float minY;
    float maxX;
    float maxY;

    private void ChangeRect(Material m, Transform target) {
        Vector3[] corners = new Vector3[4];
        RectTransform rectTransform = transform as RectTransform;
        rectTransform.GetWorldCorners(corners);
        //var scan = target.localScale;
        minX = corners[0].x;
        minY = corners[0].y;
        maxX = corners[2].x;
        maxY = corners[2].y;
        m.SetFloat("_MinX", minX);
        m.SetFloat("_MinY", minY);
        m.SetFloat("_MaxX", maxX);
        m.SetFloat("_MaxY", maxY);
    }
    public void Init()
    {
        //if (!newShader)
        //    newShader = Shader.Find("Particles/NewAdditive");
        Renderer[] rds = transform.GetComponentsInChildren<Renderer>(true);
        //逐一遍历他的子物体中的Renderer
        foreach (Renderer render in rds)
        {
            //print(render.gameObject.name);
            for (int i = 0; i < render.materials.Length; i++)
            {
                var m = render.materials[i];
                //var texture = m.mainTexture;
                // render.materials[i] = material;
                // var newp = render.materials[i];
                //m.shader = newShader;
                //m.mainTexture = texture;
                //if (m.HasProperty("_TintColor"))
                //{
                //    var col = m.GetColor("_TintColor");
                //    m.SetColor("_TintColor", col);
                //}
                ChangeRect(m, render.transform);
            }
        }
    }
}




















































