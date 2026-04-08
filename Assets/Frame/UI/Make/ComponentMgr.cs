using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ComponentMgr : Single<ComponentMgr>
{
    // Use this for initialization
    public void SetStyle(Transform tran, string str)
    {
        //Table.color_style config = null;
        //TableCache.Instance.color_styleTable = null;
        //foreach (var item in TableMgr.Instance.ReadTable<Table.color_style>(TableName.color_style))
        //{
        //    if (str == item.Value.name)
        //    {
        //        config = item.Value;
        //        break;
        //    }
        //}
        //if(config == null)
        //{
        //    Debug.LogError(str + ":" + tran.name);
        //    return;
        //}
        //var text = tran.GetComponent<Text>();
        //text.color = Color.white;
        //var colorArr = config.Code.Split('_');
        //Color oneColor;
        //ColorUtility.TryParseHtmlString(colorArr[0], out oneColor);
        //if (colorArr.Length == 1)//修改字体颜色
        //    text.color = oneColor;
        //var gradient = tran.gameObject.GetComponent<GradientAuto>();//修改描边
        //if (gradient != null)
        //    GameObject.DestroyImmediate(gradient);
        //var outLine = tran.gameObject.GetComponent<Outline>();//修改描边
        //if (outLine != null)
        //    GameObject.DestroyImmediate(outLine);
        //var shadows = tran.gameObject.GetComponents<Shadow>();//修改投影
        //Shadow shadow = null;
        //foreach (var item in shadows)
        //{
        //    if (item.GetType().Name == "Shadow")
        //    {
        //        shadow = item;
        //        break;
        //    }
        //}
        //if (shadow != null)
        //    GameObject.DestroyImmediate(shadow);
        //if (colorArr.Length == 2)   //修改渐变
        //{
        //    Color twoColor;
        //    ColorUtility.TryParseHtmlString(colorArr[1], out twoColor);
        //    if (gradient == null)
        //        gradient = tran.gameObject.AddComponent<GradientAuto>();

        //    gradient.OneColor = oneColor;
        //    gradient.TwoColor = twoColor;
        //    gradient.m_Whole = (GradientAuto.Whole)config.GradientWhole;
        //    gradient.m_GradientType = (GradientAuto.GradientType)config.GradientType;
        //}
        //if (!string.IsNullOrEmpty(config.outline))
        //{
        //    if (outLine == null)
        //        outLine = tran.gameObject.AddComponent<Outline>();

        //    Color newColor;
        //    if (ColorUtility.TryParseHtmlString(config.outline, out newColor))
        //    {
        //        outLine.effectColor = newColor;
        //    }
        //    if (!string.IsNullOrEmpty(config.outlineRang))
        //    {
        //        var strs = config.outlineRang.Split('_');
        //        outLine.effectDistance = new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
        //    }
        //    else
        //    {
        //        outLine.effectDistance = new Vector2(0.5f, -0.5f);
        //    }

        //}
        //if (!string.IsNullOrEmpty(config.shadow))
        //{
        //    if (shadow == null)
        //        shadow = tran.gameObject.AddComponent<Shadow>();

        //    Color newColor;
        //    if (ColorUtility.TryParseHtmlString(config.shadow, out newColor))
        //    {
        //        shadow.effectColor = new Color(newColor.r, newColor.g, newColor.b, 0.4f);
        //    }
        //    if (!string.IsNullOrEmpty(config.shadowRang))
        //    {
        //        var strs = config.shadowRang.Split('_');
        //        shadow.effectDistance = new Vector2(float.Parse(strs[0]), float.Parse(strs[1]));
        //    }
        //}
    }
}
