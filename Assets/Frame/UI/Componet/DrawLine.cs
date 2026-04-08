using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class drawLine:MonoBehaviour
{
    private LineRenderer line;
    Vector3[] path;
    private float time = 0;
    List<Vector3> pos = new List<Vector3>();
    void Awake() {
        path = pos.ToArray();//初始化
        line = GetComponent<LineRenderer>();//获得该物体上的LineRender组件
    }
    void Update() {
        time += Time.deltaTime;
        if(time > 0.1)//每0.1秒绘制一次
        {
            time = 0;
            pos.Add(transform.position);//添加当前坐标进链表
            path = pos.ToArray();//转成数组
        }
        if(path.Length != 0)//有数据时候再绘制
        {
            line.positionCount = path.Length;//设置顶点数      
            line.SetPositions(path);//设置顶点位置
        }
    }
}